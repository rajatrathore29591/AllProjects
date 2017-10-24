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
    public class CustomerProductService : ICustomerProductService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public CustomerProductService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        /// <summary>
        /// get all customer by customerId.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public List<CustomerProduct> GetAllCustomerProductByCustomerId(Guid customerId)
        {
            try
            {
                return unitOfWork.CustomerProductRepository.SearchBy<CustomerProduct>(x => x.CustomerId == customerId).OrderByDescending(x => x.ModifiedDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// This method for Get All Customer Product.
        /// </summary>
        /// <returns>Customer</returns>
        public List<CustomerProduct> GetAllCustomerProduct()
        {
            try
            {
                return unitOfWork.CustomerProductRepository.GetAll<CustomerProduct>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get all customer by productId.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<CustomerProduct> GetAllCustomerProductByProductId(Guid productId)
        {
            try
            {
                return unitOfWork.CustomerProductRepository.SearchBy<CustomerProduct>(x => x.ProductId == productId && x.Status == "Active").ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method is used to get detail from customerProduct table by CustomerId and ProductId
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public CustomerProduct GetAllCustomerProductByProductIdCustomerId(Guid customerId, Guid productId)
        {
            try
            {
                return unitOfWork.CustomerProductRepository.FindBy<CustomerProduct>(x => x.CustomerId == customerId && x.ProductId == productId && x.Status != "Reject");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method use for Insert record into CustomerProduct entity.
        /// </summary>
        /// <param name="CustomerId">CustomerId</param>
        /// <param name="ProductId">ProductId</param>
        /// <param name="Investment">Investment</param>
        /// <param name="PaymentStatus">PaymentStatus</param>
        /// <returns>result</returns>
        public bool InvestInProduct(CustomerProductDataModel customerProductData)
        {
            try
            {
                //map entity.
                CustomerProduct entity = new CustomerProduct()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = new Guid(customerProductData.CustomerId),
                    ProductId = new Guid(customerProductData.ProductId),
                    Investment = (float)(Convert.ToDecimal(customerProductData.Investment)),
                    WeeklyEarning = 0.0f,
                    Status = customerProductData.Status,
                    StopCalculation = false,
                    SaleEarning = 0.0f,
                    PaymentStatus = Convert.ToBoolean(customerProductData.PaymentStatus),
                    PaymentType = customerProductData.PaymentType,
                    WalletAmount = (float)(Convert.ToDecimal(customerProductData.WalletAmount)),
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    StartCalculationDate = customerProductData.PaymentType == "CreditCard" ? DateTime.UtcNow : (DateTime?)null

                };
                //add record from in database.
                unitOfWork.CustomerProductRepository.Insert<CustomerProduct>(entity);
                ////save changes in database.
                unitOfWork.CustomerProductRepository.Commit();
                // response = entity.Id.ToString();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }

        /// <summary>
        /// method used to update weekly amount 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="productId"></param>
        /// <param name="weeklyAmount"></param>
        /// <returns></returns>
        public bool UpdateWeeklyEarning(Guid customerId, Guid productId, float weeklyAmount)
        {
            try
            {
                //get existing record.
                CustomerProduct entity = GetAllCustomerProductByProductIdCustomerId(customerId, productId);

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.WeeklyEarning = weeklyAmount;
                    entity.ModifiedDate = DateTime.UtcNow;
                    if (entity.StartCalculationDate == null)
                    {
                        entity.StartCalculationDate = entity.CreatedDate.AddDays(1);
                        entity.LastWeeklyWithdrawDate = entity.CreatedDate.AddDays(1);
                    }

                    //update record from existing entity in database.
                    unitOfWork.CustomerProductRepository.Update<CustomerProduct>(entity);
                    ////save changes in database.
                    unitOfWork.CustomerProductRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method is used to update sale amount
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="productId"></param>
        /// <param name="saleEarning"></param>
        /// <returns></returns>
        public bool UpdateSaleEarning(Guid customerId, Guid productId, float saleEarning)
        {
            try
            {
                //get existing record.
                CustomerProduct entity = GetAllCustomerProductByProductIdCustomerId(customerId, productId);

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.SaleEarning = saleEarning;
                    entity.ModifiedDate = DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.CustomerProductRepository.Update<CustomerProduct>(entity);
                    ////save changes in database.
                    unitOfWork.CustomerProductRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       /// <summary>
       /// method is used to update investmement by customerId and productId
       /// </summary>
       /// <param name="customerId"></param>
       /// <param name="productId"></param>
       /// <param name="investment"></param>
       /// <returns></returns>
        public bool UpdateInvestment(Guid customerId, Guid productId, float investment)
        {
            try
            {
                //get existing record.
                CustomerProduct entity = GetAllCustomerProductByProductIdCustomerId(customerId, productId);

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.Investment = investment;
                    entity.ModifiedDate = DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.CustomerProductRepository.Update<CustomerProduct>(entity);
                    ////save changes in database.
                    unitOfWork.CustomerProductRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       /// <summary>
       /// method is used to update stopCalculation status and date
       /// </summary>
       /// <param name="customerId"></param>
       /// <param name="productId"></param>
       /// <param name="stopCalculation"></param>
       /// <param name="status"></param>
       /// <returns></returns>
        public bool StopCalculation(Guid customerId, Guid productId, bool stopCalculation, bool status)
        {
            try
            {
                //get existing record.
                CustomerProduct entity = GetAllCustomerProductByProductIdCustomerId(customerId, productId);

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.StopCalculation = stopCalculation;
                    entity.StopCalculationDate = DateTime.UtcNow;
                    if (status == false)
                    {
                        entity.Status = "InActive";
                    }
                    //update record from existing entity in database.
                    unitOfWork.CustomerProductRepository.Update<CustomerProduct>(entity);
                    ////save changes in database.
                    unitOfWork.CustomerProductRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method is used update customer product status
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="productId"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public bool UpdateCustomerProductStatus(Guid customerId, Guid productId, string Status)
        {
            try
            {
                //get existing record.
                CustomerProduct entity = GetAllCustomerProductByProductIdCustomerId(customerId, productId);

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.Status = Status;
                    if (Status != "Reject")
                    {
                        entity.PaymentStatus = true;
                    }
                    entity.ModifiedDate = DateTime.UtcNow;
                    entity.InvestmentWithdrawDate = DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.CustomerProductRepository.Update<CustomerProduct>(entity);
                    ////save changes in database.
                    unitOfWork.CustomerProductRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method is used to update SaleEarningStatus
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="productId"></param>
        /// <param name="Status"></param>
        /// <param name="withdrawStatus"></param>
        /// <returns></returns>
        public bool UpdateSaleEarningStatus(Guid customerId, Guid productId, string Status, bool withdrawStatus)
        {
            try
            {
                //get existing record.
                CustomerProduct entity = GetAllCustomerProductByProductIdCustomerId(customerId, productId);

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.SaleEarningStatus = Status;
                    entity.WithdrawStatus = withdrawStatus;
                    entity.ModifiedDate = DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.CustomerProductRepository.Update<CustomerProduct>(entity);

                    ////save changes in database.
                    unitOfWork.CustomerProductRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method use for updating customer password into customer entity.
        /// </summary>
        /// <param name="barcode">firstName</param>
        /// <param name="barcodeurl">lastName</param>        
        /// <returns>result</returns>
        public bool UpdateCustomerProductWithBarCode(CustomerProductDataModel customerProductDataDataModel)
        {
            try
            {
                //get existing record.
                CustomerProduct entity = unitOfWork.CustomerProductRepository.FindBy<CustomerProduct>(x => x.CustomerId == new Guid(customerProductDataDataModel.CustomerId) && x.ProductId == new Guid(customerProductDataDataModel.ProductId) && x.Status != "Reject");

                //check entity is null.
                if (entity != null)
                {
                    //map entity                   
                    entity.BarCodeUrl = customerProductDataDataModel.BarCodeUrl;
                    entity.BarCode = customerProductDataDataModel.BarCode;
                    entity.ModifiedDate = DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.CustomerProductRepository.Update<CustomerProduct>(entity);

                    ////save changes in database.
                    unitOfWork.CustomerProductRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method is used to update enable date of weekly earning
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="productId"></param>
        /// <param name="weeklyDate"></param>
        /// <returns></returns>
        public bool UpdateWeeklyEarningEnableDate(Guid customerId, Guid productId, DateTime weeklyDate)
        {
            try
            {
                //get existing record.
                CustomerProduct entity = GetAllCustomerProductByProductIdCustomerId(customerId, productId);

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.LastWeeklyWithdrawEnableDate = weeklyDate;

                    //update record from existing entity in database.
                    unitOfWork.CustomerProductRepository.Update<CustomerProduct>(entity);

                    ////save changes in database.
                    unitOfWork.CustomerProductRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method is used to Delete CustomerProduct By CustomerId and ProductId
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool DeleteCustomerProductByCustomerIdProductId(Guid customerId, Guid productId)
        {
            try
            {
                //get existing record.
                CustomerProduct entity = GetAllCustomerProductByProductIdCustomerId(customerId, productId);

                //check entity is null.
                if (entity != null)
                {
                    unitOfWork.CustomerProductRepository.Delete<CustomerProduct>(entity);
                    unitOfWork.CustomerProductRepository.Commit();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// method is used to get detail from customerProduct table by CustomerProductId
        /// </summary>
        /// <param name="customerProductId"></param>
        /// <returns></returns>
        public CustomerProduct GetAllCustomerProductByCustomerProductId(Guid customerProductId)
        {
            try
            {
                return unitOfWork.CustomerProductRepository.FindBy<CustomerProduct>(x => x.Id == customerProductId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method is used to update last weekly enable date 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="productId"></param>
        /// <param name="lastWeeklyEnableDate"></param>
        /// <returns></returns>
        public bool UpdateLastWeeklyEnableDate(Guid customerId, Guid productId, DateTime lastWeeklyEnableDate)
        {
            try
            {
                //get existing record.
                CustomerProduct entity = GetAllCustomerProductByProductIdCustomerId(customerId, productId);

                var CurrentDate = System.DateTime.UtcNow;
                var month = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
                var first = month.AddMonths(-1);
                var last = month.AddDays(-1);
                var Date = CurrentDate.Year + "-" + first.Month + "-" + last.Day;

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.LastWeeklyWithdrawEnableDate = lastWeeklyEnableDate;
                    entity.ModifiedDate = System.DateTime.UtcNow;
                    entity.LastWithdrawMonthDate = Convert.ToDateTime(Date);

                    //update record from existing entity in database.
                    unitOfWork.CustomerProductRepository.Update<CustomerProduct>(entity);

                    ////save changes in database.
                    unitOfWork.CustomerProductRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// updating last weekly enable date 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="productId"></param>
        /// <param name="lastWeeklyDate"></param>
        /// <returns></returns>
        public bool UpdateLastWeeklyDate(Guid customerId, Guid productId, DateTime lastWeeklyDate)
        {
            try
            {
                //get existing record.
                CustomerProduct entity = GetAllCustomerProductByProductIdCustomerId(customerId, productId);

                //check entity is null.
                if (entity != null)
                {
                    //map entity  
                    entity.LastWeeklyWithdrawDate = lastWeeklyDate;
                    entity.ModifiedDate = System.DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.CustomerProductRepository.Update<CustomerProduct>(entity);

                    ////save changes in database.
                    unitOfWork.CustomerProductRepository.Commit();

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
