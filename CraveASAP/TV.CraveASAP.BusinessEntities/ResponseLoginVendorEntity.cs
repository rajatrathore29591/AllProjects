using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public class ResponseLoginVendorEntity
    {
        public int id { get; set; }
        public virtual ICollection<AssignedVendorEntity> assignedVendor { get; set; }

    }
}
