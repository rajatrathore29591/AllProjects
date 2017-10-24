using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TradeSystem.Framework.Identity;

namespace TradeSystem.Framework.Entities
{
    [Table("CompanyUser")]
    public partial class CompanyUser
    {
        public CompanyUser()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        public Guid? DocumentId { get; set; }

      
        public string UserId { get; set; }

        [StringLength(150)]
        public string RoleId { get; set; }

        

        [StringLength(200)]
        [Column(TypeName = "NVARCHAR")]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Email { get; set; }

       
        [StringLength(20)]
        [Column(TypeName = "NVARCHAR")]
        public string Phone { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Password { get; set; }

       
        [StringLength(300)]
        [Column(TypeName = "NVARCHAR")]
        public string Address { get; set; }

        [Required]
        public bool IsSuperAdmin { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual Document Document { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ApplicationRole Role { get; set; }

        public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
    }
}
