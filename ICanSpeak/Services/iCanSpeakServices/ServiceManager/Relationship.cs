using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Data.Linq.SqlClient;
using Newtonsoft.Json;

namespace iCanSpeakServices.ServiceManager
{
    public class Relationship
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        //public Stream SaveUserRelation(Stream objStream)
        //{
        //    Friend objFriend = new Friend();
        //    StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
        //    String requestString = reader.ReadToEnd();

        //    try
        //    {
        //       // requestString = "{\"userId\":\"2\",\"friendUserId\":\"7\",\"status\":\"request\",\"message\":\"hello\"}";
        //        var userFriend = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

        //        objFriend.UserId = Convert.ToInt32(userFriend["userId"]);
        //        objFriend.FriendUserId = Convert.ToInt32(userFriend["friendUserId"]);
        //        objFriend.Status = userFriend["status"];
        //        objFriend.Message = userFriend["message"];
        //        icanSpeakContext.Friends.InsertOnSubmit(objFriend);
        //        icanSpeakContext.SubmitChanges();

        //        var js = JsonConvert.SerializeObject("request send.", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
        //        return new MemoryStream(Encoding.UTF8.GetBytes(js));
        //        //return javaScriptSerializer.Serialize("request send.");

        //    }
        //    catch (Exception ex)
        //    {
        //        Helper.ErrorLog(ex, "GetUserProfile");
        //        var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
        //        return new MemoryStream(Encoding.UTF8.GetBytes(js));
        //    }
        //}


    }
}