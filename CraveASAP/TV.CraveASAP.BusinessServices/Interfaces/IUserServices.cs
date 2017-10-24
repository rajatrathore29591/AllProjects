using System.Collections.Generic;
using TV.CraveASAP.BusinessEntities;


namespace TV.CraveASAP.BusinessServices.Interfaces
{
    public interface IUserServices
    {
        UserEntity GetUserById(int productId);
        IEnumerable<UserEntity> GetAllUsers();
        int CreateUser(UserEntity userEntity);
        bool UpdateUser(int userId, UserEntity userEntity);
        bool DeleteUser(int userId);
        UserEntity UserLogIn(UserEntity loginUserEntity);
        UserEntity SkipUser(UserEntity loginUserEntity);
        IEnumerable<UserLocationEntity> UpdateMyLocation(IEnumerable<UserLocationEntity> userLocationEntity);
        bool DeleteMyLocation(UserLocationEntity userLocationEntity);
        string AddVenueToFavorite(UserFavouriteEntity userFavouriteEntity);
        IEnumerable<UserFavouriteEntity> UserFavRestaurant(int id);
        IEnumerable<UserAppEntity> GetUserByUserId(int UserID);
        IEnumerable<RewardEntity> GetAllReward(UserEntity userEntity);
        IEnumerable<PointEntity> GetAllPoint();
        IEnumerable<UserLocationEntity> GetMyLocation(int userId);
        IEnumerable<UserFavouriteEntity> UpdateUserFavouriteRest(int id);
        bool AddReward(UserRewardEntity userRewardEntity);
        bool UpdateReward(UserRewardEntity userRewardEntity, int userRewardId);
        bool DeleteVenueToFavorite(UserFavouriteEntity userFavouriteEntity);
        IEnumerable<PromotionsUserEntity> GetUserPromotionByUserId(int UserID, string deviceToken, string devicePlatform);
        bool DeletePromotionByUserId(int userId, int promotionId, string devicePlatform, string deviceToken);
    }
}
