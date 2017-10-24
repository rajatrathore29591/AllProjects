using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BorgCivil.Service
{
    public class SupervisorService : ISupervisorService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public SupervisorService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region SupervisorServices

        /// <summary>
        /// getting all Supervisor from Supervisor table
        /// </summary>
        /// <returns></returns>
        public List<Supervisor> GetAllSupervisor()
        {
            try
            {
                return unitOfWork.SupervisorRepository.SearchBy<Supervisor>(x => x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting Supervisor detail by SupervisorId
        /// </summary>
        /// <param name="SupervisorId"></param>
        /// <returns></returns>
        public Supervisor GetSupervisorBySupervisorId(Guid SupervisorId)
        {
            try
            {
                //map entity.
                return unitOfWork.SupervisorRepository.GetById<Supervisor>(SupervisorId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting Supervisor detail by siteId
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public List<Supervisor> GetSupervisorListBySiteId(Guid SiteId)
        {
            try
            {
                //map entity.
                return unitOfWork.SupervisorRepository.SearchBy<Supervisor>(x => x.SiteId == SiteId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add data in Supervisor table
        /// </summary>
        /// <param name="SupervisorDataModel"></param>
        /// <returns></returns>
        public string AddSupervisor(SupervisorDataModel SupervisorDataModel)
        {
            try
            {
                // created new guid
                var Id = Guid.NewGuid();

                //map entity.
                Supervisor entity = new Supervisor()
                {
                    SupervisorId = Id,
                    SiteId = SupervisorDataModel.SiteId,
                    SupervisorName = SupervisorDataModel.SupervisorName,
                    Email = SupervisorDataModel.Email,
                    MobileNumber = SupervisorDataModel.MobileNumber,
                    IsActive = SupervisorDataModel.IsActive,
                    CreatedBy = SupervisorDataModel.CreatedBy != null ? SupervisorDataModel.CreatedBy : (Guid?)null,
                    EditedBy = SupervisorDataModel.EditedBy != null ? SupervisorDataModel.EditedBy : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow
                };
                //add record from in database.
                unitOfWork.SupervisorRepository.Insert<Supervisor>(entity);

                ////save changes in database.
                unitOfWork.SupervisorRepository.Commit();

                return Id.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to update the supervisor tables
        /// </summary>
        /// <param name="SupervisorDataModel"></param>
        /// <returns></returns>
        public string UpdateSupervisor(SupervisorDataModel SupervisorDataModel)
        {
            try
            {
                ////get supervisor data by supervisorId.
                var SupervisorDetail = GetSupervisorBySupervisorId(SupervisorDataModel.SupervisorId);

                ////case of not null return 
                if (SupervisorDetail != null)
                {
                    ////map entity.
                    //SupervisorDetail.SiteId = SupervisorDataModel.SiteId;
                    SupervisorDetail.SupervisorName = SupervisorDataModel.SupervisorName;
                    SupervisorDetail.Email = SupervisorDataModel.Email;
                    SupervisorDetail.MobileNumber = SupervisorDataModel.MobileNumber;
                    SupervisorDetail.IsActive = SupervisorDataModel.IsActive;
                    SupervisorDetail.CreatedBy = SupervisorDataModel.CreatedBy != null ? SupervisorDataModel.CreatedBy : (Guid?)null;
                    SupervisorDetail.EditedBy = SupervisorDataModel.EditedBy != null ? SupervisorDataModel.EditedBy : (Guid?)null;
                    SupervisorDetail.EditedDate = System.DateTime.UtcNow;

                    ////update record into database entity.
                    unitOfWork.SupervisorRepository.Update<Supervisor>(SupervisorDetail);

                    ////save changes in database.
                    unitOfWork.SupervisorRepository.Commit();

                    ////case of update.
                    return SupervisorDetail.SupervisorId.ToString();
                }
                else
                {
                    ////case of null return null object.
                    return "error";
                }
            }
            catch (Exception ex)
            {
                ////case of error throw
                throw ex;
            }
        }

        /// <summary>
        /// delete Supervisor from supervisor table
        /// </summary>
        /// <param name="SupervisorId"></param>
        /// <returns></returns>
        public bool DeleteSupervisor(Guid SupervisorId)
        {
            try
            {
                ////get supervisor data by SupervisorId.
                var Supervisor = GetSupervisorBySupervisorId(SupervisorId);

                ////case of not null return 
                if (Supervisor != null)
                {
                    ////update record into database entity.
                    unitOfWork.SupervisorRepository.Delete<Supervisor>(Supervisor);

                    ////save changes in database.
                    unitOfWork.SupervisorRepository.Commit();

                    ////case of update.
                    return true;
                }
                else
                {
                    ////case of null return null object.
                    return false;
                }
            }
            catch (Exception ex)
            {
                ////case of error throw
                throw ex;
            }
        }

        /// <summary>
        /// This method for Get Supervisor for select list type controls [Text, Value] pair.
        /// </summary>
        /// params SiteId
        /// <returns>List of Select Value from Supervisor entity.</returns>
        public List<SelectListModel> GetSupervisorList(Guid SiteId)
        {
            try
            {
                ////get all Supervisor records
                var types = unitOfWork.SupervisorRepository.SearchBy<Supervisor>(x => x.SiteId == SiteId).ToList();

                ////check types is null or empity
                if (types != null)
                {
                    ////map entity to model.
                    return types.Select(item => new SelectListModel
                    {
                        Text = item.SupervisorName,
                        Value = item.SupervisorId.ToString()
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
