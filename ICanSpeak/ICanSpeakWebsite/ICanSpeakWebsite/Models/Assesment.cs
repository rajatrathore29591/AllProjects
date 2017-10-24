using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICanSpeakWebsite.Models
{
    public class DialogAssesment
    {
        public string QuestionID { get; set; }
        public string DialogId { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string OptionText1 { get; set; }
        public string OptionText2 { get; set; }
        public string OptionText3 { get; set; }
        public string OptionAudio1 { get; set; }
        public string OptionAudio2 { get; set; }
        public string OptionAudio3 { get; set; }
        public string FillAnswerText { get; set; }
        public string TrueFalseAnswer { get; set; }
        public string OptionCorrectAnswer { get; set; }
    }
    public class NextDialogAssesment
    {
        public string QuestionID { get; set; }
        public string DialogId { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string OptionText1 { get; set; }
        public string OptionText2 { get; set; }
        public string OptionText3 { get; set; }
        public string OptionAudio1 { get; set; }
        public string OptionAudio2 { get; set; }
        public string OptionAudio3 { get; set; }
        public string FillAnswerText { get; set; }
        public string TrueFalseAnswer { get; set; }
        public string OptionCorrectAnswer { get; set; }
    }

    public class AssesmentModel
    {
        public string QuestionID { get; set; }
        public string VocabID { get; set; }
        public string Questions { get; set; }
        public string optionA { get; set; }
        public string optionB { get; set; }
        public string optionC { get; set; }
        public string optionD { get; set; }
        public string image { get; set; }
        public string audio1 { get; set; }
        public string audio2 { get; set; }
        public string audio3 { get; set; }
        public string audio4 { get; set; }
        public string CorrectAnswer { get; set; }

        // public string VocabQuestions { get; set; }

    }
}
