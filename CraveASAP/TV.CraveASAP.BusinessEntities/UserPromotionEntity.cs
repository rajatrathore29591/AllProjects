using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
   public partial class UserPromotionEntity
    {
        public int userPromotionId { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<int> promotionId { get; set; }
        public Nullable<bool> isRecommended { get; set; }
        public Nullable<bool> isShareFacebook { get; set; }
        public Nullable<bool> isShareTwitter { get; set; }
        public Nullable<bool> isShareInstagram { get; set; }
        public Nullable<bool> isViewed { get; set; }
        public Nullable<bool> isUsed { get; set; }
        public string userTime { get; set; }
        public virtual UserEntity UserEntity { get; set; }
        public bool like { get; set; }
        public string deviceToken { get; set; }
        public string devicePlatform { get; set; }
    }
}
