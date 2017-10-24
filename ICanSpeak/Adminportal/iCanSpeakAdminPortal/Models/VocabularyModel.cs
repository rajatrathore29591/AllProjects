using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCanSpeakAdminPortal.Models
{
    public class VocabularyModel
    {
        public int VocabularyId { get; set; }
        public int SelectSet { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string ArabicText { get; set; }
        [Required]
        public HttpPostedFileBase Image { get; set; }
        [Required]
        public HttpPostedFileBase Audio { get; set; }
        public string ImageUrl { get; set; }
        public string AudioUrl { get; set; }
        public string SampleSentance { get; set; }
        public bool IsActive { get; set; }
        public string CreatedDate { get; set; }
        public string subcategory { get; set; }
        public string issubcategory { get; set; }
        public int Price { get; set; }
        public string Duration { get; set; }
        public string RewardPoints { get; set; }
        public string MaxScore { get; set; }
        public bool IsFree { get; set; }
        public int SubCategoryCount { get; set; }
        public int WordCount{ get; set; }
    }

    public class VocabularySubCategoryModel
    {
        public string SubCategoryName { get; set; }
        public int VocabularyId { get; set; }
        public int VacabularySubId { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string subcategory { get; set; }
        public bool IsActive { get; set; }
        public string WordCount { get; set; }
        public string SetCount { get; set; }
    }

    public class VocabularyWordModel
    {
        public int WordId { get; set; }
        public int VocabularySubId { get; set; }
        public string EnglishText{get;set;}
        [Required]
        public List<String> EnglishTexts { get; set; }
        [Required]
        public string ArabicText { get; set; }
        public List<String> ArabicTexts { get; set; }
        [Required]
        public List<HttpPostedFileBase>  Image { get; set; }
        [Required]
        public List<HttpPostedFileBase> Audio { get; set; }
        public HttpPostedFileBase ImageFile {get;set; }
        public HttpPostedFileBase AudioFile { get; set; }
        public string PictureUrl { get; set; }
        public string AudioUrl { get; set; }
        public bool IsActive { get; set; }
        public string CreateDate { get; set; }
        public string ModifyDate { get; set; }
        public string wordname { get; set; }
        public string vocabid { get; set; }
    }


}