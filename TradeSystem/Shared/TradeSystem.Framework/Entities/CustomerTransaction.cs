using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeSystem.Framework.Entities
{
    [Table("CustomerTransaction")]
    public partial class CustomerTransaction
    {
        public CustomerTransaction()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        public Guid CustomerProductId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string PaymentMode { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string PaymentType { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string TransactionFor { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Amount { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual CustomerProduct CustomerProduct { get; set; }

        
    }
}
