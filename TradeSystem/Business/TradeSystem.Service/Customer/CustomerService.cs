using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TradeSystem.Framework.Entities;
using TradeSystem.Repositories;
using TradeSystem.Utils.Models;

namespace TradeSystem.Service
{
    public class CustomerService : ICustomerService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public CustomerService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        /// <summary>
        /// method is used to get customer detail by email and password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Customer GetCustomerByEmailPassword(string email, string password)
        {
            try
            {
                //return single row corresponding to taskAssignmentId
                return unitOfWork.CustomerRepository.FindBy<Customer>(x => x.Email == email && x.Password == password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method is used to get customer detail by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Customer GetCustomerByEmail(string email)
        {
            try
            {
                //return single row corresponding to taskAssignmentId
                return unitOfWork.CustomerRepository.FindBy<Customer>(x => x.Email == email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// This method for Get All Customer.
        /// </summary>
        /// <returns>Customer</returns>
        public List<Customer> GetAllCustomer()
        {
            try
            {
                return unitOfWork.CustomerRepository.GetAll<Customer>().OrderByDescending(x => x.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get customer by customerId.
        /// </summary>
        /// <param name="id">Customer Id</param>
        /// <returns>Object Customer entity</returns>
        public Customer GetCustomerById(Guid customerId)
        {
            try
            {
                ////return object entity
                return unitOfWork.CustomerRepository.FindBy<Customer>(customerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get customers by countryId.
        /// </summary>
        /// <param name="id">countryId</param>
        /// <returns>multiple customers by countryId</returns>
        public List<Customer> GetCustomerListByCountryId(Guid countryId)
        {
            try
            {
                ////return object entity
                return unitOfWork.CustomerRepository.SearchBy<Customer>(x => x.CountryId == countryId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get customers by stateId.
        /// </summary>
        /// <param name="id">stateId</param>
        /// <returns>multiple customers by stateId</returns>
        public List<Customer> GetCustomerListByStateId(Guid stateId)
        {
            try
            {
                ////return object entity
                return unitOfWork.CustomerRepository.SearchBy<Customer>(x => x.StateId == stateId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get all Referalcustomer by customerReferalId.
        /// </summary>
        /// <param name="customerReferalId">customerReferalId</param>
        /// <returns>Object Customer entity</returns>
        public List<Customer> GetAllReferalCustomerByCustomerId(Guid customerReferalId)
        {
            try
            {
                return unitOfWork.CustomerRepository.SearchBy<Customer>(x => x.CustomerReferalId == customerReferalId).OrderByDescending(x => x.ModifiedDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all customer select list 
        /// </summary>
        /// <returns></returns>
        public List<SelectListModel> GetAllCustomerSelectList()
        {
            try
            {
                var customer = unitOfWork.CustomerRepository.GetAll<Customer>().ToList();
                if (customer != null)
                {
                    ////map entity to model.
                    return customer.Select(item => new SelectListModel
                    {
                        Text = item.FirstName + " " + item.MiddleName + " " + item.LastName,
                        Value = item.Id.ToString()
                    }
                   ).ToList();
                }
                //map entity.
                return new List<SelectListModel>();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method use for Insert record into companyUser entity.
        /// </summary>
        /// <param name="customerReferalId">customerReferalId</param>
        /// <param name="firstName">firstName</param>
        /// <param name="middleName">middleName</param>
        /// <param name="lastName">lastName</param>
        /// <param name="isActive">isActive</param>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <param name="phone">phone</param>
        /// <param name="userName">userName</param>
        /// <param name="motherLastName">motherLastName</param>
        /// <param name="birthDate">birthDate</param>
        /// <param name="RFC">RFC</param>
        /// <param name="bankId">bankId</param>
        /// <param name="bankAccount">bankAccount</param>
        /// <param name="clabe">clabe</param>
        /// <param name="benificaryName">benificaryName</param>
        /// <param name="wrongAttempt">wrongAttempt</param>
        /// <param name="isLocked">isLocked</param>
        /// <returns>result</returns>
        public string AddCustomer(CustomerDataModel customerData)
        {
            try
            {
                DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                dtinfo.ShortDatePattern = "MM/dd/yyyy";
                dtinfo.DateSeparator = "/";

                var response = string.Empty;
                //map entity.
                Customer entity = new Customer()
                {
                    Id = Guid.NewGuid(),
                    UserId = customerData.UserId,
                    CustomerReferalId = customerData.CustomerReferalId != null ? new Guid(customerData.CustomerReferalId) : (Guid?)null,
                    CountryId = customerData.CountryId != null ? new Guid(customerData.CountryId) : (Guid?)null,
                    StateId = customerData.StateId != null ? new Guid(customerData.StateId) : (Guid?)null,
                    FirstName = customerData.FirstName,
                    MiddleName = customerData.MiddleName != null ? customerData.MiddleName : "",
                    LastName = customerData.LastName,
                    IsActive = customerData.IsActive,
                    Email = customerData.Email,
                    Password = customerData.Password,
                    Phone = customerData.Phone,
                    UserName = customerData.Email,
                    MotherLastName = customerData.MotherLastName,
                    BirthDate = Convert.ToDateTime(customerData.BirthDate, dtinfo),
                    RFC = customerData.RFC,
                    BankId = customerData.BankId != null ? new Guid(customerData.BankId) : (Guid?)null,
                    BankName = customerData.BankName,
                    BankAccount = customerData.BankAccount,
                    Clabe = customerData.Clabe,
                    BenificiaryName = customerData.BenificiaryName,
                    WrongAttempt = 0,
                    LastLogin = DateTime.UtcNow,
                    IsLocked = customerData.IsLocked != null ? true : false,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    MaxSaleEarningPercent = customerData.Commission
                };
                //add record from in database.
                unitOfWork.CustomerRepository.Insert<Customer>(entity);
                ////save changes in database.
                unitOfWork.CustomerRepository.Commit();
                response = entity.Id.ToString();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       /// <summary>
       /// method is used to update new password
       /// </summary>
       /// <param name="id"></param>
       /// <param name="newPassword"></param>
       /// <returns></returns>
        public bool EditCustomerPassword(Guid id, string newPassword)
        {
            try
            {
                //get existing record.
                //Customer entity = GetCustomerById(id);
                var entity = unitOfWork.CustomerRepository.FindBy<Customer>(x => x.Id == id);

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.Password = newPassword;
                    entity.ModifiedDate = DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.CustomerRepository.Update<Customer>(entity);
                    ////save changes in database.
                    unitOfWork.CustomerRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method use for updating customer data into customer entity.
        /// </summary>
        /// <param name="firstName">firstName</param>
        /// <param name="middleName">lastName</param>
        /// <param name = "lastName" > firstName </ param >
        /// <param name="email">lastName</param>
        /// <param name="phone">lastName</param>
        /// <param name="password">lastName</param>
        /// <returns>result</returns>
        public bool EditCustomer(CustomerDataModel customerDataModel)
        {
            try
            {
                //get existing record.
                Customer entity = GetCustomerById(new Guid(customerDataModel.Id));

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.IsActive = customerDataModel.IsActive;
                    entity.MaxSaleEarningPercent = customerDataModel.Commission;
                    entity.ModifiedDate = DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.CustomerRepository.Update<Customer>(entity);
                    ////save changes in database.
                    unitOfWork.CustomerRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method use for updating customer profile date into customer entity.
        /// </summary>
        /// <param name="firstName">firstName</param>
        /// <param name="middleName">lastName</param>
        /// <param name = "lastName" > firstName </ param >
        /// <param name="phone">lastName</param>
        /// <returns>result</returns>
        public bool EditCustomerProfile(CustomerDataModel customerDataModel)
        {
            try
            {
                //get existing record.
                Customer entity = GetCustomerById(new Guid(customerDataModel.Id));

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.FirstName = customerDataModel.FirstName;
                    entity.MiddleName = customerDataModel.MiddleName != null ? customerDataModel.MiddleName : "";
                    entity.LastName = customerDataModel.LastName;
                    entity.Phone = customerDataModel.Phone;
                    entity.OpenPayCustomerId = customerDataModel.OpenpayPaymentCustomerId;
                    entity.ModifiedDate = DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.CustomerRepository.Update<Customer>(entity);
                    ////save changes in database.
                    unitOfWork.CustomerRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  This method use for update customer logo.
        /// </summary>
        /// <returns>result true/false</returns>
        public bool UpdateLogo(Guid id, Guid? documentId)
        {
            try
            {
                ////get customer data by id.
                var existingCustomer = GetCustomerById(id);

                ////case of not null return customer data object.
                if (existingCustomer != null)
                {
                    if (documentId != Guid.Empty && documentId != null)
                    {
                        ////map entity.
                        existingCustomer.DocumentId = documentId;

                        ////update record into database entity.
                        unitOfWork.CustomerRepository.Update<Customer>(existingCustomer);

                        ////save changes in database.
                        unitOfWork.CustomerRepository.Commit();
                    }

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
        /// method is used to get customer detail by aspnetUserId
        /// </summary>
        /// <param name="aspNetUserId"></param>
        /// <returns></returns>
        public Customer GetCustomerByAspNetUserId(string aspNetUserId)
        {
            try
            {
                //return single row corresponding to userName & password
                return unitOfWork.CustomerRepository.FindBy<Customer>(x => x.UserId == aspNetUserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       /// <summary>
       /// method is used to update device token and device type
       /// </summary>
       /// <param name="loginDataModel"></param>
       /// <returns></returns>
        public bool EditCustomerByLogin(LoginDataModel loginDataModel)
        {
            try
            {

                //get existing record.
                Customer entity = GetCustomerByEmail(loginDataModel.Email);

                //check entity is null.
                if (entity != null)
                {

                    entity.DeviceToken = loginDataModel.DeviceToken;
                    entity.DeviceType = loginDataModel.DeviceType;
                    entity.ModifiedDate = DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.CustomerRepository.Update<Customer>(entity);
                    ////save changes in database.
                    unitOfWork.CustomerRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
