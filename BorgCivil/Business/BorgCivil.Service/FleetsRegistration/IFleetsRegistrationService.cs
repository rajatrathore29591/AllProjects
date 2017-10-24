using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IFleetsRegistrationService : IService
    {
        List<FleetsRegistration> GetAllFleetRegistration();
        List<FleetsRegistration> GetRegisterFleetsByFleetTypeId(Guid FleetTypeId);

        List<SelectListModel> GetFleetRegistrationList(Guid FleetTypeId);

        FleetsRegistration GetRegisterFleetsByFleetRegistrationId(Guid FleetRegistrationId);

        string AddFleetRegistration(FleetRegistrationDataModel FleetRegistrationDataModel);

        string UpdateFleetRegistration(FleetRegistrationDataModel FleetRegistrationDataModel);

        bool DeleteFleetRegistration(Guid FleetRegistrationId);

        bool UpdateDocumentId(Guid FleetRegistrationId, string DocumentId);
    }
}
