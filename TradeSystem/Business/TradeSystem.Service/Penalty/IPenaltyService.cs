using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;
using TradeSystem.Utils.Models;

namespace TradeSystem.Service
{
    public interface IPenaltyService : IService
    {
        List<Penalty> GetAllPenalty();
        bool AddPenalty(PenaltyDataModel penaltyDataModel);
        List<Penalty> GetAllPenaltyByProductId(string productId);

        Penalty GetPenaltyByCustomerId(Guid customerId);
    }
}
