using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICanSpeakWebsite.Models
{
    public class FriendDetail
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string ProfilePicture { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
    }

    public class FriendDetails
    {
        public List<FriendDetail> GetSuggestList { get; set; }
        public List<FriendDetail> GetFriends { get; set; }
    }
}
