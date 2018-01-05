using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Utilities
{
    public static class NLogLogger
    {
        static NLogLogger()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

        public static Logger Logger { get; set; } 

        public static void LogInfo(string message)
        {
            Info(message, false);
        }

        public static void Info(string message, bool sendMail)
        {
            var mes = GetCalleeString() + Environment.NewLine + "\t" + message;
            Logger.Info(":\t" + mes);

        }

        public static void Info(string message)
        {
            Info(message, false);
        }


        public static void TraceMessage(string message)
        {
            Logger.Trace("\t" + message);
        }
        public static void PublishException(Exception ex, bool sendmail)
        {
            DebugMessage(ex.Message + Environment.NewLine + ex.StackTrace, sendmail);

        }
        public static void PublishException(Exception ex)
        {
            PublishException(ex, true);
        }

        public static void DebugMessage(object o)
        {
            DebugMessage(GetValueOfObject(o));
        }

        public static void DebugMessage(string message, bool sendEmail)
        {
            var m = GetCalleeString() + Environment.NewLine + "\t" + message;
            Logger.Debug(":\t" + m);



        }

        public static void DebugMessage(string message)
        {
            DebugMessage(message, false);
        }

        public static void LogDebug(string p)
        {
            DebugMessage(p);
        }

        public static void LogWarning(object o)
        {
            LogWarning(GetValueOfObject(o));
        }

        public static void LogWarning(string message, bool sendMail)
        {
            var error = GetCalleeString() + Environment.NewLine + "\t" + message;

            Logger.Warn(":\t" + error);

            if (sendMail)
                SendmailHelper.SendError(error);
        }

        public static void LogWarning(string message)
        {
            LogWarning(message, true);
        }

        public static void Fatal(string message)
        {
            Logger.Fatal(":\t" + GetCalleeString() + Environment.NewLine + "\t" + message);
        }

        private static string GetCalleeString()
        {
            foreach (var sf in new StackTrace().GetFrames())
            {
                if (sf.GetMethod().ReflectedType.Namespace != "Portal.Common")
                {
                    return string.Format("{0}.{1} ", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name);
                }
            }

            return string.Empty;
        }


        public static string GetValueOfObject(object ob)
        {
            var sb = new StringBuilder();
            try
            {
                foreach (System.Reflection.PropertyInfo piOrig in ob.GetType().GetProperties())
                {
                    object editedVal = ob.GetType().GetProperty(piOrig.Name).GetValue(ob, null);
                    sb.AppendFormat("{0}:{1}\t ", piOrig.Name, editedVal);

                }
            }
            catch { }
            return sb.ToString();
        }
    }




    public class SendMail
    {
        public int MailId { get; set; }

        public string MailFromName { get; set; }

        public string MailFrom { get; set; }

        public string MailTo { get; set; }

        public string MailSubject { get; set; }

        public string MailBody { get; set; }

        public int Status { get; set; }

        public int ErrorCode { get; set; }
    }


    public class SendmailHelper
    {
        /// <summary>
        /// Send mail by smtp
        /// format by nguyen van khuyen
        /// date : 22/01/2010
        /// </summary>
        private const string VtcDomain = "sandbox.vtcebank.vn"; //"smtp.mail.vtc.vn";
        private const string VtcUser = "log";
        private const string VtcPassword = "log";
        const string ProjectName = "Paygate";
        private static void Send(SendMail mailInfo)
        {
            SmtpClient smtpClient;
            var message = new MailMessage();

            try
            {
                smtpClient = new SmtpClient(VtcDomain, 25)
                {
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(VtcUser, VtcPassword)
                };
                //smtpClient = new SmtpClient("64.71.158.201", 25);
                message.From = new MailAddress("log@sandbox.vtcebank.vn", string.Format("VTC DEBUG [{0}]", Environment.MachineName));
                message.IsBodyHtml = false;
                message.Subject = mailInfo.MailSubject.Replace("\r\n", " ") + " from " + Environment.MachineName;
                message.Body = mailInfo.MailBody;
                message.To.Add(mailInfo.MailTo);
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex, false);

                smtpClient = new SmtpClient(VtcDomain, 25);
                message.From = new MailAddress("taipt@sandbox.vtcebank.vn", string.Format("Alter VTC Debug [{0}]", Environment.MachineName));
                message.IsBodyHtml = false;
                message.Subject = mailInfo.MailSubject.Replace("\r\n", " ") + " from " + Environment.MachineName;
                message.Body = mailInfo.MailBody;
                message.To.Add(mailInfo.MailTo);
                smtpClient.Send(message);
            }

        }


        /// <summary>
        /// gửi thông báo lỗi tới email được config khi chương trình gặp lỗi
        /// </summary>
        /// <param name="messenger"></param>
        public static void SendError(string messenger)
        {

            try
            {
                messenger = messenger + Environment.NewLine + "Date :" + DateTime.Now;
                var listMail = ConfigurationManager.AppSettings["ErrorToEmail"].Split(';');
                foreach (string t in listMail)
                {
                    var sendmail = new SendMail
                    {
                        MailSubject = string.Format("[{0}][ErrorMail][{1}] {2}!", Environment.MachineName, ProjectName, messenger.Length > 20 ? messenger.Substring(0, 16) : "Error!").Replace("\r\n", " "),
                        MailTo = t ?? "taipt@sandbox.vtcebank.vn",
                        MailBody = messenger
                    };
                    Send(sendmail);
                }
                return;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex, false);
                return;
            }
        }

        /// <summary>
        /// gửi thông báo lỗi tới email được config khi chương trình gặp lỗi
        /// </summary>
        /// <param name="messenger"></param>
        public static void SendDebug(string messenger)
        {
            try
            {
                messenger = messenger + Environment.NewLine + "Date :" + DateTime.Now;
                var sendmail = new SendMail
                {
                    MailSubject = string.Format("[{0}][DebugInfo][{1}] {2}!", Environment.MachineName, ProjectName, messenger.Length > 20 ? messenger.Substring(0, 16) : "Debug Info!").Replace("\r\n", " "),
                    MailTo = ConfigurationManager.AppSettings["ErrorToEmail"] ?? "taipt@sandbox.vtcebank.vn",
                    MailBody = messenger
                };
                Send(sendmail);

                return;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex, false);
                return;
            }
        }

    }
}
