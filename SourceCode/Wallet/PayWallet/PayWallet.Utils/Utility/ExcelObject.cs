using System;
using System.Collections.Generic;
using System.IO;
using System.Data.OleDb;
using System.Text;
using System.Data;
using System.Threading;

namespace PayWallet.Utils
{
    public class ExcelObject : IDisposable
    {
        private const string excelObject = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=\"Excel {3};HDR=YES\"";
        private string _filepath = string.Empty;
        private OleDbConnection _con;

        public delegate void ProgressWork(float percentage);
        private event ProgressWork Reading;
        private event ProgressWork Writeing;
        private event EventHandler ConnectionStringChange;

        public event ProgressWork ReadProgress
        {
            add
            {
                Reading += value;
            }
            remove
            {
                Reading -= value;
            }
        }

        public virtual void OnReadProgress(float percentage)
        {
            if (Reading != null)
                Reading(percentage);
        }


        public event ProgressWork WriteProgress
        {
            add
            {


                Writeing += value;
            }
            remove
            {
                Writeing -= value;
            }
        }

        public virtual void OnWriteProgress(float percentage)
        {
            if (Writeing != null)
                Writeing(percentage);
        }


        public event EventHandler ConnectionStringChanged
        {
            add
            {
                ConnectionStringChange += value;
            }
            remove
            {
                ConnectionStringChange -= value;
            }
        }

        public virtual void OnConnectionStringChanged()
        {
            if (Connection != null && !Connection.ConnectionString.Equals(ConnectionString))
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Dispose();
                _con = null;

            }
            if (ConnectionStringChange != null)
            {
                ConnectionStringChange(this, new EventArgs());
            }
        }

        public string ConnectionString
        {
            get
            {
                if (!string.IsNullOrEmpty(_filepath))
                {
                    //Check for File Format
                    var fi = new FileInfo(_filepath);
                    if (fi.Extension.Equals(".xls"))
                    {
                        return string.Format(excelObject, "Jet", "4.0", _filepath, "8.0");
                    }
                    if (fi.Extension.Equals(".xlsx"))
                    {
                        return string.Format(excelObject, "Ace", "12.0", _filepath, "12.0");
                    }
                }
                else
                {
                    return string.Empty;
                }
                return "";
            }
        }

        public OleDbConnection Connection
        {
            get { return _con ?? (_con = new OleDbConnection { ConnectionString = ConnectionString }); }
        }


        public ExcelObject(string path)
        {

            _filepath = path;
            OnConnectionStringChanged();
        }

        public DataTable GetSchema()
        {
            if (Connection.State != ConnectionState.Open) Connection.Open();
            return Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        }

        public DataTable ReadTable(string tableName)
        {
            return ReadTable(tableName, "");
        }

        public DataTable ReadTable(string tableName, string criteria)
        {

            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                    OnReadProgress(10);

                }
                var cmdText = "Select * from [{0}]";
                if (!string.IsNullOrEmpty(criteria))
                    cmdText += " Where " + criteria;
                var cmd = new OleDbCommand(string.Format(cmdText, tableName)) { Connection = Connection };
                var adpt = new OleDbDataAdapter(cmd);
                OnReadProgress(30);

                var ds = new DataSet();
                OnReadProgress(50);

                adpt.Fill(ds, tableName);
                OnReadProgress(100);
                cmd.Clone();
                cmd.Dispose();
                adpt.Dispose();
                Connection.Close();
                return ds.Tables.Count == 1 ? ds.Tables[0] : null;
            }
            catch
            {
                return null;
            }
        }


        public bool DeleteData(string tablename)
        {
            try
            {
                var oledbAdapter = new OleDbDataAdapter();
                const string sql = "Delete From [{0}]";
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                    OnWriteProgress(10);

                }
                oledbAdapter.DeleteCommand = Connection.CreateCommand();
                oledbAdapter.DeleteCommand.CommandText = string.Format(sql, tablename);
                oledbAdapter.DeleteCommand.ExecuteNonQuery();
                oledbAdapter.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                OnWriteProgress(0);
                return false;
            }

        }

        public bool DropTable(string tablename)
        {

            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                const string cmdText = "Drop Table [{0}]";
                var myCommand = new OleDbCommand
                {
                    Connection = Connection,
                    CommandText = string.Format(cmdText, tablename)
                };
                myCommand.ExecuteNonQuery();
                Thread.Sleep(1000);
                myCommand.Clone();
                myCommand.Dispose();
                Connection.Close();


                return true;
            }
            catch (Exception ex)
            {
                OnWriteProgress(0);
                return false;
            }
        }

        public bool WriteTable(string tableName, Dictionary<string, string> tableDefination)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var com = new OleDbCommand
                {
                    Connection = Connection,
                    CommandText = GenerateCreateTable(tableName, tableDefination)
                };
                com.ExecuteNonQuery();
                com.Clone();
                com.Dispose();
                Connection.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool InsertStatement(DataTable dataTable, string tableName)
        {
            try
            {
                if (Connection.State == ConnectionState.Closed)
                    Connection.Open();
                foreach (DataRow row in dataTable.Rows)
                {
                    AddNewRow(row, tableName);
                }
                Connection.Close();
                return true;
            }
            catch (Exception)
            {
                Connection.Close();
                return false;
            }
        }

        public bool AddNewRow(DataRow dr, string tableName)
        {
            var myCommand = new OleDbCommand
            {
                Connection = Connection,
                CommandText = GenerateInsertStatement(dr, tableName)
            };

            myCommand.ExecuteNonQuery();

            myCommand.Clone();
            myCommand.Dispose();
            return true;
        }

        /// <summary>
        /// Generates Create Table Script
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableDefination"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string GenerateCreateTable(string tableName, Dictionary<string, string> tableDefination)
        {

            var sb = new StringBuilder();
            sb.AppendFormat("CREATE TABLE [{0}](", tableName);
            var firstcol = true;
            foreach (var keyvalue in tableDefination)
            {
                if (!firstcol)
                {
                    sb.Append(",");
                }
                firstcol = false;
                sb.AppendFormat("{0} {1}", keyvalue.Key, keyvalue.Value);
            }

            sb.Append(")");
            return sb.ToString();
        }




        private static string GenerateInsertStatement(DataRow dr, string tableName)
        {
            var sb = new StringBuilder();
            var firstcol = true;
            sb.AppendFormat("INSERT INTO [{0}](", tableName);


            foreach (DataColumn dc in dr.Table.Columns)
            {
                if (!firstcol)
                {
                    sb.Append(",");
                }
                firstcol = false;

                sb.Append("[" + dc.Caption + "]");
            }

            sb.Append(") VALUES(");
            for (var i = 0; i <= dr.Table.Columns.Count - 1; i++)
            {
                if (!ReferenceEquals(dr.Table.Columns[i].DataType, typeof(int)))
                {
                    sb.Append("'");
                    sb.Append(dr[i].ToString().Replace("'", "''"));
                    sb.Append("'");
                }
                else
                {
                    sb.Append(dr[i].ToString().Replace("'", "''"));
                }
                if (i != dr.Table.Columns.Count - 1)
                {
                    sb.Append(",");
                }
            }

            sb.Append(")");
            return sb.ToString();
        }



        #region IDisposable Members

        public void Dispose()
        {
            if (_con != null && _con.State == ConnectionState.Open)
                _con.Close();
            if (_con != null)
                _con.Dispose();
            _con = null;
            _filepath = string.Empty;
        }

        #endregion
    }
}
