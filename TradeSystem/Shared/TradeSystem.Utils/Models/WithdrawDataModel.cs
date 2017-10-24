using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Utils.Models
{
    public partial class WithdrawDataModel
    {
        public string Id { get; set; }
        public string CustomerProductId { get; set; }
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public float WithdrawAmount { get; set; }
        public bool IsEarning { get; set; }
        public bool IsSale { get; set; }
        public bool IsVirtualWallet { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime EstimatedDepositDate { get; set; }
        public string CustomerName { get; set; }
        public string WithDrawalFor { get; set; }
        public string RemainingDaysForDepositing { get; set; }
        public string Lang { get; set; }
        public float AvailableBalance { get; set; }
        public string IsWalletOrBank { get; set; }
        public string SessionCustomerId { get; set; }
        public bool WithdrawStatus { get; set; }
        public string ToWithdrawStatus { get; set; } 

    }
    public partial class WeeklyWithdrawHistoryDataModel
    {
        public string CreatedDate { get; set; }
        public string WithdrawAmount { get; set; }
        public string WithDrawalFor { get; set; }
    }

    public partial class WithdrawTableDataModel
    {
        public string CustomerId { get; set; }

        public float AvailableBalance { get; set; }
    }
}
