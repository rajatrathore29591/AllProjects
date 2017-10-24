using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace TradeSystem.Utils
{
    public class UploadImage
    {
        public static string base64ToImage(string logImg, string ImageName)
        {
            string filePath = string.Empty;
            string image = string.Empty;
            var path = DateTime.Now;
            //var data = String.Format("{0:d/M/yyyy HH:mm:ss}", path);
            //data = data.Replace(@"/", "").Trim(); data = data.Replace(@":", "").Trim(); data = data.Replace(" ", String.Empty);
            if (!string.IsNullOrEmpty(logImg))
            {
                filePath = HostingEnvironment.MapPath("~/Pictures/");
                image = ImageName;
                //if (File.Exists(filePath + oldImage))
                //{
                //    System.IO.File.Delete((filePath + oldImage));
                //}
                byte[] bytes = System.Convert.FromBase64String(logImg);
                FileStream fs = new FileStream(filePath + image, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
            return image;
        }
    }
}
