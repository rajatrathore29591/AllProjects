using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IBookingFleetService : IService
    {
        List<BookingFleet> GetAllBookingFleet();
        List<BookingFleet> GetBookingFleetsByBookingId(Guid BookingId);
        BookingFleet GetBookingFleetDetailByBookingFleetId(Guid BookingFleetId);
        List<BookingFleet> GetAllBookingFleetByStatusLookupId(Guid StatusLookupId);
        string AddBookingFleet(BookingFleetDataModel BookingFleetDataModel);
        bool UpdateBookingFleet(BookingFleetDataModel BookingFleetDataModel);
        bool DeleteBookingFleet(Guid BookingFleetId);
        bool UpdateBookingFleetStatus(Guid BookingFleetId, Guid StatusLookUpId);
    }
}
