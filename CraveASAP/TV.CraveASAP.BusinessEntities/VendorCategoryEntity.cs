using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public partial class VendorCategoryEntity
    {
        
        public int vendorId { get;  set; } 

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
        public bool isVendorActive { get; set; }
        public int favouriteId { get; set; }
        public bool isFavourite { get; set; }
        public string fromday { get; set; }
        public string today { get; set; }
        public string fromtime { get; set; }
        public string totime { get; set; }  
       
       
    }
}
