using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeSystem.Framework.Entities
{
    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }
        [Key]
        [Required]
        public Guid Id { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string Name { get; set; }

        public string Description { get; set; }
        [Required]
        [StringLength(100)]
        public string PercentWeeklyEarning { get; set; }
        [Required]
        [StringLength(100)]
        public string PercentSaleEarning { get; set; }

        [Required]
        [StringLength(50)]
        public string WeeklyFromWithdrawDay { get; set; }

        [Required]
        [StringLength(50)]
        public string WeeklyToWithdrawDay { get; set; }

        [Required]
        [StringLength(50)]
        public string SaleWithdrawDay { get; set; }

        public DateTime InvestmentWithdrawDate { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        public Guid? DocumentId { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Condition1 { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Condition2 { get; set; }

        [StringLength(500)]
        [Column(TypeName = "NVARCHAR")]
        public string Condition3 { get; set; }

        public float MinPrice { get; set; }

        public float MaxPrice { get; set; }

        public float TotalValueOfInvestment { get; set; }

        public float RemainingValueOfInvestment { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string ByUserName { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string ByCountry { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string ByState { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string ByFromAmount { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string ByToAmount { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string ByFromSale { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string ByToSale { get; set; }

        public bool IsActive { get; set; }

        public string LastWeeklyWithdrawDate { get; set; }

        public virtual ICollection<CustomerProduct> CustomerProducts { get; set; }

        public virtual ICollection<Penalty> Penaltys { get; set; }

        public virtual Document Document { get; set; }

    }
}
