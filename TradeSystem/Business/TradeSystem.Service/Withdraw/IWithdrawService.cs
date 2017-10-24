using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;
using TradeSystem.Utils.Models;

namespace TradeSystem.Service
{
    public interface IWithdrawService : IService
    {
        List<Withdraw> GetAllFinanceList();
        Withdraw GetFinanceByFinanceId(Guid FinanceId);
        List<Withdraw> GetTotalWithdrawByCustomerProductId(Guid CustomerProductId);
        bool EditFinance(Guid id, string Status);
        string AddWithdraw(WithdrawDataModel withdrawDataModel);
        List<Withdraw> GetAllFinanceInvestmentHistory(Guid customerProductId);
        List<Withdraw> GetAllWithdrawSaleByCustomerId(Guid customerId);
        List<Withdraw> GetAllWithdrawByCustomerId(Guid customerId);
    }
}
