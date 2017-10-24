using System.Web;
using System.Web.Mvc;

namespace TV.CraveASAP.Web.VendorPortal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}