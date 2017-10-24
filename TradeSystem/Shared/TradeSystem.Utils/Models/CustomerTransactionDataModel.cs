using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Utils.Models
{
    public partial class CustomerTransactionDataModel
    {
        public string Id { get; set; }

        public string CustomerProductId { get; set; }

        public string PaymentMode { get; set; }
        
    }
}
