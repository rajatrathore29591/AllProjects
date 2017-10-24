using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.CraveASAP.BusinessEntities;

namespace TV.CraveASAP.BusinessServices.Interfaces
{
    public interface IAppDefaultLandingPageServices
    {
        IEnumerable<CategoryEntity> GetLandingDetails();
        bool UpdateLandingDetails(int Id);
        string GetLandingDetailApp(string time);
        string GetLandingDetailTime(string time);
    }
}
