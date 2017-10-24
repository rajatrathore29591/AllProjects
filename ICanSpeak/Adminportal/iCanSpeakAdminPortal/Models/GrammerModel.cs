using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCanSpeakAdminPortal.Models
{
    public class GrammerModel
    {
        public int UnitId { get; set; }
        [Required]
        public string UnitNameEnglish { get; set; }
        [Required]
        public string UnitNameArabic { get; set; }
         [Required]
        public HttpPostedFileBase PPT { get; set; }
        [Required]
        public HttpPostedFileBase Video { get; set; }
        public HttpPostedFileBase Video2 { get; set; }
        public string PPTUrl { get; set; }
        public string VideoUrl { get; set; }
        public string AudioUrl { get; set; }
         [Required]
        public int Price { get; set; }
         public string Duration { get; set; }
         public string RewardPoints { get; set; }
         public string MaxScore { get; set; }
         public bool IsFree { get; set; }
        public string AssessmentSlots { get; set; }
        public string CorrectAnswer { get; set; }
        public string errormsg { get; set; }

        [Required]
        public string DescriptionEnglish { get; set; }
         [Required]
        public string DescriptionArabic { get; set; }
        public bool IsActive { get; set; }
        public string CreateDate { get; set; }
        public string ModifyDate { get; set; }
    }
}