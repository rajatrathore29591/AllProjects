using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.CraveASAP.BusinessEntities;
using TV.CraveASAP.BusinessServices.Interfaces;
using TV.CraveASAP.DataModel.UnitOfWork;
using TV.CraveASAP.DataModel;
using System.Transactions;
using AutoMapper;
using System.Device.Location;
using TV.CraveASAP.BusinessServices.HelperClass;
using System.Threading;
using System.Reflection;
using System.Resources;
using System.Globalization;

namespace TV.CraveASAP.BusinessServices
{
    public class UserPromotionsServices : IUserPromotionsServices
    {
        private readonly UnitOfWork _unitOfWork;

        public UserPromotionsServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        #region Deals Method
        public IEnumerable<PromotionsUserEntity> GetPromotionCode(UserPromotionEntity userPromotion)
        {
            bool isMore = true;
            if (userPromotion.userId != 0)
            {
                var promotion = _unitOfWork.UserPromotionRepository.GetByCondition(d => d.userId == userPromotion.userId && d.promotionId == userPromotion.promotionId && d.isDeleted == false).FirstOrDefault();

                if (promotion == null)
                {
                    using (var scope = new TransactionScope())
                    {
                        var _UserPromotion = new UserPromotion
                        {
                            userId = userPromotion.userId,
                            promotionId = userPromotion.promotionId,
                            isRecommended = userPromotion.isRecommended,
                            isShareFacebook = userPromotion.isShareFacebook,
                            isShareTwitter = userPromotion.isShareTwitter,
                            isShareInstagram = userPromotion.isShareInstagram,
                            isViewed = userPromotion.isViewed,
                            isUsed = userPromotion.isUsed,
                            isDeleted = false,
                            userTime = GetLandingDetailApp()
                        };
                        _unitOfWork.UserPromotionRepository.Insert(_UserPromotion);
                        _unitOfWork.Save();
                        scope.Complete();
                    }
                }
            }

            List<PromotionsUserEntity> promotionsUserEntity = new List<PromotionsUserEntity>();
            if (userPromotion.userId > 0)
            {
                var vicinityVendor = _unitOfWork.VendorRepository.GetByCondition(d => d.isDeleted == false);
                var promotionCode = _unitOfWork.PromotionCodeRepository.GetByCondition(p => p.isActive == true && p.promotionCodeId == userPromotion.promotionId && p.isDeleted == false).Join(vicinityVendor, p => p.vendorId, v => v.vendorId, (p, v) => new { p, v }).ToList();

                foreach (var Item in promotionCode)
                {
                    DateTime UTCTime = System.DateTime.UtcNow;
                    DateTime dates2 = UTCTime.AddHours(7.0);
                    var favourite = _unitOfWork.UserFavoriteRepository.GetByCondition(f => f.userId == userPromotion.userId && f.vendorId == Item.p.vendorId).FirstOrDefault();
                    promotionsUserEntity.Add(new PromotionsUserEntity()
                    {
                        promotionCodeId = Item.p.promotionCodeId,
                        code = Item.p.code,
                        name = Item.p.name,
                        descriptionEnglish = Item.p.descriptionEnglish,
                        descriptionThai = Item.p.descriptionThai,
                        createdDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.p.createdDate),
                        expiryDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.p.expiryDate),
                        isActive = Item.p.isActive,
                        vendorId = Item.p.vendorId,
                        quantity = Item.p.quantity != null ? Item.p.quantity : 0,
                        recommendation = Item.p.recommendation,
                        isPinned = Item.p.isPinned,
                        categoryId = Item.p.categoryId,
                        shareFacebookCount = Item.p.shareFacebookCount,
                        shareInstagramCount = Item.p.shareInstagramCount,
                        shareTwitterCount = Item.p.shareTwitterCount,
                        price = Item.p.price,
                        useCount = Item.p.useCount,
                        viewCount = Item.p.viewCount,
                        recommendationCount = Item.p.recommendationCount,
                        promotionImage = Item.p.promotionImage,
                        companyName = Item.v.companyName,
                        fullDescription = Item.v.fullDescription == null ? "" : Item.v.fullDescription,
                        shortDescription = Item.v.shortDescription == null ? "" : Item.v.shortDescription,
                        latitude = Item.v.latitude,
                        longitude = Item.v.longitude,
                        logoimg = Item.v.logoImg,
                        fromday = fromday(Item.v.vendorId),
                        today = today(Item.v.vendorId),
                        fromtime = fromtime(Item.v.vendorId),
                        totime = totime(Item.v.vendorId),
                        isMore = isMore,
                        isFavourite = favourite == null ? false : favourite.isActive,
                        favouriteId = favourite == null ? "" : favourite.favoriteRestaurantId.ToString(),
                        serverTime = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2)
                    });
                }
            }

            else if (userPromotion.userId == 0)
            {
                var vicinityVendor = _unitOfWork.VendorRepository.GetByCondition(d => d.isDeleted == false);
                var promotionCode = _unitOfWork.PromotionCodeRepository.GetByCondition(p => p.isActive == true && p.isDeleted == false && p.promotionCodeId == userPromotion.promotionId).Join(vicinityVendor, p => p.vendorId, v => v.vendorId, (p, v) => new { p, v }).ToList();

                foreach (var Item in promotionCode)
                {
                    DateTime UTCTime = System.DateTime.UtcNow;
                    DateTime dates2 = UTCTime.AddHours(7.0);
                    var favourite = _unitOfWork.UserFavoriteRepository.GetByCondition(f => f.userId == userPromotion.userId && f.vendorId == Item.p.vendorId).FirstOrDefault();
                    promotionsUserEntity.Add(new PromotionsUserEntity()
                    {
                        promotionCodeId = Item.p.promotionCodeId,
                        code = Item.p.code,
                        name = Item.p.name,
                        descriptionEnglish = Item.p.descriptionEnglish,
                        descriptionThai = Item.p.descriptionThai,
                        createdDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.p.createdDate),
                        expiryDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.p.expiryDate),
                        isActive = Item.p.isActive,
                        vendorId = Item.p.vendorId,
                        quantity = Item.p.quantity != null ? Item.p.quantity : 0,
                        recommendation = Item.p.recommendation,
                        isPinned = Item.p.isPinned,
                        categoryId = Item.p.categoryId,
                        shareFacebookCount = Item.p.shareFacebookCount,
                        shareInstagramCount = Item.p.shareInstagramCount,
                        shareTwitterCount = Item.p.shareTwitterCount,
                        price = Item.p.price,
                        useCount = Item.p.useCount,
                        viewCount = Item.p.viewCount,
                        recommendationCount = Item.p.recommendationCount,
                        promotionImage = Item.p.promotionImage,
                        companyName = Item.v.companyName,
                        fullDescription = Item.v.fullDescription,
                        latitude = Item.v.latitude,
                        longitude = Item.v.longitude,
                        logoimg = Item.v.logoImg,
                        fromday = fromday(Item.v.vendorId),
                        today = today(Item.v.vendorId),
                        fromtime = fromtime(Item.v.vendorId),
                        totime = totime(Item.v.vendorId),
                        isMore = isMore,
                        isFavourite = favourite == null ? false : favourite.isActive,
                        favouriteId = favourite == null ? "" : favourite.favoriteRestaurantId.ToString(),
                        serverTime = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2)
                    });
                }
            }
            else
            {
                var promotionWithoutLocation = _unitOfWork.PromotionCodeRepository.GetByCondition(d => d.isDeleted == false).Take(3).ToList();
                if (promotionWithoutLocation.Count() > 0)
                {
                    foreach (var Item in promotionWithoutLocation)
                    {
                        DateTime UTCTime = System.DateTime.UtcNow;
                        DateTime dates2 = UTCTime.AddHours(7.0);
                        var favourite = _unitOfWork.UserFavoriteRepository.GetByCondition(f => f.userId == userPromotion.userId && f.vendorId == Item.vendorId).FirstOrDefault();
                        var vendor = _unitOfWork.VendorRepository.GetByCondition(v => v.vendorId == Item.vendorId).FirstOrDefault();

                        promotionsUserEntity.Add(new PromotionsUserEntity()
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
                            companyName = vendor.companyName,
                            fullDescription = vendor.fullDescription,
                            latitude = vendor.latitude,
                            longitude = vendor.longitude,
                            logoimg = vendor.logoImg,
                            fromday = fromday(vendor.vendorId),
                            today = today(vendor.vendorId),
                            fromtime = fromtime(vendor.vendorId),
                            totime = totime(vendor.vendorId),
                            isMore = isMore,
                            isFavourite = favourite == null ? false : favourite.isActive,
                            favouriteId = favourite == null ? "" : favourite.favoriteRestaurantId.ToString(),
                            serverTime = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2)
                        });
                    }
                }
            }

            return promotionsUserEntity;
        }

        public IEnumerable<PromotionCodeEntity> GetOpenPromotions(int userId, string lang)
        {
            var userPromotionCode = _unitOfWork.UserPromotionRepository.GetByCondition(o => o.userId == userId && o.isDeleted == false).ToList();
            var promotionCode = _unitOfWork.PromotionCodeRepository.GetByCondition(p => p.isActive == true && p.isDeleted == false).Join(userPromotionCode, p => p.promotionCodeId, u => u.promotionId, (p, u) => new { p }).Select(p => p.p).ToList();


            if (promotionCode.Any())
            {
                Mapper.CreateMap<PromotionCode, PromotionCodeEntity>();
                var userPromotionModel = Mapper.Map<List<PromotionCode>, List<PromotionCodeEntity>>(promotionCode);
                return userPromotionModel;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vendorType"></param>
        /// <param name="userLocationLat"></param>
        /// <param name="userLocationLong"></param>
        /// <param name="id"></param>
        /// <returns>list of promotions of 2.5km from the current location of user with the user prefrence
        /// and if no promotion corresponding to user prefernce then we return GetAllDeal data</returns>

        public IEnumerable<PromotionsUserEntity> GetICrave(int vendorType, string userLocationLat, string userLocationLong, int id, int page)
        {
            bool crave = true;
            DateTime UTCTime1 = System.DateTime.UtcNow;
            DateTime dates3 = UTCTime1.AddHours(7.0);
            if (page == 1)
            {
                crave = false;
            }
            List<PromotionsUserEntity> promotionsUserEntity = new List<PromotionsUserEntity>();
            var vicinityVendor = onLocationChanged(Convert.ToDouble(userLocationLat), Convert.ToDouble(userLocationLong), 1);
            bool isMore = true;
            var promotionCode = _unitOfWork.PromotionCodeRepository.GetByCondition(p => p.isActive == true && p.categoryId == vendorType && p.isDeleted == false).Join(vicinityVendor, p => p.vendorId, q => q.vendorId, (p, q) => new { p, q }).ToList();
           // var promotionCode = _unitOfWork.PromotionCodeRepository.GetByCondition(p => p.isActive == true && p.isDeleted == false).Join(vicinityVendor, p => p.vendorId, v => v.vendorId, (p, v) => new { p, v }).ToList();
            var qtyPromo = promotionCode.Where(z => z.p.expiryDate == null).ToList(); var datePromo = promotionCode.Where(z => z.p.expiryDate != null && z.p.expiryDate >= dates3).ToList();
            promotionCode = null; promotionCode = qtyPromo; promotionCode.AddRange(datePromo);
            promotionCode = promotionCode.OrderBy(o => o.q.Distance).ThenBy(o => o.q.Distance).ToList();
            int itemCount = promotionCode.Count(); itemCount = itemCount % 3;
            var userSub = _unitOfWork.UserPrefrenceRepository.GetByCondition(up => up.isActive.Equals(true) && up.userId == id && up.categoryId == vendorType && up.type == "sub").Select(s => s.prefrencesId).ToList();
            var userOpt = _unitOfWork.UserPrefrenceRepository.GetByCondition(up => up.isActive.Equals(true) && up.userId == id && up.categoryId == vendorType && up.type == "opt").Select(s => Convert.ToInt32(s.prefrencesId)).ToList();
            if (userSub.Count() > 0)
            {
                var temp = promotionCode.Where(c => userSub.Contains(c.p.subCategoryId)).ToList();
                //crave = true;
                if (temp.Count() == 0)
                {
                    //temp.AddRange(promotionCode.Where(c => userSub.Contains(c.p.subCategoryId)).ToList());
                    //promotionCode = temp.GroupBy(x => x.p.promotionCodeId).Select(y => y.First()).ToList();
                    var temp1 = promotionCode.Where(c => userOpt.Contains(c.p.optCategoryId)).ToList();
                    if (temp1.Count() > 0)
                    {
                        temp = promotionCode;
                        temp.AddRange(promotionCode.Where(c => userOpt.Contains(c.p.optCategoryId)).ToList());
                        promotionCode = temp.GroupBy(x => x.p.promotionCodeId).Select(y => y.First()).ToList();
                        crave = true;
                    }
                    else
                    {
                        crave = false;
                    }
                }
                else
                {
                    temp.AddRange(promotionCode.Where(c => userOpt.Contains(c.p.optCategoryId)).ToList());
                    promotionCode = temp.GroupBy(x => x.p.promotionCodeId).Select(y => y.First()).ToList();
                    crave = true;

                }

            }
            else if (userOpt.Count() > 0)
            {
                promotionCode = promotionCode.Where(c => userOpt.Contains(c.p.optCategoryId)).ToList();
                if (promotionCode.Count() > 0)
                {
                    crave = true;
                }
                else
                {
                    crave = false;
                }
                // crave = true;
            }

            var favourite = _unitOfWork.UserFavoriteRepository.GetByCondition(d => d.userId == id && d.isActive == true).Join(promotionCode, f => f.vendorId, v => v.p.vendorId, (f, v) => new { f, v }).ToList();
            if (favourite.Count() > 0)
            {
                if (page > 0)
                {

                    int skip = 0; skip = (page - 1) * 3;
                    isMore = (favourite.Skip(skip).Count() > 3) ? true : false;
                    favourite = favourite.Skip(skip).Take(3).ToList();
                    crave = true;
                }
            }
            else
            {
                if (promotionCode.Count > 0)
                {
                    if (page > 0)
                    {
                        int skip = 0; skip = (page - 1) * 3;
                        isMore = (promotionCode.Skip(skip).Count() > 3) ? true : false;
                        promotionCode = promotionCode.Skip(skip).Take(3).ToList();
                    }
                }

            }
            if (favourite.Count() == 0)
            {
                if (promotionCode != null && promotionCode.Count > 0)
                {
                    foreach (var Item in promotionCode)
                    {
                        DateTime UTCTime = System.DateTime.UtcNow;
                        DateTime dates2 = UTCTime.AddHours(7.0);
                        var GetUserfav = _unitOfWork.UserFavoriteRepository.GetByCondition(u => u.userId == id && u.vendorId == Item.p.vendorId).FirstOrDefault();
                        promotionsUserEntity.Add(new PromotionsUserEntity()
                        {
                            promotionCodeId = Item.p.promotionCodeId,
                            code = Item.p.code,
                            name = Item.p.name,
                            descriptionEnglish = Item.p.descriptionEnglish,
                            descriptionThai = Item.p.descriptionThai,
                            createdDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.p.createdDate),
                            expiryDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.p.expiryDate),
                            isActive = Item.p.isActive,
                            vendorId = Item.p.vendorId,
                            quantity = Item.p.quantity,
                            recommendation = Item.p.recommendation,
                            isPinned = Item.p.isPinned,
                            categoryId = Item.p.categoryId,
                            shareFacebookCount = Item.p.shareFacebookCount,
                            shareInstagramCount = Item.p.shareInstagramCount,
                            shareTwitterCount = Item.p.shareTwitterCount,
                            price = Item.p.price,
                            useCount = Item.p.useCount,
                            viewCount = Item.p.viewCount,
                            recommendationCount = Item.p.recommendationCount,
                            promotionImage = Item.p.promotionImage,
                            companyName = Item.q.companyName,
                            fullDescription = Item.q.fullDescription == null ? "" : Item.q.fullDescription,
                            shortDescription = Item.q.shortDescription == null ? "" : Item.q.shortDescription,
                            latitude = Item.q.latitude,
                            longitude = Item.q.longitude,
                            logoimg = Item.q.logoImg,
                            fromday = fromday(Item.q.vendorId),
                            today = today(Item.q.vendorId),
                            fromtime = fromtime(Item.q.vendorId),
                            totime = totime(Item.q.vendorId),
                            isFavourite = GetUserfav != null ? GetUserfav.isActive : false,
                            favouriteId = "",
                            serverTime = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2),
                            isMore = isMore,
                            optCategoryId = Item.p.optCategoryId,
                            subCategoryId = Item.p.subCategoryId,
                            isIcrave = crave,
                            expiryDate1 = String.Format("{0:dd/MM/yyyy HH:mm:ss}", Item.p.expiryDate),
                            serverTime1 = String.Format("{0:dd/MM/yyyy HH:mm:ss}", dates2)

                        });
                    }
                }
            }
            else
            {
                foreach (var Item in favourite)
                {
                    DateTime UTCTime = System.DateTime.UtcNow;
                    DateTime dates2 = UTCTime.AddHours(7.0);
                    var GetUserfav = _unitOfWork.UserFavoriteRepository.GetByCondition(u => u.userId == id && u.vendorId == Item.f.vendorId).FirstOrDefault();
                    promotionsUserEntity.Add(new PromotionsUserEntity()
                    {
                        promotionCodeId = Item.v.p.promotionCodeId,
                        code = Item.v.p.code,
                        name = Item.v.p.name,
                        descriptionEnglish = Item.v.p.descriptionEnglish,
                        descriptionThai = Item.v.p.descriptionThai,
                        createdDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.v.p.createdDate),
                        expiryDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.v.p.expiryDate),
                        isActive = Item.v.p.isActive,
                        vendorId = Item.v.p.vendorId,
                        quantity = Item.v.p.quantity,
                        recommendation = Item.v.p.recommendation,
                        isPinned = Item.v.p.isPinned,
                        categoryId = Item.v.p.categoryId,
                        shareFacebookCount = Item.v.p.shareFacebookCount,
                        shareInstagramCount = Item.v.p.shareInstagramCount,
                        shareTwitterCount = Item.v.p.shareTwitterCount,
                        price = Item.v.p.price,
                        useCount = Item.v.p.useCount,
                        viewCount = Item.v.p.viewCount,
                        recommendationCount = Item.v.p.recommendationCount,
                        promotionImage = Item.v.p.promotionImage,
                        companyName = Item.v.q.companyName,
                        fullDescription = Item.v.q.fullDescription == null ? "" : Item.v.q.fullDescription,
                        shortDescription = Item.v.q.shortDescription == null ? "" : Item.v.q.shortDescription,
                        latitude = Item.v.q.latitude,
                        longitude = Item.v.q.longitude,
                        logoimg = Item.v.q.logoImg,
                        fromday = fromday(Item.v.q.vendorId),
                        today = today(Item.v.q.vendorId),
                        fromtime = fromtime(Item.v.q.vendorId),
                        totime = totime(Item.v.q.vendorId),
                        isFavourite = GetUserfav != null ? GetUserfav.isActive : false,
                        favouriteId = "",
                        serverTime = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2),
                        isMore = isMore,
                        optCategoryId = Item.v.p.optCategoryId,
                        subCategoryId = Item.v.p.subCategoryId,
                        isIcrave = crave,
                        expiryDate1 = String.Format("{0:dd/MM/yyyy HH:mm:ss}", Item.v.p.expiryDate),
                        serverTime1 = String.Format("{0:dd/MM/yyyy HH:mm:ss}", dates2)
                    });
                }
            }
            return promotionsUserEntity;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vendorType"></param>
        /// <param name="page"></param>
        /// <param name="userLocationLat"></param>
        /// <param name="userLocationLong"></param>
        /// <param name="langType"></param>
        /// <param name="resType"></param>
        /// <param name="userId"></param>
        /// <returns>Promotion list which are in between 2.5 km from the current location of user</returns>
        public IEnumerable<PromotionsUserEntity> GetAllDeals(int vendorType, int page, string userLocationLat, string userLocationLong, string langType, string resType, string userId)
        {
            bool isMore = true;
            DateTime UTCTime1 = System.DateTime.UtcNow;
            DateTime dates3 = UTCTime1.AddHours(7.0);
            var getUserPromotion = _unitOfWork.UserPromotionRepository.GetByCondition(u => u.userId == Convert.ToInt32(userId) && u.isDeleted == false).Select(p => new { p.isRecommended, p.promotionId }).ToList();
          
            List<PromotionsUserEntity> promotionsUserEntity = new List<PromotionsUserEntity>();
            var vicinityVendor = onLocationChanged(Convert.ToDouble(userLocationLat), Convert.ToDouble(userLocationLong), 1);
            if (vicinityVendor != null)
            {

               // var promotionCode = _unitOfWork.PromotionCodeRepository.GetByCondition(p => p.isActive == true && p.isDeleted == false && p.expiryDate >= dates3).Join(vicinityVendor, p => p.vendorId, v => v.vendorId, (p, v) => new { p, v }).ToList();
                var promotionCode = _unitOfWork.PromotionCodeRepository.GetByCondition(p => p.isActive == true && p.isDeleted == false).Join(vicinityVendor, p => p.vendorId, v => v.vendorId, (p, v) => new { p, v }).ToList();
                var qtyPromo = promotionCode.Where(z => z.p.expiryDate == null).ToList(); var datePromo = promotionCode.Where(z => z.p.expiryDate != null && z.p.expiryDate >= dates3).ToList();
                promotionCode = null; promotionCode = qtyPromo; promotionCode.AddRange(datePromo);
                if (vendorType > 0)
                {
                    promotionCode = promotionCode.Where(z => z.p.categoryId == vendorType).ToList();
                }
                promotionCode = promotionCode.OrderBy(o => o.v.Distance).ThenBy(o => o.v.Distance).ToList(); int itemCount = promotionCode.Count(); itemCount = itemCount % 3;
                if (promotionCode.Count > 0)
                {
                    var SelectRandoms = promotionCode;
                    SelectRandoms = null;
                    if (page > 0)
                    {
                        int Take = 3; int skip = 3;
                        if (page == 1)
                        {
                            isMore = (promotionCode.Count > 3) ? true : false;

                            if (promotionCode.Count > 0)
                            {
                                SelectRandoms = promotionCode.Take(1).ToList(); promotionCode.RemoveRange(0, 1);
                                promotionCode = promotionCode.OrderBy(o => o.p.price).ThenBy(o => o.p.price).ToList();
                            }
                            if (promotionCode.Count > 0)
                            {
                                SelectRandoms.AddRange(promotionCode.Take(1).ToList()); promotionCode.RemoveRange(0, 1);
                                promotionCode = promotionCode.OrderByDescending(o => o.p.recommendationCount).ThenBy(o => o.p.recommendationCount).ToList();
                            }
                            if (promotionCode.Count > 0)
                            {
                                SelectRandoms.AddRange(promotionCode.Take(1).ToList());
                            }
                            promotionCode = SelectRandoms;
                        }
                        else
                        {
                            if (promotionCode.Count > 0) { promotionCode.RemoveRange(0, 1); promotionCode = promotionCode.OrderBy(o => o.p.price).ThenBy(o => o.p.price).ToList(); }
                            if (promotionCode.Count > 0) { promotionCode.RemoveRange(0, 1); promotionCode = promotionCode.OrderByDescending(o => o.p.recommendationCount).ThenBy(o => o.p.recommendationCount).ToList(); promotionCode.RemoveRange(0, 1); }
                            isMore = ((promotionCode.Count - 3) > 0) ? true : false;

                            if (promotionCode.Count > 0)
                            {
                                promotionCode = promotionCode.OrderBy(o => o.p.createdDate).ThenBy(o => o.p.createdDate).ToList();
                                SelectRandoms = (promotionCode.Take(1).ToList()); promotionCode.RemoveRange(0, 1);
                            }
                            if (promotionCode.Count > 0)
                            {
                                promotionCode = promotionCode.OrderBy(o => o.p.quantity).ThenBy(o => o.p.quantity).ToList();
                                SelectRandoms.AddRange(promotionCode.Take(1).ToList()); promotionCode.RemoveRange(0, 1);
                            }
                            if (promotionCode.Count > 0)
                            {
                                promotionCode = promotionCode.OrderBy(o => o.v.Distance).ThenBy(o => o.v.Distance).ToList();
                                SelectRandoms.AddRange(promotionCode.Take(1).ToList()); promotionCode.RemoveRange(0, 1);

                            }
                            if (page == 2)
                            {
                                promotionCode = SelectRandoms;
                            }
                            else
                            {
                                if (page == 3)
                                {
                                    isMore = ((promotionCode.Count - 3) > 0) ? true : false;
                                    promotionCode = promotionCode.Take(3).ToList();
                                }
                                else
                                {
                                    skip = skip * (page - 3);
                                    isMore = ((promotionCode.Count - skip) > 3) ? true : false;
                                    promotionCode = promotionCode.Skip(skip).Take(3).ToList();
                                }
                            }
                        }
                    }
                }

                foreach (var Item in promotionCode)
                {
                    DateTime UTCTime = System.DateTime.UtcNow;
                    DateTime dates2 = UTCTime.AddHours(7.0);
                    promotionsUserEntity.Add(new PromotionsUserEntity()
                    {
                        promotionCodeId = Item.p.promotionCodeId,
                        code = Item.p.code,
                        name = Item.p.name,
                        descriptionEnglish = Item.p.descriptionEnglish,
                        descriptionThai = Item.p.descriptionThai,
                        createdDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.p.createdDate),
                        expiryDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.p.expiryDate),
                        isActive = Item.p.isActive,
                        vendorId = Item.p.vendorId,
                        quantity = Item.p.quantity != null ? Item.p.quantity : 0,
                        recommendation = Item.p.recommendation,
                        isPinned = Item.p.isPinned,
                        categoryId = Item.p.categoryId,
                        shareFacebookCount = Item.p.shareFacebookCount,
                        shareInstagramCount = Item.p.shareInstagramCount,
                        shareTwitterCount = Item.p.shareTwitterCount,
                        price = Item.p.price,
                        useCount = Item.p.useCount,
                        viewCount = Item.p.viewCount,
                        recommendationCount = Item.p.recommendationCount,
                        promotionImage = Item.p.promotionImage,
                        companyName = Item.v.companyName,
                        fullDescription = Item.v.fullDescription == null ? "" : Item.v.fullDescription,
                        shortDescription = Item.v.shortDescription == null ? "" : Item.v.shortDescription,
                        latitude = Item.v.latitude,
                        longitude = Item.v.longitude,
                        logoimg = Item.v.logoImg,
                        fromday = fromday(Item.v.vendorId),
                        today = today(Item.v.vendorId),
                        fromtime = fromtime(Item.v.vendorId),
                        totime = totime(Item.v.vendorId),
                        isRecommended = getUserPromotion != null ? (getUserPromotion.Where(p => p.promotionId == Item.p.promotionCodeId && p.isRecommended == true).Count() > 0 ? true : false) : false,
                        isMore = isMore,
                        serverTime = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2),
                        expiryDate1 = String.Format("{0:dd/MM/yyyy HH:mm:ss}", Item.p.expiryDate),
                        serverTime1 = String.Format("{0:dd/MM/yyyy HH:mm:ss}", dates2)

                    });
                }
                return promotionsUserEntity;
            }
            return promotionsUserEntity;
        }

        public bool isRecommended(int userId, int pId)
        {
            bool success = false;
            var getUserPromotion = _unitOfWork.UserPromotionRepository.GetByCondition(u => u.userId == userId && u.promotionId == pId && u.isDeleted == false).Select(p => p.isRecommended).FirstOrDefault();
            if (getUserPromotion == true) { success = true; } else { success = false; }
            return success;

        }

        public string fromday(int id)
        {
            int i = 0;
            string day = string.Empty;
            var vendorOpeningHours = _unitOfWork.OpeningHourRepository.GetByCondition(x => x.fkvendorId == id).ToList();
            if (vendorOpeningHours != null)
            {
                foreach (var item in vendorOpeningHours)
                {
                    if (i == 0)
                    {
                        day += item.fromday.ToString();
                    }
                    else { day += "," + item.fromday.ToString(); }

                    i++;
                }
            }
            return day;
        }

        public string today(int id)
        {
            int i = 0;
            string day = string.Empty;
            var vendorOpeningHours = _unitOfWork.OpeningHourRepository.GetByCondition(x => x.fkvendorId == id).ToList();
            if (vendorOpeningHours != null)
            {
                foreach (var item in vendorOpeningHours)
                {
                    if (i == 0)
                    {
                        day += item.today.ToString();
                    }
                    else { day += "," + item.today.ToString(); }

                    i++;
                }
            }
            return day;
        }

        public string totime(int id)
        {
            int i = 0;
            string totime = string.Empty;
            var vendorOpeningHours = _unitOfWork.OpeningHourRepository.GetByCondition(x => x.fkvendorId == id).ToList();
            if (vendorOpeningHours != null)
            {
                foreach (var item in vendorOpeningHours)
                {
                    if (i == 0)
                    {
                        totime += item.totime.ToString();
                    }
                    else { totime += "," + item.totime.ToString(); }

                    i++;
                }
            }
            return totime;
        }

        public string fromtime(int id)
        {
            int i = 0;
            string fromtime = string.Empty;
            var vendorOpeningHours = _unitOfWork.OpeningHourRepository.GetByCondition(x => x.fkvendorId == id).ToList();
            if (vendorOpeningHours != null)
            {
                foreach (var item in vendorOpeningHours)
                {
                    if (i == 0)
                    {
                        fromtime += item.fromtime.ToString();
                    }
                    else { fromtime += "," + item.fromtime.ToString(); }

                    i++;
                }
            }
            return fromtime;
        }

        public IEnumerable<PromotionCodeEntity> GetPromotionDetails()
        {
            var userPromotionDetailsEntity = _unitOfWork.PromotionCodeRepository.GetByCondition(d => d.isDeleted == false).ToList();

            if (userPromotionDetailsEntity.Any())
            {
                Mapper.CreateMap<PromotionCode, PromotionCodeEntity>();
                var userPromotionDetailsModel = Mapper.Map<List<PromotionCode>, List<PromotionCodeEntity>>(userPromotionDetailsEntity);
                return userPromotionDetailsModel;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>Random promotion from the list of promotion which is active</returns>
        public IEnumerable<PromotionsUserEntity> SelectRandom(int userID, string latitude, string longitude, string vendorType)
        {
            List<PromotionsUserEntity> promotionsUserEntity = new List<PromotionsUserEntity>();
            DateTime UTCTime1 = System.DateTime.UtcNow;
            DateTime dates3 = UTCTime1.AddHours(7.0);
           
                var getUserPromo = _unitOfWork.UserPromotionRepository.GetByCondition(d => d.userId == userID && d.isDeleted == false).Select(z => z.promotionId).ToList();

                //promotionCode = promotionCode.Where(x => !promotionIdList.Contains(x.p.promotionCodeId)).OrderBy(u => u.p.name).ToList();
           
            Random rnd = new Random();

            var vicinityVendor = onLocationChanged(Convert.ToDouble(latitude), Convert.ToDouble(longitude), 1);
            //var promotion = _unitOfWork.PromotionCodeRepository.GetByCondition(p => p.isActive == true && p.isDeleted == false && p.categoryId == Convert.ToInt32(vendorType)).Join(vicinityVendor, p => p.vendorId, v => v.vendorId, (p, v) => new { p, v }).OrderBy(c => rnd.Next()).Take(1).ToList();
            var promotion = _unitOfWork.PromotionCodeRepository.GetByCondition(p => p.isActive == true && p.isDeleted == false && p.categoryId == Convert.ToInt32(vendorType)).Join(vicinityVendor, p => p.vendorId, v => v.vendorId, (p, v) => new { p, v }).ToList();
            //var promotionCode = _unitOfWork.PromotionCodeRepository.GetByCondition(p => p.isActive == true && p.isDeleted == false).Join(vicinityVendor, p => p.vendorId, v => v.vendorId, (p, v) => new { p, v }).ToList();
            var qtyPromo = promotion.Where(z => z.p.expiryDate == null).ToList(); var datePromo = promotion.Where(z => z.p.expiryDate != null && z.p.expiryDate >= dates3).ToList();
            promotion = null; promotion = qtyPromo; promotion.AddRange(datePromo);
            promotion = promotion.Where(x => !getUserPromo.Contains(x.p.promotionCodeId)).OrderBy(c => rnd.Next()).Take(1).ToList();
            if (promotion.Any())
            {
                foreach (var Item in promotion)
                {
                    DateTime UTCTime = System.DateTime.UtcNow;
                    DateTime dates2 = UTCTime.AddHours(7.0);

                    var vendorId = _unitOfWork.VendorRepository.GetByCondition(d => d.vendorId == Item.p.vendorId).FirstOrDefault();
                    promotionsUserEntity.Add(new PromotionsUserEntity()
                    {
                        promotionCodeId = Item.p.promotionCodeId,
                        code = Item.p.code,
                        name = Item.p.name,
                        descriptionEnglish = Item.p.descriptionEnglish,
                        descriptionThai = Item.p.descriptionThai,
                        createdDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.p.createdDate),
                        expiryDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.p.expiryDate),
                        isActive = Item.p.isActive,
                        vendorId = Item.p.vendorId,
                        quantity = Item.p.quantity,
                        recommendation = Item.p.recommendation,
                        isPinned = Item.p.isPinned,
                        categoryId = Item.p.categoryId,
                        shareFacebookCount = Item.p.shareFacebookCount,
                        shareInstagramCount = Item.p.shareInstagramCount,
                        shareTwitterCount = Item.p.shareTwitterCount,
                        price = Item.p.price,
                        useCount = Item.p.useCount,
                        viewCount = Item.p.viewCount,
                        recommendationCount = Item.p.recommendationCount,
                        promotionImage = Item.p.promotionImage,
                        companyName = vendorId.companyName,
                        fullDescription = vendorId.fullDescription,
                        latitude = vendorId.latitude,
                        longitude = vendorId.longitude,
                        isFavourite = false,
                        fromday = fromday(Item.p.vendorId),
                        today = today(Item.p.vendorId),
                        fromtime = fromtime(Item.p.vendorId),
                        totime = totime(Item.p.vendorId),
                        serverTime = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2),
                        logoimg = vendorId.logoImg,
                        serverTime1 = String.Format("{0:dd/MM/yyyy HH:mm:ss}", dates2),
                        expiryDate1 = String.Format("{0:dd/MM/yyyy HH:mm:ss}", Item.p.expiryDate),
                       

                    });
                }
            }
            return promotionsUserEntity;

        }

        public IEnumerable<VendorEntity> onLocationChanged(double latCurrent, double lngCurrent, int typeFillter)
        {
            List<VendorEntity> VendorLists = new List<VendorEntity>();
            IEnumerable<Vendor> vendarlistfull = new List<Vendor>(_unitOfWork.VendorRepository.GetByCondition(d => d.isDeleted == false));
            if (vendarlistfull.Count() > 0)
            {
                foreach (var elementList in vendarlistfull)
                {
                    var latSave = elementList.latitude;
                    var longSave = elementList.longitude;
                    if (latSave != null && longSave != null)
                    {
                        VendorLists.Add(new VendorEntity()
                        {
                            vendorId = elementList.vendorId,
                            loginVendorName = elementList.companyName,
                            companyName = elementList.companyName,
                            businessCategory = elementList.businessCategory,
                            shortDescription = elementList.shortDescription,
                            fullDescription = elementList.fullDescription,
                            email = elementList.email,
                            phoneNo = elementList.phoneNo,
                            contactPerson = elementList.contactPerson,
                            contactPhoneNo = elementList.contactPhoneNo,
                            contactEmail = elementList.contactEmail,
                            streetName = elementList.streetName,
                            postCode = elementList.postCode,
                            buildingName = elementList.buildingName,
                            floor = elementList.floor,
                            area = elementList.area,
                            city = elementList.city,
                            longitude = elementList.longitude,
                            latitude = elementList.latitude,
                            password = elementList.password,
                            logoImg = elementList.logoImg,
                            Distance = distance(Convert.ToDouble(latSave), Convert.ToDouble(longSave), latCurrent, lngCurrent)
                        });
                    }
                }
            }
            if (typeFillter == 1)
            {
               // VendorLists = VendorLists.Where(x => x.Distance <= 2500.00).ToList();
                VendorLists = VendorLists.Where(x => x.Distance <= 10000.00).ToList();
            }
            var Shortlist = VendorLists.OrderBy(o => o.Distance).ThenBy(o => o.Distance).ToList();

            if (Shortlist.Any())
            {
                Mapper.CreateMap<Vendor, VendorEntity>();
                var vendorsModel = Mapper.Map<List<VendorEntity>, List<VendorEntity>>(Shortlist);
                return vendorsModel;
            }
            return null;
        }

        public IEnumerable<VendorEntity> GetAllVendors()
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

        /** calculates the distance between two locations in METER */
        private double distance(double lat1, double lng1, double lat2, double lng2)
        {

            var firstCordinate = new GeoCoordinate(lat1, lng1);
            var secondCordinate = new GeoCoordinate(lat2, lng2);

            double distance = firstCordinate.GetDistanceTo(secondCordinate);
            return distance;
        }

        public class VendorList
        {
            public double ID { get; set; }
            public double Distance { get; set; }
        }
        #endregion
        #region View, Use, Like Etc
        public UserFavouriteEntity ViewedPromotion(int ID, int vendorId, int userId)
        {
            UserFavouriteEntity uf = new UserFavouriteEntity();
            using (var scope = new TransactionScope())
            {
                var promo = _unitOfWork.PromotionCodeRepository.GetByID(ID);
                if (promo != null)
                {
                    int beforeCount = Convert.ToInt32(promo.viewCount);
                    promo.viewCount = beforeCount + 1;

                    _unitOfWork.PromotionCodeRepository.Update(promo);
                    _unitOfWork.Save();

                    scope.Complete();

                }
            }

            var favourite = _unitOfWork.UserFavoriteRepository.GetByCondition(u => u.userId == userId && u.vendorId == vendorId).FirstOrDefault();
            if (favourite != null)
            {

                uf.favoriteRestaurantId = favourite.favoriteRestaurantId;
                uf.isActive = favourite.isActive;

            }
            DateTime UTCTime = System.DateTime.UtcNow;
            DateTime dates2 = UTCTime.AddHours(7.0);
            uf.serverTime = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2);

            return uf;
        }

        public IEnumerable<UserPointEntity> ShareApp(string ShareVia, int userId)
        {
            var userData = _unitOfWork.UserRepository.GetByCondition(u => u.userId == userId).FirstOrDefault();
            var pointconfig = _unitOfWork.PointConfigurationRepository.GetByCondition(d => d.ptConfigurationId == 1).FirstOrDefault();
            DateTime facDates = Convert.ToDateTime(userData.facebookInviteDate);
            DateTime facDates2 = DateTime.Now;
            DateTime twiDates = Convert.ToDateTime(userData.twitterInviteDate);
            DateTime twiDates2 = DateTime.Now;
            DateTime instDates = Convert.ToDateTime(userData.instagramInviteDate);
            DateTime instDates2 = DateTime.Now;
            using (var scope = new TransactionScope())
            {
                if (facDates.Date != facDates2.Date)
                {
                    if (ShareVia.Trim() == "Facebook")
                    {
                        userData.facebookInviteCount = 1;
                        userData.facebookInviteDate = System.DateTime.Now;
                        userData.points += pointconfig.pointsEarned;
                        _unitOfWork.UserRepository.Update(userData);
                        _unitOfWork.Save();
                    }
                }

                if (twiDates.Date != twiDates2.Date)
                {
                    if (ShareVia.Trim() == "Twitter")
                    {
                        userData.twitterInviteCount = 1;
                        userData.twitterInviteDate = System.DateTime.Now;
                        userData.points += pointconfig.pointsEarned;
                        _unitOfWork.UserRepository.Update(userData);
                        _unitOfWork.Save();
                    }
                }

                if (instDates.Date != instDates2.Date)
                {
                    if (ShareVia.Trim() == "Instagram")
                    {
                        userData.instagramInviteCount = 1;
                        userData.instagramInviteDate = System.DateTime.Now;
                        userData.points += pointconfig.pointsEarned;
                        _unitOfWork.UserRepository.Update(userData);
                        _unitOfWork.Save();
                    }
                }
                scope.Complete();
            }
            using (var scope = new TransactionScope())
            {
                var userData1 = _unitOfWork.UserRepository.GetByCondition(u => u.userId == userId).FirstOrDefault();
                var pointconfig1 = _unitOfWork.PointConfigurationRepository.GetByCondition(d => d.ptConfigurationId == 1).FirstOrDefault();

                if (facDates.Date.Equals(facDates2.Date))
                {
                    if (Convert.ToInt32(pointconfig1.limit) > userData1.facebookInviteCount)
                    {
                        if (ShareVia.Trim() == "Facebook")
                        {
                            userData1.facebookInviteCount += 1;
                        }
                        userData1.points += pointconfig1.pointsEarned;
                        _unitOfWork.UserRepository.Update(userData1);
                        _unitOfWork.Save();
                    }
                    else
                    {
                        userData1.facebookInviteCount += 1;
                        _unitOfWork.UserRepository.Update(userData1);
                        _unitOfWork.Save();
                    }
                }

                if (twiDates.Date.Equals(twiDates2.Date))
                {
                    if (Convert.ToInt32(pointconfig1.limit) > userData1.twitterInviteCount)
                    {
                        if (ShareVia.Trim() == "Twitter")
                        {
                            userData1.twitterInviteCount += 1;
                        }
                        userData1.points += pointconfig1.pointsEarned;
                        _unitOfWork.UserRepository.Update(userData1);
                        _unitOfWork.Save();
                    }
                    else
                    {
                        userData1.twitterInviteCount += 1;
                        _unitOfWork.UserRepository.Update(userData1);
                        _unitOfWork.Save();
                    }
                }

                if (instDates.Date.Equals(instDates2.Date))
                {
                    if (Convert.ToInt32(pointconfig1.limit) > userData1.instagramInviteCount)
                    {
                        if (ShareVia.Trim() == "Instagram")
                        {
                            userData1.instagramInviteCount += 1;
                        }
                        userData1.points += pointconfig1.pointsEarned;
                        _unitOfWork.UserRepository.Update(userData1);
                        _unitOfWork.Save();
                    }
                    else
                    {
                        userData1.instagramInviteCount += 1;
                        _unitOfWork.UserRepository.Update(userData1);
                        _unitOfWork.Save();
                    }
                }
                scope.Complete();
            }
            var user = _unitOfWork.UserRepository.GetByCondition(d => d.userId == userId).FirstOrDefault();
            List<UserPointEntity> userPointEntity = new List<UserPointEntity>();

            userPointEntity.Add(new UserPointEntity()
            {
                userPoint = (user.points).ToString(),
            });

            return userPointEntity;
        }

        public bool ShareDeal(string ShareVia, int userId, int promotionId)
        {
            bool success = false;
            if (userId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var IsUsed = _unitOfWork.UserPromotionRepository.GetByCondition(d => d.isDeleted == false).ToList();
                    if (IsUsed.Count() > 0)
                    {
                        var promo = _unitOfWork.PromotionCodeRepository.GetByID(promotionId);
                        if (promo != null)
                        {
                            if (ShareVia.Trim() == "Facebook")
                            {
                                int beforeCount = Convert.ToInt32(promo.shareFacebookCount);
                                promo.shareFacebookCount = (beforeCount + 1).ToString();
                                _unitOfWork.PromotionCodeRepository.Update(promo);
                                _unitOfWork.Save();
                                var user = _unitOfWork.UserPromotionRepository.GetByCondition(d => d.promotionId == promotionId && d.userId == userId && d.isDeleted == false).FirstOrDefault();
                                if (user != null)
                                {
                                    user.isShareFacebook = true;
                                    _unitOfWork.UserPromotionRepository.Update(user);
                                    _unitOfWork.Save();
                                }
                            }

                            if (ShareVia.Trim() == "Instagram")
                            {
                                int beforeCount = Convert.ToInt32(promo.shareInstagramCount);
                                promo.shareInstagramCount = (beforeCount + 1).ToString();
                                _unitOfWork.PromotionCodeRepository.Update(promo);
                                _unitOfWork.Save();
                                var user = _unitOfWork.UserPromotionRepository.GetByCondition(d => d.promotionId == promotionId && d.userId == userId && d.isDeleted == false).FirstOrDefault();
                                if (user != null)
                                {
                                    user.isShareInstagram = true;
                                    _unitOfWork.UserPromotionRepository.Update(user);
                                    _unitOfWork.Save();
                                }

                            }

                            if (ShareVia.Trim() == "Twitter")
                            {
                                int beforeCount = Convert.ToInt32(promo.shareTwitterCount);
                                promo.shareTwitterCount = (beforeCount + 1).ToString();
                                _unitOfWork.PromotionCodeRepository.Update(promo);
                                _unitOfWork.Save();
                                var user = _unitOfWork.UserPromotionRepository.GetByCondition(d => d.promotionId == promotionId && d.userId == userId && d.isDeleted == false).FirstOrDefault();
                                if (user != null)
                                {
                                    user.isShareTwitter = true;
                                    _unitOfWork.UserPromotionRepository.Update(user);
                                    _unitOfWork.Save();
                                }
                            }
                        }

                        var userData = _unitOfWork.UserRepository.GetByCondition(u => u.userId == userId).FirstOrDefault();
                        var promotionCount = _unitOfWork.PromotionCodeRepository.GetByCondition(d => d.promotionCodeId == promotionId && d.isDeleted == false).FirstOrDefault();
                        var pointconfig = _unitOfWork.PointConfigurationRepository.GetByCondition(d => d.ptConfigurationId == 2).FirstOrDefault();
                        DateTime facDates = Convert.ToDateTime(userData.dealFacebookInviteDate);
                        DateTime facDates2 = DateTime.Now;
                        DateTime twiDates = Convert.ToDateTime(userData.dealTwitterInviteDate);
                        DateTime twiDates2 = DateTime.Now;
                        DateTime instDates = Convert.ToDateTime(userData.dealInstagramInviteDate);
                        DateTime instDates2 = DateTime.Now;
                        if (facDates.Date.Equals(facDates2.Date))
                        {
                            if (Convert.ToInt32(pointconfig.limit) > userData.dealFacebookInviteCount)
                            {
                                if (ShareVia.Trim() == "Facebook")
                                {
                                    userData.points += pointconfig.pointsEarned;
                                }
                            }
                            userData.dealFacebookInviteCount += 1;
                            _unitOfWork.UserRepository.Update(userData);
                            _unitOfWork.Save();
                        }

                        if (twiDates.Date.Equals(twiDates2.Date))
                        {
                            if (Convert.ToInt32(pointconfig.limit) > userData.dealTwitterInviteCount)
                            {
                                if (ShareVia.Trim() == "Twitter")
                                {
                                    userData.points += pointconfig.pointsEarned;
                                }
                            }
                            userData.dealTwitterInviteCount += 1;
                            _unitOfWork.UserRepository.Update(userData);
                            _unitOfWork.Save();

                        }

                        if (instDates.Date.Equals(instDates2.Date))
                        {
                            if (Convert.ToInt32(pointconfig.limit) > userData.dealInstagramInviteCount)
                            {
                                if (ShareVia.Trim() == "Instagram")
                                {
                                    userData.points += pointconfig.pointsEarned;
                                }
                            }
                            userData.dealInstagramInviteCount += 1;
                            _unitOfWork.UserRepository.Update(userData);
                            _unitOfWork.Save();

                        }

                        else
                        {
                            if (facDates.Date != facDates2.Date)
                            {
                                if (ShareVia.Trim() == "Facebook")
                                {
                                    userData.dealFacebookInviteDate = System.DateTime.Now;
                                    userData.dealFacebookInviteCount = 1;
                                    userData.points += pointconfig.pointsEarned;
                                    _unitOfWork.UserRepository.Update(userData);
                                    _unitOfWork.Save();

                                }
                            }

                            if (twiDates.Date != twiDates2.Date)
                            {
                                if (ShareVia.Trim() == "Twitter")
                                {
                                    userData.dealTwitterInviteDate = System.DateTime.Now;
                                    userData.dealTwitterInviteCount = 1;
                                    userData.points += pointconfig.pointsEarned;
                                    _unitOfWork.UserRepository.Update(userData);
                                    _unitOfWork.Save();

                                }
                            }
                            if (instDates.Date != instDates2.Date)
                            {
                                if (ShareVia.Trim() == "Instagram")
                                {
                                    userData.dealInstagramInviteDate = System.DateTime.Now;
                                    userData.dealInstagramInviteCount = 1;
                                    userData.points += pointconfig.pointsEarned;
                                    _unitOfWork.UserRepository.Update(userData);
                                    _unitOfWork.Save();

                                }
                            }
                        }
                    }

                    scope.Complete();
                }
            }
            else
            {
                var promotion = _unitOfWork.PromotionCodeRepository.GetByCondition(d => d.promotionCodeId == promotionId && d.isDeleted == false).FirstOrDefault();
                using (var scope = new TransactionScope())
                {
                    if (ShareVia.Trim() == "Facebook")
                    {
                        promotion.shareFacebookCount = (Convert.ToInt32(promotion.shareFacebookCount) + 1).ToString();
                        _unitOfWork.PromotionCodeRepository.Update(promotion);
                        _unitOfWork.Save();

                    }


                    if (ShareVia.Trim() == "Twitter")
                    {
                        promotion.shareTwitterCount = (Convert.ToInt32(promotion.shareTwitterCount) + 1).ToString();
                        _unitOfWork.PromotionCodeRepository.Update(promotion);
                        _unitOfWork.Save();

                    }


                    if (ShareVia.Trim() == "Instagram")
                    {
                        promotion.shareInstagramCount = (Convert.ToInt32(promotion.shareInstagramCount) + 1).ToString();
                        _unitOfWork.PromotionCodeRepository.Update(promotion);
                        _unitOfWork.Save();

                    }
                    scope.Complete();
                }
            }
            success = true;
            return success;
        }

        public string LeftTime(int qty, string expiryDate)
        {
            string returndata = string.Empty;
            if (qty == 0 && expiryDate != "")
            {
                TimeSpan ts = Convert.ToDateTime(expiryDate) - DateTime.Now;
                if (ts.Days == 0) { returndata = Convert.ToString(ts.Hours) + " Hours Left "; }
                else
                {
                    if (ts.Days > 1) { returndata = Convert.ToString(ts.Days) + " Days Left "; }
                    else
                    {
                        returndata = Convert.ToString(ts.Days) + " Day Left ";
                    }
                }

            }
            else { returndata = "Left " + Convert.ToString(qty) + " Qty"; }
            return returndata;
        }

        public bool IsBetween(DateTime time, DateTime startTime, DateTime endTime)
        {
            if (time.TimeOfDay == startTime.TimeOfDay) return true;
            if (time.TimeOfDay == endTime.TimeOfDay) return true;

            if (startTime.TimeOfDay <= endTime.TimeOfDay)
                return (time.TimeOfDay >= startTime.TimeOfDay && time.TimeOfDay <= endTime.TimeOfDay);
            else
                return !(time.TimeOfDay >= endTime.TimeOfDay && time.TimeOfDay <= startTime.TimeOfDay);
        }

        public int GetLandingDetailAppNext()
        {
            string category = string.Empty;


            string requestTime = System.DateTime.UtcNow.AddHours(7.0).ToString("HH:mm");

            int catId = 0; TimeSpan time1 = TimeSpan.FromMinutes(1);
            var landing = _unitOfWork.AppDefaultLandingPageRepository.GetByCondition(s => s.categoryStatus == "success").ToList();
            if (landing.Any())
            {
                foreach (var item in landing)
                {
                    string[] fromTime = item.fromTime.Split('.');
                    TimeSpan from = new TimeSpan(Convert.ToInt32(fromTime[0]), Convert.ToInt32(fromTime[1]), 0);
                    string[] toTime = item.toTime.Split('.');
                    TimeSpan to = new TimeSpan(Convert.ToInt32(toTime[0]), Convert.ToInt32(toTime[1]), 0);
                    string[] currentTime = requestTime.Split(':');
                    TimeSpan current = new TimeSpan(Convert.ToInt32(currentTime[0]), Convert.ToInt32(currentTime[1]), 0);


                    if (from <= current && to >= current)
                    {

                        var nowTime = to.Add(time1).ToString(@"hh\:mm").Replace(":", ".");
                        catId = landing.Where(z => z.fromTime.Equals(Convert.ToString(nowTime)) && z.categoryStatus == "success").FirstOrDefault().categoryId;
                    }

                    DateTime dtNow = DateTime.Now;
                    DateTime dtCurrent = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, current.Hours, current.Minutes, current.Seconds);
                    DateTime dtFrom = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, from.Hours, from.Minutes, from.Seconds);
                    DateTime dtTo = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, to.Hours, to.Minutes, to.Seconds);

                    if (IsBetween(dtCurrent, dtFrom, dtTo))
                    {
                        var nowTime = to.Add(time1).ToString(@"hh\:mm").Replace(":", ".");
                        catId = landing.Where(z => z.fromTime.Equals(nowTime) && z.categoryStatus == "success").FirstOrDefault().categoryId;
                    }
                }
            }

            return catId;

        }
        public string GetLandingDetailApp()
        {
            string category = string.Empty;
            DateTime Time = DateTime.Now; string requestTime = Time.ToString("HH:mm");

            string catId = string.Empty;
            var landing = _unitOfWork.AppDefaultLandingPageRepository.GetByCondition(s => s.categoryStatus == "success").ToList();
            if (landing.Any())
            {
                foreach (var item in landing)
                {
                    string[] fromTime = item.fromTime.Split('.');
                    TimeSpan from = new TimeSpan(Convert.ToInt32(fromTime[0]), Convert.ToInt32(fromTime[1]), 0);
                    string[] toTime = item.toTime.Split('.');
                    TimeSpan to = new TimeSpan(Convert.ToInt32(toTime[0]), Convert.ToInt32(toTime[1]), 0);
                    string[] currentTime = requestTime.Split(':');
                    TimeSpan current = new TimeSpan(Convert.ToInt32(currentTime[0]), Convert.ToInt32(currentTime[1]), 0);


                    if (from <= current && to >= current)
                    {
                        catId = item.type;
                    }

                    DateTime dtNow = DateTime.Now;
                    DateTime dtCurrent = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, current.Hours, current.Minutes, current.Seconds);
                    DateTime dtFrom = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, from.Hours, from.Minutes, from.Seconds);
                    DateTime dtTo = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, to.Hours, to.Minutes, to.Seconds);
                    if (IsBetween(dtCurrent, dtFrom, dtTo))
                    {
                        catId = item.type;
                    }
                }
            }

            return catId;

        }
        public bool CompareDate(int userId, int promoId)
        {
            bool success = true; int dateCompare = 0;
            var IsUsed = _unitOfWork.UserPromotionRepository.GetByCondition(d => d.userId == userId && d.promotionId == promoId && d.isUsed == true && d.isDeleted == false).LastOrDefault();
            var promoRelaunchCount = _unitOfWork.PromotionCodeRepository.GetByCondition(p => p.promotionCodeId.Equals(promoId) && p.isDeleted == false).FirstOrDefault();
            if (IsUsed != null)
            {
                if (IsUsed.useDate != null) dateCompare = DateTime.Compare(Convert.ToDateTime(IsUsed.useDate), Convert.ToDateTime(promoRelaunchCount.lastRelaunchDate));
                if (dateCompare >= 0) { success = false; } else { success = true; }
            }

            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="userId"></param>
        /// <returns>list of promotions which are not used by user near by 2.5 km from the current location of user
        /// if no promotion from the current location of user then we will return default promotions which are active
        /// </returns>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="userId"></param>
        /// <returns>list of promotions which are not used by user near by 2.5 km from the current location of user
        /// if no promotion from the current location of user then we will return default promotions which are active
        /// </returns>
        public IEnumerable<PromotionsUserEntity> UsePromotion(int ID, int userId, string deviceToken, string devicePlatform)
        {
            int promoId = ID;
            int tempValue = 0;
            bool success = false;
            List<PromotionsUserEntity> promotionsUserEntity = new List<PromotionsUserEntity>();
            int countPromo = 0;
            if (userId > 0)
            {
                var updatedQuantity = _unitOfWork.UserPromotionRepository.GetByCondition(d => d.promotionId == ID && d.userId == userId && d.useDate != null && d.isDeleted == false).Count();
                countPromo = updatedQuantity;
            }
            else
            {
                var updatedQuantity = _unitOfWork.UserPromotionSkipRepository.GetByCondition(d => d.promotionId == ID && d.deviceToken == deviceToken && d.devicePlatform == devicePlatform && d.useDate != null).Count();
                countPromo = updatedQuantity;
            }
            var expired = _unitOfWork.PromotionCodeRepository.GetByCondition(c => c.promotionCodeId == promoId).FirstOrDefault().quantity;

            if (userId == 0)
            {
                #region anonymous user
                var chkdata = _unitOfWork.UserPromotionSkipRepository.GetByCondition(d => d.deviceToken == deviceToken && d.devicePlatform == devicePlatform && d.promotionId == promoId).Count();
                if (chkdata == 0)
                {
                    using (var Skeepscope = new TransactionScope())
                    {
                        var _UserPromotionSkip = new UserPromotionSkip
                        {
                            userId = userId,
                            promotionId = promoId,
                            isRecommended = false,
                            isShareFacebook = false,
                            isShareTwitter = false,
                            isShareInstagram = false,
                            isViewed = false,
                            isUsed = true,
                            useDate = DateTime.Now,
                            userTime = GetLandingDetailApp(),
                            deviceToken = deviceToken,
                            devicePlatform = devicePlatform
                        };
                        _unitOfWork.UserPromotionSkipRepository.Insert(_UserPromotionSkip);
                        _unitOfWork.Save();
                        Skeepscope.Complete();
                        Thread thread = new Thread(() => SendNotificationSkip(userId, promoId, 1, deviceToken, devicePlatform));
                        thread.Start();
                    }
                    var promo = _unitOfWork.PromotionCodeRepository.GetByID(ID);
                    int dateCompare = -2; if (promo.expiryDate != null) dateCompare = DateTime.Compare(Convert.ToDateTime(promo.expiryDate), DateTime.Now);
                    if (promo != null && (promo.quantity > 0) || dateCompare >= 0)
                    {
                        if (promo.isActive)
                        {
                            int beforeCount = Convert.ToInt32(promo.useCount); int beforeQty = Convert.ToInt32(promo.quantity);
                            promo.useCount = beforeCount + 1;
                            if (promo.expiryDate == null)
                            {
                                promo.quantity = beforeQty - 1;
                            }
                            if (beforeQty == 0)
                            {
                                promo.isActive = true;

                            }
                            _unitOfWork.PromotionCodeRepository.Update(promo);
                            _unitOfWork.Save();
                           
                        }
                    }

                    var PromoData = _unitOfWork.PromotionCodeRepository.GetByID(ID);
                    int dateCompare2 = -3; if (PromoData.expiryDate != null) dateCompare2 = DateTime.Compare(Convert.ToDateTime(PromoData.expiryDate), DateTime.Now);

                    if (PromoData != null && (PromoData.quantity == 0) && dateCompare2 < 0)
                    {
                        using (var scopes = new TransactionScope())
                        {
                            PromoData.isActive = false;
                            _unitOfWork.PromotionCodeRepository.Update(PromoData);
                            _unitOfWork.Save();
                            scopes.Complete();
                        }
                    }

                }
                #endregion
            }
            else
            {
                #region without anonymous user
                using (var scope = new TransactionScope())
                {
                    var IsUsed = _unitOfWork.UserPromotionRepository.GetByCondition(d => d.userId == userId && d.promotionId == ID && d.isUsed == true && d.isDeleted == false).ToList();
                    var promoRelaunchCount = _unitOfWork.PromotionCodeRepository.GetByCondition(p => p.promotionCodeId.Equals(ID)).FirstOrDefault();

                    var promo = _unitOfWork.PromotionCodeRepository.GetByID(ID);
                    if (CompareDate(userId, promoId))

                        if (Convert.ToInt32(promoRelaunchCount.isRelaunch) >= IsUsed.Count())
                        {
                            int dateCompare = -2; if (promo.expiryDate != null) dateCompare = DateTime.Compare(Convert.ToDateTime(promo.expiryDate), DateTime.Now);
                            if (promo != null && (promo.quantity > 0) || dateCompare >= 0)
                            {
                                if (promo.isActive)
                                {
                                    int beforeCount = Convert.ToInt32(promo.useCount); int beforeQty = Convert.ToInt32(promo.quantity);
                                    promo.useCount = beforeCount + 1;
                                    if (promo.expiryDate == null)
                                    {
                                        promo.quantity = beforeQty - 1;
                                    }
                                    if (beforeQty == 0)
                                    {
                                        promo.isActive = true;

                                    }
                                    _unitOfWork.PromotionCodeRepository.Update(promo);
                                    _unitOfWork.Save();
                                    Thread thread = new Thread(() => SendNotification(userId, promoId, 1));
                                    thread.Start();
                                }
                                var user = _unitOfWork.UserPromotionRepository.GetByCondition(d => d.promotionId == ID && d.userId == userId && d.isDeleted == false).FirstOrDefault();
                                if (user != null)
                                {
                                    user.isUsed = true;
                                    user.useDate = DateTime.Now;
                                    user.isDeleted = false;
                                    _unitOfWork.UserPromotionRepository.Update(user);
                                    _unitOfWork.Save();
                                }
                                else
                                {
                                    var _UserPromotion = new UserPromotion
                                    {
                                        userId = userId,
                                        promotionId = ID,
                                        isRecommended = false,
                                        isShareFacebook = false,
                                        isShareTwitter = false,
                                        isShareInstagram = false,
                                        isViewed = false,
                                        isUsed = true,
                                        useDate = DateTime.Now,
                                        isDeleted = false,
                                        userTime = GetLandingDetailApp()
                                    };
                                    _unitOfWork.UserPromotionRepository.Insert(_UserPromotion);
                                    _unitOfWork.Save();
                                }
                            }
                            var userData = _unitOfWork.UserRepository.GetByCondition(u => u.userId == userId).FirstOrDefault();
                            var pointconfig = _unitOfWork.PointConfigurationRepository.GetByCondition(d => d.ptConfigurationId == 4).FirstOrDefault();
                            if (userData.usePromoDate == System.DateTime.Now)
                            {

                                if (Convert.ToInt32(pointconfig.limit) < userData.useCount)
                                {
                                    userData.useCount += 1;
                                    userData.points += pointconfig.pointsEarned;
                                    _unitOfWork.UserRepository.Update(userData);
                                    _unitOfWork.Save();
                                }
                            }
                            else
                            {
                                userData.usePromoDate = System.DateTime.Now;
                                userData.useCount = 1;
                                userData.points += pointconfig.pointsEarned;
                                _unitOfWork.UserRepository.Update(userData);
                                _unitOfWork.Save();

                            }
                        }

                    var PromoData = _unitOfWork.PromotionCodeRepository.GetByID(ID);
                    int dateCompare2 = -3; if (PromoData.expiryDate != null) dateCompare2 = DateTime.Compare(Convert.ToDateTime(PromoData.expiryDate), DateTime.Now);

                    if (PromoData != null && (PromoData.quantity == 0) && dateCompare2 < 0)
                    {
                        using (var scopes = new TransactionScope())
                        {
                            PromoData.isActive = false;
                            _unitOfWork.PromotionCodeRepository.Update(PromoData);
                            _unitOfWork.Save();
                            scopes.Complete();
                        }
                    }
                    success = true;
                    scope.Complete();
                }
                #endregion
            }
            List<int> promotionIdList = new List<int>();

            if (userId != 0)
            {
                var getUserPromo = _unitOfWork.UserPromotionRepository.GetByCondition(d => d.userId == userId && d.isDeleted == false).Select(z => z.promotionId).ToList();
                foreach (var item in getUserPromo)
                {
                    promotionIdList.Add(Convert.ToInt32(item));
                }

            }
            else
            {
                var getSkipUserPromo = _unitOfWork.UserPromotionSkipRepository.GetByCondition(d => d.userId == userId && d.deviceToken == deviceToken && d.devicePlatform == devicePlatform).Select(z => z.promotionId).ToList();
                foreach (var item in getSkipUserPromo)
                {
                    promotionIdList.Add(Convert.ToInt32(item));
                }
            }
            var vendorType = _unitOfWork.PromotionCodeRepository.GetByID(ID).categoryId;
            var promotionvendor = _unitOfWork.PromotionCodeRepository.GetByCondition(t => t.promotionCodeId == promoId).FirstOrDefault().vendorId;
            var vendorLoc = _unitOfWork.VendorRepository.GetByCondition(v => v.vendorId == promotionvendor).FirstOrDefault();

            ///* GetAll Vendor if UserDefault Location Is 0*/
            var vendorAll = _unitOfWork.VendorRepository.GetAll().FirstOrDefault();
            /* GetAll Vendor Corresponding Vendor if UserDefault Location Is 0*/
            var vicinityVendor = onLocationChanged(Convert.ToDouble(vendorAll.latitude), Convert.ToDouble(vendorAll.longitude), 1);
            if (vendorLoc != null)
            {
                vicinityVendor = onLocationChanged(Convert.ToDouble(vendorLoc.latitude), Convert.ToDouble(vendorLoc.longitude), 1);
                if (vicinityVendor == null || vicinityVendor.Count() == 0)
                {
                    vicinityVendor = onLocationChanged(Convert.ToDouble(vendorLoc.latitude), Convert.ToDouble(vendorLoc.longitude), 0);
                }
            }

            if (vendorType > 0)
            {
                var Get = vicinityVendor.Where(z => z.businessCategory == vendorType).ToList();
                if (Get.Count() >= 3) { vicinityVendor = Get; }

            }
            Random rnd = new Random();
            var nextMealCategoryID = GetLandingDetailAppNext();
            var promotionCode = _unitOfWork.PromotionCodeRepository.GetByCondition(p => p.isActive == true && p.isDeleted == false).Join(vicinityVendor, p => p.vendorId, v => v.vendorId, (p, v) => new { p, v }).ToList();
            promotionCode = promotionCode.OrderBy(e => e.p.expiryDate).OrderBy(q => q.p.quantity).ToList();
            promotionCode = promotionCode.Where(x => !promotionIdList.Contains(x.p.promotionCodeId)).OrderBy(u => u.p.name).ToList();
            var temp = promotionCode.Where(c => c.p.categoryId.Equals(nextMealCategoryID)).OrderBy(c => rnd.Next()).Take(1).ToList();
            var cDate = System.DateTime.UtcNow.AddHours(7.0).AddDays(1);
            temp.AddRange(promotionCode.Where(c => !c.p.categoryId.Equals(nextMealCategoryID)).OrderBy(c => rnd.Next()).Take(1));
            temp.AddRange(promotionCode.Where(c => !c.p.categoryId.Equals(temp.Select(d => d.p.categoryId)) && !temp.Select(z => z.p.promotionCodeId).Contains(c.p.promotionCodeId)).OrderBy(c => rnd.Next()).Take(1));
            temp.AddRange(promotionCode.Where(c => c.p.categoryId.Equals(nextMealCategoryID) && c.p.expiryDate > cDate && !temp.Select(z => z.p.promotionCodeId).Contains(c.p.promotionCodeId)).OrderBy(c => rnd.Next()).Take(1));
            promotionCode = temp;

            if (promotionCode.Count() > 0)
            {
                {
                    foreach (var Item in promotionCode)
                    {
                        DateTime UTCTime = System.DateTime.UtcNow;
                        DateTime dates2 = UTCTime.AddHours(7.0);
                        var favourite = _unitOfWork.UserFavoriteRepository.GetByCondition(f => f.userId == userId && f.vendorId == Item.p.vendorId).FirstOrDefault();
                        var vendor = _unitOfWork.VendorRepository.GetByCondition(v => v.vendorId == Item.p.vendorId).FirstOrDefault();
                        promotionsUserEntity.Add(new PromotionsUserEntity()
                        {
                            promotionCodeId = Item.p.promotionCodeId,
                            code = Item.p.code,
                            name = Item.p.name,
                            descriptionEnglish = Item.p.descriptionEnglish,
                            descriptionThai = Item.p.descriptionThai,
                            createdDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.p.createdDate),
                            expiryDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.p.expiryDate),
                            isActive = Item.p.isActive,
                            vendorId = Item.p.vendorId,
                            quantity = Item.p.quantity,
                            recommendation = Item.p.recommendation,
                            isPinned = Item.p.isPinned,
                            categoryId = Item.p.categoryId,
                            shareFacebookCount = Item.p.shareFacebookCount,
                            shareInstagramCount = Item.p.shareInstagramCount,
                            shareTwitterCount = Item.p.shareTwitterCount,
                            price = Item.p.price,
                            useCount = Item.p.useCount == null ? 0 : Item.p.useCount,
                            viewCount = Item.p.viewCount == null ? 0 : Item.p.viewCount,
                            recommendationCount = Item.p.recommendationCount,
                            promotionImage = Item.p.promotionImage,
                            isFavourite = favourite != null ? favourite.isActive : false,
                            favouriteId = favourite != null ? favourite.favoriteRestaurantId.ToString() : "",
                            latitude = Item.v.latitude,
                            longitude = Item.v.longitude,
                            fromday = fromday(vendor.vendorId),
                            today = today(vendor.vendorId),
                            fromtime = fromtime(vendor.vendorId),
                            totime = totime(vendor.vendorId),
                            logoimg = vendor.logoImg,
                            fullDescription = "",
                            serverTime = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2),
                            tempQuantity = countPromo > 0 ? "true" : "false",
                            promoExpired = expired == 0 ? true : false
                        });
                    }

                    Thread threadPre = new Thread(() => SetSuccessInPredicativeNotification(userId, promoId));
                    threadPre.Start();
                    return GetDefaultPromotion(promotionsUserEntity, userId);
                }
            }
            else
            {
                var promotionWithoutLocation = _unitOfWork.PromotionCodeRepository.GetByCondition(d => d.isActive == true && !promotionIdList.Contains(d.promotionCodeId) && d.isDeleted == false).OrderBy(u => u.name).Take(4).ToList();
                if (promotionWithoutLocation.Count() > 0)
                {
                    foreach (var Item in promotionWithoutLocation)
                    {
                        DateTime UTCTime = System.DateTime.UtcNow;
                        DateTime dates2 = UTCTime.AddHours(7.0);
                        var favourite = _unitOfWork.UserFavoriteRepository.GetByCondition(f => f.userId == userId && f.vendorId == Item.vendorId).FirstOrDefault();
                        var vendor = _unitOfWork.VendorRepository.GetByCondition(v => v.vendorId == Item.vendorId).FirstOrDefault();

                        promotionsUserEntity.Add(new PromotionsUserEntity()
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
                            quantity = Item.quantity,
                            recommendation = Item.recommendation,
                            isPinned = Item.isPinned,
                            categoryId = Item.categoryId,
                            shareFacebookCount = Item.shareFacebookCount,
                            shareInstagramCount = Item.shareInstagramCount,
                            shareTwitterCount = Item.shareTwitterCount,
                            price = Item.price,
                            useCount = Item.useCount == null ? 0 : Item.useCount,
                            viewCount = Item.viewCount == null ? 0 : Item.
                            viewCount,
                            recommendationCount = Item.recommendationCount,
                            promotionImage = Item.promotionImage,
                            isFavourite = favourite != null ? favourite.isActive : false,
                            favouriteId = favourite != null ? favourite.favoriteRestaurantId.ToString() : "",
                            latitude = vendor.latitude,
                            longitude = vendor.longitude,
                            fromday = fromday(vendor.vendorId),
                            today = today(vendor.vendorId),
                            fromtime = fromtime(vendor.vendorId),
                            totime = totime(vendor.vendorId),
                            logoimg = vendor.logoImg,
                            fullDescription = "",
                            serverTime = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2),
                            tempQuantity = countPromo > 0 ? "true" : "false",
                            promoExpired = expired == 0 ? true : false
                        });
                    }

                    Thread threadPre = new Thread(() => SetSuccessInPredicativeNotification(userId, promoId));
                    threadPre.Start();
                    return GetDefaultPromotion(promotionsUserEntity, userId);
                }
            }
            return promotionsUserEntity.Take(4);
        }

        public IEnumerable<PromotionsUserEntity> GetDefaultPromotion(List<PromotionsUserEntity> promotionsUserEntity, int userId)
        {
            var ids = promotionsUserEntity.Select(x => x.promotionCodeId).ToList();
            ids.AddRange(_unitOfWork.UserPromotionRepository.GetByCondition(u => u.userId == userId && u.isDeleted == false).Select(p => Convert.ToInt32(p.promotionId)).ToList());
            var promotionWithoutLocation = _unitOfWork.PromotionCodeRepository.GetByCondition(z => !ids.Contains(z.promotionCodeId) && z.isActive == true && z.isDeleted == false).Take(4 - promotionsUserEntity.Count()).ToList();
            foreach (var Item in promotionWithoutLocation)
            {
                var favourite = _unitOfWork.UserFavoriteRepository.GetByCondition(f => f.userId == userId && f.vendorId == Item.vendorId).FirstOrDefault();
                var vendor = _unitOfWork.VendorRepository.GetByCondition(v => v.vendorId == Item.vendorId).FirstOrDefault();

                promotionsUserEntity.Add(new PromotionsUserEntity()
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
                    quantity = Item.quantity,
                    recommendation = Item.recommendation,
                    isPinned = Item.isPinned,
                    categoryId = Item.categoryId,
                    shareFacebookCount = Item.shareFacebookCount,
                    shareInstagramCount = Item.shareInstagramCount,
                    shareTwitterCount = Item.shareTwitterCount,
                    price = Item.price,
                    useCount = Item.useCount == null ? 0 : Item.useCount,
                    viewCount = Item.viewCount == null ? 0 : Item.
                    viewCount,
                    recommendationCount = Item.recommendationCount,
                    promotionImage = Item.promotionImage,
                    isFavourite = favourite != null ? favourite.isActive : false,
                    favouriteId = favourite != null ? favourite.favoriteRestaurantId.ToString() : "",
                    latitude = vendor.latitude,
                    longitude = vendor.longitude,
                    fromday = fromday(vendor.vendorId),
                    today = today(vendor.vendorId),
                    fromtime = fromtime(vendor.vendorId),
                    totime = totime(vendor.vendorId),
                    logoimg = vendor.logoImg,
                    fullDescription = ""
                });

            }
            return promotionsUserEntity;
        }

        public void SetSuccessInPredicativeNotification(int uId, int pId)
        {
            var predi = _unitOfWork.PredictiveNotificationRepository.GetByCondition(z => z.userId == uId && z.preId == pId).FirstOrDefault();
            using (var scope = new TransactionScope())
            {
                if (predi != null)
                {
                    predi.status = true;

                    _unitOfWork.PredictiveNotificationRepository.Update(predi);
                    _unitOfWork.Save();
                    scope.Complete();
                }
            }
        }

        public void SendNotification(int userId, int pId, int isUsed)
        {
            System.Threading.Thread.Sleep(5000);
            ResourceManager resmgr = new ResourceManager("TV.CraveASAP.BusinessServices.EddeeResourceFile ", Assembly.GetExecutingAssembly());
            var UserDevice = _unitOfWork.UserRepository.GetByID(userId);
            var promotion = _unitOfWork.PromotionCodeRepository.GetByCondition(p => p.promotionCodeId.Equals(pId)).FirstOrDefault();
            var cateName = _unitOfWork.CategoryRepository.GetByCondition(z => z.categoryId == promotion.categoryId).FirstOrDefault().categoryName;
            List<DeviceEntity> Device = new List<DeviceEntity>();
            PushNotification pn = new PushNotification();
            var usePromo = _unitOfWork.UserPromotionRepository.GetByCondition(u => u.userId == userId && u.isUsed == true && u.isDeleted == false).ToList();
            if (isUsed == 1)
            {
                if (usePromo != null)
                {
                    Device = new List<DeviceEntity>();
                    Device.Add(new DeviceEntity
                    {
                        Device = UserDevice.deviceToken,
                        Alert = "Eddee Notification",
                        MessageEnglish = "Dear User, You have recently used  " + promotion.code + " promotion!!!",
                        DeviceType = UserDevice.devicePlatform,
                        AppType = "User",
                        type = "UsePromotion",
                        badge = usePromo.Count(),
                    });
                    pn.SendPushNotification(Device);
                }
            }

            if (usePromo.Count >= 4)
            {
                var subcategoryID = _unitOfWork.PromotionCodeRepository.GetByCondition(s => s.promotionCodeId.Equals(pId)).FirstOrDefault();
                var promo = _unitOfWork.PromotionCodeRepository.GetByCondition(z => usePromo.Select(p => p.promotionId).Contains(z.promotionCodeId)).ToList();
                var subNoti = promo.Where(z => z.subCategoryId == subcategoryID.subCategoryId).Count();
                var userPrefrencesSub = _unitOfWork.UserPrefrenceRepository.GetByCondition(z => z.userId.Equals(userId) && z.type == "sub" && z.prefrencesId == subcategoryID.subCategoryId).ToList();
                if (subNoti == 4 && userPrefrencesSub.Count() <= 0)
                {
                    var subTbl = _unitOfWork.SubCategoryRepository.GetByCondition(z => z.subCategoryId == subcategoryID.subCategoryId).FirstOrDefault();
                    Device = new List<DeviceEntity>();
                    Device.Add(new DeviceEntity
                    {
                        Device = UserDevice.deviceToken,
                        Alert = "Eddee Notification",
                        MessageEnglish = "Dear User, Do you want to add " + subTbl.subCategoryName + " cuisine on your favourite cuisines?",
                        DeviceType = UserDevice.devicePlatform,
                        AppType = "User",
                        type = "Cuisine",
                        subCategory = Convert.ToString(subTbl.subCategoryId) + "," + Convert.ToString(subTbl.categoryId),
                        badge = 1,
                    });
                    pn.SendPushNotification(Device);
                }

                var promoOpt = _unitOfWork.PromotionCodeRepository.GetByCondition(z => usePromo.Select(p => p.promotionId).Contains(z.promotionCodeId)).ToList();
                var OptNoti = promo.Where(z => z.subCategoryId == subcategoryID.optCategoryId).Count();
                var userPrefrencesOpt = _unitOfWork.UserPrefrenceRepository.GetByCondition(z => z.userId.Equals(userId) && z.type == "opt" && z.prefrencesId == subcategoryID.optCategoryId).ToList();
                if (OptNoti == 4 && userPrefrencesOpt.Count() <= 0)
                {
                    var optTbl = _unitOfWork.OptionalCategoryRepository.GetByCondition(z => z.subCategoryId == subcategoryID.optCategoryId).FirstOrDefault();
                    Device = new List<DeviceEntity>();
                    Device.Add(new DeviceEntity
                    {
                        Device = UserDevice.deviceToken,
                        Alert = "Eddee Notification",
                        MessageEnglish = "Dear User, Do you want to add " + optTbl.optCategoryName + " cuisine on your favourite cuisines?",
                        MessageThai = "ผู้ใช้ที่รักคุณต้องการที่จะเพิ่ม",
                        DeviceType = UserDevice.devicePlatform,
                        AppType = "User",
                        type = "Cuisine",
                        optCategory = Convert.ToString(optTbl.optCategoryId) + "," + Convert.ToString(optTbl.categoryId),
                        badge = 1,

                    });
                    pn.SendPushNotification(Device);
                }
            }

            var userFavourite = _unitOfWork.UserFavoriteRepository.GetByCondition(z => z.userId.Equals(userId) && z.vendorId.Equals(promotion.vendorId)).ToList();
            var usePromoCount = _unitOfWork.PromotionCodeRepository.GetByCondition(u => usePromo.Select(p => p.promotionId).Contains(u.promotionCodeId) && u.vendorId == promotion.vendorId).ToList();

            if (userFavourite.Count() <= 0 && usePromoCount.Count() == 4)
            {
                Device = new List<DeviceEntity>();
                Device.Add(new DeviceEntity
                {
                    Device = UserDevice.deviceToken,
                    Alert = "Eddee Notification",
                    MessageEnglish = "Dear User, Do you want to add " + _unitOfWork.VendorRepository.GetByCondition(x => x.vendorId.Equals(promotion.vendorId)).FirstOrDefault().companyName + " venue on your favourite venue?",
                    //     MessageThai ="ผู้ใช้ที่รักคุณต้องการที่จะเพิ่ม",
                    badge = 1,
                    DeviceType = UserDevice.devicePlatform,
                    type = "Venue",
                    AppType = "User",
                    vendorId = Convert.ToString(promotion.vendorId),
                });

                pn.SendPushNotification(Device);
            }
        }

        public void SendNotificationSkip(int userId, int pId, int isUsed, string deviceToken, string devicePlatform)
        {
            System.Threading.Thread.Sleep(5000);
            ResourceManager resmgr = new ResourceManager("TV.CraveASAP.BusinessServices.EddeeResourceFile ", Assembly.GetExecutingAssembly());
            var UserDevice = _unitOfWork.UserPromotionSkipRepository.GetByCondition(d => d.userId == userId && d.deviceToken == deviceToken && d.devicePlatform == devicePlatform).FirstOrDefault();
            var promotion = _unitOfWork.PromotionCodeRepository.GetByCondition(p => p.promotionCodeId.Equals(pId)).FirstOrDefault();
            var cateName = _unitOfWork.CategoryRepository.GetByCondition(z => z.categoryId == promotion.categoryId).FirstOrDefault().categoryName;
            List<DeviceEntity> Device = new List<DeviceEntity>();
            PushNotification pn = new PushNotification();
            var usePromo = _unitOfWork.UserPromotionSkipRepository.GetByCondition(u => u.userId == userId && u.isUsed == true && u.deviceToken == deviceToken && u.devicePlatform == devicePlatform).ToList();
            if (isUsed == 1)
            {
                if (usePromo != null)
                {
                    Device = new List<DeviceEntity>();
                    Device.Add(new DeviceEntity
                    {
                        Device = UserDevice.deviceToken,
                        Alert = "Eddee Notification",
                        MessageEnglish = "Dear User, You have recently used  " + promotion.code + " promotion!!!",
                        DeviceType = UserDevice.devicePlatform,
                        AppType = "User",
                        type = "UsePromotion",
                        badge = usePromo.Count(),

                    });
                    pn.SendPushNotification(Device);
                }
            }

        }

        public bool LikePromotion(int ID, int userId, bool like)
        {
            bool success = false;
            using (var scope = new TransactionScope())
            {
                if (userId == 0)
                {
                    var promo = _unitOfWork.PromotionCodeRepository.GetByID(ID);
                    if (promo != null)
                    {
                        int beforeCount = Convert.ToInt32(promo.recommendationCount);
                        if (like)
                            promo.recommendationCount = beforeCount + 1;
                        else
                            promo.recommendationCount = beforeCount - 1;

                        _unitOfWork.PromotionCodeRepository.Update(promo);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }

                else
                {
                    var promo = _unitOfWork.PromotionCodeRepository.GetByID(ID);
                    if (promo != null)
                    {
                        int beforeCount = Convert.ToInt32(promo.recommendationCount);
                        if (like)
                            promo.recommendationCount = beforeCount + 1;
                        else
                            promo.recommendationCount = beforeCount - 1;

                        var userPromo = _unitOfWork.UserPromotionRepository.GetByCondition(d => d.promotionId == ID && d.userId == userId && d.isDeleted == false).FirstOrDefault();
                        if (userPromo != null)
                        {
                            if (like)
                                userPromo.isRecommended = true;
                            else
                                userPromo.isRecommended = false;
                            _unitOfWork.UserPromotionRepository.Update(userPromo);
                            _unitOfWork.Save();
                        }
                        else
                        {
                            using (var Skeepscope = new TransactionScope())
                            {
                                var _userPromotion = new UserPromotion
                                {
                                    userId = userId,
                                    promotionId = ID,
                                    isRecommended = like,
                                    isShareFacebook = false,
                                    isShareTwitter = false,
                                    isShareInstagram = false,
                                    isViewed = false,
                                    isUsed = false,
                                    useDate = null,
                                    userTime = "",
                                    isDeleted = false

                                };
                                _unitOfWork.UserPromotionRepository.Insert(_userPromotion);
                                _unitOfWork.Save();
                                Skeepscope.Complete();
                            }
                        }
                        _unitOfWork.PromotionCodeRepository.Update(promo);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }

            if (userId > 0)
            {
                var userData = _unitOfWork.UserRepository.GetByCondition(u => u.userId == userId).FirstOrDefault();
                var pointconfig = _unitOfWork.PointConfigurationRepository.GetByCondition(d => d.ptConfigurationId == 3).FirstOrDefault();
                if (userData.likePromoDate == System.DateTime.Now)
                {
                    if (Convert.ToInt32(pointconfig.limit) < userData.likeCount)
                    {
                        userData.likeCount += 1;
                        userData.points += pointconfig.pointsEarned;
                        _unitOfWork.UserRepository.Update(userData);
                        _unitOfWork.Save();
                    }
                }
                else
                {
                    userData.likePromoDate = System.DateTime.Now;
                    userData.likeCount = 1;
                    userData.points = pointconfig.pointsEarned;
                }
            }
            return success;
        }

        #endregion

        #region App
        public IEnumerable<PromotionsUserEntity> GetAllDealWebApp()
        {
            bool isMore = true;
            var vicinityVendor = _unitOfWork.VendorRepository.GetByCondition(d => d.isDeleted == false);
            var promotionCode = _unitOfWork.PromotionCodeRepository.GetByCondition(p => p.isActive == true && p.isDeleted == false).Join(vicinityVendor, p => p.vendorId, v => v.vendorId, (p, v) => new { p, v }).ToList();
            var promotionCodeCopy = _unitOfWork.PromotionCodeRepository.GetByCondition(p => p.isActive == true && p.isDeleted == false).Join(vicinityVendor, p => p.vendorId, v => v.vendorId, (p, v) => new { p, v }).ToList();
            promotionCode = promotionCode.OrderBy(o => o.p.createdDate).ThenBy(o => o.p.createdDate).ToList();
            var getTop = promotionCodeCopy; var pTemp = promotionCode; getTop.Clear();

            for (int i = 0; i < 3; i++)
            {
                pTemp = promotionCode.Where(x => x.p.categoryId == i + 1).ToList();
                if (pTemp.Count >= 3) getTop.AddRange(pTemp.Take(3)); else getTop.AddRange(pTemp);
            }
            promotionCode = getTop;
            List<PromotionsUserEntity> promotionsUserEntity = new List<PromotionsUserEntity>();
            foreach (var Item in promotionCode)
            {
                promotionsUserEntity.Add(new PromotionsUserEntity()
                {
                    promotionCodeId = Item.p.promotionCodeId,
                    code = Item.p.code,
                    name = Item.p.name,
                    descriptionEnglish = Item.p.descriptionEnglish,
                    descriptionThai = Item.p.descriptionThai,
                    createdDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.p.createdDate),
                    expiryDate = LeftTime(Convert.ToInt32(Item.p.quantity), Convert.ToString(Item.p.expiryDate)),
                    isActive = Item.p.isActive,
                    vendorId = Item.p.vendorId,
                    quantity = Item.p.quantity,
                    recommendation = Item.p.recommendation,
                    isPinned = Item.p.isPinned,
                    categoryId = Item.p.categoryId,
                    shareFacebookCount = Item.p.shareFacebookCount,
                    shareInstagramCount = Item.p.shareInstagramCount,
                    shareTwitterCount = Item.p.shareTwitterCount,
                    price = Item.p.price,
                    useCount = Item.p.useCount,
                    viewCount = Item.p.viewCount,
                    recommendationCount = Item.p.recommendationCount,
                    promotionImage = Item.v.logoImg,
                    companyName = Item.v.companyName,
                    fullDescription = Item.v.fullDescription,
                    latitude = Item.v.latitude,
                    longitude = Item.v.longitude,
                    logoimg = Item.v.logoImg,
                    fromday = fromday(Item.v.vendorId),
                    today = today(Item.v.vendorId),
                    fromtime = fromtime(Item.v.vendorId),
                    totime = totime(Item.v.vendorId),
                    isMore = isMore

                });
            }
            return promotionsUserEntity;
        }

        public IEnumerable<VendorEntity> GetMapLocation()
        {
            var isActivePromotion = _unitOfWork.PromotionCodeRepository.GetByCondition(a => a.isActive.Equals(true) && a.isDeleted == false).Select(v => v.vendorId).ToList();
            var vendors = _unitOfWork.VendorRepository.GetByCondition(z => isActivePromotion.Contains(z.vendorId)).ToList();
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

        #endregion

    }
}
