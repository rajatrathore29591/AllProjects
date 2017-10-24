using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TV.CraveASAP.BusinessEntities;
using TV.CraveASAP.BusinessServices.Interfaces;
using TV.CraveASAP.DataModel;
using TV.CraveASAP.DataModel.UnitOfWork;
using System.Drawing;
using System.Web.Hosting;
using System.Globalization;
using System.IO;
using System.Web.UI.WebControls;
using System.Web;
using System.Drawing.Imaging;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Diagnostics;
using TV.CraveASAP.BusinessServices.HelperClass;

namespace TV.CraveASAP.BusinessServices
{
    public class BannerServices : IBannerServices
    {
        private readonly UnitOfWork _unitOfWork;
        public BannerServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        public IEnumerable<BannersEntity> GetAllBanners(string type, string platfrom, string language)
        {
            var Banners = _unitOfWork.BannerRepository.GetByCondition(x => x.type == type.Trim() && x.platform == platfrom.Trim() && x.language == language.Trim()).ToList();
            if (Banners.Any())
            {
                Mapper.CreateMap<Banner, BannersEntity>();
                var BannerModel = Mapper.Map<List<Banner>, List<BannersEntity>>(Banners);
                return BannerModel;
            }
            return null;
        }

        public bool UpdateBanners(int bannerId, BannersEntity bannerEntity)
        {
            bool success = true;
            return success;
        }

        public BannersEntity UploadVideo(Stream Uploading, string VidoType)
        {
            string filePath = HostingEnvironment.MapPath("~/Video/");
            BannersEntity upload = new BannersEntity
            {
                FilePath = Path.Combine(filePath, VidoType + "Video.mp4")
            };

            int length = 0;
            using (FileStream writer = new FileStream(upload.FilePath, FileMode.Create))
            {
                int readCount;
                var buffer = new byte[8192];
                while ((readCount = Uploading.Read(buffer, 0, buffer.Length)) != 0)
                {
                    writer.Write(buffer, 0, readCount);
                    length += readCount;
                }
            }
            upload.FileLength = Convert.ToString(length);
            using (var scope = new TransactionScope())
            {
                var record = _unitOfWork.ContentMgtRepository.GetByID(VidoType == "User" ? 2 : 1);
                record.contentLink = VidoType + "Video.mp4";
                _unitOfWork.ContentMgtRepository.Update(record);
                _unitOfWork.Save();
                scope.Complete();
            }
            return upload;
        }

        public int CreateBanner(IEnumerable<BannersEntity> _BannerEntity)
        {
            foreach (var Banner in _BannerEntity)
            {
                if (Encrypt_decrypt.IsBase64(Banner.imageURL))
                {
                    using (var scope = new TransactionScope())
                    {
                        var banner = new Banner
                        {
                            type = Banner.type,
                            platform = Banner.platform,
                            reference = Banner.reference,
                            createdDate = DateTime.Now,
                            delay = Banner.delay,
                            language = Banner.language
                        };

                        _unitOfWork.BannerRepository.Insert(banner);
                        _unitOfWork.Save();
                        var MaxId = _unitOfWork.BannerRepository.GetAll().Max(U => U.bannerId);
                        var oldImg = _unitOfWork.BannerRepository.GetByID(MaxId);
                        var imgPath = base64ToImage1(Banner, MaxId, oldImg.imageURL);
                        if (Banner.imageURL != "video".Trim())
                        {
                            if (imgPath != null)
                            {
                                var record = _unitOfWork.BannerRepository.GetByID(MaxId);
                                record.imageURL = imgPath;
                                _unitOfWork.BannerRepository.Update(record);
                                _unitOfWork.Save();
                                scope.Complete();
                            }
                        }
                        else
                        {
                            var record = _unitOfWork.BannerRepository.GetByID(MaxId);
                            record.imageURL = record.bannerId + "_video.mp4";
                            _unitOfWork.BannerRepository.Update(record);
                            _unitOfWork.Save();
                            scope.Complete();
                        }
                    }
                }
            }
            return 1;
        }

        public string base64ToImage1(BannersEntity Banner, int MaxId, string oldImage)
        {
            string filePath = string.Empty;
            string image = string.Empty;
            var path = DateTime.Now;
            var data = String.Format("{0:d/M/yyyy HH:mm:ss}", path);
            data = data.Replace(@"/", "").Trim(); data = data.Replace(@":", "").Trim(); data = data.Replace(" ", String.Empty);
            if (!string.IsNullOrEmpty(Banner.imageURL))
            {
                filePath = HostingEnvironment.MapPath("~/Pictures/");
                image = MaxId + "_AppBannerPic.png" + data + ".png";
                if (File.Exists(filePath + oldImage))
                {
                    System.IO.File.Delete((filePath + oldImage));
                }
                byte[] bytes = System.Convert.FromBase64String(Banner.imageURL);
                FileStream fs = new FileStream(filePath + image, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
            return image;
        }

        public IEnumerable<BannersEntity> GetAllHowItWork(string platform)
        {
           // var language = _unitOfWork.UserRepository.get
            var Banners = _unitOfWork.BannerRepository.GetByCondition(x => x.type == "2" && x.platform == platform.Trim()).ToList();
            if (Banners.Any())
            {
                Mapper.CreateMap<Banner, BannersEntity>();
                var BannerModel = Mapper.Map<List<Banner>, List<BannersEntity>>(Banners);
                return BannerModel;
            }
            return null;
        }
        public int CreateHowItWork(BannersEntity Banner)
        {
            using (var scope = new TransactionScope())
            {
                var banner = new Banner
                {
                    type = Banner.type,
                    platform = Banner.platform,
                    imageURL = Banner.imageURL,
                    createdDate = Banner.createdDate,
                    delay = Banner.delay
                };

                _unitOfWork.BannerRepository.Insert(banner);
                _unitOfWork.Save();

                var MaxId = _unitOfWork.BannerRepository.GetAll().Max(U => U.bannerId);
                var oldImg = _unitOfWork.BannerRepository.GetByID(MaxId);
                var imgPath = base64ToImage(Banner, MaxId, oldImg.imageURL);
                if (imgPath != null)
                {
                    var record = _unitOfWork.BannerRepository.GetByID(MaxId);
                    record.imageURL = imgPath;
                    _unitOfWork.BannerRepository.Update(record);
                    _unitOfWork.Save();
                    scope.Complete();

                }

                return banner.bannerId;
            }
        }
        public bool DeleteHowItWork(int id)
        {
            bool success = true;
            return success;
        }
        public bool DeleteBanners(int id)
        {
            var success = false;

            using (var scope = new TransactionScope())
            {
                var deleteBanner = _unitOfWork.BannerRepository.GetByCondition(z => z.bannerId.Equals(id)).FirstOrDefault();
                _unitOfWork.BannerRepository.Delete(deleteBanner);
                _unitOfWork.Save();
                scope.Complete();
                success = true;
            }
            return success;
        }

        public string base64ToImage(BannersEntity Banner, int MaxId, string oldImage)
        {
            string filePath = string.Empty;
            string image = string.Empty;
            var path = DateTime.Now;
            var data = String.Format("{0:d/M/yyyy HH:mm:ss}", path);
            data = data.Replace(@"/", "").Trim(); data = data.Replace(@":", "").Trim(); data = data.Replace(" ", String.Empty);
            if (!string.IsNullOrEmpty(Banner.imageURL))
            {
                filePath = HostingEnvironment.MapPath("~/Pictures/");
                image = MaxId + "_HowitWorkPic_" + data + ".png";
                if (File.Exists(filePath + oldImage))
                {
                    System.IO.File.Delete((filePath + oldImage));
                }
                byte[] bytes = System.Convert.FromBase64String(Banner.imageURL);
                FileStream fs = new FileStream(filePath + image, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
            return image;
        }



        public void CreateSubAdminTutor(int type, System.Drawing.Image image, int maxWidth, int maxHeight)
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


                    string filePath = string.Empty;
                    var path = DateTime.Now;
                    var data = String.Format("{0:ddd, MMM d, yyyy}", path);
                    data = data.Replace(@",", "").Trim(); data = data.Replace(" ", String.Empty);

                    Bitmap newImage = new Bitmap(newWidth, newHeight);
                    newImage.SetResolution(326, 326); //web resolution 326;      
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
                    System.Drawing.Image newImage1 = cropImage(newImage, new System.Drawing.Rectangle(0, 0, newWidth, newHeight));

                    newImage1.Save(HttpContext.Current.Server.MapPath("~/Pictures/" + data + "_" + type + "_ProfilePic.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);

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


        private static Bitmap bmpCrop;
        private static System.Drawing.Image cropImage(System.Drawing.Image img, System.Drawing.Rectangle cropArea)
        {
            try
            {
                if (cropArea.Width != 0 && cropArea.Height != 0)
                {
                    Bitmap bmpImage = new Bitmap(img);
                    bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
                    bmpImage.Dispose();
                    return (System.Drawing.Image)(bmpCrop);
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
