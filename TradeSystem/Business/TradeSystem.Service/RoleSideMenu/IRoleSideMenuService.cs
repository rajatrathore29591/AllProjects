using TradeSystem.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Service;

namespace TradeSystem.Service
{
    public interface IRoleSideMenuService : IService
    {
        List<RoleSideMenu> GetRoleSideMenuByRoleId(Guid roleId);
        bool AddRoleSideMenu(Guid sideMenuId, string roleId);
        bool DeleteRoleSideMenu(Guid roleId);
    }
}   
