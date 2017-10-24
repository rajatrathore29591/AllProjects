using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IDocketCheckboxListService : IService
    {
        List<DocketCheckList> GetAllDocketCheckList();
        DocketCheckList GetDocketCheckListDetail(Guid DocketCheckListId);
        string AddDocketCheckboxList(DocketCheckboxListDataModel DocketCheckboxListDataModel);
        string UpdateDocketCheckboxList(DocketCheckboxListDataModel DocketCheckboxListDataModel);
        bool DeleteDocketCheckboxList(Guid DocketCheckListId);
    }
}
