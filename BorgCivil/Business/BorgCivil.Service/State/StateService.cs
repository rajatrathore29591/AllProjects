using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BorgCivil.Service
{

    public class StateService : BaseService, IStateService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public StateService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion
        
        public State GetStateByStateId(Guid stateId)
        {
            try
            {
                //map entity.
                return unitOfWork.StateRepository.GetById<State>(stateId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all state list by country id
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public List<SelectListModel> GetAllStateByCountryId(Guid countryId)
        {
            try
            {
                var state = unitOfWork.StateRepository.SearchBy<State>(x => x.CountryId == countryId).ToList().OrderBy(z => z.Name);
                if (state != null)
                {
                    ////map entity to model.
                    return state.Select(item => new SelectListModel
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
                    }
                   ).ToList();
                }
                //map entity.
                return new List<SelectListModel>();
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
