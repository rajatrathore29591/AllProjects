using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BorgCivil.Service
{
    public class EmploymentCategoryService : IEmploymentCategoryService
    {

        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public EmploymentCategoryService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region EmploymentCategoryService

        /// <summary>
        /// This method for Get category for select list type controls [Text, Value] pair.
        /// </summary>
        /// <returns>List of Select Value from EmploymentCategory entity.</returns>
        public List<SelectListModel> GetEmploymentCategoryList()
        {
            try
            {
                ////get all EmploymentCategory records
                var types = unitOfWork.EmploymentCategoryRepository.GetAll<EmploymentCategory>().ToList();

                ////check types is null or empity
                if (types != null)
                {
                    ////map entity to model.
                    return types.Select(item => new SelectListModel
                    {
                        Text = item.Category,
                        Value = item.EmploymentCategoryId.ToString()
                    }
                   ).ToList();
                }
                return new List<SelectListModel>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
