using BorgCivil.Framework.Entities;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IStateService : IService
    {
        State GetStateByStateId(Guid stateId);
        //List<Document> GetDocuments(Guid userId);

        List<SelectListModel> GetAllStateByCountryId(Guid countryId);
    }
}
