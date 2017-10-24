using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using TV.CraveASAP.BusinessEntities;
using TV.CraveASAP.BusinessServices;
using TV.CraveASAP.BusinessServices.Interfaces;

namespace CraveASAPRestServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ICraveServices
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "CreateSubscribedUsers", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        int CreateSubscribedUsers(SubscribeEntity subscribeEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "UserLogIn", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        UserEntity UserLogIn(UserEntity UserEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "SkipUser", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        UserEntity SkipUser(UserEntity UserEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetUserById{userId}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(UserEntity))]
        GetResponseEntity GetUserById(string userId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetICrave{vendorType},{userLocationLat},{userLocationLong},{userId},{page}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<PromotionsUserEntity>))]
        GetResponseEntity GetICrave(string vendorType, string userLocationLat, string userLocationLong, string userId,string page);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetUserByUserId{userId}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<UserAppEntity>))]
        GetResponseEntity GetUserByUserId(string userId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllUsers", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<UserEntity>))]
        GetResponseEntity GetAllUsers();

        [OperationContract]
        [WebInvoke(UriTemplate = "DeleteSubscribedUsers", Method = "DELETE", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool DeleteSubscribedUsers(string id);

        [OperationContract]
        [WebInvoke(UriTemplate = "DeletePromotionByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity DeletePromotionByUserId(UserPromotionEntity user);

        [OperationContract]
        [WebInvoke(UriTemplate = "CreateVendor", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity CreateVendor(VendorEntity vendorEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetVendorById{Id}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(VendorEntity))]
        GetResponseEntity GetVendorById(string Id);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetActivePromotionByVendorId{Id}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<PromotionCodeEntity>))]
        GetResponseEntity GetActivePromotionByVendorId(string Id);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllHTMLContentById{Id}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(DynamicHTMLContentEntity))]
        GetResponseEntity GetAllHTMLContentById(string Id);

        [OperationContract]
        [WebInvoke(UriTemplate = "UpdateMyLocation", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(UserLocationEntity))]
        GetResponseEntity UpdateMyLocation(IEnumerable<UserLocationEntity> userLocationEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "DeleteMyLocation", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity DeleteMyLocation(UserLocationEntity userLocationEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "UpdateVendor", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(VendorEntity))]
        GetResponseEntity UpdateVendor(VendorEntity Vendor);

        [OperationContract]
        [WebInvoke(UriTemplate = "DeActivatePromotion{promotionCodeId},{vendorId}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(VendorEntity))]
        GetResponseEntity DeActivatePromotion(string promotionCodeId, string vendorId);

        [OperationContract]
        [WebInvoke(UriTemplate = "DeleteVendor", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool DeleteVendor(VendorEntity Vendor);

        [OperationContract]
        [WebInvoke(UriTemplate = "CreateVendorBranch", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        int CreateVendorBranch(VendorBranchEntity vendorBranchEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllPromotionsById{Id}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(PromotionCodeEntity))]
        GetResponseEntity GetAllPromotionsById(string Id);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllVendors", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<VendorListEntity>))]
        GetResponseEntity GetAllVendors();

        //[OperationContract]
        //[WebInvoke(UriTemplate = "GetVendorsList", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        //[ServiceKnownType(typeof(IEnumerable<VendorCategoryEntity>))]
        //GetResponseEntity GetVendorsList();

        [OperationContract]
        [WebInvoke(UriTemplate = "UpdateHTMLContent", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateHTMLContent(DynamicHTMLContentEntity HTMLContentEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "VendorLogout", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity VendorLogout(VendorEntity Vendor);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllVendorCategory", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<CategoryEntity>))]
        GetResponseEntity GetAllVendorCategory();

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllContent{page},{content},{lang}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<DynamicHTMLContentEntity>))]
        GetResponseEntity GetAllContent(string page, string content, string lang);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllContentByType{content}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<DynamicHTMLContentEntity>))]
        GetResponseEntity GetAllContentByType(string content);

        [OperationContract]
        [WebInvoke(UriTemplate = "DeleteHTMLContent", Method = "DELETE", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool DeleteHTMLContent(string id);

        [OperationContract]
        [WebInvoke(UriTemplate = "CreateHTMLContent", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        int CreateHTMLContent(DynamicHTMLContentEntity dynamicHTMLContentEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllDeals{venderType},{pageId},{userLocationLat},{userLocationLong},{langType},{resType},{userId}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<PromotionsUserEntity>))]
        GetResponseEntity GetAllDeals(string venderType, string pageId, string userLocationLong, string userLocationLat, string langType, string resType, string userId);

        [OperationContract]
        [WebInvoke(UriTemplate = "SelectRandom{id},{venderType},{userLocationLong},{userLocationLat},{langType},{resType}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<PromotionsUserEntity>))]
        GetResponseEntity SelectRandom(string id, string venderType, string userLocationLong, string userLocationLat, string langType, string resType);

        [OperationContract]
        [WebInvoke(UriTemplate = "VendorLogin{emailId},{password},{deviceToken},{deviceType}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<VendorEntity>))]
        GetResponseEntity VendorLogin(string emailId, string password, string deviceToken, string deviceType);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetConfiguration", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IEnumerable<ConfigurationEntity> GetConfiguration();

        [OperationContract]
        [WebInvoke(UriTemplate = "SuperAdminLogin{emailId},{password}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<SuperAdminLoginEntity>))]
        GetResponseEntity SuperAdminLogin(string emailId, string password);

        [OperationContract]
        [WebInvoke(UriTemplate = "ForgotVendorPassword", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity ForgotVendorPassword(VendorEntity vendorEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "ChangeVendorPassword", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity ChangeVendorPassword(VendorEntity vendorEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "ChangeAdminPassword", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity ChangeAdminPassword(SuperAdminLoginEntity vendorEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "IsVendorHavingActivePromotion", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity IsVendorHavingActivePromotion(VendorEntity vendorEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllCategoryByCategoryId{CategoryId}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<CategoryMapEntity>))]
        GetResponseEntity GetAllCategoryByCategoryId(string CategoryId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllVendorByCategoryId{categoryId},{userId},{userLocationLat},{userLocationLong}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<VendorCategoryEntity>))]
        GetResponseEntity GetAllVendorByCategoryId(string categoryId, string userId, string userLocationLat, string userLocationLong);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetOpenPromotions{Id},{lang}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<PromotionCodeEntity>))]
        GetResponseEntity GetOpenPromotions(string Id, string lang);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetPromotionCodeByPID", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IEnumerable<PromotionsUserEntity> GetPromotionCodeByPID(UserPromotionEntity userPromotion);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetPromotionDetails", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<PromotionCodeEntity>))]
        GetResponseEntity GetPromotionDetails();

        //[OperationContract]
        //[WebInvoke(UriTemplate = "GetAllActivePromotions", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        //IEnumerable<PromotionCodeEntity> GetAllActivePromotions();

        [OperationContract]
        [WebInvoke(UriTemplate = "GetMyLocation{userId}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<UserLocationEntity>))]
        GetResponseEntity GetMyLocation(string userId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllPoint", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<PointEntity>))]
        GetResponseEntity GetAllPoint();

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllReward", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<RewardEntity>))]
        GetResponseEntity GetAllReward(UserEntity userEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllPointConfiguration", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IEnumerable<PointsConfigurationEntity> GetAllPointConfiguration();

        [OperationContract]
        [WebInvoke(UriTemplate = "UpdatePointsRewards", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IEnumerable<PointsConfigurationEntity> UpdatePointsRewards(PointsConfigurationEntity pointsConfigurationEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllBanners{type},{platform},{language}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IEnumerable<BannersEntity> GetAllBanners(string type, string platform, string language);

        [OperationContract]
        [WebInvoke(UriTemplate = "CreateBanner", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        int CreateBanner(IEnumerable<BannersEntity> bannerEntity);

        [OperationContract(Name = "Upload")][DataContractFormat]
        [WebInvoke(Method = "POST", UriTemplate = "Upload/{VidoType}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        BannersEntity Upload(Stream Uploading, string VidoType);

        [OperationContract]
        [WebInvoke(UriTemplate = "Deleteflash", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool Deleteflash(BannersEntity bannerEntity);
        
        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllPromotions", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<VendorEntity>))]
        GetResponseEntity GetAllPromotions();

        [OperationContract]
        [WebInvoke(UriTemplate = "GetPromotion", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<PromotionListEntity>))]
        GetResponseEntity GetPromotion();

        [OperationContract]
        [WebInvoke(UriTemplate = "DeletePromotion", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity DeletePromotion(PromotionCodeEntity promotionCodeEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "CreateVendorPromotions", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        int CreateVendorPromotions(PromotionCodeEntity VendorPromotionEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "DeleteVendorPromotion", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity DeleteVendorPromotion(PromotionCodeEntity VendorPromotion);

        [OperationContract]
        [WebInvoke(UriTemplate = "PinVendorPromotion", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity PinVendorPromotion(PromotionCodeEntity VendorPromotion);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetLandingDetails", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<CategoryEntity>))]
        GetResponseEntity GetLandingDetails();

        [OperationContract]
        [WebInvoke(UriTemplate = "GetLandingDetailApp{time}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity GetLandingDetailApp(string time);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetLandingDetailTime{time}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity GetLandingDetailTime(string time);

        [OperationContract]
        [WebInvoke(UriTemplate = "UpdateLandingDetails", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateLandingDetails(AppDefaultLandingPageEntity appDefaultLandingPageEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "UpdateVendorPromotion", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity UpdateVendorPromotion(PromotionCodeEntity VendorPromotion);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllCategory", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<CategoryEntity>))]
        GetResponseEntity GetAllCategory();

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllSubCategory", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<SubCategoryEntity>))]
        GetResponseEntity GetAllSubCategory();

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllOptionalCategory", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<OptionalCategoryEntity>))]
        GetResponseEntity GetAllOptionalCategory();

        [OperationContract]
        [WebInvoke(UriTemplate = "UpdateBanner", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateBanner(BannersEntity bannerEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllHowItWork{platform}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<BannersEntity>))]
        GetResponseEntity GetAllHowItWork(string platform);

        [OperationContract]
        [WebInvoke(UriTemplate = "CreateHowItWork", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        int CreateHowItWork(BannersEntity bannerEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "DeleteHowItWork", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool DeleteHowItWork(string Id);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetPromotionByVendorId{Id}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<PromotionCodeEntity>))]
        GetResponseEntity GetPromotionByVendorId(string Id);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetPromotionByPromotionCodeId{Id}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<PromotionCodeEntity>))]
        GetResponseEntity GetPromotionByPromotionCodeId(string Id);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetUserPrefrenceById{Id}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<UserPrefrencesEntity>))]
        GetResponseEntity GetUserPrefrenceById(string Id);

        [OperationContract]
        [WebInvoke(UriTemplate = "CreateUserPrefrence", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity CreateUserPrefrence(UserPrefrencesEntity UserPrefrencesEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "UpdateUserPrefrence", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity UpdateUserPrefrence(UserPrefrencesEntity UserPrefrencesEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "DeleteUserPrefrence{Id}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity DeleteUserPrefrence(string Id);

        [OperationContract]
        [WebInvoke(UriTemplate = "ViewedPromotion", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity ViewedPromotion(PromotionCodeEntity promotionCodeId);

        [OperationContract]
        [WebInvoke(UriTemplate = "UsePromotion{promotionId},{userId},{deviceToken},{devicePlatform}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<PromotionsUserEntity>))]
        GetResponseEntity UsePromotion(string promotionId, string userId, string deviceToken, string devicePlatform);

        [OperationContract]
        [WebInvoke(UriTemplate = "LikePromotion", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity LikePromotion(UserPromotionEntity promotionCodeId);

        [OperationContract]
        [WebInvoke(UriTemplate = "AddVenueToFavorite", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity AddVenueToFavorite(UserFavouriteEntity userFavouriteEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "UserFavRestaurant{Id}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<UserFavouriteEntity>))]
        GetResponseEntity UserFavRestaurant(string Id);

        [OperationContract]
        [WebInvoke(UriTemplate = "UpdateUserFavouriteRest{Id},{userId}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<UserFavouriteEntity>))]
        GetResponseEntity UpdateUserFavouriteRest(string Id, string userId);

        [OperationContract]
        [WebInvoke(UriTemplate = "ShareApp{ShareVia},{userId}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<UserPointEntity>))]
        GetResponseEntity ShareApp(string ShareVia, string userId);

        [OperationContract]
        [WebInvoke(UriTemplate = "ShareDeal{ShareVia},{userId},{promotionId}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(GetResponseEntity))]
        GetResponseEntity ShareDeal(string ShareVia, string userId, string promotionId);

        [OperationContract]
        [WebInvoke(UriTemplate = "AddUserReward", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity AddUserReward(UserRewardEntity userRewardEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "UpdateUserReward", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity UpdateUserReward(UserRewardEntity userRewardEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "DeleteVenueToFavorite", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity DeleteVenueToFavorite(UserFavouriteEntity userFavouriteEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetUserPromotionByUserId{userId},{deviceToken},{devicePlatform}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<PromotionsUserEntity>))]
        GetResponseEntity GetUserPromotionByUserId(string userId, string deviceToken, string devicePlatform);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetRewards", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IEnumerable<RewardEntity> GetRewards();

        [OperationContract]
        [WebInvoke(UriTemplate = "GetRandomUsers{num}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IEnumerable<UserEntity> GetRandomUsers(string num);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetALLRewardByType{type}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IEnumerable<RewardEntity> GetALLRewardByType(string type);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetRewardById{ID}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        RewardEntity GetRewardById(string ID);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetUserCount", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        RewardEntity GetUserCount();

        [OperationContract]
        [WebInvoke(UriTemplate = "DeleteReward{ID}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity DeleteReward(string ID);

        [OperationContract]
        [WebInvoke(UriTemplate = "CreateReward", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity CreateReward(RewardEntity rewardEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "UpdateReward", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity UpdateReward(RewardEntity rewardEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllVideoWebApp", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IEnumerable<ContentManagementEntity> GetAllVideoWebApp();

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllDealWebApp", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IEnumerable<PromotionsUserEntity> GetAllDealWebApp();


        [OperationContract]
        [WebInvoke(UriTemplate = "GetMapLocation", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IEnumerable<VendorEntity> GetMapLocation();

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllChart", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<VendorEntity>))]
        GetResponseEntity GetAllChart();


        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllChartByVendorID{vendorId},{fromDate},{toDate}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<VendorEntity>))]
        GetResponseEntity GetAllChartByVendorID(string vendorId, string fromDate, string toDate);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAllChartByTime{vendorId},{CategoryType},{time},{fromDate},{toDate}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<VendorEntity>))]
        GetResponseEntity GetAllChartByTime(string vendorId, string CategoryType, string time, string fromDate, string toDate);



        [OperationContract]
        [WebInvoke(UriTemplate = "GetTrackUsage", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<VendorEntity>))]
        GetResponseEntity GetTrackUsage();

        [OperationContract]
        [WebInvoke(UriTemplate = "GetMultipleUsage", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<VendorEntity>))]
        GetResponseEntity GetMultipleUsage();

        [OperationContract]
        [WebInvoke(UriTemplate = "GetSocialMedia", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<VendorEntity>))]
        GetResponseEntity GetSocialMedia();

        [OperationContract]
        [WebInvoke(UriTemplate = "GetUserPerDayUse", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<VendorEntity>))]
        GetResponseEntity GetUserPerDayUse();


        [OperationContract]
        [WebInvoke(UriTemplate = "NotificationManual", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<DeviceEntity>))]
        GetResponseEntity NotificationManual(DeviceEntity deviceEntity);

       [OperationContract]
       [WebInvoke(UriTemplate = "PredictiveNotication", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
       [ServiceKnownType(typeof(IEnumerable<DeviceEntity>))]
       GetResponseEntity PredictiveNotication(PredictiveNotificationEntity predictiveNotificationEntity);

       [OperationContract]
       [WebInvoke(UriTemplate = "GetPredictiveNotication", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
       [ServiceKnownType(typeof(IEnumerable<PredictiveNotificationEntity>))]
       GetResponseEntity GetPredictiveNotication();

        [OperationContract]
        [WebInvoke(UriTemplate = "GetRestaurants", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(IEnumerable<UserFavouriteEntity>))]
        GetResponseEntity GetRestaurants();

        [OperationContract]
        [WebInvoke(UriTemplate = "GetSubCategoryById{Id}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(SubCategoryEntity))]
        GetResponseEntity GetSubCategoryById(string Id);

        [OperationContract]
        [WebInvoke(UriTemplate = "AddSubCategory", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity AddSubCategory(SubCategoryEntity subCategoryEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "UpdateSubCategory", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity UpdateSubCategory(SubCategoryEntity subCategoryEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "DeleteSubCategory", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity DeleteSubCategory(SubCategoryEntity subCategoryEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetOptCategoryById{Id}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [ServiceKnownType(typeof(OptionalCategoryEntity))]
        GetResponseEntity GetOptCategoryById(string Id);

        [OperationContract]
        [WebInvoke(UriTemplate = "AddOptionalCategory", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity AddOptionalCategory(OptionalCategoryEntity optCategoryEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "UpdateOptCategory", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity UpdateOptCategory(OptionalCategoryEntity optCategoryEntity);

        [OperationContract]
        [WebInvoke(UriTemplate = "DeleteOptCategory", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResponseEntity DeleteOptCategory(OptionalCategoryEntity optCategoryEntity);
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
