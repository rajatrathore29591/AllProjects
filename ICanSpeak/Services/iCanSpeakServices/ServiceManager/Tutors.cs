using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace iCanSpeakServices.ServiceManager
{
    public class Tutors
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataSet ds = new DataSet();
        DataTable Table = new DataTable();

        public Stream SaveStudentTutor(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                //StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                //String requestString = reader.ReadToEnd();
                //var data = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                //Tutor tutorobj = new Tutor();

                //tutorobj.TutorId = Convert.ToInt32(data["TutorId"]);
                //tutorobj.StudentUserId = Convert.ToInt32(data["StudentUserId"]);
                //icanSpeakContext.Tutors.InsertOnSubmit(tutorobj);
                //icanSpeakContext.SubmitChanges();
                //string[] subtutoridarray = data["SubTutorID"].Split('~');
                //foreach (string subtutorid in subtutoridarray)
                //{
                //    SubTutor objsubtutor = new SubTutor();
                //    objsubtutor.StudentUserId = Convert.ToInt32(data["StudentUserId"]);
                //    objsubtutor.SubTutorId = Convert.ToInt32(subtutorid);
                //    icanSpeakContext.SubTutors.InsertOnSubmit(objsubtutor);
                //    icanSpeakContext.SubmitChanges();
                //}
                var data = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var checkstudent = (from mapping in icanSpeakContext.TutorSubTutorMappings
                                    where mapping.StudentUserId == Convert.ToInt32(data["StudentUserId"]) &&
                                     mapping.TutorId == Convert.ToInt32(data["TutorId"])
                                    select new
                                    {
                                        mapping.TutorId,
                                        mapping.StudentUserId
                                    }).ToList();

                if (checkstudent.Count () > 0 && checkstudent != null)
                {

                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Failure")));   
                }
                else
                {
                    TutorSubTutorMapping tutorobj = new TutorSubTutorMapping();
                    tutorobj.TutorId = Convert.ToInt32(data["TutorId"]);
                    tutorobj.SubTutorId = Convert.ToInt32(data["SubTutorID"]);
                    tutorobj.StudentUserId = Convert.ToInt32(data["StudentUserId"]);
                    tutorobj.CreatedDate = System.DateTime.Now;
                    icanSpeakContext.TutorSubTutorMappings.InsertOnSubmit(tutorobj);
                    icanSpeakContext.SubmitChanges();
                    return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize("success")).Replace("\\/", "/")));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "UpdateSubAdmin");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetAllTutors(Stream objStream)
        {
            SubAdmin objtutor = new SubAdmin();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String responseString = reader.ReadToEnd();
            try
            {
                //var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(responseString);

                var TutorList = (from tutor in icanSpeakContext.SubAdmins
                                 where tutor.RoleId == 3
                                 select new {
                                     tutor.UserId,
                                     tutor.FirstName,
                                     tutor.LastName,
                                     tutor.Education,
                                     ProfilePicture = Service.GetUrl() + "ProfilePictures/" + tutor.ProfilePicture,
                                     tutor.Experience,
                                     tutor.Expertise,
                                     tutor.City,
                                     tutor.Email,
                                     tutor.ContactNo,                                    
                                 }).ToList();

                if (TutorList.Count() > 0 && TutorList != null)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(TutorList); ;
                    dt.TableName = "Tutors";
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
                Helper.ErrorLog(ex, "GetAllTutors");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }

        public Stream GetTutorsByUserId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String responseString = reader.ReadToEnd();
            try
            {
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(responseString);
                int userid = Convert.ToInt32(Service.Decrypt(userRequest["userid"]));
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                var userList = (from users in icanSpeakContext.Users
                                join tutor in icanSpeakContext.TutorSubTutorMappings on users.UserId equals tutor.StudentUserId
                                join admin in icanSpeakContext.SubAdmins on tutor.TutorId equals admin.UserId
                                where tutor.StudentUserId == userid
                                select new
                                {
                                    UserId = Service.Encrypt(users.UserId.ToString()),
                                    users.Email,
                                    Country = textInfo.ToTitleCase(users.Country),
                                    users.CreatedDate,
                                    FirstName = textInfo.ToTitleCase(admin.FirstName),
                                    LastName = textInfo.ToTitleCase(admin.LastName),
                                    admin.Gender,
                                    DOB = admin.DOB,
                                    admin.ContactNo,
                                    users.IsActive,
                                    admin.Experience,
                                    users.Specialisation,
                                    users.Description,
                                    Name = admin.FirstName
                                }).ToList();

                if (userList.Count() > 0 && userList != null)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(userList); ;
                    dt.TableName = "Tutors";
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
                Helper.ErrorLog(ex, "GetAllTutors");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }

        public Stream GetAllTutor(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String responseString = reader.ReadToEnd();
            try
            {
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(responseString);
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                var userList = (from users in icanSpeakContext.Users
                                where users.RoleId == 3 || users.RoleId == 6
                                select new
                                {
                                    UserId = Service.Encrypt(users.UserId.ToString()),
                                    users.Email,
                                    Country = textInfo.ToTitleCase(users.Country),
                                    users.CreatedDate,
                                    FirstName = textInfo.ToTitleCase(users.FirstName),
                                    LastName = textInfo.ToTitleCase(users.LastName),
                                    users.Gender,
                                    DOB = users.DOB,
                                    users.Phone,
                                    ProfilePicture = Service.GetUrl() + "ProfilePictures/" + users.ProfilePicture,
                                    users.IsActive,
                                    users.Experience,
                                    users.Specialisation,
                                    users.Description
                                }).ToList();

                if (userList.Count() > 0 && userList != null)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(userList); ;
                    dt.TableName = "Tutors";
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
                Helper.ErrorLog(ex, "GetAllTutors");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }

        public Stream GetStudentByUserId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String responseString = reader.ReadToEnd();
            try
            {
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(responseString);
                //int userid = Convert.ToInt32(userRequest["email"]);
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                var userid = (from tutor in icanSpeakContext.SubAdmins
                              where tutor.Email == userRequest["email"]
                              select new { tutor.UserId }).FirstOrDefault();

                int Uid = userid.UserId;
                var userList = (from users in icanSpeakContext.Users
                                join tutor in icanSpeakContext.TutorSubTutorMappings on users.UserId equals tutor.StudentUserId
                                //join mark in icanSpeakContext.Marks on users.UserId equals mark.StudentId
                                where tutor.TutorId == Uid
                                select new
                                {
                                    UserId = users.UserId,
                                    TutorId = Uid,
                                    users.Email,
                                    Country = textInfo.ToTitleCase(users.Country),
                                    users.CreatedDate,
                                    FirstName = textInfo.ToTitleCase(users.FirstName),
                                    LastName = textInfo.ToTitleCase(users.LastName),
                                    users.Gender,
                                    DOB = users.DOB,
                                    users.Phone,
                                    ProfilePicture = Service.GetUrl() + "ProfilePictures/" + users.ProfilePicture,
                                    users.IsActive,
                                    users.Experience,
                                    users.Specialisation,
                                    users.Description,
                                    Status = Status(Uid, users.UserId)
                                }).ToList();


                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                DataTable dt = Service.ConvertToDataTable(userList); ;
                dt.TableName = "Student";
                ds.Tables.Add(Table);
                ds.Tables.Add(dt);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetStudentByUserId");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }

        public string Status(int userid, int studentsid)
        {
            var status = (from mark in icanSpeakContext.Marks where mark.TutorId == userid && mark.StudentId == studentsid select new { mark.IsMark }).FirstOrDefault();

            if (status != null)
            {
                return status.IsMark.ToString();
            }
            else
            {
                return "False";
            }

        }

        public Stream AddStudentByTutorId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int TutorId = Convert.ToInt32(userRequest["TutorId"]);
                int userID = Convert.ToInt32(userRequest["StudentId"]);


                Mark objmark = new Mark();

                objmark.TutorId = TutorId;
                objmark.StudentId = Convert.ToInt32(userRequest["StudentId"]);
                objmark.Marks = userRequest["Marks"];
                objmark.TotalScore = userRequest["TotalScore"];
                objmark.CreatedDate = System.DateTime.Now;
                objmark.IsMark = Convert.ToBoolean("True");
                icanSpeakContext.Marks.InsertOnSubmit(objmark);
                icanSpeakContext.SubmitChanges();


                var UserData = (from user in icanSpeakContext.Users
                                where user.UserId == userID
                                select
                                user).FirstOrDefault();


                //==============If Answer Correct, Increment User My Score Starts=======================================================================\\
                if (userRequest["Marks"] != null)
                {
                    UserData.MyScore = UserData.MyScore + Convert.ToInt32(objmark.Marks);
                }
                //==============If Answer Correct, Increment User My Score Ends=======================================================================\\
                if (UserData.TotalScore == null)
                {
                    UserData.TotalScore = Convert.ToInt32(userRequest["TotalScore"]);
                }
                else
                {
                    UserData.TotalScore = UserData.TotalScore + Convert.ToInt32(userRequest["TotalScore"]);
                }
                icanSpeakContext.SubmitChanges();


                Notification objnotification = new Notification();
                objnotification.UserId = Convert.ToInt32(userRequest["StudentId"]);
                objnotification.Message = "Your tutor scored you marks " + userRequest["Marks"] + "out of 100 marks";
                objnotification.CreatedDate = System.DateTime.Now;
                icanSpeakContext.Notifications.InsertOnSubmit(objnotification);
                icanSpeakContext.SubmitChanges();


                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Added Successfully")));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddStudentByTutorId");
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message)).Replace("\\/", "/")));
            }
        }
    }
}