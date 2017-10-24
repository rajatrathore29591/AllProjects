using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
  public class DeviceEntity
    {
       public string Device { get; set; }
       public string DeviceType { get; set; }
       public string AppType { get; set; }
       public string Sound { get; set; }
       public string Alert { get; set; }
       public string MessageEnglish { get; set; }
       public string MessageThai { get; set; }
       public int badge { get; set; }
       public string type { get; set; }
       public string vendorId { get; set; }
       public string subCategory { get; set; }
       public string optCategory { get; set; }
    }
}
