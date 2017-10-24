using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("SentEmail")]
    public partial class SentEmail
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        [Column(TypeName = "NVARCHAR")]
        public string UserId { get; set; }

        [Required]
        public string ToEmail { get; set; }

        [StringLength(255)]
        public string CcEmail { get; set; }

        [StringLength(255)]
        public string BccEmail { get; set; }

        [Required]
        [StringLength(255)]
        public string FromEmail { get; set; }

        [Required]
        [StringLength(255)]
        public string Subject { get; set; }

        public string Message { get; set; }

        public DateTime? DateSent { get; set; }

        public DateTime? DateFailed { get; set; }

        public string FailedReason { get; set; }

        public string ModuleName { get; set; }

        public bool? Archived { get; set; }
       
    }
}
