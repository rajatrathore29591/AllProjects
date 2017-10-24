using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeSystem.Framework.Entities
{
    [Table("Wallet")]
    public partial class Wallet 
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        public Guid? CustomerId { get; set; }

        public decimal AvailableBalance { get; set; }

        public virtual Customer Customer { get; set; }

    }
}
