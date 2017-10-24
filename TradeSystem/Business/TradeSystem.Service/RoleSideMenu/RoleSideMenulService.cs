using System;
using System.Collections.Generic;
using TradeSystem.Framework.Entities;
using TradeSystem.Repositories;

namespace TradeSystem.Service
{
    public class RoleSideMenuService : BaseService, IRoleSideMenuService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public RoleSideMenuService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        /// <summary>
        /// This method use for get roles side menu by roleId
        /// </summary>
        /// <returns>role detail</returns>
        public List<RoleSideMenu> GetRoleSideMenuByRoleId(Guid roleId)
        {
            try
            {
                //return entity object as per result.
                return unitOfWork.RoleSideMenuRepository.GetRoleSideMenuListByRoleId(roleId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method use for Insert record into rolesidemenu entity.
        /// </summary>
        /// <param name="sideMenuId">sideMenuId</param>
        /// <param name="roleId">roleId</param>
        /// <returns>result</returns>
        public bool AddRoleSideMenu(Guid sideMenuId, string roleId)
        {
            try
            {
                //map entity.
                RoleSideMenu entity = new RoleSideMenu()
                {
                    Id = Guid.NewGuid(),
                    SideMenuId = sideMenuId,
                    RoleId = roleId,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow

                };
                //add record from in database.
                unitOfWork.RoleSideMenuRepository.Insert<RoleSideMenu>(entity);
                ////save changes in database.
                unitOfWork.RoleSideMenuRepository.Commit();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  This method use for delete record into rolesidemenu entity.
        /// </summary>
        /// <param name="id">role Id</param>
        /// <returns>result</returns>
        public bool DeleteRoleSideMenu(Guid roleId)
        {
            try
            {
                //get existing record.
                List<RoleSideMenu> entity = GetRoleSideMenuByRoleId(roleId);
                //check entity is null.
                if (entity != null)
                {
                    //delete record from existing entity in database.
                    unitOfWork.RoleSideMenuRepository.Delete<RoleSideMenu>(entity);
                    ////save changes in database.
                    unitOfWork.RoleSideMenuRepository.Commit();

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
