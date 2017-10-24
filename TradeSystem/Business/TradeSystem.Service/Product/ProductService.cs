using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TradeSystem.Framework.Entities;
using TradeSystem.Repositories;
using TradeSystem.Utils.Models;

namespace TradeSystem.Service
{
    public class ProductService : BaseService, IProductService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;
       
        //Initialized Parameterized Constructor.
        public ProductService(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            
        }

        #endregion
       
        /// This method for Get All Product.
        /// </summary>
        /// <returns>Products</returns>
        public List<Product> GetAllProduct()
        {
            try
            {
                return unitOfWork.ProductRepository.GetAll<Product>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        /// <summary>
        /// get all product by productId.
        /// </summary>
        /// <param name="id">product Id</param>
        /// <returns>Object product entity</returns>
        public Product GetProductByProductId(Guid productId)
        {
            try
            {
                return unitOfWork.ProductRepository.FindBy<Product>(x => x.Id == productId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// This method for Get All Products of Customer.
        /// </summary>
        /// <returns>Products</returns>
        public List<Product> GetAllCustomerProduct(string customerId)
        {
            try
            {
                //var productIds = customerProductService.GetAllCustomerProductByCustomerId(new Guid(customerId));
                var productIds = unitOfWork.CustomerProductRepository.SearchBy<CustomerProduct>(x => x.CustomerId == new Guid(customerId)).ToList();
                var product = unitOfWork.ProductRepository.SearchBy<Product>(y => y.IsActive == true).Join(productIds, p => p.Id, c => c.ProductId, (p, c) => new { p, c }).Select(pro => pro.p).ToList();
                return product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method use for Insert record into companyUser entity.
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="Description">Description</param>
        /// <param name="PercentWeeklyEarning">percentWeeklyEarning</param>
        /// <param name="PercentWeeklySale">PercentWeeklySale</param>
        /// <param name="WeeklyFromWithdrawDay">WeeklyFromWithdrawDay</param>
        /// <param name="WeeklyToWithdrawDay">WeeklyToWithdrawDay</param>
        /// <param name="SaleWithdrawDay">SaleWithdrawDay</param>
        /// <param name="InvestmentWithdrawDate">InvestmentWithdrawDate</param>
        /// <param name="Condition1">Condition1</param>
        /// <param name="Condition2">Condition2</param>
        /// <param name="Condition3">Condition3</param>
        /// <param name="MinPrice">MinPrice</param>
        /// <param name="MaxPrice">MaxPrice</param>
        /// <param name="TotalValueOfInvestment">TotalValueOfInvestment</param>
        /// <param name="RemainingValueOfInvestment">RemainingValueOfInvestment</param>
        /// <param name="ByCountry">ByCountry</param>
        /// <param name="ByState">ByState</param>
        /// <param name="ByFromAmount">ByFromAmount</param>
        /// <param name="ByToAmount">ByToAmount</param>
        /// <param name="ByFromSale">ByFromSale</param>
        /// <param name="ByToSale">ByToSale</param>
        /// <returns>result</returns>
        public string AddProduct(ProductDataModel productDataModel)
        {
            try
            {
                DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                dtinfo.ShortDatePattern = "dd/MM/yyyy";
                dtinfo.DateSeparator = "/";                
               
                var response = string.Empty;
                //map entity.
                Product entity = new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = productDataModel.Name,
                    Description = productDataModel.Description,
                    PercentWeeklyEarning = productDataModel.PercentWeeklyEarning,
                    PercentSaleEarning = productDataModel.PercentSaleEarning,
                    WeeklyFromWithdrawDay = productDataModel.WeeklyFromWithdrawDay,
                    WeeklyToWithdrawDay = productDataModel.WeeklyToWithdrawDay,
                    SaleWithdrawDay = productDataModel.SaleWithdrawDay,
                    InvestmentWithdrawDate = Convert.ToDateTime(productDataModel.InvestmentWithdrawDate,dtinfo),
                    Condition1 = productDataModel.Condition1 != null ? productDataModel.Condition1 : "0",
                    Condition2 = productDataModel.Condition2 != null ? productDataModel.Condition2 : "0",
                    Condition3 = productDataModel.Condition3 != null ? productDataModel.Condition3 : "0",
                    MinPrice = productDataModel.MinPrice,
                    MaxPrice = productDataModel.MaxPrice,
                    TotalValueOfInvestment = productDataModel.TotalValueOfInvestment,
                    RemainingValueOfInvestment = productDataModel.TotalValueOfInvestment,
                    ByCountry = productDataModel.ByCountry,
                    ByState = productDataModel.ByState,
                    ByFromAmount = productDataModel.ByFromAmount,
                    ByToAmount = productDataModel.ByToAmount,
                    ByFromSale = productDataModel.ByFromSale,
                    ByToSale = productDataModel.ByToSale,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IsActive = true
                    
                };
                //add record from in database.
                unitOfWork.ProductRepository.Insert<Product>(entity);
                ////save changes in database.
                unitOfWork.ProductRepository.Commit();
                response = entity.Id.ToString();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method for Get Product for select list type controls [Text, Value] pair.
        /// </summary>
        /// <returns>List of Select Value from TaskType entity.</returns>
        public List<SelectListModel> GetProductSelectList()
        {
            try
            {
                ////get all active records
                var types = unitOfWork.ProductRepository.GetAll<Product>().ToList();

                ////check types is null or empity
                if (types != null)
                {
                    ////map entity to model.
                    return types.Select(item => new SelectListModel
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
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

        /// <summary>
        ///  This method use for update customer logo.
        /// </summary>
        /// <returns>result true/false</returns>
        public bool UpdateLogo(Guid id, Guid? documentId)
        {
            try
            {
                ////get product data by id.
                var Product = GetProductByProductId(id);

                ////case of not null return Product data object.
                if (Product != null)
                {
                    if (documentId != Guid.Empty && documentId != null)
                    {
                        ////map entity.
                        Product.DocumentId = documentId;

                        ////update record into database entity.
                        unitOfWork.ProductRepository.Update<Product>(Product);

                        ////save changes in database.
                        unitOfWork.ProductRepository.Commit();
                    }

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
        /// updating InvestmentAmount in Product table
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="remainingAmount"></param>
        /// <returns></returns>
        public bool UpdateInvestmentAmount(Guid productId, float remainingAmount)
        {
            try
            {
                //get existing record.
                Product entity = GetProductByProductId(productId);

                
                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.RemainingValueOfInvestment = remainingAmount;
                    entity.ModifiedDate = DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.ProductRepository.Update<Product>(entity);
                    ////save changes in database.
                    unitOfWork.ProductRepository.Commit();

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
