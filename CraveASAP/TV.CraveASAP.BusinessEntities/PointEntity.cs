using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public partial class PointEntity
    {
        public int pointId { get; set; }
        public Nullable<int> quantity { get; set; }
        public string type { get; set; }
    }
}
