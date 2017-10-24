using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipm.Hub.Utilities
{
    /// <summary>
    /// This class for create authorization token for access api.
    /// </summary>
    public static class AppHelper
    {

        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
