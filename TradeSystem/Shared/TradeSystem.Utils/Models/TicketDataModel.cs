using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Utils.Models
{
    public partial class TicketDataModel
    {
        public string Id { get; set; }

        public string CustomerId { get; set; }

        public string TicketStatusId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CustomerName { get; set; }

        public string Status { get; set; }

        public bool Process { get; set; }
        public bool Completed { get; set; }

        public int AutoIncrementedNo { get; set; }

        public string CompanyUserId { get; set; }
        public  string Lang { get; set; }
    }
}
