using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Utils.Models
{
    public partial class RoleSideMenuDataModel
    {
        public string Id { get; set; }

        public string RoleId { get; set; }

        public string SideMenuId { get; set; }
        
        public bool IsChecked { get; set; }

        public string SideMenuName { get; set; }
        public string SideMenuDescription { get; set; }

        //public List<RoleSideMenuDataModel> roleSideMenuList { get; set; }

        //public bool Status { get; set; }

    }
}
