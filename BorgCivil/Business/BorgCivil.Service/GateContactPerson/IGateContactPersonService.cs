using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IGateContactPersonService : IService
    {
        List<GateContactPerson> GetAllGateContactPerson();

        List<GateContactPerson> GetGateContactPersonList(Guid GateId);

        GateContactPerson GetGateContactByGateContactId(Guid GateContactPersonId);

        GateContactPerson GetGateContactIsDefaultByGateId(Guid GateId);

        bool AddGateContactPerson(GateContactPersonDataModel GateContactPersonDataModel);

        bool UpdateGateContactPerson(GateContactPersonDataModel GateContactPersonDataModel);

        bool UpdateIsDefaultStatus(Guid GateContactPersonId, bool Status);
    }

}
