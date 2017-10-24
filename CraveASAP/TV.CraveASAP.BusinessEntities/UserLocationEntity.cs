using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
   public  class UserLocationEntity
    {
        public int userLocationId { get; set; }
        public int fkUser { get; set; }
        public Nullable<bool> isDefault { get; set; }
        public string mealTime { get; set; }
        public string locationAddress { get; set; }
        public string locationLat { get; set; }
        public string locationLong { get; set; }
        public Nullable<bool> isCurrent { get; set; }
        public virtual UserEntity UserEntity { get; set; }
       
    }
}
