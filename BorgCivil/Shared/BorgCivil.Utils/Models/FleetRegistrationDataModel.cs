using System;

namespace BorgCivil.Utils.Models
{

    public partial class FleetRegistrationDataModel
    {
        public Guid FleetRegistrationId { get; set; }

        public Guid FleetTypeId { get; set; }

        public string AttachmentId { get; set; }

        public string SubcontractorId { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Capacity { get; set; }

        public string Year { get; set; }

        public string RegistrationImageBase64 { get; set; }

        public string ServiceImageBase64 { get; set; }

        public string PlantRiskImageBase64 { get; set; }

        public string NHVRImageBase64 { get; set; }

        public string Registration { get; set; }

        public string BorgCivilPlantNumber { get; set; }

        public string VINNumber { get; set; }

        public string EngineNumber { get; set; }

        public string InsuranceDate { get; set; }

        public string CurrentMeterReading { get; set; }

        public string LastServiceMeterReading { get; set; }

        public string ServiceInterval { get; set; }

        public string HVISType { get; set; }

        public bool IsBooked { get; set; }

        public bool IsUpdated { get; set; }

        public bool IsActive { get; set; }

        public string UnavailableFromDate { get; set; }

        public string UnavailableToDate { get; set; }

        public string UnavailableNote { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

    }
}
