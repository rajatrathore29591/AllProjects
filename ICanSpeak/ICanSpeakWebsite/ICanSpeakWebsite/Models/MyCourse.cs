using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ICanSpeakWebsite.Models
{

    public class MyCourseDataUser
    {
        public string ProfilePicture { get; set; }
        public string Userid { get; set; }
        public string Username { get; set; }
        public string Country { get; set; }
    }

    public class MyCourseDataGrammar
    {
        public string RowIndex { get; set; }
        public string UnitId { get; set; }
        public string UnitNameEnglish { get; set; }
        public string UnitNameArabic { get; set; }
    }

    public class MyCourseDataDialog
    {
        public string DialogId { get; set; }
        public string DialogEnglish { get; set; }
        public string DialogArabic { get; set; }
    }

    public class MyCourseDataVocab
    {
        public string VocabularyId { get; set; }
        public string VocabEnglish { get; set; }
        public string VocabArabic { get; set; }
    }


    public class TestUrl
    {
        public string url { get; set; }
    }


}