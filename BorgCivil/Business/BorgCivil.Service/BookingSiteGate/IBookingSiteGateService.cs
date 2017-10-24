using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IBookingSiteGateService : IService
    {
        List<BookingSiteGate> GetAllBookingSiteGate();
        BookingSiteGate GetBookingSiteGateByBookingSiteGateId(Guid BookingSiteGateId);
        BookingSiteGate GetBookingSiteGateByGateId(Guid GateId);
        List<BookingSiteGate> GetBookingSiteGateByBookingId(Guid BookingId);
        bool AddBookingSiteGate(BookingSiteDetailDataModel BookingSiteDetailDataModel);
        bool UpdateBookingSiteGate(BookingSiteDetailDataModel BookingSiteDetailDataModel);
        bool DeleteBookingSiteGate(Guid GateId);
        bool DeleteBookingSiteGateByBookingId(Guid BookingId);
    }
}
