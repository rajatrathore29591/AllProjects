using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Data;
using System.ServiceModel.Web;

namespace iCanSpeakServices.ServiceManager
{
    public class UserFriends
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataTable Table = new DataTable();
        DataSet ds = new DataSet();

        public Stream GetUnfriendsByUserId(Stream objStream)
        {
            //Friend objFriend = new Friend();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            //===For Unfriends List
            DataTable UnFriendListDataStatus = new DataTable();
            DataTable UnFriendList = new DataTable();
            //===For Friends List
            DataTable FriendListDataStatus = new DataTable();
            DataTable FriendList = new DataTable();

            DataTable MyscoreDataStatus = new DataTable();
            DataTable TotalFlashDataStatus = new DataTable();
            DataTable Myscores = new DataTable();

            try
            {
                var searchResult = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int userid = Convert.ToInt32(Service.Decrypt(searchResult["userid"]));

                //var getinvitesdID = from frnd in icanSpeakContext.Friends where frnd.UserID == userid select new { frnd.InvitedID };
                //var getuserID = from frnd in icanSpeakContext.Friends where frnd.InvitedID == userid select new { frnd.UserID };

                var UnfrndResult = ((from user in icanSpeakContext.Users
                                     join friend in icanSpeakContext.Friends on user.UserId equals friend.UserID into Users
                                     from Friends in Users.DefaultIfEmpty()
                                     where !(from frnd in icanSpeakContext.Friends where frnd.UserID == userid select frnd.InvitedID).Contains(user.UserId) && !(from frnd in icanSpeakContext.Friends where frnd.InvitedID == userid select frnd.UserID).Contains(user.UserId) && user.UserId != userid
                                     select new
                                     {
                                         UserId = Service.Encrypt(user.UserId.ToString()),
                                         UserName = user.FirstName + " " + user.LastName,
                                         user.Country,
                                         Status = "2",
                                         ProfilePicture = Service.GetUrl() + "ProfilePictures/" + user.ProfilePicture
                                     }).Union(
                         from user in icanSpeakContext.Users
                         join friend in icanSpeakContext.Friends on user.UserId equals friend.InvitedID
                         where friend.Status == "0" && friend.UserID == userid
                         select new
                         {
                             UserId = Service.Encrypt(user.UserId.ToString()),
                             UserName = user.FirstName + " " + user.LastName,
                             user.Country,
                             friend.Status,
                             ProfilePicture = Service.GetUrl() + "ProfilePictures/" + user.ProfilePicture
                         }
                             )).ToList();


                if (UnfrndResult != null)
                {
                    UnFriendListDataStatus = Service.Message("success", "1");
                    UnFriendListDataStatus.TableName = "UnFriendListDataStatus";
                    UnFriendListDataStatus.AcceptChanges();
                    UnFriendList = Service.ConvertToDataTable(UnfrndResult); ;
                    UnFriendList.TableName = "UnFrndUserProfile";

                    //==============================FriendsList Section Start===============================================//

                    var frndResult = ((from user in icanSpeakContext.Users
                                       join friend in icanSpeakContext.Friends on user.UserId equals friend.UserID
                                       where friend.Status == "0" && friend.InvitedID == userid
                                       select new
                                       {
                                           UserId = Service.Encrypt(user.UserId.ToString()),
                                           UserName = user.FirstName + " " + user.LastName,
                                           user.Country,
                                           friend.Status,
                                           ProfilePicture = Service.GetUrl() + "ProfilePictures/" + user.ProfilePicture
                                       }).Union(
                         from user in icanSpeakContext.Users
                         join friend in icanSpeakContext.Friends on user.UserId equals friend.InvitedID
                         where friend.Status == "1" && friend.UserID == userid
                         select new
                         {
                             UserId = Service.Encrypt(user.UserId.ToString()),
                             UserName = user.FirstName + " " + user.LastName,
                             user.Country,
                             friend.Status,
                             ProfilePicture = Service.GetUrl() + "ProfilePictures/" + user.ProfilePicture
                         }
                             ).Union(from user in icanSpeakContext.Users
                                     join friend in icanSpeakContext.Friends on user.UserId equals friend.UserID
                                     where friend.Status == "1" && friend.InvitedID == userid
                                     select new
                                     {
                                         UserId = Service.Encrypt(user.UserId.ToString()),
                                         UserName = user.FirstName + " " + user.LastName,
                                         user.Country,
                                         friend.Status,
                                         ProfilePicture = Service.GetUrl() + "ProfilePictures/" + user.ProfilePicture
                                     })).ToList();

                    if (frndResult.Count > 0)
                    {
                        FriendListDataStatus = Service.DataStatus("success", "1");
                        FriendListDataStatus.TableName = "FriendListDataStatus";
                        FriendList = Service.ConvertToDataTable(frndResult);
                        FriendList.TableName = "FrndUserProfile";

                    }
                    else
                    {
                        FriendListDataStatus = Service.DataStatus("No Data", "0");
                        FriendListDataStatus.TableName = "FriendListDataStatus";
                    }

                    //==============================FriendsList Section End===============================================//


                    var Myscore = (from user in icanSpeakContext.Users
                                   where user.UserId == userid
                                   select new
                                   {
                                       user.MyScore,
                                       user.TotalScore,
                                       user.FlashCard
                                   }).ToList();

                    if (Myscore.Count > 0)
                    {
                        MyscoreDataStatus = Service.DataStatus("success", "1");
                        MyscoreDataStatus.TableName = "MyscoreDataStatus";
                        Myscores = Service.ConvertToDataTable(Myscore);
                        Myscores.TableName = "Myscores";
                    }
                    else
                    {
                        MyscoreDataStatus = Service.DataStatus("No Data", "0");
                        MyscoreDataStatus.TableName = "MyscoreDataStatus";
                    }
                    //=========================

                    //ds.Tables.Add(Table);
                   // ds.Tables.Add(Vocab);
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    ds.Tables.Add(MyscoreDataStatus);
                    ds.Tables.Add(Myscores);


                    ds.Tables.Add(UnFriendListDataStatus);
                    ds.Tables.Add(UnFriendList);
                    ds.Tables.Add(FriendListDataStatus);
                    ds.Tables.Add(FriendList);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
                else
                {
                    DataTable Table = Service.Message("No Data", "0");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetUnfriendsByUserId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream SendFreindRequest(Stream objStream)
        {
            DataSet ds = new DataSet("Result");
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                var paramter = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                Friend objcard = new Friend();
                objcard.UserID = Convert.ToInt32(Service.Decrypt(paramter["userid"]));
                objcard.InvitedID = Convert.ToInt32(Service.Decrypt(paramter["invitedid"]));
                objcard.Status = "0";
                objcard.CreatedOn = System.DateTime.Now;
                icanSpeakContext.Friends.InsertOnSubmit(objcard);
                icanSpeakContext.SubmitChanges();

                var username = (from user in icanSpeakContext.Users
                                where user.UserId == Convert.ToInt32(Service.Decrypt(paramter["invitedid"]))
                                select user
                               ).FirstOrDefault();

                string usernames = username.Username;

                MyActivity objmyactivity = new MyActivity();
                objmyactivity.UserId = Convert.ToInt32(Service.Decrypt(paramter["userid"]));
                objmyactivity.Message = "Your friend request has been sent to " + usernames + "";
                objmyactivity.CreatedDate = System.DateTime.Now;
                icanSpeakContext.MyActivities.InsertOnSubmit(objmyactivity);
                icanSpeakContext.SubmitChanges();


                Table = Service.Message("success", "0");
                Table.AcceptChanges();
                DataTable ResultMessage = Service.ResultMessage("save", "1");

                ds.Tables.Add(Table);
                ds.Tables.Add(ResultMessage);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, requestString);
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));// javaScriptSerializer.Serialize(ex.Message.ToString());
            }
        }

        public Stream CancelSendfriendsRequestByUserId(Stream objStream)
        {
            //Friend objFriend = new Friend();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            //===For Unfriends List
            DataTable CancelfriendsDataStatus = new DataTable();
            DataTable Cancelfriends = new DataTable();

            try
            {
                var searchResult = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int userid = Convert.ToInt32(Service.Decrypt(searchResult["userid"]));
                int invitedid = Convert.ToInt32(Service.Decrypt(searchResult["invitedid"]));
                //var getinvitesdID = from frnd in icanSpeakContext.Friends where frnd.UserID == userid select new { frnd.InvitedID };
                //var getuserID = from frnd in icanSpeakContext.Friends where frnd.InvitedID == userid select new { frnd.UserID };

                var CancelfrndResult = (from user in icanSpeakContext.Friends
                                        where (user.UserID == userid) && (user.InvitedID == invitedid) && (user.Status == "0")
                                        select user).FirstOrDefault();


                if (CancelfrndResult != null)
                {
                    icanSpeakContext.Friends.DeleteOnSubmit(CancelfrndResult);
                    icanSpeakContext.SubmitChanges();

                    CancelfriendsDataStatus = Service.Message("success", "1");
                    CancelfriendsDataStatus.AcceptChanges();
                    Cancelfriends = Service.ResultMessage("Cancel", "1");
                    Cancelfriends.TableName = "Cancelfriends";

                    ds.Tables.Add(CancelfriendsDataStatus);
                    ds.Tables.Add(Cancelfriends);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
                else
                {
                    DataTable Table = Service.Message("No Data", "0");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "CancelSendfriendsRequestByUserId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream AcceptGetfriendsRequestByUserId(Stream objStream)
        {
            //Friend objFriend = new Friend();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            //===For Unfriends List
            DataTable AcceptfriendsDataStatus = new DataTable();
            DataTable Acceptfriends = new DataTable();

            try
            {
                var searchResult = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int userid = Convert.ToInt32(Service.Decrypt(searchResult["userid"]));
                int invitedid = Convert.ToInt32(Service.Decrypt(searchResult["invitedid"]));
                //var getinvitesdID = from frnd in icanSpeakContext.Friends where frnd.UserID == userid select new { frnd.InvitedID };
                //var getuserID = from frnd in icanSpeakContext.Friends where frnd.InvitedID == userid select new { frnd.UserID };

                var AcceptfrndResult = (from user in icanSpeakContext.Friends
                                        where (user.UserID == invitedid) && (user.InvitedID == userid) && (user.Status == "0")
                                        select user).FirstOrDefault();


                if (AcceptfrndResult != null)
                {
                    AcceptfrndResult.Status = "1";
                    icanSpeakContext.SubmitChanges();

                    AcceptfriendsDataStatus = Service.Message("success", "1");
                    AcceptfriendsDataStatus.AcceptChanges();
                    //UnFriendList = Service.ConvertToDataTable(CancelfrndResult);
                    Acceptfriends = Service.ResultMessage("Accpet", "1");
                    Acceptfriends.TableName = "Acceptfriends";

                    ds.Tables.Add(AcceptfriendsDataStatus);
                    ds.Tables.Add(Acceptfriends);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
                else
                {
                    DataTable Table = Service.Message("No Data", "0");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AcceptGetfriendsRequestByUserId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream CancelGetfriendsRequestByUserId(Stream objStream)
        {
            //Friend objFriend = new Friend();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            //===For Unfriends List
            DataTable CancelfriendsDataStatus = new DataTable();
            DataTable Cancelfriends = new DataTable();

            try
            {
                var searchResult = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int userid = Convert.ToInt32(Service.Decrypt(searchResult["userid"]));
                int invitedid = Convert.ToInt32(Service.Decrypt(searchResult["invitedid"]));

                var CancelfrndResult = (from user in icanSpeakContext.Friends
                                        where (user.UserID == invitedid) && (user.InvitedID == userid) && (user.Status == "0")
                                        select user).FirstOrDefault();

                if (CancelfrndResult != null)
                {
                    icanSpeakContext.Friends.DeleteOnSubmit(CancelfrndResult);
                    icanSpeakContext.SubmitChanges();

                    CancelfriendsDataStatus = Service.Message("success", "1");
                    CancelfriendsDataStatus.AcceptChanges();
                    //UnFriendList = Service.ConvertToDataTable(CancelfrndResult);
                    Cancelfriends = Service.ResultMessage("Cancel", "1");
                    Cancelfriends.TableName = "Cancelfriends";

                    ds.Tables.Add(CancelfriendsDataStatus);
                    ds.Tables.Add(Cancelfriends);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
                else
                {
                    DataTable Table = Service.Message("No Data", "0");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "CancelGetfriendsRequestByUserId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream UnFriendRequestByUserId(Stream objStream)
        {
            //Friend objFriend = new Friend();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            //===For Unfriends List
            DataTable UnFriendfriendsDataStatus = new DataTable();
            DataTable UnFriendfriends = new DataTable();

            try
            {
                var searchResult = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int userid = Convert.ToInt32(Service.Decrypt(searchResult["userid"]));
                int invitedid = Convert.ToInt32(Service.Decrypt(searchResult["invitedid"]));

                var CancelfrndResult = (from user in icanSpeakContext.Friends
                                        where (user.UserID == userid && user.InvitedID == invitedid && user.Status == "1") || (user.UserID == invitedid && user.InvitedID == userid && user.Status == "1")
                                        select user).FirstOrDefault();

                if (CancelfrndResult != null)
                {
                    icanSpeakContext.Friends.DeleteOnSubmit(CancelfrndResult);
                    icanSpeakContext.SubmitChanges();

                    UnFriendfriendsDataStatus = Service.Message("success", "1");
                    UnFriendfriendsDataStatus.AcceptChanges();
                    //UnFriendList = Service.ConvertToDataTable(CancelfrndResult);
                    UnFriendfriends = Service.ResultMessage("Cancel", "1");
                    UnFriendfriends.TableName = "UnFriendfriends";

                    ds.Tables.Add(UnFriendfriendsDataStatus);
                    ds.Tables.Add(UnFriendfriends);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
                else
                {
                    DataTable Table = Service.Message("No Data", "0");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "UnFriendRequestByUserId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }


        public Stream SearchFriendsByName(Stream objStream)
        {
            Friend objFriend = new Friend();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();

            try
            {

                requestString = "{\"userId\":\"1\",\"friendUserId\":\"2\",\"searchString\":\"rahul\",\"pageNumber\":\"0\"}";
                var searchResult = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from user in icanSpeakContext.Users
                              join friend in icanSpeakContext.Friends on user.UserId equals friend.InviteFriendID
                              where (user.FirstName.StartsWith(searchResult["searchString"].ToString())
                              && friend.UserID == Convert.ToInt32(searchResult["userId"]))
                              select new
                              {
                                  user.UserId,
                                  UserName = user.FirstName + " " + user.LastName,
                                  friend.Status,
                              }).Skip(Convert.ToInt32(searchResult["pageNumber"].ToString())).Take(10).ToList();

                if (result.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                   // return javaScriptSerializer.Serialize(result);
                }
                else
                {
                    var js = JsonConvert.SerializeObject("Record not found", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                  //  return javaScriptSerializer.Serialize("Record not found");
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "SearchFriendsByName");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }

        }

        public Stream GetFriendByUserId(Stream objStream)
        {
            Friend objFriend = new Friend();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();

            try
            {

                //requestString = "{\"userId\":\"1\",\"friendUserId\":\"2\",\"pageNumber\":\"0\"}";
                var searchResult = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                string pagenumber = searchResult["pagenumber"];
                var Srno = 0;

                var result = ((from user in icanSpeakContext.Users
                         join friend in icanSpeakContext.Friends on user.UserId equals friend.UserID
                               where friend.InviteFriendID == Convert.ToInt32(searchResult["loginuserid"])
                               select new { user.UserId, 
                                            user.Username,
                                            ProfilePicture = Service.GetUrl() + "ProfilePictures/" + user.ProfilePicture
                               }).Union(
                         from user in icanSpeakContext.Users
                         join friend in icanSpeakContext.Friends on user.UserId equals friend.InviteFriendID
                         where friend.UserID == Convert.ToInt32(searchResult["loginuserid"])
                         select new
                         {

                             user.UserId,
                             user.Username,
                             ProfilePicture = Service.GetUrl() + "ProfilePictures/" + user.ProfilePicture
                         }
                             )).Skip(0).Take(10).ToList();
              
                             

                //var result = (from user in icanSpeakContext.Users
                //               from friend in icanSpeakContext.Friends
                //               where (user.UserId== friend.UserId || user.UserId==friend.FriendId) 
                //              // && (friend.UserId == Convert.ToInt32(searchResult["friendUserId"]) || friend.FriendId == Convert.ToInt32(searchResult["friendUserId"]))
                //              && (friend.Status == "friend")
                //              select user).Skip(Convert.ToInt32(searchResult["pageNumber"].ToString())).Take(10).ToList();

                if (result.Count > 0)
                {

                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(result); ;
                    dt.TableName = "Profile";
                    ds.Tables.Add(Table);
                    ds.Tables.Add(dt);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
                else
                {
                    DataTable Table = Service.Message("No Data", "0");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetFriendByUserId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

    }
}