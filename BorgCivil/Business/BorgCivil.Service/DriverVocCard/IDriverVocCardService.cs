using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IDriverVocCardService : IService
    {
        List<DriverVocCard> GetDriverVocCardByDriverId(Guid DriverId);

        bool AddDriverVocCard(DriverVocCardDataModel DriverVocCardDataModel);

        bool DeleteDriverVocCardByDriverId(Guid DriverId);
    }
}
