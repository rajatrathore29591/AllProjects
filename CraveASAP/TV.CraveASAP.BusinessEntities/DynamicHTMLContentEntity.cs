using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public partial class DynamicHTMLContentEntity 
    {
        
        public int Id { get; set; }
        public string pageName { get; set; }
        public string language { get; set; }
        public string pageContent { get; set; }
        public string contentFor { get; set; }
        public string plainText { get; set; }
       
    }
}
