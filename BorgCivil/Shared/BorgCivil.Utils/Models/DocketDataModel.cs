using System;
using System.Collections.Generic;

namespace BorgCivil.Utils.Models
{

    public partial class DocketDataModel
    {

        public Guid DocketId { get; set; }

        public Guid BookingFleetId { get; set; }

        public Guid SupervisorId { get; set; }

        public Guid SiteId { get; set; }

        public Guid FleetRegistrationId { get; set; }

        public string DocumentId { get; set; }

        public string DocketNo { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public int StartKMs { get; set; }

        public int FinishKMsA { get; set; }

        public TimeSpan LunchBreak1From { get; set; }

        public TimeSpan LunchBreak1End { get; set; }

        public TimeSpan LunchBreak2From { get; set; }

        public TimeSpan LunchBreak2End { get; set; }

        public string AttachmentIds { get; set; }

        public string ImageBase64 { get; set; }

        public string DocketCheckListId { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public string EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string DocketDate { get; set; }

        public DateTime EditedDate { get; set; }

        public List<LoadDocketDataModel> LoadDocketDataModel { get; set; }

    }

    public partial class LoadDocketDataModel
    {
        public Guid DocketLoadtId { get; set; }

        public Guid? DocketId { get; set; }

        public string LoadingSite { get; set; }

        public int Weight { get; set; }

        public string LoadTime { get; set; }

        public string TipOffSite { get; set; }

        public string TipOffTime { get; set; }

        public string Material { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }
    }
}
