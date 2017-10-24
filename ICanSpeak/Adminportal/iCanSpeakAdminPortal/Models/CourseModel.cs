using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCanSpeakAdminPortal.Models
{
    public class CourseModel
    {
        public int CourseId { get; set; }
        [Required]
        public string CourseName { get; set; }
        [Required]
        public string CourseDescription { get; set; }
        [Required]
        public string CourseType { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public string RewardPoints { get; set; }
        [Required]
        public int Price { get; set; }

        public string ImageUrl { get; set; }
        public string AudioUrl { get; set; }

        [Required]
        public HttpPostedFileBase Image { get; set; }
        [Required]
        public HttpPostedFileBase Audio { get; set; }
        [Required]
        public HttpPostedFileBase Video { get; set; }
        [Required]
        public string MaxScore { get; set; }
        [Required]
        public bool IsFree { get; set; }
        public bool IsActive { get; set; }
        public string CreateDate { get; set; }
        [Required]
        public int Unit { get; set; }
    }
}