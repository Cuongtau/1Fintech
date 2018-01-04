using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IRepository;

namespace DAL.Repository
{
    public class SMSGateway: ISMSGateway
    {


        //public List<CinemaDB> GetListCinema_WEB(int cinemaID, int pCinemaID, int locationID, ref int totalRow)
        //{
        //    try
        //    {
        //        var pars = new SqlParameter[4];
        //        pars[0] = new SqlParameter("@_Cinema_ID", cinemaID <= 0 ? DBNull.Value : (object)cinemaID);
        //        pars[1] = new SqlParameter("@_P_Cinema_ID", pCinemaID <= 0 ? DBNull.Value : (object)pCinemaID);
        //        pars[2] = new SqlParameter("@_Location_ID", locationID <= 0 ? DBNull.Value : (object)locationID);
        //        pars[3] = new SqlParameter("@_TotalRow", SqlDbType.Int) { Direction = ParameterDirection.Output };
        //        var list = new DBHelper(Config.DBFilmAPIConnectionString).GetListSP<CinemaDB>("SP_CinemaList_WEB", pars);
        //        if (list == null || list.Count <= 0)
        //            return new List<CinemaDB>();
        //        totalRow = Convert.ToInt32(pars[3].Value);
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        NLogLogger.LogInfo(ex.ToString());
        //        return new List<CinemaDB>();
        //    }
        //}
    }
}
