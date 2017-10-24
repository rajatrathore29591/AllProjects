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
    public class PromotionService : BaseService, IPromotionService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public PromotionService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        /// <summary>
        /// get all Promotion by Type
        /// </summary>
        /// <returns>Object Promotion entity</returns>
        public List<Promotion> GetAllPromotionByType()
        {
            try
            {
                return unitOfWork.PromotionRepository.SearchBy<Promotion>(x => x.PromotionType == "Alert on Web & App" && x.To == null).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get all Promotion by customertId.
        /// </summary>
        /// <param name="id">customer Id</param>
        /// <returns>Object Promotion entity</returns>
        public Promotion GetPromotionByPromotionId(Guid promotionId)
        {
            try
            {
                return unitOfWork.PromotionRepository.FindBy<Promotion>(x => x.Id == promotionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method is used to add promtion
        /// </summary>
        /// <param name="promotionDataModel"></param>
        /// <returns></returns>
        public bool AddPromotion(PromotionDataModel promotionDataModel)
        {
            try
            {
                //map entity.
                Promotion entity = new Promotion()
                {
                    Id = Guid.NewGuid(),
                    PromotionType = promotionDataModel.PromotionType,
                    Subject = promotionDataModel.Subject,
                    SubjectSpanish = promotionDataModel.SubjectSpanish,
                    To = promotionDataModel.To,
                    Description = promotionDataModel.Description,
                    DescriptionSpanish = promotionDataModel.DescriptionSpanish,
                    Url = promotionDataModel.Url,
                    Viewed = false,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };
                //add record from in database.
                unitOfWork.PromotionRepository.Insert<Promotion>(entity);
                ////save changes in database.
                unitOfWork.PromotionRepository.Commit();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method for Get Customer for select list type controls [Text, Value] pair.
        /// </summary>
        /// <returns>List of Select Value from TaskType entity.</returns>
        public List<SelectListModel> GetCustomerSelectList()
        {
            try
            {
                ////get all active records
                var types = unitOfWork.CustomerRepository.GetAll<Customer>().ToList();

                ////check types is null or empity
                if (types != null)
                {
                    ////map entity to model.
                    return types.Select(item => new SelectListModel
                    {
                        Text = item.Email,
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

        /// This method for Get All Customer.
        /// </summary>
        /// <returns>Customer</returns>
        public List<Customer> GetAllCustomerByActive()
        {
            try
            {
                return unitOfWork.CustomerRepository.SearchBy<Customer>(x => x.IsActive == true).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// This method for Get All Promotion By CustomerId.
        /// </summary>
        /// <returns>Customer</returns>
        public List<Promotion> GetAllPromotionByCustomerId(string customerId)
        {
            try
            {
                return unitOfWork.PromotionRepository.SearchBy<Promotion>(x => x.To == customerId).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// This method for Get All Promotion By CustomerId with unview.
        /// </summary>
        /// <returns>Customer</returns>
        public List<Promotion> GetAllPromotionByCustomerIdAndUnView(string customerId)
        {
            try
            {
                return unitOfWork.PromotionRepository.SearchBy<Promotion>(x => x.To == customerId && x.Viewed == false).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method is used to update promotion viewed status 
        /// </summary>
        /// <param name="promotionId"></param>
        /// <param name="viewed"></param>
        /// <returns></returns>
        public bool UpdatePromotionViewed(Guid promotionId, bool viewed)
        {
            try
            {
                //get existing record.
                Promotion entity = GetPromotionByPromotionId(promotionId);

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.Viewed = viewed;
                    //entity.ModifiedDate = DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.PromotionRepository.Update<Promotion>(entity);
                    ////save changes in database.
                    unitOfWork.PromotionRepository.Commit();

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
