using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICanSpeakWebsite.Models
{
    public class UserMessage
    {
        public string Username { get; set; }
        public string MessageId { get; set; }
        public string Subject { get; set; }
        public string IsRead { get; set; }
        public string Date { get; set; }
    }

    public class MessageDetail
    {
        public string MessageId { get; set; }
        public string SenderId { get; set; }
        public string Subject { get; set; }
        public string ProfilePicture { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Date { get; set; }
        public string DetailMessage { get; set; }
    }

    public class SearchUser
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class SentMessage
    {
        public string recieverid { get; set; }
        public string subject { get; set; }
        public string messagebody { get; set; }
    }

    public class ReplyMessage
    {
        public string UserName { get; set; }
        public string RecieverId { get; set; }
        public string Subject { get; set; }
        public string Messagebody { get; set; }
    }

    public class ForwardMessage
    {
        public string UserName { get; set; }
        public string RecieverId { get; set; }
        public string Subject { get; set; }
        public string Messagebody { get; set; }
    }
}