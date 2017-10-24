using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;
using TradeSystem.Utils.Models;

namespace TradeSystem.Service
{
    public interface IWalletService : IService
    {
        Wallet GetWalletByCustomerId(Guid customertId);
        bool AddVirtualAmount(WithdrawDataModel withdrawDataModel);
        bool AddCustomerWalletWithZero(WithdrawTableDataModel WithdrawTableDataModel);
        bool UpdateWalletAmount(Guid customerId, float virtualAmount);
    }
}
