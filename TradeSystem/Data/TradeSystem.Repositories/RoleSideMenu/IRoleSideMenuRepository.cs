﻿using TradeSystem.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Repositories
{
    public interface IRoleSideMenuRepository<T> : IRepository
    {
        List<RoleSideMenu> GetRoleSideMenuListByRoleId(Guid roleId);
    }
}   
