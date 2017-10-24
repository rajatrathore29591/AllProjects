using System;
using System.Collections.Generic;
using BorgCivil.Framework.Entities;

namespace BorgCivil.Service
{
    public interface ICountryService : IService
    {
        Country GetCountryById(Guid countryId);

        //List<Document> GetDocuments(Guid userId);
        List<SelectListModel> GetCountryList();
    }
}
