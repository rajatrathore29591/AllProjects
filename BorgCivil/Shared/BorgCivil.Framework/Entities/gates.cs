using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("Gates")]
    public partial class Gates
    {
        [Key]
        public Guid GateId { get; set; }

        [Key]
        public Guid? SiteId { get; set; }

        [StringLength(20)]
        [Column(TypeName = "NVARCHAR")]
        public string GateNumber { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string ContactPerson { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Email { get; set; }

        [StringLength(12)]
        [Column(TypeName = "NVARCHAR")]
        public string MobileNumber { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

    }
}
