using System.Web.Mvc;

namespace TradeSystem.WebApi.Helpers
{
    public static class HtmlHelpers
    {
        public static string IsActive(this HtmlHelper html, bool compare)
        {
            return compare ? "active" : "";
        }
    }
}