using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BorgCivil.Service
{
    public class CustomerService : ICustomerService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public CustomerService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region CustomerServices

        /// <summary>
        /// getting all customer from customer table
        /// </summary>
        /// <returns></returns>
        public List<Customer> GetAllCustomer()
        {
            try
            {
                return unitOfWork.CustomerRepository.SearchBy<Customer>(x => x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting customer detail by customerId
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public Customer GetCustomerByCustomerId(Guid CustomerId)
        {
            try
            {
                //map entity.
                return unitOfWork.CustomerRepository.GetById<Customer>(CustomerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add data in customer table
        /// </summary>
        /// <param name="CustomerDataModel"></param>
        /// <returns></returns>
        public string AddCustomer(CustomerDataModel CustomerDataModel)
        {
            try
            {
                // created new guid
                var Id = Guid.NewGuid();

                //map entity.
                Customer entity = new Customer()
                {
                    CustomerId = Id,
                    CustomerName = CustomerDataModel.CustomerName,
                    ABN = CustomerDataModel.ABN,
                    OfficeStreet = CustomerDataModel.OfficeStreet,
                    OfficeState = CustomerDataModel.OfficeState,
                    OfficeSuburb = CustomerDataModel.OfficeSuburb,
                    PostalSuburb = CustomerDataModel.PostalSuburb,
                    OfficePostalCode = CustomerDataModel.OfficePostalCode,
                    PostalState = CustomerDataModel.PostalState,
                    PostalPostCode = CustomerDataModel.PostalPostCode,
                    PhoneNumber1 = CustomerDataModel.PhoneNumber1,
                    PhoneNumber2 = CustomerDataModel.PhoneNumber2,
                    MobileNumber1 = CustomerDataModel.MobileNumber1,
                    MobileNumber2 = CustomerDataModel.MobileNumber2,
                    Fax = CustomerDataModel.Fax,
                    ContactName = CustomerDataModel.ContactName,
                    ContactNumber = CustomerDataModel.ContactNumber,
                    AccountsContact = CustomerDataModel.AccountsContact,
                    AccountsNumber = CustomerDataModel.AccountsNumber,
                    EmailForInvoices = CustomerDataModel.EmailForInvoices,
                    IsActive = CustomerDataModel.IsActive,
                    CreatedBy = CustomerDataModel.CreatedBy != null ? CustomerDataModel.CreatedBy : (Guid?)null,
                    EditedBy = CustomerDataModel.EditedBy != null ? CustomerDataModel.EditedBy : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow
                };
                //add record from in database.
                unitOfWork.CustomerRepository.Insert<Customer>(entity);

                ////save changes in database.
                unitOfWork.CustomerRepository.Commit();

                return Id.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to update the customer tables
        /// </summary>
        /// <param name="CustomerDataModel"></param>
        /// <returns></returns>
        public string UpdateCustomer(CustomerDataModel CustomerDataModel)
        {
            try
            {
                ////get customer data by CustomerId.
                var CustomerDetail = GetCustomerByCustomerId(CustomerDataModel.CustomerId);

                ////case of not null return 
                if (CustomerDetail != null)
                {
                    ////map entity.
                    CustomerDetail.CustomerName = CustomerDataModel.CustomerName;
                    CustomerDetail.ABN = CustomerDataModel.ABN;
                    CustomerDetail.OfficeStreet = CustomerDataModel.OfficeStreet;
                    CustomerDetail.OfficeState = CustomerDataModel.OfficeState;
                    CustomerDetail.OfficeSuburb = CustomerDataModel.OfficeSuburb;
                    CustomerDetail.PostalSuburb = CustomerDataModel.PostalSuburb;
                    CustomerDetail.OfficePostalCode = CustomerDataModel.OfficePostalCode;
                    CustomerDetail.PostalState = CustomerDataModel.PostalState;
                    CustomerDetail.PostalPostCode = CustomerDataModel.PostalPostCode;
                    CustomerDetail.PhoneNumber1 = CustomerDataModel.PhoneNumber1;
                    CustomerDetail.PhoneNumber2 = CustomerDataModel.PhoneNumber2;
                    CustomerDetail.MobileNumber1 = CustomerDataModel.MobileNumber1;
                    CustomerDetail.MobileNumber2 = CustomerDataModel.MobileNumber2;
                    CustomerDetail.Fax = CustomerDataModel.Fax;
                    CustomerDetail.ContactName = CustomerDataModel.ContactName;
                    CustomerDetail.ContactNumber = CustomerDataModel.ContactNumber;
                    CustomerDetail.AccountsContact = CustomerDataModel.AccountsContact;
                    CustomerDetail.AccountsNumber = CustomerDataModel.AccountsNumber;
                    CustomerDetail.EmailForInvoices = CustomerDataModel.EmailForInvoices;
                    CustomerDetail.IsActive = CustomerDataModel.IsActive;
                    CustomerDetail.CreatedBy = CustomerDataModel.CreatedBy != null ? CustomerDataModel.CreatedBy : (Guid?)null;
                    CustomerDetail.EditedBy = CustomerDataModel.EditedBy != null ? CustomerDataModel.EditedBy : (Guid?)null;
                    CustomerDetail.CreatedDate = System.DateTime.UtcNow;

                    ////update record into database entity.
                    unitOfWork.CustomerRepository.Update<Customer>(CustomerDetail);

                    ////save changes in database.
                    unitOfWork.CustomerRepository.Commit();

                    ////case of update.
                    return CustomerDetail.CustomerId.ToString();
                }
                else
                {
                    ////case of null return null object.
                    return "error";
                }
            }
            catch (Exception ex)
            {
                ////case of error throw
                throw ex;
            }
        }

        /// <summary>
        /// delete customer from customer table
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public bool DeleteCustomer(Guid CustomerId)
        {
            try
            {
                ////get customer data by CustomerId.
                var Customer = GetCustomerByCustomerId(CustomerId);

                ////case of not null return 
                if (Customer != null)
                {
                    ////update record into database entity.
                    unitOfWork.CustomerRepository.Delete<Customer>(Customer);

                    ////save changes in database.
                    unitOfWork.CustomerRepository.Commit();

                    ////case of update.
                    return true;
                }
                else
                {
                    ////case of null return null object.
                    return false;
                }
            }
            catch (Exception ex)
            {
                ////case of error throw
                throw ex;
            }
        }

        /// <summary>
        /// This method for Get Customer for select list type controls [Text, Value] pair.
        /// </summary>
        /// <returns>List of Select Value from Customer entity.</returns>
        public List<CustomerSelectListModel> GetCustomerList()
        {
            try
            {
                ////get all customer records
                var types = unitOfWork.CustomerRepository.GetAll<Customer>().ToList();

                ////check types is null or empity
                if (types != null)
                {
                    ////map entity to model.
                    return types.Select(item => new CustomerSelectListModel
                    {
                        Text = item.CustomerName,
                        Value = item.CustomerId.ToString()
                    }
                   ).ToList();
                }
                return new List<CustomerSelectListModel>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
