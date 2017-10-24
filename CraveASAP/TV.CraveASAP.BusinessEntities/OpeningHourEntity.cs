using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
  public partial  class OpeningHourEntity
    {
        public int openingHoursId { get; set; }
        public string fromday { get; set; }
        public string today { get; set; }
        public string fromtime { get; set; }
        public string totime { get; set; }
        public int fkvendorId { get; set; }
    
    }
}
