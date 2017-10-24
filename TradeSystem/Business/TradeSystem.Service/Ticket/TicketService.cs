using System;
using System.Collections.Generic;
using System.Linq;
using TradeSystem.Framework.Entities;
using TradeSystem.Repositories;
using TradeSystem.Utils.Models;

namespace TradeSystem.Service
{
    public class TicketService : BaseService, ITicketService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public TicketService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        /// This method for Get All Ticket.
        /// </summary>
        /// <returns>Tasks</returns>
        public List<Ticket> GetAllTicket()
        {
            try
            {
                return unitOfWork.TicketRepository.GetAll<Ticket>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get task by id.
        /// </summary>
        /// <param name="id">Task Id</param>
        /// <returns>Object Task entity</returns>
        public List<Ticket> GetTicketByCustomerId(Guid customerId)
        {
            try
            {
                ////return object entity
                return unitOfWork.TicketRepository.SearchBy<Ticket>(x => x.CustomerId == customerId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method use for Insert record into companyUser entity.
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <param name="ticketStatusId">ticketStatusId</param>
        /// <param name="title">title</param>
        /// <param name="description">description</param>
        /// <returns>result</returns>
        public bool AddTicket(TicketDataModel ticketDataModel)
        {
            try
            {
                var response = string.Empty;
                //map entity.
                Ticket entity = new Ticket()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = new Guid(ticketDataModel.CustomerId),
                    TicketStatusId = new Guid(ticketDataModel.TicketStatusId),
                    Title = ticketDataModel.Title,
                    Description = ticketDataModel.Description,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };
                //add record from in database.
                unitOfWork.TicketRepository.Insert<Ticket>(entity);
                ////save changes in database.
                unitOfWork.TicketRepository.Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method use for update existing record form database.
        /// ticket entity.
        /// </summary>
        /// <param name="ticketId">ticketId</param>
        /// <param name="ticketStatusId">ticketStatusId</param>
        /// <returns>result</returns>
        public bool EditTicket(Guid ticketId, Guid ticketStatusId)
        {
            try
            {
                //get existing record.
                Ticket entity = unitOfWork.TicketRepository.FindBy<Ticket>(ticketId);

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.TicketStatusId = ticketStatusId;
                    //update record from existing entity in database.
                    unitOfWork.TicketRepository.Update<Ticket>(entity);
                    ////save changes in database.
                    unitOfWork.TicketRepository.Commit();

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
        /// This method use for get roles by roleId
        /// </summary>
        /// <returns>role detail</returns>
        public Ticket GetTicketDetailByTicketId(Guid ticketId)
        {
            try
            {
                //return entity object as per result.
                //return unitOfWork.TicketRepository.GetById<Ticket>(ticketId); 
                return unitOfWork.TicketRepository.FindBy<Ticket>(x => x.Id == ticketId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


      

    }
}
