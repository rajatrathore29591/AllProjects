using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;

namespace TradeSystem.Repositories
{
    public class CompanyUserRepository : Repository, ICompanyUserRepository
    {
        /// <summary>
        /// This method for Get All CompanyUser for select list type controls.
        /// </summary>
        /// <returns>List of Select Value from CompanyUser entity.</returns>
        public List<CompanyUser> GetAllCompanyUserList()
        {
            try
            {
                ////return  select value from Skill entity.
                return SearchBy<CompanyUser>(x => x.IsActive.Equals(true)).OrderBy(o => o.CompanyName).ToList();
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
        public CompanyUser GetCompanyUserByCompanyUserId(Guid companyUserId)
        {
            try
            {
                ////return object entity
                return FindBy<CompanyUser>(companyUserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///// <summary>
        ///// GetTaskByTaskId by taskid.
        ///// </summary>
        ///// <param name="id">Task Id</param>
        ///// <returns>Object Task entity</returns>
        //public CompanyUser GetRoleCompanyUserByRoleId(Guid roleId)
        //{
        //    try
        //    {
        //        ////return object entity
        //        return SearchBy<CompanyUser>(x => x.RoleId == roleId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
