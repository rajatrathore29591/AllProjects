using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
 public class OptionalCategoryEntity
    {
        public int optCategoryId { get; set; }
        public string optCategoryName { get; set; }
        public string description { get; set; }
        public string language { get; set; }
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }
        public string categoryName { get; set; }

       // public virtual SubCategoryEntity SubCategory { get; set; }
    }
}
