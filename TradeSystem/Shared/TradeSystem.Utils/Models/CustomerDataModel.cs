using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeSystem.Utils.Models
{
    public partial class CustomerDataModel
    {
      
        public string Id { get; set; }

        public string UserId { get; set; }
        public string DocumentId { get; set; }

        public string CustomerReferalId { get; set; }

        public string CountryId { get; set; }

        public string CountryName { get; set; }

        public string StateId { get; set; }

        public string StateName { get; set; }

        public string FirstName { get; set; }
        
        public string MiddleName { get; set; }
       
        public string LastName { get; set; }
       
        public bool IsActive { get; set; }
        
        public string Email { get; set; }
        public string ConfirmEmail { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Phone { get; set; }
        
        public string UserName { get; set; }
       
        public string MotherLastName { get; set; }

        public string BirthDate { get; set; }
        
        public string RFC { get; set; }

        public string DNI { get; set; }

        public string BankId { get; set; }

        public string BankName { get; set; }

        public string BankAccount { get; set; }
        
        public string Clabe { get; set; }
        
        public string BenificiaryName { get; set; }

        public int WrongAttempt { get; set; }

        public DateTime LastLogin { get; set; }

        public bool IsLocked { get; set; }

        public string ImageBase64 { get; set; }
        public float Commission { get; set; }
        //CheckStatus for check IsActive condition
        public string CheckStatus { get; set; }

        public bool IsChecked { get; set; }

        public DateTime CreatedDate { get; set; }

        public string TotalValueOfInvestment { get; set; }

        public string companyUserId { get; set; }
        public string Lang { get; set; }
        public string OpenpayPaymentCustomerId { get; set; }

    }

    public class CustomerPasswordDataModel
    {
        [Required]
        public string Id { get; set; }

        
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "password")]
        public string Password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        public string Lang { get; set; }
        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm new password")]
        //[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }

    }

    public partial class CustomerListDataModel
    {

        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UserName { get; set; }

        public string CustomerName { get; set; }

        public string Status { get; set; }

        public string TotalInvestment { get; set; }

        public float TotalInvestmentFloat { get; set; }

        public string TotalInvestmentCount { get; set; }
        public string TotalSaleCount { get; set; }
    }
}
