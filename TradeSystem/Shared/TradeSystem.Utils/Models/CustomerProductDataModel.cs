using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Utils.Models
{
    public partial class CustomerProductDataModel
    {

        public string Id { get; set; }

        public string CustomerId { get; set; }

        public string ProductId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime InvestmentWithdrawDate { get; set; }

        public string Investment { get; set; }

        public float MiniMarketAmount { get; set; }

        // create replica for getting error when addOn mexico
        public float InvestmentFloat { get; set; }

        // created after challenges in translation
        public float InvestmentCustomer { get; set; }

        public string InvestmentName { get; set; }

        public string CustomerName { get; set; }

        public string WeeklyEarning { get; set; }

        // create replica for getting error when addOn mexico
        public float WeeklyEarningFloat { get; set; }

        public string Status { get; set; }

        public string PaymentType { get; set; }

        public bool StopCalculation { get; set; }

        public string SaleEarning { get; set; }

        // create replica for getting error when addOn mexico
        public float SaleEarningFloat { get; set; }

        // created after challenges in translation
        public float SaleEarningCustomer { get; set; }

        public bool PaymentStatus { get; set; }

        public string BarCodeUrl { get; set; }

        public string BarCode { get; set; }

        public string BarCodeImage { get; set; }

        public string Description { get; set; }

        public string Email { get; set; }

        public string lang { get; set; }

        public string Name { get; set; }

        public Nullable<float> CurrentWeekTotalEarning { get; set; }

        public List<WeeklyPercentDataModel> CurrentWeekEarning { get; set; }

        public string IsActive { get; set; }

        public string PreviousMonthEarning { get; set; }

        public float PreviousMonthEarningFloat { get; set; }

        public DateTime DatetoWithdrawEarning { get; set; }

        public string ImageUrl { get; set; }

        public string TotalWeeklyEarning { get; set; }

        public string WalletAmount { get; set; }

        public DateTime LastWeeklyWithdrawDate { get; set; }

        public DateTime LastWeeklyWithdrawEnableDate { get; set; }

        public bool WithdrawStatus { get; set; }

        public DateTime SaleWithdrawDate { get; set; }

        public DateTime CurrentTodayDate { get; set; }

    }
}
