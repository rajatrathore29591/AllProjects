using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace TradeSystem.Utils.Models
{
    public class LoginDataModel
    {
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        public string Id { get; set; }

        public string DeviceToken { get; set; }

        public string DeviceType { get; set; }

        public string Lang { get; set; }

        public bool IsAdmin { get; set; }
        public string OldPassword { get; set; }

    }

}
