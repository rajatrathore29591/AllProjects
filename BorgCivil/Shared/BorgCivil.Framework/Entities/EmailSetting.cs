using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BorgCivil.Framework.Entities
{
    [Table("EmailSetting")]
    [ScaffoldTable(true)]
    public partial class EmailSetting
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string SmtpServer { get; set; }

        [Required]
        public int SmtpPort { get; set; }

        [Required]
        [StringLength(50)]
        public string SmtpUserName { get; set; }

        [Required]
        [StringLength(50)]
        public string SmtpPassword { get; set; }

        [Required]
        [StringLength(50)]
        public string SmtpEmail { get; set; }

        [Required]
        public int IntervelTime { get; set; }
        
        public Guid? OrganisationId { get; set; }

        public bool IsActive { get; set; }

        //public virtual CompanyUser CompanyName { get; set; }
    }
}
