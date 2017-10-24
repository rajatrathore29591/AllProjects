using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BorgCivil.Service
{
    public class LoadDocketService : ILoadDocketService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public LoadDocketService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region LoadDocketServices


        /// <summary>
        /// getting all loaddocket from loaddocket table by DocketId
        /// </summary>
        /// <param name="DocketId"></param>
        /// <returns></returns>
        public List<LoadDocket> GetAllLoadDocketByDocketId(Guid DocketId)
        {
            try
            {
                return unitOfWork.LoadDocketRepository.SearchBy<LoadDocket>(x => x.IsActive == true && x.DocketId == DocketId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting docket detail by docketId
        /// </summary>
        /// <param name="DocketId"></param>
        /// <returns></returns>
        //public Docket GetDocketByDocketId(Guid DocketId)
        //{
        //    try
        //    {
        //        //map entity.
        //        return unitOfWork.DocketRepository.GetById<Docket>(DocketId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// method to add data in loadDocket table
        /// </summary>
        /// <param name="DocketDataModel"></param>
        /// <returns></returns>
        public bool AddLoadDocket(LoadDocketDataModel LoadDocketDataModel)
        {
            try
            {
                DateTime LT= DateTime.ParseExact(LoadDocketDataModel.LoadTime, "h:mm tt", CultureInfo.InvariantCulture);
                DateTime TOT = DateTime.ParseExact(LoadDocketDataModel.TipOffTime, "h:mm tt", CultureInfo.InvariantCulture);
                
                //if you really need a TimeSpan this will get the time elapsed since midnight:
                TimeSpan LoadTime = LT.TimeOfDay;
                TimeSpan TipOffTime = TOT.TimeOfDay;

                // created new guid
                var Id = Guid.NewGuid();
                //map entity.
                LoadDocket entity = new LoadDocket()
                {

                    DocketLoadtId = Id,
                    DocketId = LoadDocketDataModel.DocketId,
                    LoadingSite = LoadDocketDataModel.LoadingSite,
                    Weight = LoadDocketDataModel.Weight,
                    LoadTime = LoadTime,
                    TipOffSite = LoadDocketDataModel.TipOffSite,
                    TipOffTime = TipOffTime,
                    Material = LoadDocketDataModel.Material,
                    IsActive = true,
                    CreatedBy = LoadDocketDataModel.CreatedBy != null ? LoadDocketDataModel.CreatedBy : (Guid?)null,
                    EditedBy = LoadDocketDataModel.EditedBy != null ? LoadDocketDataModel.EditedBy : (Guid?)null,
                    CreatedDate = System.DateTime.UtcNow,
                    EditedDate = null
                };
                //add record from in database.
                unitOfWork.LoadDocketRepository.Insert<LoadDocket>(entity);

                ////save changes in database.
                unitOfWork.LoadDocketRepository.Commit();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// delete load Docket from loadDocket table
        /// </summary>
        /// <param name="DocketId"></param>
        /// <returns></returns>
        public bool DeleteLoadDocket(Guid DocketId)
        {
            try
            {
                ////get load docket data by DocketId.
                var LoadDocket = GetAllLoadDocketByDocketId(DocketId);

                ////case of not null return 
                if (LoadDocket != null)
                {
                    ////update record into database entity.
                    unitOfWork.LoadDocketRepository.Delete<LoadDocket>(LoadDocket);

                    ////save changes in database.
                    unitOfWork.LoadDocketRepository.Commit();

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


        #endregion
    }
}
