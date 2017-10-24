using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
  public partial  class PromotionsUserEntity
    {
        public int promotionCodeId { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string descriptionEnglish { get; set; }
        public string descriptionThai { get; set; }
        public string createdDate { get; set; }
        public string expiryDate { get; set; }
        public bool isActive { get; set; }
        public int vendorId { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable <int> recommendation { get; set; }
        public Nullable<bool> isPinned { get; set; }
        public Nullable<int> categoryId { get; set; }
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
        public string companyName { get; set; }
        public string fullDescription { get; set; }
        public string shortDescription { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string logoimg { get; set; }
        public string fromday { get; set; }
        public string today { get; set; }
        public string fromtime { get; set; }
        public string totime { get; set; }
        public bool isMore { get; set; }
        public Nullable<bool> isFavourite { get; set; }
        public string favouriteId { get; set; }
        public string serverTime { get; set; }
        public bool isRecommended { get; set; }
        public string tempQuantity { get; set; }
        public bool promoExpired { get; set; }
        public bool isIcrave { get; set; }
        public string expiryDate1 { get; set; }
        public string serverTime1 { get; set; }
      
      
       
      
    }
}
