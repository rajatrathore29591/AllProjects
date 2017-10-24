using System;
using System.Collections.Generic;

namespace BorgCivil.Utils.Models
{

    public partial class DriverDataModel
    {

        public Guid DriverId { get; set; }

        public string FleetRegistrationId { get; set; }

        public string CountryId { get; set; }

        public string StatusLookupId { get; set; }

        public string EmploymentCategoryId { get; set; }

        public string ProfilePic { get; set; }

        public string LicenseClassId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }
        public string ImageBase64 { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string Street { get; set; }

        public string Suburb { get; set; }

        public string StateId { get; set; }

        public string PostCode { get; set; }

        public string CardNumber { get; set; }

        public string DocumentIds { get; set; }

        public string LicenseNumber { get; set; }

        public string ExpiryDate { get; set; }

        public decimal? BaseRate { get; set; }

        public string Shift { get; set; }

        public string Awards { get; set; }

        public string TypeFromDate { get; set; }

        public string TypeToDate { get; set; }

        public string TypeNote { get; set; }

        public string LeaveFromDate { get; set; }

        public string LeaveToDate { get; set; }

        public string LeaveNote { get; set; }

        public int? AnnualLeaveBalance { get; set; }

        public int? SickLeaveBalance { get; set; }

        public string City { get; set; }

        public string EmergencyContactName { get; set; }

        public string EmergencyContactNumber { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public List<AnonymousFieldDataModel> AnonymousFieldDataModel { get; set; }

        public List<DriverWhiteCardDataModel> DriverWhiteCardDataModel { get; set; }

        public List<DriverInductionCardDataModel> DriverInductionCardDataModel { get; set; }

        public List<DriverVocCardDataModel> DriverVocCardDataModel { get; set; }

    }

    public partial class DriverWhiteCardDataModel
    {
        public Guid DriverWhiteCardId { get; set; }

        public Guid DriverId { get; set; }

        public string CardNumber { get; set; }

        public DateTime IssueDate { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public string EditedBy { get; set; }
    }

    public partial class DriverInductionCardDataModel
    {
        public Guid DriverInductionCardId { get; set; }

        public Guid DriverId { get; set; }

        public string CardNumber { get; set; }

        public string SiteCost { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public string EditedBy { get; set; }
    }

    public partial class DriverVocCardDataModel
    {
        public Guid DriverVocCardId { get; set; }

        public Guid DriverId { get; set; }

        public string CardNumber { get; set; }

        public string RTONumber { get; set; }

        public DateTime IssueDate { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public string EditedBy { get; set; }
    }

    public partial class AnonymousFieldDataModel
    {
        public Guid AnonymousFieldId { get; set; }

        public Guid DriverId { get; set; }

        public string Title { get; set; }

        public string Other1 { get; set; }

        public string Other2 { get; set; }

        public string IssueDate { get; set; }

        public string ExpiryDate { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public string EditedBy { get; set; }
    }

}
