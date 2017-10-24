
using System;
using System.Collections.Generic;
using TradeSystem.Framework.Entities;
using TradeSystem.Framework.Identity;

namespace TradeSystem.Service
{
    public interface IRoleService : IService
    {
        ApplicationRole GetRoleById(Guid roleId);
        bool GetRoleCompanyUserByRoleId(Guid roleId);
        // DeleteRoleCompanyUser(Guid roleid);
    }
}
