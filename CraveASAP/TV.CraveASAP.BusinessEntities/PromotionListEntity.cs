using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public partial class PromotionListEntity
    {
        public int promotionCodeId { get; set; }
        public string code { get; set; }
        public string descriptionEnglish { get; set; }
        public string descriptionThai { get; set; }
        public string createdDate { get; set; }
        public string expiryDate { get; set; }
        public int vendorId { get; set; }
        public int categoryId { get; set; }
        public Nullable<int> price { get; set; }
        public string vendorName { get; set; }
        public string categoryName { get; set; }
     
     
       

    }
}
