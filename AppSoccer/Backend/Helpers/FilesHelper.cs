using System;
using System.IO;
using System.Web;

namespace Backend.Helpers
{
    public class FilesHelper
    {
        public static string UploadPhoto(HttpPostedFileBase file, string folder)
        {
            string path = string.Empty;
            string pic = string.Empty;

            if (file != null)
            {
                pic = Path.GetFileName(file.FileName);
                path = Path.Combine(HttpContext.Current.Server.MapPath(folder), pic);
                file.SaveAs(path);
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
            }

            return pic;
        }

        public  void ErrorLogging(System.Exception ex)
        {
         //   string strPath = @"..\App_Data\Log.txt";
            string fullSavePath = HttpContext.Current.Server.MapPath("~/App_Data/Log.txt" );
            if (!File.Exists(fullSavePath))
            {
                File.Create(fullSavePath).Dispose();
            }
            using (StreamWriter sw = File.AppendText(fullSavePath))
            {
                sw.WriteLine("=============Error Logging ===========");
                sw.WriteLine("===========Start============= " + DateTime.Now);
                sw.WriteLine("Error Message: " + ex.Message);
                sw.WriteLine("Stack Trace: " + ex.StackTrace);
                sw.WriteLine("===========End============= " + DateTime.Now);

            }
        }
    }
}