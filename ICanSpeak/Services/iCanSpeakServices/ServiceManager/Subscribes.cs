using iCanSpeakServices.HelperClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

using System.Web.Script.Serialization;
using System.Net.Mail;

namespace iCanSpeakServices.ServiceManager
{
    public class Subscribes
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataSet ds = new DataSet("Result");
        DataTable Table = new DataTable();

        public Stream AddSubscribeType(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                Subscribe objSubscribe = new Subscribe();
                //  requestString = "{\"UnitNameEnglish\":\"test 1\",\"UnitNameArabic\":\"test 1\",\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\",\"PPTUrl\":\"www.google.com\",\"VideoUrl\":\"www.google.com\",\"Price\":\"3\",\"Duration\":\"1 Month\",\"AssessmentSlots\":\"2.35,4,5,6,8.2,9\",\"DescriptionEnglish\":\"test english\",\"DescriptionArabic\":\"test arabic\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var check = (from type in icanSpeakContext.Subscribes
                             where type.Email == userRequest["email"]
                                 select new { 
                                     type.SubscribeId,
                                     type.Email,
                                     type.IsSubscribe
                                 
                                 }).ToList();

                if (check.Count == 0)
                {
                    objSubscribe = new Subscribe();

                    objSubscribe.Email = userRequest["email"];
                    objSubscribe.IsSubscribe = Convert.ToBoolean(userRequest["isSubscribe"]);
                    icanSpeakContext.Subscribes.InsertOnSubmit(objSubscribe);
                    icanSpeakContext.SubmitChanges();

                    MailMessage message = new MailMessage();
                    string Subject = string.Empty;
                    string Body = string.Empty;
                    bool check1;
                    var newPassword = Helper.RandomString(6);

                    var userName = (from user in icanSpeakContext.Users
                                    where user.Email == userRequest["email"]
                                    select new { Name = user.FirstName + " " + user.LastName }).FirstOrDefault();

                    if (userName != null)
                    {
                        message = new MailMessage();
                        Subject = "News Letter.";
                        Body = @"<table border='0' cellpadding='0' cellspacing='0'><tr><td>Dear subscriber,  " +
                            "<br/><br/>  Welcome to I Can Speak newsletter :)   <br/><br/>" +
                          "We are happy that you decided to join our successful community of English learners. Stay tuned for our valuable lessons, offers and tips to master speaking English!</br><br/>Best regards,</br></br>I Can Speak Team </td></tr></table>";
                        message.Subject = Subject;
                        message.IsBodyHtml = true;
                        message.Body = Body;
                        message.To.Add(userRequest["email"]);
                        EmailSendUtility SendEmail = new EmailSendUtility();
                        check1 = SendEmail.SendMail(Subject, Body, userRequest["email"]);
                        if (check1 != true)
                        {

                            DataTable Table1 = Service.Message("error", "0");
                            Table1.AcceptChanges();
                            ds.Tables.Add(Table1);
                            string js2 = JsonConvert.SerializeObject(ds);
                            return new MemoryStream(Encoding.UTF8.GetBytes(js2));
                        }
                        else
                        {
                            //User objuser = icanSpeakContext.Users.Single(u => u.Email == userRequest["email"]);
                            //objuser.Password = newPassword;
                            //icanSpeakContext.SubmitChanges();
                            DataTable Table1 = Service.Message("Email sent", "1");
                            Table1.AcceptChanges();
                            ds.Tables.Add(Table1);
                            string js2 = JsonConvert.SerializeObject(ds);
                            return new MemoryStream(Encoding.UTF8.GetBytes(js2));
                        }
                    }
                    //return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
                }
                //else
                //{
                //    objSubscribe = icanSpeakContext.Subscribes.Single(key => key.SubscribeId == check[0].SubscribeId);
                //    objSubscribe.Email = userRequest["email"];
                //    objSubscribe.IsSubscribe = Convert.ToBoolean(userRequest["isSubscribe"]);
                //    icanSpeakContext.SubmitChanges();

                //    //return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
                //}
                Table = Service.Message("success", "1");
                Table.AcceptChanges();
               // DataTable ResultMessage = Service.ResultMessage("Marked", "1");
                ds.Tables.Add(Table);
               // ds.Tables.Add(ResultMessage);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                //return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("error")));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddSubscribeType");
                //WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message)).Replace("\\/", "/")));
            }
        }
    }
}