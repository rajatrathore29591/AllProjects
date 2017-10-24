using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public partial class SuperAdminLoginEntity
    {
        public int loginId { get; set; }
        public string loginName { get; set; }
        public string password { get; set; }
        public string oldPassword { get; set; }

    }
}
