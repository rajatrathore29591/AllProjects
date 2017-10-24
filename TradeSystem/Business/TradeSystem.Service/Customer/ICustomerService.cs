using System;
using System.Collections.Generic;
using TradeSystem.Framework.Entities;
using TradeSystem.Utils.Models;

namespace TradeSystem.Service
{
    public interface ICustomerService : IService
    {
        Customer GetCustomerByEmailPassword(string email, string password);
        Customer GetCustomerByEmail(string email);
        List<Customer> GetAllCustomer();
        Customer GetCustomerById(Guid customerId);
        List<Customer> GetCustomerListByCountryId(Guid countryId);
        List<Customer> GetCustomerListByStateId(Guid stateId);
        List<Customer> GetAllReferalCustomerByCustomerId(Guid customerReferalId);
        List<SelectListModel> GetAllCustomerSelectList();
        //string AddCustomer(Guid? customerReferalId, Guid? countryId, Guid? stateId, string firstName, string middleName, string lastName, bool isActive, string email, string password, string phone, string userName, string motherLastName, DateTime birthDate, string RFC, Guid? bankId, string bankAccount, string clabe, string benificaryName, int wrongAttempt, bool isLocked);
        string AddCustomer(CustomerDataModel customerData);
        bool EditCustomer(CustomerDataModel customerDataModel);
        bool EditCustomerProfile(CustomerDataModel customerDataModel);
        bool EditCustomerPassword(Guid id, string newPassword);
        bool UpdateLogo(Guid id, Guid? documentId);
        Customer GetCustomerByAspNetUserId(string aspNetUserId);
        bool EditCustomerByLogin(LoginDataModel loginDataModel);
    }
}
