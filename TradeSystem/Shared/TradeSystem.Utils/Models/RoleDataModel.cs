using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Utils.Models
{
    public partial class RoleDataModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string RoleSideMenu { get; set; }
        public string SideMenuId { get; set; }

        public bool isChcked { get; set; }
        public string RoleId { get; set; }

        public bool RoleExistInCompanyUser { get; set; }
        //public bool Status { get; set; }

    }
}
