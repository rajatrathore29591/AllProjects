using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Routing;
using System.Web.Security;
using System.Web.Services.Description;
using System.Web.SessionState;
using TV.CraveASAP.BusinessServices;
using TV.CraveASAP.BusinessServices.HelperClass;

namespace CraveASAPRestServices
{
    public class Global : System.Web.HttpApplication
    {
        ScheduledJobs scheduledJobs = new ScheduledJobs();
        protected void Application_Start(object sender, EventArgs e)
        {
          //  RouteTable.Routes.Add(new ServiceRoute("",
          //new WebServiceHostFactory(),
          //typeof(CraveServices)));

            RouteTable.Routes.Add(new ServiceRoute("", new WebServiceHostFactory(), typeof(CraveServices)));
            //AddTask("DeletePromotions", 2592000);
            //AddTask("PredictiveNotification", 86400);
            AddTask("DeactivatePromotions",900);
        }
        private static CacheItemRemovedCallback OnCacheRemove = null;
        //private void AddTask(string name, int seconds)
        //{
        //    OnCacheRemove = new CacheItemRemovedCallback(CacheItemRemoved);
        //    HttpRuntime.Cache.Insert(name, seconds, null,
        //        DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration,
        //        CacheItemPriority.NotRemovable, OnCacheRemove);
        //}

        private void AddTask(string name, int seconds)
        {
            Thread thread = new Thread(() => TimeTask(name, seconds));
            thread.Start();
        }

        public void TimeTask(string name, int seconds)
        {
            var cSec = DateTime.Now.Minute % 15;
            OnCacheRemove = new CacheItemRemovedCallback(CacheItemRemoved);
            if (cSec == 0)
            {
                HttpRuntime.Cache.Insert(name, seconds, null,
                    DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration,
                    CacheItemPriority.NotRemovable, OnCacheRemove);
            }
            else
            {
                System.Threading.Thread.Sleep(60000);
                TimeTask(name, seconds);
            }
        }
        
        public void CacheItemRemoved(string k, object v, CacheItemRemovedReason r)
        {
            //if (k == "DeletePromotions")
            //    scheduledJobs.DeletePromotionsMonthly();
            if (k == "DeactivatePromotions")
                scheduledJobs.DeactivatePromotions();
            if (k == "PredictiveNotification")
                scheduledJobs.PredictiveNotification();
            // re-add our task so it recurs
            AddTask(k, Convert.ToInt32(v));
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();

            EnableCrossDmainAjaxCall();
        }

        private void EnableCrossDmainAjaxCall()
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "http://craveasapadmin.techvalens.net");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods",
                              "GET, POST, PUT, DELETE");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers",
                              "Content-Type, Accept, oAuthKey");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age",
                              "1728000");
                HttpContext.Current.Response.End();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}