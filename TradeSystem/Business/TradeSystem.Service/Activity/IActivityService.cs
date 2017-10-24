using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;
using TradeSystem.Utils.Models;

namespace TradeSystem.Service
{
    public interface IActivityService : IService
    {
        bool AddActivity(ActivityLogDataModel activityObj);
        List<ActivityLog> GetActivityByCompanyUserId(Guid companyUserId);
    }
}
