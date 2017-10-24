using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TV.CraveASAP.BusinessEntities
{
    public class BannersEntity
    {
        public int bannerId { get; set; }
        public string type { get; set; }
        public string platform { get; set; }
        public string imageURL { get; set; }
        public string reference { get; set; }
        public Nullable<System.DateTime> createdDate { get; set; }
        public string delay { get; set; }
        public string language { get; set; }
        public string FilePath { get; set; }
        public string FileLength { get; set; }
        public string FileName { get; set; }

       
        
    }
}
