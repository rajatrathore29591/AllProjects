using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("FleetType")]
    public partial class FleetType
    {
        [Key]
        public Guid FleetTypeId { get; set; }

        public Guid? CompanyId { get; set; }

        public Guid? DocumentId { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string Fleet { get; set; }

        [Column(TypeName = "NVARCHAR")]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual Company Companies { get; set; }

        public virtual Document Documents { get; set; }

        public virtual ICollection<BookingFleet> BookingFleets { get; set; }

        public virtual ICollection<FleetsRegistration> FleetsRegistrations { get; set; }

        public virtual ICollection<FleetHistory> FleetHistorys { get; set; }

    }
}
