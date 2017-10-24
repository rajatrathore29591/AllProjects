using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
  public partial  class UserRewardEntity
    {
        public int userRewardId { get; set; }
        public Nullable<int> rewardId { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<bool> isUsed { get; set; }
        public string useTime { get; set; }

        public virtual UserEntity UserEntity { get; set; }
    }
}
