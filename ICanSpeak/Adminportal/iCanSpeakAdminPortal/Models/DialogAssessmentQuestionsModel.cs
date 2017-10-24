using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCanSpeakAdminPortal.Models
{
    public class DialogAssessmentQuestionsModel
    {
        public int QuestionId { get; set; }
        [Required]
        public string Question { get; set; }
        [Required]
        public string QuestionType { get; set; }
        [Required]
        public string AnswerOptions { get; set; }
        [Required]
        public string CorrectAnswer { get; set; }

        public int DialogId { get; set; }
        
        public string CreateDate { get; set; }
        public string ModifiedDate { get; set; }
        public Boolean IsActive { get; set; }
        public string FillAnsText { get; set; }
        public Boolean TrueFalseType { get; set; }
        public string ObjOpt1txt { get; set; }
        public string ObjOpt2txt { get; set; }
        public string ObjOpt3txt { get; set; }
        public string OptionAudio1 { get; set; }
        public string OptionAudio2 { get; set; }
        public string OptionAudio3 { get; set; }
        public string OptionCorrectAnswer { get; set; }
        [Required]
        public HttpPostedFileBase OptionAudioUrl1 { get; set; }
        [Required]
        public HttpPostedFileBase OptionAudioUrl2 { get; set; }
        [Required]
        public HttpPostedFileBase OptionAudioUrl3 { get; set; }
        public string errormsg { get; set; }
        public string TrueFalseTypes { get; set; }
       

    }
}