using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;

namespace TradeSystem.Service
{
    public interface ICompanyUserService : IService
    {
        List<CompanyUser> GetAllCompanyUser();
        CompanyUser GetCompanyUserByCompanyUserId(Guid CompanyUserId);
        CompanyUser GetCompanyUserByEmailPassword(string userName, string password);
        CompanyUser GetCompanyUserByAspNetUserId(string aspNetUserId);
        CompanyUser GetCompanyUserByEmail(string email);
        string AddCompanyUser(string roleId, string UserId, string companyName, string firstName, string middleName, string lastName, string email, string phone, bool isActive, string userName, string password, string address, bool isSuperAdmin);
        bool EditCompanyUser(Guid id, string roleId, string firstName, string middleName, string lastName, string email, string phone, string address, bool isActive);
        bool DeleteCompanyUser(Guid id);
        bool EditCompanyUserPassword(Guid Id, string newPassword);
      
    }
}
