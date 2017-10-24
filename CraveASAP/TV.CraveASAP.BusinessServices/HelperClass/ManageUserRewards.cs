using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.CraveASAP.BusinessEntities;
using TV.CraveASAP.DataModel.UnitOfWork;

namespace TV.CraveASAP.BusinessServices.HelperClass
{
    public class ManageUserRewards
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public ManageUserRewards()
        {
            _unitOfWork = new UnitOfWork();
        }

        public void ManageRewards(int userId)
        {
            var reward = _unitOfWork.RewardRepository.GetAll().ToList();
            var userRewards = _unitOfWork.UserRewardRepository.GetByCondition(x => x.userId == userId).ToList();
            foreach (var item in reward)
            {
                foreach (var userItem in userRewards)
                {
                    if (item.rewardId == userItem.rewardId)
                    { 
                        //Check if the time has over after the user has activated the reward
                        DateTime dtCurrent = DateTime.Now;
                        DateTime dtUsedTime = Convert.ToDateTime(userItem.useTime);
                        TimeSpan span = dtCurrent.Subtract(dtUsedTime);
                        double hours = span.TotalHours;
                        if (hours >= Convert.ToDouble(item.expiryHours))
                        {
                            var userToUpdate = _unitOfWork.UserRewardRepository.GetByID(userItem.userRewardId);
                            if (hours >= Convert.ToDouble(item.nextAvailability) * 24)
                            {
                                //Delete from UserRewards where userRewardId = userItem.userRewardId
                                _unitOfWork.UserRewardRepository.Delete(userToUpdate);
                            }
                            else
                            {
                                //Update query to update userRewards with isUsed = true
                                userToUpdate.isUsed = true;
                                _unitOfWork.UserRewardRepository.Update(userToUpdate);
                            }
                        }
                    }
                }
            }
        }
    }
}
