using BorgCivil.Framework.Entities;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface ISubcontractorService : IService
    {

        List<SelectListModel> GetSubcontractor();
    }
}
