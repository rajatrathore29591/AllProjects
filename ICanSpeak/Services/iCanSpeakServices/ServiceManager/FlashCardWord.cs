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
using System.Reflection;
using System.ServiceModel.Web;
using System.Data.Objects.SqlClient;
using System.Globalization;



namespace iCanSpeakServices.ServiceManager
{
    public class FlashCardWord
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataTable Table = new DataTable();

        public string FlashCardStatus(int wordId, int userid)
        {
            string result = string.Empty;
            var status = (from flash in icanSpeakContext.FlashCards
                          where flash.WordId == wordId && flash.UserId == userid
                          select new { flash.FlashCardId }
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


        public Stream AddFlashCardWord(Stream objStream)
        {

            //Exception ex1 = new Exception();
            //Helper.ErrorLog(ex1, requestString);
            DataSet ds = new DataSet("Result");
            String requestString = string.Empty;
            try
            {

                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                requestString = reader.ReadToEnd();
                var paramter = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                FlashCard objcard = new FlashCard();
                objcard.UserId = Convert.ToInt32(Service.Decrypt(paramter["userid"]));
                objcard.WordId = Convert.ToInt32(Service.Decrypt(paramter["wordid"]));
                objcard.CreatedDate = System.DateTime.Now;
                icanSpeakContext.FlashCards.InsertOnSubmit(objcard);
                icanSpeakContext.SubmitChanges();

                var UserData = (from user in icanSpeakContext.Users
                                where user.UserId == Convert.ToInt32(Service.Decrypt(paramter["userid"]))
                                select
                                user).FirstOrDefault();

                UserData.FlashCard = UserData.FlashCard + 1;
                icanSpeakContext.SubmitChanges();

                var wordname = (from word in icanSpeakContext.VocabularyWords
                                where word.WordId == Convert.ToInt32(Service.Decrypt(paramter["wordid"]))
                                select word
                               ).FirstOrDefault();

                string wordnames = wordname.EnglishText;

                MyActivity objmyactivity = new MyActivity();
                objmyactivity.UserId = Convert.ToInt32(Service.Decrypt(paramter["userid"]));
                objmyactivity.Message = "You have added a word " + wordnames + " to flash card.";
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

        public Stream RemoveFlashCardWord(Stream objStream)
        {

            //Exception ex1 = new Exception();
            //Helper.ErrorLog(ex1, requestString);
            DataSet ds = new DataSet("Result");
            String requestString = string.Empty;
            try
            {

                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                requestString = reader.ReadToEnd();
                var paramter = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var summary_delete = (from flash in icanSpeakContext.FlashCards
                                      where flash.WordId == Convert.ToInt32(Service.Decrypt(paramter["wordid"]))
                                      && flash.UserId == Convert.ToInt32(Service.Decrypt(paramter["userid"]))
                                      select flash).FirstOrDefault();
                icanSpeakContext.FlashCards.DeleteOnSubmit(summary_delete);
                icanSpeakContext.SubmitChanges();

                var UserData = (from user in icanSpeakContext.Users
                                where user.UserId == Convert.ToInt32(Service.Decrypt(paramter["userid"]))
                                select
                                user).FirstOrDefault();

                UserData.FlashCard = UserData.FlashCard - 1;
                icanSpeakContext.SubmitChanges();

                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                DataTable ResultMessage = Service.ResultMessage("delete", "1");
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


        public Stream FlashCardByUserId(Stream objStream)
        {

            //Exception ex1 = new Exception();
            //Helper.ErrorLog(ex1, requestString);
            DataSet ds = new DataSet("Result");
            String requestString = string.Empty;
            try
            {

                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                requestString = reader.ReadToEnd();
                var paramter = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int userid = Convert.ToInt32(Service.Decrypt(paramter["userid"]));

                var result = (from flash in icanSpeakContext.FlashCards
                              join word in icanSpeakContext.VocabularyWords on flash.WordId equals Convert.ToInt32(word.WordId)
                              join vocab in icanSpeakContext.Vocabularies on word.VocabularyId equals Convert.ToInt32(vocab.VocabularyId)
                              where flash.UserId == userid
                              select new
                              {
                                  FlashCardId = Service.Encrypt(flash.FlashCardId.ToString()),
                                  WordId = Service.Encrypt(flash.WordId.ToString()),
                                  VocabularyId = Service.Encrypt(vocab.VocabularyId.ToString()),
                                  Word = word.EnglishText,
                                  flash.CreatedDate
                              }
                            ).OrderByDescending(x => x.CreatedDate).ToList();

                if (result.Count > 0)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable FlashCard = Service.ConvertToDataTable(result);
                    FlashCard.TableName = "FlashCard";
                    ds.Tables.Add(Table);
                    ds.Tables.Add(FlashCard);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
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

                Helper.ErrorLog(ex, requestString);
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));// javaScriptSerializer.Serialize(ex.Message.ToString());


            }

        }

        public string ChechlastFlashCardsId(int flashcardid, int userid)
        {
            string result = string.Empty;
            var lastFlashCardsId = (from flashCount in icanSpeakContext.FlashCards
                                    where flashCount.UserId == userid
                                    orderby flashCount.FlashCardId
                                    select new
                                    {
                                        flashCount.FlashCardId
                                    }).Take(1).ToList();

            for (int i = 0; i < lastFlashCardsId.Count; i++)
            {
                if (flashcardid == (i))
                {
                    result = "1";
                }
                else
                {
                    result = "0";
                }
            }

            return result;
        }

        public Stream NextFlashCardDetail(Stream objStream)
        {
            DataSet ds = new DataSet();
            try
            {
                VocabularyWord vocabularyWord = new VocabularyWord();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                var vocabWordData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int wordId = Convert.ToInt32(Service.Decrypt(vocabWordData["wordId"]));
                int vocabid = Convert.ToInt32(Service.Decrypt(vocabWordData["vocabid"]));
                int userid = Convert.ToInt32(Service.Decrypt(vocabWordData["userid"]));
                int flashcardid = Convert.ToInt32(Service.Decrypt(vocabWordData["flashcardid"]));


                string subcategoryname = string.Empty;
                var subcategorys = (from subcategory in icanSpeakContext.VocabularySubCategories
                                    where subcategory.VocabularyId == vocabid
                                    select new
                                    {
                                        subcategory.SubCategoryName

                                    }).FirstOrDefault();
                if (subcategorys != null)
                {
                    subcategoryname = subcategorys.SubCategoryName;
                }
                else
                {
                    subcategoryname = "";
                }

                var lastFlashCardsId = (from flashCount in icanSpeakContext.FlashCards
                                        where flashCount.UserId == userid
                                        orderby flashCount.FlashCardId
                                        select new
                                        {
                                            flashCount.FlashCardId
                                        }).Take(1).ToList();


                var vocabWords = (from flash in icanSpeakContext.FlashCards
                                  join vocabWord in icanSpeakContext.VocabularyWords on flash.WordId equals Convert.ToInt32(vocabWord.WordId)
                                  join vocab in icanSpeakContext.Vocabularies on vocabWord.VocabularyId equals Convert.ToInt32(vocab.VocabularyId)
                                  where flash.FlashCardId < flashcardid && flash.UserId == userid
                                  orderby flash.FlashCardId descending
                                  select new
                                  {
                                      WordId = Service.Encrypt(vocabWord.WordId.ToString()),
                                      VocabularyId = Service.Encrypt(vocab.VocabularyId.ToString()),
                                      SubCategory = subcategoryname,
                                      vocabWord.ArabicText,
                                      vocabWord.EnglishText,
                                      
                                      AudioUrl = Service.GetUrl() + "VocabularyAudios/" + vocabWord.AudioUrl,
                                      PictureUrl = Service.GetUrl() + "VocabularyImages/" + vocabWord.PictureUrl,
                                      vocab.Text,
                                      FlashCardId = Service.Encrypt(flash.FlashCardId.ToString()),
                                      thisFlashCardID = flash.FlashCardId

                                  }).Take(1).ToList();

                if (vocabWords.Count > 0)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(vocabWords);

                    DataColumn newCol = new DataColumn("isLastData", typeof(string));
                    newCol.AllowDBNull = true;
                    dt.Columns.Add(newCol);


                    if (vocabWords.ElementAtOrDefault(0).thisFlashCardID == lastFlashCardsId.ElementAtOrDefault(0).FlashCardId)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            row["isLastData"] = "1";
                        }
                    }
                    else
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            row["isLastData"] = "0";
                        }
                    }

                    dt.TableName = "WordDetail";
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
                Helper.ErrorLog(ex, "GetVocabWordBySubCategoryId");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }

        }

        public Stream BackFlashCardDetail(Stream objStream)
        {
            DataSet ds = new DataSet();
            try
            {
                VocabularyWord vocabularyWord = new VocabularyWord();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                var vocabWordData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int wordId = Convert.ToInt32(Service.Decrypt(vocabWordData["wordId"]));
                int vocabid = Convert.ToInt32(Service.Decrypt(vocabWordData["vocabid"]));
                int userid = Convert.ToInt32(Service.Decrypt(vocabWordData["userid"]));
                int flashcardid = Convert.ToInt32(Service.Decrypt(vocabWordData["flashcardid"]));

                string subcategoryname = string.Empty;
                var subcategorys = (from subcategory in icanSpeakContext.VocabularySubCategories where subcategory.VocabularyId == vocabid select new { subcategory.SubCategoryName }).FirstOrDefault();
                if (subcategorys != null)
                {
                    subcategoryname = subcategorys.SubCategoryName;
                }
                else
                {
                    subcategoryname = "";
                }

                var lastFlashCardsId = (from flashCount in icanSpeakContext.FlashCards
                                        where flashCount.UserId == userid
                                        orderby flashCount.FlashCardId descending
                                        select new
                                        {
                                            flashCount.FlashCardId
                                        }).Take(1).ToList();


                var vocabWords = (from flash in icanSpeakContext.FlashCards
                                  join vocabWord in icanSpeakContext.VocabularyWords on flash.WordId equals Convert.ToInt32(vocabWord.WordId)
                                  join vocab in icanSpeakContext.Vocabularies on vocabWord.VocabularyId equals Convert.ToInt32(vocab.VocabularyId)
                                  where flash.FlashCardId > flashcardid && flash.UserId == userid
                                  orderby flash.FlashCardId
                                  select new
                                  {
                                      WordId = Service.Encrypt(vocabWord.WordId.ToString()),
                                      VocabularyId = Service.Encrypt(vocab.VocabularyId.ToString()),
                                      SubCategory = subcategoryname,
                                      vocabWord.ArabicText,
                                      vocabWord.EnglishText,
                                      AudioUrl = Service.GetUrl() + "VocabularyAudios/" + vocabWord.AudioUrl,
                                      PictureUrl = Service.GetUrl() + "VocabularyImages/" + vocabWord.PictureUrl,
                                      vocab.Text,
                                      FlashCardId = Service.Encrypt(flash.FlashCardId.ToString()),
                                      thisFlashCardID = flash.FlashCardId
                                  }).Take(1).ToList();

                if (vocabWords.Count > 0)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(vocabWords); ;

                    DataColumn newCol = new DataColumn("isLastData", typeof(string));
                    newCol.AllowDBNull = true;
                    dt.Columns.Add(newCol);


                    if (vocabWords.ElementAtOrDefault(0).thisFlashCardID == lastFlashCardsId.ElementAtOrDefault(0).FlashCardId)
                    {

                        foreach (DataRow row in dt.Rows)
                        {
                            row["isLastData"] = "1";
                        }

                    }
                    else
                    {

                        foreach (DataRow row in dt.Rows)
                        {
                            row["isLastData"] = "0";
                        }
                    }

                    dt.TableName = "WordDetail";
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
                Helper.ErrorLog(ex, "GetVocabWordBySubCategoryId");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }

        }



    }
}