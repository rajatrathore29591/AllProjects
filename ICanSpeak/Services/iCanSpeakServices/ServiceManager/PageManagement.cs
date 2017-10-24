using iCanSpeakServices.HelperClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Data.Objects.SqlClient;
using System.Data;
using System.ServiceModel.Web;

namespace iCanSpeakServices.ServiceManager
{
    public class PageManagement
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataSet ds = new DataSet();
        DataTable Table = new DataTable();
        SaveImage objsaveimage = new SaveImage();

        public Stream AddUpdateHelpContent(Stream objStream)
        {
            try
            {
                Help help = new Help();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                //  requestString = "{\"helpId\":\"1\",\"title\":\"HelpContent\",\"screenName\":\"Profile\",\"description\":\"This is help content related to profile\"}";
                // requestString = "{\"helpId\":\"0\"}";
                var content = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                if (content["operation"] == "select")
                {
                    var result = (from helpcontent in icanSpeakContext.Helps
                                  select new { Signup = helpcontent.SignupContent, Practice = helpcontent.PracticeContent, Friends = helpcontent.FriendsContent, Payment = helpcontent.PaymentContent, Tutors = helpcontent.TutorsContent });
                    var js = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                if (content["operation"] == "update")
                {
                    var helpResult = (from data in icanSpeakContext.Helps
                                      select data).FirstOrDefault();
                    if (content["ContentType"] == "Signup and login")
                    {
                        helpResult.SignupContent = content["Content"];
                    }
                    if (content["ContentType"] == "Practice & Review")
                    {
                        helpResult.PracticeContent = content["Content"];
                    }
                    if (content["ContentType"] == "Make Friends")
                    {
                        helpResult.FriendsContent = content["Content"];
                    }
                    if (content["ContentType"] == "Payment")
                    {
                        helpResult.PaymentContent = content["Content"];
                    }
                    if (content["ContentType"] == "Tutors")
                    {
                        helpResult.TutorsContent = content["Content"];
                    }
                    icanSpeakContext.SubmitChanges();

                }
                //var js1 = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddUpdateHelpContent");

                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }


        }


        public Stream GetSettingContent(Stream objStream)
        {

            //Exception ex1 = new Exception();
            //Helper.ErrorLog(ex1, requestString);
            DataSet ds = new DataSet("Result");
            try
            {
                Help help = new Help();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"email\":\"bhishamraj.rathore@techvalens.com\",\"password\":\"wf125h\"}";
                var userLogin = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from helpcontent in icanSpeakContext.Helps
                              select new
                              {
                                  Aboutus = helpcontent.AboutContent,
                                  PrivacyPolicy = helpcontent.PrivacyPolicyContent,
                                  TermsCondition = helpcontent.TermsConditionContent,
                                  helpcontent.Faq
                              });

                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                DataTable dt = Service.ConvertToDataTable(result); ;
                dt.TableName = "SettingContent";
                ds.Tables.Add(Table);
                ds.Tables.Add(dt);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {

                Helper.ErrorLog(ex, "GetHelpContent");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));// javaScriptSerializer.Serialize(ex.Message.ToString());


            }

        }


        public Stream GetHelpContent(Stream objStream)
        {

            //Exception ex1 = new Exception();
            //Helper.ErrorLog(ex1, requestString);
            DataSet ds = new DataSet("Result");
            try
            {
                Help help = new Help();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"email\":\"bhishamraj.rathore@techvalens.com\",\"password\":\"wf125h\"}";
                var userLogin = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from helpcontent in icanSpeakContext.Helps
                              select new
                              {
                                  Signup = helpcontent.SignupContent,
                                  Practice = helpcontent.PracticeContent,
                                  Friends = helpcontent.FriendsContent,
                                  Payment = helpcontent.PaymentContent,
                                  Tutors = helpcontent.TutorsContent
                              });

                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                DataTable dt = Service.ConvertToDataTable(result); ;
                dt.TableName = "Help";
                ds.Tables.Add(Table);
                ds.Tables.Add(dt);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {

                Helper.ErrorLog(ex, "GetHelpContent");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));// javaScriptSerializer.Serialize(ex.Message.ToString());


            }

        }

        public Stream AddUpdateAboutContent(Stream objStream)
        {
            try
            {
                About about = new About();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                //requestString = "{\"aboutId\":\"1\",\"title\":\"HelpContent\",\"screenName\":\"Profile\",\"description\":\"This is help content related to profile\",\"operation\":\"select\"}";
                // requestString = "{\"helpId\":\"0\"}";
                var content = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                ////////mahenra block start//
                if (content["operation"] == "select")
                {
                    var result = (from abouttbl in icanSpeakContext.Abouts
                                  select new { abouttbl.Title, abouttbl.Description });
                    var js = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                if (content["operation"] == "update")
                {
                    var aboutResult = (from data in icanSpeakContext.Abouts
                                       //           where data.AboutId == Convert.ToInt32(content["aboutId"])
                                       select data).FirstOrDefault();

                    aboutResult.Title = content["title"];
                    aboutResult.ScreenName = content["screenName"];
                    aboutResult.Description = content["description"];
                    icanSpeakContext.SubmitChanges();

                }
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));


                ////////mahenra block end//
                //if (content["aboutId"] == "0")
                //{
                //    about.Title = content["title"];
                //    about.ScreenName = content["screenName"];
                //    about.Description = content["description"];
                //    icanSpeakContext.Abouts.InsertOnSubmit(about);

                //    icanSpeakContext.SubmitChanges();

                //    var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                //    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                //}
                //else
                //{
                //    //var aboutResult = (from data in icanSpeakContext.Abouts
                //    //                   where data.AboutId == Convert.ToInt32(content["aboutId"])
                //    //                   select data).FirstOrDefault();
                //    //if (aboutResult.AboutId > 0)
                //    //{

                //    //    aboutResult.Title = content["title"];
                //    //    aboutResult.ScreenName = content["screenName"];
                //    //    aboutResult.Description = content["description"];
                //    //    icanSpeakContext.SubmitChanges();
                //    //    var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                //    //    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                //    //}
                //    else
                //    {
                //        var js = JsonConvert.SerializeObject("Error", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                //        return new MemoryStream(Encoding.UTF8.GetBytes(js));
                //    }
                //  }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AboutContent");

                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream AddUpdateFaqContent(Stream objStream)
        {
            try
            {
                Faq faq = new Faq();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                var content = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                ////////mahenra block start//
                if (content["operation"] == "select")
                {
                    var result = (from faqs in icanSpeakContext.Faqs
                                  select new { faqs.Title, faqs.Description });
                    var js = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                if (content["operation"] == "update")
                {
                    var aboutResult = (from data in icanSpeakContext.Faqs
                                       //           where data.AboutId == Convert.ToInt32(content["aboutId"])
                                       select data).FirstOrDefault();

                    aboutResult.Title = content["title"];
                    aboutResult.ScreenName = content["screenName"];
                    aboutResult.Description = content["description"];
                    icanSpeakContext.SubmitChanges();

                }
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));


                ////////mahenra block end//
                //if (content["aboutId"] == "0")
                //{
                //    about.Title = content["title"];
                //    about.ScreenName = content["screenName"];
                //    about.Description = content["description"];
                //    icanSpeakContext.Abouts.InsertOnSubmit(about);

                //    icanSpeakContext.SubmitChanges();

                //    var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                //    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                //}
                //else
                //{
                //    //var aboutResult = (from data in icanSpeakContext.Abouts
                //    //                   where data.AboutId == Convert.ToInt32(content["aboutId"])
                //    //                   select data).FirstOrDefault();
                //    //if (aboutResult.AboutId > 0)
                //    //{

                //    //    aboutResult.Title = content["title"];
                //    //    aboutResult.ScreenName = content["screenName"];
                //    //    aboutResult.Description = content["description"];
                //    //    icanSpeakContext.SubmitChanges();
                //    //    var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                //    //    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                //    //}
                //    else
                //    {
                //        var js = JsonConvert.SerializeObject("Error", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                //        return new MemoryStream(Encoding.UTF8.GetBytes(js));
                //    }
                //  }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AboutContent");

                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }


        public Stream GetAllFAQ1s(Stream objStream)
        {
            Faq objDialog = new Faq();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                var questionlist = (from faq in icanSpeakContext.Faqs
                                    select new { faq.FaqId, faq.Title, faq.Description, faq.CreateDate, faq.ScreenName }).ToList();
                if (questionlist.Count() > 0 && questionlist != null)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(questionlist); ;
                    dt.TableName = "FAQList";
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
                Helper.ErrorLog(ex, "AboutContent");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }



        public Stream GetAllFAQ1(Stream objStream)
        {
            Faq objDialog = new Faq();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"UserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                // var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);


                var questionlist = (from faq in icanSpeakContext.Faqs
                                    select new { faq.FaqId, faq.Title, faq.Description, faq.CreateDate, faq.ScreenName }).ToList();

                if (questionlist.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(questionlist, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No Data")));
                }
                //}
                //if (questionlist.Count() > 0 && questionlist != null)
                //{
                //    Table = Service.Message("success", "1");
                //    Table.AcceptChanges();
                //    DataTable dt = Service.ConvertToDataTable(questionlist); ;
                //    dt.TableName = "FAQList";
                //    ds.Tables.Add(Table);
                //    ds.Tables.Add(dt);
                //    string js = JsonConvert.SerializeObject(ds);
                //    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                //    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                //}
                //else
                //{
                //    DataTable Table = Service.Message("No Data", "0");
                //    Table.AcceptChanges();
                //    ds.Tables.Add(Table);
                //    string js1 = JsonConvert.SerializeObject(ds);
                //    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                //}

            }

            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AboutContent");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }


        public Stream AddFaq1(Stream objStream)
        {
            try
            {
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var checkUnit = (from unit in icanSpeakContext.Faqs
                                 where unit.Title == userRequest["Title"]
                                 select new { unit.FaqId }).ToList();

                if (checkUnit.Count == 0)
                {
                    Faq objfaq = new Faq();

                    objfaq.Title = userRequest["Title"];
                    objfaq.Description = userRequest["Description"];
                    objfaq.CreateDate = System.DateTime.Now;
                    icanSpeakContext.Faqs.InsertOnSubmit(objfaq);
                    icanSpeakContext.SubmitChanges();
                    var FaqId = icanSpeakContext.Faqs.ToList().Max(U => U.FaqId);

                    var js = JsonConvert.SerializeObject(FaqId, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize("Question with this name already exists.")).Replace("\\/", "/")));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AboutContent");
                //WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message)).Replace("\\/", "/")));
            }
        }

        public Stream GetFaqByFaqId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();

            var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
            try
            {
                var checkUnit = (from faq in icanSpeakContext.Faqs
                                 where faq.FaqId == Convert.ToInt32(userRequest["FaqId"])
                                 select new
                                 {
                                     faq.FaqId,
                                     faq.Title,
                                     faq.Description,
                                 }).ToList();

                if (checkUnit.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(checkUnit, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No data")));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetFaqByFaqId");
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message)).Replace("\\/", "/")));
            }
        }


        public Stream UpdateFAQ(Stream objStream)
        {
            try
            {
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var FaId = (from faq in icanSpeakContext.Faqs
                            where faq.FaqId == Convert.ToInt32(userRequest["FaqId"])
                            select faq).FirstOrDefault();
                if (FaId.FaqId > 0)
                {

                    FaId.Title = userRequest["Title"];
                    FaId.Description = userRequest["Description"];
                    FaId.ModifyDate = System.DateTime.Now;
                    icanSpeakContext.SubmitChanges();
                    var js = JsonConvert.SerializeObject("Frequently Asked Question updated successfully", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    var js = JsonConvert.SerializeObject("Record not available for updation", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AboutContent");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream DeleteFAQByFaqId(Stream objStream)
        {
            try
            {
                Faq objFaq = new Faq();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                var conversationData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from faq in icanSpeakContext.Faqs
                              where faq.FaqId == Convert.ToInt32(conversationData["FaqId"])
                              select faq).FirstOrDefault();
                if (conversationData["softDelete"] == "true")
                {
                }
                else
                {
                    icanSpeakContext.Faqs.DeleteOnSubmit(result);
                }
                icanSpeakContext.SubmitChanges();
                var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteFAQByFaqId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream AddUpdateWelcomeContent(Stream objStream)
        {
            try
            {
                Welcome welcome = new Welcome();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                // requestString = "{\"id\":\"1\",\"title\":\"HelpContent\",\"screenName\":\"Profile\",\"description\":\"This is help content related to profile\"}";
                // requestString = "{\"helpId\":\"0\"}";
                var content = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                if (content["operation"] == "select")
                {
                    var result = (from welcomeContent in icanSpeakContext.Welcomes
                                  select new { welcomeContent.Header, welcomeContent.Message });
                    var js = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                if (content["operation"] == "update")
                {
                    var welcomeResult = (from data in icanSpeakContext.Welcomes
                                         select data).FirstOrDefault();

                    welcomeResult.Header = content["header"];
                    welcomeResult.Message = content["message"];
                    icanSpeakContext.SubmitChanges();

                }

                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));

                //if (content["id"] == "0")
                //{
                //    welcome.Header = content["title"];
                //    welcome.Message = content["screenName"];

                //    icanSpeakContext.Welcomes.InsertOnSubmit(welcome);

                //    icanSpeakContext.SubmitChanges();

                //    var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                //    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                //}
                //else
                //{
                //    var welcomeResult = (from data in icanSpeakContext.Welcomes
                //                         where data.id == Convert.ToInt32(content["id"])
                //                         select data).FirstOrDefault();
                //    if (welcomeResult.id > 0)
                //    {

                //        welcome.Header = content["title"];
                //        welcome.Message = content["screenName"];
                //        icanSpeakContext.SubmitChanges();
                //        var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                //        return new MemoryStream(Encoding.UTF8.GetBytes(js));
                //    }
                //    else
                //    {
                //        var js = JsonConvert.SerializeObject("Error", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                //        return new MemoryStream(Encoding.UTF8.GetBytes(js));
                //    }
                //}
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AboutContent");

                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream NewsLetter(Stream objStream)
        {
            try
            {

                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                // requestString = "{\"id\":\"1\",\"title\":\"HelpContent\",\"screenName\":\"Profile\",\"description\":\"This is help content related to profile\"}";
                // requestString = "{\"helpId\":\"0\"}";
                var content = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var userEmails = (from data in icanSpeakContext.Users
                                  select data.Email).ToList();


                if (userEmails.Count > 0)
                {
                    EmailSendUtility objEmail = new EmailSendUtility();
                    foreach (var email in userEmails)
                    {
                        NewsLatter newsLatter = new NewsLatter();

                        newsLatter.MeaageTo = email;
                        newsLatter.MessageFrom = ConfigurationManager.AppSettings["EmailID"];
                        newsLatter.MessageBody = content["messageBody"];
                        newsLatter.Subject = content["subject"];
                        icanSpeakContext.NewsLatters.InsertOnSubmit(newsLatter);
                        icanSpeakContext.SubmitChanges();

                        bool status = objEmail.SendMail(content["subject"], content["messageBody"], email);
                        if (status != true)
                        {

                            return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Please check email and try again!")));

                            // return javaScriptSerializer.Serialize("Please check email and try again!");
                        }
                    }

                }

                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AboutContent");

                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream TermsAndCondition(Stream objStream)
        {
            try
            {
                TermsCondition termsCondition = new TermsCondition();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                //  requestString = "{\"helpId\":\"1\",\"title\":\"HelpContent\",\"screenName\":\"Profile\",\"description\":\"This is help content related to profile\"}";
                // requestString = "{\"helpId\":\"0\"}";
                var content = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                if (content["operation"] == "select")
                {

                    var result = (from conditionContent in icanSpeakContext.TermsConditions
                                  select new { conditionContent.Id, conditionContent.Text, conditionContent.IsActive, conditionContent.CreatedDate }).FirstOrDefault();

                    if (result.Id > 0)
                    {
                        var js = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                        return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    }
                    else
                    {
                        return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No Data")));

                    }
                }
                if (content["operation"] == "update")
                {
                    var result = (from data in icanSpeakContext.TermsConditions
                                  select data).FirstOrDefault();

                    result.Text = content["text"];
                    result.IsActive = true;
                    //icanSpeakContext.TermsConditions.InsertOnSubmit(result);
                    icanSpeakContext.SubmitChanges();

                }
                //var js1 = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));


            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "TermsAndCondition");

                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Error")));
            }


        }

        public Stream SuccessStoryList(Stream objStream)
        {
            SuccessStory objsotry = new SuccessStory();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                var storylist = (from story in icanSpeakContext.SuccessStories
                                 select new
                                 {
                                     story.StoryId,
                                     story.ClientName,
                                     ClientImageUrl = Service.GetUrl() + "ClientImage/" + story.ClientImageUrl,
                                     story.ClientStory,
                                     story.CreatedDate
                                 }).ToList();

                if (storylist.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(storylist, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No Data")));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AboutContent");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }

        public Stream AddSuccessStory(Stream objStream)
        {
            try
            {
                SuccessStory objsotry = new SuccessStory();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                var addstory = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                var clientCheck = icanSpeakContext.SuccessStories.Any(clientName => clientName.ClientName == addstory["clientName"]);
                if (clientCheck == true)
                {
                    var js = JsonConvert.SerializeObject(" This client name is already exist", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    objsotry.ClientName = addstory["clientName"];
                    objsotry.ClientStory = addstory["clientStory"];
                    objsotry.CreatedDate = System.DateTime.Now;
                    icanSpeakContext.SuccessStories.InsertOnSubmit(objsotry);
                    icanSpeakContext.SubmitChanges();
                    int StoryId = icanSpeakContext.SuccessStories.ToList().Max(U => U.StoryId);
                    objsotry.ClientImageUrl = StoryId + "_ClientImage.jpg";
                    icanSpeakContext.SubmitChanges();
                    var js = JsonConvert.SerializeObject(StoryId, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AboutContent");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream DeleteStoryByStoryId(Stream objStream)
        {

            try
            {
                SuccessStory objsotry = new SuccessStory();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"userId\":\"8\",\"softDelete\":\"true\"}";
                var addstory = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from story in icanSpeakContext.SuccessStories
                              where story.StoryId == Convert.ToInt32(addstory["StoryId"])
                              select story).FirstOrDefault();
                icanSpeakContext.SuccessStories.DeleteOnSubmit(result);
                icanSpeakContext.SubmitChanges();
                // return javaScriptSerializer.Serialize("User deleted successfully");
                var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteStory");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetStoryByStoryId(Stream objStream)
        {
            try
            {
                SuccessStory objstory = new SuccessStory();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                var addstory = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from story in icanSpeakContext.SuccessStories
                              where story.StoryId == Convert.ToInt32(addstory["StoryId"])
                              select new { story.StoryId, story.ClientName, story.ClientStory, ClientImageUrl = Service.GetUrl() + "ClientImage/" + story.ClientImageUrl, story.CreatedDate }).ToList();

                if (result.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    var js = JsonConvert.SerializeObject("No data", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAllVocabCategory");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream UpdateStoryByStoryId(Stream objStream)
        {
            try
            {
                SuccessStory objstory = new SuccessStory();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                var storydata = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                var result = (from story in icanSpeakContext.SuccessStories
                              where story.StoryId == Convert.ToInt32(storydata["storyId"])
                              select story).FirstOrDefault();
                if (result.StoryId > 0)
                {
                    result.ClientName = storydata["clientName"].ToString();
                    result.ClientStory = storydata["clientStory"].ToString();
                    // result.ClientImageUrl = storydata["clientImageUrl"].ToString();
                    result.ModifiedDate = System.DateTime.Now;
                    icanSpeakContext.SubmitChanges();

                    var js = JsonConvert.SerializeObject("success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    var js = JsonConvert.SerializeObject("Record not available for updation", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "UpdateStoryByStoryId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetAllStory(Stream objStream)
        {
            SuccessStory objStory = new SuccessStory();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                var storylist = (from story in icanSpeakContext.SuccessStories
                                 select new
                                 {
                                     story.StoryId,
                                     story.ClientName,
                                     story.ClientStory,
                                     ClientImageUrl = Service.GetUrl() + "ClientImage/" + story.ClientImageUrl,
                                     story.CreatedDate
                                 }).ToList();
                if (storylist.Count() > 0 && storylist != null)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(storylist); ;
                    dt.TableName = "StoryList";
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
                Helper.ErrorLog(ex, "AboutContent");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }

        public Stream GetVideoDetail(Stream objStream)
        {
            DemoVideo objVideo = new DemoVideo();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {            
                var video = (from vid in icanSpeakContext.DemoVideos                            
                             select new
                             {
                                 vid.VideoId,
                                 vid.VideoName,
                                 VideoUrl = Service.GetUrl() + "DemoVideo/" + vid.VideoUrl,
                             }).ToList();
                //if (video.Count > 0)
                //{
                //    var js = JsonConvert.SerializeObject(video, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                //    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                //}
                //else
                //{
                //    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No Data")));
                //}
                //}           
                //catch (Exception ex)
                //{
                //    Helper.ErrorLog(ex, "GetVideoByVideoId");
                //    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
                //}

                if (video.Count() > 0 && video != null)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(video); ;
                    dt.TableName = "DemoVideo";
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
                Helper.ErrorLog(ex, "AboutContent");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }
        public Stream UpdateVideo(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                DemoVideo objVideo = new DemoVideo();
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);


                var VideoResult = (from data in icanSpeakContext.DemoVideos
                                   select data).FirstOrDefault();
                if (VideoResult != null)
                {
                    objVideo = icanSpeakContext.DemoVideos.Single(key => key.VideoId == VideoResult.VideoId);
                    objVideo.VideoName = userRequest["VideoName"];                    
                    icanSpeakContext.SubmitChanges();
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("success")));

                }
                else
                {
                    objVideo.VideoName = userRequest["VideoName"];                    
                    icanSpeakContext.DemoVideos.InsertOnSubmit(objVideo);
                    icanSpeakContext.SubmitChanges();
                    int ViedoId = icanSpeakContext.DemoVideos.FirstOrDefault().VideoId;
                    objVideo.VideoUrl = ViedoId + "_DemoVideo.mp4";
                    icanSpeakContext.SubmitChanges();

                    var Result = (from data in icanSpeakContext.DemoVideos
                                  select data).FirstOrDefault();
                    var js = JsonConvert.SerializeObject(Result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));


                }
            }

            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "UpdateVideo");
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message))));
            }
        }
    }
}

