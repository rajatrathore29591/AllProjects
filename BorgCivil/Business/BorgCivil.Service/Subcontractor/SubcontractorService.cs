using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BorgCivil.Service
{

    public class SubcontractorService : BaseService, ISubcontractorService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public SubcontractorService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        /// <summary>
        /// Get all Subcontractor list
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<SelectListModel> GetSubcontractor()
        {
            try
            {
                var Subcontractor = unitOfWork.SubcontractorRepository.SearchBy<Subcontractor>(x => x.IsActive == true).ToList();
                if (Subcontractor != null)
                {
                    ////map entity to model.
                    return Subcontractor.Select(item => new SelectListModel
                    {
                        Text = item.Name,
                        Value = item.SubcontractorId.ToString()
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
