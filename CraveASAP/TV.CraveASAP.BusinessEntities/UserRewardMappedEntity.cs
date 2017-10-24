using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public partial class UserRewardMappedEntity
    {
        //public UserRewardMappedEntity()
        //{
        //    this.Users = new HashSet<UserAppEntity>();
        //    this.Rewards = new HashSet<RewardEntity>();
        //}

        public int userRewardId { get; set; }
        public Nullable<int> rewardId { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<bool> isUsed { get; set; }

       



    }
}
