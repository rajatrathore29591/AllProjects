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
    public class Subscription
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataSet ds = new DataSet();
        DataTable Table = new DataTable();


        public Stream GetSubscriptionDetail(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String responseString = reader.ReadToEnd();
            try
            {
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(responseString);
                //int userid = Convert.ToInt32(userRequest["email"]);
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;


                var SubscriptionList = (from Subscriptions in icanSpeakContext.PremiumSubscriptions

                                        select new
                                        {
                                            Subscriptions.PremiumSubscriptionId,
                                            Subscriptions.SubscriptionName,
                                            Subscriptions.Price,
                                            Subscriptions.GrammerCount,
                                            Subscriptions.VocabCount,
                                            Subscriptions.DialogCount,
                                            Subscriptions.CreatedDate
                                        }).ToList();


                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                DataTable dt = Service.ConvertToDataTable(SubscriptionList); ;
                dt.TableName = "Subscription";
                ds.Tables.Add(Table);
                ds.Tables.Add(dt);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetSubscriptionDetail");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }

        public Stream GetPremiumSubscriptionDetail(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String responseString = reader.ReadToEnd();
            try
            {
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(responseString);
                //int userid = Convert.ToInt32(userRequest["email"]);
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;


                var SubscriptionList = (from Subscriptions in icanSpeakContext.PremiumSubscriptions
                                        //join MapSubscription in icanSpeakContext.MappingSubscriptions on Subscriptions.PremiumSubscriptionId equals MapSubscription.SubscriptionId into Inners
                                        //from MapSubscription in Inners.DefaultIfEmpty()
                                        select new
                                        {
                                            Subscriptions.PremiumSubscriptionId,
                                            Subscriptions.SubscriptionName,
                                            Subscriptions.Price,
                                            Subscriptions.GrammerCount,
                                            Subscriptions.VocabCount,
                                            Subscriptions.DialogCount,
                                            Subscriptions.VocabWordCount,

                                            Subscriptions.CreatedDate

                                            //MapSubscription.CourseId
                                        }).ToList();


                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                DataTable dt = Service.ConvertToDataTable(SubscriptionList); ;
                dt.TableName = "Subscription";
                ds.Tables.Add(Table);
                ds.Tables.Add(dt);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetSubscriptionDetail");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }

        public Stream GetGrammerBySubscriptionId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String responseString = reader.ReadToEnd();
            try
            {
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(responseString);
                //int userid = Convert.ToInt32(userRequest["email"]);
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                var SubscriptionIds = Convert.ToInt32(userRequest["PremiumSubscriptionId"]);

                var SubscriptionList = (from grameer in icanSpeakContext.GrammerUnits
                                        join Subscriptions in icanSpeakContext.MappingSubscriptions on (Convert.ToInt32(grameer.UnitId)) equals Subscriptions.CourseId
                                        join premiumsubscription in icanSpeakContext.PremiumSubscriptions on Subscriptions.SubscriptionId equals premiumsubscription.PremiumSubscriptionId
                                        where Subscriptions.SubscriptionId == SubscriptionIds

                                        select new
                                        {
                                            grameer.UnitNameEnglish,
                                            premiumsubscription.SubscriptionName,
                                            grameer.UnitId,
                                            Subscriptions.MappingId,
                                            Subscriptions.SubscriptionId,
                                            premiumsubscription.PremiumSubscriptionId

                                        }).ToList();


                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                DataTable dt = Service.ConvertToDataTable(SubscriptionList); ;
                dt.TableName = "GrammerDetail";
                ds.Tables.Add(Table);
                ds.Tables.Add(dt);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetGrammerBySubscriptionId");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }

        public Stream GetDialogBySubscriptionId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String responseString = reader.ReadToEnd();
            try
            {
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(responseString);
                //int userid = Convert.ToInt32(userRequest["email"]);
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                var SubscriptionIds = Convert.ToInt32(userRequest["PremiumSubscriptionId"]);

                var SubscriptionList = (from dialog in icanSpeakContext.Dialogs
                                        join Subscriptions in icanSpeakContext.MappingSubscriptions on (Convert.ToInt32(dialog.DialogId)) equals Subscriptions.CourseId
                                        join premiumsubscription in icanSpeakContext.PremiumSubscriptions on Subscriptions.SubscriptionId equals premiumsubscription.PremiumSubscriptionId
                                        where Subscriptions.SubscriptionId == SubscriptionIds
                                        select new
                                        {
                                            dialog.EnglishName,
                                            premiumsubscription.SubscriptionName,
                                            dialog.DialogId,
                                            Subscriptions.MappingId,
                                            Subscriptions.SubscriptionId,
                                            premiumsubscription.PremiumSubscriptionId
                                        }).ToList();


                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                DataTable dt = Service.ConvertToDataTable(SubscriptionList); ;
                dt.TableName = "DialogDetail";
                ds.Tables.Add(Table);
                ds.Tables.Add(dt);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetDialogBySubscriptionId");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }

        public Stream GetVocabBySubscriptionId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String responseString = reader.ReadToEnd();
            try
            {
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(responseString);
                //int userid = Convert.ToInt32(userRequest["email"]);
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                var SubscriptionIds = Convert.ToInt32(userRequest["PremiumSubscriptionId"]);

                var SubscriptionList = (from vocab in icanSpeakContext.Vocabularies
                                        join Subscriptions in icanSpeakContext.MappingSubscriptions on (Convert.ToInt32(vocab.VocabularyId)) equals Subscriptions.CourseId
                                        join premiumsubscription in icanSpeakContext.PremiumSubscriptions on Subscriptions.SubscriptionId equals premiumsubscription.PremiumSubscriptionId
                                        where Subscriptions.SubscriptionId == SubscriptionIds
                                        select new
                                        {
                                            vocab.Text,
                                            premiumsubscription.SubscriptionName,
                                            vocab.VocabularyId,
                                            Subscriptions.MappingId,
                                            Subscriptions.SubscriptionId,
                                            premiumsubscription.PremiumSubscriptionId

                                        }).ToList();


                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                DataTable dt = Service.ConvertToDataTable(SubscriptionList); ;
                dt.TableName = "VocabDetail";
                ds.Tables.Add(Table);
                ds.Tables.Add(dt);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetVocabBySubscriptionId");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }

        public Stream GetGrammerUnitDetail(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                //  requestString = "{\"UnitNameEnglish\":\"test 1\",\"UnitNameArabic\":\"test 1\",\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\",\"PPTUrl\":\"www.google.com\",\"VideoUrl\":\"www.google.com\",\"Price\":\"3\",\"Duration\":\"1 Month\",\"AssessmentSlots\":\"2.35,4,5,6,8.2,9\",\"DescriptionEnglish\":\"test english\",\"DescriptionArabic\":\"test arabic\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var checkUnit = (from unit in icanSpeakContext.GrammerUnits
                                 where !(from subscription in icanSpeakContext.MappingSubscriptions
                                         where Convert.ToInt32(unit.UnitId) == subscription.CourseId
                                         select subscription.CourseId)
                                         .Contains(Convert.ToInt32(unit.UnitId))
                                 select new
                                 {
                                     unit.UnitNameEnglish,
                                     unit.UnitId

                                 }).ToList();
                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                DataTable dt = Service.ConvertToDataTable(checkUnit); ;
                dt.TableName = "GrammerDetail";
                ds.Tables.Add(Table);
                ds.Tables.Add(dt);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetGrammerDetail");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }

        public Stream GetVocabUnitDetail(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                //  requestString = "{\"UnitNameEnglish\":\"test 1\",\"UnitNameArabic\":\"test 1\",\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\",\"PPTUrl\":\"www.google.com\",\"VideoUrl\":\"www.google.com\",\"Price\":\"3\",\"Duration\":\"1 Month\",\"AssessmentSlots\":\"2.35,4,5,6,8.2,9\",\"DescriptionEnglish\":\"test english\",\"DescriptionArabic\":\"test arabic\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var checkUnit = (from vocab in icanSpeakContext.Vocabularies
                                 where !(from subscription in icanSpeakContext.MappingSubscriptions
                                         where Convert.ToInt32(vocab.VocabularyId) == subscription.CourseId
                                         select subscription.CourseId)
                                         .Contains(Convert.ToInt32(vocab.VocabularyId))
                                 select new
                                 {

                                     vocab.Text,
                                     vocab.VocabularyId

                                 }).ToList();
                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                DataTable dt = Service.ConvertToDataTable(checkUnit); ;
                dt.TableName = "VocabDetail";
                ds.Tables.Add(Table);
                ds.Tables.Add(dt);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetVocabDetail");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }

        public Stream GetDialogUnitDetail(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                //  requestString = "{\"UnitNameEnglish\":\"test 1\",\"UnitNameArabic\":\"test 1\",\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\",\"PPTUrl\":\"www.google.com\",\"VideoUrl\":\"www.google.com\",\"Price\":\"3\",\"Duration\":\"1 Month\",\"AssessmentSlots\":\"2.35,4,5,6,8.2,9\",\"DescriptionEnglish\":\"test english\",\"DescriptionArabic\":\"test arabic\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var checkUnit = (from dialog in icanSpeakContext.Dialogs
                                 where !(from subscription in icanSpeakContext.MappingSubscriptions
                                         where Convert.ToInt32(dialog.DialogId) == subscription.CourseId
                                         select subscription.CourseId)
                                         .Contains(Convert.ToInt32(dialog.DialogId))
                                 select new
                                 {

                                     dialog.EnglishName,
                                     dialog.DialogId

                                 }).ToList();
                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                DataTable dt = Service.ConvertToDataTable(checkUnit); ;
                dt.TableName = "DialogDetail";
                ds.Tables.Add(Table);
                ds.Tables.Add(dt);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetDialogDetail");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }

        public Stream AddGrammerBySubscriptionId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                MappingSubscription objsubscription = new MappingSubscription();
                //objgrmrassesmentquestn = icanSpeakContext.GrammerAssessmentQuestions.Single(key => key.SlotId == Convert.ToInt32(userRequest["SlotId"]));
                objsubscription.SubscriptionId = Convert.ToInt32(userRequest["SubscriptionId"]);
                objsubscription.CourseId = Convert.ToInt32(userRequest["CourseId"]);
                objsubscription.CourseType = "Grammer";
                objsubscription.CreatedDate = System.DateTime.Now;
                icanSpeakContext.MappingSubscriptions.InsertOnSubmit(objsubscription);
                icanSpeakContext.SubmitChanges();



                var result = (from subscription in icanSpeakContext.PremiumSubscriptions
                              where subscription.PremiumSubscriptionId == Convert.ToInt32(userRequest["SubscriptionId"])
                              select subscription).FirstOrDefault();
                if (result != null)
                {
                    result.GrammerCount = result.GrammerCount + 1;
                    icanSpeakContext.SubmitChanges();

                }

                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAssessmentQuestionBySlotId");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }

        public Stream AddDialogBySubscriptionId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                MappingSubscription objsubscription = new MappingSubscription();
                //objgrmrassesmentquestn = icanSpeakContext.GrammerAssessmentQuestions.Single(key => key.SlotId == Convert.ToInt32(userRequest["SlotId"]));
                objsubscription.SubscriptionId = Convert.ToInt32(userRequest["SubscriptionId"]);
                objsubscription.CourseId = Convert.ToInt32(userRequest["CourseId"]);
                objsubscription.CourseType = "Dialog";
                objsubscription.CreatedDate = System.DateTime.Now;
                icanSpeakContext.MappingSubscriptions.InsertOnSubmit(objsubscription);
                icanSpeakContext.SubmitChanges();

                var result = (from subscription in icanSpeakContext.PremiumSubscriptions
                              where subscription.PremiumSubscriptionId == Convert.ToInt32(userRequest["SubscriptionId"])
                              select subscription).FirstOrDefault();
                if (result != null)
                {
                    result.DialogCount = result.DialogCount + 1;
                    icanSpeakContext.SubmitChanges();

                }

                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAssessmentQuestionBySlotId");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }

        public Stream AddVocabBySubscriptionId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                MappingSubscription objsubscription = new MappingSubscription();
                //objgrmrassesmentquestn = icanSpeakContext.GrammerAssessmentQuestions.Single(key => key.SlotId == Convert.ToInt32(userRequest["SlotId"]));
                objsubscription.SubscriptionId = Convert.ToInt32(userRequest["SubscriptionId"]);
                objsubscription.CourseId = Convert.ToInt32(userRequest["CourseId"]);
                objsubscription.CourseType = "Vocab";
                objsubscription.CreatedDate = System.DateTime.Now;
                icanSpeakContext.MappingSubscriptions.InsertOnSubmit(objsubscription);
                icanSpeakContext.SubmitChanges();


                var wordcount = (from vocab in icanSpeakContext.VocabularyWords
                                 where vocab.VocabularyId == Convert.ToInt32(userRequest["CourseId"])
                                 select new { vocab.WordId }
                                   ).ToList();
                int wordcounts = 0;
                if (wordcount.Count() > 0)
                {
                    wordcounts = wordcount.Count();
                }



                var result = (from subscription in icanSpeakContext.PremiumSubscriptions
                              where subscription.PremiumSubscriptionId == Convert.ToInt32(userRequest["SubscriptionId"])
                              select subscription).FirstOrDefault();
                if (result != null)
                {
                    result.VocabCount = result.VocabCount + 1;
                    result.VocabWordCount = result.VocabWordCount + wordcounts;
                    icanSpeakContext.SubmitChanges();

                }

                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddVocabBySubscriptionId");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }

        public Stream DeleteBySubscriptionId(Stream objStream)
        {

            try
            {
                MappingSubscription objsubscription = new MappingSubscription();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"userId\":\"8\",\"softDelete\":\"true\"}";
                var GrammerName = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from subscription in icanSpeakContext.MappingSubscriptions
                              where subscription.MappingId == Convert.ToInt32(GrammerName["MappingId"])
                              select subscription).FirstOrDefault();


                icanSpeakContext.MappingSubscriptions.DeleteOnSubmit(result);
                icanSpeakContext.SubmitChanges();

                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteBySubscriptionId");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }



        //////////////////////////////////////////////////////////Start Normal Plan///////////////////////////////////////////////////////////////////////

        public Stream GetNormalPlanSubscriptionDetail(Stream objStream)
        {

            try
            {

                var result1 = (from vocab in icanSpeakContext.Vocabularies
                              select new
                              {
                               vocab.VocabularyId,
                               vocab.Text,
                               vocab.ArabicText

                              }).ToList();


                var result2 = (from dialog in icanSpeakContext.Dialogs
                              
                               select new
                               {
                                 dialog.DialogId,
                                 dialog.EnglishName,
                                 dialog.ArabicName
                               }).ToList();

                var result3 = (from grammer in icanSpeakContext.GrammerUnits

                               select new
                               {
                                   grammer.UnitId,
                                   grammer.UnitNameEnglish,
                                   grammer.UnitNameArabic
                               }).ToList();


                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                DataTable Result1 = Service.ConvertToDataTable(result1);
                Result1.TableName = "Result1";
                DataTable Result2 = Service.ConvertToDataTable(result2);
                Result2.TableName = "Result2";
                DataTable Result3 = Service.ConvertToDataTable(result3);
                Result3.TableName = "Result3";
                ds.Tables.Add(Table);
                ds.Tables.Add(Result1);
                ds.Tables.Add(Result2);
                ds.Tables.Add(Result3);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetNormalPlanSubscriptionDetail");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }

        



        //////////////////////////////////////////////////////////End Normal Plan ////////////////////////////////////////////////////////////////////////






    }
}
