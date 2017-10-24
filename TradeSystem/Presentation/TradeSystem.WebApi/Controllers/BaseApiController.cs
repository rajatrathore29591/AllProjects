using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace TradeSystem.WebApi.Controllers
{
    public class BaseApiController : ApiController
    {
        //For log file manage for api
        public ILog logger = log4net.LogManager.GetLogger(typeof(ApiController));

        // Define resoures language variable 
        public ResourceManager resmanager = new ResourceManager(typeof(TradeSystem.WebApi.App_GlobalResources.Languages));
        public string projectUrl = WebConfigurationManager.AppSettings["ProjectUrl"];
    }

    public class AccountService
    {
        public static void testmylog()
        {
            ILog logger2 = log4net.LogManager.GetLogger(typeof(BaseController));
        }
    }
}