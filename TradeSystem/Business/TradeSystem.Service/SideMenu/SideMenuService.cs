using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;
using TradeSystem.Repositories;

namespace TradeSystem.Service
{
    public class SideMenuService : BaseService, ISideMenuService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public SideMenuService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        /// This method for Get All SideMenu.
        /// </summary>
        /// <returns>SideMenu</returns>
        public List<SideMenu> GetAllSideMenu()
        {
            try
            {
                return unitOfWork.SideMenuRepository.GetAll<SideMenu>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get sidemenu by Id.
        /// </summary>
        /// <param name="id">SideMenu Id</param>
        /// <returns>Object sidemenu entity</returns>
        public SideMenu GetSideMenuById(Guid sideMenuId)
        {
            try
            {
                ////return object entity
                return unitOfWork.SideMenuRepository.FindBy<SideMenu>(sideMenuId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
