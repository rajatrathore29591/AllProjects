using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("LoadDocket")]
    public partial class LoadDocket
    {
        [Key]
        public Guid DocketLoadtId { get; set; }

        public Guid? DocketId { get; set; }
        
        public string LoadingSite { get; set; }

        public int Weight { get; set; }

        [Column(TypeName = "TIME")]
        public TimeSpan LoadTime { get; set; }

        public string TipOffSite { get; set; }

        [Column(TypeName = "TIME")]
        public TimeSpan TipOffTime { get; set; }

        [StringLength(200)]
        [Column(TypeName = "NVARCHAR")]
        public string Material { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual Docket Docket { get; set; }

    }
}
