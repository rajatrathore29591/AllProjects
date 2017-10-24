using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BorgCivil.Service
{
    public class FleetHistoryService : IFleetHistoryService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public FleetHistoryService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region FleetHistoryServices

        /// <summary>
        /// getting all History from FleetHistory table
        /// </summary>
        /// <param name="BookingFleetId"></param>
        /// <returns></returns>
        public List<FleetHistory> GetAllFleetHistoryByBookingFleetId(Guid BookingFleetId)
        {
            try
            {
                return unitOfWork.FleetHistoryRepository.SearchBy<FleetHistory>(x => x.IsActive == true && x.BookingFleetId == BookingFleetId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add data in fleetHistory table
        /// </summary>
        /// <param name="BookingFleetDataModel"></param>
        /// <returns></returns>
        public string AddFleetHistory(BookingFleetDataModel BookingFleetDataModel)
        {
            try
            {
                // created new guid
                var Id = Guid.NewGuid();
                //map entity.
                FleetHistory entity = new FleetHistory()
                {
                    FleetHistoryId = Id,
                    BookingId = new Guid(BookingFleetDataModel.BookingId),
                    BookingFleetId = new Guid(BookingFleetDataModel.BookingFleetId),
                    FleetTypeId = new Guid(BookingFleetDataModel.FleetTypeId),
                    FleetRegistrationId = BookingFleetDataModel.FleetRegistrationId != "" ? new Guid(BookingFleetDataModel.FleetRegistrationId) : (Guid?)null,
                    DriverId = BookingFleetDataModel.DriverId != "" ? new Guid(BookingFleetDataModel.DriverId) : (Guid?)null,
                    FleetStatus = new Guid(BookingFleetDataModel.StatusLookupId),
                    IsDayShift = BookingFleetDataModel.IsDayShift,
                    Iswethire = BookingFleetDataModel.Iswethire,
                    AttachmentIds = BookingFleetDataModel.AttachmentIds,
                    NotesForDrive = BookingFleetDataModel.NotesForDrive,
                    Reason = BookingFleetDataModel.Reason,
                    IsfleetCustomerSite = BookingFleetDataModel.IsfleetCustomerSite,
                    IsActive = BookingFleetDataModel.IsActive,
                    CreatedBy = BookingFleetDataModel.CreatedBy != null ? new Guid(BookingFleetDataModel.CreatedBy) : (Guid?)null,
                    EditedBy = BookingFleetDataModel.EditedBy != null ? new Guid(BookingFleetDataModel.EditedBy) : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow,
                    EditedDate = null,
                };
                //add record from in database.
                unitOfWork.FleetHistoryRepository.Insert<FleetHistory>(entity);

                ////save changes in database.
                unitOfWork.FleetHistoryRepository.Commit();

                return Id.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
