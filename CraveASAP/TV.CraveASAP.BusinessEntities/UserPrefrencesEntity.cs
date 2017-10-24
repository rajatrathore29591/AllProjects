using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
  public partial class UserPrefrencesEntity
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int categoryId { get; set; }
        public Nullable<int> prefrencesId { get; set; }
        public string type { get; set; }
        public Nullable<bool> isActive { get; set; }
        public virtual UserEntity UserEntity { get; set; }
        public string oAuthkey { get; set; }
        public string description { get; set; }
    
    }
}
