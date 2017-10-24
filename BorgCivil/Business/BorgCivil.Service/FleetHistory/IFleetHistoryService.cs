using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IFleetHistoryService : IService
    {
        List<FleetHistory> GetAllFleetHistoryByBookingFleetId(Guid BookingFleetId);

        string AddFleetHistory(BookingFleetDataModel BookingFleetDataModel);
    }
}
