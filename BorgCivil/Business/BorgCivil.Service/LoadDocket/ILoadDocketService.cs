using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface ILoadDocketService : IService
    {
        List<LoadDocket> GetAllLoadDocketByDocketId(Guid DocketId);
        bool AddLoadDocket(LoadDocketDataModel LoadDocketDataModel);
        bool DeleteLoadDocket(Guid DocketId);


    }
}
