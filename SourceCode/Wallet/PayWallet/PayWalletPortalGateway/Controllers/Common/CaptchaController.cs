using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Web.Http;
using PayWallet.Utils;
using PayWallet.Utils.Security;
using Encoder = System.Drawing.Imaging.Encoder;

namespace PayWallet.PortalGateway.Controllers.Common
{
    #region Captcha new

    public class CaptchaController : ApiController
    {
        ////public Captcha CurrentCaptcha { get; set; }
        //// GET api/Captcha/length
        //[HttpGet]
        //public dynamic Get(int length = 6, int width = 60, int height = 26, string oldVerify = "")
        //{
        //    return CaptchaUtil.GetCaptcha(length, width, height, oldVerify);
        //}

        [HttpGet]
        public dynamic Get(string oldVerify = "")
        {
            return CaptchaUtil.GetCaptcha(6, 60, 16, oldVerify);
        }

        [HttpGet]
        public dynamic Get(int length, int width, int height,string oldVerify = "")
        {
            return CaptchaUtil.GetCaptcha(length, width, height, oldVerify);
        }
 

        [HttpGet]
        public dynamic VerifyCaptcha(string captcha, string verify)
        {
            var result = CaptchaUtil.VerifyCaptcha(captcha, verify);

            return result > 0 ? 1 : -1;
        }
    }

    public class CaptchaUtil
    {
        private static ConcurrentDictionary<string, string> CaptchaTexts = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Sinh mã captcha
        /// </summary>
        /// <returns>
        ///     string[]
        ///     [0]: verify
        ///     [1]: image data contain captcha text
        /// </returns>
        public static CaptchaData GetCaptcha(int length, int width, int height, string oldVerify = "")
        {
            string verify = string.Empty;
            string captchaText = string.Empty;

            //refresh captcha
            if (!string.IsNullOrEmpty(oldVerify))
            {
                CaptchaTexts.TryRemove(oldVerify, out captchaText);
            }

            string token = Security.GetVerifyToken(ref verify);
            captchaText = Security.GetTokenPlainText(token);
            var resultAdd = CaptchaTexts.TryAdd(verify, token);

            var imageData = CaptchaUtil.ImageGenarate(captchaText, width, height);
            return new CaptchaData { Verify = verify, ImageData = imageData };
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của Captcha
        /// </summary>
        /// <param name="captchaText"></param>
        /// <param name="verify"></param>
        /// <returns>
        ///  1: Captcha hợp lệ
        /// -1: Captcha không chính xác
        /// -2: Captcha hết hạn
        /// 
        /// -99: Exception -> Captcha không hợp lệ
        /// </returns>
        public static int VerifyCaptcha(string captchaText, string verify)
        {
            try
            {
                //DateTime time = Security.GetTokenTime(verify);

                //if (DateTime.Compare(time.AddMinutes(30), DateTime.Now) < 0)
                //{
                //    return -2;
                //}

                string savedToken;
                CaptchaTexts.TryGetValue(verify, out savedToken);

                string savedCaptchaText = Security.GetTokenPlainText(savedToken);
                if (captchaText.ToUpper().Equals(savedCaptchaText))
                {
                    //if verified then remove captcha
                    CaptchaTexts.TryRemove(verify, out savedToken);
                    return 1;
                }

                return -1;
            }
            catch (Exception exception)
            {
                NLogLogger.PublishException(exception);
                return -99;
            }
        }

        private static string ImageGenarate(string captchaText, int width = 60, int height = 26)
        {
            var iNum = new Bitmap(width, height);

            var gf = Graphics.FromImage(iNum);
            gf.InterpolationMode = InterpolationMode.HighQualityBicubic;
            var br = new SolidBrush(Color.FromArgb(255, 255, 255));
            gf.FillRectangle(br, 0, 0, 120, 28);
            var strFormat = new StringFormat { Alignment = StringAlignment.Center };

            var rnd = new Random();
            for (var i = 0; i < captchaText.Length; i++)
            {
                var h = rnd.Next(0, 4);
                var cFont = new Font("arial", rnd.Next(9, 11), FontStyle.Bold);
                gf.DrawString(captchaText[i].ToString(CultureInfo.InvariantCulture), cFont, Brushes.Black, new RectangleF((i + 1) * 5, h, (i + 2) * 5, h + 16), strFormat);
            }

            var codec = GetEncoderInfo("image/jpeg");

            // set image quality
            var eps = new EncoderParameters();
            eps.Param[0] = new EncoderParameter(Encoder.Quality, (long)95);
            var ms = new MemoryStream();
            iNum.Save(ms, codec, eps);

            byte[] bitmapBytes = ms.GetBuffer();
            string result = Convert.ToBase64String(bitmapBytes, Base64FormattingOptions.InsertLineBreaks);

            ms.Close();
            ms.Dispose();
            iNum.Dispose();
            br.Dispose();

            return result;
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            var myEncoders =
                ImageCodecInfo.GetImageEncoders();

            foreach (var myEncoder in myEncoders)
                if (myEncoder.MimeType == mimeType)
                    return myEncoder;
            return null;
        }
    }

    public class CaptchaData
    {
        public string Verify { get; set; }
        public string ImageData { get; set; }
    }




    #endregion Captcha new
}