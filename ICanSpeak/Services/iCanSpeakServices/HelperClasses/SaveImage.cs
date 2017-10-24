using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Net;
using System.Drawing.Imaging;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace iCanSpeakServices.HelperClasses
{
    public class SaveImage
    {
       

        /// <summary>
        /// CreateSuccess
        /// </summary>
        /// <param name="SuccessMessage">Success Message</param>
        /// <returns></returns>
        

        

        public Image Base64ToImage(string base64String)
        {

            try
            {
               

                base64String = base64String.Replace(" ", "+");
                int mod4 = base64String.Length % 4;
                if (mod4 > 0)
                {
                    base64String += new string('=', 4 - mod4);
                }
                // Convert Base64 String to byte[]
                byte[] imageBytes = Convert.FromBase64String(base64String);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                // Convert byte[] to Image
                ms.Write(imageBytes, 0, imageBytes.Length);
                Image image = Image.FromStream(ms, true);
                return image;


            }

            catch (Exception ex)
            {

                //do Not throw any error
            }

            return null;
        }

        public static void CreateSubAdminTutor(int userid, Image image, int maxWidth, int maxHeight)
        {
            try
            {
                if (image != null)
                {

                    int newWidth = 0;
                    int newHeight = 0;
                    int currentWidth = image.Width;
                    int currentHeight = image.Height;

                    if ((currentWidth / (double)maxWidth) < (currentHeight / (double)maxHeight))
                    {
                        newWidth = maxWidth;
                        newHeight = Convert.ToInt32(currentHeight * (maxWidth / (double)currentWidth));
                        if (newHeight > maxHeight)
                        {
                            newWidth = Convert.ToInt32(maxWidth * (maxHeight / (double)newHeight));
                            newHeight = maxHeight;
                            if (newWidth > maxWidth)
                            {
                                newWidth = maxWidth;
                                newHeight = Convert.ToInt32(maxHeight * (maxWidth / (double)newWidth));
                            }
                        }
                    }
                    else
                    {
                        newWidth = Convert.ToInt32(currentWidth * (maxHeight / (double)currentHeight));
                        newHeight = maxHeight;
                        if (newHeight > maxHeight)
                        {
                            newWidth = Convert.ToInt32(maxWidth * (maxHeight / (double)newHeight));
                            newHeight = maxHeight;
                        }
                    }

                    Bitmap newImage = new Bitmap(newWidth, newHeight);
                    newImage.SetResolution(326, 326); //web resolution 326;      

                    //create a graphics object      
                    Graphics gr = Graphics.FromImage(newImage);

                    //just in case it's a transparent GIF force the bg to white     
                    SolidBrush sb = new SolidBrush(System.Drawing.Color.White);
                    gr.FillRectangle(sb, 0, 0, newImage.Width, newImage.Height);

                    //Re-draw the image to the specified height and width     
                    gr.DrawImage(image, 0, 0, newImage.Width, newImage.Height);

                    // Dispose to free up resources
                    sb.Dispose();
                    gr.Dispose();
                    // Rectangle cropArea = new Rectangle(0, 0, 560, 560);
                    //newImage.Save(HttpContext.Current.Server.MapPath("feedimage/" + mediaid + "_560x560.jpg"));
                    Image newImage1 = cropImage(newImage, new System.Drawing.Rectangle(0, 0, newWidth, newHeight));

                    newImage1.Save(HttpContext.Current.Server.MapPath("~/ProfilePictures/" + userid + "_ProfilePic.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                   // newImage1.GetThumbnailImage(144, 144, null, IntPtr.Zero).Save(HttpContext.Current.Server.MapPath("~/feedimage/" + mediaid + "_" + photoNumber + "_thumb.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);



                    #region MyRegion
                    //Crop
                    //Graphics gfx = Graphics.FromImage(newImage);
                    //gfx.SmoothingMode = SmoothingMode.AntiAlias;
                    //gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    //gfx.DrawImage(image, new Rectangle(0, 0, 280, 280), 0, 0, 280, 280, GraphicsUnit.Pixel);
                    //newImage.Save(HttpContext.Current.Server.MapPath("feedimage/" + mediaid + "_280x560.jpg")) 
                    #endregion;

                    // Dispose to free up resources
                    newImage.Dispose();
                }
            }
            catch (Exception)
            {

            }
        }

        //public static void CreateSubAdminTutor(int userid, Image image, int maxWidth, int maxHeight)
        //{

        //    try
        //    {
        //        if (image != null)
        //        {

        //            int newWidth = 0;
        //            int newHeight = 0;
        //            int currentWidth = image.Width;
        //            int currentHeight = image.Height;

        //            if ((currentWidth / (double)maxWidth) < (currentHeight / (double)maxHeight))
        //            {
        //                newWidth = maxWidth;
        //                newHeight = Convert.ToInt32(currentHeight * (maxWidth / (double)currentWidth));
        //                if (newHeight > maxHeight)
        //                {
        //                    newWidth = Convert.ToInt32(maxWidth * (maxHeight / (double)newHeight));
        //                    newHeight = maxHeight;
        //                    if (newWidth > maxWidth)
        //                    {
        //                        newWidth = maxWidth;
        //                        newHeight = Convert.ToInt32(maxHeight * (maxWidth / (double)newWidth));
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                newWidth = Convert.ToInt32(currentWidth * (maxHeight / (double)currentHeight));
        //                newHeight = maxHeight;
        //                if (newHeight > maxHeight)
        //                {
        //                    newWidth = Convert.ToInt32(maxWidth * (maxHeight / (double)newHeight));
        //                    newHeight = maxHeight;
        //                }
        //            }



        //            Bitmap newImage = new Bitmap(newWidth, newHeight);
        //            newImage.SetResolution(326, 326); //web resolution 326;      

        //            //create a graphics object      
        //            Graphics gr = Graphics.FromImage(newImage);

        //            //just in case it's a transparent GIF force the bg to white     
        //            SolidBrush sb = new SolidBrush(System.Drawing.Color.White);
        //            gr.FillRectangle(sb, 0, 0, newImage.Width, newImage.Height);

        //            //Re-draw the image to the specified height and width     
        //            gr.DrawImage(image, 0, 0, newImage.Width, newImage.Height);

        //            // Dispose to free up resources
        //            sb.Dispose();
        //            gr.Dispose();
        //            // Rectangle cropArea = new Rectangle(0, 0, 560, 560);
        //            //newImage.Save(HttpContext.Current.Server.MapPath("feedimage/" + mediaid + "_560x560.jpg"));
        //            Image newImage1 = cropImage(newImage, new System.Drawing.Rectangle(0, 0, newWidth, newHeight));



        //            newImage1.Save(HttpContext.Current.Server.MapPath("~/ProfilePictures/" + userid + "_ProfilePic.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);

        //            //  newImage1.GetThumbnailImage(100, 100, null, IntPtr.Zero).Save(HttpContext.Current.Server.MapPath("../../profileimage/" + partyId + "_profilethumbnail.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);



        //            #region MyRegion
        //            //Crop
        //            //Graphics gfx = Graphics.FromImage(newImage);
        //            //gfx.SmoothingMode = SmoothingMode.AntiAlias;
        //            //gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            //gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //            //gfx.DrawImage(image, new Rectangle(0, 0, 280, 280), 0, 0, 280, 280, GraphicsUnit.Pixel);
        //            //newImage.Save(HttpContext.Current.Server.MapPath("feedimage/" + mediaid + "_280x560.jpg")) 
        //            #endregion;

        //            // Dispose to free up resources
        //            newImage.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {



        //    }
        //}


      


        private static Bitmap bmpCrop;
        private static Image cropImage(Image img, System.Drawing.Rectangle cropArea)
        {
            try
            {
                if (cropArea.Width != 0 && cropArea.Height != 0)
                {
                    Bitmap bmpImage = new Bitmap(img);
                    // bmpImage.SetResolution(326, 326);
                    bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
                    bmpImage.Dispose();
                    return (Image)(bmpCrop);
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="Decrypt">encrypted string</param>
        /// <returns></returns>
        public string Decrypt(string sData)
        {
            try
            {

                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();

                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(sData);

                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);

                char[] decoded_char = new char[charCount];

                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);

                string result = new String(decoded_char);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Decode" + ex.Message);
            }

        }
    

    }
}