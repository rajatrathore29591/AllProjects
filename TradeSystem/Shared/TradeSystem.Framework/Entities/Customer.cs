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
    [Table("Customer")]
    public partial class Customer
    {
        public Customer()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        public Guid? DocumentId { get; set; }

        public string UserId { get; set; }

        public Guid? CountryId { get; set; }
        public Guid? StateId { get; set; }

        public Guid? CustomerReferalId { get; set; }

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
        public bool IsActive { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Email { get; set; }

        [Required]
        [StringLength(400)]
        [Column(TypeName = "NVARCHAR")]
        public string Password { get; set; }


        [StringLength(20)]
        [Column(TypeName = "NVARCHAR")]
        public string Phone { get; set; }


        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string MotherLastName { get; set; }

        [Required]

        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string RFC { get; set; }

        public Guid? BankId { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string BankName { get; set; }


        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string BankAccount { get; set; }


        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string Clabe { get; set; }


        [StringLength(200)]
        [Column(TypeName = "NVARCHAR")]
        public string BenificiaryName { get; set; }

        public int WrongAttempt { get; set; }

        public DateTime LastLogin { get; set; }

        public bool IsLocked { get; set; }

        public float MaxSaleEarningPercent { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string DeviceToken { get; set; }
        public string DeviceType { get; set; }

        [StringLength(200)]
        public string OpenPayCustomerId { get; set; }

        public virtual Document Document { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Customer Customers { get; set; }

        public virtual ICollection<Customer> Customerss { get; set; }

        public virtual Bank Bank { get; set; }

        public virtual Country Country { get; set; }

        public virtual State State { get; set; }

        public virtual ICollection<CustomerProduct> CustomerProducts { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }

        public virtual ICollection<Wallet> Wallets { get; set; }

    }
}
