using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public partial class PromotionCodeEntity
    {
        public int promotionCodeId { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string descriptionEnglish { get; set; }
        public string descriptionThai { get; set; }
        //public DateTime createdDate { get; set; }
        //public Nullable<DateTime> expiryDate { get; set; }
        public string createdDate { get; set; }
        public string expiryDate { get; set; }
        public bool isActive { get; set; }
        public int vendorId { get; set; }
        public Nullable<int> quantity { get; set; }
        public int recommendation { get; set; }
        public bool isPinned { get; set; }
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
        public string subCategoryname { get; set; }
        public string optionalCategoryName { get; set; }
        public string vendorName { get; set; }
        public string categoryName { get; set; }
        public int userId { get; set; }
        public string day { get; set; }
        public string logoImg { get; set; }
        public string serverTime { get; set; }
        public string deviceToken { get; set; }
        public string devicePlatform { get; set; }
        public string createdDate1 { get; set; }
        public string expiryDate1 { get; set; }
        public string serverTime1 { get; set; }

        //public string createdDateVirtual { get { return Convert.ToString(String.Format("{0:dd/MM/yyyy h:mm:ss tt}", createdDate)); } set { Convert.ToString(createdDate); } }
        //public string expiryDateVirtual { get { return Convert.ToString(String.Format("{0:dd/MM/yyyy h:mm:ss tt}", expiryDate)); } set { Convert.ToString(expiryDate); } }
     
     
       

    }
}
