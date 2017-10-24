using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.CraveASAP.BusinessEntities;

namespace TV.CraveASAP.BusinessServices.Interfaces
{
   public interface IAdminManageActivePromotionsServices
    {
       IEnumerable<VendorEntity> GetAllPromotions();
       bool DeletePromotion(int Id);
       PromotionCodeEntity GetAllPromotionsById(int promotionCodeId);
       IEnumerable<PromotionListEntity> GetPromotion();

    }
}
