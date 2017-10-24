using BorgCivil.Framework.Entities;
using BorgCivil.Service;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IStatusLookupService : IService
    {
        StatusLookup GetStatusByTitle(string Tiltle);

        List<SelectListModel> GetStatusLookupList();
    }
}
