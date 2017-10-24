using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace BorgCivil.Service
{
    public class FleetsRegistrationService : IFleetsRegistrationService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public FleetsRegistrationService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region FleetsRegistrationServices

        /// <summary>
        /// this method is used for getting all fleet registration
        /// </summary>
        /// <returns></returns>
        public List<FleetsRegistration> GetAllFleetRegistration()
        {
            try
            {
                //map entity.
                return unitOfWork.FleetsRegistrationRepository.SearchBy<FleetsRegistration>(x => x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting fleet registration detail by FleetTypeId
        /// </summary>
        /// <param name="FleetTypeId"></param>
        /// <returns></returns>
        public List<FleetsRegistration> GetRegisterFleetsByFleetTypeId(Guid FleetTypeId)
        {
            try
            {
                //map entity.
                return unitOfWork.FleetsRegistrationRepository.SearchBy<FleetsRegistration>(x => x.FleetTypeId == FleetTypeId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method for Get Registration Fleet for select list type controls [Text, Value] pair.
        /// </summary>
        /// <returns>List of Select Value from FleetRegistration entity.</returns>
        public List<SelectListModel> GetFleetRegistrationList(Guid FleetTypeId)
        {
            try
            {
                ////get all FleetTypes records
                var FleetTypes = unitOfWork.FleetsRegistrationRepository.SearchBy<FleetsRegistration>(x => x.FleetTypeId == FleetTypeId).ToList();

                ////check types is null or empity
                if (FleetTypes != null)
                {
                    ////map entity to model.
                    return FleetTypes.Select(item => new SelectListModel
                    {
                        Text = item.Registration,
                        Value = item.FleetRegistrationId.ToString()
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
        /// this method is used for fleet registration detail by fleetRegistrationId
        /// </summary>
        /// <param name="FleetRegistrationId"></param>
        /// <returns></returns>
        public FleetsRegistration GetRegisterFleetsByFleetRegistrationId(Guid FleetRegistrationId)
        {
            try
            {
                //map entity.
                return unitOfWork.FleetsRegistrationRepository.FindBy<FleetsRegistration>(x => x.FleetRegistrationId == FleetRegistrationId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add data in fleetRegistration table
        /// </summary>
        /// <param name="FleetRegistrationDataModel"></param>
        /// <returns></returns>
        public string AddFleetRegistration(FleetRegistrationDataModel FleetRegistrationDataModel)
        {
            try
            {
                // for long date
                DateTimeFormatInfo dtinfoLong = new DateTimeFormatInfo();
                dtinfoLong.ShortDatePattern = "dd/MM/yyyy hh:mm tt";
                dtinfoLong.DateSeparator = "/";

                // created new guid
                var Id = Guid.NewGuid();

                //map entity.
                FleetsRegistration entity = new FleetsRegistration()
                {
                    FleetRegistrationId = Id,
                    FleetTypeId = FleetRegistrationDataModel.FleetTypeId,
                    SubcontractorId = FleetRegistrationDataModel.SubcontractorId != "" ? new Guid(FleetRegistrationDataModel.SubcontractorId) : (Guid?)null,
                    Make = FleetRegistrationDataModel.Make,
                    Model = FleetRegistrationDataModel.Model,
                    Capacity = FleetRegistrationDataModel.Capacity,
                    Year = FleetRegistrationDataModel.Year,
                    Registration = FleetRegistrationDataModel.Registration,
                    BorgCivilPlantNumber = FleetRegistrationDataModel.BorgCivilPlantNumber,
                    VINNumber = FleetRegistrationDataModel.VINNumber,
                    EngineNumber = FleetRegistrationDataModel.EngineNumber,
                    InsuranceDate = Convert.ToDateTime(FleetRegistrationDataModel.InsuranceDate, dtinfoLong),
                    CurrentMeterReading = FleetRegistrationDataModel.CurrentMeterReading,
                    LastServiceMeterReading = FleetRegistrationDataModel.LastServiceMeterReading,
                    ServiceInterval = FleetRegistrationDataModel.ServiceInterval,
                    HVISType = FleetRegistrationDataModel.HVISType,
                    AttachmentId = FleetRegistrationDataModel.AttachmentId != "" ? new Guid(FleetRegistrationDataModel.AttachmentId) : (Guid?)null,
                    IsUpdated = false,
                    IsBooked = FleetRegistrationDataModel.IsBooked,
                    UnavailableFromDate = FleetRegistrationDataModel.UnavailableFromDate != null ? Convert.ToDateTime(FleetRegistrationDataModel.UnavailableFromDate) : (DateTime?)null,
                    UnavailableToDate = FleetRegistrationDataModel.UnavailableToDate != null ? Convert.ToDateTime(FleetRegistrationDataModel.UnavailableToDate) : (DateTime?)null,
                    UnavailableNote = FleetRegistrationDataModel.UnavailableNote,
                    IsActive = FleetRegistrationDataModel.IsActive,
                    CreatedBy = FleetRegistrationDataModel.CreatedBy != null ? FleetRegistrationDataModel.CreatedBy : (Guid?)null,
                    EditedBy = FleetRegistrationDataModel.EditedBy != null ? FleetRegistrationDataModel.EditedBy : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow,
                };
            //add record from in database.
            unitOfWork.FleetsRegistrationRepository.Insert<FleetsRegistration>(entity);

            ////save changes in database.
            unitOfWork.FleetsRegistrationRepository.Commit();

            return Id.ToString();
        }
            catch (Exception ex)
            {
                throw ex;
            }
}

/// <summary>
/// method to update the FleetRegistration tables
/// </summary>
/// <param name="FleetRegistrationDataModel"></param>
/// <returns></returns>
public string UpdateFleetRegistration(FleetRegistrationDataModel FleetRegistrationDataModel)
{
    try
    {
        // for long date
        DateTimeFormatInfo dtinfoLong = new DateTimeFormatInfo();
        dtinfoLong.ShortDatePattern = "dd/MM/yyyy hh:mm tt";
        dtinfoLong.DateSeparator = "/";

        ////get FleetRegistration data by FleetRegistrationId.
        var FleetRegistrationDetail = GetRegisterFleetsByFleetRegistrationId(FleetRegistrationDataModel.FleetRegistrationId);

        ////case of not null return 
        if (FleetRegistrationDetail != null)
        {
            ////map entity.
            FleetRegistrationDetail.FleetTypeId = FleetRegistrationDataModel.FleetTypeId;
            FleetRegistrationDetail.SubcontractorId = FleetRegistrationDataModel.SubcontractorId != null ? new Guid(FleetRegistrationDataModel.SubcontractorId) : (Guid?)null;
            FleetRegistrationDetail.Make = FleetRegistrationDataModel.Make;
            FleetRegistrationDetail.Model = FleetRegistrationDataModel.Model;
            FleetRegistrationDetail.Capacity = FleetRegistrationDataModel.Capacity;
            FleetRegistrationDetail.Year = FleetRegistrationDataModel.Year;
            FleetRegistrationDetail.Registration = FleetRegistrationDataModel.Registration;
            FleetRegistrationDetail.BorgCivilPlantNumber = FleetRegistrationDataModel.BorgCivilPlantNumber;
            FleetRegistrationDetail.VINNumber = FleetRegistrationDataModel.VINNumber;
            FleetRegistrationDetail.EngineNumber = FleetRegistrationDataModel.EngineNumber;
            FleetRegistrationDetail.InsuranceDate = Convert.ToDateTime(FleetRegistrationDataModel.InsuranceDate, dtinfoLong);
            FleetRegistrationDetail.CurrentMeterReading = FleetRegistrationDataModel.CurrentMeterReading;
            FleetRegistrationDetail.LastServiceMeterReading = FleetRegistrationDataModel.LastServiceMeterReading;
            FleetRegistrationDetail.ServiceInterval = FleetRegistrationDataModel.ServiceInterval;
            FleetRegistrationDetail.HVISType = FleetRegistrationDataModel.HVISType;
            FleetRegistrationDetail.AttachmentId = FleetRegistrationDataModel.AttachmentId != "" ? new Guid(FleetRegistrationDataModel.AttachmentId) : (Guid?)null;
            FleetRegistrationDetail.IsUpdated = false;
            FleetRegistrationDetail.IsBooked = FleetRegistrationDataModel.IsBooked;
            FleetRegistrationDetail.UnavailableFromDate = FleetRegistrationDataModel.UnavailableFromDate != null ? Convert.ToDateTime(FleetRegistrationDataModel.UnavailableFromDate) : (DateTime?)null;
            FleetRegistrationDetail.UnavailableToDate = FleetRegistrationDataModel.UnavailableToDate != null ? Convert.ToDateTime(FleetRegistrationDataModel.UnavailableToDate) : (DateTime?)null;
            FleetRegistrationDetail.UnavailableNote = FleetRegistrationDataModel.UnavailableNote;
            FleetRegistrationDetail.IsActive = FleetRegistrationDataModel.IsActive;
            FleetRegistrationDetail.CreatedBy = FleetRegistrationDataModel.CreatedBy != null ? FleetRegistrationDataModel.CreatedBy : (Guid?)null;
            FleetRegistrationDetail.EditedBy = FleetRegistrationDataModel.EditedBy != null ? FleetRegistrationDataModel.EditedBy : (Guid?)null;
            FleetRegistrationDetail.CreatedDate = System.DateTime.UtcNow;

            ////update record into database entity.
            unitOfWork.FleetsRegistrationRepository.Update<FleetsRegistration>(FleetRegistrationDetail);

            ////save changes in database.
            unitOfWork.FleetsRegistrationRepository.Commit();

            ////case of update.
            return FleetRegistrationDetail.FleetRegistrationId.ToString();
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
/// delete fleetregistration from fleetregistration table
/// </summary>
/// <param name="FleetRegistrationId"></param>
/// <returns></returns>
public bool DeleteFleetRegistration(Guid FleetRegistrationId)
{
    try
    {
        ////get FleetRegistration data by FleetRegistrationId.
        var FleetRegistration = GetRegisterFleetsByFleetRegistrationId(FleetRegistrationId);

        ////case of not null return 
        if (FleetRegistration != null)
        {
            ////update record into database entity.
            unitOfWork.FleetsRegistrationRepository.Delete<FleetsRegistration>(FleetRegistration);

            ////save changes in database.
            unitOfWork.FleetsRegistrationRepository.Commit();

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
/// method to update the documentId in fleetRegistration table by FleetRegistrationId
/// </summary>
/// <param name="FleetRegistrationId"></param>
/// <param name="DocumentId"></param>
/// <returns></returns>
public bool UpdateDocumentId(Guid FleetRegistrationId, string DocumentId)
{
    try
    {
        // defineing global variable for comma seprated string
        var DocumentIds = string.Empty;

        ////get FleetRegistration data by FleetRegistrationId.
        var FleetRegistrationDetail = GetRegisterFleetsByFleetRegistrationId(FleetRegistrationId);

        ////case of not null return 
        if (FleetRegistrationDetail != null)
        {
            // condition to store comma seprated DocumentIds
            if (FleetRegistrationDetail.DocumentId == null)
            {
                DocumentIds = DocumentId;
            }
            else
            {
                DocumentIds = FleetRegistrationDetail.DocumentId + "," + DocumentId;
            }

            ////map entity.
            FleetRegistrationDetail.DocumentId = DocumentIds;
            FleetRegistrationDetail.EditedDate = System.DateTime.UtcNow;

            ////update record into database entity.
            unitOfWork.FleetsRegistrationRepository.Update<FleetsRegistration>(FleetRegistrationDetail);

            ////save changes in database.
            unitOfWork.FleetsRegistrationRepository.Commit();

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
