using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BorgCivil.Service
{
    public class LicenseClassService : ILicenseClassService
    {

        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public LicenseClassService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region LicenseClassService

        /// <summary>
        /// This method for Get LicenseClass for select list type controls [Text, Value] pair.
        /// </summary>
        /// <returns>List of Select Value from LicenseClass entity.</returns>
        public List<SelectListModel> GetLicenseClassList()
        {
            try
            {
                ////get all LicenseClass records
                var types = unitOfWork.LicenseClassRepository.GetAll<LicenseClass>().ToList();

                ////check types is null or empity
                if (types != null)
                {
                    ////map entity to model.
                    return types.Select(item => new SelectListModel
                    {
                        Text = item.Class,
                        Value = item.LicenseClassId.ToString()
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
