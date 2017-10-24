using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface ISupervisorService : IService
    {
        List<Supervisor> GetAllSupervisor();

        Supervisor GetSupervisorBySupervisorId(Guid SupervisorId);

        List<Supervisor> GetSupervisorListBySiteId(Guid SiteId);

        string AddSupervisor(SupervisorDataModel SupervisorDataModel);

        string UpdateSupervisor(SupervisorDataModel SupervisorDataModel);

        bool DeleteSupervisor(Guid SupervisorId);

        List<SelectListModel> GetSupervisorList(Guid SiteId);
    }
}
