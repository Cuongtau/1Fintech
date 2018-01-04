using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IRepository;
using DAL.Model;
using Services.IService;

namespace Services.Service
{
    public class SMSGatewayService : ISMSGatewayService
    {
        private ISMSGateway _iSMSGateway;
        public SMSGatewayService(ISMSGateway iSMSGateway)
        {
            _iSMSGateway = iSMSGateway;
        }

        public int SMS_MO_Insert(string userId, string serviceId, string commandCode, string message, long requestId,
            string message_GOC)
        {

            return _iSMSGateway.SMS_MO_Insert(userId, serviceId, commandCode, message, requestId, message_GOC);
        }

        public int SMS_MT_Insert(long MO_Id, string userId, string serviceId, string commandCode,string message, long requestId, Int16 msg_Type, int contentType, Int16 isCDR, int priority)
        {

            return _iSMSGateway.SMS_MT_Insert(MO_Id, userId, serviceId, commandCode, message, requestId, msg_Type, contentType, isCDR, priority);
        }

        public List<SMS_MO> GetTopData_MO(short status, int top, ref int totalRow)
        {
            return _iSMSGateway.SMS_MO_GetTop(status, top, ref totalRow);
        }

        public List<SMS_MT> GetTopData_MT(short status, int top, ref int totalRow)
        {
            return _iSMSGateway.SMS_MT_GetTop(status, top, ref totalRow);
        }


    }
}
