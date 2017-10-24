using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;

namespace TradeSystem.Service
{
    public interface ISideMenuService : IService
    {
        List<SideMenu> GetAllSideMenu();
        SideMenu GetSideMenuById(Guid sideMenuId);
    }
}
