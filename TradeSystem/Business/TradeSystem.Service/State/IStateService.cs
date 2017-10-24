using TradeSystem.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Service
{
    public interface IStateService : IService
    {
        State GetStateByStateId(Guid stateId);

        List<SelectListModel> GetAllStateByCountryId(Guid countryId);
    }
}
