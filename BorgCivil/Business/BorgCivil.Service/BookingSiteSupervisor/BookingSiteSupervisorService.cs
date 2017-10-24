using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BorgCivil.Service
{
    public class BookingSiteSupervisorService : IBookingSiteSupervisorService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public BookingSiteSupervisorService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region BookingSiteSupervisorService

        /// <summary>
        /// this method is used for getting booking site gate by GetBookingSiteSupervisorByBookingSupervisorId
        /// </summary>
        /// <param name="BookingSiteSupervisorId"></param>
        /// <returns></returns>
        public BookingSiteSupervisor GetBookingSiteSupervisorByBookingSupervisorId(Guid BookingSiteSupervisorId)
        {
            try
            {
                //map entity.
                return unitOfWork.BookingSiteSupervisorRepository.GetById<BookingSiteSupervisor>(BookingSiteSupervisorId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting booking site gate by GetBookingSiteSupervisorBySupervisorId
        /// </summary>
        /// <param name="SupervisorId"></param>
        /// <returns></returns>
        public BookingSiteSupervisor GetBookingSiteSupervisorBySupervisorId(Guid SupervisorId)
        {
            try
            {
                //map entity.
                return unitOfWork.BookingSiteSupervisorRepository.FindBy<BookingSiteSupervisor>(x => x.SupervisorId == SupervisorId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting booking site gate by GetBookingSiteSupervisorByBookingId
        /// </summary>
        /// <param name="BookingId"></param>
        /// <returns></returns>
        public List<BookingSiteSupervisor> GetBookingSiteSupervisorByBookingId(Guid BookingId)
        {
            try
            {
                //map entity.
                return unitOfWork.BookingSiteSupervisorRepository.SearchBy<BookingSiteSupervisor>(x => x.BookingId == BookingId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add data in booking site supervisor table
        /// </summary>
        /// <param name="BookingSiteSupervisorDataModel"></param>
        /// <returns></returns>
        public bool AddBookingSiteSupervisor(BookingSiteDetailDataModel BookingSiteDetailDataModel)
        {
            try
            {
                // created new guid
                var Id = Guid.NewGuid();
                //map entity.
                BookingSiteSupervisor entity = new BookingSiteSupervisor()
                {
                    BookingSiteSupervisorId = Id,
                    SupervisorId = new Guid(BookingSiteDetailDataModel.SupervisorId),
                    BookingId = new Guid(BookingSiteDetailDataModel.BookingId),
                    IsActive = BookingSiteDetailDataModel.IsActive,
                    CreatedBy = BookingSiteDetailDataModel.CreatedBy != null ? new Guid(BookingSiteDetailDataModel.CreatedBy) : (Guid?)null,
                    EditedBy = BookingSiteDetailDataModel.EditedBy != null ? new Guid(BookingSiteDetailDataModel.EditedBy) : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow,
                    EditedDate = null
                };
                //add record from in database.
                unitOfWork.BookingSiteSupervisorRepository.Insert<BookingSiteSupervisor>(entity);

                ////save changes in database.
                unitOfWork.BookingSiteSupervisorRepository.Commit();

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
        public bool UpdateBookingSiteSupervisor(BookingSiteDetailDataModel BookingSiteDetailDataModel)
        {
            try
            {
                ////get booking site supervisor data by BookingSiteSupervisorId.
                var BookingSiteSupervisorDetail = GetBookingSiteSupervisorByBookingSupervisorId(new Guid(BookingSiteDetailDataModel.BookingSiteSupervisorId));

                ////case of not null return 
                if (BookingSiteSupervisorDetail != null)
                {
                    ////map entity.
                    BookingSiteSupervisorDetail.SupervisorId = new Guid(BookingSiteDetailDataModel.SupervisorId);
                    BookingSiteSupervisorDetail.EditedDate = System.DateTime.UtcNow;

                    ////update record into database entity.
                    unitOfWork.BookingSiteSupervisorRepository.Update<BookingSiteSupervisor>(BookingSiteSupervisorDetail);

                    ////save changes in database.
                    unitOfWork.BookingSiteSupervisorRepository.Commit();

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
        /// deleteing bookingSiteSupervisor
        /// </summary>
        /// <param name="SupervisorId"></param>
        /// <returns></returns>
        public bool DeleteBookingSiteSupervisor(Guid SupervisorId)
        {
            try
            {
                ////get supervisor data by SupervisorId.
                var BookingDetail = GetBookingSiteSupervisorBySupervisorId(SupervisorId);

                ////case of not null return 
                if (BookingDetail != null)
                {

                    ////update record into database entity.
                    unitOfWork.BookingSiteSupervisorRepository.Delete<BookingSiteSupervisor>(BookingDetail);

                    ////save changes in database.
                    unitOfWork.BookingSiteSupervisorRepository.Commit();

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
        /// deleteing DeleteBookingSiteSupervisorByBookingId
        /// </summary>
        /// <param name="BookingId"></param>
        /// <returns></returns>
        public bool DeleteBookingSiteSupervisorByBookingId(Guid BookingId)
        {
            try
            {
                ////get supervisor data by BookingId.
                var BookingDetail = GetBookingSiteSupervisorByBookingId(BookingId);

                ////case of not null return 
                if (BookingDetail != null)
                {

                    ////update record into database entity.
                    unitOfWork.BookingSiteSupervisorRepository.Delete<BookingSiteSupervisor>(BookingDetail);

                    ////save changes in database.
                    unitOfWork.BookingSiteSupervisorRepository.Commit();

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
