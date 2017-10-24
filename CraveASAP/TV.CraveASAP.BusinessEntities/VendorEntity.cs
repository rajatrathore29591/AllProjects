using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public partial class VendorEntity
    {
        public VendorEntity()
        {

            this.PromotionCodes = new HashSet<PromotionCodeEntity>();
            this.OpeningHours = new HashSet<OpeningHourEntity>();

        }
        public int _vendorId = 0;
        public int vendorId { get { return _vendorId; } set { _vendorId = value; } }

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
        public string deviceType { get; set; }
        public bool isVendorActive { get; set; }

        public string openingHoursId { get; set; }
        public string fromday { get; set; }
        public string today { get; set; }
        public string fromtime { get; set; }
        public string totime { get; set; }

        public double Distance { get; set; }
        public string oldPassword { get; set; }
        public string oAuthkey { get; set; }

        public virtual ICollection<PromotionCodeEntity> PromotionCodes { get; set; }
        public virtual ICollection<OpeningHourEntity> OpeningHours { get; set; }

    }
}
