﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public class CategoryEntity
    {
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public string description { get; set; }
        public string language { get; set; }
        
        //public virtual ICollection<AppDefaultLandingPageEntity> Categories { get; set; }
        public virtual ICollection<AppDefaultLandingPageEntity> AppDefaultLandingPages { get; set; }
        //public virtual ICollection<OptionalCategoryEntity> OptionalCategories { get; set; }
        //public virtual ICollection<SubCategoryEntity> SubCategories { get; set; }
    }
}