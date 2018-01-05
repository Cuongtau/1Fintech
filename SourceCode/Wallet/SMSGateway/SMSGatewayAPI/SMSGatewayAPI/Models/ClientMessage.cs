using Newtonsoft.Json;
using System.Collections.Generic;
namespace SMSGatewayAPI.Models
{
    public class ClientMessage
    {
        [JsonProperty("c")]
        public int Code { get; set; }
        [JsonProperty("m")]
        public string Message { get; set; }
        [JsonProperty("p", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<object> Pars { get; set; }
        [JsonProperty("d", NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }
        public ClientMessage()
        {}

        public ClientMessage(int code, object data)
        {
            Code = code;
            Data = data;
        }
        public ClientMessage(int code)
        {
            Code = code;
        }
        public ClientMessage(int code, string message)
        {
            Code = code;
            Message = message;

        }
        public ClientMessage(int status, IEnumerable<object> pars)
        {
            Code = status;
            Pars = pars;
        }
        public ClientMessage(int status, object Data, IEnumerable<object> pars)
        {
            Code = status;
            Pars = pars;
            this.Data = Data;
        }
    }
}