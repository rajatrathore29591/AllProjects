using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeSystem.Framework.Entities
{
    [Table("Ticket")]
    public partial class Ticket
    {
        public Ticket()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? TicketStatusId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AutoIncrementedNo { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual TicketStatus TicketStatus { get; set; }
    }
}
