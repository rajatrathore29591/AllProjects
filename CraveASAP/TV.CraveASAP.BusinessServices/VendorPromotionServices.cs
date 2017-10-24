using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class VendorPromotionServices : IVendorPromotionServices
    {
        private readonly UnitOfWork _unitOfWork;
        OAuthServices oAuthServices = new OAuthServices();
        public VendorPromotionServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        //public IEnumerable<PromotionCodeEntity> GetActivePromotionByVendorId(int Id)
        //{
        //    List<PromotionCodeEntity> promotionsCodeEntity = new List<PromotionCodeEntity>();
        //    var PromotionInfo = _unitOfWork.AdminManageActivePromotionRepository.GetByCondition(d => d.isActive == true && d.vendorId == Id && d.isDeleted == false).OrderByDescending(o => o.promotionCodeId).ToList();
        //    if (PromotionInfo != null)
        //    {
        //        foreach (var Item in PromotionInfo)
        //        {
        //            var subCategory = _unitOfWork.SubCategoryRepository.GetByID(Item.subCategoryId);
        //            var optionalCategory = _unitOfWork.OptionalCategoryRepository.GetByID(Item.optCategoryId);
        //            var vendorImg = _unitOfWork.VendorRepository.GetByID(Item.vendorId);
        //            DateTime UTCTime = System.DateTime.UtcNow;
        //            DateTime dates2 = UTCTime.AddHours(7.0);
        //            promotionsCodeEntity.Add(new PromotionCodeEntity()
        //            {
        //                promotionCodeId = Item.promotionCodeId,
        //                code = Item.code,
        //                name = Item.name,
        //                descriptionEnglish = Item.descriptionEnglish,
        //                descriptionThai = Item.descriptionThai,
        //                createdDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.createdDate),
        //                expiryDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.expiryDate),
        //                isActive = Item.isActive,
        //                vendorId = Item.vendorId,
        //                quantity = Item.quantity != null ? Item.quantity : 0,
        //                recommendation = Item.recommendation,
        //                isPinned = Item.isPinned,
        //                categoryId = Item.categoryId,
        //                shareFacebookCount = Item.shareFacebookCount,
        //                shareInstagramCount = Item.shareInstagramCount,
        //                shareTwitterCount = Item.shareTwitterCount,
        //                price = Item.price,
        //                useCount = Item.useCount,
        //                viewCount = Item.viewCount,
        //                recommendationCount = Item.recommendationCount,
        //                promotionImage = Item.promotionImage,
        //                subCategoryId = Item.subCategoryId,
        //                optCategoryId = Item.optCategoryId,
        //                subCategoryname = subCategory != null ? subCategory.subCategoryName : "",
        //                optionalCategoryName = optionalCategory != null ? optionalCategory.optCategoryName : "",
        //                day = LeftTime(Convert.ToInt32(Item.quantity), Convert.ToString(Item.expiryDate), Convert.ToString(dates2)),
        //                logoImg = vendorImg.logoImg == null ? "" : vendorImg.logoImg,
        //                serverTime = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2)
        //            });
        //        }
        //        return promotionsCodeEntity;
        //    }
        //    return promotionsCodeEntity;
        //}

        public IEnumerable<PromotionCodeEntity> GetActivePromotionByVendorId(int Id)
        {
            List<PromotionCodeEntity> promotionsCodeEntity = new List<PromotionCodeEntity>();
            DateTime dates2 = System.DateTime.UtcNow.AddHours(7.0);
            var PromotionInfo = _unitOfWork.AdminManageActivePromotionRepository.GetByCondition(d => d.isActive == true && d.vendorId == Id && d.isDeleted == false).OrderByDescending(o => o.promotionCodeId).ToList();
            var qtyPromo = PromotionInfo.Where(z => z.expiryDate == null).ToList(); var datePromo = PromotionInfo.Where(z => z.expiryDate != null && z.expiryDate >= dates2).ToList();
            PromotionInfo = null; PromotionInfo = qtyPromo; PromotionInfo.AddRange(datePromo);
            PromotionInfo = PromotionInfo.OrderByDescending(o => o.promotionCodeId).ToList();
            var subCategory = _unitOfWork.SubCategoryRepository.GetByCondition(d => d.isDeleted == false).ToList();
            var optionalCategory = _unitOfWork.OptionalCategoryRepository.GetByCondition(d => d.isDeleted == false).ToList();
            //var vendorImg = _unitOfWork.VendorRepository.GetByCondition(p => PromotionInfo.Select(v => v.vendorId).Contains(p.vendorId)).ToList();
            var vendorImg = _unitOfWork.VendorRepository.GetByCondition(d => d.isDeleted == false).ToList();
           
            if (PromotionInfo != null)
            {
                AutoMapper.Mapper.Reset();
                AutoMapper.Mapper.CreateMap<PromotionCode, PromotionCodeEntity>().ForMember(d => d.subCategoryname, o => o.ResolveUsing(s => s.subCategoryId == 0 ? "" : Convert.ToString(subCategory.Where(z => z.subCategoryId.Equals(s.subCategoryId)).FirstOrDefault().subCategoryName))).
                    ForMember(d => d.optionalCategoryName, o => o.ResolveUsing(s => s.optCategoryId == 0 ? "" : Convert.ToString(optionalCategory.Where(z => z.optCategoryId.Equals(s.optCategoryId)).FirstOrDefault().optCategoryName))).
                    ForMember(d => d.logoImg, o => o.ResolveUsing(s => s.vendorId == 0 ? "" : Convert.ToString(vendorImg.Where(z => z.vendorId.Equals(s.vendorId)).FirstOrDefault().logoImg))).
                    ForMember(d => d.serverTime, o => o.ResolveUsing(s => s.promotionCodeId == 0 ? "date" : String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2))).
                    ForMember(d => d.expiryDate, o => o.ResolveUsing(s => s.expiryDate == null ? "" : String.Format("{0:dd/MM/yyyy h:mm:ss tt}", s.expiryDate))).
                    ForMember(d => d.createdDate, o => o.ResolveUsing(s => s.createdDate == null ? "" : String.Format("{0:dd/MM/yyyy h:mm:ss tt}", s.createdDate))).
                    ForMember(d => d.expiryDate1, o => o.ResolveUsing(s => s.expiryDate == null ? "" : String.Format("{0:dd/MM/yyyy HH:mm:ss}", s.expiryDate))).
                    ForMember(d => d.createdDate1, o => o.ResolveUsing(s => s.createdDate == null ? "" : String.Format("{0:dd/MM/yyyy HH:mm:ss}", s.createdDate))).
                    ForMember(d => d.serverTime1, o => o.ResolveUsing(s => s.promotionCodeId == null ? "date" : String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.UtcNow.AddHours(7.0)))).
                    ForMember(d => d.day, o => o.ResolveUsing(s => s.code != "" ? LeftTime(Convert.ToInt32(s.quantity), Convert.ToString(s.expiryDate), Convert.ToString(DateTime.UtcNow.AddHours(7.0))) : ""));
                var promotionModel = Mapper.Map<List<PromotionCode>, List<PromotionCodeEntity>>(PromotionInfo);
                AutoMapper.Mapper.Reset();
                return promotionModel;
            }
            return null;
        }

        public string LeftTime(int qty, string expiryDate, string serverTime)
        {
            string returndata = string.Empty;
            if (qty == 0 && expiryDate != "")
            {
                TimeSpan ts = Convert.ToDateTime(expiryDate) - Convert.ToDateTime(serverTime);
                if (ts.Days == 0) { returndata = Convert.ToString(ts.Hours) + "h" + ":" + Convert.ToString(ts.Minutes) + "m"; }
                else if (ts.Days > 0 && ts.Minutes > 0) { returndata = Convert.ToString(ts.Days + 1); }
                else { returndata = Convert.ToString(ts.Days); }
            }
            else { returndata = Convert.ToString(qty); }
            return returndata;
        }

        public bool DeActivatePromotion(int promotionCodeId)
        {
            bool success = false;
            if (promotionCodeId != null)
            {
                using (var scope = new TransactionScope())
                {
                    var promotion = _unitOfWork.PromotionCodeRepository.GetByID(promotionCodeId);
                    if (promotion != null)
                    {
                        promotion.isActive = false;
                        _unitOfWork.PromotionCodeRepository.Update(promotion);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public IEnumerable<PromotionCodeEntity> GetPromotionByVendorId(int ID)
        {
            List<PromotionCodeEntity> promotionsCodeEntity = new List<PromotionCodeEntity>();
            var PromotionInfo = _unitOfWork.AdminManageActivePromotionRepository.GetByCondition(d => d.vendorId == ID && d.isDeleted == false).OrderByDescending(d => d.isActive).ToList();
            if (PromotionInfo != null)
            {
                foreach (var Item in PromotionInfo)
                {
                    var subCategory = _unitOfWork.SubCategoryRepository.GetByID(Item.subCategoryId);
                    var optionalCategory = _unitOfWork.OptionalCategoryRepository.GetByID(Item.optCategoryId);
                    var vendorImg = _unitOfWork.VendorRepository.GetByID(Item.vendorId);
                    DateTime UTCTime = System.DateTime.UtcNow;
                    DateTime dates2 = UTCTime.AddHours(7.0);
                    promotionsCodeEntity.Add(new PromotionCodeEntity()
                    {
                        promotionCodeId = Item.promotionCodeId,
                        code = Item.code,
                        name = Item.name,
                        descriptionEnglish = Item.descriptionEnglish,
                        descriptionThai = Item.descriptionThai,
                        createdDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.createdDate),
                        expiryDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.expiryDate),
                        isActive = Item.isActive,
                        vendorId = Item.vendorId,
                        quantity = Item.quantity != null ? Item.quantity : 0,
                        recommendation = Item.recommendation,
                        isPinned = Item.isPinned,
                        categoryId = Item.categoryId,
                        shareFacebookCount = Item.shareFacebookCount,
                        shareInstagramCount = Item.shareInstagramCount,
                        shareTwitterCount = Item.shareTwitterCount,
                        price = Item.price,
                        useCount = Item.useCount,
                        viewCount = Item.viewCount,
                        recommendationCount = Item.recommendationCount,
                        promotionImage = Item.promotionImage,
                        subCategoryId = Item.subCategoryId,
                        optCategoryId = Item.optCategoryId,
                        subCategoryname = subCategory != null ? subCategory.subCategoryName : "",
                        optionalCategoryName = optionalCategory != null ? optionalCategory.optCategoryName : "",
                        day = LeftTime(Convert.ToInt32(Item.quantity), Convert.ToString(Item.expiryDate), Convert.ToString(dates2)),
                        logoImg = vendorImg.logoImg == null ? "" : vendorImg.logoImg,
                        serverTime = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2)

                    });
                }
                return promotionsCodeEntity;
            }
            return promotionsCodeEntity;
        }

        public IEnumerable<PromotionCodeEntity> GetPromotionByPromotionCodeId(int Id)
        {
            List<PromotionCodeEntity> promotionsCodeEntity = new List<PromotionCodeEntity>();
            var PromotionInfo = _unitOfWork.AdminManageActivePromotionRepository.GetByCondition(d => d.promotionCodeId == Id).ToList();
            if (PromotionInfo != null)
            {
                foreach (var Item in PromotionInfo)
                {
                    var subCategory = _unitOfWork.SubCategoryRepository.GetByID(Item.subCategoryId);
                    var optionalCategory = _unitOfWork.OptionalCategoryRepository.GetByID(Item.optCategoryId);
                    DateTime UTCTime = System.DateTime.UtcNow;
                    DateTime dates2 = UTCTime.AddHours(7.0);
                    promotionsCodeEntity.Add(new PromotionCodeEntity()
                    {
                        promotionCodeId = Item.promotionCodeId,
                        code = Item.code,
                        name = Item.name,
                        descriptionEnglish = Item.descriptionEnglish,
                        descriptionThai = Item.descriptionThai,
                        createdDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.createdDate),
                        expiryDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.expiryDate),
                        isActive = Item.isActive,
                        vendorId = Item.vendorId,
                        quantity = Item.quantity != null ? Item.quantity : 0,
                        recommendation = Item.recommendation,
                        isPinned = Item.isPinned,
                        categoryId = Item.categoryId,
                        shareFacebookCount = Item.shareFacebookCount,
                        shareInstagramCount = Item.shareInstagramCount,
                        shareTwitterCount = Item.shareTwitterCount,
                        price = Item.price,
                        useCount = Item.useCount,
                        viewCount = Item.viewCount,
                        recommendationCount = Item.recommendationCount,
                        promotionImage = Item.promotionImage,
                        subCategoryId = Item.subCategoryId,
                        optCategoryId = Item.optCategoryId,
                        subCategoryname = subCategory != null ? subCategory.subCategoryName : "",
                        optionalCategoryName = optionalCategory != null ? optionalCategory.optCategoryName : "",
                        serverTime = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2),
                        createdDate1 = String.Format("{0:dd/MM/yyyy HH:mm:ss}", Item.createdDate),
                        expiryDate1 = String.Format("{0:dd/MM/yyyy HH:mm:ss}", Item.expiryDate),
                        serverTime1 = String.Format("{0:dd/MM/yyyy HH:mm:ss}", dates2)

                    });
                }
                return promotionsCodeEntity;
            }
            return promotionsCodeEntity;
        }

        /// <summary>
        /// /////////////////////////////  Get Promotion Details  //////////////////////

        public int CreateVendorPromotion(PromotionCodeEntity VendorPromotion)
        {
            DateTime? dt = DateTime.Now;
            if (!string.IsNullOrEmpty(VendorPromotion.expiryDate))
                dt = Convert.ToDateTime(VendorPromotion.expiryDate);
            using (var scope = new TransactionScope())
            {
                var promotionCode = new PromotionCode
                {
                    code = VendorPromotion.code != null ? VendorPromotion.code : "",
                    name = VendorPromotion.name != null ? VendorPromotion.name : "",
                    descriptionEnglish = VendorPromotion.descriptionEnglish != null ? VendorPromotion.descriptionEnglish : "",
                    descriptionThai = VendorPromotion.descriptionThai != null ? VendorPromotion.descriptionThai : "",
                    promotionCreatedDate = System.DateTime.Now,
                    createdDate = Convert.ToDateTime(VendorPromotion.createdDate),
                    expiryDate = string.IsNullOrEmpty(VendorPromotion.expiryDate) ? null : dt,
                    isActive = true,
                    isRelaunch = 0,
                    vendorId = VendorPromotion.vendorId,
                    quantity = VendorPromotion.quantity > 0 ? VendorPromotion.quantity : 0,
                    recommendation = VendorPromotion.recommendation != 0 ? VendorPromotion.recommendation : 0,
                    isPinned = false,
                    categoryId = VendorPromotion.categoryId,
                    subCategoryId = VendorPromotion.subCategoryId,
                    optCategoryId = VendorPromotion.optCategoryId,
                    shareFacebookCount = VendorPromotion.shareFacebookCount != null ? VendorPromotion.shareFacebookCount : "0",
                    shareInstagramCount = VendorPromotion.shareInstagramCount != null ? VendorPromotion.shareInstagramCount : "0",
                    shareTwitterCount = VendorPromotion.shareTwitterCount != null ? VendorPromotion.shareTwitterCount : "0",
                    price = VendorPromotion.price != null ? VendorPromotion.price : 0,
                    useCount = VendorPromotion.useCount != null ? VendorPromotion.useCount : 0,
                    viewCount = VendorPromotion.viewCount != null ? VendorPromotion.viewCount : 0,
                    recommendationCount = VendorPromotion.recommendationCount != null ? VendorPromotion.recommendationCount : 0,
                    isDeleted = false
                };

                _unitOfWork.VendorPromotionRepository.Insert(promotionCode);
                _unitOfWork.Save();

                scope.Complete();
                using (var scopes = new TransactionScope())
                {
                    var promotionCodeId = _unitOfWork.VendorPromotionRepository.GetByID(promotionCode.promotionCodeId);
                    if (promotionCodeId != null)
                    {
                        promotionCode.promotionImage = base64ToImage(VendorPromotion.promotionImage, promotionCodeId.promotionCodeId, VendorPromotion.promotionImage);
                        _unitOfWork.VendorPromotionRepository.Update(promotionCode);
                        _unitOfWork.Save();
                        scopes.Complete();
                    }
                }
                return promotionCode.promotionCodeId;
            }

        }

        public string base64ToImage(string logImg, int MaxId, string oldImage)
        {
            try
            {
                string filePath = string.Empty;
                string image = string.Empty;
                var path = DateTime.Now;
                var data = String.Format("{0:d/M/yyyy HH:mm:ss}", path);
                data = data.Replace(@"/", "").Trim(); data = data.Replace(@":", "").Trim(); data = data.Replace(" ", String.Empty);
                if (!string.IsNullOrEmpty(logImg))
                {
                    filePath = HostingEnvironment.MapPath("~/Pictures/");
                    image = MaxId + "_PromotionCodepic" + data + ".png";
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
            catch (Exception ex)
            {
                EventLog.CreateEventSource("EddeeApp", "AppLogs");
                EventLog.WriteEntry("EddeeApp", ex.InnerException.Message, EventLogEntryType.Error);
                EventLog.WriteEntry("EddeeApp", ex.Message, EventLogEntryType.Error);
                return string.Empty;
            }

        }

        public bool UpdateVendorPromotion(int promotionCodeId, PromotionCodeEntity VendorPromotion)
        {
            DateTime? dt = DateTime.Now;
            if (!string.IsNullOrEmpty(VendorPromotion.expiryDate))
                dt = Convert.ToDateTime(VendorPromotion.expiryDate);
            var success = false;
            var promotion = _unitOfWork.VendorPromotionRepository.GetByID(VendorPromotion.promotionCodeId);

            if (promotion != null)
            {
                if (VendorPromotion.name == "Edit".Trim())
                {
                    using (var scope = new TransactionScope())
                    {
                        promotion.name = VendorPromotion.name != null ? VendorPromotion.name : "";
                        promotion.descriptionEnglish = VendorPromotion.descriptionEnglish != null ? VendorPromotion.descriptionEnglish : "";
                        promotion.descriptionThai = VendorPromotion.descriptionThai != null ? VendorPromotion.descriptionThai : "";
                        promotion.createdDate = Convert.ToDateTime(VendorPromotion.createdDate);
                        promotion.expiryDate = string.IsNullOrEmpty(VendorPromotion.expiryDate) ? null : dt;
                        promotion.modifyDate = System.DateTime.Now;
                        promotion.isActive = VendorPromotion.isActive;
                        promotion.quantity = VendorPromotion.quantity != null ? VendorPromotion.quantity : 0;
                        promotion.isPinned = VendorPromotion.isPinned;
                        promotion.categoryId = VendorPromotion.categoryId;
                        promotion.subCategoryId = VendorPromotion.subCategoryId;
                        promotion.optCategoryId = VendorPromotion.optCategoryId;
                        promotion.price = VendorPromotion.price != 0 ? VendorPromotion.price : 0;
                        _unitOfWork.VendorPromotionRepository.Update(promotion);
                        _unitOfWork.Save();
                        scope.Complete();

                        var promotionImage = _unitOfWork.VendorPromotionRepository.GetByID(VendorPromotion.promotionCodeId);
                        if (VendorPromotion.promotionImage != null && VendorPromotion.promotionImage != "")
                        {
                            using (var scopes = new TransactionScope())
                            {
                                promotionImage.promotionImage = base64ToImage(VendorPromotion.promotionImage, VendorPromotion.promotionCodeId, promotionImage.promotionImage);
                                _unitOfWork.VendorPromotionRepository.Update(promotionImage);
                                _unitOfWork.Save();
                                scopes.Complete();
                            }
                        }

                        success = true;
                    }
                }
                else
                {
                    using (var scope = new TransactionScope())
                    {
                        promotion.name = VendorPromotion.name != null ? VendorPromotion.name : "";
                        promotion.descriptionEnglish = VendorPromotion.descriptionEnglish != null ? VendorPromotion.descriptionEnglish : "";
                        promotion.descriptionThai = VendorPromotion.descriptionThai != null ? VendorPromotion.descriptionThai : "";
                        promotion.expiryDate = string.IsNullOrEmpty(VendorPromotion.expiryDate) ? null : dt;
                        promotion.modifyDate = System.DateTime.Now;
                        promotion.isActive = true;
                        promotion.isRelaunch++;
                        promotion.lastRelaunchDate = System.DateTime.Now;
                        promotion.quantity = VendorPromotion.quantity != null ? VendorPromotion.quantity : 0;
                        promotion.price = VendorPromotion.price != 0 ? VendorPromotion.price : 0;
                        _unitOfWork.VendorPromotionRepository.Update(promotion);
                        _unitOfWork.Save();
                        scope.Complete();

                        var promotionImage = _unitOfWork.VendorPromotionRepository.GetByID(VendorPromotion.promotionCodeId);
                        if (VendorPromotion.promotionImage != null && VendorPromotion.promotionImage != "")
                        {
                            using (var scopes = new TransactionScope())
                            {
                                promotionImage.promotionImage = base64ToImage(VendorPromotion.promotionImage, VendorPromotion.promotionCodeId, promotionImage.promotionImage);
                                _unitOfWork.VendorPromotionRepository.Update(promotionImage);
                                _unitOfWork.Save();
                                scopes.Complete();
                            }
                        }

                        success = true;
                    }
                }
            }
            return success;
        }



        public bool DeleteVendorPromotion(int promotionCodeId, PromotionCodeEntity PromotionCode)
        {
            var success = false;
            if (PromotionCode != null)
            {
                using (var scope = new TransactionScope())
                {
                    var Id = _unitOfWork.VendorPromotionRepository.GetByID(promotionCodeId);
                    if (Id != null)
                    {
                        Id.isDeleted = true;
                        _unitOfWork.VendorPromotionRepository.Update(Id);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public bool IsVendorHavingActivePromotion(int vendorId, string oAuthkey)
        {
            var success = false;

            var promotion = _unitOfWork.PromotionCodeRepository.GetByCondition(p => p.vendorId == vendorId && p.isActive == true && p.isDeleted == false).ToList();
            if (promotion.Count() > 0)
            {
                success = true;
            }

            return success;
        }

        public bool PinVendorPromotion(int promotionCodeId, PromotionCodeEntity PromotionCode)
        {
            var success = false;
            if (PromotionCode != null)
            {
                using (var scope = new TransactionScope())
                {
                    var promotion = _unitOfWork.PromotionCodeRepository.GetByID(promotionCodeId);
                    if (promotion != null)
                    {
                        if (promotion.isPinned == false)
                        {
                            promotion.isPinned = true;
                            promotion.PinnedDate = DateTime.Now;
                            success = true;

                        }
                        else
                        {
                            promotion.isPinned = false;
                            promotion.PinnedDate = null;
                            success = false;
                        }


                        _unitOfWork.PromotionCodeRepository.Update(promotion);
                        _unitOfWork.Save();
                        scope.Complete();
                    }
                }
            }
            return success;
        }

    }
}
