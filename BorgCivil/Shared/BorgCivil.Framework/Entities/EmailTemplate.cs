using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("EmailTemplate")]
    public partial class EmailTemplate
    {
        public EmailTemplate()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "NVARCHAR")]
        public string Name { get; set; }

        [StringLength(255)]
        [Column(TypeName = "NVARCHAR")]
        public string Subject { get; set; }

        [MaxLength]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Body { get; set; }

        [Required]
        public int Template { get; set; }

        [Required]
        public Guid OrganisationId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

    }
}
