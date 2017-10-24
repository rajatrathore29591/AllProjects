using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BorgCivil.Utils.Models
{
    public class AccessTokenModel
    {
        //public string AccessToken { get; set; }
        //public string TokenType { get; set; }
        //public string ExpiresIn { get; set; }
        //public string UserName { get; set; }
        //public string Issued { get; set; }
        //public string Expires { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string userName { get; set; }
        public string issued { get; set; }
        public string expires { get; set; }

        public string organisationId { get; set; }
        public string LoginUserId { get; set; }
        public string LoginUserName { get; set; }
        public string ImageUrl { get; set; }        

    }
}