using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public partial class RewardEntity
    {
        public int rewardId { get; set; }
        public string rewardName { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public string platform { get; set; }
        public string code { get; set; }
        public string endDate { get; set; }
        public string endDate1 { get; set; }
        public string expiryHours { get; set; }
        public Nullable<bool> isSpecial { get; set; }
        public string nextAvailability { get; set; }
        public string type { get; set; }
        public Nullable<int> usedCount { get; set; }
        public Nullable<bool> isActive { get; set; }
        public string language { get; set; }
        public string point { get; set; }
        public Nullable<bool> isUsed { get; set; }
        public int userRewardId { get; set; }
        public string useTime { get; set; }
        public string useTime1 { get; set; }
        public string hyperLink { get; set; }
        public string count { get; set; }
        public string message { get; set; }
        public string temp { get; set; }
        public string link { get; set; }
        public string serverTime { get; set; }
        public string serverTime1 { get; set; }

    }
}
