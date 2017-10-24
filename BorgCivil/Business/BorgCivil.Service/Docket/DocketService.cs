using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BorgCivil.Service
{
    public class DocketService : IDocketService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public DocketService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region DocketServices


        /// <summary>
        /// getting all docket from docket table by BookingFleetId
        /// </summary>
        /// <param name="BookingFleetId"></param>
        /// <returns></returns>
        public List<Docket> GetAllDocketByBookingId(Guid BookingFleetId)
        {
            try
            {
                return unitOfWork.DocketRepository.SearchBy<Docket>(x => x.IsActive == true && x.BookingFleetId == BookingFleetId).ToList();
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
        public Docket GetDocketByDocketId(Guid DocketId)
        {
            try
            {
                //map entity.
                return unitOfWork.DocketRepository.GetById<Docket>(DocketId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to add data in docket table
        /// </summary>
        /// <param name="DocketDataModel"></param>
        /// <returns></returns>
        public string AddDocket(DocketDataModel DocketDataModel)
        {
            try
            {
                // for long date
                DateTimeFormatInfo dtinfoLong = new DateTimeFormatInfo();
                dtinfoLong.ShortDatePattern = "dd/MM/yyyy hh:mm tt";
                dtinfoLong.DateSeparator = "/";

                // created new guid
                var Id = Guid.NewGuid();
                //map entity.
                Docket entity = new Docket()
                {

                    DocketId = Id,
                    BookingFleetId = DocketDataModel.BookingFleetId,
                    SupervisorId = DocketDataModel.SupervisorId,
                    FleetRegistrationId = DocketDataModel.FleetRegistrationId,
                    DocumentId = DocketDataModel.DocumentId != null ? new Guid(DocketDataModel.DocumentId) : (Guid?)null,
                    DocketNo = DocketDataModel.DocketNo,
                    StartTime = DocketDataModel.StartTime,
                    EndTime = DocketDataModel.EndTime,
                    StartKMs = DocketDataModel.StartKMs,
                    FinishKMsA = DocketDataModel.FinishKMsA,
                    LunchBreak1From = DocketDataModel.LunchBreak1From,
                    LunchBreak1End = DocketDataModel.LunchBreak1End,
                    LunchBreak2From = DocketDataModel.LunchBreak2From,
                    LunchBreak2End = DocketDataModel.LunchBreak2End,
                    AttachmentIds = DocketDataModel.AttachmentIds,
                    DocketCheckListId = DocketDataModel.DocketCheckListId,
                    IsActive = DocketDataModel.IsActive,
                    CreatedBy = DocketDataModel.CreatedBy != null ? new Guid(DocketDataModel.CreatedBy) : (Guid?)null,
                    EditedBy = DocketDataModel.EditedBy != null? new Guid(DocketDataModel.EditedBy) : (Guid?)null,
                    DocketDate = Convert.ToDateTime(DocketDataModel.DocketDate, dtinfoLong),
                    CreatedDate = System.DateTime.UtcNow,
                    EditedDate = null
                };
                //add record from in database.
                unitOfWork.DocketRepository.Insert<Docket>(entity);

                ////save changes in database.
                unitOfWork.DocketRepository.Commit();

                return Id.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// method to update the docket table by docketId
        /// </summary>
        /// <param name="DocketDataModel"></param>
        /// <returns></returns>
        public string UpdateDocket(DocketDataModel DocketDataModel)
        {
            try
            {
                // for long date
                DateTimeFormatInfo dtinfoLong = new DateTimeFormatInfo();
                dtinfoLong.ShortDatePattern = "dd/MM/yyyy hh:mm tt";
                dtinfoLong.DateSeparator = "/";

                ////get docket data by docketIdId.
                var DocketDetail = GetDocketByDocketId(DocketDataModel.DocketId);

                ////case of not null return 
                if (DocketDetail != null)
                {
                    ////map entity.
                    DocketDetail.FleetRegistrationId = DocketDataModel.FleetRegistrationId !=null ? DocketDataModel.FleetRegistrationId : (Guid?)null;
                    DocketDetail.SupervisorId = DocketDataModel.SupervisorId;
                    DocketDetail.AttachmentIds = DocketDataModel.AttachmentIds;
                    DocketDetail.DocketCheckListId = DocketDataModel.DocketCheckListId;
                    DocketDetail.DocumentId = DocketDataModel.DocumentId != null ? new Guid(DocketDataModel.DocumentId) : (Guid?)null;
                    DocketDetail.DocketNo = DocketDataModel.DocketNo;
                    DocketDetail.StartTime = DocketDataModel.StartTime;
                    DocketDetail.EndTime = DocketDataModel.EndTime;
                    DocketDetail.StartKMs = DocketDataModel.StartKMs;
                    DocketDetail.FinishKMsA = DocketDataModel.FinishKMsA;
                    DocketDetail.LunchBreak1From = DocketDataModel.LunchBreak1From;
                    DocketDetail.LunchBreak1End = DocketDataModel.LunchBreak1End;
                    DocketDetail.LunchBreak2From = DocketDataModel.LunchBreak2From;
                    DocketDetail.LunchBreak2End = DocketDataModel.LunchBreak2From;
                    DocketDetail.DocketDate = Convert.ToDateTime(DocketDataModel.DocketDate, dtinfoLong);
                    DocketDetail.EditedDate = System.DateTime.UtcNow;

                    ////update record into database entity.
                    unitOfWork.DocketRepository.Update<Docket>(DocketDetail);

                    ////save changes in database.
                    unitOfWork.DocketRepository.Commit();

                    ////case of update.
                    return DocketDetail.DocketId.ToString();
                }
                else
                {
                    ////case of null return null object.
                    return "error";
                }
            }
            catch (Exception ex)
            {
                ////case of error throw
                throw ex;
            }
        }

        /// <summary>
        /// method to update the documentId in docket table by docketId
        /// </summary>
        /// <param name="DocketId"></param>
        /// <param name="DocumentId"></param>
        /// <returns></returns>
        public bool UpdateDocumentId(Guid DocketId, Guid DocumentId)
        {
            try
            {
                ////get docket data by docketIdId.
                var DocketDetail = GetDocketByDocketId(DocketId);

                ////case of not null return 
                if (DocketDetail != null)
                {
                    ////map entity.
                    DocketDetail.DocumentId = DocumentId;
                    DocketDetail.EditedDate = System.DateTime.UtcNow;

                    ////update record into database entity.
                    unitOfWork.DocketRepository.Update<Docket>(DocketDetail);

                    ////save changes in database.
                    unitOfWork.DocketRepository.Commit();

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
