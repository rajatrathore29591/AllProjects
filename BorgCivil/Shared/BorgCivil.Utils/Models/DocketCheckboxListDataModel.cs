using System;

namespace BorgCivil.Utils.Models
{

    public partial class DocketCheckboxListDataModel
    {

        public Guid DocketCheckListId { get; set; }

        public string Title { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public string EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

    }
}
