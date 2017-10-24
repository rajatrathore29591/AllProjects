using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Enum;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BorgCivil.Service
{
    public class BookingFleetService : IBookingFleetService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public BookingFleetService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region BookingFleetServices

        /// <summary>
        /// getting all fleet from booking fleet table 
        /// </summary>
        /// <returns></returns>
        public List<BookingFleet> GetAllBookingFleet()
        {
            try
            {
                return unitOfWork.BookingFleetsRepository.SearchBy<BookingFleet>(x => x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting booking fleets by bookingId
        /// </summary>
        /// <param name="BookingId"></param>
        /// <returns></returns>
        public List<BookingFleet> GetBookingFleetsByBookingId(Guid BookingId)
        {
            try
            {
                //map entity.
                return unitOfWork.BookingFleetsRepository.SearchBy<BookingFleet>(x => x.BookingId == BookingId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for BookingFleet details
        /// </summary>
        /// <param name="BookingFleetId"></param>
        /// <returns></returns>
        public BookingFleet GetBookingFleetDetailByBookingFleetId(Guid BookingFleetId)
        {
            try
            {
                //map entity.
                return unitOfWork.BookingFleetsRepository.GetById<BookingFleet>(BookingFleetId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting all bookingFleets by StatusLookupId
        /// </summary>
        /// <param name="StatusLookupId"></param>
        /// <returns></returns>
        public List<BookingFleet> GetAllBookingFleetByStatusLookupId(Guid StatusLookupId)
        {
            try
            {
                //map entity.
                return unitOfWork.BookingFleetsRepository.SearchBy<BookingFleet>(x => x.StatusLookupId == StatusLookupId && x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add data in booking fleet table
        /// </summary>
        /// <param name="BookingDataModel"></param>
        /// <returns></returns>
        public string AddBookingFleet(BookingFleetDataModel BookingFleetDataModel)
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
                BookingFleet entity = new BookingFleet()
                {
                    BookingFleetId = Id,
                    BookingId = new Guid(BookingFleetDataModel.BookingId),
                    FleetTypeId = new Guid(BookingFleetDataModel.FleetTypeId),
                    FleetRegistrationId = BookingFleetDataModel.FleetRegistrationId != "" ? new Guid(BookingFleetDataModel.FleetRegistrationId) : (Guid?)null,
                    DriverId = BookingFleetDataModel.DriverId != "" ? new Guid(BookingFleetDataModel.DriverId) : (Guid?)null,
                    StatusLookupId = new Guid(BookingFleetDataModel.StatusLookupId),
                    IsDayShift = BookingFleetDataModel.IsDayShift,
                    Iswethire = BookingFleetDataModel.Iswethire,
                    AttachmentIds = BookingFleetDataModel.AttachmentIds,
                    NotesForDrive = BookingFleetDataModel.NotesForDrive,
                    IsActive = BookingFleetDataModel.IsActive,
                    CreatedBy = BookingFleetDataModel.CreatedBy != null ? new Guid(BookingFleetDataModel.CreatedBy) : (Guid?)null,
                    EditedBy = BookingFleetDataModel.EditedBy != null ? new Guid(BookingFleetDataModel.EditedBy) : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow,
                    EditedDate = null,
                    FleetBookingDateTime = Convert.ToDateTime(BookingFleetDataModel.FleetBookingDateTime,dtinfoLong),
                    FleetBookingEndDate = BookingFleetDataModel.FleetBookingEndDate,
                    //Reason = BookingFleetDataModel.Reason,
                    //IsfleetCustomerSite = BookingFleetDataModel.IsfleetCustomerSite
                };
                //add record from in database.
                unitOfWork.BookingFleetsRepository.Insert<BookingFleet>(entity);

                ////save changes in database.
                unitOfWork.BookingFleetsRepository.Commit();

                return Id.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for updating BookingFleet table
        /// </summary>
        /// <param name="BookingFleetDataModel"></param>
        /// <returns></returns>
        public bool UpdateBookingFleet(BookingFleetDataModel BookingFleetDataModel)
        {
            try
            {
                // for long date
                DateTimeFormatInfo dtinfoLong = new DateTimeFormatInfo();
                dtinfoLong.ShortDatePattern = "dd/MM/yyyy hh:mm tt";
                dtinfoLong.DateSeparator = "/";

                //get existing record.
                BookingFleet entity = GetBookingFleetDetailByBookingFleetId(new Guid(BookingFleetDataModel.BookingFleetId));

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.BookingId = new Guid(BookingFleetDataModel.BookingId);
                    entity.FleetTypeId = new Guid(BookingFleetDataModel.FleetTypeId);
                    entity.FleetRegistrationId = BookingFleetDataModel.FleetRegistrationId != null ? new Guid(BookingFleetDataModel.FleetRegistrationId) : (Guid?)null;
                    entity.DriverId = BookingFleetDataModel.DriverId != null? new Guid(BookingFleetDataModel.DriverId) : (Guid?)null;
                    entity.StatusLookupId = BookingFleetDataModel.StatusLookupId != ""? new Guid(BookingFleetDataModel.StatusLookupId) : (Guid?)null;
                    entity.IsDayShift = BookingFleetDataModel.IsDayShift;
                    entity.Iswethire = BookingFleetDataModel.Iswethire;
                    entity.AttachmentIds = BookingFleetDataModel.AttachmentIds;
                    entity.NotesForDrive = BookingFleetDataModel.NotesForDrive;
                    entity.IsfleetCustomerSite = BookingFleetDataModel.IsfleetCustomerSite;
                    entity.FleetBookingDateTime = Convert.ToDateTime(BookingFleetDataModel.FleetBookingDateTime,dtinfoLong);
                    entity.FleetBookingEndDate = BookingFleetDataModel.FleetBookingEndDate;
                    entity.EditedDate = System.DateTime.UtcNow;
                    entity.Reason = BookingFleetDataModel.Reason;

                    //update record from existing entity in database.
                    unitOfWork.BookingFleetsRepository.Update<BookingFleet>(entity);

                    ////save changes in database.
                    unitOfWork.BookingFleetsRepository.Commit();

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
        /// delete booked fleet from bookingFleet table
        /// </summary>
        /// <param name="BookingFleetId"></param>
        /// <returns></returns>
        public bool DeleteBookingFleet(Guid BookingFleetId)
        {
            try
            {
                ////get booking fleet data by bookingFleetId.
                var BookingFleet = GetBookingFleetDetailByBookingFleetId(BookingFleetId);

                ////case of not null return 
                if (BookingFleet != null)
                {
                    ////update record into database entity.
                    unitOfWork.BookingFleetsRepository.Delete<BookingFleet>(BookingFleet);

                    ////save changes in database.
                    unitOfWork.BookingFleetsRepository.Commit();

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
        /// method to update the status lookupId of bookingFleet
        /// </summary>
        /// <param name="BookingFleetId"></param>
        /// <param name="StatusLookUpId"></param>
        /// <returns></returns>
        public bool UpdateBookingFleetStatus(Guid BookingFleetId, Guid StatusLookUpId)
        {
            try
            {
                ////get booking fleet data by bookingFleetId.
                var BookingFleetDetail = GetBookingFleetDetailByBookingFleetId(BookingFleetId);

                ////case of not null return 
                if (BookingFleetDetail != null)
                {
                    ////map entity.
                    BookingFleetDetail.StatusLookupId = StatusLookUpId;
                 
                    ////update record into database entity.
                    unitOfWork.BookingFleetsRepository.Update<BookingFleet>(BookingFleetDetail);

                    ////save changes in database.
                    unitOfWork.BookingFleetsRepository.Commit();

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
