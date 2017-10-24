using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BorgCivil.Service
{
    public class BookingSiteGateService : IBookingSiteGateService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public BookingSiteGateService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region BookingSiteGateServices

        /// <summary>
        /// this method is used for getting all booking site gate 
        /// </summary>
        /// <returns></returns>
        public List<BookingSiteGate> GetAllBookingSiteGate()
        {
            try
            {
                //map entity.
                return unitOfWork.BookingSiteGatesRepository.SearchBy<BookingSiteGate>(x => x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting booking site gate by bookingSiteGateId
        /// </summary>
        /// <param name="BookingSiteGateId"></param>
        /// <returns></returns>
        public BookingSiteGate GetBookingSiteGateByBookingSiteGateId(Guid BookingSiteGateId)
        {
            try
            {
                //map entity.
                return unitOfWork.BookingSiteGatesRepository.GetById<BookingSiteGate>(BookingSiteGateId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting booking site gate by GateId
        /// </summary>
        /// <param name="GateId"></param>
        /// <returns></returns>
        public BookingSiteGate GetBookingSiteGateByGateId(Guid GateId)
        {
            try
            {
                //map entity.
                return unitOfWork.BookingSiteGatesRepository.GetById<BookingSiteGate>(GateId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting booking site gate by BookingId
        /// </summary>
        /// <param name="BookingId"></param>
        /// <returns></returns>
        public List<BookingSiteGate> GetBookingSiteGateByBookingId(Guid BookingId)
        {
            try
            {
                //map entity.
                return unitOfWork.BookingSiteGatesRepository.SearchBy<BookingSiteGate>(x => x.BookingId == BookingId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add gate detail in bookingsitegate table
        /// </summary>
        /// <param name="BookingSiteGateDataModel"></param>
        /// <returns></returns>
        public bool AddBookingSiteGate(BookingSiteDetailDataModel BookingSiteDetailDataModel)
        {
            try
            {
                // created new guid
                var Id = Guid.NewGuid();
                //map entity.
                BookingSiteGate entity = new BookingSiteGate()
                {
                    BookingSiteGateId = Id,
                    GateId = new Guid(BookingSiteDetailDataModel.GateId),
                    FleetRegistrationId = new Guid(BookingSiteDetailDataModel.FleetRegistrationId),
                    GateContactPersonId = new Guid(BookingSiteDetailDataModel.GateContactPersonId),
                    BookingId = new Guid(BookingSiteDetailDataModel.BookingId),
                    IsActive = BookingSiteDetailDataModel.IsActive,
                    CreatedBy = BookingSiteDetailDataModel.CreatedBy != null ? new Guid(BookingSiteDetailDataModel.CreatedBy) : (Guid?)null,
                    EditedBy = BookingSiteDetailDataModel.EditedBy != null ? new Guid(BookingSiteDetailDataModel.EditedBy) : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow,
                    EditedDate = null
                };
                //add record from in database.
                unitOfWork.BookingSiteGatesRepository.Insert<BookingSiteGate>(entity);

                ////save changes in database.
                unitOfWork.BookingSiteGatesRepository.Commit();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       /// <summary>
       /// this method is used for updating BookingSiteGate table
       /// </summary>
       /// <param name="BookingSiteDetailDataModel"></param>
       /// <returns></returns>
        public bool UpdateBookingSiteGate(BookingSiteDetailDataModel BookingSiteDetailDataModel)
        {
            try
            {
                ////get booking site gate data by bookingId.
                var BookingSiteGateDetail = GetBookingSiteGateByBookingSiteGateId(new Guid(BookingSiteDetailDataModel.BookingSiteGateId));

                ////case of not null return 
                if (BookingSiteGateDetail != null)
                {
                    ////map entity.
                    BookingSiteGateDetail.GateId = new Guid(BookingSiteDetailDataModel.GateId);
                    BookingSiteGateDetail.FleetRegistrationId = new Guid(BookingSiteDetailDataModel.FleetRegistrationId);
                    BookingSiteGateDetail.EditedDate = System.DateTime.UtcNow;

                    ////update record into database entity.
                    unitOfWork.BookingSiteGatesRepository.Update<BookingSiteGate>(BookingSiteGateDetail);

                    ////save changes in database.
                    unitOfWork.BookingSiteGatesRepository.Commit();

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
        /// deleteing bookingSiteGate
        /// </summary>
        /// <param name="GateId"></param>
        /// <returns></returns>
        public bool DeleteBookingSiteGate(Guid GateId)
        {
            try
            {
                ////get bookingGate data by GateId.
                var BookingGate = GetBookingSiteGateByGateId(GateId);

                ////case of not null return 
                if (BookingGate != null)
                {
                    ////update record into database entity.
                    unitOfWork.BookingSiteGatesRepository.Delete<BookingSiteGate>(BookingGate);

                    ////save changes in database.
                    unitOfWork.BookingSiteGatesRepository.Commit();

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
        /// deleteing bookingSiteGate
        /// </summary>
        /// <param name="BookingId"></param>
        /// <returns></returns>
        public bool DeleteBookingSiteGateByBookingId(Guid BookingId)
        {
            try
            {
                ////get bookingGate data by GateId.
                var BookingGate = GetBookingSiteGateByBookingId(BookingId);

                ////case of not null return 
                if (BookingGate != null)
                {
                    ////update record into database entity.
                    unitOfWork.BookingSiteGatesRepository.Delete<BookingSiteGate>(BookingGate);

                    ////save changes in database.
                    unitOfWork.BookingSiteGatesRepository.Commit();

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
