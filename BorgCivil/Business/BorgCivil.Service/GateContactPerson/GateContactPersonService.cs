using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BorgCivil.Service
{
    public class GateContactPersonService : IGateContactPersonService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public GateContactPersonService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region GateContactPersonService

        /// <summary>
        /// this method is used for getting all GateContactPerson 
        /// </summary>
        /// <returns></returns>
        public List<GateContactPerson> GetAllGateContactPerson()
        {
            try
            {
                //map entity.
                return unitOfWork.GateContactPersonRepository.SearchBy<GateContactPerson>(x => x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method for Get gate Contact for select list type controls [Text, Value] pair.
        /// </summary>
        /// params SiteId
        /// <returns>List of Select Value from GateContactPerson entity.</returns>
        public List<GateContactPerson> GetGateContactPersonList(Guid GateId)
        {
            try
            {
                //map entity.
                return unitOfWork.GatesRepository.SearchBy<GateContactPerson>(x => x.GateId == GateId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method for Get gate Contact person by GateContactPersonId
        /// </summary>
        /// params GateContactPersonId
        /// <returns>List of Select Value from GateContactPerson entity.</returns>
        public GateContactPerson GetGateContactByGateContactId(Guid GateContactPersonId)
        {
            try
            {
                //map entity.
                return unitOfWork.GateContactPersonRepository.FindBy<GateContactPerson>(x => x.GateContactPersonId == GateContactPersonId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method for Get gate Contact person by GateId
        /// </summary>
        /// params GateId
        /// <returns>List of Select Value from GetGateContactIsDefaultByContactId entity.</returns>
        public GateContactPerson GetGateContactIsDefaultByGateId(Guid GateId)
        {
            try
            {
                //map entity.
                return unitOfWork.GateContactPersonRepository.FindBy<GateContactPerson>(x => x.GateId == GateId && x.IsDefault == true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add data in gatecontactperson table
        /// </summary>
        /// <param name="GateContactPersonDataModel"></param>
        /// <returns></returns>
        public bool AddGateContactPerson(GateContactPersonDataModel GateContactPersonDataModel)
        {
            try
            {
                // created new guid
                var Id = Guid.NewGuid();
                //map entity.
                GateContactPerson entity = new GateContactPerson()
                {
                    GateContactPersonId = Id,
                    GateId = GateContactPersonDataModel.GateId,
                    ContactPerson = GateContactPersonDataModel.ContactPerson,
                    Email = GateContactPersonDataModel.Email,
                    MobileNumber = GateContactPersonDataModel.MobileNumber,
                    IsActive = GateContactPersonDataModel.IsActive,
                    IsDefault = GateContactPersonDataModel.IsDefault,
                    CreatedBy = GateContactPersonDataModel.CreatedBy != null ? GateContactPersonDataModel.CreatedBy : (Guid?)null,
                    EditedBy = GateContactPersonDataModel.EditedBy != null ? GateContactPersonDataModel.EditedBy : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow,
                    EditedDate = null,
                };
                //add record from in database.
                unitOfWork.GateContactPersonRepository.Insert<GateContactPerson>(entity);

                ////save changes in database.
                unitOfWork.GateContactPersonRepository.Commit();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for updating gateContactPerson table
        /// </summary>
        /// <param name="GateContactPersonDataModel"></param>
        /// <returns></returns>
        public bool UpdateGateContactPerson(GateContactPersonDataModel GateContactPersonDataModel)
        {
            try
            {
                //get existing record.
                GateContactPerson entity = GetGateContactByGateContactId(GateContactPersonDataModel.GateContactPersonId);

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.GateId =GateContactPersonDataModel.GateId;
                    entity.ContactPerson = GateContactPersonDataModel.ContactPerson;
                    entity.Email = GateContactPersonDataModel.Email ;
                    entity.MobileNumber = GateContactPersonDataModel.MobileNumber;
                    entity.IsDefault = GateContactPersonDataModel.IsDefault;
                    entity.EditedBy = GateContactPersonDataModel.EditedBy != null ? GateContactPersonDataModel.EditedBy : (Guid?)null;
                    entity.EditedDate = System.DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.GateContactPersonRepository.Update<GateContactPerson>(entity);

                    ////save changes in database.
                    unitOfWork.GateContactPersonRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for updating status table
        /// </summary>
        /// <param name="GateContactPersonId"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public bool UpdateIsDefaultStatus(Guid GateContactPersonId, bool Status)
        {
            try
            {
                //get existing record.
                GateContactPerson entity = GetGateContactByGateContactId(GateContactPersonId);

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.IsDefault = Status;

                    //update record from existing entity in database.
                    unitOfWork.GateContactPersonRepository.Update<GateContactPerson>(entity);

                    ////save changes in database.
                    unitOfWork.GateContactPersonRepository.Commit();

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
