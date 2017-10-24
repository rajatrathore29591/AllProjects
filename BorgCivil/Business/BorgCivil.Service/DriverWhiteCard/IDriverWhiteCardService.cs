using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IDriverWhiteCardService : IService
    {
        List<DriverWhiteCard> GetDriverWhiteCardByDriverId(Guid DriverId);

        bool AddDriverWhiteCard(DriverWhiteCardDataModel DriverWhiteCardDataModel);

        bool DeleteDriverWhiteCardByDriverId(Guid DriverId);
    }
}
