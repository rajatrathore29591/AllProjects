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
    
    public partial class Vendor
    {
        public Vendor()
        {
            this.OpeningHours = new HashSet<OpeningHour>();
            this.PromotionCodes = new HashSet<PromotionCode>();
            this.UserFavourites = new HashSet<UserFavourite>();
            this.VendorBranches = new HashSet<VendorBranch>();
        }
    
        public int vendorId { get; set; }
        public string loginVendorName { get; set; }
        public string companyName { get; set; }
        public int businessCategory { get; set; }
        public string shortDescription { get; set; }
        public string fullDescription { get; set; }
        public string email { get; set; }
        public string phoneNo { get; set; }
        public string contactPerson { get; set; }
        public string contactPhoneNo { get; set; }
        public string contactEmail { get; set; }
        public string streetName { get; set; }
        public string postCode { get; set; }
        public string buildingName { get; set; }
        public string floor { get; set; }
        public string area { get; set; }
        public string city { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string password { get; set; }
        public int taxId { get; set; }
        public string logoImg { get; set; }
        public string deviceToken { get; set; }
        public string devicePlatform { get; set; }
        public Nullable<bool> isDeleted { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual ICollection<OpeningHour> OpeningHours { get; set; }
        public virtual ICollection<PromotionCode> PromotionCodes { get; set; }
        public virtual ICollection<UserFavourite> UserFavourites { get; set; }
        public virtual ICollection<VendorBranch> VendorBranches { get; set; }
    }
}
