using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public partial class UserAppEntity
    {
        public int userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string profilePicture { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        //public Nullable<System.DateTime> createdDate { get; set; }
        public Nullable<int> points { get; set; }
        //public string facebookId { get; set; }
        //public Nullable<System.DateTime> facebookInviteDate { get; set; }
        //public Nullable<int> facebookInviteCount { get; set; }
        //public Nullable<System.DateTime> instagramInviteDate { get; set; }
        //public Nullable<int> instagramInviteCount { get; set; }
        //public Nullable<System.DateTime> twitterInviteDate { get; set; }
        //public Nullable<int> twitterInviteCount { get; set; }
        public string language { get; set; }
        public string deviceToken { get; set; }

        public int userRewardId { get; set; }
        public Nullable<int> rewardId { get; set; }
        public Nullable<bool> isUsed { get; set; }
        public Nullable<System.DateTime> useTime { get; set; }

        public string rewardName { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public string platform { get; set; }
        public string code { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        public string expiryHours { get; set; }
        public Nullable<bool> isSpecial { get; set; }
        public string nextAvailability { get; set; }
        public string type { get; set; }
        public Nullable<int> usedCount { get; set; }
        public string status { get; set; }

        public Nullable<int> earnedPoint { get; set; }
        //public string language { get; set; }

    }
}
