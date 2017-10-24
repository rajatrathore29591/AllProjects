using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeSystem.Framework.Entities
{
    [Table("Withdraw")]
    public partial class Withdraw
    {
        public Withdraw()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }
        [Key]
        [Required]
        public Guid Id { get; set; }
        public Guid? CustomerProductId { get; set; }
        public Guid? CustomerId { get; set; }
        [Required]
        public float WithdrawAmount { get; set; }
        public bool IsEarning { get; set; }
        public bool IsSale { get; set; }
        public bool IsVirtualWallet { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public virtual CustomerProduct CustomerProduct { get; set; }
    }
}
