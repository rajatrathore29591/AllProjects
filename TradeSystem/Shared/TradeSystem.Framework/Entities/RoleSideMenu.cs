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
    [Table("RoleSideMenu")]
    public partial class RoleSideMenu
    {
        public RoleSideMenu()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }
        [Key]
        [Required]
        public Guid Id { get; set; }

        public Guid SideMenuId { get; set; }

        [Required]
        [StringLength(150)]
        public string RoleId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        public virtual SideMenu SideMenu { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}
