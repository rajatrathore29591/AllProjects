using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;



namespace TradeSystem.Utils.Models
{
    public partial class PromotionDataModel
    {
        public string Id { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public string SubjectSpanish { get; set; }

        public string Description { get; set; }

        public string DescriptionSpanish { get; set; }

        public string PromotionType { get; set; }

        public string Url { get; set; }

        public string CreatedDate { get; set; }

        public string ModifiedDate { get; set; }

        public string Alert { get; set; }
        public bool Email { get; set; }
        public string Lang { get; set; }
        public bool Viewed { get; set; }

    }
}
