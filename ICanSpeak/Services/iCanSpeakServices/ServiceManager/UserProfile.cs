using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Data;

using System.Drawing;
using System.Web.Hosting;
using System.ServiceModel.Web;
using System.Globalization;
using System.Data.Objects.SqlClient;

namespace iCanSpeakServices.ServiceManager
{
    public class UserProfile
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataTable Table = new DataTable();
        DataTable ResultMessage = new DataTable();
        DataSet ds = new DataSet();

        public Stream GetUserProfileEdit(Stream objStream)
        {
            User objUser = new User();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();

            try
            {
                //requestString = "{\"userId\":\"8\"}";
                var userProfile = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int userid = Convert.ToInt32(Service.Decrypt(userProfile["UserId"]));
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                var userData = from userInfo in icanSpeakContext.Users
                               where userInfo.UserId == userid
                               select new
                                            {
                                            UserId = Service.Encrypt(userInfo.UserId.ToString()), 
                                            FirstName = textInfo.ToTitleCase(userInfo.FirstName),
                                            LastName = textInfo.ToTitleCase(userInfo.LastName),
                                            Country = textInfo.ToTitleCase(userInfo.Country),
                                            City = textInfo.ToTitleCase(userInfo.City),
                                            Gender = textInfo.ToTitleCase(userInfo.Gender),
                                            DOB = userInfo.DOB ,
                                            userInfo.ZipCode,
                                            AboutMe = Service.GetUrl() + "AboutMeAudio/" + userInfo.AboutMe,
                                            ProfilePicture = Service.GetUrl() + "ProfilePictures/" + userInfo.ProfilePicture 
                                           };

                if (userData != null)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(userData); ;
                    dt.TableName = "Profile";
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["Gender"].ToString() == "Child Male")
                        {
                            dr["Gender"] = dr["Gender"].ToString() == "Child Male" ? "Male" : (dr["Gender"].ToString());
                        }
                        else
                        {
                            dr["Gender"] = dr["Gender"].ToString() == "Child Female" ? "Female" : (dr["Gender"].ToString());
                        }

                        //dr["VocabularyBookmark"] = dr["VocabularyBookmark"].ToString() == "" ? "" : Service.Encrypt(dr["VocabularyBookmark"].ToString());
                        //dr["DialogBookmark"] = dr["DialogBookmark"].ToString() == "" ? "" : Service.Encrypt(dr["DialogBookmark"].ToString());
                        //dr["VocabId"] = dr["VocabId"].ToString() == "" ? "" : Service.Encrypt(dr["VocabId"].ToString());
                        //dr["VocabSubId"] = dr["VocabSubId"].ToString() == "" ? "" : Service.Encrypt(dr["VocabSubId"].ToString());
                    }
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
                Helper.ErrorLog(ex, "GetUserProfile");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream  GetUserProfile(Stream objStream)
        {
            User objUser = new User();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();

            try
            {
                //requestString = "{\"userId\":\"8\"}";
                var userProfile = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                int userid = Convert.ToInt32(userProfile["userId"]);
                var userData = from user in icanSpeakContext.Users
                               join cu in icanSpeakContext.Courses on user.UserId equals cu.UserId
                               into a
                               from b in a.DefaultIfEmpty()
                               join ic in icanSpeakContext.iCanSpeakCourses on b.iCanSpeakCourseId equals ic.CourseId
                               into a1
                               from b1 in a1.DefaultIfEmpty()
                               where user.UserId == userid
                               select new
                               {
                                   user.UserId,
                                   Username = textInfo.ToTitleCase(user.Username),
                                   FirstName = textInfo.ToTitleCase(user.FirstName),
                                   LastName = textInfo.ToTitleCase(user.LastName),
                                   user.Email,
                                   user.Password,
                                   Status = true,
                                   ProfilePicture = Service.GetUrl() + "ProfilePictures/" + user.ProfilePicture,
                                   user.RoleId,
                                   AboutMe = Service.GetUrl() + "AboutMeAudio/" + user.AboutMe,
                                   user.AccessToken,
                                   user.Age,
                                   user.CreatedDate,
                                   user.DeviceId,
                                   DOB = user.DOB,
                                   user.Gender,
                                   user.IsActive,
                                   user.LastLogin,
                                   user.Country,
                                   user.City,
                                   user.Messages,
                                   NotificationCount = user.BatchCount,
                                   CourseStartDate = b == null ? null : b.CreatedDate.ToShortDateString(),
                                   CourseType = b1.CourseType,
                                   CourseDuration = b1.Duration,
                                   EndDate = b.EndDate,
                                   Level = "",
                                   Performance = "",
                                   CourseDaysLeft = (b == null ? 0 : SqlFunctions.DateDiff("dd", b.EndDate, DateTime.Now))

                               };


                
                if (userData != null)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(userData); ;
                    dt.TableName = "Profile";
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["Gender"].ToString() == "Child Male")
                        {
                            dr["Gender"] = dr["Gender"].ToString() == "Child Male" ? "Male" : (dr["Gender"].ToString());
                        }
                        else
                        {
                            dr["Gender"] = dr["Gender"].ToString() == "Child Female" ? "Female" : (dr["Gender"].ToString());
                        }

                        //dr["VocabularyBookmark"] = dr["VocabularyBookmark"].ToString() == "" ? "" : Service.Encrypt(dr["VocabularyBookmark"].ToString());
                        //dr["DialogBookmark"] = dr["DialogBookmark"].ToString() == "" ? "" : Service.Encrypt(dr["DialogBookmark"].ToString());
                        //dr["VocabId"] = dr["VocabId"].ToString() == "" ? "" : Service.Encrypt(dr["VocabId"].ToString());
                        //dr["VocabSubId"] = dr["VocabSubId"].ToString() == "" ? "" : Service.Encrypt(dr["VocabSubId"].ToString());
                    }
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
                Helper.ErrorLog(ex, "GetUserProfile");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream EditUserProfile(Stream objStream)
        {
            User objUser = new User();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                //requestString = "{\"userId\":\"1\",\"accessToken\":\"xdoykz\",\"FirstName\":\"test fname\",\"LastName\":\"test lname\"}";
                var userProfile = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int userid = Convert.ToInt32(Service.Decrypt(userProfile["UserId"]));
                objUser = icanSpeakContext.Users.Single(u => u.UserId == userid);
                objUser.FirstName = userProfile["Firstname"];
                objUser.LastName = userProfile["Lastname"];
                objUser.DOB = Convert.ToDateTime(userProfile["DOB"]);
                objUser.ZipCode = userProfile["ZipCode"];
                objUser.Country = userProfile["Country"];
                if (userProfile["ProfilePicture"] != "")
                {
                    objUser.ProfilePicture = userProfile["ProfilePicture"];
                }
                else
                {
                    if (userProfile["imagebase64"] != "")
                    {
                        objUser.ProfilePicture = userid + "_profilepic.png";
                    }
                }
                icanSpeakContext.SubmitChanges();

                if (!string.IsNullOrEmpty(userProfile["audiobase64"]))
                {
                    string filePath = HostingEnvironment.MapPath("/AboutMeAudio/" + userid + "_aboutme.mp3");
                    if (File.Exists(filePath))
                    {
                        System.IO.File.Delete((filePath));
                    }
                    byte[] bytes = System.Convert.FromBase64String(userProfile["audiobase64"]);
                    FileStream fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }

                if (!string.IsNullOrEmpty(userProfile["imagebase64"]))
                {
                    string filePath = HostingEnvironment.MapPath("/ProfilePictures/" + userid + "_profilepic.png");
                    if (File.Exists(filePath))
                    {
                        System.IO.File.Delete((filePath));
                    }
                    byte[] bytes = System.Convert.FromBase64String(userProfile["imagebase64"]);
                    FileStream fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }


                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                ResultMessage = Service.ResultMessage("Profile update successfully !", "1");
                ResultMessage.AcceptChanges();
                ds.Tables.Add(Table);
                ds.Tables.Add(ResultMessage);
                string js1 = JsonConvert.SerializeObject(ds);
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));

                // return javaScriptSerializer.Serialize("Profile update successfully !");
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "EditUserProfile");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }


        }


        public Stream DeleteUser(Stream objStream)
        {
            try
            {
                User objUserTable = new User();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                string requestString = reader.ReadToEnd();
                // requestString = "{\"Email\":\"jhjh@gmail.com\"}";
                var givenDetails = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var query = from user in icanSpeakContext.Users
                            where user.Email == givenDetails["Email"]
                            select user.Email;

                if (query.FirstOrDefault() == givenDetails["Email"])
                {
                    objUserTable = icanSpeakContext.Users.Single(email => email.Email == givenDetails["Email"]);
                    icanSpeakContext.Users.DeleteOnSubmit(objUserTable);
                    icanSpeakContext.SubmitChanges();

                    var js = JsonConvert.SerializeObject("User Account Successfully Deleted", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    //return javaScriptSerializer.Serialize("User Account Successfully Deleted");
                }
                else
                {
                    var js = JsonConvert.SerializeObject("You enter an invalid Email Id to Delete User Account", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    //return javaScriptSerializer.Serialize("You enter an invalid Email Id to Delete User Account");
                }
            }
            catch (Exception ex)
            {
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream ChangePassword(Stream objStream)
        {
            User objUser = new User();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            DataSet ds = new DataSet("Result");

            try
            {
                //  requestString = "{\"Email\":\"mahendra@techvalens.com\",\"Password\":\"h\",\"NewPassword\":\"123456\"}";
                var userLogin = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int userid = Convert.ToInt32(Service.Decrypt(userLogin["userid"]));
                var query = from user in icanSpeakContext.Users
                            where user.UserId == userid 
                            && user.Password == userLogin["Password"]
                            select user.Password;

                if (Convert.ToString(query.FirstOrDefault()) == userLogin["Password"].ToString())
                {
                    //update
                    objUser = icanSpeakContext.Users.Single(user => user.UserId == userid);
                    objUser.Password = userLogin["NewPassword"].ToString();
                    icanSpeakContext.SubmitChanges();

                    MyActivity objmyactivity = new MyActivity();
                    objmyactivity.UserId = userid;
                    objmyactivity.Message = "You have changed your password";
                    objmyactivity.CreatedDate = System.DateTime.Now;
                    icanSpeakContext.MyActivities.InsertOnSubmit(objmyactivity);
                    icanSpeakContext.SubmitChanges();


                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    ResultMessage = Service.ResultMessage("changed", "1");
                    ResultMessage.AcceptChanges();
                    ds.Tables.Add(Table);
                    ds.Tables.Add(ResultMessage);
                    string js1 = JsonConvert.SerializeObject(ds);
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
  
                }
                else
                {
                    Table = Service.Message("error", "0");
                    Table.AcceptChanges();
                    ResultMessage = Service.ResultMessage("invalid", "1");
                    ResultMessage.AcceptChanges();
                    ds.Tables.Add(Table);
                    ds.Tables.Add(ResultMessage);
                    string js1 = JsonConvert.SerializeObject(ds);
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
            }
            catch (Exception ex)
            {

                Helper.ErrorLog(ex, "ChangePassword");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }

        public Stream ChangePasswordAdmin(Stream objStream)
        {
            SubAdmin objUser = new SubAdmin();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                //  requestString = "{\"Email\":\"mahendra@techvalens.com\",\"Password\":\"h\",\"NewPassword\":\"123456\"}";
                var userLogin = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var query = from user in icanSpeakContext.SubAdmins
                            where user.Email == userLogin["Email"]
                            && user.Password == userLogin["Password"]

                            select user.Password;

                if (Convert.ToString(query.FirstOrDefault()) == userLogin["Password"].ToString())
                {
                    //update
                    objUser = icanSpeakContext.SubAdmins.Single(user => user.Email == userLogin["Email"].ToString());
                    objUser.Password = userLogin["NewPassword"].ToString();
                    icanSpeakContext.SubmitChanges();

                    //var js = JsonConvert.SerializeObject("Password Changed Successfully", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Password Changed Successfully")));
                }
                else
                {
                    //var js = JsonConvert.SerializeObject("Password Changed Successfully", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("You enter wrong current password")));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "Login");

                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

    }
}