using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using TradeSystem.Framework.Entities;

namespace TradeSystem.Framework.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }
        public string Description { get; set; }
        public virtual ICollection<RoleSideMenu> RoleSideMenus { get; set; }

        public virtual ICollection<CompanyUser> CompanyUsers { get; set; }
    }
}

