using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICanSpeakWebsite.Models
{
    public class WordDetail
    {
        public string UserId { get; set; }
        public string WordId { get; set; }
        public string VocabularyId { get; set; }
        public string SubCategory { get; set; }
        public string VocabularySubId { get; set; }
        public string ArabicText { get; set; }
        public string EnglishText { get; set; }
        public string AudioUrl { get; set; }
        public string PictureUrl { get; set; }
        public string Vocab { get; set; }
        public string FlashCardStatus { get; set; }
        public string BookMarkStatus { get; set; }
        public string isLastData { get; set; }
    }

    public class FlashCardWordDetails
    {
        public string UserId { get; set; }
        public string WordId { get; set; }
        public string VocabularyId { get; set; }
        public string FlashCardId { get; set; }
        public string SubCategory { get; set; }
        public string ArabicText { get; set; }
        public string EnglishText { get; set; }
        public string AudioUrl { get; set; }
        public string PictureUrl { get; set; }
        public string Vocab { get; set; }
        public string FlashCardStatus { get; set; }
        public string isLastData { get; set; }
    }
}