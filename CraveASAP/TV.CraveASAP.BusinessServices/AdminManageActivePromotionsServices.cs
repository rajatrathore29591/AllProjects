using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Hosting;
using TV.CraveASAP.BusinessEntities;
using TV.CraveASAP.BusinessServices.Interfaces;
using TV.CraveASAP.DataModel;
using TV.CraveASAP.DataModel.UnitOfWork;

namespace TV.CraveASAP.BusinessServices
{
    public class AdminManageActivePromotionsServices : IAdminManageActivePromotionsServices
    {
        private readonly UnitOfWork _unitOfWork;
        public AdminManageActivePromotionsServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        public IEnumerable<VendorEntity> GetAllPromotions()
        {
            var vendors = _unitOfWork.VendorRepository.GetByCondition(d => d.isDeleted == false).ToList();
            if (vendors.Any())
            {
                Mapper.CreateMap<Vendor, VendorEntity>();
                Mapper.CreateMap<PromotionCode, PromotionCodeEntity>();
                Mapper.CreateMap<OpeningHour, OpeningHourEntity>();
                var vendorsModel = Mapper.Map<List<Vendor>, List<VendorEntity>>(vendors);
                return vendorsModel;
            }
            return null;
        }

        public IEnumerable<PromotionListEntity> GetPromotion()
        {
            var promotion = _unitOfWork.VendorPromotionRepository.GetByCondition(d => d.isDeleted == false && d.isActive == true).ToList();
            var allvendor = _unitOfWork.VendorRepository.GetByCondition( d => d.isDeleted == false).ToList();
            var category = _unitOfWork.CategoryRepository.GetAll().ToList();
            if (promotion.Any())
            {
                //AutoMapper.Mapper.Reset();
                AutoMapper.Mapper.CreateMap<PromotionCode, PromotionListEntity>().ForMember(d => d.vendorName, o => o.MapFrom(s => s.vendorId == null ? "VendorNameA" : Convert.ToString(allvendor.Where(z => z.vendorId.Equals(s.vendorId)).FirstOrDefault().companyName))).ForMember(d => d.categoryName, o => o.MapFrom(s => s.vendorId == null ? "VendorNameA" : Convert.ToString(category.Where(z => z.categoryId.Equals(s.categoryId)).FirstOrDefault().categoryName)));
                var promotionModel = Mapper.Map<List<PromotionCode>, List<PromotionListEntity>>(promotion);
                return promotionModel;
            }
            return null;
        }

        public PromotionCodeEntity GetAllPromotionsById(int promotionCodeId)
        {
            var promotion = _unitOfWork.AdminManageActivePromotionRepository.GetByCondition(d => d.promotionCodeId == promotionCodeId).FirstOrDefault();
            if (promotion != null)
            {
                Mapper.CreateMap<PromotionCode, PromotionCodeEntity>();
                var promotionModel = Mapper.Map<PromotionCode, PromotionCodeEntity>(promotion);
                return promotionModel;
            }
            return null;
        }

        public IEnumerable<PromotionCodeEntity> GetAllActivePromotion()
        {
            var promotion = _unitOfWork.VendorPromotionRepository.GetByCondition(d => d.isDeleted == false).OrderByDescending(d => d.createdDate).Take(3).ToList();
            if (promotion.Any())
            {
                Mapper.CreateMap<PromotionCode, PromotionCodeEntity>();
                var promotionModel = Mapper.Map<List<PromotionCode>, List<PromotionCodeEntity>>(promotion);
                return promotionModel;
            }
            return null;
        }

        public string base64ToImage(string logImg, int MaxId, string oldImage)
        {
            string filePath = string.Empty;
            string image = string.Empty;
            var path = DateTime.Now;
            var data = String.Format("{0:d/M/yyyy HH:mm:ss}", path);
            data = data.Replace(@"/", "").Trim(); data = data.Replace(@":", "").Trim(); data = data.Replace(" ", String.Empty);
            if (!string.IsNullOrEmpty(logImg))
            {
                filePath = HostingEnvironment.MapPath("~/Pictures/");
                image = MaxId + "_PromotionCodepic_" + data + ".png";
                if (File.Exists(filePath + oldImage))
                {
                    System.IO.File.Delete((filePath + oldImage));
                }
                byte[] bytes = System.Convert.FromBase64String(logImg);
                FileStream fs = new FileStream(filePath + image, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
            return image;
        }

        public bool DeletePromotion(int ID)
        {
            var success = false;
            if (ID > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var user = _unitOfWork.AdminManageActivePromotionRepository.GetByID(ID);
                    if (user != null)
                    {
                        _unitOfWork.AdminManageActivePromotionRepository.Delete(user);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

    }
}
