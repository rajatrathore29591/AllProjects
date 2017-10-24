using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BorgCivil.Service
{
    public class DriversService : IDriversService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public DriversService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region DriversService

        /// <summary>
        /// getting all driver from driver table
        /// </summary>
        /// <returns></returns>
        public List<Driver> GetAllDriver()
        {
            try
            {
                return unitOfWork.DriversRepository.SearchBy<Driver>(x => x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting driver detail by driverId
        /// </summary>
        /// <param name="DriverId"></param>
        /// <returns></returns>
        public Driver GetDriverByDriverId(Guid DriverId)
        {
            try
            {
                //map entity.
                return unitOfWork.DriversRepository.GetById<Driver>(DriverId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method for Get Driver for select list type controls [Text, Value] pair.
        /// </summary>
        /// <returns>List of Select Value from Drivers entity.</returns>
        public List<SelectListModel> GetDriversList(Guid FleetRegistrationId)
        {
            try
            {
                ////get all Drivers records
                var Drivers = unitOfWork.DriversRepository.SearchBy<Driver>(x => x.FleetRegistrationId == FleetRegistrationId).ToList();

                ////check drivers is null or empity
                if (Drivers != null)
                {
                    ////map entity to model.
                    return Drivers.Select(item => new SelectListModel
                    {
                        Text = item.FirstName + ' ' + item.LastName,
                        Value = item.DriverId.ToString()
                    }
                   ).ToList();
                }
                return new List<SelectListModel>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add data in driver table
        /// </summary>
        /// <param name="DriverDataModel"></param>
        /// <returns></returns>
        public string AddDriver(DriverDataModel DriverDataModel)
        {
            try
            {
                // created new guid
                var Id = Guid.NewGuid();

                //map entity.
                Driver entity = new Driver()
                {
                    DriverId = Id,
                    FleetRegistrationId = DriverDataModel.FleetRegistrationId != "" ? new Guid(DriverDataModel.FleetRegistrationId) : (Guid?)null,
                    CountryId = new Guid(DriverDataModel.CountryId),
                    StateId = DriverDataModel.StateId != "" ? new Guid(DriverDataModel.StateId) : (Guid?)null,
                    EmploymentCategoryId = DriverDataModel.EmploymentCategoryId != "" ? new Guid(DriverDataModel.EmploymentCategoryId) : (Guid?)null,
                    LicenseClassId = DriverDataModel.LicenseClassId != "" ? new Guid(DriverDataModel.LicenseClassId) : (Guid?)null,
                    FirstName = DriverDataModel.FirstName,
                    LastName = DriverDataModel.LastName,
                    Email = DriverDataModel.Email,
                    MobileNumber = DriverDataModel.MobileNumber,
                    AddressLine1 = DriverDataModel.AddressLine1,
                    AddressLine2 = DriverDataModel.AddressLine2,
                    Suburb = DriverDataModel.Suburb,
                    PostCode = DriverDataModel.PostCode,
                    CardNumber = DriverDataModel.CardNumber,
                    LicenseNumber = DriverDataModel.LicenseNumber,
                    ExpiryDate = DriverDataModel.ExpiryDate != null ? Convert.ToDateTime(DriverDataModel.ExpiryDate) : (DateTime?)null,
                    BaseRate = DriverDataModel.BaseRate,
                    Shift = DriverDataModel.Shift,
                    TypeFromDate = DriverDataModel.TypeFromDate != null ? Convert.ToDateTime(DriverDataModel.TypeFromDate) : (DateTime?)null,
                    TypeToDate = DriverDataModel.TypeToDate != null ? Convert.ToDateTime(DriverDataModel.TypeToDate) : (DateTime?)null,
                    TypeNote = DriverDataModel.TypeNote,
                    LeaveFromDate = DriverDataModel.LeaveFromDate != null ? Convert.ToDateTime(DriverDataModel.LeaveFromDate) : (DateTime?)null,
                    LeaveToDate = DriverDataModel.LeaveToDate != null ? Convert.ToDateTime(DriverDataModel.LeaveToDate) : (DateTime?)null,
                    LeaveNote = DriverDataModel.LeaveNote,
                    City = DriverDataModel.City,
                    AnnualLeaveBalance = DriverDataModel.AnnualLeaveBalance,
                    SickLeaveBalance = DriverDataModel.SickLeaveBalance,
                    EmergencyContactName = DriverDataModel.EmergencyContactName,
                    EmergencyContactNumber = DriverDataModel.EmergencyContactNumber,
                    IsActive = DriverDataModel.IsActive,
                    CreatedBy = DriverDataModel.CreatedBy != null ? DriverDataModel.CreatedBy : (Guid?)null,
                    EditedBy = DriverDataModel.EditedBy != null ? DriverDataModel.EditedBy : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow,
                    StatusLookupId = new Guid(DriverDataModel.StatusLookupId),
                };
                //add record from in database.
                unitOfWork.DriversRepository.Insert<Driver>(entity);

                ////save changes in database.
                unitOfWork.DriversRepository.Commit();

                return Id.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add data in driver table
        /// </summary>
        /// <param name="DriverDataModel"></param>
        /// <returns></returns>
        public string UpdateDriver(DriverDataModel DriverDataModel)
        {
            try
            {
                // created new guid
                var DriverDetail = GetDriverByDriverId(DriverDataModel.DriverId);

                //map entity.
                if (DriverDetail != null)
                {
                    DriverDetail.FleetRegistrationId = DriverDataModel.FleetRegistrationId != "" ? new Guid(DriverDataModel.FleetRegistrationId) : (Guid?)null;
                    DriverDetail.CountryId = new Guid(DriverDataModel.CountryId);
                    DriverDetail.StateId = new Guid(DriverDataModel.StateId);
                    DriverDetail.EmploymentCategoryId = new Guid(DriverDataModel.EmploymentCategoryId);
                    DriverDetail.LicenseClassId = new Guid(DriverDataModel.LicenseClassId);
                    DriverDetail.FirstName = DriverDataModel.FirstName;
                    DriverDetail.LastName = DriverDataModel.LastName;
                    DriverDetail.Email = DriverDataModel.Email;
                    DriverDetail.MobileNumber = DriverDataModel.MobileNumber;
                    DriverDetail.AddressLine1 = DriverDataModel.AddressLine1;
                    DriverDetail.AddressLine2 = DriverDataModel.AddressLine2;
                    DriverDetail.Suburb = DriverDataModel.Suburb;
                    DriverDetail.PostCode = DriverDataModel.PostCode;
                    DriverDetail.CardNumber = DriverDataModel.CardNumber;
                    DriverDetail.LicenseNumber = DriverDataModel.LicenseNumber;
                    DriverDetail.ExpiryDate = DriverDataModel.ExpiryDate != null ? Convert.ToDateTime(DriverDataModel.ExpiryDate) : (DateTime?)null;
                    DriverDetail.BaseRate = DriverDataModel.BaseRate;
                    DriverDetail.Shift = DriverDataModel.Shift;
                    DriverDetail.TypeFromDate = DriverDataModel.TypeFromDate != null ? Convert.ToDateTime(DriverDataModel.TypeFromDate) : (DateTime?)null;
                    DriverDetail.TypeToDate = DriverDataModel.TypeToDate != null ? Convert.ToDateTime(DriverDataModel.TypeToDate) : (DateTime?)null;
                    DriverDetail.TypeNote = DriverDataModel.TypeNote;
                    DriverDetail.LeaveFromDate = DriverDataModel.LeaveFromDate != null ? Convert.ToDateTime(DriverDataModel.LeaveFromDate) : (DateTime?)null;
                    DriverDetail.LeaveToDate = DriverDataModel.LeaveToDate != null ? Convert.ToDateTime(DriverDataModel.LeaveToDate) : (DateTime?)null;
                    DriverDetail.LeaveNote = DriverDataModel.LeaveNote;
                    DriverDetail.City = DriverDataModel.City;
                    DriverDetail.AnnualLeaveBalance = DriverDataModel.AnnualLeaveBalance;
                    DriverDetail.SickLeaveBalance = DriverDataModel.SickLeaveBalance;
                    DriverDetail.EmergencyContactNumber = DriverDataModel.EmergencyContactNumber;
                    DriverDetail.EmergencyContactName = DriverDataModel.EmergencyContactName;
                    DriverDetail.IsActive = DriverDataModel.IsActive;
                    DriverDetail.CreatedBy = DriverDataModel.CreatedBy != null ? DriverDataModel.CreatedBy : (Guid?)null;
                    DriverDetail.EditedBy = DriverDataModel.EditedBy != null ? DriverDataModel.EditedBy : (Guid?)null;
                    DriverDetail.EditedDate = System.DateTime.UtcNow;
                    DriverDetail.StatusLookupId = new Guid(DriverDataModel.StatusLookupId);
                };
                //add record from in database.
                unitOfWork.DriversRepository.Update<Driver>(DriverDetail);

                ////save changes in database.
                unitOfWork.DriversRepository.Commit();

                return DriverDetail.DriverId.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to update the documentId in driver table by driverId
        /// </summary>
        /// <param name="DriverId"></param>
        /// <param name="DocumentId"></param>
        /// <returns></returns>
        public bool UpdateDocumentId(Guid DriverId, Guid DocumentId)
        {
            try
            {
                ////get driver data by DriverId.
                var DriverDetail = GetDriverByDriverId(DriverId);

                ////case of not null return 
                if (DriverDetail != null)
                {
                    ////map entity.
                    DriverDetail.ProfilePic = DocumentId;
                    DriverDetail.EditedDate = System.DateTime.UtcNow;

                    ////update record into database entity.
                    unitOfWork.DriversRepository.Update<Driver>(DriverDetail);

                    ////save changes in database.
                    unitOfWork.DriversRepository.Commit();

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
        /// method to update the DocumentId in Driver table by DriverId
        /// </summary>
        /// <param name="DriverId"></param>
        /// <param name="DocumentId"></param>
        /// <returns></returns>
        public bool UpdateDocumentLicenseId(Guid DriverId, string DocumentId)
        {
            try
            {
                // defineing global variable for comma seprated string
                var DocumentIds = string.Empty;

                ////get Driver data by DriverId.
                var DriverDetail = GetDriverByDriverId(DriverId);

                ////case of not null return 
                if (DriverDetail != null)
                {
                    // condition to store comma seprated DocumentIds
                    if (DriverDetail.DocumentId == null)
                    {
                        DocumentIds = DocumentId;
                    }
                    else
                    {
                        DocumentIds = DriverDetail.DocumentId + "," + DocumentId;
                    }

                    ////map entity.
                    DriverDetail.DocumentId = DocumentIds;
                    DriverDetail.EditedDate = System.DateTime.UtcNow;

                    ////update record into database entity.
                    unitOfWork.DriversRepository.Update<Driver>(DriverDetail);

                    ////save changes in database.
                    unitOfWork.DriversRepository.Commit();

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
        /// delete driver from driver table
        /// </summary>
        /// <param name="DriverId"></param>
        /// <returns></returns>
        public bool DeleteDriver(Guid DriverId)
        {
            try
            {
                ////get driver data by DriverId.
                var Driver = GetDriverByDriverId(DriverId);

                ////case of not null return 
                if (Driver != null)
                {
                    ////update record into database entity.
                    unitOfWork.DriversRepository.Delete<Driver>(Driver);

                    ////save changes in database.
                    unitOfWork.DriversRepository.Commit();

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

        #endregion
    }
}
