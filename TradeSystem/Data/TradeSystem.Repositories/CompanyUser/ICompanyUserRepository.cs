using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;

namespace TradeSystem.Repositories
{
    public interface ICompanyUserRepository : IRepository
    {
        List<CompanyUser> GetAllCompanyUserList();
        CompanyUser GetCompanyUserByCompanyUserId(Guid id);

        //CompanyUser GetRoleCompanyUserByRoleId(Guid roleId);
    }
}
