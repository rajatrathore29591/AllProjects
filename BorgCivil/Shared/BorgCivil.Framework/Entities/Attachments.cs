using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("Attachments")]
    public partial class Attachments
    {
        [Key]
        public Guid AttachmentId { get; set; }

        public Guid? CompanyId { get; set; }

        [StringLength(30)]
        [Column(TypeName = "NVARCHAR")]
        public string AttachmentTitle { get; set; }

        public bool IsAttachment { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual Companies Companies { get; set; }

        public virtual ICollection<FleetsRegistration> FleetsRegistrations { get; set; }

    }
}
