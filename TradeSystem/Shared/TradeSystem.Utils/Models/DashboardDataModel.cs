using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TradeSystem.Utils.Models
{
    public  class DashboardDataModel
    {
        public float TotalValueOfInvestment { get; set; }
        public int ProductCount { get; set; }
        public float ProductAmount { get; set; }
        public int CustomerCount { get; set; }
        public float EarningByInvestment { get; set; }
        public float EarningBySales { get; set; }
        public float VirtualWalletValue { get; set; }
        public int SaleCount { get; set; }

    }
}
