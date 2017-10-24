using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;
using TradeSystem.Repositories;
using TradeSystem.Utils.Models;

namespace TradeSystem.Service
{
    public class ActivityService : BaseService, IActivityService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public ActivityService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        /// <summary>
        /// method used to get company detail by CompanyId
        /// </summary>
        /// <param name="companyUserId"></param>
        /// <returns></returns>
        public List<ActivityLog> GetActivityByCompanyUserId(Guid companyUserId)
        {
            try
            {
                //map entity.
                return unitOfWork.ActivityLogRepository.SearchBy<ActivityLog>(x => x.CompanyUserId == companyUserId).OrderByDescending(y => y.CreatedDate).Take(10).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to log activity performs by admin
        /// </summary>
        /// <param name="activityObj"></param>
        /// <returns></returns>
        public bool AddActivity(ActivityLogDataModel activityObj)
        {
            try
            {
                var response = string.Empty;
                //map entity.
                ActivityLog entity = new ActivityLog()
                {
                    Id = Guid.NewGuid(),
                    CompanyUserId = new Guid(activityObj.CompanyUserId),
                    Activity = activityObj.Activity,
                    Description = activityObj.Description,
                    IsCompanyUser = activityObj.IsCompanyUser,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };
                //add record from in database.
                unitOfWork.ActivityLogRepository.Insert<ActivityLog>(entity);
                ////save changes in database.
                unitOfWork.ActivityLogRepository.Commit();
                
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
