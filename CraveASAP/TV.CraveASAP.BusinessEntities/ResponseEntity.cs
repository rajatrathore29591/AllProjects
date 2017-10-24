using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public  class ResponseEntity
    {
        public int responseCode{ get; set;}
        public string responseMessage {get; set;}
        public string IsActivePromotion { get; set; }  
        public string isUserActive { get; set; }
        public string data { get; set; }
        public string Image { get; set; }
        public string isFavouriteId { get; set; }
        public Nullable<bool> isFavourite { get; set; }
        public string category { get; set; }
        public string serverTime { get; set; }


       

             
    }
}
 