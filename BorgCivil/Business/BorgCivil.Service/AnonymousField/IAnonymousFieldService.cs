using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IAnonymousFieldService : IService
    {
        List<AnonymousField> GetAnonymousFieldByAnonymousFieldId(Guid DriverId);

        bool AddAnonymousField(AnonymousFieldDataModel AnonymousFieldDataModel);

        bool DeleteAnonymousFieldByDriverId(Guid DriverId);
    }
}
