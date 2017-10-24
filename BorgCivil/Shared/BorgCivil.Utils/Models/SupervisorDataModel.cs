using System;
using System.ComponentModel.DataAnnotations;

namespace BorgCivil.Utils.Models
{
    public partial class SupervisorDataModel
    {
        public Guid SupervisorId { get; set; }

        public Guid SiteId { get; set; }

        public string SupervisorName { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

    }

}
