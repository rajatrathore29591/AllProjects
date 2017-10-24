using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BorgCivil.Service
{
    public class AnonymousFieldService : IAnonymousFieldService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public AnonymousFieldService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region AnonymousFieldService

        /// <summary>
        /// this method is used for getting AnonymousField by DriverId
        /// </summary>
        /// <param name="DriverId"></param>
        /// <returns></returns>
        public List<AnonymousField> GetAnonymousFieldByAnonymousFieldId(Guid DriverId)
        {
            try
            {
                //map entity.
                return unitOfWork.AnonymousFieldRepository.SearchBy<AnonymousField>(x => x.DriverId == DriverId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add data in AnonymousField table
        /// </summary>
        /// <param name="AnonymousFieldDataModel"></param>
        /// <returns></returns>
        public bool AddAnonymousField(AnonymousFieldDataModel AnonymousFieldDataModel)
        {
            try
            {
                // created new guid
                var Id = Guid.NewGuid();
                //map entity.
                AnonymousField entity = new AnonymousField()
                {
                    AnonymousFieldId = Id,
                    DriverId = AnonymousFieldDataModel.DriverId,
                    Title = AnonymousFieldDataModel.Title,
                    Other1 = AnonymousFieldDataModel.Other1,
                    Other2 = AnonymousFieldDataModel.Other2,
                    IssueDate = Convert.ToDateTime(AnonymousFieldDataModel.IssueDate),
                    ExpiryDate = Convert.ToDateTime(AnonymousFieldDataModel.ExpiryDate),
                    Notes = AnonymousFieldDataModel.Notes,
                    IsActive = AnonymousFieldDataModel.IsActive,
                    CreatedBy = AnonymousFieldDataModel.CreatedBy != null ? new Guid(AnonymousFieldDataModel.CreatedBy) : (Guid?)null,
                    EditedBy = AnonymousFieldDataModel.EditedBy != null ? new Guid(AnonymousFieldDataModel.EditedBy) : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow,
                    ModifiedDate = null
                };
                //add record from in database.
                unitOfWork.AnonymousFieldRepository.Insert<AnonymousField>(entity);

                ////save changes in database.
                unitOfWork.AnonymousFieldRepository.Commit();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// deleteing AnonymousField
        /// </summary>
        /// <param name="AnonymousFieldId"></param>
        /// <returns></returns>
        public bool DeleteAnonymousFieldByDriverId(Guid DriverId)
        {
            try
            {
                ////get AnonymousField data by AnonymousFieldId.
                var AnonymousFieldDetail = GetAnonymousFieldByAnonymousFieldId(DriverId);

                ////case of not null return 
                if (AnonymousFieldDetail != null)
                {

                    ////update record into database entity.
                    unitOfWork.AnonymousFieldRepository.Delete<AnonymousField>(AnonymousFieldDetail);

                    ////save changes in database.
                    unitOfWork.AnonymousFieldRepository.Commit();

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
