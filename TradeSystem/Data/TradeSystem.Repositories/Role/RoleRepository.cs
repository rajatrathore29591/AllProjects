using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Validation;
using TradeSystem.Framework.Identity;
using TradeSystem.Framework.Entities;

namespace TradeSystem.Repositories
{
    public class RoleRepository : Repository, IRoleRepository
    {
        public ApplicationRole GetRoleById(Guid roleId)
        {
            try
            {
                ////get all active records
                return FindBy<ApplicationRole>(x => x.Id == roleId.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetTaskByTaskId by taskid.
        /// </summary>
        /// <param name="id">Task Id</param>
        /// <returns>Object Task entity</returns>
        public List<CompanyUser> GetRoleCompanyUserByRoleId(Guid roleId)
        {
            try
            {
                string _roleId = roleId.ToString();
                ////return object entity
                var result = SearchBy<CompanyUser>(x => x.RoleId == _roleId).ToList();
                return result != null ? result : new List<CompanyUser>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
