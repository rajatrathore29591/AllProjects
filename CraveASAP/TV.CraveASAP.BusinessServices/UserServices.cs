using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using TV.CraveASAP.BusinessEntities;
using TV.CraveASAP.DataModel;
using TV.CraveASAP.DataModel.UnitOfWork;
using TV.CraveASAP.BusinessServices.Interfaces;
using System.Web.Hosting;
using System.IO;
using System;
using TV.CraveASAP.BusinessServices.HelperClass;
using TV.Yokal.BusinessServices.HelperClass;
using System.Web;

namespace TV.CraveASAP.BusinessServices
{
    public class UserServices : IUserServices
    {
        OAuthServices oAuthServices = new OAuthServices();
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public UserServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public UserEntity GetUserById(int userId)
        {
            var user = _unitOfWork.UserRepository.GetByID(userId);
            if (user != null)
            {
                Mapper.CreateMap<User, UserEntity>();
                Mapper.CreateMap<UserPromotion, UserPromotionEntity>();
                Mapper.CreateMap<UserReward, UserRewardEntity>();
                Mapper.CreateMap<UserLocation, UserLocationEntity>();
                Mapper.CreateMap<UserPrefrence, UserPrefrencesEntity>();
                var userModel = Mapper.Map<User, UserEntity>(user);
                return userModel;
            }
            return null;
        }

        public IEnumerable<RewardEntity> GetAllReward(UserEntity userEntity)
        {
            List<RewardEntity> rewardEntity = new List<RewardEntity>();
            var userPoint = _unitOfWork.UserRepository.GetByID(userEntity.userId);
            var user = _unitOfWork.UserRewardRepository.GetByCondition(d => d.userId == Convert.ToInt32(userEntity.userId)).ToList();
            var reward = _unitOfWork.RewardRepository.GetByCondition(z => z.endDate > DateTime.Now).GroupJoin(user, r => r.rewardId, u => u.rewardId, (r, u) => new { r, u }).OrderByDescending(b => b.r.isSpecial).ToList();
            var temp = reward.Where(p => p.r.isSpecial == true && p.r.type.Equals("Prem")).ToList();
            temp.AddRange(reward.Where(p => p.r.type.Equals("Prem")).ToList()); temp.AddRange(reward.Where(p => p.r.isSpecial == true && p.r.type.Equals("Gold")).ToList());
            temp.AddRange(reward.Where(p => p.r.type.Equals("Gold")).ToList());
            reward = temp.Distinct().ToList();
            if (userPoint.points < 1000)
            {
                reward = reward.Where(x => x.r.type == "Gold").ToList();
            }

            if (reward.Count() > 0 && user.Count > 0)
            {
                foreach (var Item in reward)
                {
                    if (Item.u.Count() > 0)
                    {
                        int id = Convert.ToInt32(Item.u.FirstOrDefault().userId);
                        if (id != 0)
                        {
                            using (var scope = new TransactionScope())
                            {

                                var Id = Item.u.FirstOrDefault().userId;
                                var userReward = _unitOfWork.UserRewardRepository.GetByCondition(d => d.userId == Convert.ToInt32(Id) && d.rewardId == Item.r.rewardId).FirstOrDefault();
                                DateTime time = Convert.ToDateTime(Item.u.FirstOrDefault().useTime);
                                var expiryHoursLater = time.AddHours(Convert.ToInt32(Item.r.expiryHours));
                                TimeSpan expiryTS = DateTime.UtcNow.AddHours(7.0) - Convert.ToDateTime(expiryHoursLater);
                                if (expiryTS.Hours >= 0 && expiryTS.Minutes >= 0)
                                {
                                    userReward.isUsed = true;
                                    _unitOfWork.UserRewardRepository.Update(userReward);
                                    _unitOfWork.Save();
                                    scope.Complete();
                                }
                                else
                                {
                                    userReward.isUsed = false;
                                    _unitOfWork.UserRewardRepository.Update(userReward);
                                    _unitOfWork.Save();
                                    scope.Complete();
                                }

                            }

                            using (var scope = new TransactionScope())
                            {
                                var Id = Item.u.FirstOrDefault().userId;
                                var userReward = _unitOfWork.UserRewardRepository.GetByCondition(d => d.userId == Convert.ToInt32(Id) && d.rewardId == Item.r.rewardId).FirstOrDefault();
                                if (userReward.isUsed == true)
                                {
                                    DateTime time = Convert.ToDateTime(Item.u.FirstOrDefault().useTime);
                                    var availabiltyHoursLater = time.AddDays(Convert.ToInt32(Item.r.nextAvailability));
                                    TimeSpan availabiltyTS = DateTime.UtcNow.AddHours(7.0) - Convert.ToDateTime(availabiltyHoursLater);
                                    if (availabiltyTS.Days >= 0 && availabiltyTS.Minutes >= 0)
                                    {

                                        _unitOfWork.UserRewardRepository.Delete(userReward);
                                        _unitOfWork.Save();
                                        scope.Complete();

                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (reward.Count() > 0)
            {
                foreach (var Item in reward)
                {
                    TimeSpan ts = Convert.ToDateTime(Item.r.endDate) - Convert.ToDateTime(userEntity.currentdate);
                    DateTime UTCTime = System.DateTime.UtcNow;
                    DateTime dates2 = UTCTime.AddHours(7.0);
                    if (ts.Days >= 0)
                    {
                        rewardEntity.Add(new RewardEntity()
                        {
                            rewardId = Item.r.rewardId,
                            rewardName = Item.r.rewardName,
                            description = Item.r.description,
                            image = Item.r.image,
                            platform = Item.r.platform,
                            code = Item.r.code,
                            endDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.r.endDate),
                            endDate1 = String.Format("{0:dd/MM/yyyy HH:mm:ss}", Item.r.endDate),
                            expiryHours = Item.r.expiryHours,
                            isSpecial = Item.r.isSpecial == null ? false : Item.r.isSpecial,
                            nextAvailability = Item.r.nextAvailability,
                            type = Item.r.type,
                            usedCount = Item.r.usedCount == null ? 0 : Item.r.usedCount,
                            isActive = Item.r.isActive,
                            language = Item.r.language,
                            point = Item.r.point,
                            isUsed = Item.u.Select(z => z.isUsed).FirstOrDefault() == null ? false : Item.u.Select(z => z.isUsed).FirstOrDefault(),
                            userRewardId = Item.u.Select(z => z.userRewardId).FirstOrDefault() == null ? 0 : Item.u.Select(z => z.userRewardId).FirstOrDefault(),
                            useTime = Item.u.Select(z => z.useTime).FirstOrDefault() == null ? "0" : String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.u.Select(z => z.useTime).FirstOrDefault()),
                            useTime1 = Item.u.Select(z => z.useTime).FirstOrDefault() == null ? "0" : String.Format("{0:dd/MM/yyyy HH:mm:ss}", Item.u.Select(z => z.useTime).FirstOrDefault()),
                            hyperLink = Item.r.link == null ? "" : Item.r.link,
                            serverTime = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2),
                            serverTime1 = String.Format("{0:dd/MM/yyyy HH:mm:ss}", dates2)

                        });
                    }
                    else
                    {
                        using (var scope = new TransactionScope())
                        {
                            var success = false;
                            var reawrdID = _unitOfWork.RewardRepository.GetByCondition(d => d.rewardId == Item.r.rewardId && d.isActive == false || d.isActive == null).FirstOrDefault();
                            if (reawrdID != null)
                            {
                                reawrdID.isActive = false;
                                _unitOfWork.RewardRepository.Update(reawrdID);
                                _unitOfWork.Save();
                                scope.Complete();
                                success = true;
                            }
                        }
                    }
                }
            }

            return rewardEntity;
        }

        public IEnumerable<PointEntity> GetAllPoint()
        {
            var point = _unitOfWork.PointRepository.GetAll().ToList();
            if (point.Count() > 0)
            {
                Mapper.CreateMap<Point, PointEntity>();
                var pointModel = Mapper.Map<List<Point>, List<PointEntity>>(point);
                return pointModel;
            }
            return null;
        }

        /// <summary>
        /// Fetches all the products.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserEntity> GetAllUsers()
        {

            var users = _unitOfWork.UserRepository.GetAll().ToList();
            if (users.Any())
            {
                Mapper.CreateMap<User, UserEntity>();
                Mapper.CreateMap<UserPromotion, UserPromotionEntity>();
                Mapper.CreateMap<UserReward, UserRewardEntity>();
                Mapper.CreateMap<UserLocation, UserLocationEntity>();
                Mapper.CreateMap<UserPrefrence, UserPrefrencesEntity>();
                var usersModel = Mapper.Map<List<User>, List<UserEntity>>(users);
                return usersModel;
            }
            return null;
        }

        /// <summary>
        /// Creates a product
        /// </summary>
        /// <param name="productEntity"></param>
        /// <returns></returns>
        public int CreateUser(UserEntity userEntity)
        {
            using (var scope = new TransactionScope())
            {
                var user = new User
                {
                    firstName = userEntity.firstName,
                    lastName = userEntity.lastName,
                    email = userEntity.email,
                };
                _unitOfWork.UserRepository.Insert(user);
                _unitOfWork.Save();
                scope.Complete();
                return user.userId;
            }
        }

        public bool UpdateUser(int userId, UserEntity userEntity)
        {
            var success = false;
            if (userEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var user = _unitOfWork.UserRepository.GetByID(userId);
                    if (user != null)
                    {
                        user.firstName = userEntity.firstName;
                        user.lastName = userEntity.lastName;
                        user.email = userEntity.email;

                        _unitOfWork.UserRepository.Update(user);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public bool DeleteUser(int userId)
        {
            var success = false;
            if (userId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var user = _unitOfWork.UserRepository.GetByID(userId);
                    if (user != null)
                    {
                        _unitOfWork.UserRepository.Delete(user);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public UserEntity SkipUser(UserEntity UserEntity)
        {
            UserEntity.oAuthkey = oAuthServices.MakeCallSignature("0", UserEntity.deviceToken, "User", "EddeeDevice#");
            return UserEntity;
        }

        public UserEntity UserLogIn(UserEntity UserEntity)
        {
            if (UserEntity.userId == 0 || UserEntity.userId == null)
            {
                var userData = ChkExitUser(UserEntity);
                if (userData == null)
                {
                    var Id = InsertUserLogIn(UserEntity);
                    UpdateImage(UserEntity, Id);
                    UserEntity.userId = Id;
                    var loginModels = ChkExitUser(UserEntity);

                    loginModels.oAuthkey = oAuthServices.MakeCallSignature(Convert.ToString(loginModels.userId), loginModels.deviceToken, "User", "EddeeDevice#");
                    return loginModels;
                }
                else
                {
                    ManageMultipleDevices manageMultipleDevive = new ManageMultipleDevices();
                    var dvcTok = manageMultipleDevive.InsertDeviceToken(UserEntity.deviceToken, UserEntity.deviceType, "UserEntity", userData.userId.ToString());
                    var checkPlt = manageMultipleDevive.InsertDevicePlt(UserEntity.deviceToken, UserEntity.deviceType, "UserEntity", userData.userId.ToString());
                    using (var scope = new TransactionScope())
                    {
                        var user = _unitOfWork.UserRepository.GetByID(userData.userId);
                        if (user != null)
                        {
                            user.deviceToken = dvcTok;
                            user.devicePlatform = checkPlt;
                            //user.devicePlatform = UserEntity.deviceType;
                            _unitOfWork.UserRepository.Update(user);
                            _unitOfWork.Save();
                            scope.Complete();

                        }
                    }
                    userData.oAuthkey = oAuthServices.MakeCallSignature(Convert.ToString(userData.userId), userData.deviceToken, "User", "EddeeDevice#");
                    return userData;
                }
            }
            else
            {
                using (var scope = new TransactionScope())
                {
                    var user = _unitOfWork.UserRepository.GetByID(UserEntity.userId);
                    if (user != null)
                    {
                        user.language = UserEntity.language;
                        _unitOfWork.UserRepository.Update(user);
                        _unitOfWork.Save();
                        scope.Complete();
                    }
                }
                var userData = ChkExitUser(UserEntity);
                return userData;
            }
            return null;
        }

        /// <summary>
        /// /////////////////////////// Check Exiting Data /////////////////////////////
        /// </summary>
        /// <param name="UserEntity"></param>
        /// <returns></returns>
        public UserEntity ChkExitUser(UserEntity UserEntity)
        {
            var IsData = _unitOfWork.UserRepository.GetByCondition(x => x.firstName == UserEntity.firstName && x.facebookId == UserEntity.facebookId).FirstOrDefault();
            if (IsData != null)
            {
                Mapper.CreateMap<User, UserEntity>();
                Mapper.CreateMap<UserPromotion, UserPromotionEntity>();
                Mapper.CreateMap<UserReward, UserRewardEntity>();
                Mapper.CreateMap<UserLocation, UserLocationEntity>();
                Mapper.CreateMap<UserPrefrence, UserPrefrencesEntity>();
                var UserLogInfoModel = Mapper.Map<User, UserEntity>(IsData);
                return UserLogInfoModel;
            }
            return null;
        }

        /// <summary>
        /// //////////////////  Insert User LogIn Details  ///////////////////
        /// </summary>
        /// <param name="HTMLContentEntity"></param>
        /// <returns></returns>
        /// 

        public int InsertUserLogIn(UserEntity UserEntity)
        {
            using (var scope = new TransactionScope())
            {
                var _UserEntity = new User
                {

                    firstName = UserEntity.firstName,
                    lastName = UserEntity.lastName,
                    gender = UserEntity.gender,
                    email = UserEntity.email,
                    createdDate = UserEntity.createdDate,
                    facebookId = UserEntity.facebookId,
                    facebookInviteDate = UserEntity.facebookInviteDate,
                    facebookInviteCount = UserEntity.facebookInviteCount,
                    instagramInviteDate = UserEntity.instagramInviteDate,
                    instagramInviteCount = UserEntity.instagramInviteCount,
                    twitterInviteDate = UserEntity.twitterInviteDate,
                    twitterInviteCount = UserEntity.twitterInviteCount,
                    language = UserEntity.language,
                    deviceToken = UserEntity.deviceToken,
                    devicePlatform = UserEntity.deviceType

                };
                _unitOfWork.UserRepository.Insert(_UserEntity);
                _unitOfWork.Save();
                var user = _unitOfWork.UserRepository.GetByCondition(u => u.userId == _UserEntity.userId).FirstOrDefault();
                var pointConfig = _unitOfWork.PointConfigurationRepository.GetByCondition(d => d.ptConfigurationId == 5).FirstOrDefault();
                if (pointConfig != null)
                {
                    user.points = pointConfig.pointsEarned;
                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Save();
                }
                scope.Complete();
                return _UserEntity.userId;
            }
        }

        public bool UpdateImage(UserEntity UserEntity, int MaxId)
        {
            var user1 = _unitOfWork.UserRepository.GetByID(MaxId);
            var imgPath = base64ToImage(UserEntity, MaxId, user1.profilePicture);
            using (var scope = new TransactionScope())
            {
                var user = _unitOfWork.UserRepository.GetByID(MaxId);
                if (user != null)
                {
                    user.profilePicture = imgPath;

                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Save();
                    scope.Complete();
                }
                return true;
            }
            return false;
        }

        public string base64ToImage(UserEntity UserEntity, int MaxId, string oldImage)
        {
            string filePath = string.Empty;
            string image = string.Empty;
            var path = DateTime.Now;
            var data = String.Format("{0:d/M/yyyy HH:mm:ss}", path);
            data = data.Replace(@"/", "").Trim(); data = data.Replace(@":", "").Trim(); data = data.Replace(" ", String.Empty);
            if (!string.IsNullOrEmpty(UserEntity.profilePicture))
            {
                filePath = HostingEnvironment.MapPath("~/Pictures/");
                image = MaxId + "_FaceBookPic" + data + ".png";
                if (File.Exists(filePath + oldImage))
                {
                    System.IO.File.Delete((filePath + oldImage));
                }
                byte[] bytes = System.Convert.FromBase64String(UserEntity.profilePicture);
                FileStream fs = new FileStream(filePath + image, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
            return image;
        }

        /// <summary>
        /// /////////////////////////////////////// Get All Data User Login  ////////////////////////////////
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DynamicHTMLContentEntity> GetAllUserLogIn()
        {
            var userLogIn = _unitOfWork.DynamicHTMLContentRepository.GetAll().ToList();
            if (userLogIn.Any())
            {
                Mapper.CreateMap<DynamicHTMLContent, DynamicHTMLContentEntity>();
                var HTMLContentModel = Mapper.Map<List<DynamicHTMLContent>, List<DynamicHTMLContentEntity>>(userLogIn);
                return HTMLContentModel;
            }
            return null;
        }

        public IEnumerable<UserLocationEntity> UpdateMyLocation(IEnumerable<UserLocationEntity> userLocationEntity)
        {
            int userId = 0;
            var value = "";

            DateTime UTCTime = System.DateTime.UtcNow; DateTime currentDate = UTCTime.AddHours(7.0);
            foreach (UserLocationEntity item in userLocationEntity)
            {
                userId = item.fkUser;

                if (item.userLocationId == null || item.userLocationId == 0 && item.isCurrent == false)
                {
                    var location = _unitOfWork.UserLocationRepository.GetByCondition(d => d.locationAddress == item.locationAddress.Trim() && d.fkUser == item.fkUser && d.isDeleted == false).FirstOrDefault();
                    if (location == null)
                    {
                        using (var scope = new TransactionScope())
                        {
                            var userlocation = new UserLocation
                            {
                                fkUser = item.fkUser,
                                isDefault = item.isDefault,
                                mealTime = item.mealTime,
                                locationAddress = item.locationAddress,
                                locationLat = item.locationLat,
                                locationLong = item.locationLong,
                                isCurrent = item.isCurrent,
                                isDeleted = false

                            };
                            _unitOfWork.UserLocationRepository.Insert(userlocation);
                            _unitOfWork.Save();
                            scope.Complete();
                            var userLocatinId = _unitOfWork.UserLocationRepository.GetByID(userlocation.userLocationId).userLocationId;
                            value = userLocatinId.ToString();
                        }
                    }
                    else
                    {
                        value = "already exist";
                    }

                }

                else if (item.isCurrent == true)
                {
                    using (var scope = new TransactionScope())
                    {
                        var userlocation = _unitOfWork.UserLocationRepository.GetByID(item.userLocationId);

                        if (userlocation != null)
                        {
                            userlocation.visitedDatetime = currentDate;
                            _unitOfWork.UserLocationRepository.Update(userlocation);
                            _unitOfWork.Save();

                            scope.Complete();
                            var userLocatinId = _unitOfWork.UserLocationRepository.GetByID(userlocation.userLocationId).userLocationId;
                            value = userLocatinId.ToString();
                        }

                    }
                }

                else
                {
                    using (var scope = new TransactionScope())
                    {
                        var userlocation = _unitOfWork.UserLocationRepository.GetByID(item.userLocationId);

                        if (userlocation != null)
                        {
                            userlocation.fkUser = item.fkUser;
                            userlocation.isDefault = item.isDefault;
                            userlocation.mealTime = item.mealTime.Trim();
                            userlocation.locationAddress = item.locationAddress.Trim();
                            userlocation.locationLat = item.locationLat.Trim();
                            userlocation.locationLong = item.locationLong.Trim();
                            userlocation.isCurrent = false;
                            _unitOfWork.UserLocationRepository.Update(userlocation);
                            _unitOfWork.Save();

                            scope.Complete();
                            var userLocatinId = _unitOfWork.UserLocationRepository.GetByID(userlocation.userLocationId).userLocationId;
                            value = userLocatinId.ToString();
                        }
                    }
                }
            }
            return GetMyLocation(userId);
        }

        public bool DeleteMyLocation(UserLocationEntity userLocationEntity)
        {

            bool success = false;
            if (userLocationEntity.userLocationId != null)
            {
                using (var scope = new TransactionScope())
                {
                    var userLocationId = _unitOfWork.UserLocationRepository.GetByID(userLocationEntity.userLocationId);
                    if (userLocationId != null)
                    {
                        userLocationId.isDeleted = true;
                        _unitOfWork.UserLocationRepository.Update(userLocationId);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public IEnumerable<UserLocationEntity> GetMyLocation(int userId)
        {
            List<UserLocationEntity> userLocationEntity = new List<UserLocationEntity>();
            if (userId == 0)
            {
                userLocationEntity.Add(new UserLocationEntity()
                {
                    fkUser = 0
                });

                return userLocationEntity;
            }

            var location = _unitOfWork.UserLocationRepository.GetByCondition(d => d.fkUser == userId && d.isDeleted == false).OrderByDescending(d => d.visitedDatetime).OrderByDescending(d => d.isDefault).ToList();
            if (location.Count() > 0)
            {
                foreach (var Item in location)
                {
                    userLocationEntity.Add(new UserLocationEntity()
                    {
                        userLocationId = Item.userLocationId,
                        fkUser = Item.fkUser,
                        isDefault = Item.isDefault,
                        mealTime = Item.mealTime,
                        locationAddress = Item.locationAddress,
                        locationLat = Item.locationLat,
                        locationLong = Item.locationLong,
                        isCurrent = Item.isCurrent
                    });
                }
                return userLocationEntity;
            }

            return null;
        }

        public UserEntity UserEntity(int UserID)
        {
            var IsData = _unitOfWork.UserRepository.GetByID(UserID);
            if (IsData != null)
            {
                Mapper.CreateMap<User, UserEntity>();
                Mapper.CreateMap<UserPromotion, UserPromotionEntity>();
                Mapper.CreateMap<UserReward, UserRewardEntity>();
                Mapper.CreateMap<UserLocation, UserLocationEntity>();
                Mapper.CreateMap<UserPrefrence, UserPrefrencesEntity>();
                var UserLogInfoModel = Mapper.Map<User, UserEntity>(IsData);
                return UserLogInfoModel;
            }
            return null;
        }

        public IEnumerable<PromotionsUserEntity> GetUserPromotionByUserId(int UserID, string deviceToken, string devicePlatform)
        {
            List<PromotionsUserEntity> promotionsUserEntity = new List<PromotionsUserEntity>();
            if (UserID == 0)
            {
                var user = _unitOfWork.UserPromotionSkipRepository.GetByCondition(d => d.deviceToken == deviceToken && d.devicePlatform == devicePlatform && d.isUsed == true).ToList();

                var promotion = _unitOfWork.PromotionCodeRepository.GetByCondition(d => d.isDeleted == false).Join(user, p => p.promotionCodeId, u => u.promotionId, (p, u) => new { p, u }).ToList();
                if (promotion.Count() > 0)
                {
                    foreach (var Item in promotion)
                    {
                        var vendor = _unitOfWork.VendorRepository.GetByCondition(d => d.vendorId == Item.p.vendorId).FirstOrDefault();
                        promotionsUserEntity.Add(new PromotionsUserEntity()
                        {
                            promotionCodeId = Item.p.promotionCodeId,
                            code = Item.p.code,
                            name = Item.p.name,
                            descriptionEnglish = Item.p.descriptionEnglish == null ? "" : Item.p.descriptionEnglish,
                            descriptionThai = Item.p.descriptionThai == null ? "" : Item.p.descriptionThai,
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
                            useCount = Item.p.useCount != null ? Item.p.useCount : 0,
                            viewCount = Item.p.viewCount != null ? Item.p.viewCount : 0,
                            recommendationCount = Item.p.recommendationCount,
                            promotionImage = Item.p.promotionImage,
                            companyName = vendor.companyName,
                            latitude = vendor.latitude,
                            longitude = vendor.longitude,
                            isFavourite = false,
                            favouriteId = "",
                            fromday = "",
                            fromtime = "",
                            today = "",
                            totime = "",
                            logoimg = vendor.logoImg,

                        });
                    }
                }
            }

            else
            {
                var user = _unitOfWork.UserPromotionRepository.GetByCondition(d => d.userId == UserID && d.isDeleted == false && d.isUsed == true).ToList();
                var promotion = _unitOfWork.PromotionCodeRepository.GetByCondition(d => d.isDeleted == false).Join(user, p => p.promotionCodeId, u => u.promotionId, (p, u) => new { p, u }).ToList();
                if (promotion.Count() > 0)
                {
                    foreach (var Item in promotion)
                    {
                        var vendor = _unitOfWork.VendorRepository.GetByCondition(d => d.vendorId == Item.p.vendorId).FirstOrDefault();
                        promotionsUserEntity.Add(new PromotionsUserEntity()
                        {
                            promotionCodeId = Item.p.promotionCodeId,
                            code = Item.p.code,
                            name = Item.p.name,
                            descriptionEnglish = Item.p.descriptionEnglish == null ? "" : Item.p.descriptionEnglish,
                            descriptionThai = Item.p.descriptionThai == null ? "" : Item.p.descriptionThai,
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
                            useCount = Item.p.useCount != null ? Item.p.useCount : 0,
                            viewCount = Item.p.viewCount != null ? Item.p.viewCount : 0,
                            recommendationCount = Item.p.recommendationCount,
                            promotionImage = Item.p.promotionImage,
                            companyName = vendor.companyName,
                            latitude = vendor.latitude,
                            longitude = vendor.longitude,
                            isFavourite = false,
                            favouriteId = "",
                            fromday = "",
                            fromtime = "",
                            today = "",
                            totime = "",
                            logoimg = vendor.logoImg,

                        });
                    }
                }
            }
            return promotionsUserEntity;
        }

        public IEnumerable<UserAppEntity> GetUserByUserId(int UserID)
        {
            var IsData = _unitOfWork.UserRepository.GetByID(UserID);
            List<UserAppEntity> userAppEntity = new List<UserAppEntity>();
            if (IsData != null)
            {
                var user = _unitOfWork.UserRepository.GetByCondition(d => d.userId == UserID).ToList();
                var userReward = _unitOfWork.UserRewardRepository.GetAll().Join(user, r => r.userId, u => u.userId, (r, u) => new { r, u });
                var reward = _unitOfWork.RewardRepository.GetAll().Join(userReward, q => q.rewardId, s => s.r.rewardId, (q, s) => new { q, s });

                /*--------------------------------------------------start logic if user not use promotion for 7 days to deducet 100 points if user point greater 500-------------------------------------------------------*/

                if (Convert.ToInt32(user.FirstOrDefault().points) > 500)
                {
                    var userPromotion = _unitOfWork.UserPromotionRepository.GetByCondition(d => d.userId == UserID && d.isDeleted == false && d.useDate >= DateTime.UtcNow.AddHours(7.0).AddDays(7.0)).OrderByDescending(d => d.useDate).FirstOrDefault();

                    if (userPromotion != null)
                    {
                        using (var scope = new TransactionScope())
                        {
                            var userpoint = _unitOfWork.UserRepository.GetByCondition(d => d.userId == UserID).FirstOrDefault();
                            userpoint.points -= 100;
                            _unitOfWork.UserRepository.Update(userpoint);
                            _unitOfWork.Save();
                            scope.Complete();
                        }
                    }
                }

                /*------------------------------------------------End logic if user not use promotion for 7 days to deducet 100 points if user point greater 500-------------------------------------------------------*/
                if (reward.Count() > 0)
                {
                    foreach (var Item in reward)
                    {
                        userAppEntity.Add(new UserAppEntity()
                        {
                            firstName = Item.s.u.firstName,
                            lastName = Item.s.u.lastName,
                            profilePicture = Item.s.u.profilePicture,
                            gender = Item.s.u.gender,
                            email = Item.s.u.email,
                            points = Item.s.u.points != null ? Item.s.u.points : 0,
                            language = Item.s.u.language,
                            deviceToken = Item.s.u.deviceToken,
                            userRewardId = Item.s.r.userRewardId,
                            rewardId = Item.s.r.rewardId,
                            isUsed = Item.s.r.isUsed,
                            useTime = Item.s.r.useTime,
                            rewardName = Item.q.rewardName,
                            description = Item.q.description,
                            image = Item.q.image,
                            platform = Item.q.platform,
                            code = Item.q.code,
                            endDate = Item.q.endDate,
                            expiryHours = Item.q.expiryHours,
                            isSpecial = Item.q.isSpecial,
                            nextAvailability = Item.q.nextAvailability,
                            type = Item.q.type,
                            usedCount = Item.q.usedCount,
                            status = Item.q.isActive.ToString(),
                            earnedPoint = earnPoint()
                        });
                    }
                }
                else
                {
                    foreach (var Item in user)
                    {
                        userAppEntity.Add(new UserAppEntity()
                        {
                            firstName = Item.firstName,
                            lastName = Item.lastName,
                            profilePicture = Item.profilePicture,
                            gender = Item.gender,
                            email = Item.email,
                            points = Item.points,
                            language = Item.language,
                            deviceToken = Item.deviceToken,
                            earnedPoint = earnPoint()
                        });
                    }
                }
                return userAppEntity;
            }
            return userAppEntity;
        }

        public int earnPoint()
        {
            int earnPoint = 0;
            var earnedPoint = _unitOfWork.PointConfigurationRepository.GetByCondition(p => p.ptConfigurationId == 1).ToList();
            if (earnedPoint != null)
            {
                foreach (var item in earnedPoint)
                {
                    earnPoint = Convert.ToInt32(item.pointsEarned);
                }
            }
            return earnPoint;
        }

        public string AddVenueToFavorite(UserFavouriteEntity userFavouriteEntity)
        {
            var success = false;
            var userId = _unitOfWork.UserFavoriteRepository.GetByCondition(d => d.userId == userFavouriteEntity.userId && d.vendorId == userFavouriteEntity.vendorId).ToList();
            ResponseEntity response = new ResponseEntity();
            if (userId.Count <= 0)
            {
                using (var scope = new TransactionScope())
                {
                    var userFavourite = new UserFavourite
                    {
                        vendorId = userFavouriteEntity.vendorId,
                        userId = userFavouriteEntity.userId,
                        isActive = userFavouriteEntity.isActive
                    };
                    _unitOfWork.UserFavoriteRepository.Insert(userFavourite);
                    _unitOfWork.Save();
                    var maxId = _unitOfWork.UserFavoriteRepository.GetByID(userFavourite.favoriteRestaurantId);
                    response.isFavouriteId = maxId.favoriteRestaurantId.ToString();
                    scope.Complete();
                    success = true;
                }
                return response.isFavouriteId;
            }
            else
            {
                var data = _unitOfWork.UserFavoriteRepository.GetByCondition(d => d.userId == userFavouriteEntity.userId && d.vendorId == userFavouriteEntity.vendorId).FirstOrDefault();
                using (var scope = new TransactionScope())
                {

                    if (data != null)
                    {

                        data.isActive = userFavouriteEntity.isActive;
                    }
                    _unitOfWork.UserFavoriteRepository.Update(data);
                    _unitOfWork.Save();
                    scope.Complete();
                    success = true;
                }
                return data.favoriteRestaurantId.ToString();
            }

            return null;
        }

        public bool DeleteVenueToFavorite(UserFavouriteEntity userFavouriteEntity)
        {
            var success = false;

            using (var scope = new TransactionScope())
            {
                var favouriteRestaurantId = _unitOfWork.UserFavoriteRepository.GetByID(userFavouriteEntity.favoriteRestaurantId);
                if (favouriteRestaurantId != null)
                {
                    _unitOfWork.UserFavoriteRepository.Delete(userFavouriteEntity.favoriteRestaurantId);
                    _unitOfWork.Save();
                    scope.Complete();
                    success = true;

                } return success;
            }
        }

        public bool DeletePromotionByUserId(int userId, int promotionId, string devicePlatform, string deviceToken)
        {
            var success = false;

            if (userId > 0 && promotionId == 0)
            {
                var user = _unitOfWork.UserPromotionRepository.GetByCondition(d => d.userId == userId && d.isDeleted == false).ToList();
                using (var scope = new TransactionScope())
                {
                    foreach (var item in user)
                    {
                        if (user != null)
                        {
                            item.isDeleted = true;
                            _unitOfWork.UserPromotionRepository.Update(item);
                            _unitOfWork.Save();
                        }
                    }
                    scope.Complete();
                    success = true;
                }
            }
            if (userId > 0 && promotionId > 0)
            {
                var user = _unitOfWork.UserPromotionRepository.GetByCondition(d => d.userId == userId && d.promotionId == promotionId && d.isDeleted == false).ToList();
                using (var scope = new TransactionScope())
                {
                    foreach (var item in user)
                    {
                        if (user != null)
                        {
                            item.isDeleted = true;
                            _unitOfWork.UserPromotionRepository.Update(item);
                            _unitOfWork.Save();
                        }
                    }
                    scope.Complete();
                    success = true;
                }
            }

            if (deviceToken != null && devicePlatform != null && userId == 0 && promotionId == 0)
            {
                var user = _unitOfWork.UserPromotionSkipRepository.GetByCondition(d => d.devicePlatform == devicePlatform && d.deviceToken == deviceToken).ToList();
                using (var scope = new TransactionScope())
                {
                    foreach (var item in user)
                    {
                        if (user != null)
                        {
                            _unitOfWork.UserPromotionSkipRepository.Delete(item.userPromotionId);
                            _unitOfWork.Save();
                        }
                    }
                    scope.Complete();
                    success = true;
                }
            }

            if (deviceToken != null && devicePlatform != null && userId == 0 && promotionId > 0)
            {
                var user = _unitOfWork.UserPromotionSkipRepository.GetByCondition(d => d.devicePlatform == devicePlatform && d.deviceToken == deviceToken && d.promotionId == promotionId).ToList();
                using (var scope = new TransactionScope())
                {
                    foreach (var item in user)
                    {
                        if (user != null)
                        {
                            _unitOfWork.UserPromotionSkipRepository.Delete(item.userPromotionId);
                            _unitOfWork.Save();
                        }
                    }
                    scope.Complete();
                    success = true;
                }
            }

            return success;
        }

        public bool DeleteUserPromotionByUserID(UserPromotionEntity userpromotionEntity)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                var userId = _unitOfWork.UserPromotionRepository.GetByID(userpromotionEntity.userId);
                if (userId != null)
                {
                    _unitOfWork.UserFavoriteRepository.Delete(userpromotionEntity.userId);
                    _unitOfWork.Save();
                    scope.Complete();
                    success = true;

                } return success;
            }
        }

        public bool AddReward(UserRewardEntity userRewardEntity)
        {
            var success = false;
            var userRewardEntity1 = _unitOfWork.UserRewardRepository.GetByCondition(d => d.rewardId == userRewardEntity.rewardId && d.userId == userRewardEntity.userId).ToList();
            if (userRewardEntity1.Count() == 0)
            {
                using (var scope = new TransactionScope())
                {
                    var userReward = new UserReward
                    {
                        rewardId = userRewardEntity.rewardId,
                        userId = userRewardEntity.userId,
                        useTime = Convert.ToDateTime(userRewardEntity.useTime),
                    };
                    _unitOfWork.UserRewardRepository.Insert(userReward);
                    _unitOfWork.Save();
                    scope.Complete();
                    success = true;
                }
                using (var scope = new TransactionScope())
                {

                    var rewardPoint = _unitOfWork.RewardRepository.GetByID(userRewardEntity.rewardId);
                    var userPoint = _unitOfWork.UserRepository.GetByID(userRewardEntity.userId);
                    if (userPoint != null)
                    {
                        userPoint.points = userPoint.points - Convert.ToInt32(rewardPoint.point);
                    }
                    _unitOfWork.UserRepository.Update(userPoint);
                    _unitOfWork.Save();
                    scope.Complete();
                    success = true;

                }
            }
            return success;
        }

        public bool UpdateReward(UserRewardEntity userRewardEntity, int userRewardId)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                var userReward = _unitOfWork.UserRewardRepository.GetByID(userRewardId);
                if (userReward != null)
                {
                    userReward.rewardId = userRewardEntity.rewardId;
                    userReward.userId = userRewardEntity.userId;
                    userReward.useTime = Convert.ToDateTime(userRewardEntity.useTime);
                }
                _unitOfWork.UserRewardRepository.Update(userReward);
                _unitOfWork.Save();
                scope.Complete();
                success = true;
            }
            return success;
        }

        public IEnumerable<UserFavouriteEntity> UserFavRestaurant(int id)
        {
            var usersFavourite = _unitOfWork.UserFavoriteRepository.GetByCondition(x => x.userId == id).Join(_unitOfWork.VendorRepository.GetAll(), f => f.vendorId, v => v.vendorId, (f, v) => new { f, v });
            usersFavourite = usersFavourite.OrderBy(u => u.v.companyName);
            if (usersFavourite.Count() > 0)
            {
                List<UserFavouriteEntity> userFavouriteEntityList = new List<UserFavouriteEntity>();
                foreach (var item in usersFavourite)
                {
                    userFavouriteEntityList.Add(new UserFavouriteEntity()
                    {
                        favoriteRestaurantId = item.f.favoriteRestaurantId,
                        userId = item.f.userId,
                        vendorId = item.f.vendorId,
                        isActive = item.f.isActive == null ? false : item.f.isActive,
                        companyName = item.v.companyName
                    });

                }
                return userFavouriteEntityList;
            }
            return null;
        }

        public IEnumerable<UserFavouriteEntity> UpdateUserFavouriteRest(int id)
        {
            var usersFavouriteRest = _unitOfWork.UserFavoriteRepository.GetByCondition(x => x.favoriteRestaurantId == id).FirstOrDefault();
            if (usersFavouriteRest != null)
            {
                if (usersFavouriteRest.isActive == true)
                {
                    usersFavouriteRest.isActive = false;
                    _unitOfWork.UserFavoriteRepository.Update(usersFavouriteRest);
                    _unitOfWork.Save();
                }
                else
                {
                    usersFavouriteRest.isActive = true;
                    _unitOfWork.UserFavoriteRepository.Update(usersFavouriteRest);
                    _unitOfWork.Save();
                }
                var usersFavouriteRest1 = _unitOfWork.UserFavoriteRepository.GetByCondition(x => x.favoriteRestaurantId == id).ToList();
                List<UserFavouriteEntity> userFavouriteEntityList = new List<UserFavouriteEntity>();
                foreach (var item in usersFavouriteRest1)
                {
                    userFavouriteEntityList.Add(new UserFavouriteEntity()
                    {
                        userId = item.userId,
                        vendorId = item.vendorId,
                        isActive = item.isActive,
                    });

                }
                return userFavouriteEntityList;
            }
            return null;
        }

        private void DeductPoints(int rewardId, int userId)
        {
            var user = _unitOfWork.UserRepository.GetByID(userId);
            var reward = _unitOfWork.RewardRepository.GetByID(rewardId);
            int pointsToDeduct = Convert.ToInt32(reward.point);
            user.points = pointsToDeduct < user.points ? (user.points - pointsToDeduct) : 0;
            _unitOfWork.UserRepository.Update(user);
        }

    }
}
