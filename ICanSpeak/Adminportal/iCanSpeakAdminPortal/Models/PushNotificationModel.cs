﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCanSpeakAdminPortal.Models
{
    public class PushNotificationModel
    {
        [Required]
        public string Message { get; set; }
    }
}