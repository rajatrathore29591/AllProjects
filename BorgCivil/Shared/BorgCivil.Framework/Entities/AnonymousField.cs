using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("AnonymousField")]
    public partial class AnonymousField
    {
        [Key]
        public Guid AnonymousFieldId { get; set; }

        public Guid? DriverId { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string Title { get; set; }

        [StringLength(150)]
        [Column(TypeName = "NVARCHAR")]
        public string Other1 { get; set; }

        [StringLength(150)]
        [Column(TypeName = "NVARCHAR")]
        public string Other2 { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string Notes { get; set; }

        public bool? IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual Driver Driver { get; set; }

    }
}
