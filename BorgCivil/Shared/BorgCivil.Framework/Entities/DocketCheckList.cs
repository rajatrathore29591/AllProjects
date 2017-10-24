using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("DocketCheckList")]
    public partial class DocketCheckList
    {
        [Key]
        public Guid DocketCheckListId { get; set; }

        [StringLength(200)]
        [Column(TypeName = "NVARCHAR")]
        public string Title { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

    }
}
