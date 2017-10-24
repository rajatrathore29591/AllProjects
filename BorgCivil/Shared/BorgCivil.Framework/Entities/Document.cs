using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("Document")]
    public partial class Document
    {
        [Key]
        public Guid DocumentId { get; set; }

        [StringLength(200)]
        [Column(TypeName = "NVARCHAR")]
        public string Name { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string OriginalName { get; set; }

        [StringLength(255)]
        [Column(TypeName = "NVARCHAR")]
        public string URL { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Title { get; set; }

        [Column(TypeName = "NVARCHAR")]
        public string Description { get; set; }

        [StringLength(20)]
        [Column(TypeName = "NVARCHAR")]
        public string Extension { get; set; }

        public int FileSize { get; set; }

        public bool Private { get; set; }

        [Column(TypeName = "NVARCHAR")]
        public string Tags { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string ThumbnailFileName { get; set; }
     
        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual ICollection<Docket> Dockets { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<FleetType> FleetTypes { get; set; }

        public virtual ICollection<Driver> Drivers { get; set; }

        public virtual ICollection<Site> Sites { get; set; }

    }
}
