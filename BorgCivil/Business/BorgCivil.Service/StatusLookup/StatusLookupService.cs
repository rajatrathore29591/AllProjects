using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BorgCivil.Service
{
    public class StatusLookupService : IStatusLookupService
    {

        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public StatusLookupService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region StatusLookupService

        /// <summary>
        /// this method is used for getting record on the basis of group type
        /// </summary>
        /// <param name="GroupName"></param>
        /// <returns></returns>
        public StatusLookup GetStatusByTitle(string Tiltle)
        {
            try
            {
                //map entity.
                return unitOfWork.StatusLookupRepository.FindBy<StatusLookup>(x => x.Title == Tiltle && x.IsActive == true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method for Get Status for select list type controls [Text, Value] pair.
        /// </summary>
        /// <returns>List of Select Value from StatusLookUp entity.</returns>
        public List<SelectListModel> GetStatusLookupList()
        {
            try
            {
                ////get all StatusLookup records
                var types = unitOfWork.StatusLookupRepository.GetAll<StatusLookup>().Where(x => x.Group == "WorkAllocation").ToList();

                ////check types is null or empity
                if (types != null)
                {
                    ////map entity to model.
                    return types.Select(item => new SelectListModel
                    {
                        Text = item.Title,
                        Value = item.StatusLookupId.ToString()
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
