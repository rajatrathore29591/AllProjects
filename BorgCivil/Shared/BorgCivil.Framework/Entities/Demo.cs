using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("Demo")]
    public partial class Demo
    {
        [Key]
        public Guid DemoId { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Name { get; set; }

        [Column(TypeName = "NVARCHAR")]
        public string Address { get; set; }

        public DateTime? CurrentDate { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string RadioGender { get; set; }

        [Column(TypeName = "NVARCHAR")]

        public string CheckBoxGender { get; set; }

        [Column(TypeName = "NVARCHAR")]

        public string DropDownGender { get; set; }

    }
}
