using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BorgCivil.Service
{
    public class DocketCheckboxListService : IDocketCheckboxListService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public DocketCheckboxListService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region DocketCheckboxListService

        /// <summary>
        /// getting all CheckList from DocketCheckList table
        /// </summary>
        /// <returns></returns>
        public List<DocketCheckList> GetAllDocketCheckList()
        {
            try
            {
                return unitOfWork.DocketCheckListRepository.SearchBy<DocketCheckList>(x => x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// getting CheckList detail from DocketCheckList table
        /// </summary>
        /// <param name="DocketCheckListId"></param>
        /// <returns></returns>
        public DocketCheckList GetDocketCheckListDetail(Guid DocketCheckListId)
        {
            try
            {
                return unitOfWork.DocketCheckListRepository.FindBy<DocketCheckList>(x => x.IsActive == true && x.DocketCheckListId == DocketCheckListId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add docketCheckList
        /// </summary>
        /// <param name="DocketCheckboxListDataModel"></param>
        /// <returns></returns>
        public string AddDocketCheckboxList(DocketCheckboxListDataModel DocketCheckboxListDataModel)
        {
            try
            {
                // created new guid
                var Id = Guid.NewGuid();
                //map entity.
                DocketCheckList entity = new DocketCheckList()
                {

                    DocketCheckListId = Id,
                    Title = DocketCheckboxListDataModel.Title,
                    IsActive = DocketCheckboxListDataModel.IsActive,
                    CreatedBy = DocketCheckboxListDataModel.CreatedBy != null ? new Guid(DocketCheckboxListDataModel.CreatedBy) : (Guid?)null,
                    EditedBy = DocketCheckboxListDataModel.EditedBy != null ? new Guid(DocketCheckboxListDataModel.EditedBy) : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow,
                    EditedDate = null
                };
                //add record from in database.
                unitOfWork.DocketCheckListRepository.Insert<DocketCheckList>(entity);

                ////save changes in database.
                unitOfWork.DocketCheckListRepository.Commit();

                return Id.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
      /// method to update DocketCheckList entity
      /// </summary>
      /// <param name="DocketCheckboxListDataModel"></param>
      /// <returns></returns>
        public string UpdateDocketCheckboxList(DocketCheckboxListDataModel DocketCheckboxListDataModel)
        {
            try
            {
                ////get docketCheckList data by docketCheckListId.
                var DocketCheckListDetail = GetDocketCheckListDetail(DocketCheckboxListDataModel.DocketCheckListId);

                ////case of not null return 
                if (DocketCheckListDetail != null)
                {
                    ////map entity.
                    DocketCheckListDetail.Title = DocketCheckboxListDataModel.Title;
                    DocketCheckListDetail.EditedDate = System.DateTime.UtcNow;

                    ////update record into database entity.
                    unitOfWork.DocketCheckListRepository.Update<DocketCheckList>(DocketCheckListDetail);

                    ////save changes in database.
                    unitOfWork.DocketCheckListRepository.Commit();

                    ////case of update.
                    return DocketCheckListDetail.DocketCheckListId.ToString();
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
        /// delete DocketCheckbox from DocketCheckbox table
        /// </summary>
        /// <param name="DocketCheckListId"></param>
        /// <returns></returns>
        public bool DeleteDocketCheckboxList(Guid DocketCheckListId)
        {
            try
            {
                ////get DocketCheckList data by DocketCheckListId.
                var DocketCheckListDetail = GetDocketCheckListDetail(DocketCheckListId);

                ////case of not null return 
                if (DocketCheckListDetail != null)
                {
                    ////update record into database entity.
                    unitOfWork.DocketCheckListRepository.Delete<DocketCheckList>(DocketCheckListDetail);

                    ////save changes in database.
                    unitOfWork.DocketCheckListRepository.Commit();

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

        #endregion

    }
}
