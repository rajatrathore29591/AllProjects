using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data.Entity.Validation;
using TV.CraveASAP.DataModel.GenericRepository;

namespace TV.CraveASAP.DataModel.UnitOfWork
{
    /// <summary>
    /// Unit of Work class responsible for DB transactions
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        #region Private member variables...

        private DB_9D9221_eddeeEntities _context = null;
        private GenericRepository<User> _userRepository;
        private GenericRepository<Subscribe> _subscribeRepository;
        private GenericRepository<Vendor> _vendorRepository;
        private GenericRepository<DynamicHTMLContent> _dynamicHTMLContent;
        private GenericRepository<UserLocation> _userLocationRepository;
        private GenericRepository<PromotionCode> _adminManageActivePromotionsRepository;
        private GenericRepository<PromotionCode> _vendorPromotionRepository;
        private GenericRepository<PointsConfiguration> _pointConfigurationRepository;
        private GenericRepository<VendorBranch> _vendorBranchRepository;
        private GenericRepository<Banner> _bannerRepository;
        private GenericRepository<Category> _categoryRepository;
        private GenericRepository<SubCategory> _subCategoryRepository;
        private GenericRepository<OptionalCategory> _optionalCategoryRepository;
        private GenericRepository<AppDefaultLandingPage> _appDefaultLandingPageRepository;
        private GenericRepository<UserPromotion> _userPromotionRepository;
        private GenericRepository<UserPromotionSkip> _userPromotionSkipRepository;
        private GenericRepository<UserPrefrence> _userPrefrenceRepository;
        private GenericRepository<OpeningHour> _openingHourRepository;
        private GenericRepository<UserFavourite> _userFavouriteRepository;
        private GenericRepository<UserReward> _userRewardRepository;
        private GenericRepository<Reward> _rewardRepository;
        private GenericRepository<Point> _pointRepository;
        private GenericRepository<ContentManagement> _contentMgtRepository;
        private GenericRepository<SuperAdminLogin> _superAdminLoginRepository;
        private GenericRepository<Configuration> _configurationRepository;
        private GenericRepository<OAuth> _oAuthRepository;
        private GenericRepository<PredictiveNotification> _predictiveNotificationRepository;
        
        #endregion

        public UnitOfWork()
        {
            _context = new DB_9D9221_eddeeEntities();
            
        }
      
        #region Public Repository Creation properties...

        /// <summary>
        /// Get/Set Property for product repository.
        /// </summary>
        //public GenericRepository<Restaurant> RestaurantRepository
        //{
        //    get
        //    {
        //        if (this._restaurantRepository == null)
        //            this._restaurantRepository = new GenericRepository<Restaurant>(_context);
        //        return _restaurantRepository;
        //    }
        //}


        ///
        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        /// 

        public GenericRepository<SuperAdminLogin> SuperAdminLoginRepository
        {
            get
            {
                if (this._superAdminLoginRepository == null)
                    this._superAdminLoginRepository = new GenericRepository<SuperAdminLogin>(_context);
                return _superAdminLoginRepository;
            }
        }

        public GenericRepository<PredictiveNotification> PredictiveNotificationRepository
        {
            get
            {
                if (this._predictiveNotificationRepository == null)
                    this._predictiveNotificationRepository = new GenericRepository<PredictiveNotification>(_context);
                return _predictiveNotificationRepository;
            }
        }

        public GenericRepository<User> UserRepository
        {
            get
            {
                if (this._userRepository == null)
                    this._userRepository = new GenericRepository<User>(_context);
                return _userRepository;
            }
        }

        public GenericRepository<UserReward> UserRewardRepository
        {
            get
            {
                if (this._userRewardRepository == null)
                    this._userRewardRepository = new GenericRepository<UserReward>(_context);
                return _userRewardRepository;
            }
        }


        public GenericRepository<OAuth> OAuthRepository
        {
            get
            {
                if (this._oAuthRepository == null)
                    this._oAuthRepository = new GenericRepository<OAuth>(_context);
                return _oAuthRepository;
            }
        }

        public GenericRepository<Configuration> ConfigurationRepository
        {
            get
            {
                if (this._configurationRepository == null)
                    this._configurationRepository = new GenericRepository<Configuration>(_context);
                return _configurationRepository;
            }
        }

        public GenericRepository<Reward> RewardRepository
        {
            get
            {
                if (this._rewardRepository == null)
                    this._rewardRepository = new GenericRepository<Reward>(_context);
                return _rewardRepository;
            }
        }

        public GenericRepository<OpeningHour>OpeningHourRepository
        {
            get
            {
                if (this._openingHourRepository == null)
                    this._openingHourRepository = new GenericRepository<OpeningHour>(_context);
                return _openingHourRepository;
            }
        }

        public GenericRepository<UserFavourite> UserFavoriteRepository
        {
            get
            {
                if (this._userFavouriteRepository == null)
                    this._userFavouriteRepository = new GenericRepository<UserFavourite>(_context);
                return _userFavouriteRepository;

            }
        }

        public GenericRepository<Category> CategoryRepository
        {
            get
            {
                if (this._categoryRepository == null)
                    this._categoryRepository = new GenericRepository<Category>(_context);
                return _categoryRepository;
            }
        }

        public GenericRepository<SubCategory> SubCategoryRepository
        {
            get
            {
                if (this._subCategoryRepository == null)
                    this._subCategoryRepository = new GenericRepository<SubCategory>(_context);
                return _subCategoryRepository;
            }
        }

        public GenericRepository<OptionalCategory> OptionalCategoryRepository
        {
            get
            {
                if (this._optionalCategoryRepository == null)
                    this._optionalCategoryRepository = new GenericRepository<OptionalCategory>(_context);
                return _optionalCategoryRepository;
            }
        }

        public GenericRepository<Banner> BannerRepository
        {
            get
            {
                if (this._bannerRepository == null)
                    this._bannerRepository = new GenericRepository<Banner>(_context);
                return _bannerRepository;
            }
        }

        public GenericRepository<VendorBranch> VendorBranchRepository
        {
            get
            {
                if (this._vendorBranchRepository == null)
                    this._vendorBranchRepository = new GenericRepository<VendorBranch>(_context);
                return _vendorBranchRepository;
            }
        }

        public GenericRepository<Subscribe> SubscribeRepository
        {
            get
            {
                if (this._subscribeRepository == null)
                    this._subscribeRepository = new GenericRepository<Subscribe>(_context);
                return _subscribeRepository;
            }
        }

       

        public GenericRepository<AppDefaultLandingPage> AppDefaultLandingPageRepository
        {
            get
            {
                if (this._appDefaultLandingPageRepository == null)
                    this._appDefaultLandingPageRepository = new GenericRepository<AppDefaultLandingPage>(_context);
                return _appDefaultLandingPageRepository;
            }
        }

        public GenericRepository<Vendor> VendorRepository
        {
            get
            {
                if (this._vendorRepository == null)
                    this._vendorRepository = new GenericRepository<Vendor>(_context);
                return _vendorRepository;
            }
        }

      

        public GenericRepository<DynamicHTMLContent> DynamicHTMLContentRepository
        {
            get
            {
                if (this._dynamicHTMLContent == null)
                    this._dynamicHTMLContent = new GenericRepository<DynamicHTMLContent>(_context);
                return _dynamicHTMLContent;
            }
        }

       
     
      
   
     
        public GenericRepository<UserPrefrence> UserPrefrenceRepository
        {
            get
            {
                if (this._userPrefrenceRepository == null)
                    this._userPrefrenceRepository = new GenericRepository<UserPrefrence>(_context);
                return _userPrefrenceRepository;

            }
        }
       
   
    
        public GenericRepository<UserLocation> UserLocationRepository
        {
            get
            {
                if (this._userLocationRepository == null)
                    this._userLocationRepository = new GenericRepository<UserLocation>(_context);
                return _userLocationRepository;

            }
        }

        public GenericRepository<PromotionCode> VendorPromotionRepository
        {
            get
            {
                if (this._vendorPromotionRepository == null)
                    this._vendorPromotionRepository = new GenericRepository<PromotionCode>(_context);
                return _vendorPromotionRepository;

            }
        }
        public GenericRepository<PromotionCode> PromotionCodeRepository
        {
            get
            {
                if (this._vendorPromotionRepository == null)
                    this._vendorPromotionRepository = new GenericRepository<PromotionCode>(_context);
                return _vendorPromotionRepository;

            }
        }

        public GenericRepository<UserPromotion> UserPromotionRepository
        {
            get
            {
                if (this._userPromotionRepository == null)
                    this._userPromotionRepository = new GenericRepository<UserPromotion>(_context);
                return _userPromotionRepository;

            }
        }
        public GenericRepository<UserPromotionSkip> UserPromotionSkipRepository
        {
            get
            {
                if (this._userPromotionSkipRepository == null)
                    this._userPromotionSkipRepository = new GenericRepository<UserPromotionSkip>(_context);
                return _userPromotionSkipRepository;

            }
        }
        public GenericRepository<PointsConfiguration> PointConfigurationRepository
        {
            get
            {
                if (this._pointConfigurationRepository == null)
                    this._pointConfigurationRepository = new GenericRepository<PointsConfiguration>(_context);
                return _pointConfigurationRepository;
            }
        }

        public GenericRepository<Point> PointRepository
        {
            get
            {
                if (this._pointRepository == null)
                    this._pointRepository = new GenericRepository<Point>(_context);
                return _pointRepository;
            }
        }

        

        public GenericRepository<ContentManagement> ContentMgtRepository
        {
            get
            {
                if (this._contentMgtRepository == null)
                    this._contentMgtRepository = new GenericRepository<ContentManagement>(_context);
                return _contentMgtRepository;

            }
        }


        #region Admin Repository
        ///////////////////////////////  Manage Promotion Repository ////////////////////
        public GenericRepository<PromotionCode> AdminManageActivePromotionRepository
        {
            get
            {
                if (this._adminManageActivePromotionsRepository == null)
                    this._adminManageActivePromotionsRepository = new GenericRepository<PromotionCode>(_context);
                return _adminManageActivePromotionsRepository;

            }
        }
        #endregion

        #endregion

        #region Public member methods...
        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }

        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false; 
        #endregion

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        } 
        #endregion
    }
}