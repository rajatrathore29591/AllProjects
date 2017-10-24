using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public partial class VendorListEntity
    {
        
        public int vendorId { get;  set; } 
        public string loginVendorName { get; set; }
        public string companyName { get; set; }
        public int businessCategory { get; set; }
        public string email { get; set; }
        public string phoneNo { get; set; }
        public string streetName { get; set; }
        public string city { get; set; }
   
       
       
    }
}
