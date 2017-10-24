using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BorgCivil.Service
{
    public class BookingService : IBookingService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public BookingService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region BookingServices

        /// <summary>
        /// getting all booking from booking table 
        /// </summary>
        /// <returns></returns>
        public List<Booking> GetAllBooking()
        {
            try
            {
                return unitOfWork.BookingRepository.SearchBy<Booking>(x => x.IsActive == true && x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// getting all booking from booking table with date range filter
        /// </summary>
        /// <returns></returns>
        public List<Booking> GetAllBookingDateRange(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return unitOfWork.BookingRepository.SearchBy<Booking>(x => x.IsActive == true && x.IsDeleted == false && x.FleetBookingDateTime >= FromDate && x.FleetBookingDateTime <= ToDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting all booking by StatusLookupId
        /// </summary>
        /// <param name="StatusLookupId"></param>
        /// <returns></returns>
        public List<Booking> GetAllBookingByStatusLookupId(Guid StatusLookupId)
        {
            try
            {
                //map entity.
                return unitOfWork.BookingRepository.SearchBy<Booking>(x => x.StatusLookupId == StatusLookupId && x.IsDeleted == false && x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting booking detail by bookingId
        /// </summary>
        /// <param name="BookingId"></param>
        /// <returns></returns>
        public Booking GetBookingByBookingId(Guid BookingId)
        {
            try
            {
                //map entity.
                return unitOfWork.BookingRepository.GetById<Booking>(BookingId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting booking detail by CustomerId
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<Booking> GetBookingByCustomerId(Guid CustomerId)
        {
            try
            {
                //map entity.
                return unitOfWork.BookingRepository.SearchBy<Booking>(x => x.CustomerId == CustomerId && x.IsActive == true && x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add data in booking table
        /// </summary>
        /// <param name="BookingDataModel"></param>
        /// <returns></returns>
        public string AddBooking(BookingDataModel BookingDataModel)
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
                Booking entity = new Booking()
                {
                    BookingId = Id,
                    CustomerId = new Guid(BookingDataModel.CustomerId),
                    SiteId = new Guid(BookingDataModel.SiteId),
                    WorktypeId = new Guid(BookingDataModel.WorktypeId),
                    StatusLookupId = new Guid(BookingDataModel.StatusLookupId),
                    CallingDateTime = Convert.ToDateTime(BookingDataModel.CallingDateTime, dtinfoLong),
                    FleetBookingDateTime = BookingDataModel.FleetBookingDateTime,
                    EndDate = BookingDataModel.EndDate,
                    AllocationNotes = BookingDataModel.AllocationNotes,
                    IsActive = BookingDataModel.IsActive,
                    CreatedBy = BookingDataModel.CreatedBy != null ? new Guid(BookingDataModel.CreatedBy) : (Guid?)null,
                    EditedBy = BookingDataModel.EditedBy != null? new Guid(BookingDataModel.EditedBy) : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow,
                    EditedDate = null,
                    IsDeleted = false
                };
                //add record from in database.
                unitOfWork.BookingRepository.Insert<Booking>(entity);

                ////save changes in database.
                unitOfWork.BookingRepository.Commit();

                return Id.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// updating isdeleted column to true
        /// </summary>
        /// <param name="BookingId"></param>
        /// <returns></returns>
        public bool DeleteBooking(Guid BookingId)
        {
            try
            {
                ////get booking data by bookingId.
                var BookingDetail = GetBookingByBookingId(BookingId);

                ////case of not null return 
                if (BookingDetail != null)
                {
                    ////map entity.
                    BookingDetail.IsDeleted = true;

                    ////update record into database entity.
                    unitOfWork.BookingRepository.Update<Booking>(BookingDetail);

                    ////save changes in database.
                    unitOfWork.BookingRepository.Commit();

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
        /// method to update the status lookupId
        /// </summary>
        /// <param name="BookingId"></param>
        /// <param name="StatusLookUpId"></param>
        /// <param name="CancelNote"></param>
        /// <param name="Rate"></param>
        /// <returns></returns>
        public bool UpdateBookingStatus(Guid BookingId, Guid StatusLookUpId, string CancelNote, string Rate)
        {
            try
            {
                ////get booking data by bookingId.
                var BookingDetail = GetBookingByBookingId(BookingId);

                ////case of not null return 
                if (BookingDetail != null)
                {
                    ////map entity.
                    BookingDetail.StatusLookupId = StatusLookUpId;
                    BookingDetail.CancelNote = CancelNote != "" ? CancelNote : "";
                    BookingDetail.Rate = Rate != "" ? Convert.ToDecimal(Rate) : Convert.ToDecimal("0");
                    BookingDetail.EditedDate = System.DateTime.UtcNow;

                    ////update record into database entity.
                    unitOfWork.BookingRepository.Update<Booking>(BookingDetail);

                    ////save changes in database.
                    unitOfWork.BookingRepository.Commit();

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
        /// this method is used for updating booking site note
        /// </summary>
        /// <param name="BookingId"></param>
        /// <param name="SiteNote"></param>
        /// <returns></returns>
        public bool UpdateSiteNote(Guid BookingId, string SiteNote)
        {
            try
            {
                ////get booking data by bookingId.
                var BookingDetail = GetBookingByBookingId(BookingId);

                ////case of not null return 
                if (BookingDetail != null)
                {
                    ////map entity.
                    BookingDetail.SiteNote = SiteNote;
                    BookingDetail.EditedDate = System.DateTime.UtcNow;

                    ////update record into database entity.
                    unitOfWork.BookingRepository.Update<Booking>(BookingDetail);

                    ////save changes in database.
                    unitOfWork.BookingRepository.Commit();

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
