using BorgCivil.Framework.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace BorgCivil.Framework.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }
        public string Description { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }

        //public virtual ICollection<CompanyUser> CompanyUsers { get; set; }
    }
}

