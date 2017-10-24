using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IDocketService : IService
    {
        List<Docket> GetAllDocketByBookingId(Guid BookingFleetId);
        Docket GetDocketByDocketId(Guid DocketId);
        string AddDocket(DocketDataModel DocketDataModel);
        string UpdateDocket(DocketDataModel DocketDataModel);
        bool UpdateDocumentId(Guid DocketId, Guid DocumentId);


    }
}
