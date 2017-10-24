using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("FleetsRegistration")]
    public partial class FleetsRegistration
    {
        [Key]
        public Guid FleetRegistrationId { get; set; }

        public Guid FleetTypeId { get; set; }

        public Guid? AttachmentId { get; set; }

        public Guid? SubcontractorId { get; set; }

        public string DocumentId { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string Make { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string Model { get; set; }

        [StringLength(10)]
        [Column(TypeName = "NVARCHAR")]
        public string Capacity { get; set; }

        [StringLength(5)]
        [Column(TypeName = "NVARCHAR")]
        public string Year { get; set; }

        [StringLength(10)]
        [Column(TypeName = "NVARCHAR")]
        public string Registration { get; set; }

        [StringLength(10)]
        [Column(TypeName = "NVARCHAR")]
        public string BorgCivilPlantNumber { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string VINNumber { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string EngineNumber { get; set; }

        public DateTime? InsuranceDate { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string CurrentMeterReading { get; set; }

        [StringLength(30)]
        [Column(TypeName = "NVARCHAR")]
        public string LastServiceMeterReading { get; set; }

        [StringLength(30)]
        [Column(TypeName = "NVARCHAR")]
        public string ServiceInterval { get; set; }

        [StringLength(30)]
        [Column(TypeName = "NVARCHAR")]
        public string HVISType { get; set; }

        [Required]
        public bool IsBooked { get; set; }

        public bool IsUpdated { get; set; }

        public DateTime? UnavailableFromDate { get; set; }

        public DateTime? UnavailableToDate { get; set; }

        public string UnavailableNote { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual ICollection<BookingFleet> BookingFleets { get; set; }

        public virtual FleetType FleetType { get; set; }

        public virtual Subcontractor Subcontractor { get; set; }

        public virtual Attachment Attachment { get; set; }

        public virtual ICollection<Driver> Drivers { get; set; }

        public virtual ICollection<DriversFleetsMapping> DriversFleetsMappings { get; set; }

        public virtual ICollection<BookingSiteGate> BookingSiteGates { get; set; }

        public virtual ICollection<FleetHistory> FleetHistorys { get; set; }

        public virtual ICollection<Docket> Dockets { get; set; }
    }

}
