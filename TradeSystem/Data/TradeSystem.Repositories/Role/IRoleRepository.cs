using TradeSystem.Framework.Entities;
using System.Collections.Generic;
using TradeSystem.Framework.Identity;
using System;

namespace TradeSystem.Repositories
{
    public interface IRoleRepository : IRepository
    {
        ApplicationRole GetRoleById(Guid roleId);

        List<CompanyUser> GetRoleCompanyUserByRoleId(Guid roleId);
    }
}
