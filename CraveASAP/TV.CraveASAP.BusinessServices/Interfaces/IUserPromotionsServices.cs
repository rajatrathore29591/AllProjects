using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.CraveASAP.BusinessEntities;


namespace TV.CraveASAP.BusinessServices.Interfaces
{
    public interface IUserPromotionsServices
    {
        IEnumerable<PromotionCodeEntity> GetOpenPromotions(int userId, string lang);
        IEnumerable<PromotionsUserEntity> GetPromotionCode(UserPromotionEntity userPromotion);
        IEnumerable<PromotionCodeEntity> GetPromotionDetails();
        IEnumerable<PromotionsUserEntity> GetAllDeals(int vendorType, int page, string userLocationLat, string userLocationLong, string langType, string resType, string userId);
        IEnumerable<PromotionsUserEntity> SelectRandom(int userID,string latitude,string longitude,string vendorType);
        IEnumerable<PromotionsUserEntity> GetICrave(int vendorType, string userLocationLat, string userLocationLong, int id,int page);
        //bool ViewedPromotion(int ID);
        UserFavouriteEntity ViewedPromotion(int ID, int vendorId, int userId);
        IEnumerable<PromotionsUserEntity> UsePromotion(int ID, int userId, string deviceToken, string devicePlatform);
        bool LikePromotion(int ID, int userId, bool like);
        bool ShareDeal(string ShareVia, int userId, int promotionId);
        IEnumerable<UserPointEntity> ShareApp(string ShareVia, int userId);
        IEnumerable<PromotionsUserEntity> GetAllDealWebApp();
        IEnumerable<VendorEntity> GetMapLocation();


    }
}
