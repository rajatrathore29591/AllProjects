using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Utils.Models
{
    public partial class CustomerReportDataModel
    {
        public string Id { get; set; }
       
        public string ReportType { get; set; }

        public string CountryId { get; set; }

        public string StateId { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string ProductId { get; set; }
        public ICollection<CustomerReportDataModelList> CustomerReportDataModelList { get; set; }
       


    }

    public class CustomerReportDataModelList
    {
        public DateTime CreatedDate { get; set; }

        public string TotalValueOfInvestment { get; set; }

        public float TotalValueOfInvestmentFloat { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }

        public string Name { get; set; }

        public string ReportType { get; set; }

        public string ProductName { get; set; }

        public string MoneyWithdrawal { get; set; }

        public float MoneyWithdrawalFloat { get; set; }

        public string TicketId { get; set; }

        public string Status { get; set; }

        public string Title { get; set; }

        public DateTime CustomerWithdrawDate { get; set; }

        public string ReferalsCount { get; set; }
    }
}
