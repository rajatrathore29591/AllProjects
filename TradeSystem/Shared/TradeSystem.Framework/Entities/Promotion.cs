using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Framework.Entities
{
    public class Promotion
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Column(TypeName = "NVARCHAR")]
        public string To { get; set; }

        [Required]
        [StringLength(300)]
        [Column(TypeName = "NVARCHAR")]
        public string Subject { get; set; }

        [StringLength(300)]
        [Column(TypeName = "NVARCHAR")]
        public string SubjectSpanish { get; set; }

        [Required]
        public string Description { get; set; }

        public string DescriptionSpanish { get; set; }

        [Required]
        public string PromotionType { get; set; }

        [StringLength(300)]
        [Column(TypeName = "NVARCHAR")]
        public string Url { get; set; }

        public bool Viewed { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
