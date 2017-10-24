using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TradeSystem.Utils.Models
{
    public partial class ProductDataModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public string PercentWeeklyEarning { get; set; }
        public string PercentSaleEarning { get; set; }

        public string WeeklyFromWithdrawDay { get; set; }

        public string DurationWithdraw { get; set; }

        public string WeeklyToWithdrawDay { get; set; }

        public string MonthlyToWithdrawDay { get; set; }

        public string SaleWithdrawDay { get; set; }

        public string InvestmentWithdrawDate { get; set; }

        public string DocumentId { get; set; }

        public string Condition1 { get; set; }

        public string Condition2 { get; set; }

        public string Condition3 { get; set; }

        public float MinPrice { get; set; }

        public float MaxPrice { get; set; }

        public float TotalValueOfInvestment { get; set; }

        public float RemainingValueOfInvestment { get; set; }

        public string ByUserName { get; set; }
        public string ByCountry { get; set; }

        public string ByState { get; set; }

        public string ByFromAmount { get; set; }

        public string ByToAmount { get; set; }

        public string ByFromSale { get; set; }

        public string ByToSale { get; set; }

        public string IsActive { get; set; }

        public string CustomerProductsCount { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ImageBase64 { get; set; }

        public string ImageUrl { get; set; }

        public string CompanyUserId { get; set; }

        public string TableJson { get; set; }

        public bool AllowUserToInvest { get; set; }

        public int TotalDaysOfInvestment { get; set; }

        public bool IsInvest { get; set; }

        public bool LastRemainingInvestmentAmount { get; set; }

        public List<PenaltyDataModel> Penalty { get; set; }

    }


}
