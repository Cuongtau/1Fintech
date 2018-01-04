using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;

namespace DAL.IRepository
{
    public interface ISMSGateway
    {
        int SMS_MO_Insert(string userId, string serviceId, string commandCode, string message, long requestId,
            string message_GOC);

        int SMS_MT_Insert(long MO_Id, string userId, string serviceId, string commandCode,
            string message, long requestId, Int16 msg_Type, int contentType, Int16 isCDR, int priority);

        List<SMS_MO> SMS_MO_GetTop(Int16 status, int top, ref int totalRow);

        List<SMS_MT> SMS_MT_GetTop(Int16 status, int top, ref int totalRow);
    }
}
