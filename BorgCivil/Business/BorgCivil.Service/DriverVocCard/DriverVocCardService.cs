using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BorgCivil.Service
{
    public class DriverVocCardService : IDriverVocCardService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public DriverVocCardService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region DriverVocCardService

        /// <summary>
        /// this method is used for getting DriverVocCard by DriverId
        /// </summary>
        /// <param name="DriverId"></param>
        /// <returns></returns>
        public List<DriverVocCard> GetDriverVocCardByDriverId(Guid DriverId)
        {
            try
            {
                //map entity.
                return unitOfWork.DriverVocCardRepository.SearchBy<DriverVocCard>(x => x.DriverId == DriverId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add data in DriverVocCard table
        /// </summary>
        /// <param name="DriverVocCardDataModel"></param>
        /// <returns></returns>
        public bool AddDriverVocCard(DriverVocCardDataModel DriverVocCardDataModel)
        {
            try
            {
                // created new guid
                var Id = Guid.NewGuid();
                //map entity.
                DriverVocCard entity = new DriverVocCard()
                {
                    DriverVocCardId = Id,
                    DriverId = DriverVocCardDataModel.DriverId,
                    CardNumber = DriverVocCardDataModel.CardNumber,
                    RTONumber = DriverVocCardDataModel.RTONumber,
                    IssueDate = DriverVocCardDataModel.IssueDate,
                    Notes = DriverVocCardDataModel.Notes,
                    IsActive = DriverVocCardDataModel.IsActive,
                    CreatedBy = DriverVocCardDataModel.CreatedBy != null ? new Guid(DriverVocCardDataModel.CreatedBy) : (Guid?)null,
                    EditedBy = DriverVocCardDataModel.EditedBy != null ? new Guid(DriverVocCardDataModel.EditedBy) : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow,
                    ModifiedDate = null
                };
                //add record from in database.
                unitOfWork.DriverVocCardRepository.Insert<DriverVocCard>(entity);

                ////save changes in database.
                unitOfWork.DriverVocCardRepository.Commit();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// deleteing DriverVocCard
        /// </summary>
        /// <param name="DriverId"></param>
        /// <returns></returns>
        public bool DeleteDriverVocCardByDriverId(Guid DriverId)
        {
            try
            {
                ////get DriverVocCard data by DriverId.
                var DriverVocCardDetail = GetDriverVocCardByDriverId(DriverId);

                ////case of not null return 
                if (DriverVocCardDetail != null)
                {

                    ////update record into database entity.
                    unitOfWork.DriverVocCardRepository.Delete<DriverVocCard>(DriverVocCardDetail);

                    ////save changes in database.
                    unitOfWork.DriverVocCardRepository.Commit();

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
