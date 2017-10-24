using TradeSystem.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Service
{
    public interface ITicketStatusService : IService
    {
        TicketStatus GetTicketStatusByStatus(string ticketStatus);

        List<SelectListModel> GetAllTicketStatusList();
    }
}
