using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IBookingService : IService
    {
        List<Booking> GetAllBooking();

        List<Booking> GetAllBookingDateRange(DateTime FromDate, DateTime ToDate);

        List<Booking> GetAllBookingByStatusLookupId(Guid StatusLookupId);

        Booking GetBookingByBookingId(Guid BookingId);

        List<Booking> GetBookingByCustomerId(Guid CustomerId);

        string AddBooking(BookingDataModel BookingDataModel);

        bool DeleteBooking(Guid BookingId);

        bool UpdateBookingStatus(Guid BookingId, Guid StatusLookUpId, string CancelNote, string Rate);

        bool UpdateSiteNote(Guid BookingId, string SiteNote);
    }
}
