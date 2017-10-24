using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Service;
using TradeSystem.Framework.Entities;
using TradeSystem.Repositories;
using Microsoft.AspNet.Identity;



namespace TradeSystem.Service
{
    public class CompanyUserService : ICompanyUserService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;
        private Framework.Identity.ApplicationUserManager _roleManager;

        //Initialized Parameterized Constructor.
        public CompanyUserService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        // Repository object 
        CompanyUserRepository companyUserRepository = new CompanyUserRepository();
        //public Framework.Identity.ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _roleManager ?? Request.GetOwinContext().GetUserManager<Framework.Identity.ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        _roleManager = value;
        //    }
        //}

        #endregion

        /// This method for Get All CompanyUser.
        /// </summary>
        /// <returns>CompanyUsers</returns>
        public List<CompanyUser> GetAllCompanyUser()
        {
            try
            {
                return unitOfWork.CompanyUserRepository.SearchBy<CompanyUser>(x => x.IsSuperAdmin == false).OrderByDescending(x => x.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method is used for getting company user detail by Email password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public CompanyUser GetCompanyUserByEmailPassword(string userName, string password)
        {
            try
            {
                //return single row corresponding to userName & password
                return unitOfWork.CompanyUserRepository.FindBy<CompanyUser>(x => x.Email == userName && x.Password == password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method is used for getting company user detail by UserId
        /// </summary>
        /// <param name="aspNetUserId"></param>
        /// <returns></returns>
        public CompanyUser GetCompanyUserByAspNetUserId(string aspNetUserId)
        {
            try
            {
                //return single row corresponding to userName & password
                return unitOfWork.CompanyUserRepository.FindBy<CompanyUser>(x => x.UserId == aspNetUserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get companyuser by id.
        /// </summary>
        /// <param name="id">companyUser Id</param>
        /// <returns>Object companyUser entity</returns>
        public CompanyUser GetCompanyUserByCompanyUserId(Guid CompanyUserId)
        {
            try
            {
                ////return object entity
                return unitOfWork.CompanyUserRepository.FindBy<CompanyUser>(CompanyUserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method is used for getting company user detail by Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public CompanyUser GetCompanyUserByEmail(string email)
        {
            try
            {
                //return single row corresponding to taskAssignmentId
                return unitOfWork.CompanyUserRepository.FindBy<CompanyUser>(x => x.Email == email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method use for Insert record into companyUser entity.
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <param name="companyName">companyName</param>
        /// <param name="firstName">firstName</param>
        /// <param name="lastName">lastName</param>
        /// <param name="email">email</param>
        /// <param name="phone">phone</param>
        /// <param name="isActive">isActive</param>
        /// <param name="userName">userName</param>
        /// <param name="password">password</param>
        /// <param name="address">address</param>
        /// <param name="isSuperAdmin">isSuperAdmin</param>
        /// <returns>result</returns>
        public string AddCompanyUser(string roleId, string UserId, string companyName, string firstName, string middleName, string lastName, string email, string phone, bool isActive, string userName, string password, string address, bool isSuperAdmin)
        {
            try
            {
                var response = string.Empty;
                //map entity.
                CompanyUser entity = new CompanyUser()
                {
                    Id = Guid.NewGuid(),
                    RoleId = roleId,
                    UserId = UserId,
                    CompanyName = companyName,
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    Email = email,
                    Phone = phone,
                    IsActive = isActive,
                    UserName = userName,
                    Password = password,
                    Address = address,
                    IsSuperAdmin = isSuperAdmin,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };
                //add record from in database.
                unitOfWork.CompanyUserRepository.Insert<CompanyUser>(entity);
                ////save changes in database.
                unitOfWork.CompanyUserRepository.Commit();
                response = entity.Id.ToString();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method use for updating companyUser record into companyUser entity.
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <param name="companyName">companyName</param>
        /// <param name="firstName">firstName</param>
        /// <param name="lastName">lastName</param>
        /// <param name="email">email</param>
        /// <param name="phone">phone</param>
        /// <param name="isActive">isActive</param>
        /// <param name="userName">userName</param>
        /// <param name="password">password</param>
        /// <param name="address">address</param>
        /// <param name="isSuperAdmin">isSuperAdmin</param>
        /// <returns>result</returns>
        public bool EditCompanyUser(Guid id, string roleId, string firstName, string middleName, string lastName, string email, string phone, string address, bool isActive)
        {
            try
            {
                //get existing record.
                CompanyUser entity = unitOfWork.CompanyUserRepository.GetCompanyUserByCompanyUserId(id);

                //check entity is null.
                if (entity != null)
                {
                    //Bind Role object in CompanyUser Property

                    //map entity
                    entity.RoleId = roleId;
                    entity.FirstName = firstName;
                    entity.MiddleName = middleName;
                    entity.LastName = lastName;
                    //entity.Email = email;
                    entity.Phone = phone;
                    entity.IsActive = isActive;
                    entity.Address = address;
                    entity.ModifiedDate = DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.CompanyUserRepository.Update<CompanyUser>(entity);

                    ////save changes in database.
                    unitOfWork.CompanyUserRepository.Commit();

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
        ///  This method use for delete record into Task entity.
        /// </summary>
        /// <param name="id">Task Id</param>
        /// <returns>result</returns>
        public bool DeleteCompanyUser(Guid id)
        {
            try
            {
                //get existing record.
                CompanyUser entity = companyUserRepository.GetCompanyUserByCompanyUserId(id);

                //check entity is null.
                if (entity != null)
                {
                    //delete record from existing entity in database.
                    unitOfWork.CompanyUserRepository.Delete<CompanyUser>(entity);

                    ////save changes in database.
                    unitOfWork.CompanyUserRepository.Commit();
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
       /// method used to update password by Id
       /// </summary>
       /// <param name="Id"></param>
       /// <param name="newPassword"></param>
       /// <returns></returns>
        public bool EditCompanyUserPassword(Guid Id, string newPassword)
        {
            try
            {
                //get existing record.
                CompanyUser entity = GetCompanyUserByCompanyUserId(Id);

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.Password = newPassword;
                    entity.ModifiedDate = DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.CompanyUserRepository.Update<CompanyUser>(entity);
                    ////save changes in database.
                    unitOfWork.CompanyUserRepository.Commit();

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
