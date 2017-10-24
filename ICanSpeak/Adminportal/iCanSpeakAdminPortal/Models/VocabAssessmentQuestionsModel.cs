using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iCanSpeakAdminPortal.Models
{
    public class VocabAssessmentQuestionsModel
    {
        public int VocabularyId { get; set; }
        public int QuestionId { get; set; }
        public int WordCount { get; set; }
        public int SelectSet { get; set; }
        public string Question { get;set; }
        public string CorrectAnswer { get; set; }
        public string CreatedDate { get; set; }
        public Boolean IsActive { get; set; }
        public string OptionsA { get; set; }
        public string OptionsB { get; set; }
        public string OptionsC { get; set; }
        public string OptionsD { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public string ImageURL { get; set; }
        public string Option1AudioUrl { get; set; }
        public string Option2AudioUrl { get; set; }
        public string Option3AudioUrl { get; set; }
        public string Option4AudioUrl { get; set; }
        public HttpPostedFileBase optaud1 { get; set; }
        public HttpPostedFileBase optaud2 { get; set; }
        public HttpPostedFileBase optaud3 { get; set; }
        public HttpPostedFileBase optaud4 { get; set; }
    }
}