using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BorgCivil.Utils
{
    public static class WebApiHelper
    {
        public static class ApiStatus
        {
            //Status
            public static string OK = "OK";
            public static string FAILED = "FAILED";
            public static string BADREQUEST = "BADREQUEST";
            public static string INVALIDTKOEN = "INVALIDTKOEN";
        }
        public static class ApiMessage
        {
            //Status
            public static string SUCCESS = "Success";
            public static string ERROR = "Error";
            public static string EXCEPTION = "Exception";
            public static string NO_RECORD_FOUND = "No Record Found";
            public static string NOT_EXIST = "Record does not exist";
            public static string INVALID_ID = "Invalid Id";
            public static string INVALID_TKOEN = "Invalid Token";
        }
    }
}
