using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeSystem.Framework.Entities
{
    [Table("Bank")]
    public partial class Bank
    {
        public Bank()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "NVARCHAR")]
        public string BankName { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }


        public virtual ICollection<Customer> Customers { get; set; }
    }
}
