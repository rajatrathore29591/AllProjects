using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public  class SubCategoryEntity
    {
        //public SubCategoryEntity()
        //{
        //    this.OptionalCategories = new HashSet<OptionalCategoryEntity>();
        //}
        public int subCategoryId { get; set; }
        public string subCategoryName { get; set; }
        public string description { get; set; }
        public string language { get; set; }
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public bool isDeleted { get; set; }
       
        //public virtual CategoryEntity Category { get; set; }
        //public virtual ICollection<OptionalCategoryEntity> OptionalCategories { get; set; }
    
    }
}
