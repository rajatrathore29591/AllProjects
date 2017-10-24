using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;

namespace TradeSystem.Service
{
    public interface IBankService : IService
    {
        Bank GetBankByBankId(Guid bankId);
        List<SelectListModel> GetBankList();
    }
}
