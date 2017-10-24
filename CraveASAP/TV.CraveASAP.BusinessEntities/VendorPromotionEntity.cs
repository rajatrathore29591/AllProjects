using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public partial class VendorPromotionEntity
    {
        public int promotionCodeId { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string language { get; set; }
        public Nullable<System.DateTime> createdDate { get; set; }
        public Nullable<System.DateTime> expiryDate { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<int> vendorId { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable<int> recommendation { get; set; }
        public Nullable<bool> isPinned { get; set; }
        public Nullable<int> categoryId { get; set; }
        public Nullable<int> subCategoryId { get; set; }
        public Nullable<int> optCategoryId { get; set; }
        public Nullable<int> vendorBranchId { get; set; }
        public string shareFacebookCount { get; set; }
        public string shareInstagramCount { get; set; }
        public string shareTwitterCount { get; set; }
        public Nullable<int> price { get; set; }
        public Nullable<int> useCount { get; set; }
        public Nullable<int> viewCount { get; set; }
        public Nullable<int> recommendationCount { get; set; }
        public string promotionImage { get; set; }

       
    }
}
