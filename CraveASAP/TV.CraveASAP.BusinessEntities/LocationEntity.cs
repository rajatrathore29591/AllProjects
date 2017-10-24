using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessServices
{
    class LocationEntity
    {
        public int locationId { get; set; }
        public string locationName { get; set; }
        public string city { get; set; }
        public string region { get; set; }
        public string address { get; set; }
    }
}
