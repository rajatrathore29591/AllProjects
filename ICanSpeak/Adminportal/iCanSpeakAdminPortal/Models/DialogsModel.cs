using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCanSpeakAdminPortal.Models
{
    public class DialogsModel
    {
        public int DialogId { get; set; }
        [Required]
        public string EnglishName { get; set; }
        [Required]
        public string ArabicName { get; set; }
        [Required]
        public HttpPostedFileBase Video { get; set; }
        [Required]
        public HttpPostedFileBase Audio1 { get; set; }
        public HttpPostedFileBase Audio2 { get; set; }
        [Required]
        public HttpPostedFileBase EnglishSubtitle { get; set; }
        [Required]
        public HttpPostedFileBase ArabicSubtitle { get; set; }
        [Required]
        public HttpPostedFileBase BothSubtitle { get; set; }
        public string VideoUrl { get; set; }
        public string AudioUrl { get; set; }
        public string Audio2Url { get; set; }
        public string EnglishSubtitleUrl { get; set; }
        public string ArabicSubtitleUrl { get; set; }
        public string BothSubtitleUrl { get; set; }
        [Required]
        public string StoryArabic { get; set; }
        [Required]
        public string StoryEnglish { get; set; }
        [Required]
        public string DescriptionArabic { get; set; }
        [Required]
        public string DescriptionEngilsh { get; set; }
        [Required]
        public string DialogGender { get; set; }
        public bool IsActive { get; set; }
        public string CreateDate { get; set; }
        public int Price { get; set; }
        public string Duration { get; set; }
        public string RewardPoints { get; set; }
        public string MaxScore { get; set; }
        public bool IsFree { get; set; }
        public string errormsg { get; set; }

    }

    public class KeyPhraseModel
    {
        public int DialogId { get; set; }
        public int KeyPhrasesId { get; set; }
        public string EnglishText { get; set; }
        public string ArabicText { get; set; }
        [Required]
        public string AudioUrl { get; set; }
        public string CreateDate { get; set; }
        public bool IsActive { get; set; }
        public List<HttpPostedFileBase> Audio { get; set; }
        public HttpPostedFileBase EditAudio { get; set; }
    }

    public class AddKeyPhraseModel
    {
        public int DialogId { get; set; }
        public List<String> Arabictxt { get; set; }
        public List<String> Englishtxt { get; set; }
    }

    public class EditKeyPhrasemodel
    {
        public int DialogId { get; set; }
        public int KeyPhrasesId { get; set; }
        public string EnglishText { get; set; }
        public string ArabicText { get; set; }
    }

    public class ConversationModel
    {
        public int ConversationId { get; set; }
        public int DialogId { get; set; }
        [Required]
        public string Person1Text { get; set; }
        [Required]
        public string Person2Text { get; set; }
        [Required]
        public string Person1ArabicText { get; set; }
        [Required]
        public string Person2ArabicText { get; set; }
        public string CreateDate { get; set; }
        public bool IsActive { get; set; }
        public string DialogGender { get; set; }
        public string DialogName { get; set; }
        public string ModifyDate { get; set; }
    }

    public class AddConversation
    {
        public List<String> oneengtxt { get; set; }
        public List<String> onearbtxt { get; set; }
        public List<String> twoengtxt { get; set; }
        public List<String> twoarbtxt { get; set; }
        public int DialogId { get; set; }
        public string gender { get; set; }
    }
}