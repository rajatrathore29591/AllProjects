using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BorgCivil.Utils
{
    public static class LogHelper
    {
        public partial class CommonLog
        {
            public string Method { get; set; }
            public string Exception { get; set; }
            public Exception InnerException { get; set; }

        }
    }
}
