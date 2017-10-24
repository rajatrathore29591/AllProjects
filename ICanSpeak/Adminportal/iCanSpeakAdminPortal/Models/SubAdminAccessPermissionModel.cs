using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iCanSpeakAdminPortal.Models
{
    public class SubAdminAccessPermissionModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
    }
    public class SubAdminAccessPermissionDisplayNamesModel
    {
        public int MenuId { get; set; }
        public string DisplayName { get; set; }
        public string URL { get; set; }
    }
}