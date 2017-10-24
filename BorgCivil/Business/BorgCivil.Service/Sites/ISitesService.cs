using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface ISitesService : IService
    {
        List<Site> GetAllSites();

        Site GetSitesBySiteId(Guid SiteId);

        string AddSite(SiteDataModel SiteDataModel);

        string UpdateSite(SiteDataModel SiteDataModel);

        bool DeleteSite(Guid SiteId);

        bool UpdateDocumentId(Guid SiteId, Guid DocumentId);

        List<SelectListModel> GetSitesListByCustomerId(Guid CustomerId);

    }
}
