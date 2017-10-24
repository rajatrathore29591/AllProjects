using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BorgCivil.Service
{
    public class DriverInductionCardService : IDriverInductionCardService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public DriverInductionCardService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region DriverInductionCardService

        /// <summary>
        /// this method is used for getting DriverInductionCard by DriverId
        /// </summary>
        /// <param name="DriverId"></param>
        /// <returns></returns>
        public List<DriverInductionCard> GetDriverInductionCardByDriverId(Guid DriverId)
        {
            try
            {
                //map entity.
                return unitOfWork.DriverInductionCardRepository.SearchBy<DriverInductionCard>(x => x.DriverId == DriverId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add data in DriverInductionCard table
        /// </summary>
        /// <param name="DriverInductionCardDataModel"></param>
        /// <returns></returns>
        public bool AddDriverInductionCard(DriverInductionCardDataModel DriverInductionCardDataModel)
        {
            try
            {
                // created new guid
                var Id = Guid.NewGuid();
                //map entity.
                DriverInductionCard entity = new DriverInductionCard()
                {
                    DriverInductionCardId = Id,
                    DriverId = DriverInductionCardDataModel.DriverId,
                    CardNumber = DriverInductionCardDataModel.CardNumber,
                    SiteCost = DriverInductionCardDataModel.SiteCost,
                    ExpiryDate = DriverInductionCardDataModel.ExpiryDate,
                    Notes = DriverInductionCardDataModel.Notes,
                    IssueDate = DriverInductionCardDataModel.IssueDate,
                    IsActive = DriverInductionCardDataModel.IsActive,
                    CreatedBy = DriverInductionCardDataModel.CreatedBy != null ? new Guid(DriverInductionCardDataModel.CreatedBy) : (Guid?)null,
                    EditedBy = DriverInductionCardDataModel.EditedBy != null ? new Guid(DriverInductionCardDataModel.EditedBy) : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow,
                    ModifiedDate = null
                };
                //add record from in database.
                unitOfWork.DriverInductionCardRepository.Insert<DriverInductionCard>(entity);

                ////save changes in database.
                unitOfWork.DriverInductionCardRepository.Commit();

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
        public bool DeleteDriverInductionCardByDriverId(Guid DriverId)
        {
            try
            {
                ////get DriverInductionCard data by DriverId.
                var DriverInductionCardDetail = GetDriverInductionCardByDriverId(DriverId);

                ////case of not null return 
                if (DriverInductionCardDetail != null)
                {

                    ////update record into database entity.
                    unitOfWork.DriverInductionCardRepository.Delete<DriverInductionCard>(DriverInductionCardDetail);

                    ////save changes in database.
                    unitOfWork.DriverInductionCardRepository.Commit();

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
