using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BorgCivil.Service
{
    public class GatesService : IGatesService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public GatesService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region GatesServices

        /// <summary>
        /// getting all gate from gates table
        /// </summary>
        /// <returns></returns>
        public List<Gate> GetAllGates()
        {
            try
            {
                return unitOfWork.GatesRepository.SearchBy<Gate>(x => x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting gates detail by gateId
        /// </summary>
        /// <param name="GateId"></param>
        /// <returns></returns>
        public Gate GetGateByGateId(Guid GateId)
        {
            try
            {
                //map entity.
                return unitOfWork.GatesRepository.GetById<Gate>(GateId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting gate list by siteId
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public List<Gate> GetGateListBySiteId(Guid SiteId)
        {
            try
            {
                //map entity.
                return unitOfWork.GatesRepository.SearchBy<Gate>(x => x.SiteId == SiteId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method for Get gates for select list type controls [Text, Value] pair.
        /// </summary>
        /// params SiteId
        /// <returns>List of Select Value from Gates entity.</returns>
        public List<SelectListModel> GetGatesList(Guid SiteId)
        {
            try
            {
                ////get all gates records
                var types = unitOfWork.GatesRepository.SearchBy<Gate>(x => x.SiteId == SiteId).ToList();

                ////check types is null or empity
                if (types != null)
                {
                    ////map entity to model.
                    return types.Select(item => new SelectListModel
                    {
                        Text = item.GateNumber,
                        Value = item.GateId.ToString()
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

        /// <summary>
        /// method to add data in gate table
        /// </summary>
        /// <param name="GateDataModel"></param>
        /// <returns></returns>
        public bool AddGate(GateDataModel GateDataModel)
        {
            try
            {
                // created new guid
                var Id = Guid.NewGuid();
                //map entity.
                Gate entity = new Gate()
                {
                    GateId = Id,
                    SiteId = GateDataModel.SiteId,
                    GateNumber = GateDataModel.GateNumber,
                    TipOffRate = GateDataModel.TipOffRate,
                    TippingSite = GateDataModel.TippingSite,
                    IsActive = GateDataModel.IsActive,
                    CreatedBy = GateDataModel.CreatedBy != null ? GateDataModel.CreatedBy : (Guid?)null,
                    EditedBy = GateDataModel.EditedBy != null ? GateDataModel.EditedBy : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow,
                    EditedDate = null,
                };
                //add record from in database.
                unitOfWork.GatesRepository.Insert<Gate>(entity);

                ////save changes in database.
                unitOfWork.GatesRepository.Commit();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for updating gate table
        /// </summary>
        /// <param name="GateDataModel"></param>
        /// <returns></returns>
        public bool UpdateGate(GateDataModel GateDataModel)
        {
            try
            {
                //get existing record.
                Gate entity = GetGateByGateId(GateDataModel.GateId);

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.SiteId = GateDataModel.SiteId;
                    entity.GateNumber = GateDataModel.GateNumber;
                    entity.TipOffRate = GateDataModel.TipOffRate;
                    entity.TippingSite = GateDataModel.TippingSite;
                    entity.EditedDate = System.DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.GatesRepository.Update<Gate>(entity);

                    ////save changes in database.
                    unitOfWork.GatesRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
