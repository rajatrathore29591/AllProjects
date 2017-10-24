using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("Driver")]
    public partial class Driver
    {
        [Key]
        public Guid DriverId { get; set; }

        public Guid? FleetRegistrationId { get; set; }

        public Guid CountryId { get; set; }

        public Guid? EmploymentCategoryId { get; set; }

        public Guid? StatusLookupId { get; set; }

        public Guid? ProfilePic { get; set; }

        public Guid? LicenseClassId { get; set; }

        public string DocumentId { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string LastName { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Email { get; set; }

        [Required]
        [StringLength(12)]
        [Column(TypeName = "NVARCHAR")]
        public string MobileNumber { get; set; }
       
        [Column(TypeName = "NVARCHAR")]
        public string AddressLine1 { get; set; }

        [Column(TypeName = "NVARCHAR")]
        public string AddressLine2 { get; set; }

        [StringLength(30)]
        [Column(TypeName = "NVARCHAR")]
        public string Street { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string Suburb { get; set; }
        
        public Guid? StateId { get; set; }

        [StringLength(10)]
        [Column(TypeName = "NVARCHAR")]
        public string PostCode { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string CardNumber { get; set; }

        [StringLength(300)]
        [Column(TypeName = "NVARCHAR")]
        public string DocumentIds { get; set; }

        [StringLength(30)]
        [Column(TypeName = "NVARCHAR")]
        public string LicenseNumber { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public decimal? BaseRate { get; set; }

        [StringLength(10)]
        [Column(TypeName = "NVARCHAR")]
        public string Shift { get; set; }
       
        [Column(TypeName = "NVARCHAR")]
        public string Awards { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string EmergencyContactName { get; set; }

        [StringLength(12)]
        [Column(TypeName = "NVARCHAR")]
        public string EmergencyContactNumber { get; set; }

        public DateTime? TypeFromDate { get; set; }

        public DateTime? TypeToDate { get; set; }

        public string TypeNote { get; set; }

        public DateTime? LeaveFromDate { get; set; }

        public DateTime? LeaveToDate { get; set; }

        public string LeaveNote { get; set; }

        public int? AnnualLeaveBalance { get; set; }

        public int? SickLeaveBalance { get; set; }

        public string City { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual ICollection<BookingFleet> BookingFleets { get; set; }

        public virtual FleetsRegistration FleetsRegistration { get; set; }

        public virtual Country Country { get; set; }

        public virtual Document Document { get; set; }

        public virtual State State { get; set; }

        public virtual EmploymentCategory EmploymentCategory { get; set; }

        public virtual StatusLookup StatusLookup { get; set; }

        public virtual LicenseClass LicenseClass { get; set; }

        public virtual ICollection<DriversFleetsMapping> DriversFleetsMappings { get; set; }

        public virtual ICollection<AnonymousField> AnonymousFields { get; set; }

        public virtual ICollection<FleetHistory> FleetHistorys { get; set; }

        public virtual ICollection<DriverWhiteCard> DriverWhiteCards { get; set; }

        public virtual ICollection<DriverInductionCard> DriverInductionCards { get; set; }

        public virtual ICollection<DriverVocCard> DriverVocCards { get; set; }
    }
}
