using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
   public class ContentManagementEntity
    {
        public int id { get; set; }
        public string contentName { get; set; }
        public string contentLink { get; set; }
        public Nullable<bool> isActive { get; set; }
    }
}
