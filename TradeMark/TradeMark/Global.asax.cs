using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace TradeMark
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DeleteSavedSearchScheduler();
        }

        /// <summary>
        /// register dashboard scheduler
        /// </summary>
        public static void DeleteSavedSearchScheduler()
        {
            //currently time is static
            var timeInterval = ConfigurationManager.AppSettings["TimeInterval"]; 
            var waitHandle = new AutoResetEvent(false);
            ThreadPool.RegisterWaitForSingleObject(waitHandle,
            // Method to execute
            (state, timeout) =>
            {
                // TODO: implement the functionality you want to be executed
                Utility.Utility.DeleteSavedSearch();
            },
            // optional state object to pass to the method
            null,
            // Execute the method after 30 seconds
            TimeSpan.FromSeconds(Convert.ToInt32(timeInterval)),
            // Set this to false to execute it repeatedly every 5 seconds
            false
            );
        }
    }
}