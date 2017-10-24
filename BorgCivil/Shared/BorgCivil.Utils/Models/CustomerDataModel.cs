using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Utils.Models
{
    public partial class CustomerDataModel
    {
    
        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string ABN { get; set; }

        public string OfficeStreet { get; set; }

        public string OfficeState { get; set; }

        public string OfficeSuburb { get; set; }

        public string PostalSuburb { get; set; }

        public string OfficePostalCode { get; set; }

        public string PostalState { get; set; }

        public string PostalPostCode { get; set; }

        public string PhoneNumber1 { get; set; }

        public string PhoneNumber2 { get; set; }

        public string MobileNumber1 { get; set; }

        public string MobileNumber2 { get; set; }

        public string Fax { get; set; }

        public string ContactName { get; set; }

        public string ContactNumber { get; set; }

        public string AccountsContact { get; set; }

        public string AccountsNumber { get; set; }

        public string EmailForInvoices { get; set; }
      
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime? EditedDate { get; set; }

        public List<CustomerSelectListModel> CustomerSelectListModel { get; set; }
    

    }

    public class CustomerSelectListModel
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }
    }
}
