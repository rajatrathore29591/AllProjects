using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using TV.CraveASAP.BusinessEntities;
using TV.CraveASAP.BusinessServices;
using TV.CraveASAP.BusinessServices.Interfaces;

namespace CraveASAPRestServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class CraveServices : ICraveServices
    {
        ISubscribeServices _subscribeServices;
        IVendorService _vendorServices;
        IDynamicHTMLContentServices _dynamicHTMLContentServices;
        IUserPromotionsServices _userPromotionsServices;
        IAdminManageActivePromotionsServices _adminManageActivePromotionsServices;
        IVendorPromotionServices _vendorPromotionServices;
        IPointConfigurationServices _pointConfigurationServices;
        IAppDefaultLandingPageServices _appDefaultLandingServices;
        IBannerServices _bannerServices;
        ICategoryServices _iCategoryServices;
        IUserServices _userServices;
        IUserPrefrenceServices _userPrefrenceServices;
        IAdminReward _AdminReward;
        IAdminChartServices _AdminChartServices;
        IPushNotificationServices _PushNotificationServices;

        OAuthServices _OAuthServices = new OAuthServices();
        public CraveServices()
        {
            _subscribeServices = new SubscribeServices();
            _vendorServices = new VendorServices();
            _dynamicHTMLContentServices = new DynamicHTMLContentServices();
            _userPromotionsServices = new UserPromotionsServices();
            _adminManageActivePromotionsServices = new AdminManageActivePromotionsServices();
            _vendorPromotionServices = new VendorPromotionServices();
            _pointConfigurationServices = new PointConfigurationServices();
            _bannerServices = new BannerServices();
            _appDefaultLandingServices = new AppDefaultLandingPageServices();
            _iCategoryServices = new CategoryServices();
            _userServices = new UserServices();
            _userPrefrenceServices = new UserPrefrenceServices();
            _AdminReward = new AdminReward();
            _AdminChartServices = new AdminChartServices();
            _PushNotificationServices = new PushNotificationServices();

        }

        public GetResponseEntity GetUserByUserId(string userId)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                int id = Convert.ToInt32(userId);
                if (_OAuthServices.GetoAuthkey(id, "User", oAuthkey))
                {
                    response.data = _userServices.GetUserByUserId(id);
                    if (response.data.Count() > 0)
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                    return response;
                } response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity GetICrave(string vendorType, string userLocationLat, string userLocationLong, string userId,string page)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                if (_OAuthServices.GetoAuthkey(Convert.ToInt32(userId), "User", oAuthkey))
                {
                    response.data = _userPromotionsServices.GetICrave(Convert.ToInt32(vendorType), userLocationLat, userLocationLong, Convert.ToInt32(userId),Convert.ToInt32(page));
                    if (response.data.Count() > 0)
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                    return response;
                }
                response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity SuperAdminLogin(string emailId, string password)
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data = _vendorServices.SuperAdminLogin(emailId, password);
                if (response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                    response.oAuthkey = _OAuthServices.MakeCallSignature("1", "MMlPoCvDh518sjLTOiyy91KgqJw%3d", "Admin", "EddeeDevice#");
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity VendorLogin(string emailId, string password, string deviceToken, string deviceType)
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                var id = 0;
                var getlogin = _vendorServices.VendorLogin(emailId, password, deviceToken, deviceType);
                response.data = getlogin;
                if (getlogin != null)
                {
                    id = getlogin.Select(z => z._vendorId).FirstOrDefault();
                }
                if (response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                    response.oAuthkey = _OAuthServices.MakeCallSignature(Convert.ToString(id), deviceToken, "Vendor", "EddeeDevice#");
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }

        }

        public ResponseEntity VendorLogout(VendorEntity Vendor)
        {
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                if (_vendorServices.VendorLogout(Vendor))
                {
                    responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                }
                else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
            }
            catch (Exception ex)
            {
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
                throw new FaultException(ex.Message);
            }
            return responseEntity;
        }

        public UserEntity UserLogIn(UserEntity UserLogIn)
        {
            try
            {
                return _userServices.UserLogIn(UserLogIn);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public UserEntity SkipUser(UserEntity UserLogIn)
        {
            try
            {
                return _userServices.SkipUser(UserLogIn);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public ResponseEntity ForgotVendorPassword(VendorEntity vendorEntity)
        {
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                    if (_vendorServices.ForgotVendorPassword(vendorEntity.email))
                    {
                        responseEntity.responseMessage = "success"; responseEntity.responseCode = 1; responseEntity.isUserActive = "true";
                    }
                    else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; responseEntity.isUserActive = "false"; }
            }

            catch (Exception ex)
            {
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
                throw new FaultException(ex.Message);
            }
            return responseEntity;
        }

        public ResponseEntity ChangeVendorPassword(VendorEntity vendorEntity)
        {

            ResponseEntity responseEntity = new ResponseEntity();
            try
            {

                if (_vendorServices.ChangeVendorPassword(vendorEntity.vendorId, vendorEntity.password, vendorEntity.oldPassword))
                {
                    responseEntity.responseMessage = "success"; responseEntity.responseCode = 1; responseEntity.isUserActive = "true";
                }
                else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; responseEntity.isUserActive = "false"; }
            }
            catch (Exception ex)
            {
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
                throw new FaultException(ex.Message);
            }
            return responseEntity;
        }

        public ResponseEntity ChangeAdminPassword(SuperAdminLoginEntity vendorEntity)
        {

            ResponseEntity responseEntity = new ResponseEntity();
            try
            {

                if (_vendorServices.ChangeAdminPassword(vendorEntity.loginId, vendorEntity.password, vendorEntity.oldPassword))
                {
                    responseEntity.responseMessage = "success"; responseEntity.responseCode = 1; responseEntity.isUserActive = "true";
                }
                else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; responseEntity.isUserActive = "false"; }
            }
            catch (Exception ex)
            {
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
                throw new FaultException(ex.Message);
            }
            return responseEntity;
        }

        public ResponseEntity IsVendorHavingActivePromotion(VendorEntity vendorEntity)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                if (_OAuthServices.GetoAuthkey(vendorEntity._vendorId, "Vendor", oAuthkey))
                {
                    if (_vendorPromotionServices.IsVendorHavingActivePromotion(vendorEntity.vendorId, vendorEntity.oAuthkey))
                    {
                        responseEntity.responseMessage = "success"; responseEntity.responseCode = 1; responseEntity.IsActivePromotion = "true";
                    }
                    else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; responseEntity.IsActivePromotion = "false"; }
                }
                else { responseEntity.responseMessage = "Unauthorized"; responseEntity.responseCode = 401; }
            }
            catch (Exception ex)
            {
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
                throw new FaultException(ex.Message);
            }


            return responseEntity;
        }

        public GetResponseEntity GetAllCategoryByCategoryId(string CategoryId)
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data = _vendorServices.GetAllCategoryByCategoryId(Convert.ToInt32(CategoryId));
                if (response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }

        }

        public GetResponseEntity GetAllVendorByCategoryId(string categoryId, string userId, string userLocationLat, string userLocationLong)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                if (_OAuthServices.GetoAuthkey(Convert.ToInt32(userId), "User", oAuthkey))
                {
                    response.data = _vendorServices.GetAllVendorByCategoryId(Convert.ToInt32(categoryId), Convert.ToInt32(userId), userLocationLat, userLocationLong);
                    if (response.data.Count() > 0)
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                    return response;
                }
                response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public int CreateSubscribedUsers(SubscribeEntity subscribeEntity)
        {
            return _subscribeServices.CreateSubscribeUser(subscribeEntity);
        }

        public bool DeleteSubscribedUsers(string id)
        {
            try
            {
                int subscribeId = Convert.ToInt32(id);

                return _subscribeServices.DeleteSubscribeUser(subscribeId);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public ResponseEntity CreateVendor(VendorEntity vendorEntity)
        {
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                var Id = _vendorServices.CreateVendor(vendorEntity);
                if (Id > 0)
                {
                    responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                }
                else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
            }

            return responseEntity;

        }

        public GetResponseEntity GetVendorById(string Id)
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                int ID = Convert.ToInt32(Id);
                response.data1 = _vendorServices.GetVendorById(ID);
                if (response.data1 != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity GetAllHTMLContentById(string Id)
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                int ID = Convert.ToInt32(Id);
                response.data1 = _dynamicHTMLContentServices.GetAllHTMLContentById(ID);
                if (response.data1 != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity GetActivePromotionByVendorId(string Id)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                int ID = Convert.ToInt32(Id);
                if (_OAuthServices.GetoAuthkey(ID, "Vendor", oAuthkey))
                {
                    response.data = _vendorPromotionServices.GetActivePromotionByVendorId(ID);
                    if (response.data.Count() > 0)
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                    return response;
                }
                response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public int CreateVendorBranch(VendorBranchEntity vendorBranchEntity)
        {
            return _vendorServices.CreateVendorBranch(vendorBranchEntity);
        }

        public GetResponseEntity GetAllVendors()
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data = _vendorServices.GetAllVendors();
                if (response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public IEnumerable<BannersEntity> GetAllBanners(string type, string platform, string language)
        {
            try
            {
                return _bannerServices.GetAllBanners(type, platform, language);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public GetResponseEntity GetAllVendorCategory()
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data = _vendorServices.GetAllVendorCategory();
                if (response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public int CreateBanner(IEnumerable<BannersEntity> bannerEntity)
        {
            return _bannerServices.CreateBanner(bannerEntity);
        }

        public BannersEntity Upload(Stream Uploading, string VidoType)
        {
            return _bannerServices.UploadVideo(Uploading, VidoType);
        }

        public bool Deleteflash(BannersEntity bannerEntity)
        {
            return _bannerServices.DeleteBanners(bannerEntity.bannerId);
        }

        public GetResponseEntity GetAllContent(string page, string content, string lang)
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data = _dynamicHTMLContentServices.GetAllHTMLContent(page, content, lang);
                if (response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity GetAllContentByType(string content)
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data = _dynamicHTMLContentServices.GetAllContentByType(content);
                if (response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public int CreateHTMLContent(DynamicHTMLContentEntity dynamicHTMLContentEntity)
        {
            return _dynamicHTMLContentServices.CreateHTMLContent(dynamicHTMLContentEntity);
        }

        public bool DeleteHTMLContent(string id)
        {
            try
            {
                int contentId = Convert.ToInt32(id);
                return _dynamicHTMLContentServices.DeleteHTMLContent(contentId);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public GetResponseEntity GetAllDeals(string venderType, string pageId, string userLocationLat, string userLocationLong, string langType, string resType, string userId)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                int VenderTyp = Convert.ToInt32(venderType);
                int pages = Convert.ToInt32(pageId);
                if (_OAuthServices.GetoAuthkey(Convert.ToInt32(userId), "User", oAuthkey))
                {
                    response.data = _userPromotionsServices.GetAllDeals(VenderTyp, pages, userLocationLong, userLocationLat, langType, resType, userId);
                    if (response.data.Count() > 0)
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                    return response;
                }
                response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;

                return response;
            }

            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity SelectRandom(string id, string venderType, string userLocationLong, string userLocationLat, string langType, string resType)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                int userID = Convert.ToInt32(id);
                int VenderTyp = Convert.ToInt32(venderType);
                if (_OAuthServices.GetoAuthkey(userID, "User", oAuthkey))
                {
                    response.data = _userPromotionsServices.SelectRandom(userID, userLocationLat, userLocationLong,venderType);
                    if (response.data.Count() > 0 )
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                    return response;
                }
                response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }

        }

        /// <summary>
        /// ////////////////////////////// Get Open Promotion /////////////////
        /// </summary>
        /// <returns></returns>
        public GetResponseEntity GetOpenPromotions(string Id, string lang)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                int userId = Convert.ToInt32(Id);
                if (_OAuthServices.GetoAuthkey(userId, "User", oAuthkey))
                {
                    response.data = _userPromotionsServices.GetOpenPromotions(userId, lang);
                    if (response.data != null)
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                    return response;
                }
                response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public IEnumerable<PromotionsUserEntity> GetPromotionCodeByPID(UserPromotionEntity userPromotion)
        {

            try
            {
                return _userPromotionsServices.GetPromotionCode(userPromotion);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public GetResponseEntity GetPromotionDetails()
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data = _userPromotionsServices.GetPromotionDetails();
                if (response.data.Count() > 0 || response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity GetMyLocation(string userId)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                int id = Convert.ToInt32(userId);
                if (_OAuthServices.GetoAuthkey(id, "User", oAuthkey))
                {
                    response.data = _userServices.GetMyLocation(id);
                    if (response.data != null)
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                    return response;
                }
                response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity GetAllReward(UserEntity userentity)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                if (_OAuthServices.GetoAuthkey(userentity.userId, "User", oAuthkey))
                {
                    response.data = _userServices.GetAllReward(userentity);
                    if (response.data.Count() > 0)
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                    return response;
                }
                response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }


        public GetResponseEntity DeActivatePromotion(string promotionCodeId, string vendorId)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                int ID = Convert.ToInt32(promotionCodeId);
                int vendorID = Convert.ToInt32(vendorId);
                if (_OAuthServices.GetoAuthkey(vendorID, "Vendor", oAuthkey))
                {

                    if (_vendorPromotionServices.DeActivatePromotion(ID))
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                    return response;
                }
                response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }




        public GetResponseEntity GetAllPoint()
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data = _userServices.GetAllPoint();
                if (response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity ShareDeal(string ShareVia, string userId, string promotionId)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                if (_OAuthServices.GetoAuthkey(Convert.ToInt32(userId), "User", oAuthkey))
                {
                    var data = _userPromotionsServices.ShareDeal(ShareVia, Convert.ToInt32(userId), Convert.ToInt32(promotionId));
                    if (data == true)
                    {

                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                    return response;
                }
                response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity ShareApp(string ShareVia, string userId)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                if (_OAuthServices.GetoAuthkey(Convert.ToInt32(userId), "User", oAuthkey))
                {
                    response.data = _userPromotionsServices.ShareApp(ShareVia, Convert.ToInt32(userId));
                    if (response.data != null)
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                    return response;
                }
                response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }
        public GetResponseEntity GetSubCategoryById(string Id)
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                //int id = Convert.ToInt32(userId);
                response.data1 = _vendorServices.GetSubCategoryById(Convert.ToInt32(Id));
                if (response.data1 != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public ResponseEntity AddSubCategory(SubCategoryEntity subCategoryEntity)
        {
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                var id = _vendorServices.AddSubCategory(subCategoryEntity);
                if (id > 0)
                {
                    responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                }
                else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
            }

            return responseEntity;
        }
        public ResponseEntity UpdateSubCategory(SubCategoryEntity subCategoryEntity)
        {
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                if (_vendorServices.UpdateSubCategory(subCategoryEntity))
                {
                    responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                }
                else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
            }

            return responseEntity;
        }
        public ResponseEntity DeleteSubCategory(SubCategoryEntity subCategoryEntity)
        {
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                if (_vendorServices.DeleteSubCategory(subCategoryEntity.subCategoryId))
                {
                    responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                }
                else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
            }

            return responseEntity;

        }
        public GetResponseEntity GetOptCategoryById(string Id)
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                //int id = Convert.ToInt32(userId);
                response.data1 = _vendorServices.GetOptCategoryById(Convert.ToInt32(Id));
                if (response.data1 != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }
        public ResponseEntity AddOptionalCategory(OptionalCategoryEntity optCategoryEntity)
        {
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                var id = _vendorServices.AddOptionalCategory(optCategoryEntity);
                if (id > 0)
                {
                    responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                }
                else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
            }

            return responseEntity;
        }
        public ResponseEntity UpdateOptCategory(OptionalCategoryEntity optCategoryEntity)
        {
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                if (_vendorServices.UpdateOptCategory(optCategoryEntity))
                {
                    responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                }
                else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
            }

            return responseEntity;
        }
        public ResponseEntity DeleteOptCategory(OptionalCategoryEntity optCategoryEntity)
        {
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                if (_vendorServices.DeleteOptCategory(optCategoryEntity.optCategoryId))
                {
                    responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                }
                else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
            }

            return responseEntity;

        }
        public ResponseEntity AddUserReward(UserRewardEntity userRewardEntity)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                if (_OAuthServices.GetoAuthkey(Convert.ToInt32(userRewardEntity.userId), "User", oAuthkey))
                {
                    if (_userServices.AddReward(userRewardEntity))
                    {
                        responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                    }
                    else { responseEntity.responseMessage = "Already Exist"; responseEntity.responseCode = -1; }
                }
                else { responseEntity.responseMessage = "Unauthorized"; responseEntity.responseCode = 401; }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
            }

            return responseEntity;
        }

        public ResponseEntity UpdateUserReward(UserRewardEntity userRewardEntity)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                if (_OAuthServices.GetoAuthkey(Convert.ToInt32(userRewardEntity.userId), "User", oAuthkey))
                {
                    if (_userServices.UpdateReward(userRewardEntity, userRewardEntity.userRewardId))
                    {
                        responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                    }
                    else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
                }
                else { responseEntity.responseMessage = "Unauthorized"; responseEntity.responseCode = 401; }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
            }

            return responseEntity;
        }

        public IEnumerable<PointsConfigurationEntity> GetAllPointConfiguration()
        {
            try
            {
                var pointsConfigurationEntites = new List<PointsConfigurationEntity>();
                var point = _pointConfigurationServices.GetAllPoint();
                if (point != null)
                {
                    pointsConfigurationEntites = point as List<PointsConfigurationEntity> ?? point.ToList();

                }
                return pointsConfigurationEntites;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public IEnumerable<PointsConfigurationEntity> UpdatePointsRewards(PointsConfigurationEntity pointsConfigurationEntity)
        {
            try
            {
                return _pointConfigurationServices.UpdatePointsRewards(pointsConfigurationEntity);

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public int CreateVendorPromotions(PromotionCodeEntity VendorPromotionEntity)
        {
            try
            {
                return _vendorPromotionServices.CreateVendorPromotion(VendorPromotionEntity);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
               
            }
        }

        public ResponseEntity UpdateVendorPromotion(PromotionCodeEntity VendorPromotion)
        {
            ResponseEntity responseEntity = new ResponseEntity();
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            try
            {
                if (_OAuthServices.GetoAuthkey(VendorPromotion.vendorId, "Vendor", oAuthkey))
                {
                    if (_vendorPromotionServices.UpdateVendorPromotion(VendorPromotion.promotionCodeId, VendorPromotion))
                    {
                        responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                    }
                    else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
                }

                else { responseEntity.responseMessage = "Unauthorized"; responseEntity.responseCode = 401; }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
            }
            return responseEntity;
        }

        public ResponseEntity DeleteVendorPromotion(PromotionCodeEntity VendorPromotion)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                if (_OAuthServices.GetoAuthkey(VendorPromotion.vendorId, "Vendor", oAuthkey))
                {

                    if (_vendorPromotionServices.DeleteVendorPromotion(VendorPromotion.promotionCodeId, VendorPromotion))
                    {
                        responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                    }
                    else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
                }
                else { responseEntity.responseMessage = "Unauthorized"; responseEntity.responseCode = 401; }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
            }

            return responseEntity;

        }

        public ResponseEntity DeletePromotionByUserId(UserPromotionEntity user)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                if (_OAuthServices.GetoAuthkey(Convert.ToInt32(user.userId), "User", oAuthkey))
                {
                    if (_userServices.DeletePromotionByUserId(Convert.ToInt32(user.userId), Convert.ToInt32(user.promotionId), user.devicePlatform, user.deviceToken))
                    {
                        responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                    }
                    else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
                }
                else { responseEntity.responseMessage = "Unauthorized"; responseEntity.responseCode = 401; }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
            }

            return responseEntity;
        }

        public bool DeleteVendor(VendorEntity Vendor)
        {
            try
            {
                return _vendorServices.DeleteVendor(Vendor.vendorId);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool UpdateHTMLContent(DynamicHTMLContentEntity HTMLContentEntity)
        {
            try
            {
                return _dynamicHTMLContentServices.UpdateHTMLContent(HTMLContentEntity.Id, HTMLContentEntity);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public GetResponseEntity UpdateVendor(VendorEntity Vendor)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                if (_OAuthServices.GetoAuthkey(Vendor._vendorId, "Vendor", oAuthkey))
                {
                    var data1 = _vendorServices.UpdateVendor(Vendor.vendorId, Vendor);
                    if (data1 != "1")
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                        response.Image = data1;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                    return response;
                }
                response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public ResponseEntity PinVendorPromotion(PromotionCodeEntity VendorPromotion)
        {
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {

                if (_vendorPromotionServices.PinVendorPromotion(VendorPromotion.promotionCodeId, VendorPromotion))
                {

                    responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                }
                else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
            }
            catch (Exception ex)
            {

                throw new FaultException(ex.Message);
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
            }


            return responseEntity;
        }

        public GetResponseEntity UpdateMyLocation(IEnumerable<UserLocationEntity> userLocationEntity)
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data = _userServices.UpdateMyLocation(userLocationEntity);
                if (response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity GetLandingDetails()
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data = _appDefaultLandingServices.GetLandingDetails();
                if (response.data.Count() > 0 || response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public bool UpdateLandingDetails(AppDefaultLandingPageEntity appDefaultLandingPageEntity)
        {
            try
            {
                int Id = Convert.ToInt32(appDefaultLandingPageEntity.landingPageId);
                return _appDefaultLandingServices.UpdateLandingDetails(Id);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public GetResponseEntity GetAllHowItWork(string platform)
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data = _bannerServices.GetAllHowItWork(platform);
                if (response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public int CreateHowItWork(BannersEntity bannerEntity)
        {
            try
            {
                return _bannerServices.CreateHowItWork(bannerEntity);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool DeleteHowItWork(string id)
        {
            try
            {

                int HowItWorkId = Convert.ToInt32(id);

                return _bannerServices.DeleteHowItWork(HowItWorkId);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool UpdateBanner(BannersEntity bannerEntity)
        {
            try
            {
                return _bannerServices.UpdateBanners(bannerEntity.bannerId, bannerEntity);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public GetResponseEntity GetAllCategory()
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data = _iCategoryServices.GetAllCategory();
                if (response.data.Count() > 0 || response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity GetAllSubCategory()
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data = _iCategoryServices.GetAllSubCategory();
                if (response.data.Count() > 0 || response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }

        }

        public GetResponseEntity GetAllOptionalCategory()
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data = _iCategoryServices.GetAllOptionalCategory();
                if (response.data.Count() > 0 || response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity GetAllPromotionsById(string Id)
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data1 = _adminManageActivePromotionsServices.GetAllPromotionsById(Convert.ToInt32(Id));
                if (response.data1 != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }

        }

        public IEnumerable<ConfigurationEntity> GetConfiguration()
        {
            try
            {
                return _dynamicHTMLContentServices.GetConfiguration();
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public GetResponseEntity GetPromotionByVendorId(string Id)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                int ID = Convert.ToInt32(Id);
                if (_OAuthServices.GetoAuthkey(ID, "Vendor", oAuthkey))
                {
                    response.data = _vendorServices.GetPromotionByVendorId(ID);
                    if (response.data.Count() > 0)
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                }
                else { response.statusMessage = "Unauthorized"; response.statusCode = "401"; }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity GetPromotionByPromotionCodeId(string Id)
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                int ID = Convert.ToInt32(Id);
                response.data = _vendorPromotionServices.GetPromotionByPromotionCodeId(ID);
                if (response.data.Count() > 0 || response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }

        }

        public GetResponseEntity GetUserPrefrenceById(string Id)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];

            GetResponseEntity response = new GetResponseEntity();
            try
            {
                int ID = Convert.ToInt32(Id);
                if (_OAuthServices.GetoAuthkey(ID, "User", oAuthkey))
                {
                    response.data = _userPrefrenceServices.GetUserPrefrenceById(ID);
                    if (response.data != null)
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                    return response;
                }
                response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public ResponseEntity CreateUserPrefrence(UserPrefrencesEntity UserPrefrencesEntity)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                if (_OAuthServices.GetoAuthkey(UserPrefrencesEntity.userId, "User", oAuthkey))
                {
                    var returnID = _userPrefrenceServices.CreateUserPrefrence(UserPrefrencesEntity);
                    if (returnID > 0)
                    {
                        responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                    }
                    else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
                }
                else { responseEntity.responseMessage = "Unauthorized"; responseEntity.responseCode = 401; }

            }
            catch (Exception ex)
            {
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
                throw new FaultException(ex.Message);
            }
            return responseEntity;

        }

        public GetResponseEntity GetUserById(string userId)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                int ID = Convert.ToInt32(userId);
                if (_OAuthServices.GetoAuthkey(ID, "User", oAuthkey))
                {
                    response.data1 = _userServices.GetUserById(ID);
                    if (response.data1 != null)
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                    return response;
                }
                response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity GetAllUsers()
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data = _userServices.GetAllUsers();
                if (response.data.Count() > 0 || response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public ResponseEntity ViewedPromotion(PromotionCodeEntity promotionCodeId)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                int ID = promotionCodeId.promotionCodeId;
                if (_OAuthServices.GetoAuthkey(promotionCodeId.userId, "User", oAuthkey))
                {
                    var returnID = _userPromotionsServices.ViewedPromotion(ID, promotionCodeId.vendorId, promotionCodeId.userId);
                    if (returnID != null)
                    {
                        responseEntity.responseMessage = "success"; responseEntity.responseCode = 1; responseEntity.isFavourite = returnID.isActive = returnID.isActive == null ? false : returnID.isActive; responseEntity.isFavouriteId = returnID.favoriteRestaurantId.ToString() == null ? "" : returnID.favoriteRestaurantId.ToString();
                        responseEntity.serverTime = returnID.serverTime == null ? "" : returnID.serverTime;
                    }
                    else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; responseEntity.isFavourite = returnID.isActive = false; responseEntity.isFavouriteId = ""; }
                }
                else { responseEntity.responseMessage = "Unauthorized"; responseEntity.responseCode = 401; }
            }
            catch (Exception ex)
            {
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
                throw new FaultException(ex.Message);
            }
            return responseEntity;
        }

        public GetResponseEntity UsePromotion(string promotionId, string userId, string deviceToken, string devicePlatform)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                DateTime UTCTime = System.DateTime.UtcNow;
                DateTime dates2 = UTCTime.AddHours(7.0);
                int promotionCodeId = Convert.ToInt32(promotionId);
                int UserID = Convert.ToInt32(userId);
                if (_OAuthServices.GetoAuthkey(UserID, "User", oAuthkey))
                {
                    response.data = _userPromotionsServices.UsePromotion(promotionCodeId, UserID, deviceToken, devicePlatform);
                    if (response.data.Count() > 0)
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                        response.serverTime = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2);
                        PromotionsUserEntity promo = (PromotionsUserEntity)response.data.FirstOrDefault();
                        response.exist = promo.tempQuantity;
                        response.promoUsed = promo.promoExpired;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                        response.serverTime = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2);
                    }
                    return response;
                }
                response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public ResponseEntity LikePromotion(UserPromotionEntity promotionCodeId)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];

            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                int ID = Convert.ToInt32(promotionCodeId.promotionId);
                int userId = Convert.ToInt32(promotionCodeId.userId);
                if (_OAuthServices.GetoAuthkey(userId, "User", oAuthkey))
                {
                    var returnID = _userPromotionsServices.LikePromotion(ID, userId, promotionCodeId.like);
                    if (returnID)
                    {
                        responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                    }
                    else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
                }
                else { responseEntity.responseMessage = "Unauthorized"; responseEntity.responseCode = 401; }
            }
            catch (Exception ex)
            {
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
                throw new FaultException(ex.Message);
            }
            return responseEntity;
        }

        public ResponseEntity UpdateUserPrefrence(UserPrefrencesEntity UserPrefrencesEntity)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                if (_OAuthServices.GetoAuthkey(UserPrefrencesEntity.userId, "User", oAuthkey))
                {

                    if (UserPrefrencesEntity.id == 0)
                    {
                        var returnID = _userPrefrenceServices.CreateUserPrefrence(UserPrefrencesEntity);
                        if (returnID > 0)
                        {
                            responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                        }
                        else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
                    }
                    else
                    {
                        if (_userPrefrenceServices.UpdateUserPrefrence(UserPrefrencesEntity))
                        {
                            responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                        }
                        else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
                    }
                }
                else { responseEntity.responseMessage = "Unauthorized"; responseEntity.responseCode = 401; }
            }
            catch (Exception ex)
            {
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
                throw new FaultException(ex.Message);
            }
            return responseEntity;
        }

        public ResponseEntity DeleteUserPrefrence(string Id)
        {
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                int ID = Convert.ToInt32(Id);

                if (_userPrefrenceServices.DeleteUserPrefrence(ID))
                {
                    responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                }
                else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
            }
            catch (Exception ex)
            {


                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
                throw new FaultException(ex.Message);
            }
            return responseEntity;
        }

        public ResponseEntity AddVenueToFavorite(UserFavouriteEntity userFavouriteEntity)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                string data = _userServices.AddVenueToFavorite(userFavouriteEntity);
                if (data != null)
                {
                    responseEntity.responseMessage = "success"; responseEntity.responseCode = 1; responseEntity.isFavouriteId = data;
                }
                else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; responseEntity.isFavouriteId = ""; }
            }
            catch (Exception ex)
            {
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
                throw new FaultException(ex.Message);
            }
            return responseEntity;

        }

        public ResponseEntity DeleteVenueToFavorite(UserFavouriteEntity userFavouriteEntity)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                if (_OAuthServices.GetoAuthkey(Convert.ToInt32(userFavouriteEntity.userId), "User", oAuthkey))
                {

                    if (_userServices.DeleteVenueToFavorite(userFavouriteEntity))
                    {
                        responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                    }
                    else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
                }
                else { responseEntity.responseMessage = "Unauthorized"; responseEntity.responseCode = 401; }
            }
            catch (Exception ex)
            {
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
                throw new FaultException(ex.Message);
            }
            return responseEntity;

        }

        public GetResponseEntity UserFavRestaurant(string Id)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                int ID = Convert.ToInt32(Id);
                if (_OAuthServices.GetoAuthkey(ID, "User", oAuthkey))
                {
                    response.data = _userServices.UserFavRestaurant(ID);
                    if (response.data != null)
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                    return response;
                }
                response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity GetUserPromotionByUserId(string userId, string deviceToken, string devicePlatform)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                if (_OAuthServices.GetoAuthkey(Convert.ToInt32(userId), "User", oAuthkey))
                {

                    response.data = _userServices.GetUserPromotionByUserId(Convert.ToInt32(userId),deviceToken, devicePlatform);
                    if (response.data != null)
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                    return response;
                }
                response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity UpdateUserFavouriteRest(string Id, string userId)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                int ID = Convert.ToInt32(Id);
                if (_OAuthServices.GetoAuthkey(Convert.ToInt32(userId), "User", oAuthkey))
                {
                    response.data = _userServices.UpdateUserFavouriteRest(ID);
                    if (response.data != null)
                    {
                        response.statusCode = "1";
                        response.statusMessage = StaticMessages.dataFound;
                    }
                    else
                    {
                        response.statusCode = "0";
                        response.statusMessage = StaticMessages.emptyData;
                    }
                    return response;
                }
                response.statusCode = "401";
                response.statusMessage = StaticMessages.Unauthorized;
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public ResponseEntity GetLandingDetailApp(string time)
        {

            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                var data = _appDefaultLandingServices.GetLandingDetailApp(time);
                if (data != null)
                {

                    responseEntity.responseMessage = "success"; responseEntity.responseCode = 1; responseEntity.category = data;
                }
                else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; responseEntity.category = ""; }
            }
            catch (Exception ex)
            {

                throw new FaultException(ex.Message);
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
            }
            return responseEntity;
        }

        public ResponseEntity GetLandingDetailTime(string time)
        {
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                var data = _appDefaultLandingServices.GetLandingDetailTime(time);
                if (data != null)
                {

                    responseEntity.responseMessage = "success"; responseEntity.responseCode = 1; responseEntity.category = data;
                }
                else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; responseEntity.category = ""; }
            }
            catch (Exception ex)
            {

                throw new FaultException(ex.Message);
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
            }
            return responseEntity;
        }

        public GetResponseEntity GetAllChart()
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data1 = _AdminChartServices.GetAllChart();
                if (response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }

        }

        public GetResponseEntity GetAllChartByVendorID(string id, string fromDate, string toDate)
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data1 = _AdminChartServices.GetAllChartByVendorID(id, fromDate, toDate);
                if (response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }

        }

        public GetResponseEntity GetAllChartByTime(string id, string CategoryType, string time, string fromDate, string toDate)
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data1 = _AdminChartServices.GetAllChartByTime(id, CategoryType, time, fromDate, toDate);
                if (response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }

        }


        public GetResponseEntity GetTrackUsage()
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data1 = _AdminChartServices.GetTrackUsage();
                if (response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }

        }

        public GetResponseEntity GetMultipleUsage()
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data1 = _AdminChartServices.GetMultipleUsage();
                if (response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }

        }

        public GetResponseEntity GetSocialMedia()
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data1 = _AdminChartServices.GetSocialMedia();
                if (response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }

        }

        public GetResponseEntity GetUserPerDayUse()
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data1 = _AdminChartServices.GetUserPerDayUse();
                if (response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }

        }


        # region Admin Appliction
        //////////////////////////////  Admin Manage Promotion   /////////////////////////
        public GetResponseEntity GetAllPromotions()
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data = _adminManageActivePromotionsServices.GetAllPromotions();
                if (response.data.Count() > 0)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity GetPromotion()
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data = _adminManageActivePromotionsServices.GetPromotion();
                if (response.data.Count() > 0)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public ResponseEntity DeletePromotion(PromotionCodeEntity promotionCodeEntity)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {

                if (_OAuthServices.GetoAuthkey(promotionCodeEntity.vendorId, "Vendor", "Admin"))
                {

                    int ID = Convert.ToInt32(promotionCodeEntity.promotionCodeId);
                    if (_adminManageActivePromotionsServices.DeletePromotion(ID))
                    {
                        responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                    }
                    else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
                }
                else { responseEntity.responseMessage = "Unauthorized"; responseEntity.responseCode = 401; }
            }
            catch (Exception ex)
            {

                throw new FaultException(ex.Message);
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
            }
            return responseEntity;
        }
        public IEnumerable<RewardEntity> GetRewards()
        {
            try
            {

                return _AdminReward.GetALLReward();
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public IEnumerable<UserEntity> GetRandomUsers(string num)
        {
            try
            {

                return _AdminReward.GetRandomUsers(num);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public IEnumerable<RewardEntity> GetALLRewardByType(string type)
        {
            try
            {

                return _AdminReward.GetALLRewardByType(type);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }
        public RewardEntity GetRewardById(string ID)
        {
            try
            {

                return _AdminReward.GetRewardById(Convert.ToInt32(ID));
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public RewardEntity GetUserCount()
        {
            try
            {

                return _AdminReward.GetUserCount();
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }
        public ResponseEntity CreateReward(RewardEntity rewardEntity)
        {
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {

                var reward = _AdminReward.CreateReward(rewardEntity);
                if (reward > 0)
                {
                    responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                }
                else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
            }
            catch (Exception ex)
            {
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
                throw new FaultException(ex.Message);
            }
            return responseEntity;
        }
        public ResponseEntity UpdateReward(RewardEntity rewardEntity)
        {
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {


                if (_AdminReward.UpdateReward(rewardEntity))
                {
                    responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                }
                else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
            }
            catch (Exception ex)
            {
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
                throw new FaultException(ex.Message);
            }
            return responseEntity;
        }
        public ResponseEntity DeleteReward(string Id)
        {
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {


                if (_AdminReward.DeleteReward(Convert.ToInt32(Id)))
                {
                    responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                }
                else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
            }
            catch (Exception ex)
            {
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
                throw new FaultException(ex.Message);
            }
            return responseEntity;
        }
        public IEnumerable<ContentManagementEntity> GetAllVideoWebApp()
        {
            try
            {

                return _vendorServices.GetAllVideoWebApp();
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        #endregion

        public ResponseEntity DeleteMyLocation(UserLocationEntity userLocationEntity)
        {
            var oAuthkey = WebOperationContext.Current.IncomingRequest.Headers["oAuthkey"];
            ResponseEntity responseEntity = new ResponseEntity();
            try
            {
                if (_OAuthServices.GetoAuthkey(userLocationEntity.fkUser, "User", oAuthkey))
                {

                    if (_userServices.DeleteMyLocation(userLocationEntity))
                    {
                        responseEntity.responseMessage = "success"; responseEntity.responseCode = 1;
                    }
                    else { responseEntity.responseMessage = "Failure"; responseEntity.responseCode = -1; }
                }
                else { responseEntity.responseMessage = "Unauthorized"; responseEntity.responseCode = 401; }
            }
            catch (Exception ex)
            {

                throw new FaultException(ex.Message);
                responseEntity.responseMessage = ex.Message;
                responseEntity.responseCode = ex.GetHashCode();
            }


            return responseEntity;
        }

        public IEnumerable<PromotionsUserEntity> GetAllDealWebApp()
        {
            try
            {
                return _userPromotionsServices.GetAllDealWebApp();

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public IEnumerable<VendorEntity> GetMapLocation()
        {
            try
            {
                return _userPromotionsServices.GetMapLocation();
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }

        }
        public GetResponseEntity NotificationManual(DeviceEntity deviceEntity)
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data1 = _PushNotificationServices.NotificationManual(deviceEntity);
                if (response.data1 != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                //string P12_file = string.Empty;
                //// { P12_file = HttpContext.Current.Server.MapPath(""); }
                //P12_file = AppDomain.CurrentDomain.BaseDirectory;

                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity PredictiveNotication(PredictiveNotificationEntity predictiveNotificationEntity)
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data1 = _PushNotificationServices.PredictiveNotication(predictiveNotificationEntity);
                if (response.data1 != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity GetPredictiveNotication()
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data1 = _PushNotificationServices.GetPredictiveNotication();
                if (response.data1 != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public GetResponseEntity GetRestaurants()
        {
            GetResponseEntity response = new GetResponseEntity();
            try
            {
                response.data = _vendorServices.GetRestaurants();
                if (response.data != null)
                {
                    response.statusCode = "1";
                    response.statusMessage = StaticMessages.dataFound;
                }
                else
                {
                    response.statusCode = "0";
                    response.statusMessage = StaticMessages.emptyData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.statusCode = "-1";
                response.statusMessage = ex.Message;
                return response;
            }
        }

    }
}
