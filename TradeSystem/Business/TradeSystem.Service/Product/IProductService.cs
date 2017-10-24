using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;
using TradeSystem.Utils.Models;

namespace TradeSystem.Service
{
    public interface IProductService : IService
    {
        List<Product> GetAllProduct();
        Product GetProductByProductId(Guid productId);
        List<Product> GetAllCustomerProduct(string customerId);
        string AddProduct(ProductDataModel productDataModel);
        List<SelectListModel> GetProductSelectList();
        bool UpdateLogo(Guid id, Guid? documentId);
        bool UpdateInvestmentAmount(Guid productId, float InvestmentAmount);

    }
}
