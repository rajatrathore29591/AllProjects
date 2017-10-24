using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public class AppDefaultLandingPageEntity
    {
        public int landingPageId { get; set; }
        public string type { get; set; }
        public string fromTime { get; set; }
        public string toTime { get; set; }
        public int categoryId { get; set; }
        public string categoryStatus { get; set; }

       
    }
}
