using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("Supervisor")]
    public partial class Supervisor
    {
        [Key]
        public Guid SupervisorId { get; set; }

        public Guid SiteId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string SupervisorName { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Email { get; set; }

        [StringLength(12)]
        [Column(TypeName = "NVARCHAR")]
        public string MobileNumber { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual Site Site { get; set; }

        public virtual ICollection<BookingSiteSupervisor> BookingSiteSupervisors { get; set; }

        public virtual ICollection<Docket> Dockets { get; set; }
    }
}
