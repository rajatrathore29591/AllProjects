using System.Collections.Generic;
using System.Dynamic;

namespace BorgCivil.Utils.Models
{
    public class LoginResponseDataModel
    {
        public string Message { get; set; }
        public bool Succeeded { get; set; }
        public object DataObject { get; set; }
        public List<ExpandoObject> DataList { get; set; }
        public string ErrorInfo { get; set; }        
    }
}
