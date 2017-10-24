using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BorgCivil.Service
{
    public class EmployeeService : IEmployeeService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public EmployeeService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region EmployeeServices

        /// <summary>
        /// getting all employee from employee table
        /// </summary>
        /// <returns></returns>
        public List<Employee> GetAllEmployee()
        {
            try
            {
                return unitOfWork.EmployeeRepository.SearchBy<Employee>(x => x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting employee detail by employeeId
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public Employee GetEmployeeByEmployeeId(Guid EmployeeId)
        {
            try
            {
                //map entity.
                return unitOfWork.EmployeeRepository.GetById<Employee>(EmployeeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add data in employee table
        /// </summary>
        /// <param name="EmployeeDataModel"></param>
        /// <returns></returns>
        public string AddEmployee(EmployeeDataModel EmployeeDataModel)
        {
            try
            {
                // created new guid
                var Id = Guid.NewGuid();

                //map entity.
                Employee entity = new Employee()
                {
                    EmployeeId = Id,
                    UserId = EmployeeDataModel.UserId,
                    EmploymentCategoryId = EmployeeDataModel.EmploymentCategoryId != "" ? new Guid(EmployeeDataModel.EmploymentCategoryId) : (Guid?)null,
                    EmploymentStatusId = EmployeeDataModel.EmploymentStatusId != "" ? new Guid(EmployeeDataModel.EmploymentStatusId) : (Guid?)null,
                    RoleId = EmployeeDataModel.RoleId,
                    FirstName = EmployeeDataModel.FirstName,
                    SurName = EmployeeDataModel.SurName,
                    Address1 = EmployeeDataModel.Address1,
                    Address2 = EmployeeDataModel.Address2,
                    Password = EmployeeDataModel.Password,
                    Email = EmployeeDataModel.Email,
                    City = EmployeeDataModel.City,
                    ZipCode = EmployeeDataModel.ZipCode,
                    ContactNumber = EmployeeDataModel.ContactNumber,
                    CountryId = EmployeeDataModel.CountryId,
                    StateId = EmployeeDataModel.StateId,
                    IsActive = EmployeeDataModel.IsActive,
                    CreatedBy = EmployeeDataModel.CreatedBy != null ? EmployeeDataModel.CreatedBy : (Guid?)null,
                    EditedBy = EmployeeDataModel.EditedBy != null ? EmployeeDataModel.EditedBy : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow,
                };
                //add record from in database.
                unitOfWork.EmployeeRepository.Insert<Employee>(entity);

                ////save changes in database.
                unitOfWork.EmployeeRepository.Commit();

                return Id.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to update the employee tables
        /// </summary>
        /// <param name="EmployeeDataModel"></param>
        /// <returns></returns>
        public string UpdateEmployee(EmployeeDataModel EmployeeDataModel)
        {
            try
            {
                ////get employee data by EmployeeId.
                var EmployeeDetail = GetEmployeeByEmployeeId(EmployeeDataModel.EmployeeId);

                ////case of not null return 
                if (EmployeeDetail != null)
                {
                    ////map entity.
                    EmployeeDetail.EmploymentCategoryId = EmployeeDataModel.EmploymentCategoryId != null ? new Guid(EmployeeDataModel.EmploymentCategoryId) : (Guid?)null;
                    EmployeeDetail.EmploymentStatusId = EmployeeDataModel.EmploymentStatusId != null ? new Guid(EmployeeDataModel.EmploymentStatusId) : (Guid?)null;
                    EmployeeDetail.RoleId = EmployeeDataModel.RoleId;
                    EmployeeDetail.FirstName = EmployeeDataModel.FirstName;
                    EmployeeDetail.SurName = EmployeeDataModel.SurName;
                    EmployeeDetail.Address1 = EmployeeDataModel.Address1;
                    EmployeeDetail.Address2 = EmployeeDataModel.Address2;
                    EmployeeDetail.City = EmployeeDataModel.City;
                    EmployeeDetail.ZipCode = EmployeeDataModel.ZipCode;
                    EmployeeDetail.ContactNumber = EmployeeDataModel.ContactNumber;
                    EmployeeDetail.CountryId = EmployeeDataModel.CountryId;
                    EmployeeDetail.StateId = EmployeeDataModel.StateId;
                    EmployeeDetail.Email = EmployeeDataModel.Email;
                    EmployeeDetail.EditedDate = System.DateTime.UtcNow;

                    ////update record into database entity.
                    unitOfWork.EmployeeRepository.Update<Employee>(EmployeeDetail);

                    ////save changes in database.
                    unitOfWork.EmployeeRepository.Commit();

                    ////case of update.
                    return EmployeeDetail.EmployeeId.ToString();
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
        /// delete employee from Employee table
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public bool DeleteEmployee(Guid EmployeeId)
        {
            try
            {
                ////get employee data by EmployeeId.
                var Employee = GetEmployeeByEmployeeId(EmployeeId);

                ////case of not null return 
                if (Employee != null)
                {
                    ////update record into database entity.
                    unitOfWork.EmployeeRepository.Delete<Employee>(Employee);

                    ////save changes in database.
                    unitOfWork.EmployeeRepository.Commit();

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
        /// method to update the documentId in employee table by EmployeeId
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="DocumentId"></param>
        /// <returns></returns>
        public bool UpdateDocumentId(Guid EmployeeId, Guid DocumentId)
        {
            try
            {
                ////get employee data by EmployeeId.
                var EmployeeDetail = GetEmployeeByEmployeeId(EmployeeId);

                ////case of not null return 
                if (EmployeeDetail != null)
                {
                    ////map entity.
                    EmployeeDetail.DocumentId = DocumentId;
                    EmployeeDetail.EditedDate = System.DateTime.UtcNow;

                    ////update record into database entity.
                    unitOfWork.EmployeeRepository.Update<Employee>(EmployeeDetail);

                    ////save changes in database.
                    unitOfWork.EmployeeRepository.Commit();

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
        /// method to get the employee by email
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public Employee GetEmployeeByEmail(string Email)
        {
            try
            {
                //return single row corresponding to email
                return unitOfWork.EmployeeRepository.FindBy<Employee>(x => x.Email == Email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method is used for updating new password
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="NewPassword"></param>
        /// <returns></returns>
        public bool EditEmployeePassword(Guid EmployeeId, string NewPassword)
        {
            try
            {
                //get existing record.
                var Entity = GetEmployeeByEmployeeId(EmployeeId);

                //check entity is null.
                if (Entity != null)
                {
                    //map entity
                    Entity.Password = NewPassword;
                    Entity.EditedDate = DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.EmployeeRepository.Update<Employee>(Entity);

                    ////save changes in database.
                    unitOfWork.EmployeeRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
