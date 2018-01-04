using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class SMS_MO
    {
        public string UserID { set; get; }
        public string ServiceID { set; get; }
        public string CommandCode { set; get; }
        public string Message { set; get; }
        public Int64 RequestID { set; get; }
        public DateTime RequestTime { set; get; }
        public int RequestUnixTime { set; get; }
        public DateTime ResponseTime { set; get; }
        public int ResponseUnixTime { set; get; }
        public int Month { set; get; }
        public Int16 StatusProcess { set; get; }
        public  string TelcoCode { set; get; }
        public string Message_GOC { set; get; }
    }

    public class SMS_MT
    {
        public Int64 MT_ID { set; get; }
        public Int64 MO_ID { set; get; }
        public  string UserID { set; get; }
        public string ServiceID { set; get; }
        public string CommandCode { set; get; }
        public string Message { set; get; }
        public Int64 RequestID { set; get; }
        public Int16 MsgType { set; get; }
        public int ContentType { set; get; }
        public DateTime RequestTime { set; get; }
        public int RequestUnixTime { set; get; }
        public DateTime ResponseTime { set; get; }
        public int ResponseUnixTime { set; get; }
        public int Month { set; get; }
        public int ResponseStatus { set; get; }
        public Int16 IsCDR { set; get; }
        public int RetryCount { set; get; }
        public int MsgCount { set; get; }
        public int Priority { set; get; }
        public string TelcoCode { set; get; }
    }
}
