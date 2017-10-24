using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("Customer")]
    public partial class Customer
    {
        [Key]
        public Guid CustomerId { get; set; }

        public Guid? CompanyId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string CustomerName { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string ABN { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string OfficeStreet { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string OfficeState { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string OfficeSuburb { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string PostalSuburb { get; set; }

        [StringLength(10)]
        [Column(TypeName = "NVARCHAR")]
        public string OfficePostalCode { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string PostalState { get; set; }

        [StringLength(30)]
        [Column(TypeName = "NVARCHAR")]
        public string PostalStreetPoBox { get; set; }

        [StringLength(10)]
        [Column(TypeName = "NVARCHAR")]
        public string PostalPostCode { get; set; }

        [StringLength(12)]
        [Column(TypeName = "NVARCHAR")]
        public string PhoneNumber1 { get; set; }

        [StringLength(12)]
        [Column(TypeName = "NVARCHAR")]
        public string PhoneNumber2 { get; set; }

        [StringLength(12)]
        [Column(TypeName = "NVARCHAR")]
        public string MobileNumber1 { get; set; }

        [StringLength(12)]
        [Column(TypeName = "NVARCHAR")]
        public string MobileNumber2 { get; set; }

        [StringLength(12)]
        [Column(TypeName = "NVARCHAR")]
        public string Fax { get; set; }

        [StringLength(80)]
        [Column(TypeName = "NVARCHAR")]
        public string ContactName { get; set; }

        [StringLength(12)]
        [Column(TypeName = "NVARCHAR")]
        public string ContactNumber { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string AccountsContact { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string AccountsNumber { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string EmailForInvoices { get; set; }
      
        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime? EditedDate { get; set; }

        public virtual Company Companies { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }  

        public virtual ICollection<Site> Sites { get; set; }
    }

    public class SelectListModel
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }
    }
}
