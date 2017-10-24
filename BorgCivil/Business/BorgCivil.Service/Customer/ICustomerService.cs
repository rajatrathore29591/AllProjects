using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface ICustomerService : IService
    {
        List<Customer> GetAllCustomer();

        Customer GetCustomerByCustomerId(Guid CustomerId);

        string AddCustomer(CustomerDataModel CustomerDataModel);

        string UpdateCustomer(CustomerDataModel CustomerDataModel);

        bool DeleteCustomer(Guid CustomerId);

        List<CustomerSelectListModel> GetCustomerList();
    }
}
