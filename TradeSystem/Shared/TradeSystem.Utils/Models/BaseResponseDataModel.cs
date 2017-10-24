using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Utils.Models
{
    public class BaseResponseDataModel
    {
        public string Message { get; set; }
        public bool Succeeded { get; set; }
        public ExpandoObject DataObject { get; set; }
        public List<ExpandoObject> DataList { get; set; }
        public string ErrorInfo { get; set; }        
    }
}
