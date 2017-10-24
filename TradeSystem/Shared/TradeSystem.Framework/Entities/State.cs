using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeSystem.Framework.Entities
{
    [Table("State")]
    public partial class State
    {
        public State()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        public Guid CountryId { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "NVARCHAR")]
        public string StateName { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
