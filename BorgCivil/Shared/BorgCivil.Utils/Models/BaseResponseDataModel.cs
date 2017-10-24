using System.Collections.Generic;
using System.Dynamic;

namespace BorgCivil.Utils.Models
{
    public class BaseResponseDataModel
    {
        public string Message { get; set; }
        public bool Succeeded { get; set; }
        public ExpandoObject DataObject { get; set; }
        public List<ExpandoObject> DataList { get; set; }
        public string ErrorInfo { get; set; }     
        public int Count { get; set; }
        public string Id { get; set; }
    }
}
