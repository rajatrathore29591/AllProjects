using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.CraveASAP.BusinessEntities;

namespace TV.CraveASAP.BusinessServices.Interfaces
{
    public interface IVendorPromotionServices
    {
        IEnumerable<PromotionCodeEntity> GetActivePromotionByVendorId(int Id);
        IEnumerable<PromotionCodeEntity> GetPromotionByVendorId(int ID);
        IEnumerable<PromotionCodeEntity> GetPromotionByPromotionCodeId(int Id);
       int CreateVendorPromotion(PromotionCodeEntity VendorPromotion);
       bool DeleteVendorPromotion(int promotionCodeId, PromotionCodeEntity PromotionCode);
       bool PinVendorPromotion(int promotionCodeId, PromotionCodeEntity PromotionCode);
       bool UpdateVendorPromotion(int promotionCodeId, PromotionCodeEntity VendorPromotion);
       bool IsVendorHavingActivePromotion(int vendorId, string oAuthkey);
       bool DeActivatePromotion(int promotionCodeId);
    }
}
