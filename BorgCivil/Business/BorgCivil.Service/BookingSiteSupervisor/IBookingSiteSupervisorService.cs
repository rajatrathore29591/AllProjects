using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IBookingSiteSupervisorService : IService
    {
        BookingSiteSupervisor GetBookingSiteSupervisorByBookingSupervisorId(Guid BookingSiteSupervisorId);
        BookingSiteSupervisor GetBookingSiteSupervisorBySupervisorId(Guid SupervisorId);
        List<BookingSiteSupervisor> GetBookingSiteSupervisorByBookingId(Guid BookingId);
        bool AddBookingSiteSupervisor(BookingSiteDetailDataModel BookingSiteDetailDataModel);
        bool UpdateBookingSiteSupervisor(BookingSiteDetailDataModel BookingSiteDetailDataModel);
        bool DeleteBookingSiteSupervisor(Guid SupervisorId);
        bool DeleteBookingSiteSupervisorByBookingId(Guid BookingId);
    }
}
