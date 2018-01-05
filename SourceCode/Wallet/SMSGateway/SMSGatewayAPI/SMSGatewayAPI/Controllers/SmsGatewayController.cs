using DAL.Model;
using Newtonsoft.Json;
using Services.IService;
using SMSGatewayAPI.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilities;
using Utilities.CommonModel;

namespace SMSGatewayAPI.Controllers
{
    public class SmsGatewayController : ApiController
    {
        // GET: SmsGateway
        private ISMSGatewayService _iSMSGatewayService;

        public SmsGatewayController(ISMSGatewayService iSMSGatewayService)
        {
            _iSMSGatewayService = iSMSGatewayService;
        }


        [HttpPost]
        public HttpResponseMessage RecieveMO(dynamic data)
        {
            try
            {
                if (data.UserID == null || data.ServiceID == null || data.CommandCode == null || data.Message == null || data.RequestID == null || data.Message_GOC == null || data.TelcoCode == null)
                {
                    NLogLogger.LogInfo($"requestData:{JsonConvert.SerializeObject(data)}");
                    return HttpHelper.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(new ClientMessage(ErrorCodes.INVALID_DATA) { Message = "Request data invalid" }), "Request data invalid");
                }
                var requestData = new SMS_MO
                {
                    UserID = data.UserID,
                    ServiceID = data.ServiceID,
                    CommandCode = data.CommandCode,
                    Message = data.Message,
                    RequestID = data.RequestID,
                    Message_GOC = data.Message_GOC,
                    TelcoCode = data.TelcoCode
                };
                int response = _iSMSGatewayService.SMS_MO_Insert(requestData);
                if (response > 0)
                    return HttpHelper.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(new ClientMessage(ErrorCodes.DONE)), "Successful");

                NLogLogger.LogInfo($"requestData:{JsonConvert.SerializeObject(requestData)} | response: {response}");
                switch (response)
                {
                    case ErrorCodes.PHONE_NUMBER_INVALID:
                        return HttpHelper.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(new ClientMessage(ErrorCodes.PHONE_NUMBER_INVALID) { Message = "Invalid phone number" }), "Invalid phone number");
                    case ErrorCodes.NETWORK_SUPLIER_INVALID:
                        return HttpHelper.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(new ClientMessage(ErrorCodes.NETWORK_SUPLIER_INVALID) { Message = "Telco invalid" }), "Telco invalid");
                    case ErrorCodes.REQUEST_ID_DUPLICATE:
                        return HttpHelper.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(new ClientMessage(ErrorCodes.REQUEST_ID_DUPLICATE) { Message = "Duplicate request id" }), "Duplicate request id");
                    case ErrorCodes.SEND_TOO_FAST:
                        return HttpHelper.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(new ClientMessage(ErrorCodes.SEND_TOO_FAST) { Message = "Messenger send too fast" }), "Messenger send too fast");
                    case ErrorCodes.INVALID_DATA:
                        return HttpHelper.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(new ClientMessage(ErrorCodes.INVALID_DATA) { Message = "Data invalid" }), "Data invalid");
                    default:
                        return HttpHelper.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(new ClientMessage(ErrorCodes.SYSTEM_ERROR) { Message = "System is busy, please try again later" }), "System is busy, please try again later");
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return HttpHelper.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(new ClientMessage(ErrorCodes.SYSTEM_ERROR) { Message = "System is busy, please try again later" }), "System is busy, please try again later");
            }
        }

        [HttpPost]
        public HttpResponseMessage SendMT(dynamic data)
        {

            try
            {
                if (data.MO_ID == null || data.UserID == null || data.ServiceID == null || data.CommandCode == null || data.Message == null || data.RequestID == null || data.MsgType == null
                    || data.ContentType == null || data.IsCDR == null || data.Priority == null)
                {
                    NLogLogger.LogInfo($"requestData:{JsonConvert.SerializeObject(data)}");
                    return HttpHelper.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(new ClientMessage(ErrorCodes.INVALID_DATA) { Message = "Request data invalid" }), "Request data invalid");
                }
                var requestData = new SMS_MT
                {
                    UserID = data.UserID,
                    ServiceID = data.ServiceID,
                    CommandCode = data.CommandCode,
                    Message = data.Message,
                    RequestID = data.RequestID,
                    MsgType = data.MsgType,
                    ContentType = data.ContentType,
                    IsCDR = data.IsCDR,
                    Priority = data.Priority
                };
                int response = _iSMSGatewayService.SMS_MT_Insert(requestData);
                if (response > 0)
                    return HttpHelper.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(new ClientMessage(ErrorCodes.DONE)), "Successful");

                NLogLogger.LogInfo($"requestData:{JsonConvert.SerializeObject(requestData)} | response: {response}");
                switch (response)
                {
                    case ErrorCodes.PHONE_NUMBER_INVALID:
                        return HttpHelper.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(new ClientMessage(ErrorCodes.PHONE_NUMBER_INVALID) { Message = "Invalid phone number" }), "Invalid phone number");
                    case ErrorCodes.NETWORK_SUPLIER_INVALID:
                        return HttpHelper.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(new ClientMessage(ErrorCodes.NETWORK_SUPLIER_INVALID) { Message = "Telco invalid" }), "Telco invalid");
                    case ErrorCodes.MO_NOT_EXIST:
                        return HttpHelper.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(new ClientMessage(ErrorCodes.MO_NOT_EXIST) { Message = "MO does't exist" }), "MO does't exist");
                    case ErrorCodes.INVALID_DATA:
                        return HttpHelper.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(new ClientMessage(ErrorCodes.INVALID_DATA) { Message = "Data invalid" }), "Data invalid");
                    default:
                        return HttpHelper.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(new ClientMessage(ErrorCodes.SYSTEM_ERROR) { Message = "System is busy, please try again later" }), "System is busy, please try again later");
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return HttpHelper.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(new ClientMessage(ErrorCodes.SYSTEM_ERROR) { Message = "System is busy, please try again later" }), "System is busy, please try again later");
            }
        }
    }
}