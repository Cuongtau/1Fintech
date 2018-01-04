using System.Collections.Generic;
using DAL.Model;

namespace Services.IService
{
   public interface ISMSGatewayService
   {
       List<SMS_MO> GetTopData_MO(short status, int top, ref int totalRow);

       List<SMS_MT> GetTopData_MT(short status, int top, ref int totalRow);
   }
}
