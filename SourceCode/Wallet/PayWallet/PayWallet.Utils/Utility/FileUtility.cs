using System;
using System.IO;
using System.Web;

namespace PayWallet.Utils
{
    public class FileUtility
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static void DownloadSpeed(string fileName)
        {
            // The file path to download.
            var filepath = string.Format("{0}{1}", Config.FilePathAppsetting, fileName);

            // The file name used to save the file to the client's system..
            Stream stream = null;
            var extension = Path.GetExtension(filepath);
            string contentType;
            switch (extension.ToLower())
            {
                case ".xls":
                case ".xlsx":
                    contentType = "application/ms-excel";
                    break;
                case ".doc":
                case ".docx":
                    contentType = "application/ms-word";
                    break;
                default:
                    contentType = "application/octet-stream";
                    break;
            }
            try
            {
                // Open the file into a stream.
                stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
                // Total bytes to read:
                var bytesToRead = stream.Length;
                HttpContext.Current.Response.ContentType = contentType;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                // Read the bytes from the stream in small portions.
                while (bytesToRead > 0)
                {
                    // Make sure the client is still connected.
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        // Read the data into the buffer and write into the
                        // output stream.
                        var buffer = new Byte[10000];
                        var length = stream.Read(buffer, 0, 10000);
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                        HttpContext.Current.Response.Flush();
                        // We have already read some bytes.. need to read
                        // only the remaining.
                        bytesToRead = bytesToRead - length;
                    }
                    else
                    {
                        // Get out of the loop, if user is not connected anymore..
                        bytesToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
                // An error occurred..
            }
            finally
            {
                if (stream != null)
                {
                    HttpContext.Current.Response.End();
                    stream.Close();
                    if (File.Exists(filepath))
                        File.Delete(filepath);
                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static void DownloadNormal(string fileName)
        {
            // The file path to download.
            string filepath = string.Format("{0}{1}", Config.FilePathAppsetting, fileName);

            // The file name used to save the file to the client's system..
            Stream stream = null;
            var extension = Path.GetExtension(filepath);
            string contentType;
            switch (extension.ToLower())
            {
                case ".xls":
                case ".xlsx":
                    contentType = "application/ms-excel";
                    break;
                case ".doc":
                case ".docx":
                    contentType = "application/ms-word";
                    break;
                case ".jpg":
                    contentType = "image/jpeg";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".gif":
                    contentType = "image/gif";
                    break;
                default:
                    contentType = "application/octet-stream";
                    break;
            }
            try
            {

                HttpContext.Current.Response.ContentType = contentType;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);

                HttpContext.Current.Response.WriteFile(filepath);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
                if (File.Exists(filepath))
                    File.Delete(filepath);

            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
                // An error occurred..
            }

        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="context"></param>
        public static void DownloadNormal(string fileName, HttpContext context)
        {
            // The file path to download.

            string filepath = string.Format("{0}{1}", Config.FilePathAppsetting, fileName);

            // The file name used to save the file to the client's system..

            Stream stream = null;
            var extension = Path.GetExtension(filepath);
            string contentType;
            switch (extension.ToLower())
            {
                case ".xls":
                case ".xlsx":
                    contentType = "application/ms-excel";
                    break;
                case ".doc":
                case ".docx":
                    contentType = "application/ms-word";
                    break;
                case ".jpg":
                    contentType = "image/jpeg";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".gif":
                    contentType = "image/gif";
                    break;
                default:
                    contentType = "application/octet-stream";
                    break;
            }
            try
            {

                context.Response.ContentType = contentType;
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);

                context.Response.WriteFile(filepath);
                context.Response.Flush();
                context.Response.End();
                if (File.Exists(filepath))
                    File.Delete(filepath);

            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
                // An error occurred..
            }
        }
    }
}
