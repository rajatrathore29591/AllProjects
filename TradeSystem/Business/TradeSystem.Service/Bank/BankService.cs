using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;
using TradeSystem.Repositories;
using TradeSystem.Service;

namespace TradeSystem.Service
{
    public class BankService : BaseService, IBankService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public BankService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        /// <summary>
        /// method to add bank detail by bankId
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public Bank GetBankByBankId(Guid bankId)
        {
            try
            {
                //map entity.
                return unitOfWork.BankRepository.GetById<Bank>(bankId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method for Get TaskType for select list type controls [Text, Value] pair.
        /// </summary>
        /// <returns>List of Select Value from TaskType entity.</returns>
        public List<SelectListModel> GetBankList()
        {
            try
            {
                ////get all active records
                var types = unitOfWork.BankRepository.GetAll<Bank>().ToList();

                ////check types is null or empity
                if (types != null)
                {
                    ////map entity to model.
                    return types.Select(item => new SelectListModel
                    {
                        Text = item.BankName,
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
    }
}
