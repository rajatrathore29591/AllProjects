using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BorgCivil.Service
{
    public class DriverWhiteCardService : IDriverWhiteCardService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public DriverWhiteCardService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region DriverWhiteCardService

        /// <summary>
        /// this method is used for getting DriverWhiteCard by DriverId
        /// </summary>
        /// <param name="DriverId"></param>
        /// <returns></returns>
        public List<DriverWhiteCard> GetDriverWhiteCardByDriverId(Guid DriverId)
        {
            try
            {
                //map entity.
                return unitOfWork.DriverWhiteCardRepository.SearchBy<DriverWhiteCard>(x => x.DriverId == DriverId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add data in DriverWhiteCard table
        /// </summary>
        /// <param name="DriverWhiteCardDataModel"></param>
        /// <returns></returns>
        public bool AddDriverWhiteCard(DriverWhiteCardDataModel DriverWhiteCardDataModel)
        {
            try
            {
                // created new guid
                var Id = Guid.NewGuid();
                //map entity.
                DriverWhiteCard entity = new DriverWhiteCard()
                {
                    DriverWhiteCardId = Id,
                    DriverId = DriverWhiteCardDataModel.DriverId,
                    CardNumber = DriverWhiteCardDataModel.CardNumber,
                    IssueDate = DriverWhiteCardDataModel.IssueDate,
                    Notes = DriverWhiteCardDataModel.Notes,
                    IsActive = DriverWhiteCardDataModel.IsActive,
                    CreatedBy = DriverWhiteCardDataModel.CreatedBy != null ? new Guid(DriverWhiteCardDataModel.CreatedBy) : (Guid?)null,
                    EditedBy = DriverWhiteCardDataModel.EditedBy != null ? new Guid(DriverWhiteCardDataModel.EditedBy) : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow,
                    ModifiedDate = null
                };
                //add record from in database.
                unitOfWork.DriverWhiteCardRepository.Insert<DriverWhiteCard>(entity);

                ////save changes in database.
                unitOfWork.DriverWhiteCardRepository.Commit();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// deleteing DriverWhiteCard
        /// </summary>
        /// <param name="DriverId"></param>
        /// <returns></returns>
        public bool DeleteDriverWhiteCardByDriverId(Guid DriverId)
        {
            try
            {
                ////get DriverWhiteCard data by DriverId.
                var DriverWhiteCardDetail = GetDriverWhiteCardByDriverId(DriverId);

                ////case of not null return 
                if (DriverWhiteCardDetail != null)
                {

                    ////update record into database entity.
                    unitOfWork.DriverWhiteCardRepository.Delete<DriverWhiteCard>(DriverWhiteCardDetail);

                    ////save changes in database.
                    unitOfWork.DriverWhiteCardRepository.Commit();

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
