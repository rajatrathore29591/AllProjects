using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;

namespace TradeSystem.Repositories
{
    public class RoleSideMenuRepository<T> : Repository, IRoleSideMenuRepository<RoleSideMenu>
    {

        /// <summary>
        /// This method for Get sidemenu by role id for select list type controls.
        /// </summary>
        /// <returns>List of Select Value from rolesidemenu entity.</returns>
        public List<RoleSideMenu> GetRoleSideMenuListByRoleId(Guid roleId)
        {
            try
            {
                ////return  select value from Skill entity.
                return SearchBy<RoleSideMenu>(x => x.RoleId == roleId.ToString()).ToList();
                //return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
