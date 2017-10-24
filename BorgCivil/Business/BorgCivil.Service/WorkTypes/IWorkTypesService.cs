using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IWorkTypesService : IService
    {
        List<WorkType> GetAllWorkTypes();

        WorkType GetWorkTypeByWorkTypeId(Guid WorkTypeId);

        string AddWorkType(WorkTypeDataModel WorkTypeDataModel);

        string UpdateWorkType(WorkTypeDataModel WorkTypeDataModel);

        bool DeleteWorkType(Guid WorkTypeId);

        List<SelectListModel> GetWorkTypesList();
    }
}
