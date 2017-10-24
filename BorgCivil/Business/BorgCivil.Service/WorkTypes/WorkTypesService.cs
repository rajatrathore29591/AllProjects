using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BorgCivil.Service
{
    public class WorkTypesService : IWorkTypesService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public WorkTypesService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region WorkTypesServices

        /// <summary>
        /// getting all WorkType from WorkTypes table
        /// </summary>
        /// <returns></returns>
        public List<WorkType> GetAllWorkTypes()
        {
            try
            {
                return unitOfWork.WorkTypesRepository.SearchBy<WorkType>(x => x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting WorkTypes detail by WorkTypeId
        /// </summary>
        /// <param name="WorkTypeId"></param>
        /// <returns></returns>
        public WorkType GetWorkTypeByWorkTypeId(Guid WorkTypeId)
        {
            try
            {
                //map entity.
                return unitOfWork.WorkTypesRepository.GetById<WorkType>(WorkTypeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add data in workType table
        /// </summary>
        /// <param name="WorkTypeDataModel"></param>
        /// <returns></returns>
        public string AddWorkType(WorkTypeDataModel WorkTypeDataModel)
        {
            try
            {
                // created new guid
                var Id = Guid.NewGuid();

                //map entity.
                WorkType entity = new WorkType()
                {
                    WorkTypeId = Id,
                    Type = WorkTypeDataModel.Type,
                    IsActive = WorkTypeDataModel.IsActive,
                    CreatedBy = WorkTypeDataModel.CreatedBy != null ? WorkTypeDataModel.CreatedBy : (Guid?)null,
                    EditedBy = WorkTypeDataModel.EditedBy != null ? WorkTypeDataModel.EditedBy : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow,
                };
                //add record from in database.
                unitOfWork.WorkTypesRepository.Insert<WorkType>(entity);

                ////save changes in database.
                unitOfWork.WorkTypesRepository.Commit();

                return Id.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to update the workType tables
        /// </summary>
        /// <param name="WorkTypeDataModel"></param>
        /// <returns></returns>
        public string UpdateWorkType(WorkTypeDataModel WorkTypeDataModel)
        {
            try
            {
                ////get workType data by WorkTypeId.
                var WorkTypeDetail = GetWorkTypeByWorkTypeId(WorkTypeDataModel.WorkTypeId);

                ////case of not null return 
                if (WorkTypeDetail != null)
                {
                    ////map entity.
                    WorkTypeDetail.Type = WorkTypeDataModel.Type;
                    WorkTypeDetail.EditedDate = System.DateTime.UtcNow;

                    ////update record into database entity.
                    unitOfWork.WorkTypesRepository.Update<WorkType>(WorkTypeDetail);

                    ////save changes in database.
                    unitOfWork.WorkTypesRepository.Commit();

                    ////case of update.
                    return WorkTypeDetail.WorkTypeId.ToString();
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
        /// delete WorkType from WorkType table
        /// </summary>
        /// <param name="WorkTypeId"></param>
        /// <returns></returns>
        public bool DeleteWorkType(Guid WorkTypeId)
        {
            try
            {
                ////get WorkType data by WorkTypeId.
                var WorkType = GetWorkTypeByWorkTypeId(WorkTypeId);

                ////case of not null return 
                if (WorkType != null)
                {
                    ////update record into database entity.
                    unitOfWork.WorkTypesRepository.Delete<WorkType>(WorkType);

                    ////save changes in database.
                    unitOfWork.WorkTypesRepository.Commit();

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
        /// This method for Get WorkTypes for select list type controls [Text, Value] pair.
        /// </summary>
        /// <returns>List of Select Value from WorkTypes entity.</returns>
        public List<SelectListModel> GetWorkTypesList()
        {
            try
            {
                ////get all customer records
                var types = unitOfWork.WorkTypesRepository.GetAll<WorkType>().ToList();

                ////check types is null or empity
                if (types != null)
                {
                    ////map entity to model.
                    return types.Select(item => new SelectListModel
                    {
                        Text = item.Type,
                        Value = item.WorkTypeId.ToString()
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
