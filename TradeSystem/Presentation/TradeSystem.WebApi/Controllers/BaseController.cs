using log4net;
using System;
using System.Net.Http;
using System.Resources;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using TradeSystem.MVCWeb;

namespace TradeSystem.WebApi.Controllers
{
    public class BaseController : Controller
    {
        //For log file manage for api
        public ILog logger = log4net.LogManager.GetLogger(typeof(BaseController));
        public HttpClient client;

        //for language manage 
        public ResourceManager resmanager = new ResourceManager(typeof(TradeSystem.WebApi.App_GlobalResources.Languages));
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
            {
                lang = langCookie.Value;
            }
            else
            {
                var userLanguage = Request.UserLanguages;
                var userLang = userLanguage != null ? userLanguage[0] : "";
                if (userLang != "")
                {
                    lang = userLang;
                }
                else
                {
                    lang = SiteLanguages.GetDefaultLanguage();
                }
            }

            new SiteLanguages().SetLanguage(lang);

            
            if(Session["Access_Token"] != null)
            {
                // adding auth token in header
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Access_Token"]);
            }

            return base.BeginExecuteCore(callback, state);
        }
    }
}