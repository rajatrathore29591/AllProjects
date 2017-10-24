using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSystem.Utils.Models
{
    public class WeeklyPercentDataModel
    {
        public DateTime date { get; set; }  
        public string day { get; set; }
        public float Earning { get; set; }
    }

    public class WeeklyPercentListDataModel
    {
        public List<WeeklyPercentDataModel> Penalty { get; set; }
    }
}