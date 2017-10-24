using System;
using System.Collections.Generic;
using System.Linq;
using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;

namespace BorgCivil.Service
{
    public class CountryService : BaseService, ICountryService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public CountryService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        /// <summary>
        /// get country by countryId.
        /// </summary>
        /// <param name="id">country Id</param>
        /// <returns>Object country entity</returns>
        public Country GetCountryById(Guid countryId)
        {
            try
            {
                ////return object entity
                return unitOfWork.CountryRepository.FindBy<Country>(countryId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<Document> GetDocuments(Guid userId)
        //{
        //    throw new NotImplementedException();
        //}



        /// <summary>
        /// This method for Get TaskType for select list type controls [Text, Value] pair.
        /// </summary>
        /// <returns>List of Select Value from TaskType entity.</returns>
        public List<SelectListModel> GetCountryList()
        {
            try
            {
                ////get all active records
                var types = unitOfWork.CountryRepository.GetAll<Country>().ToList().OrderBy(z =>z.Name);

                ////check types is null or empity
                if (types != null)
                {
                    ////map entity to model.
                    return types.Select(item => new SelectListModel
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
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
    }
}
