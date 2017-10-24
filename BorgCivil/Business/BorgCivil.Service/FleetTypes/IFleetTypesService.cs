using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IFleetTypesService : IService
    {
        List<FleetType> GetAllFleetTypes();

        FleetType GetFleetTypesByFleetTypeId(Guid FleetTypeId);

        List<SelectListModel> GetFleetTypesList();

        string AddFleetType(FleetTypeDataModel FleetTypeDataModel);

        string UpdateFleetType(FleetTypeDataModel FleetTypeDataModel);

        bool DeleteFleetType(Guid FleetTypeId);

        bool UpdateDocumentId(Guid FleetTypeId, Guid DocumentId);
    }
}
