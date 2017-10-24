using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public class VendorBranchEntity
    {
        public int vendorBranchId { get; set; }
        public string businessName { get; set; }
        public string taxId { get; set; }
        public string location { get; set; }
        public string fullAddress { get; set; }
        public string deliveryAddress { get; set; }
        public string phoneNo { get; set; }
        public string contactPerson { get; set; }
        public string additionalInfo { get; set; }
        public string defaultBranch { get; set; }
        public string email { get; set; }
        public Nullable<int> vendorId { get; set; }
        public string branchCode { get; set; }
    }
}
