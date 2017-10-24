using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IGatesService : IService
    {
        List<Gate> GetAllGates();

        Gate GetGateByGateId(Guid GateId);

        List<Gate> GetGateListBySiteId(Guid SiteId);

        List<SelectListModel> GetGatesList(Guid SiteId);

        bool AddGate(GateDataModel GateDataModel);

        bool UpdateGate(GateDataModel GateDataModel);
    }
}
