using System;
using System.Collections.Generic;
using System.Linq;
using TradeSystem.Framework.Entities;
using TradeSystem.Framework.Identity;
using TradeSystem.Repositories;

namespace TradeSystem.Service
{
    public class RoleService : BaseService, IRoleService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public RoleService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        /// <summary>
        /// This method use for get roles by roleId
        /// </summary>
        /// <returns>role detail</returns>
        public ApplicationRole GetRoleById(Guid roleId)
        {
            try
            {
                //return entity object as per result.
                return unitOfWork.RoleRepository.GetRoleById(roleId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method is used to get detail by roleId
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool GetRoleCompanyUserByRoleId(Guid roleId)
        {
            try
            {
                //return entity object as per result.
                var cmuser = unitOfWork.RoleRepository.GetRoleCompanyUserByRoleId(roleId);
                if (cmuser.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method for use role update role info.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        /// <returns>result</returns>
        public bool EditRole(Guid id, string Name, string Description)
        {
            try
            {
                ////get existing record.
                ApplicationRole entity = GetRoleById(id);

                ////map entity
                entity.Name = Name;
                entity.Description = Description;

                ////update record from existing entity in database.
                unitOfWork.RoleRepository.Update<ApplicationRole>(entity);
                ////save changes in database.
                unitOfWork.RoleRepository.Commit();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
