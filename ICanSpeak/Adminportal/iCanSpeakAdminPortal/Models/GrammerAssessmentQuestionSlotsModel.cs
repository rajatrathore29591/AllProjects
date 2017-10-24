using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCanSpeakAdminPortal.Models
{
    public class GrammerAssessmentQuestionSlotsModel
    {
        public int SlotId { get; set; }
        public int UnitId { get; set; }
        [Required]
        public string Question { get; set; }
        [Required]
        public string SlotPointValue { get; set; }
        [Required]
        public string CorrectAnswer { get; set; }
        public string CreateDate { get; set; }
        public int IsActive { get; set; }
        public List<String> slotvaluearray { get; set; }
        public List<String> answerarray { get; set; }
    }
}