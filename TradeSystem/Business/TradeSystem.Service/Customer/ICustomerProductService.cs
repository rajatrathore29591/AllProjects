using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;
using TradeSystem.Utils.Models;

namespace TradeSystem.Service
{
    public interface ICustomerProductService : IService
    {
        List<CustomerProduct> GetAllCustomerProductByCustomerId(Guid customerId);
        List<CustomerProduct> GetAllCustomerProduct();
        List<CustomerProduct> GetAllCustomerProductByProductId(Guid customerId);
        CustomerProduct GetAllCustomerProductByProductIdCustomerId(Guid customerId, Guid productId);
        bool InvestInProduct(CustomerProductDataModel customerProductData);
        bool UpdateWeeklyEarning(Guid customerId, Guid productId, float weeklyAmount);
        bool UpdateSaleEarning(Guid customerId, Guid productId, float saleEarning);
        bool UpdateInvestment(Guid customerId, Guid productId, float saleEarning);
        bool StopCalculation(Guid customerId, Guid productId, bool stopCalculation, bool status);
        bool UpdateCustomerProductStatus(Guid customerId, Guid productId, string Status);
        bool UpdateSaleEarningStatus(Guid customerId, Guid productId, string Status,bool withdrawStatus);
        bool UpdateCustomerProductWithBarCode(CustomerProductDataModel customerProductDataDataModel);
        bool UpdateWeeklyEarningEnableDate(Guid customerId, Guid productId, DateTime weeklyDate);
        bool DeleteCustomerProductByCustomerIdProductId(Guid customerId, Guid productId);
        CustomerProduct GetAllCustomerProductByCustomerProductId(Guid customerProductId);
        bool UpdateLastWeeklyEnableDate(Guid customerId, Guid productId, DateTime lastWeeklyEnableDate);
        bool UpdateLastWeeklyDate(Guid customerId, Guid productId, DateTime lastWeeklyDate);
    }
}
