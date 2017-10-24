using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public partial class UserEntity
    {
        public UserEntity()
        {
            this.UserPromotions = new HashSet<UserPromotionEntity>();
            this.UserRewards = new HashSet<UserRewardEntity>();
            this.UserLocations = new HashSet<UserLocationEntity>();
            this.UserPrefrences = new HashSet<UserPrefrencesEntity>();
        }
        public string oAuthkey { get; set; }
        public int userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string profilePicture { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public Nullable<System.DateTime> createdDate { get; set; }
        public Nullable<int> points { get; set; }
        public string facebookId { get; set; }
        public Nullable<System.DateTime> facebookInviteDate { get; set; }
        public Nullable<int> facebookInviteCount { get; set; } 
        public Nullable<System.DateTime> instagramInviteDate { get; set; }
        public Nullable<int> instagramInviteCount { get; set; }
        public Nullable<System.DateTime> twitterInviteDate { get; set; }
        public Nullable<int> twitterInviteCount { get; set; }
        public Nullable<System.DateTime> dealFacebookInviteDate { get; set; }
        public Nullable<int> dealFacebookInviteCount { get; set; }
        public Nullable<System.DateTime> dealInstagramInviteDate { get; set; }
        public Nullable<int> dealInstagramInviteCount { get; set; }
        public Nullable<System.DateTime> dealTwitterInviteDate { get; set; }
        public Nullable<int> dealTwitterInviteCount { get; set; }
        public string language { get; set; }
        public string deviceToken { get; set; }
        public string deviceType { get; set; }
        public string devicePlatform { get; set; }
        public string appType { get; set; }
        public Nullable<System.DateTime> usePromoDate { get; set; }
        public Nullable<int> useCount { get; set; }
        public Nullable<System.DateTime> likePromoDate { get; set; }
        public Nullable<int> likeCount { get; set; }
        public string currentdate { get; set; }

        public virtual ICollection<UserPromotionEntity> UserPromotions { get; set; }
        public virtual ICollection<UserRewardEntity> UserRewards { get; set; }
        public virtual ICollection<UserLocationEntity> UserLocations { get; set; }
        public virtual ICollection<UserPrefrencesEntity> UserPrefrences { get; set; }
    }
}
