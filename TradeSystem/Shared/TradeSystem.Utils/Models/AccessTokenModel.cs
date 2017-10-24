using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSystem.Utils.Models
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
        public string loginUserId { get; set; }
        public string loginUserName { get; set; }
        public string imageUrl { get; set; }        
        public string CustomerManagement { get; set; }
        public string InvestmentConfiguration { get; set; }
        public string Inventory { get; set; }
        public string FinanceManagement { get; set; }
        public string TicketManagement { get; set; }
        public string AccountManagement { get; set; }
        public string Reports { get; set; }
        public string Promotions { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    }
}