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
    
    public partial class Category
    {
        public Category()
        {
            this.AppDefaultLandingPages = new HashSet<AppDefaultLandingPage>();
            this.UserPrefrences = new HashSet<UserPrefrence>();
            this.SubCategories = new HashSet<SubCategory>();
            this.OptionalCategories = new HashSet<OptionalCategory>();
            this.Vendors = new HashSet<Vendor>();
        }
    
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public string description { get; set; }
        public string language { get; set; }
    
        public virtual ICollection<AppDefaultLandingPage> AppDefaultLandingPages { get; set; }
        public virtual ICollection<UserPrefrence> UserPrefrences { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }
        public virtual ICollection<OptionalCategory> OptionalCategories { get; set; }
        public virtual ICollection<Vendor> Vendors { get; set; }
    }
}
