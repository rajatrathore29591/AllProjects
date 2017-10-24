using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Utils.Models
{
    public partial class ActivityLogDataModel
    {
        public string Id { get; set; }
        public string CompanyUserId { get; set; }
        public string Activity { get; set; }
        public string Description { get; set; }
        public bool IsCompanyUser { get; set; }
        public string CreatedDate { get; set; }
    }

    public partial class CompanyActivityListDataModel
    {
        public CompanyUserDataModel CompanyUser { get; set; }
        public List<ActivityLogDataModel> Activity { get; set; }
    }
}
