using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;
using TradeSystem.Utils.Models;

namespace TradeSystem.Service
{
    public interface IPromotionService : IService
    {
        List<Promotion> GetAllPromotionByType();
        Promotion GetPromotionByPromotionId(Guid promotionId);
        bool AddPromotion(PromotionDataModel promotionDataModel);
        List<SelectListModel> GetCustomerSelectList();
        List<Customer> GetAllCustomerByActive();
        List<Promotion> GetAllPromotionByCustomerId(string customerId);
        List<Promotion> GetAllPromotionByCustomerIdAndUnView(string customerId);
        bool UpdatePromotionViewed(Guid promotionId, bool viewed);
    }
}
