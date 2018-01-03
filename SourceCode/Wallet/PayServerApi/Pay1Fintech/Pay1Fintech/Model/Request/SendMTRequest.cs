namespace Pay1Fintech.Model.Request
{
    public class SendMTRequest
    {
        public string Partner_code { get; set; }
        public string User_id { get; set; }
        public string Message { get; set; }
        public string Service_id { get; set; }
        public string Command_code { get; set; }
        public string Request_id { get; set; }
        public byte Message_index { get; set; }
        public int Mt_end { get; set; }
        public byte Total_message { get; set; }
        public byte Content_type { get; set; }
        public int Reference_id { get; set; }
        public string Signature { get; set; }
    }
}
