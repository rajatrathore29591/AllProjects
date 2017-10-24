using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeSystem.Framework.Entities
{
    [Table("CustomerProduct")]
    public partial class CustomerProduct
    {

        public CustomerProduct()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public Guid ProductId { get; set; }
        public float Investment { get; set; }

        public float WeeklyEarning { get; set; }

        public string Status { get; set; }

        public string PaymentType { get; set; }

        public string WeeklyEarningStatus { get; set; }

        public string SaleEarningStatus { get; set; }

        public bool StopCalculation { get; set; }
        public DateTime? StopCalculationDate { get; set; }

        public DateTime? StartCalculationDate { get; set; }

        public float SaleEarning { get; set; }

        public bool PaymentStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public DateTime? InvestmentWithdrawDate { get; set; }

        public DateTime? LastWithdrawMonthDate { get; set; }

        public string ChargeId { get; set; }

        public float WalletAmount { get; set; }

        public string BarCodeUrl { get; set; }

        public string BarCode { get; set; }

        public DateTime? LastWeeklyWithdrawDate { get; set; }

        public DateTime? LastWeeklyWithdrawEnableDate { get; set; }

        public bool WithdrawStatus { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Product Product { get; set; }

        public virtual ICollection<CustomerTransaction> CustomerTransactions { get; set; }

        public virtual ICollection<Withdraw> Withdraws { get; set; }
    }
}
