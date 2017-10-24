using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;
using TradeSystem.Repositories;

namespace TradeSystem.Service
{
    public class TicketStatusService : BaseService, ITicketStatusService
    {

        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public TicketStatusService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        /// <summary>
        /// This method use for get ticketstatusid by ticketstatus
        /// </summary>
        /// <returns>role detail</returns>
        public TicketStatus GetTicketStatusByStatus(string ticketStatus)
        {
            try
            {
                //return entity object as per result.
                return unitOfWork.TicketStatusRepository.FindBy<TicketStatus>(x => x.Name == ticketStatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method for Get Ticket Status for select list type controls [Text, Value] pair.
        /// </summary>
        /// <returns>List of Select Value from Ticket Status entity.</returns>
        public List<SelectListModel> GetAllTicketStatusList()
        {
            try
            {
                ////get all records
                var types = unitOfWork.TicketStatusRepository.GetAll<TicketStatus>().ToList();

                ////check types is null or empity
                if (types != null)
                {
                    ////map entity to model.
                    return types.Select(item => new SelectListModel
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
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
    }
}
