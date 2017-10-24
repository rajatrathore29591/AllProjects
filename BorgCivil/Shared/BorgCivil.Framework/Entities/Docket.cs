using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("Docket")]
    public partial class Docket
    {
        [Key]
        public Guid DocketId { get; set; }

        public Guid? BookingFleetId { get; set; }

        public Guid? FleetRegistrationId { get; set; }

        public Guid? SupervisorId { get; set; }

        public Guid? DocumentId { get; set; }

        [Required]
        [StringLength(30)]
        [Column(TypeName = "NVARCHAR")]
        public string DocketNo { get; set; }

        [Column(TypeName = "TIME")]
        public TimeSpan StartTime { get; set; }

        [Column(TypeName = "TIME")]
        public TimeSpan EndTime { get; set; }

        public int? StartKMs { get; set; }

        public int? FinishKMsA { get; set; }

        [Column(TypeName = "TIME")]
        public TimeSpan LunchBreak1From { get; set; }

        [Column(TypeName = "TIME")]
        public TimeSpan LunchBreak1End { get; set; }

        [Column(TypeName = "TIME")]
        public TimeSpan LunchBreak2From { get; set; }

        [Column(TypeName = "TIME")]
        public TimeSpan LunchBreak2End { get; set; }

        public string AttachmentIds { get; set; }

        public string DocketCheckListId { get; set; }

        public DateTime DocketDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual BookingFleet BookingFleet { get; set; }

        public virtual Supervisor Supervisor { get; set; }

        public virtual FleetsRegistration FleetsRegistration { get; set; }

        public virtual Document Document { get; set; }

        public virtual ICollection<LoadDocket> LoadDockets { get; set; }

    }
}
