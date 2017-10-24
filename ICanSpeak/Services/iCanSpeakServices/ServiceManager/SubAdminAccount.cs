using iCanSpeakServices.HelperClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using System.Data;
using System.ServiceModel.Web;
namespace iCanSpeakServices.ServiceManager
{
    public class SubAdminAccount
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();

        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataSet ds = new DataSet();
        SaveImage objsaveimage = new SaveImage();
        public Stream CreateSubAdmin(Stream objStream)
        {
           
            try
            {
                
                SubAdmin objSubAdmin = new SubAdmin();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                // requestString =  "{\"email\":\"231gg1\",\"DOB\":\"2014-12-31\",\"firstName\":\"ggg\",\"lastName\":\"gg\",\"password\":\"gg\",\"gender\":\"\",\"profilePicture\":\"\",\"roleId\":\"3\",\"contactNo\":\"gg\",\"education\":\"gg\",\"experience\":\"gg\",\"expertise\":\"\"}";


                //requestString = "{\"email\":\"ggg\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var subAdminRegistration = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var emailCheck = icanSpeakContext.SubAdmins.Any(email => email.Email == subAdminRegistration["email"]);
                if (emailCheck == true)
                {
                    var js = JsonConvert.SerializeObject("Please choose a different email ! This one is already in use", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    //return javaScriptSerializer.Serialize("Please choose a different email ! This one is already in use");
                }

                else
                {
                    var maxuserId = icanSpeakContext.SubAdmins.ToList().Max(U => U.UserId);
                    int userid = maxuserId + 1;
                    //int age = Convert.ToInt32(Math.Round(DateTime.Now.Subtract(Convert.ToDateTime(subAdminRegistration["DOB"])).TotalDays * 0.00273790926));
                    objSubAdmin.FirstName = subAdminRegistration["firstName"];
                    objSubAdmin.LastName = subAdminRegistration["lastName"];
                    objSubAdmin.Email = subAdminRegistration["email"];
                    objSubAdmin.Password = subAdminRegistration["password"];
                    objSubAdmin.DOB = Convert.ToDateTime(subAdminRegistration["DOB"]);
                    objSubAdmin.Gender = subAdminRegistration["gender"];
                    objSubAdmin.IsActive = true;
                    objSubAdmin.RoleId = Convert.ToInt32(subAdminRegistration["roleId"]);
                    objSubAdmin.AccessToken = Helper.RandomString(6);
                    objSubAdmin.City = subAdminRegistration["city"];
                    objSubAdmin.ZipCode = subAdminRegistration["zipcode"];
                    objSubAdmin.ContactNo = subAdminRegistration["contactNo"];
                    objSubAdmin.Education = subAdminRegistration["education"];
                    objSubAdmin.Experience = subAdminRegistration["experience"];
                    objSubAdmin.Expertise =  subAdminRegistration["expertise"];
                    
                    objSubAdmin.ProfilePicture = userid + "_ProfilePic.jpg";
                    objSubAdmin.CreatedDate = System.DateTime.Now;
                    icanSpeakContext.SubAdmins.InsertOnSubmit(objSubAdmin);
                    icanSpeakContext.SubmitChanges();




                    var js = JsonConvert.SerializeObject(userid, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    //if (string.IsNullOrEmpty(subAdminRegistration["profilePicture"]))
                    //{
                    //    var bytes = Convert.FromBase64String(ConfigurationManager.AppSettings["defaultImage"]);
                    //    MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
                    //    ms.Write(bytes, 0, bytes.Length);
                    //    Image image = Image.FromStream(ms);
                    //    // filePath = "http://lla.techvalens.net/Services/ProfilePictures/" + userId + "_ProfilePic.jpg";
                    //    image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //}
                    //else
                    //{
                    //    var bytes = Convert.FromBase64String(subAdminRegistration["profilePicture"]);
                    //    MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
                    //    ms.Write(bytes, 0, bytes.Length);
                    //    Image image = Image.FromStream(ms);
                    //    // filePath = "http://lla.techvalens.net/Services/ProfilePictures/" + userId + "_ProfilePic.jpg";
                    //    image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //}

                    
                    //var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    //return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "CreateSubAdmin");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream CreateTutorSubTutor(Stream objStream)
        {

            try
            {
                SubAdmin objSubAdmin = new SubAdmin();
                User objUser = new User();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                // requestString =  "{\"email\":\"231gg1\",\"DOB\":\"2014-12-31\",\"firstName\":\"ggg\",\"lastName\":\"gg\",\"password\":\"gg\",\"gender\":\"\",\"profilePicture\":\"\",\"roleId\":\"3\",\"contactNo\":\"gg\",\"education\":\"gg\",\"experience\":\"gg\",\"expertise\":\"\"}";


                //requestString = "{\"email\":\"ggg\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var subAdminRegistration = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var emailCheck = icanSpeakContext.SubAdmins.Any(email => email.Email == subAdminRegistration["email"]);
                if (emailCheck == true)
                {
                    var js = JsonConvert.SerializeObject("Please choose a different email ! This one is already in use", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    //return javaScriptSerializer.Serialize("Please choose a different email ! This one is already in use");
                }

                else
                {
                    var maxuserId = icanSpeakContext.SubAdmins.ToList().Max(U => U.UserId);
                    int userid = maxuserId + 1;
                    objSubAdmin.RoleId = Convert.ToInt32(subAdminRegistration["roleId"]);
                    objSubAdmin.FirstName = subAdminRegistration["firstName"];
                    objSubAdmin.LastName = subAdminRegistration["lastName"];
                    objSubAdmin.ProfilePicture = userid + "_ProfilePic.jpg";
                    objSubAdmin.Email = subAdminRegistration["email"];
                    objSubAdmin.Password = subAdminRegistration["password"];
                    objSubAdmin.DOB = Convert.ToDateTime(subAdminRegistration["DOB"]);
                    objSubAdmin.Gender = subAdminRegistration["gender"];
                    objSubAdmin.IsActive = true;
                    objSubAdmin.Education = subAdminRegistration["education"];
                    objSubAdmin.Experience = subAdminRegistration["experience"];
                    objSubAdmin.Expertise = subAdminRegistration["expertise"];
                    objSubAdmin.AccessToken = Helper.RandomString(6);
                    objSubAdmin.City = subAdminRegistration["city"];
                    objSubAdmin.ContactNo = subAdminRegistration["contactNo"];
                    objSubAdmin.Description = subAdminRegistration["Description"];
                    objSubAdmin.CreatedDate = System.DateTime.Now;
                    icanSpeakContext.SubAdmins.InsertOnSubmit(objSubAdmin);
                    icanSpeakContext.SubmitChanges();




                    var js = JsonConvert.SerializeObject(userid, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "CreateSubAdmin");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream AdminLogin(Stream objStream)
        {
            try
            {
                SubAdmin objSubAdmin = new SubAdmin();

                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
               // requestString = "{\"email\":\"dfhg\",\"password\":\"dgjdgidgd\"}";
                var userLogin = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                
                var emailStatus = (from user in icanSpeakContext.SubAdmins
                                   where user.Email == userLogin["email"]
                                   select user.Email).FirstOrDefault();
                
                if (emailStatus == null)
                {
                    //var js = JsonConvert.SerializeObject("Invalid email");
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Invalid email")));
                }

                var passwordStatus = (from user in icanSpeakContext.SubAdmins
                                      where user.Password == userLogin["password"] && user.Email == userLogin["email"]
                                      select user.Email).FirstOrDefault();

                if (passwordStatus == null)
                {
                    //var js = JsonConvert.SerializeObject("Invalid password");
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Invalid password")));
                }

                var activeStatus = (from user in icanSpeakContext.SubAdmins
                                    where user.Email == userLogin["email"] && user.Password == userLogin["password"]
                                    select user.IsActive).FirstOrDefault();

                if (activeStatus.Value == false)
                {
                    //var js = JsonConvert.SerializeObject("Account inactive contact to admin");
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Account inactive contact to admin")));
                }


                var result = (from user in icanSpeakContext.SubAdmins
                              where user.Email == userLogin["email"]
                              && user.Password == userLogin["password"]
                              select new { user.UserId, user.FirstName, user.LastName, user.Email, user.Password, user.ProfilePicture, user.RoleId, user.AccessToken, user.Age, user.City, user.CreatedDate, user.DOB, user.Gender, user.IsActive, user.LastLogin }).FirstOrDefault();


                var js1 = JsonConvert.SerializeObject(result);
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AdminLogin");
                return new MemoryStream(Encoding.UTF8.GetBytes(ex.Message.ToString())); ;// javaScriptSerializer.Serialize(ex.Message.ToString());


            }

        }

        public Stream UpdateSubAdmin(Stream objStream)
        {
            try
            {
                SubAdmin objSubAdmin = new SubAdmin();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"userId\":\"1\",\"softDelete\":\"true\"}";
                var subAdminRegistration = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from user in icanSpeakContext.SubAdmins
                              where user.UserId == Convert.ToInt32(subAdminRegistration["userid"])
                              select user).FirstOrDefault();

                if (result.UserId > 0)
                {
                    result.FirstName = subAdminRegistration["firstName"];
                    result.LastName = subAdminRegistration["lastName"];
                    result.Email = subAdminRegistration["email"];
                    result.Password = subAdminRegistration["password"];
                    result.DOB = Convert.ToDateTime(subAdminRegistration["DOB"]);
                    result.Gender = subAdminRegistration["gender"];
                    result.RoleId = Convert.ToInt32(subAdminRegistration["roleId"]);
                    result.AccessToken = Helper.RandomString(6);
                    result.ContactNo = subAdminRegistration["contactNo"];
                    result.City = subAdminRegistration["city"];
                    result.ZipCode = subAdminRegistration["zipcode"];
                    result.Education = subAdminRegistration["education"];
                    result.Experience = subAdminRegistration["experience"];
                    result.Expertise = subAdminRegistration["expertise"];
                    result.ModifiedDate = System.DateTime.Now;

                    //string filePath = HostingEnvironment.MapPath("~/ProfilePictures/" + result.UserId + "_ProfilePic.jpg");

                    //var Query = icanSpeakContext.SubAdmins.Where(u => u.UserId == result.UserId).SingleOrDefault();
                    //Query.ProfilePicture = "http://lla.techvalens.net/ProfilePictures/" + result.UserId + "_ProfilePic.jpg";

                    //if (!string.IsNullOrEmpty(subAdminRegistration["profilePicture"]))
                    //{
                    //    var bytes = Convert.FromBase64String(subAdminRegistration["profilePicture"]);
                    //    MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
                    //    ms.Write(bytes, 0, bytes.Length);
                    //    Image image = Image.FromStream(ms);
                    //    image.Save(filePath);
                    //}

                    icanSpeakContext.SubmitChanges();

                    var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {

                    var js = JsonConvert.SerializeObject("Error in updation", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "UpdateSubAdmin");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetTutorSubTutorList(Stream objStream)
        {
            try
            {
                SubAdmin objSubAdmin = new SubAdmin();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"userId\":\"1\",\"softDelete\":\"true\"}";
                var userid = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var tutorlist = from tutor in icanSpeakContext.SubAdmins where tutor.RoleId==3 && tutor.IsActive == true
                                 select new {tutor.UserId,Name=tutor.FirstName+" "+tutor.LastName  };

                var subtutorlist = from subtutor in icanSpeakContext.SubAdmins where subtutor.RoleId==6 && subtutor.IsActive == true
                                   select new { subtutor.UserId, Name = subtutor.FirstName + " " + subtutor.LastName };

                if (tutorlist != null )
                {
                    DataTable tutortable = Service.ConvertToDataTable(tutorlist); ;
                    tutortable.TableName = "Tutor";
                    ds.Tables.Add(tutortable);
                
                }

                if (subtutorlist != null)
                {
                    DataTable subtutortable = Service.ConvertToDataTable(subtutorlist); ;
                    subtutortable.TableName = "SubTutor";
                    ds.Tables.Add(subtutortable);

                }

                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
              
              
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "UpdateSubAdmin");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream DeleteSubAdmin(Stream objStream)
        {

            try
            {
                SubAdmin objSubAdmin = new SubAdmin();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"userId\":\"8\",\"softDelete\":\"true\"}";
                var subAdminRegistration = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from user in icanSpeakContext.SubAdmins
                              where user.UserId == Convert.ToInt32(subAdminRegistration["userId"])
                              select user).FirstOrDefault();


                if (subAdminRegistration["softDelete"] == "true")
                {
                    if (result.IsActive == true)
                    {
                        result.IsActive = false;
                    }
                    else
                    {
                        result.IsActive = true;
                    }
                }
                else
                {

                    icanSpeakContext.SubAdmins.DeleteOnSubmit(result);
                }
                icanSpeakContext.SubmitChanges();
                // return javaScriptSerializer.Serialize("User deleted successfully");
                var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteSubAdmin");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream DeleteSubUserById(Stream objStream)
        {

            try
            {
                SubAdmin objSubAdmin = new SubAdmin();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"userId\":\"8\",\"softDelete\":\"true\"}";
                var subAdminRegistration = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from user in icanSpeakContext.SubAdmins
                              where user.UserId == Convert.ToInt32(subAdminRegistration["userid"])
                              select user).FirstOrDefault();
                icanSpeakContext.SubAdmins.DeleteOnSubmit(result);
                icanSpeakContext.SubmitChanges();
                // return javaScriptSerializer.Serialize("User deleted successfully");
                var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteSubAdmin");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetAllSubAdminUsersList()
        {
            try
            {
                


                var result = ((from user in icanSpeakContext.Users
                               where user.RoleId == 3 || user.RoleId == 6
                                     select new { user.UserId ,
                                       user.FirstName,
                                       user.LastName,
                                       user.Email,
                                       user.IsActive,
                                       user.Gender,
                                       user.City,
                                       user.CreatedDate,
                                       RoleId = user.RoleId.ToString()
                                       }
                         ).Union(
                         from tutor in icanSpeakContext.SubAdmins
                         select new { 
                         tutor.UserId,
                         tutor.FirstName,
                         tutor.LastName,
                         tutor.Email,
                         tutor.IsActive,
                         tutor.Gender,
                         tutor.City,
                         tutor.CreatedDate,
                         RoleId = tutor.RoleId.ToString()
                         }
                         )).OrderByDescending(x=>x.CreatedDate) ;

                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(result)).Replace("\\/", "/")));

                //var js = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                //return new MemoryStream(Encoding.UTF8.GetBytes(js.Replace("\\/", "/")));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAllSubAdminUsersList");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetSubAdminByUserId(Stream objStream)
        {
            try
            {
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = from user in icanSpeakContext.SubAdmins
                             where user.UserId == Convert.ToInt32(userRequest["userid"])
                             select new { user.UserId, 
                                          user.FirstName,
                                          user.LastName,
                                          user.Email,
                                          user.ContactNo,
                                          user.IsActive,
                                          user.Gender,
                                          user.CreatedDate,
                                          user.ZipCode,
                                          user.State,
                                          user.Password,
                                          user.Expertise,
                                          user.Experience,
                                          user.Education,
                                          user.DOB,
                                          user.City,
                                          user.RoleId };

                var js = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js.Replace("\\/", "/")));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAllSubAdminUsersList");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream ForgotPasswordAdmin(Stream objStream)
        {
            try
            {
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                var forgotpassword = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var emailExists = (from subadmin in icanSpeakContext.SubAdmins
                                  where subadmin.Email == forgotpassword["email"]
                                  select new { subadmin.Email }).ToList();

                if (emailExists.Count>0)
                {

                    MailMessage message = new MailMessage();
                    string Subject = string.Empty;
                    string Body = string.Empty;
                    bool check;
                    var newPassword = Helper.RandomString(6);
                    if (!string.IsNullOrEmpty(forgotpassword["email"]))
                    {
                        var userName = from user in icanSpeakContext.SubAdmins
                                       where user.Email == forgotpassword["email"]
                                       select user.FirstName + " " + user.LastName;

                        message = new MailMessage();
                        Subject = "Your  password has been changed.";
                        Body = @"<table border='0' cellpadding='0' cellspacing='0'><tr><td>Hello  " + userName.FirstOrDefault() +
                          "<br/><br/> Your  password has been changed.<br/><br/>Your new password is <b>" + newPassword + "</b><br/><br/>" +
                          "Once you have logged in, please head on over to profile/reset password to change your temporary password.</td></tr></table>";
                        message.Subject = Subject;
                        message.IsBodyHtml = true;
                        message.Body = Body;
                        message.To.Add(forgotpassword["email"]);
                        EmailSendUtility SendEmail = new EmailSendUtility();
                        check = SendEmail.SendMail(Subject, Body, forgotpassword["email"]);
                        if (check != true)
                        {
                         return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Failed to send email, please try after some time later")));
                        }

                        SubAdmin objuser = icanSpeakContext.SubAdmins.Single(u => u.Email == forgotpassword["email"]);
                        objuser.Password = newPassword;
                        icanSpeakContext.SubmitChanges();
                    }

                    //var js1 = JsonConvert.SerializeObject(Service.StringToJsonConvertor("Password sent"), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Password sent")));
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Invalid E-mail    ")));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "ForgotPasswordAdmin");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
                //   var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            }
        }
    }
}