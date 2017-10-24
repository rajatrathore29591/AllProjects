//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TV.CraveASAP.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class PromotionCode
    {
        public PromotionCode()
        {
            this.UserPromotionSkips = new HashSet<UserPromotionSkip>();
            this.UserPromotions = new HashSet<UserPromotion>();
        }
    
        public int promotionCodeId { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string descriptionEnglish { get; set; }
        public string descriptionThai { get; set; }
        public Nullable<System.DateTime> promotionCreatedDate { get; set; }
        public Nullable<System.DateTime> createdDate { get; set; }
        public Nullable<System.DateTime> expiryDate { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> modifyDate { get; set; }
        public Nullable<int> isRelaunch { get; set; }
        public Nullable<System.DateTime> lastRelaunchDate { get; set; }
        public int vendorId { get; set; }
        public Nullable<int> quantity { get; set; }
        public int recommendation { get; set; }
        public bool isPinned { get; set; }
        public Nullable<System.DateTime> PinnedDate { get; set; }
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }
        public int optCategoryId { get; set; }
        public string shareFacebookCount { get; set; }
        public string shareInstagramCount { get; set; }
        public string shareTwitterCount { get; set; }
        public Nullable<int> price { get; set; }
        public Nullable<int> useCount { get; set; }
        public Nullable<int> viewCount { get; set; }
        public Nullable<int> recommendationCount { get; set; }
        public string promotionImage { get; set; }
        public Nullable<bool> isDeleted { get; set; }
    
        public virtual ICollection<UserPromotionSkip> UserPromotionSkips { get; set; }
        public virtual ICollection<UserPromotion> UserPromotions { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}