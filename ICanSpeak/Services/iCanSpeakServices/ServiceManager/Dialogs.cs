using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Data;
//using System.ServiceModel.Web;

namespace iCanSpeakServices.ServiceManager
{
    public class Dialogs
    {
        DataTable Table = new DataTable();
        DataSet ds = new DataSet("Result");
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        public Stream AddDialog(Stream objStream)
        {
            Dialog objDialog = new Dialog();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"DialogId\":\"0\",\"UserId\":\"1\",\"AccessToken\":\"xdoykz\",\"ArabicName\":\"SampleArabic 1\",\"EnglishName\":\"SampleEnglish 5\",\"VideoUrl\":\"VideoUrl\",\"AudioUrl\":\"AudioUrl\",\"StoryArabic\":\"StoryArabic\",\"StoryEnglish\":\"StoryEnglish\",\"DescriptionArabic\":\"DescriptionArabic\",\"DescriptionEnglish\":\"DescriptionEnglish\",\"DialogGender\":\"1\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);




                var query = from dialog in icanSpeakContext.Dialogs
                            where dialog.EnglishName == userRequest["EnglishName"]

                            select dialog.DialogId;

                if (Convert.ToString(query.FirstOrDefault()) == "" || Convert.ToString(query.FirstOrDefault()) == "0")
                {
                    objDialog.ArabicName = userRequest["ArabicName"];
                    objDialog.EnglishName = userRequest["EnglishName"];
                    objDialog.IsActive = true;
                    objDialog.StoryArabic = userRequest["StoryArabic"];
                    objDialog.StoryEnglish = userRequest["StoryEnglish"];
                    objDialog.DescriptionArabic = userRequest["DescriptionArabic"];
                    objDialog.DescriptionEnglish = userRequest["DescriptionEnglish"];
                    objDialog.DialogGender = userRequest["DialogGender"];
                    objDialog.Price = Convert.ToInt32(userRequest["Price"]);
                    objDialog.Duration = userRequest["Duration"];
                    objDialog.RewardPoints = Convert.ToInt32(userRequest["RewardPoints"]);
                    objDialog.MaxScore = Convert.ToInt32(userRequest["MaxScore"]);
                    objDialog.IsFree = Convert.ToBoolean(userRequest["IsFree"]);
                    objDialog.CreateDate = System.DateTime.Now;
                    icanSpeakContext.Dialogs.InsertOnSubmit(objDialog);

                    icanSpeakContext.SubmitChanges();


                    var dialogId = icanSpeakContext.Dialogs.ToList().Max(U => U.DialogId);

                    objDialog.VideoUrl = dialogId + "_dialogvideo.mp4";
                    objDialog.AudioUrl = dialogId + "_dialogaudio1.mp3";
                    objDialog.Audio2Url = dialogId + "_dialogaudio2.mp3";
                    objDialog.EnglishSubtitleUrl = dialogId + "_englishsubtitle.vtt";
                    objDialog.ArabicSubtitleUrl = dialogId + "_arabicsubtitle.vtt";
                    objDialog.BothSubtitleUrl = dialogId + "_bothsubtitle.vtt";
                    icanSpeakContext.SubmitChanges();

                    icanSpeakContext.ExecuteCommand("update Users set batchcount=batchcount+1");

                    Notification objNotification = new Notification();
                    objNotification.UserId = 102;
                    objNotification.Message = "Added a new course";
                    objNotification.CreatedDate = System.DateTime.Now;
                    icanSpeakContext.Notifications.InsertOnSubmit(objNotification);
                    icanSpeakContext.SubmitChanges();

                    var js = JsonConvert.SerializeObject(dialogId, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    // return javaScriptSerializer.Serialize(data);
                }
                else
                {
                    // var js = JsonConvert.SerializeObject("This Dialog already exists", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("This Dialog already exists")));
                    //return javaScriptSerializer.Serialize("This Dialog already exists");
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddDialog");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }


        public Stream UpdateDialog(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                //  requestString = "{\"UnitNameEnglish\":\"test 1\",\"UnitNameArabic\":\"test 1\",\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\",\"PPTUrl\":\"www.google.com\",\"VideoUrl\":\"www.google.com\",\"Price\":\"3\",\"Duration\":\"1 Month\",\"AssessmentSlots\":\"2.35,4,5,6,8.2,9\",\"DescriptionEnglish\":\"test english\",\"DescriptionArabic\":\"test arabic\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var checkDialogname = from dialog in icanSpeakContext.Dialogs
                                       where dialog.DialogId == Convert.ToInt32(userRequest["DialogId"])
                                       select dialog.DialogId;

                if (checkDialogname != null)
                {
                    Dialog objDialog = new Dialog();
                    objDialog = icanSpeakContext.Dialogs.Single(key => key.DialogId == Convert.ToInt32(userRequest["DialogId"]));
                    objDialog.ArabicName = userRequest["ArabicName"];
                    objDialog.EnglishName = userRequest["EnglishName"];
                    objDialog.IsActive = true;
                    objDialog.StoryArabic = userRequest["StoryArabic"];
                    objDialog.StoryEnglish = userRequest["StoryEnglish"];
                    objDialog.DescriptionArabic = userRequest["DescriptionArabic"];
                    objDialog.DescriptionEnglish = userRequest["DescriptionEngilsh"];
                    objDialog.DialogGender = userRequest["DialogGender"];
                    objDialog.Price = Convert.ToInt32(userRequest["Price"]);
                    objDialog.Duration = userRequest["Duration"];
                    objDialog.RewardPoints = Convert.ToInt32(userRequest["RewardPoints"]);
                    objDialog.MaxScore = Convert.ToInt32(userRequest["MaxScore"]);
                    objDialog.IsFree = Convert.ToBoolean(userRequest["IsFree"]);
                    objDialog.ModifyDate = System.DateTime.Now;
                    icanSpeakContext.SubmitChanges();
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("success")));
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("already")));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "UpdateDialog");
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message))));
            }
        }

        public Stream GetAllDialog(Stream objStream)
        {
            Dialog objDialog = new Dialog();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"UserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                // var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);


                var query = (from dialog in icanSpeakContext.Dialogs
                             where dialog.DeleteDate == null
                             select new { dialog.DialogId, dialog.EnglishName, dialog.CreateDate, dialog.DialogGender, dialog.IsActive, dialog.Price }).ToList();

                if (query.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(query, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No Data")));
                }
            }

            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetDialogByDialogId");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }


        public Stream GetDialogDetail(Stream objStream)
        {
            Dialog objDialog = new Dialog();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                var dialogid = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);


                var query = (from dialog in icanSpeakContext.Dialogs
                             where dialog.DeleteDate == null && dialog.DialogId == Convert.ToInt32(dialogid["dialogid"])
                             select new
                             {
                                 dialog.DialogId,
                                 dialog.EnglishName,
                                 dialog.DescriptionEnglish,
                                 dialog.StoryEnglish,
                                 dialog.ArabicName,
                                 dialog.DescriptionArabic,
                                 dialog.StoryArabic,
                                 dialog.DialogGender,
                                 dialog.IsActive,
                                 dialog.Duration,
                                 dialog.RewardPoints,
                                 dialog.MaxScore,
                                 dialog.IsFree,
                                 dialog.Price,
                                 AudioUrl = Service.GetUrl() + "DialogAudio/" + dialog.AudioUrl,
                                 VideoUrl = Service.GetUrl() + "DialogVideo/" + dialog.VideoUrl,
                                 Audio2Url = Service.GetUrl() + "DialogAudio/" + dialog.Audio2Url,
                                 EnglishSubtitleUrl = Service.GetUrl() + "DialogSubtitle/" + dialog.EnglishSubtitleUrl,
                                 ArabicSubtitleUrl = Service.GetUrl() + "DialogSubtitle/" + dialog.ArabicSubtitleUrl,
                                 BothSubtitleUrl = Service.GetUrl() + "DialogSubtitle/" + dialog.BothSubtitleUrl
                             }).ToList();

                if (query.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(query, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No Data")));
                }
            }

            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetDialogByDialogId");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }



        public Stream DeleteDialogByDialogId(Stream objStream)
        {

            try
            {
                Dialog objDialog = new Dialog();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"userId\":\"8\",\"softDelete\":\"true\"}";
                var conversationData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from user in icanSpeakContext.Dialogs
                              where user.DialogId == Convert.ToInt32(conversationData["dialogid"])
                              select user).FirstOrDefault();





                result.DeleteDate = DateTime.Today;


                icanSpeakContext.SubmitChanges();
                // return javaScriptSerializer.Serialize("User deleted successfully");
                var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteDialogByDialogId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream ManageDialog(Stream objStream)
        {

            try
            {
                Dialog objDialog = new Dialog();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"userId\":\"8\",\"softDelete\":\"true\"}";
                var dialog = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from coursedialog in icanSpeakContext.Dialogs
                              where coursedialog.DialogId == Convert.ToInt32(dialog["DialogId"])
                              select coursedialog).FirstOrDefault();


                if (dialog["softDelete"] == "true")
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

                    icanSpeakContext.Dialogs.DeleteOnSubmit(result);
                }
                icanSpeakContext.SubmitChanges();

                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteDialogAssessmentQuestion");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }

        public Stream GetDialogByDialogId(Stream objStream)
        {
            Dialog objDialog = new Dialog();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"UserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);


                var query = from dialog in icanSpeakContext.Dialogs
                            where dialog.DialogId == Convert.ToInt32(userRequest["DialogId"])
                            select new { dialog.DialogId, dialog.EnglishName, dialog.ArabicName, dialog.AudioUrl, dialog.CreateDate, dialog.DescriptionArabic, dialog.DescriptionEnglish, dialog.DialogGender, dialog.StoryArabic, dialog.StoryEnglish, dialog.VideoUrl, dialog.ArabicSubtitleUrl, dialog.EnglishSubtitleUrl, dialog.BothSubtitleUrl };

                if (Convert.ToString(query.FirstOrDefault()) != "" || Convert.ToString(query.FirstOrDefault()) != "0")
                {
                    var js = JsonConvert.SerializeObject(query, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    // return javaScriptSerializer.Serialize(query);
                }
                else
                {

                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Invalid dialog id")));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetDialogByDialogId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }



        public Stream GetConversationByDialogId(Stream objStream)
        {
            Dialog objDialog = new Dialog();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"UserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var query = (from conversation in icanSpeakContext.DialogConversations
                             join dialog in icanSpeakContext.Dialogs on conversation.DialogId equals dialog.DialogId
                             where conversation.DialogId == Convert.ToInt32(userRequest["DialogId"])

                             select new
                             {
                                 DialogName = dialog.EnglishName,
                                 conversation.ConversationId,
                                 conversation.DialogId,
                                 conversation.Person1Text,
                                 conversation.Person2Text,
                                 conversation.Person1ArabicText,
                                 conversation.Person2ArabicText,
                                 conversation.CreateDate,
                                 conversation.IsActive
                             }).OrderByDescending(i => i.CreateDate);
                //select new { conversation.DialogId, dialog.EnglishName, dialog.ArabicName, dialog.AudioUrl, dialog.CreateDate, dialog.DescriptionArabic, dialog.DescriptionEnglish, dialog.DialogGender, dialog.StoryArabic, dialog.StoryEnglish, dialog.VideoUrl };

                if (Convert.ToString(query.Count()) != "0")
                {
                    var js = JsonConvert.SerializeObject(query, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    //return javaScriptSerializer.Serialize(query);
                }
                else
                {

                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Invalid dialog id")));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetConversationByDialogId");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }






        public Stream GetKeyPhrasesByDialogId(Stream objStream)
        {
            Dialog objDialog = new Dialog();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"UserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);


                var query = (from keyphrases in icanSpeakContext.DialogKeyPhrases
                             where keyphrases.DialogId == Convert.ToInt32(userRequest["DialogId"])
                             select new { keyphrases.DialogId, keyphrases.KeyPhrasesId, keyphrases.ArabicText, keyphrases.EnglishText, keyphrases.CreateDate, keyphrases.IsActive }).ToList();

                if (query.Count() != 0)
                {
                    var js = JsonConvert.SerializeObject(query, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    // return javaScriptSerializer.Serialize(query);
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Invalid dialog id")));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetKeyPhrasesByDialogId");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }

        public Stream AddKeyPhrasesByDialogId(Stream objStream)
        {

            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                //  requestString = "{\"DialogId\":\"5\",\"UserId\":\"1\",\"AccessToken\":\"xdoykz\",\"EnglishText\":\"hi\",\"ArabicText\":\"hello\"}";
                var userRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<KeyPhrasesModel>(requestString);
                string DialogId = userRequest.DialogId;
                string[] Englishtxt = userRequest.Englishtxt.ToArray();
                string[] Arabictxt = userRequest.Arabictxt.ToArray();
                int finalcount = Englishtxt.Count() - 1;

                for (int i = 0; i <= finalcount; i++)
                {
                    DialogKeyPhrase objKeyPhrases = new DialogKeyPhrase();
                    objKeyPhrases.EnglishText = Englishtxt[i].ToString();
                    objKeyPhrases.ArabicText = Arabictxt[i].ToString();
                    objKeyPhrases.DialogId = Convert.ToInt32(DialogId);
                    objKeyPhrases.IsActive = true;
                    objKeyPhrases.CreateDate = System.DateTime.Now;
                    icanSpeakContext.DialogKeyPhrases.InsertOnSubmit(objKeyPhrases);
                    icanSpeakContext.SubmitChanges();
                }
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("success")));




            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddKeyPhrasesByDialogId");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }

        public class KeyPhrasesModel
        {
            public List<string> Englishtxt { get; set; }
            public List<string> Arabictxt { get; set; }
            public string DialogId { get; set; }
        }

        public Stream AddConversationByDialogId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            //String requestString = "{\"DialogId\":\"27\",\"ConversationText\":[\"saraarb1\",\"saraarb1\"]}";//reader.ReadToEnd(); 
            String requestString = reader.ReadToEnd();
            try
            {

                var userRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<ConversationModel>(requestString);

                string DialogId = userRequest.DialogId;
                string[] oneengtxt = userRequest.oneengtxt.ToArray();
                string[] onearbtxt = userRequest.onearbtxt.ToArray();
                string[] twoengtxt = userRequest.twoengtxt.ToArray();
                string[] twoarbtxt = userRequest.twoarbtxt.ToArray();
                int finalcount = oneengtxt.Count() - 1;
                for (int i = 0; i <= finalcount; i++)
                {
                    DialogConversation objDialogConversation = new DialogConversation();
                    objDialogConversation.DialogId = Convert.ToInt32(DialogId);
                    objDialogConversation.Person1Text = oneengtxt[i].ToString();
                    objDialogConversation.Person2Text = twoengtxt[i].ToString();
                    objDialogConversation.Person1ArabicText = onearbtxt[i].ToString();
                    objDialogConversation.Person2ArabicText = twoarbtxt[i].ToString();
                    objDialogConversation.IsActive = 1;
                    objDialogConversation.CreateDate = System.DateTime.Now;

                    icanSpeakContext.DialogConversations.InsertOnSubmit(objDialogConversation);
                    icanSpeakContext.SubmitChanges();
                }

                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Conversation Added Successfully")));

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddKeyPhrasesByDialogId");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }

        public class ConversationModel
        {
            public List<string> oneengtxt { get; set; }
            public List<string> onearbtxt { get; set; }
            public List<string> twoengtxt { get; set; }
            public List<string> twoarbtxt { get; set; }
            public string DialogId { get; set; }
        }

        ///////////////// 16 sept 2014


        public Stream DeleteConversationByConversationId(Stream objStream)
        {

            try
            {
                GrammerUnit objGrammerUnit = new GrammerUnit();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"userId\":\"8\",\"softDelete\":\"true\"}";
                var conversationData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from user in icanSpeakContext.DialogConversations
                              where user.ConversationId == Convert.ToInt32(conversationData["conversationId"])
                              select user).FirstOrDefault();


                if (conversationData["softDelete"] == "true")
                {
                    if (result.IsActive == 1)
                    {
                        result.IsActive = 0;
                    }
                    else
                    {
                        result.IsActive = 1;
                    }
                }
                else
                {

                    icanSpeakContext.DialogConversations.DeleteOnSubmit(result);
                }
                icanSpeakContext.SubmitChanges();
                // return javaScriptSerializer.Serialize("User deleted successfully");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteConversationByConversationId");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }

        public Stream DeleteKeyPhrasesByKeyPhrasesId(Stream objStream)
        {

            try
            {
                GrammerUnit objGrammerUnit = new GrammerUnit();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"userId\":\"8\",\"softDelete\":\"true\"}";
                var conversationData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from user in icanSpeakContext.DialogKeyPhrases
                              where user.KeyPhrasesId == Convert.ToInt32(conversationData["keyPhrasesId"])
                              select user).FirstOrDefault();


                if (conversationData["softDelete"] == "true")
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

                    icanSpeakContext.DialogKeyPhrases.DeleteOnSubmit(result);
                }
                icanSpeakContext.SubmitChanges();
                // return javaScriptSerializer.Serialize("User deleted successfully");
                var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteKeyPhrasesByKeyPhrasesId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetKeyPhrasesByKeyPhrasesId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"UserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);


                var dialogKeyResult = (from dialogKeyPhrases in icanSpeakContext.DialogKeyPhrases
                                       where dialogKeyPhrases.KeyPhrasesId == Convert.ToInt32(userRequest["keyPhrasesId"])
                                       select new
                                       {
                                           dialogKeyPhrases.DialogId,
                                           dialogKeyPhrases.KeyPhrasesId,
                                           AudioUrl = Service.GetUrl() + "KeyPhrasesAudio/" + dialogKeyPhrases.AudioUrl,
                                           dialogKeyPhrases.EnglishText,
                                           dialogKeyPhrases.ArabicText,
                                       }).ToList();

                if (dialogKeyResult.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(dialogKeyResult, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    // return javaScriptSerializer.Serialize(checkQuestion);
                }
                else
                {
                    var js = JsonConvert.SerializeObject("No data", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    // return javaScriptSerializer.Serialize("No Question Available");
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetKeyPhrasesByKeyPhrasesId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetConversationByConversationId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"UserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);


                var dialogKeyResult = (from dialogConversations in icanSpeakContext.DialogConversations
                                       where dialogConversations.ConversationId == Convert.ToInt32(userRequest["conversationId"])
                                       select new
                                       {
                                           dialogConversations.DialogId,
                                           dialogConversations.ConversationId,
                                           dialogConversations.Person1Text,
                                           dialogConversations.Person2Text,
                                           dialogConversations.Person1ArabicText,
                                           dialogConversations.Person2ArabicText
                                       }).ToList();

                if (dialogKeyResult.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(dialogKeyResult, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    var js = JsonConvert.SerializeObject("No data", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    // return javaScriptSerializer.Serialize("No Question Available");
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetKeyPhrasesByKeyPhrasesId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        //////////////////04 sept 2014

        public Stream UpdateConversationByConversationId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"ConversationId\":\"6\",\"UserId\":\"1\",\"AccessToken\":\"xdoykz\",\"Person1Text\":\"hi\",\"Person2Text\":\"hello\",\"LanguageName\":\"Arabic\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);



                var checkconversationId = from dialogConversation in icanSpeakContext.DialogConversations
                                          where dialogConversation.ConversationId == Convert.ToInt32(userRequest["ConversationId"]) && dialogConversation.LanguageName == userRequest["LanguageName"]
                                          select dialogConversation.ConversationId;

                if (checkconversationId.FirstOrDefault() != null)
                {
                    DialogConversation objDialogConversation = new DialogConversation();

                    objDialogConversation = icanSpeakContext.DialogConversations.Single(conversaion => conversaion.ConversationId == Convert.ToInt32(userRequest["ConversationId"]));
                    objDialogConversation.ModifyDate = DateTime.Now;
                    objDialogConversation.Person1Text = userRequest["Person1Text"];
                    objDialogConversation.Person2Text = userRequest["Person2Text"];
                    objDialogConversation.Person1ArabicText = userRequest["onearbtxt"];
                    objDialogConversation.Person2ArabicText = userRequest["twoarbtxt"];
                    icanSpeakContext.SubmitChanges();

                    var js = JsonConvert.SerializeObject("Conversation Updated Successfully", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    //return javaScriptSerializer.Serialize("Conversation Updated Successfully");
                }
                else
                {
                    var js = JsonConvert.SerializeObject("Invalid conversation id with the corresponding language name", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    // return javaScriptSerializer.Serialize("Invalid conversation id with the corresponding language name");
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddKeyPhrasesByDialogId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream UpdateKeyPhrasesByKeyPhrasesId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                //  requestString = "{\"KeyPhrasesId\":\"6\",\"UserId\":\"1\",\"AccessToken\":\"xdoykz\",\"EnglishText\":\"hi\",\"ArabicText\":\"hello\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);


                var checkKeyPhrasesId = from dialogKeyPhrases in icanSpeakContext.DialogKeyPhrases
                                        where dialogKeyPhrases.KeyPhrasesId == Convert.ToInt32(userRequest["KeyPhrasesId"])
                                        select dialogKeyPhrases.KeyPhrasesId;

                if (checkKeyPhrasesId.FirstOrDefault() != null)
                {
                    DialogKeyPhrase objDialogKeyPhrase = new DialogKeyPhrase();
                    objDialogKeyPhrase = icanSpeakContext.DialogKeyPhrases.Single(key => key.KeyPhrasesId == Convert.ToInt32(userRequest["KeyPhrasesId"]));
                    objDialogKeyPhrase.ArabicText = userRequest["ArabicText"];
                    objDialogKeyPhrase.EnglishText = userRequest["EnglishText"];
                    objDialogKeyPhrase.ModifyDate = DateTime.Now;
                    icanSpeakContext.SubmitChanges();

                    var js = JsonConvert.SerializeObject("KeyPhrases Updated Successfully", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    var js = JsonConvert.SerializeObject("Invalid keyphrase id", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));

                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddKeyPhrasesByDialogId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));

            }
        }




        public Stream AddAssessmentQuestionByDialogId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                //   requestString = "{\"DialogId\":\"5\",\"UserId\":\"1\",\"AccessToken\":\"xdoykz\",\"Question\":\"hi, how _______ you ?\",\"QuestionType\":\"FillInTheBlanks\",\"AnswerOptions\":\"is,are,am\",\"CorrectAnswer\":\"are\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);


                var checkQuestion = from dialogQuestion in icanSpeakContext.DialogAssessmentQuestions
                                    where dialogQuestion.DialogId == Convert.ToInt32(userRequest["DialogId"]) && dialogQuestion.Question == userRequest["Question"] && dialogQuestion.QuestionType == userRequest["QuestionType"]
                                    select dialogQuestion.QuestionId;

                if (Convert.ToString(checkQuestion.FirstOrDefault()) != "0")
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Already")));
                }
                else
                {
                    DialogAssessmentQuestion objDialogAssessmentQuestion = new DialogAssessmentQuestion();

                    if (userRequest["QuestionType"] == "TrueFalse")
                    {
                        objDialogAssessmentQuestion.Question = userRequest["Question"];
                        objDialogAssessmentQuestion.QuestionType = userRequest["QuestionType"];
                        objDialogAssessmentQuestion.TrueFalseAnswer = Convert.ToBoolean(userRequest["TrueFalseType"]);
                        objDialogAssessmentQuestion.DialogId = Convert.ToInt32(userRequest["DialogId"]);
                        objDialogAssessmentQuestion.IsActive = 1;
                        objDialogAssessmentQuestion.CreateDate = System.DateTime.Now;
                        icanSpeakContext.DialogAssessmentQuestions.InsertOnSubmit(objDialogAssessmentQuestion);
                        icanSpeakContext.SubmitChanges();
                        return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));

                    }
                    if (userRequest["QuestionType"] == "Objective")
                    {
                        objDialogAssessmentQuestion.Question = userRequest["Question"];
                        objDialogAssessmentQuestion.QuestionType = userRequest["QuestionType"];
                        objDialogAssessmentQuestion.OptionText1 = userRequest["ObjOpt1txt"];
                        objDialogAssessmentQuestion.OptionText2 = userRequest["ObjOpt2txt"];
                        objDialogAssessmentQuestion.OptionText3 = userRequest["ObjOpt3txt"];
                        objDialogAssessmentQuestion.DialogId = Convert.ToInt32(userRequest["DialogId"]);
                        objDialogAssessmentQuestion.IsActive = 1;
                        objDialogAssessmentQuestion.CreateDate = System.DateTime.Now;
                        objDialogAssessmentQuestion.OptionCorrectAnswer = userRequest["OptionCorrectAnswer"];
                        icanSpeakContext.DialogAssessmentQuestions.InsertOnSubmit(objDialogAssessmentQuestion);
                        icanSpeakContext.SubmitChanges();
                        var questionid = icanSpeakContext.DialogAssessmentQuestions.ToList().Max(U => U.QuestionId);
                        objDialogAssessmentQuestion.OptionAudio1 = questionid + "_questionoption1.mp3";
                        objDialogAssessmentQuestion.OptionAudio2 = questionid + "_questionoption2.mp3";
                        objDialogAssessmentQuestion.OptionAudio3 = questionid + "_questionoption3.mp3";
                        icanSpeakContext.SubmitChanges();
                        return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(questionid.ToString())));

                    }
                    if (userRequest["QuestionType"] == "FillBlanks")
                    {
                        objDialogAssessmentQuestion.Question = userRequest["Question"];
                        objDialogAssessmentQuestion.QuestionType = userRequest["QuestionType"];
                        objDialogAssessmentQuestion.FillAnswerText = userRequest["FillAnsText"];
                        objDialogAssessmentQuestion.DialogId = Convert.ToInt32(userRequest["DialogId"]);
                        objDialogAssessmentQuestion.IsActive = 1;
                        objDialogAssessmentQuestion.CreateDate = System.DateTime.Now;
                        icanSpeakContext.DialogAssessmentQuestions.InsertOnSubmit(objDialogAssessmentQuestion);
                        icanSpeakContext.SubmitChanges();
                        return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
                        //icanSpeakContext.SubmitChanges();
                        //var questionid = icanSpeakContext.DialogAssessmentQuestions.ToList().Max(U => U.QuestionId);
                        //objDialogAssessmentQuestion.OptionAudio1 = questionid + "_questionoption1.mp3";
                        //objDialogAssessmentQuestion.OptionAudio2 = questionid + "_questionoption2.mp3";
                        //objDialogAssessmentQuestion.OptionAudio3 = questionid + "_questionoption3.mp3";
                        //icanSpeakContext.SubmitChanges();
                        //return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(questionid.ToString())));
                        //objDialogAssessmentQuestion.Question = userRequest["Question"];
                        //objDialogAssessmentQuestion.QuestionType = userRequest["QuestionType"];
                        //objDialogAssessmentQuestion.FillAnswerText = userRequest["FillAnsText"];
                        //objDialogAssessmentQuestion.DialogId = Convert.ToInt32(userRequest["DialogId"]);
                        //objDialogAssessmentQuestion.IsActive = 1;
                        //objDialogAssessmentQuestion.CreateDate = System.DateTime.Now;
                        //icanSpeakContext.DialogAssessmentQuestions.InsertOnSubmit(objDialogAssessmentQuestion);
                        //icanSpeakContext.SubmitChanges();
                        //return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
                    }

                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("error")));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddKeyPhrasesByDialogId");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("error")));
            }
        }

        public Stream GetAssessmentQuestionByDialogId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {

                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);


                var checkQuestion = (from dialogQuestion in icanSpeakContext.DialogAssessmentQuestions
                                     where dialogQuestion.DialogId == Convert.ToInt32(userRequest["DialogId"])
                                     select new
                                     {
                                         dialogQuestion.QuestionId,
                                         dialogQuestion.Question,
                                         dialogQuestion.QuestionType,
                                         //OptionAudio1 = Service.GetUrl() + "DialogAssesmentAudio/" + dialogQuestion.OptionAudio1,
                                         //OptionAudio2 = Service.GetUrl() + "DialogAssesmentAudio/" + dialogQuestion.OptionAudio2,
                                         //OptionAudio3 = Service.GetUrl() + "DialogAssesmentAudio/" + dialogQuestion.OptionAudio3,
                                         dialogQuestion.DialogId,
                                         dialogQuestion.IsActive,
                                         CreateDate = dialogQuestion.CreateDate
                                     }).ToList();

                if (checkQuestion.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(checkQuestion, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    // return javaScriptSerializer.Serialize(checkQuestion);
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No Question Available")));

                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddKeyPhrasesByDialogId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetDialogAssessmentQuestionByQuestionId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {

                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);


                var checkQuestion = (from dialogQuestion in icanSpeakContext.DialogAssessmentQuestions
                                     where dialogQuestion.QuestionId == Convert.ToInt32(userRequest["QuestionId"])
                                     select new
                                     {
                                         dialogQuestion.QuestionId,
                                         dialogQuestion.Question,
                                         dialogQuestion.QuestionType,
                                         dialogQuestion.FillAnswerText,
                                         dialogQuestion.TrueFalseAnswer,
                                         dialogQuestion.OptionCorrectAnswer,
                                         dialogQuestion.OptionText1,
                                         dialogQuestion.OptionText2,
                                         dialogQuestion.OptionText3,
                                         OptionAudio1 = Service.GetUrl() + "DialogAssessmentQuestionsAudio/" + dialogQuestion.OptionAudio1,
                                         OptionAudio2 = Service.GetUrl() + "DialogAssessmentQuestionsAudio/" + dialogQuestion.OptionAudio2,
                                         OptionAudio3 = Service.GetUrl() + "DialogAssessmentQuestionsAudio/" + dialogQuestion.OptionAudio3
                                     }).ToList();

                if (checkQuestion.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(checkQuestion, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    // return javaScriptSerializer.Serialize(checkQuestion);
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No Question Available")));

                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetDialogAssessmentQuestionByQuestionId");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }

        public Stream UpdateDialogAssessmentQuestion(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"QuestionId\":\"14\",\"Question\":\"hisss, how _______ you ?\",\"QuestionType\":\"\",\"AnswerOptions\":\"\",\"CorrectAnswer\":\"anup\",\"OptionAudio1\":\"audio.mp3.mp3\",\"OptionAudio2\":\"audio.mp3.mp3\",\"OptionAudio3\":\"So Jao(FreshMaza.Info).mp3\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                DialogAssessmentQuestion objDialogAssessmentQuestion = new DialogAssessmentQuestion();
                objDialogAssessmentQuestion = icanSpeakContext.DialogAssessmentQuestions.Single(key => key.QuestionId == Convert.ToInt32(userRequest["QuestionId"]));
                if (userRequest["QuestionType"] == "TrueFalse")
                {
                    objDialogAssessmentQuestion.Question = userRequest["Question"];
                    objDialogAssessmentQuestion.TrueFalseAnswer = Convert.ToBoolean(userRequest["TrueFalseType"]);
                    icanSpeakContext.SubmitChanges();
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
                }
                if (userRequest["QuestionType"] == "Objective")
                {
                    objDialogAssessmentQuestion.Question = userRequest["Question"];
                    objDialogAssessmentQuestion.OptionText1 = userRequest["ObjOpt1txt"];
                    objDialogAssessmentQuestion.OptionText2 = userRequest["ObjOpt2txt"];
                    objDialogAssessmentQuestion.OptionText3 = userRequest["ObjOpt3txt"];
                    objDialogAssessmentQuestion.OptionCorrectAnswer = userRequest["OptionCorrectAnswer"];
                    icanSpeakContext.SubmitChanges();
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(userRequest["QuestionId"])));
                }
                if (userRequest["QuestionType"] == "FillBlanks")
                {
                    objDialogAssessmentQuestion.Question = userRequest["Question"];
                    objDialogAssessmentQuestion.FillAnswerText = userRequest["FillAnsText"];
                    icanSpeakContext.SubmitChanges();
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
                }
                objDialogAssessmentQuestion.ModifiedDate = System.DateTime.Now;
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("error")));

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "UpdateDialogAssessmentQuestion");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream DeleteDialogAssessmentQuestion(Stream objStream)
        {

            try
            {
                DialogAssessmentQuestion objdialogasesmentquestion = new DialogAssessmentQuestion();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"userId\":\"8\",\"softDelete\":\"true\"}";
                var AssessmentQuestion = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from user in icanSpeakContext.DialogAssessmentQuestions
                              where user.QuestionId == Convert.ToInt32(AssessmentQuestion["QuestionId"])
                              select user).FirstOrDefault();


                if (AssessmentQuestion["softDelete"] == "true")
                {
                    if (result.IsActive == 1)
                    {
                        result.IsActive = 0;
                    }
                    else
                    {
                        result.IsActive = 1;
                    }
                }
                else
                {

                    icanSpeakContext.DialogAssessmentQuestions.DeleteOnSubmit(result);
                }
                icanSpeakContext.SubmitChanges();

                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteDialogAssessmentQuestion");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }

        public Stream SubmitAnswerByQuestionId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"QuestionId\":\"5\",\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\",\"UserId\":\"2\",\"Answer\":\"are\",\"IsCorrectAnswer\":\"1\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);


                var checkSubmitAnswer = (from submitanswer in icanSpeakContext.DialogAssessmentAnswers
                                         where submitanswer.QuestionId == Convert.ToInt32(userRequest["QuestionId"]) && submitanswer.UserId == Convert.ToInt32(userRequest["UserId"])
                                         select new { submitanswer.QuestionId }).ToList();

                if (checkSubmitAnswer.Count > 0)
                {
                    var js = JsonConvert.SerializeObject("You already answered this question", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));

                    // return javaScriptSerializer.Serialize("You already answered this question");
                }
                else
                {
                    DialogAssessmentAnswer objAnswer = new DialogAssessmentAnswer();
                    objAnswer.QuestionId = Convert.ToInt32(userRequest["QuestionId"]);
                    objAnswer.UserId = Convert.ToInt32(userRequest["UserId"]);
                    objAnswer.Answer = userRequest["Answer"];
                    objAnswer.IsCorrectAnswer = Convert.ToInt32(userRequest["IsCorrectAnswer"]);

                    icanSpeakContext.DialogAssessmentAnswers.InsertOnSubmit(objAnswer);
                    icanSpeakContext.SubmitChanges();

                    var js = JsonConvert.SerializeObject("Answer Submitted Successfully", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));

                    // return javaScriptSerializer.Serialize("Answer Submitted Successfully");
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddKeyPhrasesByDialogId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetAnswersByDialogId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var checkGetAnswers = (from question in icanSpeakContext.DialogAssessmentQuestions
                                       join answers in icanSpeakContext.DialogAssessmentAnswers on question.QuestionId equals answers.QuestionId
                                       where question.DialogId == Convert.ToInt32(userRequest["DialogId"])
                                       select new { question.DialogId, question.Question, question.QuestionId, answers.AnswerId, answers.Answer, answers.IsCorrectAnswer }).ToList();

                // return javaScriptSerializer.Serialize(checkGetAnswers);
                var js = JsonConvert.SerializeObject(checkGetAnswers, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddKeyPhrasesByDialogId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetDialogByUserId(Stream objStream)
        {
            Dialog objDialog = new Dialog();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();

            DataTable MyscoreDataStatus = new DataTable();
            DataTable TotalFlashDataStatus = new DataTable();
            DataTable Myscores = new DataTable();
            DataTable DialogAssessmentScore = new DataTable();
            DataTable AssessmentStatus = new DataTable();
            DataTable Dialog = new DataTable();
            DataTable DialogStatus = new DataTable();
            int dialogindex = 1;
            int scoreindex = 1;


            try
            {
                var userLogin = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int userid = Convert.ToInt32(Service.Decrypt(userLogin["userid"]));

                var dialogdata = (from dialog in icanSpeakContext.Dialogs
                                  where dialog.IsFree == true && dialog.DeleteDate == null
                                  select new
                                  {
                                      DialogId = Service.Encrypt(dialog.DialogId.ToString()),
                                      DialogEnglish = dialog.EnglishName,
                                      DialogArabic = dialog.ArabicName,
                                      dialog.CreateDate
                                  }).OrderByDescending(x => x.CreateDate);


                var dialogdatas = dialogdata.AsEnumerable().Select(x => new
                {
                    RowIndex = dialogindex++,
                    DialogId = x.DialogId,
                    DialogEnglish = x.DialogEnglish,
                    DialogArabic = x.DialogArabic,
                    Date = x.CreateDate
                }).ToList();

                //Table = Service.Message("success", "1");
                //Table.AcceptChanges();
                //Dialog = Service.ConvertToDataTable(dialogdatas);
                //Dialog.TableName = "Dialog";
                if (dialogdatas.Count > 0)
                {
                    DialogStatus = Service.Message("success", "1");
                    DialogStatus.AcceptChanges();
                    Dialog = Service.ConvertToDataTable(dialogdatas);
                    Dialog.TableName = "Dialog";
                    //AssessmentStatus = Service.Message("success", "1");
                    //AssessmentStatus.AcceptChanges();
                    //DialogAssessmentScore = Service.ConvertToDataTable(dialogAssessmentScores);
                    //DialogAssessmentScore.TableName = "DialogAssessmentScore";
                }
                else
                {
                    //Table = Service.Message("success", "1");
                    //Table.AcceptChanges();
                    DialogStatus = Service.Message("No Data", "0");
                    DialogStatus.TableName = "Dialog";
                }


                var dialogAssessmentScore = (from score in icanSpeakContext.DialogAssessmentScores
                                             where score.UserId == userid && score.Type == "Dialog"
                                             select new
                                             {
                                                 score.ScoreId,
                                                 DialogId = Service.Encrypt(score.DialogId.ToString()),
                                                 score.UserId,
                                                 score.Score,
                                                 score.TotalScore,
                                                 score.ModifiedDate,
                                                 score.CreatedDate
                                             }).OrderByDescending(y => y.CreatedDate);

                var dialogAssessmentScores = dialogAssessmentScore.AsEnumerable().Select(y => new
                {
                    RowIndex = scoreindex++,
                    ScoreId = y.ScoreId,
                    DialogId = y.DialogId,
                    UserId = y.UserId,
                    Score = y.Score,
                    TotalScore = y.TotalScore,
                    ModifiedDate = y.ModifiedDate,
                    CreatedDate = y.CreatedDate,
                }).ToList();

                if (dialogAssessmentScores.Count > 0)
                {
                    AssessmentStatus = Service.ResultMessage("success", "1");
                    AssessmentStatus.AcceptChanges();
                    DialogAssessmentScore = Service.ConvertToDataTable(dialogAssessmentScores);
                    DialogAssessmentScore.TableName = "DialogAssessmentScore";
                }
                else
                {
                    //Table = Service.Message("success", "1");
                    //Table.AcceptChanges();
                    AssessmentStatus = Service.ResultMessage("No Data", "0");
                    AssessmentStatus.TableName = "DialogAssessmentScore";
                }
                //var leftOuterJoin = (from dialog in icanSpeakContext.Dialogs
                //                    join score in icanSpeakContext.DialogAssessmentScores
                //                    on dialog.DialogId equals score.DialogId
                //                    into temp
                //                    from score in temp.DefaultIfEmpty()
                //                    where score.UserId == userid && score.Type == "Dialog"
                //                    select new
                //                    {
                //                        score.ScoreId,
                //                        //score.DialogId,
                //                        score.UserId,
                //                        score.Score,
                //                        score.TotalScore,
                //                        score.ModifiedDate,
                //                        score.CreatedDate
                //                    }).OrderByDescending(y => y.CreatedDate);

                //var leftOuterJoins = leftOuterJoin.AsEnumerable().Select(y => new
                //{
                //    RowIndex = scoreindex++,
                //    ScoreId = y.ScoreId,
                //    //DialogId = y.DialogId,
                //    UserId = y.UserId,
                //    Score = y.Score,
                //    TotalScore = y.TotalScore,
                //    ModifiedDate = y.ModifiedDate,
                //    CreatedDate = y.CreatedDate,
                //}).ToList();


                //var rightOuterJoin = (from score in icanSpeakContext.DialogAssessmentScores
                //                     join dialog in icanSpeakContext.Dialogs
                //                     on score.DialogId equals dialog.DialogId
                //                     into temp
                //                     from dialog in temp.DefaultIfEmpty()
                //                     where dialog.IsFree == true && dialog.DeleteDate == null
                //                     select new
                //                     {
                //                         DialogId = Service.Encrypt(dialog.DialogId.ToString()),
                //                         DialogEnglish = dialog.EnglishName,
                //                         DialogArabic = dialog.ArabicName,
                //                         dialog.CreateDate
                //                     }).OrderByDescending(y => y.CreateDate);

                //var rightOuterJoins = rightOuterJoin.AsEnumerable().Select(x => new
                //{
                //    RowIndex = dialogindex++,
                //    DialogId = x.DialogId,
                //    DialogEnglish = x.DialogEnglish,
                //    DialogArabic = x.DialogArabic,
                //    Date = x.CreateDate
                //}).ToList();

                //var fullOuterJoin = 

                //create datatable, right  - add columns for left, then check if left one has value and if it is null - ""/nil else insert value.

                //=======================
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

                ds.Tables.Add(Table);
                ds.Tables.Add(Dialog);
                ds.Tables.Add(DialogAssessmentScore);
                ds.Tables.Add(MyscoreDataStatus);
                ds.Tables.Add(Myscores);
                ds.Tables.Add(AssessmentStatus);
                ds.Tables.Add(DialogStatus);

                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetDialogByUserId");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }


        //===================================================================================================================\\
        public Stream GetDialogConversation(Stream objStream)
        {
            Dialog objDialog = new Dialog();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"UserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                int conversationindex = 1;
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int dialogid = Convert.ToInt32(Service.Decrypt(userRequest["DialogId"]));

                var query = (from conversation in icanSpeakContext.DialogConversations
                             join dialog in icanSpeakContext.Dialogs on conversation.DialogId equals dialog.DialogId
                             where conversation.DialogId == dialogid

                             select new
                             {
                                 DialogId = Service.Encrypt(conversation.DialogId.ToString()),
                                 conversation.Person1Text,
                                 conversation.Person2Text,
                                 conversation.Person1ArabicText,
                                 conversation.Person2ArabicText,
                                 conversation.CreateDate,
                             }).OrderBy(i => i.CreateDate);


                if (Convert.ToString(query.Count()) != "0")
                {
                    var conversations = query.AsEnumerable().Select(x => new
                    {
                        RowIndex = conversationindex++,
                        Person1Text = x.Person1Text,
                        Person2Text = x.Person2Text,
                        Person1ArabicText = x.Person1ArabicText,
                        Person2ArabicText = x.Person2ArabicText,
                        DialogId = x.DialogId
                    }).ToList();


                    var videourl = from dialog in icanSpeakContext.Dialogs
                                   where dialog.DialogId == dialogid
                                   select new
                                   {
                                       VideoUrl = Service.GetUrl() + "DialogVideo/" + dialog.VideoUrl,
                                       DialogName = dialog.EnglishName
                                   };


                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable Conversation = Service.ConvertToDataTable(conversations);
                    Conversation.TableName = "Conversation";
                    DataTable Video = Service.ConvertToDataTable(videourl);
                    Video.TableName = "Video";
                    ds.Tables.Add(Table);
                    ds.Tables.Add(Video);
                    ds.Tables.Add(Conversation);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
                else
                {
                    Table = Service.Message("No Data", "0");
                    ds.Tables.Add(Table);
                    return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
                }

            }
            catch (Exception ex)
            {
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }


        public string BookMarkStatus(int wordId, int userid)
        {
            string result = string.Empty;
            var status = (from Book in icanSpeakContext.BookMarks
                          where Book.CourseId == wordId && Book.UserId == userid
                          select new { Book.BookmarkId }
                         ).ToList();

            if (status.Count > 0)
            {
                result = "1";
            }
            else
            {
                result = "0";
            }

            return result;
        }


        public Stream GetDialogDetails(Stream objStream)
        {
            Dialog objDialog = new Dialog();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            DataTable ConversationDataStatus = new DataTable();
            DataTable KeyPhraseDataStatus = new DataTable();
            DataTable Dialogs = new DataTable();
            DataTable Conversation = new DataTable();
            DataTable KeyPhrase = new DataTable();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"UserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int dialogid = Convert.ToInt32(Service.Decrypt(userRequest["DialogId"]));
                int userid = Convert.ToInt32(Service.Decrypt(userRequest["userid"]));

                var lastDialogID = (from dialog in icanSpeakContext.Dialogs
                                    where dialog.IsFree == true && dialog.DeleteDate == null
                                    orderby dialog.DialogId 
                                    select new { dialog.DialogId }).Take(1).ToList();

                  var dialogids = (from dialog in icanSpeakContext.Dialogs
                                   where dialog.IsFree == true && dialog.DeleteDate == null && dialog.DialogId == dialogid
                                   orderby dialog.DialogId descending
                                   select new
                                    {
                                        DialogId = dialog.DialogId

                                    }).ToList();

                //if (dialogids.Count > 0)
                //{
                var dialogresult = (from dialog in icanSpeakContext.Dialogs
                                    where dialog.DialogId == dialogid
                                    select new
                                    {
                                        DialogId = Service.Encrypt(dialog.DialogId.ToString()),
                                        VideoUrl = Service.GetUrl() + "DialogVideo/" + dialog.VideoUrl,
                                        EnglishSubtitleUrl = Service.GetUrl() + "DialogSubtitle/" + dialog.EnglishSubtitleUrl,
                                        ArabicSubtitleUrl = Service.GetUrl() + "DialogSubtitle/" + dialog.ArabicSubtitleUrl,
                                        BothSubtitleUrl = Service.GetUrl() + "DialogSubtitle/" + dialog.BothSubtitleUrl,
                                        DialogName = dialog.EnglishName,
                                        dialog.DescriptionEnglish,
                                        dialog.DescriptionArabic,
                                        BookMarkStatus = BookMarkStatus(Convert.ToInt32(dialog.DialogId), userid),
                                       // Score = score.Score
                                    }).Take(1).ToList();
                var scored = (from Score in icanSpeakContext.DialogAssessmentScores
                              where Score.DialogId == dialogid
                              select new
                              {
                                  UserScore = Score.Score
                              }).ToList();

                if (dialogresult.Count > 0)
                {
                    //================================Dialog Section Starts=====================================================================\\
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    Dialogs = Service.ConvertToDataTable(dialogresult);
                    DataColumn newCol = new DataColumn("isLastData", typeof(string));
                    DataColumn newCol1 = new DataColumn("isScore", typeof(string));
                    newCol1.AllowDBNull = true;
                    newCol.AllowDBNull = true;
                    Dialogs.Columns.Add(newCol);
                    Dialogs.Columns.Add(newCol1);
                    if (scored.Count > 0)
                    {
                        if (scored[0].UserScore >= 10)
                        {
                            foreach (DataRow row in Dialogs.Rows)
                            {
                                row["isScore"] = "1";
                            }

                        }
                    }
                    else
                    {
                        foreach (DataRow row in Dialogs.Rows)
                        {
                            row["isScore"] = "0";
                        }
                    }
                    if (dialogids[0].DialogId == lastDialogID.ElementAtOrDefault(0).DialogId)
                    {
                        foreach (DataRow row in Dialogs.Rows)
                        {
                            row["isLastData"] = "1";
                        }
                    }
                    else
                    {
                        foreach (DataRow row in Dialogs.Rows)
                        {
                            row["isLastData"] = "0";
                        }
                    }

                    Dialogs.TableName = "Dialog";
                //}
                //else
                //{
                //    Table = Service.Message("No Data", "0");
                //    Table.AcceptChanges();
                //}
                    //================================Dialog Section Ends=====================================================================\\

                    //================================Conversation Section=====================================================================\\

                    var conversations = (from conversation in icanSpeakContext.DialogConversations
                                         join dialog in icanSpeakContext.Dialogs on conversation.DialogId equals dialog.DialogId
                                         where conversation.DialogId == dialogid

                                         select new
                                         {
                                             DialogId = Service.Encrypt(conversation.DialogId.ToString()),
                                             conversation.Person1Text,
                                             conversation.Person2Text,
                                             conversation.Person1ArabicText,
                                             conversation.Person2ArabicText,
                                             conversation.CreateDate,
                                         }).OrderBy(i => i.CreateDate).ToList();

                    if (conversations.Count > 0)
                    {
                        ConversationDataStatus = Service.DataStatus("success", "1");
                        ConversationDataStatus.TableName = "ConversationDataStatus";
                        Conversation = Service.ConvertToDataTable(conversations);
                        Conversation.TableName = "Conversation";

                    }
                    else
                    {
                        ConversationDataStatus = Service.DataStatus("No Data", "0");
                        ConversationDataStatus.TableName = "ConversationDataStatus";
                    }
                    //================================Conversation Section Ends=====================================================================\\

                    //================================KeyPhrases Section Starts====================================================================\\
                    var KeyPhrases = (from keyphrases in icanSpeakContext.DialogKeyPhrases
                                      where keyphrases.DialogId == dialogid
                                      select new
                                      {
                                          keyphrases.DialogId,
                                          keyphrases.KeyPhrasesId,
                                          keyphrases.ArabicText,
                                          keyphrases.EnglishText,
                                      }).ToList();
                    if (KeyPhrases.Count > 0)
                    {
                        KeyPhraseDataStatus = Service.DataStatus("success", "1");
                        KeyPhraseDataStatus.TableName = "KeyPhraseDataStatus";
                        KeyPhrase = Service.ConvertToDataTable(KeyPhrases);
                        KeyPhrase.TableName = "KeyPhrase";
                    }
                    else
                    {
                        KeyPhraseDataStatus = Service.DataStatus("No Data", "0");
                        KeyPhraseDataStatus.TableName = "KeyPhraseDataStatus";
                    }
                    //================================KeyPhrases Section Ends====================================================================\\

                    //====================================Final Section Starts=========================================================================\\
                    ds.Tables.Add(Table);
                    ds.Tables.Add(Dialogs);
                    ds.Tables.Add(ConversationDataStatus);
                    ds.Tables.Add(Conversation);
                    ds.Tables.Add(KeyPhraseDataStatus);
                    ds.Tables.Add(KeyPhrase);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                    //====================================Final Section Ends=========================================================================\\

                }
                else
                {
                    Table = Service.Message("No Data", "1");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetDialogByDialogId");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }

        public Stream NextDialogDetails(Stream objStream)
        {
            Dialog objDialog = new Dialog();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            DataTable ConversationDataStatus = new DataTable();
            DataTable KeyPhraseDataStatus = new DataTable();
            DataTable Dialogs = new DataTable();
            DataTable Conversation = new DataTable();
            DataTable KeyPhrase = new DataTable();
            DataTable Scores = new DataTable();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"UserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int requestdialogid = Convert.ToInt32(Service.Decrypt(userRequest["DialogId"]));
                int userid = Convert.ToInt32(Service.Decrypt(userRequest["userid"]));

                var lastDialogID = (from dialog in icanSpeakContext.Dialogs
                                    where dialog.IsFree == true && dialog.DeleteDate == null
                                    orderby dialog.DialogId
                                    select new { dialog.DialogId }).Take(1).ToList();


                var dialogids = (from dialog in icanSpeakContext.Dialogs
                                 where dialog.DialogId < requestdialogid && dialog.IsFree == true && dialog.DeleteDate == null
                                 orderby dialog.DialogId descending
                                 select new { dialog.DialogId }).Take(1).ToList();


                if (dialogids.Count > 0)
                {
                    int dialogid = Convert.ToInt32(dialogids.FirstOrDefault().DialogId);

                    var dialogresult = (from dialog in icanSpeakContext.Dialogs
                                       
                                        where dialog.DialogId == dialogid && dialog.IsFree == true && dialog.DeleteDate == null
                                        select new
                                        {
                                            DialogId = Service.Encrypt(dialog.DialogId.ToString()),
                                            VideoUrl = Service.GetUrl() + "DialogVideo/" + dialog.VideoUrl,
                                            DialogName = dialog.EnglishName,
                                            dialog.DescriptionEnglish,
                                            dialog.DescriptionArabic,
                                            BookMarkStatus = BookMarkStatus(Convert.ToInt32(dialog.DialogId), userid),
                                            //Score = score.Score
                                        }).ToList();


                    var scored = (from Score in icanSpeakContext.DialogAssessmentScores
                                  where Score.DialogId == Convert.ToInt32(Service.Decrypt(dialogresult[0].DialogId))
                                  select new
                                  {
                                      UserScore = Score.Score
                                  }).ToList();
                                 

                    if (dialogresult.Count > 0)
                    {
                        //================================Dialog Section Starts=====================================================================\\
                        Table = Service.Message("success", "1");
                        Table.AcceptChanges();
                        Dialogs = Service.ConvertToDataTable(dialogresult);
                        //Scores = Service.ConvertToDataTable(Score);
                        DataColumn newCol = new DataColumn("isLastData", typeof(string));
                        DataColumn newCol1 = new DataColumn("isScore" , typeof(string));
                        newCol1.AllowDBNull = true;
                        newCol.AllowDBNull = true;
                        Dialogs.Columns.Add(newCol);
                        Dialogs.Columns.Add(newCol1);
                        if (scored.Count > 0)
                        {
                            if (scored[0].UserScore >= 10)
                            {
                                foreach (DataRow row in Dialogs.Rows)
                                {
                                    row["isScore"] = "1";
                                }

                            }
                        }
                        else
                        {
                            foreach (DataRow row in Dialogs.Rows)
                            {
                                row["isScore"] = "0";
                            }
                        }

                        if (dialogids.ElementAtOrDefault(0).DialogId == lastDialogID.ElementAtOrDefault(0).DialogId)
                        {
                            foreach (DataRow row in Dialogs.Rows)
                            {
                                row["isLastData"] = "1";
                            }
                        }

                        else
                        {
                            foreach (DataRow row in Dialogs.Rows)
                            {
                                row["isLastData"] = "0";
                            }
                        }
                        Dialogs.TableName = "Dialog";
                    }
                    else
                    {
                        Table = Service.Message("No Data", "0");
                        Table.AcceptChanges();
                    }

                    //================================Dialog Section Ends=====================================================================\\

                    //================================Conversation Section=====================================================================\\

                    var conversations = (from conversation in icanSpeakContext.DialogConversations
                                         join dialog in icanSpeakContext.Dialogs on conversation.DialogId equals dialog.DialogId
                                         where conversation.DialogId == dialogid

                                         select new
                                         {
                                             DialogId = Service.Encrypt(conversation.DialogId.ToString()),
                                             conversation.Person1Text,
                                             conversation.Person2Text,
                                             conversation.Person1ArabicText,
                                             conversation.Person2ArabicText,
                                             conversation.CreateDate,
                                         }).OrderBy(i => i.CreateDate).ToList();

                    if (conversations.Count > 0)
                    {
                        ConversationDataStatus = Service.DataStatus("success", "1");
                        ConversationDataStatus.TableName = "ConversationDataStatus";
                        Conversation = Service.ConvertToDataTable(conversations);
                        Conversation.TableName = "Conversation";

                    }
                    else
                    {
                        ConversationDataStatus = Service.DataStatus("No Data", "0");
                        ConversationDataStatus.TableName = "ConversationDataStatus";
                    }
                    //================================Conversation Section Ends=====================================================================\\

                    //================================KeyPhrases Section Starts====================================================================\\
                    var KeyPhrases = (from keyphrases in icanSpeakContext.DialogKeyPhrases
                                      where keyphrases.DialogId == dialogid
                                      select new
                                      {
                                          keyphrases.DialogId,
                                          keyphrases.KeyPhrasesId,
                                          keyphrases.ArabicText,
                                          keyphrases.EnglishText,
                                      }).ToList();
                    if (KeyPhrases.Count > 0)
                    {
                        KeyPhraseDataStatus = Service.DataStatus("success", "1");
                        KeyPhraseDataStatus.TableName = "KeyPhraseDataStatus";
                        KeyPhrase = Service.ConvertToDataTable(KeyPhrases);
                        KeyPhrase.TableName = "KeyPhrase";
                    }
                    else
                    {
                        KeyPhraseDataStatus = Service.DataStatus("No Data", "0");
                        KeyPhraseDataStatus.TableName = "KeyPhraseDataStatus";
                    }
                    //================================KeyPhrases Section Ends====================================================================\\

                    //====================================Final Section Starts=========================================================================\\
                    ds.Tables.Add(Table);
                    ds.Tables.Add(Dialogs);
                    ds.Tables.Add(ConversationDataStatus);
                    ds.Tables.Add(Conversation);
                    ds.Tables.Add(KeyPhraseDataStatus);
                    ds.Tables.Add(KeyPhrase);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                    //====================================Final Section Ends=========================================================================\\

                }
                else
                {
                    Table = Service.Message("No Data", "0");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }


            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetDialogByDialogId");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }


        public Stream BackDialogDetails(Stream objStream)
        {
            Dialog objDialog = new Dialog();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            DataTable ConversationDataStatus = new DataTable();
            DataTable KeyPhraseDataStatus = new DataTable();
            DataTable Dialogs = new DataTable();
            DataTable Conversation = new DataTable();
            DataTable KeyPhrase = new DataTable();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"UserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int requestdialogid = Convert.ToInt32(Service.Decrypt(userRequest["DialogId"]));
                int userid = Convert.ToInt32(Service.Decrypt(userRequest["userid"]));

                var FirstDialogID = (from dialog in icanSpeakContext.Dialogs
                                     where dialog.IsFree == true && dialog.DeleteDate == null
                                     orderby dialog.DialogId descending
                                     select new { dialog.DialogId }).Take(1).ToList();

                var dialogids = (from dialog in icanSpeakContext.Dialogs
                                 where dialog.DialogId > requestdialogid && dialog.IsFree == true && dialog.DeleteDate == null
                                 orderby dialog.DialogId
                                 select new { dialog.DialogId }).Take(1).ToList();

                if (dialogids.Count > 0)
                {
                    int dialogid = Convert.ToInt32(dialogids.FirstOrDefault().DialogId);

                    var dialogresult = (from dialog in icanSpeakContext.Dialogs
                                        where dialog.DialogId == dialogid && dialog.IsFree == true && dialog.DeleteDate == null
                                        select new
                                        {
                                            DialogId = Service.Encrypt(dialog.DialogId.ToString()),
                                            VideoUrl = Service.GetUrl() + "DialogVideo/" + dialog.VideoUrl,
                                            DialogName = dialog.EnglishName,
                                            dialog.DescriptionEnglish,
                                            dialog.DescriptionArabic,
                                            BookMarkStatus = BookMarkStatus(Convert.ToInt32(dialog.DialogId), userid)
                                        }).ToList();

                    var scored = (from Score in icanSpeakContext.DialogAssessmentScores
                                  where Score.DialogId == Convert.ToInt32(Service.Decrypt(dialogresult[0].DialogId))
                                  select new
                                  {
                                      UserScore = Score.Score
                                  }).ToList();

                    if (dialogresult.Count > 0)
                    {
                        //================================Dialog Section Starts=====================================================================\\
                        Table = Service.Message("success", "1");
                        Table.AcceptChanges();
                        Dialogs = Service.ConvertToDataTable(dialogresult);
                        DataColumn newCol = new DataColumn("isLastData", typeof(string));
                        DataColumn newCol1 = new DataColumn("isScore", typeof(string));
                        newCol1.AllowDBNull = true;
                        newCol.AllowDBNull = true;
                        Dialogs.Columns.Add(newCol);
                        Dialogs.Columns.Add(newCol1);

                        if (scored.Count > 0)
                        {
                            if (scored[0].UserScore >= 10)
                            {
                                foreach (DataRow row in Dialogs.Rows)
                                {
                                    row["isScore"] = "1";
                                }

                            }
                        }
                        else
                        {
                            foreach (DataRow row in Dialogs.Rows)
                            {
                                row["isScore"] = "0";
                            }
                        }

                        foreach (DataRow row in Dialogs.Rows)
                        {
                            row["isLastData"] = "0";
                        }
                        if (dialogids.ElementAtOrDefault(0).DialogId == FirstDialogID.ElementAtOrDefault(0).DialogId)
                        {
                            foreach (DataRow row in Dialogs.Rows)
                            {
                                row["isLastData"] = "1";
                            }
                        }
                        Dialogs.TableName = "Dialog";
                    }
                    else
                    {
                        Table = Service.Message("No Data", "0");
                        Table.AcceptChanges();
                    }
                    //================================Dialog Section Ends=====================================================================\\

                    //================================Conversation Section=====================================================================\\

                    var conversations = (from conversation in icanSpeakContext.DialogConversations
                                         join dialog in icanSpeakContext.Dialogs on conversation.DialogId equals dialog.DialogId
                                         where conversation.DialogId == dialogid

                                         select new
                                         {
                                             DialogId = Service.Encrypt(conversation.DialogId.ToString()),
                                             conversation.Person1Text,
                                             conversation.Person2Text,
                                             conversation.Person1ArabicText,
                                             conversation.Person2ArabicText,
                                             conversation.CreateDate,
                                         }).OrderBy(i => i.CreateDate).ToList();

                    if (conversations.Count > 0)
                    {
                        ConversationDataStatus = Service.DataStatus("success", "1");
                        ConversationDataStatus.TableName = "ConversationDataStatus";
                        Conversation = Service.ConvertToDataTable(conversations);
                        Conversation.TableName = "Conversation";

                    }
                    else
                    {
                        ConversationDataStatus = Service.DataStatus("No Data", "0");
                        ConversationDataStatus.TableName = "ConversationDataStatus";
                    }
                    //================================Conversation Section Ends=====================================================================\\

                    //================================KeyPhrases Section Starts====================================================================\\
                    var KeyPhrases = (from keyphrases in icanSpeakContext.DialogKeyPhrases
                                      where keyphrases.DialogId == dialogid
                                      select new
                                      {
                                          keyphrases.DialogId,
                                          keyphrases.KeyPhrasesId,
                                          keyphrases.ArabicText,
                                          keyphrases.EnglishText,
                                      }).ToList();
                    if (KeyPhrases.Count > 0)
                    {
                        KeyPhraseDataStatus = Service.DataStatus("success", "1");
                        KeyPhraseDataStatus.TableName = "KeyPhraseDataStatus";
                        KeyPhrase = Service.ConvertToDataTable(KeyPhrases);
                        KeyPhrase.TableName = "KeyPhrase";
                    }
                    else
                    {
                        KeyPhraseDataStatus = Service.DataStatus("No Data", "0");
                        KeyPhraseDataStatus.TableName = "KeyPhraseDataStatus";
                    }
                    //================================KeyPhrases Section Ends====================================================================\\

                    //====================================Final Section Starts=========================================================================\\
                    ds.Tables.Add(Table);
                    ds.Tables.Add(Dialogs);
                    ds.Tables.Add(ConversationDataStatus);
                    ds.Tables.Add(Conversation);
                    ds.Tables.Add(KeyPhraseDataStatus);
                    ds.Tables.Add(KeyPhrase);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                    //====================================Final Section Ends=========================================================================\\


                }
                else
                {
                    Table = Service.Message("No Data", "0");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }


            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetDialogByDialogId");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }


        public Stream GetAssementByDialogId(Stream objStream)
        {
            Dialog objDialog = new Dialog();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
            try
            {
                var results = (from question in icanSpeakContext.DialogAssessmentQuestions
                               where question.DialogId == Convert.ToInt32(Service.Decrypt(userRequest["dialogId"]))
                               select new
                               {
                                   QuestionId = Service.Encrypt(question.QuestionId.ToString()),
                                   DialogId = Service.Encrypt(question.DialogId.ToString()),
                                   question.Question,
                                   question.QuestionType,
                                   question.FillAnswerText,
                                   question.OptionText1,
                                   question.OptionText2,
                                   question.OptionText3,
                                   OptionAudio1 = Service.GetUrl() + "DialogAssessmentQuestionsAudio/" + question.OptionAudio1,
                                   OptionAudio2 = Service.GetUrl() + "DialogAssessmentQuestionsAudio/" + question.OptionAudio2,
                                   OptionAudio3 = Service.GetUrl() + "DialogAssessmentQuestionsAudio/" + question.OptionAudio3,
                                   question.TrueFalseAnswer,
                                   question.OptionCorrectAnswer,
                                   question.CreateDate
                               }).OrderBy(x => x.CreateDate).Take(1).ToList();

                if (results.Count > 0)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable Dialog = Service.ConvertToDataTable(results);
                    Dialog.TableName = "DialogQuestion";
                    ds.Tables.Add(Table);
                    ds.Tables.Add(Dialog);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
                else
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable ResultMessage = Service.ResultMessage("No Data", "");
                    ds.Tables.Add(Table);
                    ds.Tables.Add(ResultMessage);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAssementByDialogId");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }

        public Stream GetNextAssementByDialogId(Stream objStream)
        {
            Dialog objDialog = new Dialog();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
            int questionid = Convert.ToInt32(Service.Decrypt(userRequest["questionId"]));
            int userid = Convert.ToInt32(Service.Decrypt(userRequest["userId"]));
            string coursetype = userRequest["coursetype"];
            int courseid = Convert.ToInt32(Service.Decrypt(userRequest["courseId"]));
            try
            {
                //=================================Assessement Score Table ENtry Starts==================================================================\\
                var check = (from score in icanSpeakContext.AssessmentScores
                             where score.UserId == userid && score.CourseType == "Dialog" && score.CourseId == courseid
                             && score.QuestionId == questionid
                             select new
                             {
                                 score.AssessmentScoreId
                             
                             }).ToList();
                if(check.Count == 0)
                {
                AssessmentScore objscore = new AssessmentScore();
                objscore.UserId = userid;
                objscore.CourseId = Convert.ToInt32(Service.Decrypt(userRequest["courseId"]));
                objscore.QuestionId = questionid;
                objscore.CourseType = userRequest["coursetype"];
                if (userRequest["status"] == "true")
                {
                    objscore.IsQuestionCorrect = true;
                }
                else
                {
                    objscore.IsQuestionCorrect = false;
                }
                objscore.CreatedDate = System.DateTime.Now;
                icanSpeakContext.AssessmentScores.InsertOnSubmit(objscore);
                icanSpeakContext.SubmitChanges();
                }
                else
                {
                    AssessmentScore objscore = new AssessmentScore();
                    objscore = icanSpeakContext.AssessmentScores.Single(key => key.AssessmentScoreId == check[0].AssessmentScoreId);
                    objscore.UserId = userid;
                    objscore.CourseId = Convert.ToInt32(Service.Decrypt(userRequest["courseId"]));
                    objscore.QuestionId = questionid;
                    objscore.CourseType = userRequest["coursetype"];
                    if (userRequest["status"] == "true")
                    {
                        objscore.IsQuestionCorrect = true;
                    }
                    else
                    {
                        objscore.IsQuestionCorrect = false;
                    }
                    objscore.CreatedDate = System.DateTime.Now;
                    //icanSpeakContext.AssessmentScores.InsertOnSubmit(objscore);
                    icanSpeakContext.SubmitChanges();
                    //return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
                }
                //=================================Assessement Score Table ENtry Ends==================================================================\\


                var UserData = (from user in icanSpeakContext.Users
                                where user.UserId == userid
                                select
                                user).FirstOrDefault();


                //==============If Answer Correct, Increment User My Score Starts=======================================================================\\
                if (userRequest["status"] == "true")
                {
                    UserData.MyScore = UserData.MyScore + 5;
                }
                //==============If Answer Correct, Increment User My Score Ends=======================================================================\\
                UserData.TotalScore = UserData.TotalScore + 5;
                icanSpeakContext.SubmitChanges();



                var results = (from question in icanSpeakContext.DialogAssessmentQuestions
                               where question.QuestionId > questionid &&
                               question.DialogId == Convert.ToInt32(Service.Decrypt(userRequest["dialogId"]))
                               orderby question.QuestionId ascending
                               select new
                               {
                                   QuestionId = Service.Encrypt(question.QuestionId.ToString()),
                                   DialogId = Service.Encrypt(question.DialogId.ToString()),
                                   question.Question,
                                   question.QuestionType,
                                   question.FillAnswerText,
                                   question.OptionText1,
                                   question.OptionText2,
                                   question.OptionText3,
                                   OptionAudio1 = Service.GetUrl() + "DialogAssessmentQuestionsAudio/" + question.OptionAudio1,
                                   OptionAudio2 = Service.GetUrl() + "DialogAssessmentQuestionsAudio/" + question.OptionAudio2,
                                   OptionAudio3 = Service.GetUrl() + "DialogAssessmentQuestionsAudio/" + question.OptionAudio3,
                                   question.TrueFalseAnswer,
                                   question.OptionCorrectAnswer,
                                   question.CreateDate
                               }).OrderBy(x => x.CreateDate).Take(1).ToList();

                if (results.Count > 0)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable Dialog = Service.ConvertToDataTable(results);
                    Dialog.TableName = "DialogQuestion";
                    ds.Tables.Add(Table);
                    ds.Tables.Add(Dialog);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
                else
                {
                    var total = 0;
                    var scored = 0;

                    var result1 = (from score in icanSpeakContext.AssessmentScores
                                   where score.UserId == userid && score.CourseType == userRequest["coursetype"] && score.CourseId == courseid
                                   select score).Count();
                    if(result1 > 0)
                    {
                         total = result1 * 5;

                        var result2 = (from score in icanSpeakContext.AssessmentScores
                                       where score.UserId == userid && score.CourseType == userRequest["coursetype"]
                                       && score.IsQuestionCorrect == true && score.CourseId == courseid
                                       select score).Count();
                         scored = result2 * 5;

                         var checkScore = (from dialogAssesmentScore in icanSpeakContext.DialogAssessmentScores
                                           where dialogAssesmentScore.UserId == userid && dialogAssesmentScore.DialogId == courseid
                                           select dialogAssesmentScore).FirstOrDefault();
                         if (checkScore == null)
                         {

                             DialogAssessmentScore objDialogScore = new DialogAssessmentScore();
                             objDialogScore.Score = scored;
                             objDialogScore.TotalScore = total;
                             objDialogScore.DialogId = courseid;
                             objDialogScore.UserId = userid;
                             objDialogScore.Type = "Dialog";
                             objDialogScore.CreatedDate = System.DateTime.Now;
                             icanSpeakContext.DialogAssessmentScores.InsertOnSubmit(objDialogScore);
                             icanSpeakContext.SubmitChanges();
                             //return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));

                         }
                         else
                         {
                             DialogAssessmentScore objDialogScore = new DialogAssessmentScore();
                             objDialogScore = icanSpeakContext.DialogAssessmentScores.Single(key => key.ScoreId == checkScore.ScoreId);
                             objDialogScore.Score = scored;
                             objDialogScore.TotalScore = total;
                             objDialogScore.DialogId = Convert.ToInt32(Service.Decrypt(userRequest["courseId"]));
                             objDialogScore.UserId = userid;
                             objDialogScore.Type = "Dialog";
                             objDialogScore.ModifiedDate = System.DateTime.Now;
                             icanSpeakContext.SubmitChanges();
                             //return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
                           
                         }
                    }
                    Table = Service.Message("No Data", "1");
                    Table.AcceptChanges();
                    DataTable ResultMessage = Service.ResultMessage("No Data", "");
                    DataTable dt = new DataTable();
                    dt.TableName = "Score";
                    dt.Columns.Add("TotalScore");
                    dt.Columns.Add("MyScore");
                    DataRow usernewrow = dt.NewRow();
                    usernewrow["TotalScore"] = total;
                    usernewrow["MyScore"] = scored;
                    dt.Rows.Add(usernewrow);
                    dt.AcceptChanges();
                    ds.Tables.Add(Table);
                    ds.Tables.Add(ResultMessage);
                    ds.Tables.Add(dt);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAssementByDialogId");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }


    }
}