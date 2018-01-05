using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IRepository;
using DAL.Model;
using Utilities;

namespace DAL.Repository
{
    public class SMSGateway: ISMSGateway
    {

        public int SMS_MO_Insert(string userId, string serviceId, string commandCode, string message, long requestId, string message_GOC)
        {
            try
            {
                var pars = new SqlParameter[7];
                pars[0] = new SqlParameter("@_UserId", string.IsNullOrEmpty(userId) ? DBNull.Value : (object)userId);
                pars[1] = new SqlParameter("@_ServiceID", string.IsNullOrEmpty(serviceId) ? DBNull.Value : (object)serviceId);
                pars[2] = new SqlParameter("@_CommandCode", string.IsNullOrEmpty(commandCode) ? DBNull.Value : (object)commandCode);
                pars[3] = new SqlParameter("@_Message", string.IsNullOrEmpty(message) ? DBNull.Value : (object)message);
                pars[4] = new SqlParameter("@_RequestID", requestId <= 0 ? DBNull.Value : (object)requestId);
                pars[5] = new SqlParameter("@_Message_GOC", string.IsNullOrEmpty(message_GOC) ? DBNull.Value : (object)message_GOC);
                pars[6] = new SqlParameter("@_ResponseStatus", SqlDbType.Int) { Direction = ParameterDirection.Output };
                new DBHelper.DBHelper(Config.SmsGatewayConnectionString).ExecuteNonQuerySP("SP_SMS_MO_Insert", pars);
                return Convert.ToInt32(pars[6].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return -696;
            }
        }


        public int SMS_MT_Insert(long MO_Id, string userId, string serviceId, string commandCode,
            string message, long requestId, Int16 msg_Type, int contentType, Int16 isCDR, int priority)
        {
            try
            {
                var pars = new SqlParameter[7];
                pars[0] = new SqlParameter("@_MO_ID", MO_Id <= 0 ? DBNull.Value : (object)MO_Id);
                pars[1] = new SqlParameter("@_UserId", string.IsNullOrEmpty(userId) ? DBNull.Value : (object)userId);
                pars[2] = new SqlParameter("@_ServiceID", string.IsNullOrEmpty(serviceId) ? DBNull.Value : (object)serviceId);
                pars[3] = new SqlParameter("@_CommandCode", string.IsNullOrEmpty(commandCode) ? DBNull.Value : (object)commandCode);
                pars[4] = new SqlParameter("@_RequestID", requestId <= 0 ? DBNull.Value : (object)requestId);
                pars[3] = new SqlParameter("@_Message", string.IsNullOrEmpty(message) ? DBNull.Value : (object)message);
                pars[4] = new SqlParameter("@_RequestID", requestId <= 0 ? DBNull.Value : (object)requestId);
                pars[5] = new SqlParameter("@_MsgType", msg_Type < 1 ? DBNull.Value : (object)msg_Type);
                pars[4] = new SqlParameter("@_ContentType", contentType <= 0 ? DBNull.Value : (object)contentType);
                pars[5] = new SqlParameter("@_IsCDR", isCDR < 1 ? DBNull.Value : (object)isCDR);
                pars[4] = new SqlParameter("@_Priority", priority <= 0 ? DBNull.Value : (object)priority);
                pars[6] = new SqlParameter("@_ResponseStatus", SqlDbType.Int) { Direction = ParameterDirection.Output };
                new DBHelper.DBHelper(Config.SmsGatewayConnectionString).ExecuteNonQuerySP("SP_SMS_MT_Insert", pars);
                return Convert.ToInt32(pars[6].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return -696;
            }
        }

        //@_Status TINYINT = 0,
        //    @_Top INT = 1000

        public List<SMS_MO> SMS_MO_GetTop(Int16 status, int top, ref int totalRow)
        {
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_Status", status < 1 ? DBNull.Value : (object)status);
                pars[1] = new SqlParameter("@_Top", top <= 0 ? DBNull.Value : (object)top);
                var list = new DBHelper.DBHelper(Config.SmsGatewayConnectionString).GetListSP<SMS_MO>("SP_SMS_MO_GetTop", pars);
                if (list == null || list.Count <= 0)
                    return new List<SMS_MO>();
                totalRow = list.Count;
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return new List<SMS_MO>();
            }
        }

        public List<SMS_MT> SMS_MT_GetTop(Int16 status, int top, ref int totalRow)
        { 
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_Status", status < 1 ? DBNull.Value : (object)status);
                pars[1] = new SqlParameter("@_Top", top <= 0 ? DBNull.Value : (object)top);
                var list = new DBHelper.DBHelper(Config.SmsGatewayConnectionString).GetListSP<SMS_MT>("SP_SMS_MT_GetTop", pars);
                if (list == null || list.Count <= 0)
                    return new List<SMS_MT>();
                totalRow = list.Count;
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return new List<SMS_MT>();
            }
        }


        public int SMS_Confirm_Response_SMS_MO(long MO_Id, Int16 statusProcess)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_MO_ID", MO_Id <= 0 ? DBNull.Value : (object)MO_Id);
                pars[1] = new SqlParameter("@_StatusProcess", statusProcess < 1 ? DBNull.Value : (object)statusProcess);
                pars[2] = new SqlParameter("@_ResponseStatus", SqlDbType.Int) { Direction = ParameterDirection.Output };
                new DBHelper.DBHelper(Config.SmsGatewayConnectionString).ExecuteNonQuerySP("SP_SMS_MO_ConfirmResponse", pars);
                return Convert.ToInt32(pars[2].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return -696;
            }
        }


        public int SMS_Confirm_Response_SMS_MT(long MT_Id, Int16 statusProcess, int isResend)
        {
            try 
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_MT_ID", MT_Id <= 0 ? DBNull.Value : (object)MT_Id);
                pars[1] = new SqlParameter("@_StatusProcess", statusProcess < 1 ? DBNull.Value : (object)statusProcess);
                pars[2] = new SqlParameter("@_IsResend", statusProcess <= 0 ? DBNull.Value : (object)isResend);
                pars[3] = new SqlParameter("@_ResponseStatus", SqlDbType.Int) { Direction = ParameterDirection.Output };
                new DBHelper.DBHelper(Config.SmsGatewayConnectionString).ExecuteNonQuerySP("SP_SMS_MO_ConfirmResponse", pars);
                return Convert.ToInt32(pars[3].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return -696;
            }
        }


    }
}
