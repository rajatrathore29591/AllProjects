using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;
using TradeSystem.Utils.Models;

namespace TradeSystem.Service
{
    public interface ITicketService : IService
    {
        List<Ticket> GetAllTicket();
        List<Ticket> GetTicketByCustomerId(Guid customerId);
        bool AddTicket(TicketDataModel ticketDataModel);
        bool EditTicket(Guid ticketId, Guid ticketStatusId);
        Ticket GetTicketDetailByTicketId(Guid ticketId);
        

    }
}
