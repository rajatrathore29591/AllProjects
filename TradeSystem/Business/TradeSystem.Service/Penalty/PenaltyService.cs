using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;
using TradeSystem.Repositories;
using TradeSystem.Service;
using TradeSystem.Utils.Models;

namespace TradeSystem.Service
{
    public class PenaltyService : BaseService, IPenaltyService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public PenaltyService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion
      
        /// This method for Get All Penalty.
        /// </summary>
        /// <returns>Penalty</returns>
        public List<Penalty> GetAllPenalty()
        {
            try
            {
                return unitOfWork.PenaltyRepository.GetAll<Penalty>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method is used to add penalty of product
        /// </summary>
        /// <param name="penaltyDataModel"></param>
        /// <returns></returns>
        public bool AddPenalty(PenaltyDataModel penaltyDataModel)
        {
            try
            {
                //map entity.
                Penalty entity = new Penalty()
                {
                    Id = Guid.NewGuid(),
                    ProductId = new Guid(penaltyDataModel.ProductId),
                    From = penaltyDataModel.PenaltyFrom,
                    To = penaltyDataModel.PenaltyTo,
                    PenaltyPercent = penaltyDataModel.PenaltyPercent,
                    CreatedDate = DateTime.UtcNow,          
                    ModifiedDate = DateTime.UtcNow
                };
                //add record from in database.
                unitOfWork.PenaltyRepository.Insert<Penalty>(entity);
                ////save changes in database.
                unitOfWork.PenaltyRepository.Commit();
                
                return true;
            }
            catch (Exception ex)
            {
                throw ex;               
            }          
        }

        /// This method for Get All Penalty of product by productid.
        /// </summary>
        /// <returns>Products</returns>
        public List<Penalty> GetAllPenaltyByProductId(string productId)
        {
            try
            {
                var penalty = unitOfWork.PenaltyRepository.SearchBy<Penalty>(x => x.ProductId == new Guid(productId)).ToList();
                return penalty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get penalty by customerId.
        /// </summary>
        /// <param name="id">Customer Id</param>
        /// <returns>Object Customer entity</returns>
        public Penalty GetPenaltyByCustomerId(Guid customerId)
        {
            try
            {
                ////return object entity
                return unitOfWork.PenaltyRepository.FindBy<Penalty>(customerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
