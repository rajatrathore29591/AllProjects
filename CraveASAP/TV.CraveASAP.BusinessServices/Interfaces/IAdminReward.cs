using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.CraveASAP.BusinessEntities;

namespace TV.CraveASAP.BusinessServices.Interfaces
{
  public  interface IAdminReward
    {
         IEnumerable<RewardEntity> GetALLReward();
         IEnumerable<RewardEntity> GetALLRewardByType(string type);
         RewardEntity GetRewardById(int ID);
         int CreateReward(RewardEntity rewardEntity);
         bool UpdateReward(RewardEntity rewardEntity);
         bool DeleteReward(int Id);
         IEnumerable<UserEntity> GetRandomUsers(string num);
         RewardEntity GetUserCount();

    }
}
