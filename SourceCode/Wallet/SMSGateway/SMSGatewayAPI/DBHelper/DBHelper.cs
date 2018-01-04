﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
namespace DBHelper
{
    public class DBHelper
    {
        private string _cnnString = "";
        public string cnnString { get { return _cnnString; } set { _cnnString = value; } }
        // Mark 1
        /// <summary>
        /// Chuẩn hoá connection string
        /// </summary>
        public string FixCNN(string connStr, bool Pooling)
        {
            var aconnStr = connStr.Split(';');
            var sTemp = "";

            for (var i = 0; i < aconnStr.Length; i++)
            {
                if (aconnStr[i].ToLower().StartsWith("pooling=") ||
                    aconnStr[i].ToLower().StartsWith("min pool size=") ||
                    aconnStr[i].ToLower().StartsWith("max pool size=") ||
                    aconnStr[i].ToLower().StartsWith("connect timeout=")) continue;
                if (!aconnStr[i].Equals("")) sTemp += aconnStr[i] + ";";
            }

            if (Pooling)
            {
                sTemp += "Pooling=true;Min Pool Size=1;Max Pool Size=15;";
            }
            else
            {
                sTemp += "Pooling=false;Connect Timeout=45;";
            }
            return sTemp;
        }

        public DBHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="connStr"></param>
        public DBHelper(string connStr)
        {
            _cnnString = connStr;
        }



        /// <summary>
        /// 
        /// </summary>
        private SqlConnection _ConnectionToDB;
        public SqlConnection ConnectionToDB
        {
            get { return _ConnectionToDB; }
            set { _ConnectionToDB = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public void Open()
        {
            if (_cnnString == "")
            {
                throw new Exception("Connection String can not null");
            }
            _ConnectionToDB = OpenConnection();
        }


        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            CloseConnection(_ConnectionToDB);
        }




        /// <summary>
        /// return an Open SqlConnection
        /// </summary>

        public SqlConnection OpenConnection(string connectionString)
        {
            try
            {
                _cnnString = connectionString;
                return OpenConnection();
            }
            catch (SqlException myException)
            {
                throw (new Exception(myException.Message));
            }
        }





        /// <summary>
        /// return an Open SqlConnection
        /// </summary>
        public SqlConnection OpenConnection()
        {
            if (_cnnString == "")
            {
                throw new Exception("Connection String can not null");
            }

            SqlConnection mySqlConnection;

            try
            {
                mySqlConnection = new SqlConnection(FixCNN(_cnnString, true));
                mySqlConnection.Open();
                return mySqlConnection;
            }
            catch (Exception)
            {
                // De phong truong hop bi max pool thi se fix lai connection string pooling=false
                mySqlConnection = new SqlConnection(FixCNN(_cnnString, false));
                mySqlConnection.Open();
                return mySqlConnection;
                // throw (new Exception(myException.Message));
            }
        }





        /// <summary>
        /// close an SqlConnection
        /// </summary>
        public void CloseConnection(SqlConnection mySqlConnection)
        {
            try
            {
                if (mySqlConnection != null)
                {
                    if (mySqlConnection.State == ConnectionState.Open)
                    {
                        mySqlConnection.Close();
                    }
                }
            }
            catch (SqlException myException)
            {
                throw (new Exception(myException.Message));
            }
        }





        # region GetDataTable
        public DataTable GetDataTable(SqlCommand sqlCommand)
        {
            SqlConnection conn = null;
            try
            {
                if (_ConnectionToDB == null)
                {
                    conn = OpenConnection();
                    sqlCommand.Connection = conn;
                }
                else
                {
                    sqlCommand.Connection = _ConnectionToDB;
                }
                var myDataAdapter = new SqlDataAdapter(sqlCommand);
                var dt = new DataTable();
                myDataAdapter.Fill(dt);
                myDataAdapter.Dispose();
                return dt;
            }
            catch (SqlException myException)
            {
                throw (new Exception(myException.Message));
            }
            finally
            {
                if (conn != null)
                {
                    CloseConnection(conn);
                }
            }
        }

        public DataTable GetDataTable(SqlCommand sqlCommand, params SqlParameter[] Parameters)
        {
            sqlCommand.Parameters.AddRange(Parameters);
            return GetDataTable(sqlCommand);
        }

        public DataTable GetDataTable(string strSQL)
        {
            var sqlCommand = new SqlCommand(strSQL);
            return GetDataTable(sqlCommand);
        }

        public DataTable GetDataTable(string strSQL, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(strSQL);
            return GetDataTable(sqlCommand, Parameters);
        }

        public DataTable GetDataTable(string strSQL, int commandTimeout)
        {
            var sqlCommand = new SqlCommand(strSQL);
            sqlCommand.CommandTimeout = commandTimeout;
            return GetDataTable(sqlCommand);
        }

        public DataTable GetDataTable(string strSQL, int commandTimeout, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(strSQL);
            sqlCommand.CommandTimeout = commandTimeout;
            return GetDataTable(sqlCommand, Parameters);
        }
        #endregion

        # region ExecuteScalar
        public object ExecuteScalar(SqlCommand sqlCommand)
        {
            SqlConnection conn = null;
            try
            {
                if (_ConnectionToDB == null)
                {
                    conn = OpenConnection();
                    sqlCommand.Connection = conn;
                }
                else
                {
                    sqlCommand.Connection = _ConnectionToDB;
                }
                return sqlCommand.ExecuteScalar();
            }
            catch (SqlException myException)
            {
                throw (new Exception(myException.Message));
            }
            finally
            {
                if (conn != null)
                {
                    CloseConnection(conn);
                }
            }
        }

        public object ExecuteScalar(SqlCommand sqlCommand, params SqlParameter[] Parameters)
        {
            sqlCommand.Parameters.AddRange(Parameters);
            return ExecuteScalar(sqlCommand);
        }

        public object ExecuteScalar(string strSQL)
        {
            var sqlCommand = new SqlCommand(strSQL);
            return ExecuteScalar(sqlCommand);
        }

        public object ExecuteScalar(string strSQL, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(strSQL);
            return ExecuteScalar(sqlCommand, Parameters);
        }
        #endregion

        # region ExecuteNonQuery
        public int ExecuteNonQuery(SqlCommand sqlCommand)
        {
            SqlConnection conn = null;
            try
            {
                if (_ConnectionToDB == null)
                {
                    conn = OpenConnection();
                    sqlCommand.Connection = conn;
                }
                else
                {
                    sqlCommand.Connection = _ConnectionToDB;
                }
                return sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException myException)
            {
                throw (new Exception(myException.Message));
            }
            finally
            {
                if (conn != null)
                {
                    CloseConnection(conn);
                }
            }
        }

        public int ExecuteNonQuery(SqlCommand sqlCommand, params SqlParameter[] Parameters)
        {
            sqlCommand.Parameters.AddRange(Parameters);
            return ExecuteNonQuery(sqlCommand);
        }

        public int ExecuteNonQuery(string strSQL)
        {
            var sqlCommand = new SqlCommand(strSQL);
            return ExecuteNonQuery(sqlCommand);
        }

        public int ExecuteNonQuery(string strSQL, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(strSQL);
            return ExecuteNonQuery(sqlCommand, Parameters);
        }


        public int ExecuteNonQuery(string strSQL, int commandTimeout)
        {
            var sqlCommand = new SqlCommand(strSQL);
            sqlCommand.CommandTimeout = commandTimeout;
            return ExecuteNonQuery(sqlCommand);
        }

        public int ExecuteNonQuery(string strSQL, int commandTimeout, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(strSQL);
            sqlCommand.CommandTimeout = commandTimeout;
            return ExecuteNonQuery(sqlCommand, Parameters);
        }


        #endregion

        #region ExecuteScalarSP
        public object ExecuteScalarSP(string SPName)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return ExecuteScalar(sqlCommand);
        }

        public object ExecuteScalarSP(string SPName, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return ExecuteScalar(sqlCommand, Parameters);
        }
        #endregion

        #region ExecuteNonQuerySP
        public int ExecuteNonQuerySP(string SPName)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return ExecuteNonQuery(sqlCommand);
        }

        public int ExecuteNonQuerySP(string SPName, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return ExecuteNonQuery(sqlCommand, Parameters);
        }

        public int ExecuteNonQuerySP(string SPName, int commandTimeout)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure, CommandTimeout = commandTimeout };
            return ExecuteNonQuery(sqlCommand);
        }

        public int ExecuteNonQuerySP(string SPName, int commandTimeout, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure, CommandTimeout = commandTimeout };
            return ExecuteNonQuery(sqlCommand, Parameters);
        }
        #endregion

        #region GetDataTableSP
        public DataTable GetDataTableSP(string SPName)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return GetDataTable(sqlCommand);
        }

        public DataTable GetDataTableSP(string SPName, int timeOut, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure, CommandTimeout = timeOut };
            return GetDataTable(sqlCommand, Parameters);
        }

        public DataTable GetDataTableSP(string SPName, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return GetDataTable(sqlCommand, Parameters);
        }
        #endregion

        #region[ListGenerate]
        public List<T> GetList<T>(SqlCommand sqlCommand)
        {
            try
            {
                //sqlCommand.CommandTimeout = 50;
                sqlCommand.Connection = _ConnectionToDB ?? OpenConnection();
                var dr = sqlCommand.ExecuteReader();
                if (dr == null || dr.FieldCount == 0) return null;
                var fCount = dr.FieldCount;
                var m_Type = typeof(T);
                var l_Property = m_Type.GetProperties();
                object obj;
                var m_List = new List<T>();
                string pName;
                while (dr.Read())
                {
                    obj = Activator.CreateInstance(m_Type);
                    for (var i = 0; i < fCount; i++)
                    {
                        pName = dr.GetName(i);
                        if (l_Property.Where(a => a.Name == pName).Select(a => a.Name).Count() <= 0) continue;
                        if (dr[i] != DBNull.Value)
                        {
                            m_Type.GetProperty(pName).SetValue(obj, dr[i], null);
                        }
                        else
                        {
                            m_Type.GetProperty(pName).SetValue(obj, null, null);
                        }
                    }
                    m_List.Add((T)obj);
                }
                dr.Close();
                return m_List;
            }
            catch (SqlException myException)
            {
                throw (new Exception(myException.Message));
            }
            finally
            {

                CloseConnection(sqlCommand.Connection);

            }
        }

        public List<T> GetList<T>(SqlCommand sqlCommand, params SqlParameter[] Parameters)
        {
            sqlCommand.Parameters.AddRange(Parameters);
            return GetList<T>(sqlCommand);
        }

        public List<T> GetList<T>(string strSQL)
        {
            var sqlCommand = new SqlCommand(strSQL);
            return GetList<T>(sqlCommand);
        }

        public List<T> GetList<T>(string strSQL, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(strSQL);
            sqlCommand.Parameters.AddRange(Parameters);
            return GetList<T>(sqlCommand);
        }
        #endregion

        #region[ListGenerateSP]
        public List<T> GetListSP<T>(string SPName)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return GetList<T>(sqlCommand);
        }

        public List<T> GetListSP<T>(string SPName, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return GetList<T>(sqlCommand, Parameters);
        }
        #endregion

        #region[GetInstance]
        public T GetInstance<T>(SqlCommand sqlCommand)
        {
            try
            {
                T temp = default(T);

                sqlCommand.Connection = _ConnectionToDB ?? OpenConnection();
                var dr = sqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    var fCount = dr.FieldCount;
                    var m_Type = typeof(T);
                    var l_Property = m_Type.GetProperties();
                    object obj;
                    var m_List = new List<T>();
                    string pName;

                    obj = Activator.CreateInstance(m_Type);
                    for (var i = 0; i < fCount; i++)
                    {
                        pName = dr.GetName(i);
                        if (l_Property.Where(a => a.Name == pName).Select(a => a.Name).Count() <= 0) continue;
                        if (dr[i] != DBNull.Value)
                        {
                            m_Type.GetProperty(pName).SetValue(obj, dr[i], null);
                        }
                        else
                        {
                            m_Type.GetProperty(pName).SetValue(obj, null, null);
                        }
                    }
                    dr.Close();
                    return (T)obj;
                }
                else
                {
                    return temp;
                }
            }
            catch (SqlException myException)
            {
                throw (new Exception(myException.Message));
            }
            finally
            {
                CloseConnection(sqlCommand.Connection);
            }
        }

        public T GetInstance<T>(SqlCommand sqlCommand, params SqlParameter[] Parameters)
        {
            sqlCommand.Parameters.AddRange(Parameters);
            return GetInstance<T>(sqlCommand);
        }

        public T GetInstance<T>(string strSQL)
        {
            var sqlCommand = new SqlCommand(strSQL);
            return GetInstance<T>(sqlCommand);
        }

        public T GetInstance<T>(string strSQL, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(strSQL);
            sqlCommand.Parameters.AddRange(Parameters);
            return GetInstance<T>(sqlCommand);
        }
        #endregion

        #region[GetInstanceSP]
        public T GetInstanceSP<T>(string SPName)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return GetInstance<T>(sqlCommand);
        }

        public T GetInstanceSP<T>(string SPName, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return GetInstance<T>(sqlCommand, Parameters);
        }
        #endregion

    }
}
