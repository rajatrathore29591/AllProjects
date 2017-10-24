using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeSystem.Framework.Entities
{
    [Table("Document")]
    public partial class Document
    {
        public Document()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string OriginalName { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "NVARCHAR")]
        public string URL { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Title { get; set; }

        [StringLength(500)]
        [Column(TypeName = "NVARCHAR")]
        public string Description { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "NVARCHAR")]
        public string Extension { get; set; }

        public int? FileSize { get; set; }

        public bool? Private { get; set; }

        public string Tags { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string ThumbnailFileName { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<CompanyUser> CompanyUsers { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }
}
