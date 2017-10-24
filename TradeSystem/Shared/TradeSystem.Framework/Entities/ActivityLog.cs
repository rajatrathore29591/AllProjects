using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeSystem.Framework.Entities
{
    [Table("ActivityLog")]
    public partial class ActivityLog
    {
        public ActivityLog()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }
        [Key]
        [Required]
        public Guid Id { get; set; }
        public Guid CompanyUserId { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Activity { get; set; }
        public string Description { get; set; }
        public bool IsCompanyUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public virtual CompanyUser CompanyUser { get; set; }

    }
}
