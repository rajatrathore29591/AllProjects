using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeSystem.Framework.Entities
{
    [Table("Penalty")]
    public partial class Penalty
    {
        public Penalty()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string From { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string To { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string PenaltyPercent { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual Product Product { get; set; }

    }
}
