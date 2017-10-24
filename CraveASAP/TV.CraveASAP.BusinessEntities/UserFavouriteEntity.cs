using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public partial class UserFavouriteEntity
    {
        public int favoriteRestaurantId { get; set; }
        public Nullable<int> vendorId { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<bool> isActive { get; set; }
        public string companyName { get; set; }
        public string serverTime { get; set; }
        public bool promoExist { get; set; }
    }
}
