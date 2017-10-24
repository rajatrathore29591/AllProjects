using BorgCivil.Framework.Entities;
using BorgCivil.Service;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IEmploymentCategoryService : IService
    {
        List<SelectListModel> GetEmploymentCategoryList();
    }
}
