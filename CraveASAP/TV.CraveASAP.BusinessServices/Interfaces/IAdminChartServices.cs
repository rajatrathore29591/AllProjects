using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.CraveASAP.BusinessEntities;

namespace TV.CraveASAP.BusinessServices.Interfaces
{
    public interface IAdminChartServices
    {
        string GetAllChart();
        string GetAllChartByVendorID(string id, string fromDate, string toDate);
        string GetAllChartByTime(string id, string CategoryType, string time, string fromDate, string toDate);
        string GetTrackUsage();
        string GetMultipleUsage();
        string GetSocialMedia();
        string GetUserPerDayUse();
    }
}
