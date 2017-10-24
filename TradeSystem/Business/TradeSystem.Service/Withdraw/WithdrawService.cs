using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;
using TradeSystem.Repositories;
using TradeSystem.Utils.Models;

namespace TradeSystem.Service
{
    public class WithdrawService : BaseService, IWithdrawService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public WithdrawService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        /// This method for Get All Finance List.
        /// </summary>
        /// <returns>Finance List</returns>
        public List<Withdraw> GetAllFinanceList()
        {
            try
            {
                return unitOfWork.WithdrawRepository.GetAll<Withdraw>().OrderByDescending(x => x.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method is used to get finance detail by financeId from withdraw table
        /// </summary>
        /// <param name="id">withdraw Id</param>
        /// <returns>Object withdraw entity</returns>
        public Withdraw GetFinanceByFinanceId(Guid FinanceId)
        {
            try
            {
                ////return object entity
                return unitOfWork.WithdrawRepository.FindBy<Withdraw>(FinanceId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get withdarw by CustomerProductId.
        /// </summary>
        /// <param name="id">CustomerProductId</param>
        /// <returns>Object withdraw entity</returns>
        public List<Withdraw> GetTotalWithdrawByCustomerProductId(Guid CustomerProductId)
        {
            try
            {
                ////return object entity
                return unitOfWork.WithdrawRepository.SearchBy<Withdraw>(x => x.CustomerProductId == CustomerProductId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method for use finance status update info.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public bool EditFinance(Guid id, string Status)
        {
            try
            {
                ////get existing record.
                Withdraw entity = GetFinanceByFinanceId(id);

                ////map entity
                entity.Status = Status;
                ////update record from existing entity in database.
                unitOfWork.WithdrawRepository.Update<Withdraw>(entity);
                ////save changes in database.
                unitOfWork.WithdrawRepository.Commit();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method for add finance status info.
        /// </summary>
        /// <param name="withdrawDataModel"></param>
        /// <returns></returns>
        public string AddWithdraw(WithdrawDataModel withdrawDataModel)
        {
            try
            {
                var response = string.Empty;
                //map entity.
                Withdraw entity = new Withdraw()
                {
                    Id = Guid.NewGuid(),
                    CustomerProductId = withdrawDataModel.CustomerProductId != null ? new Guid(withdrawDataModel.CustomerProductId) : (Guid?)null,
                    WithdrawAmount = withdrawDataModel.WithdrawAmount,
                    IsEarning = withdrawDataModel.IsEarning,
                    IsSale = withdrawDataModel.IsSale,
                    Status = withdrawDataModel.Status,
                    IsVirtualWallet = withdrawDataModel.IsVirtualWallet,
                    CustomerId = withdrawDataModel.SessionCustomerId != null ? new Guid(withdrawDataModel.SessionCustomerId) : new Guid(withdrawDataModel.CustomerId),
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };
                //add record from in database.
                unitOfWork.WithdrawRepository.Insert<Withdraw>(entity);
                ////save changes in database.
                unitOfWork.WithdrawRepository.Commit();
                response = entity.Id.ToString();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
       /// method is used to get finance history by customerProductId
       /// </summary>
       /// <param name="customerProductId"></param>
       /// <returns></returns>
        public List<Withdraw> GetAllFinanceInvestmentHistory(Guid customerProductId)
        {
            try
            {
                ////return object entity
                return unitOfWork.WithdrawRepository.SearchBy<Withdraw>(x => x.CustomerProductId == customerProductId && x.IsEarning == true).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
       /// method is used to get all withdraws by customerProductId
       /// </summary>
       /// <param name="customerProductId"></param>
       /// <returns></returns>
        public List<Withdraw> GetAllWithdrawSaleByCustomerId(Guid customerProductId)
        {
            try
            {
                ////return object entity
                return unitOfWork.WithdrawRepository.SearchBy<Withdraw>(x => x.CustomerProductId == customerProductId && x.IsSale == true).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
      /// method is used to get all withdraw by customerId
      /// </summary>
      /// <param name="customerId"></param>
      /// <returns></returns>
        public List<Withdraw> GetAllWithdrawByCustomerId(Guid customerId)
        {
            try
            {
                ////return object entity
                return unitOfWork.WithdrawRepository.SearchBy<Withdraw>(x => x.CustomerId == customerId).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
