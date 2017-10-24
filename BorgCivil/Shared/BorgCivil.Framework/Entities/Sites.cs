using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("Sites")]
    public partial class Sites
    {
        [Key]
        public Guid SiteId { get; set; }

        public Guid CustomerId { get; set; }

        public Guid? DocumentId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string SiteName { get; set; }
        
        [Column(TypeName = "NVARCHAR")]
        public string SiteDetail { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(50)]
        public string PoNumber { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(30)]
        public string CreditTermAgreed { get; set; }

        public decimal DayShiftRate { get; set; }

        public decimal NightShiftRate { get; set; }

        public decimal FloatCharge { get; set; }

        public decimal PilotCharge { get; set; }

        public decimal TipOffRate { get; set; }

        public bool FuelIncluded { get; set; }

        public bool TollTax { get; set; }

        [Column(TypeName = "NVARCHAR")]
        public string Note { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Document Document { get; set; }

        public virtual ICollection<Gate> Gates { get; set; }

        public virtual ICollection<Supervisor> Supervisors { get; set; }

    }
}
