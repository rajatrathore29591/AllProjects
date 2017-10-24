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
    public class WalletService : BaseService, IWalletService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public WalletService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        /// <summary>
        /// get all Wallet by customertId.
        /// </summary>
        /// <param name="id">customer Id</param>
        /// <returns>Object Wallet entity</returns>
        public Wallet GetWalletByCustomerId(Guid customertId)
        {
            try
            {
                return unitOfWork.WalletRepository.FindBy<Wallet>(x => x.CustomerId == customertId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      /// <summary>
      /// method is used to add amount in wallet
      /// </summary>
      /// <param name="withdrawDataModel"></param>
      /// <returns></returns>
        public bool AddVirtualAmount(WithdrawDataModel withdrawDataModel)
        {
            try
            {               
                var response = string.Empty;
                //map entity.
                Wallet entity = new Wallet()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = new Guid(withdrawDataModel.CustomerId),
                    AvailableBalance = Convert.ToDecimal(withdrawDataModel.WithdrawAmount)
                };
                //add record from in database.
                unitOfWork.WalletRepository.Insert<Wallet>(entity);
                ////save changes in database.
                unitOfWork.WalletRepository.Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       /// <summary>
       /// method is used to create customer wallet
       /// </summary>
       /// <param name="WithdrawTableDataModel"></param>
       /// <returns></returns>
        public bool AddCustomerWalletWithZero(WithdrawTableDataModel WithdrawTableDataModel)
        {
            try
            {
                var response = string.Empty;
                //map entity.
                Wallet entity = new Wallet()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = new Guid(WithdrawTableDataModel.CustomerId),
                    AvailableBalance = Convert.ToDecimal(WithdrawTableDataModel.AvailableBalance)
                };
                //add record from in database.
                unitOfWork.WalletRepository.Insert<Wallet>(entity);
                ////save changes in database.
                unitOfWork.WalletRepository.Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method is used to update wallet amount
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="virtualAmount"></param>
        /// <returns></returns>
        public bool UpdateWalletAmount(Guid customerId, float virtualAmount)
        {
            try
            {
                //get existing record.
                Wallet entity = GetWalletByCustomerId(customerId);

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.AvailableBalance = Convert.ToDecimal(virtualAmount);
                    //entity.ModifiedDate = DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.WalletRepository.Update<Wallet>(entity);
                    ////save changes in database.
                    unitOfWork.WalletRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
