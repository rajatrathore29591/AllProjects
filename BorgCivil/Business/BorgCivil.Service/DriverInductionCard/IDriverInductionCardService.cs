using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IDriverInductionCardService : IService
    {
        List<DriverInductionCard> GetDriverInductionCardByDriverId(Guid DriverId);

        bool AddDriverInductionCard(DriverInductionCardDataModel DriverInductionCardDataModel);

        bool DeleteDriverInductionCardByDriverId(Guid DriverId);
    }
}
