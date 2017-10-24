using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("Attachment")]
    public partial class Attachment
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

        public virtual Company Companies { get; set; }

        public virtual ICollection<FleetsRegistration> FleetsRegistrations { get; set; }

    }
}
