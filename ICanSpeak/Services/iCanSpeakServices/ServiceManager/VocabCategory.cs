using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Data;
using System.ServiceModel.Web;

namespace iCanSpeakServices.ServiceManager
{
    public class VocabCategory
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataTable Table = new DataTable();
        DataSet ds = new DataSet("Result");

        public Stream AddVocabCategory(Stream objStream)
        {
            try
            {
                Vocabulary objVocab = new Vocabulary();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                // requestString = "{\"text\":\"dfdf\",\"imageUrl\":\"img08.png\",\"audioUrl\":\"img05.jpg\",\"sampleSentance\":\"\",\"arabicText\":\"dfdf\"}";
                var vocabData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var emailVocab = icanSpeakContext.Vocabularies.Any(vocab => vocab.Text == vocabData["text"]);
                if (emailVocab == true)
                {

                    var js = JsonConvert.SerializeObject("Please choose a different vocab name ! This one is already in use", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    string subcategory = vocabData["subcategory"];
                    string[] subcategoryarray = new string[0];

                    objVocab.Text = vocabData["text"];
                    objVocab.SampleSentance = vocabData["sampleSentance"];
                    objVocab.ArabicText = vocabData["arabicText"];
                    objVocab.IsActive = true;
                    objVocab.CreatedDate = System.DateTime.Now;
                    objVocab.Price = Convert.ToInt32(vocabData["Price"]);
                    objVocab.Duration = Convert.ToInt32(vocabData["Duration"]);
                    objVocab.RewardPoints = Convert.ToInt32(vocabData["RewardPoints"]);
                    objVocab.MaxScore = Convert.ToInt32(vocabData["MaxScore"]);
                    objVocab.IsFree = Convert.ToBoolean(vocabData["IsFree"]);
                    if (subcategory != "")
                    {
                        subcategoryarray = Regex.Split(subcategory, @"\|\|\|");
                        objVocab.IsSubCategory = true;
                        objVocab.SubCategoryCount = subcategoryarray.Count();
                        objVocab.WordCount = 0;
                    }
                    else
                    {
                        objVocab.IsSubCategory = false;
                        objVocab.SubCategoryCount = 0;
                        objVocab.WordCount = 0;
                    }

                    icanSpeakContext.Vocabularies.InsertOnSubmit(objVocab);

                    icanSpeakContext.SubmitChanges();

                    // var data = icanSpeakContext.Vocabularies.ToList<Vocabulary>().LastOrDefault();

                    var vocabularyId = icanSpeakContext.Vocabularies.ToList().Max(U => U.VocabularyId);

                    //objVocab.ImageUrl = vocabularyId + "_vocabimage.jpeg";
                    //objVocab.AudioUrl = vocabularyId + "_vocabaudio.mp3";
                    //icanSpeakContext.SubmitChanges();


                    if (subcategory != "")
                    {
                        foreach (string subcategoryvalue in subcategoryarray)
                        {

                            VocabularySubCategory objvocabsubcategory = new VocabularySubCategory();
                            objvocabsubcategory.VocabularyId = vocabularyId;
                            objvocabsubcategory.SubCategoryName = subcategoryvalue;
                            objvocabsubcategory.IsActive = true;
                            objvocabsubcategory.CreatedDate = System.DateTime.Now;
                            objvocabsubcategory.WordCount = 0;
                            icanSpeakContext.VocabularySubCategories.InsertOnSubmit(objvocabsubcategory);
                            icanSpeakContext.SubmitChanges();
                        }
                    }

                    icanSpeakContext.ExecuteCommand("update Users set batchcount=batchcount+1");

                    Notification objNotification = new Notification();
                    objNotification.UserId = 102;
                    objNotification.Message = "Added a new course";
                    objNotification.CreatedDate = System.DateTime.Now;
                    icanSpeakContext.Notifications.InsertOnSubmit(objNotification);
                    icanSpeakContext.SubmitChanges();

                    var js = JsonConvert.SerializeObject(vocabularyId, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddVocabCategory");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetAllVocabCategory(Stream objStream)
        {
            try
            {
                Vocabulary objVocab = new Vocabulary();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);

                var vocabCategories = (from vocab in icanSpeakContext.Vocabularies
                                       where vocab.DeletedDate == null
                                       select new
                                       {
                                           vocab.VocabularyId,
                                           vocab.Text,
                                           vocab.IsActive,
                                           vocab.CreatedDate,
                                           issubcategory = vocab.IsSubCategory,
                                           vocab.SubCategoryCount,
                                           vocab.WordCount
                                       }).ToList();

                if (vocabCategories.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(vocabCategories, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No data")));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAllVocabCategory");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }

        }

        public Stream GetCategoryByVocabId(Stream objStream)
        {
            try
            {
                Vocabulary objVocab = new Vocabulary();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);

                String requestString = reader.ReadToEnd();

                // requestString = "{\"text\":\"dfdf\",\"imageUrl\":\"img08.png\",\"audioUrl\":\"img05.jpg\",\"sampleSentance\":\"\",\"arabicText\":\"dfdf\"}";
                var vocabData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var vocabCategories = (from vocab in icanSpeakContext.Vocabularies
                                       where vocab.VocabularyId == Convert.ToInt32(vocabData["vocabularyId"])
                                       select new { vocab.VocabularyId, vocab.ArabicText, vocab.Text, vocab.AudioUrl, vocab.ImageUrl, vocab.SampleSentance, vocab.IsActive, vocab.IsFree, vocab.CreatedDate, vocab.Price, vocab.Duration, vocab.RewardPoints, vocab.MaxScore, vocab.IsSubCategory }).ToList();

                if (vocabCategories.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(vocabCategories, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
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

        public Stream UpdateVocabCategory(Stream objStream)
        {
            try
            {
                Vocabulary objVocab = new Vocabulary();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var vocabData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var vocabId = (from voc in icanSpeakContext.Vocabularies
                               where voc.VocabularyId == Convert.ToInt32(vocabData["vocabularyId"])
                               select voc).FirstOrDefault();

                if (vocabId.VocabularyId > 0)
                {
                    //string subcategory = vocabData["subcategory"];
                    //string[] subcategoryarray = new string[0];
                    vocabId.Text = vocabData["text"];
                    vocabId.SampleSentance = vocabData["sampleSentance"];
                    vocabId.ArabicText = vocabData["arabicText"];
                    vocabId.IsActive = true;
                    vocabId.CreatedDate = System.DateTime.Now;
                    vocabId.Price = Convert.ToInt32(vocabData["Price"]);
                    vocabId.Duration = Convert.ToInt32(vocabData["Duration"]);
                    vocabId.RewardPoints = Convert.ToInt32(vocabData["RewardPoints"]);
                    vocabId.MaxScore = Convert.ToInt32(vocabData["MaxScore"]);
                    vocabId.IsFree = Convert.ToBoolean(vocabData["IsFree"]);
                    vocabId.ModifiedDate = System.DateTime.Now;
                    //if (subcategory != "")
                    //{
                    //    subcategoryarray = Regex.Split(subcategory, @"\|\|\|");
                    //    vocabId.IsSubCategory = true;
                    //    vocabId.SubCategoryCount = vocabId.SubCategoryCount + subcategoryarray.Count();
                    //}
                    //else
                    //{
                    //    vocabId.IsSubCategory = false;
                    //    vocabId.SubCategoryCount = 0;
                    //}
                    icanSpeakContext.SubmitChanges();

                    //if (subcategory != "")
                    //{
                    //    foreach (string subcategoryvalue in subcategoryarray)
                    //    {

                    //        VocabularySubCategory objvocabsubcategory = new VocabularySubCategory();
                    //        objvocabsubcategory.VocabularyId = vocabId.VocabularyId;
                    //        objvocabsubcategory.SubCategoryName = subcategoryvalue;
                    //        objvocabsubcategory.IsActive = true;
                    //        icanSpeakContext.VocabularySubCategories.InsertOnSubmit(objvocabsubcategory);
                    //        icanSpeakContext.SubmitChanges();
                    //    }
                    //}


                    var js = JsonConvert.SerializeObject("Vocab updated successfully", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
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
                Helper.ErrorLog(ex, "UpdateVocabCategory");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));

            }

        }

        public Stream DeleteVocabByCategoryId(Stream objStream)
        {

            try
            {
                Vocabulary objVocab = new Vocabulary();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var vocabData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var vocabCategory = (from voc in icanSpeakContext.Vocabularies
                                     where voc.VocabularyId == Convert.ToInt32(vocabData["vocabularyId"])
                                     select voc).FirstOrDefault();

                var vocabSubCategory = (from vocSub in icanSpeakContext.VocabularySubCategories
                                        where vocSub.VocabularyId == vocabCategory.VocabularyId
                                        select vocSub).ToList();

                var vocabWord = (from vocWord in icanSpeakContext.VocabularyWords
                                 where vocWord.VocabularySubId == vocabSubCategory.Select(i => i.VacabularySubId).FirstOrDefault()
                                 select vocWord).ToList();

                if (vocabData["softDelete"] == "true")
                {
                    if (vocabCategory.IsActive == true)
                    {

                        vocabCategory.IsActive = false;
                    }
                    else
                    {
                        vocabCategory.IsActive = true;
                    }
                }
                else
                {
                    vocabCategory.DeletedDate = System.DateTime.Now;
                }

                icanSpeakContext.SubmitChanges();

                var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }

            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteVocabByCategoryId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));

            }
        }

        //===================================Vocab Assessment=========================================================================================================//
        public Stream GetAllVocabQuestion(Stream objStream)
        {
            try
            {
                VocabQuestion vocabQuestion = new VocabQuestion();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);

                var result = (from vocabquestion in icanSpeakContext.VocabQuestions
                              select new { vocabquestion.QuestionId, vocabquestion.VocabularyId, vocabquestion.SelectSet, vocabquestion.Question, vocabquestion.CorrectAnswer, ImageUrl = Service.GetUrl() + "VocabQuestionImage/" + vocabquestion.ImageUrl, vocabquestion.CreatedDate, vocabquestion.IsActive }).ToList();

                if (result.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {

                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No Data")));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAllVocabQuestion");

                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }

        }

        public Stream GetVocabByUserId(Stream objStream)    //Voacabulay List
        {

            VocabQuestion vocabQuestion = new VocabQuestion();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();

            DataTable MyscoreDataStatus = new DataTable();
            DataTable TotalFlashDataStatus = new DataTable();
            DataTable Myscores = new DataTable();
            DataTable AssessmentStatus = new DataTable();
            DataTable VocabAssessmentScore = new DataTable();
            DataTable Vocab = new DataTable();
            DataTable VocabStatus =new DataTable();
           
            int vocabindex = 1;
            int scoreindex = 1;

            try
            {
                var userLogin = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int userid = Convert.ToInt32(Service.Decrypt(userLogin["userid"]));

                var vocabdata = (from vocab in icanSpeakContext.Vocabularies
                                 where vocab.IsFree == true && vocab.DeletedDate == null 
                                 select new
                                 {
                                     VocabularyId = Service.Encrypt(vocab.VocabularyId.ToString()),
                                     VocabEnglish = vocab.Text,
                                     VocabArabic = vocab.ArabicText,
                                     vocab.CreatedDate,
                                     IsSubCategory = vocab.IsSubCategory,
                                     SubCategoryCount=vocab.SubCategoryCount,
                                     WordCount=vocab.WordCount
                                 }).OrderByDescending(x => x.CreatedDate);

                var vocabdatas = vocabdata.AsEnumerable().Select(x => new
                {
                    RowIndex = vocabindex++,
                    VocabularyId = x.VocabularyId,
                    VocabEnglish = x.VocabEnglish,
                    VocabArabic = x.VocabArabic,
                    Date = x.CreatedDate,
                    IsSubCategory = x.IsSubCategory,
                    SubCategoryCount=x.SubCategoryCount,
                    WordCount=x.WordCount

                }).ToList();

                if (vocabdatas.Count > 0)
                {
                    VocabStatus = Service.Message("success", "1");
                    VocabStatus.AcceptChanges();
                    Vocab = Service.ConvertToDataTable(vocabdatas);
                    Vocab.TableName = "Vocab";
                   
                }
                else
                {
                    //Table = Service.Message("success", "1");
                    //Table.AcceptChanges();
                    VocabStatus = Service.Message("No Data", "0");
                    VocabStatus.TableName = "Dialog";
                }
                //Table = Service.Message("success", "1");
                //Table.AcceptChanges();
                //DataTable Vocab = Service.ConvertToDataTable(vocabdatas);
                //Vocab.TableName = "Vocab";


                var vocabAssessmentScore = (from score in icanSpeakContext.VocabAssessmentScores
                                             where score.UserId == userid && score.Type == "Vocab" 
                                             select new
                                             {
                                                 score.ScoreId,
                                                 VocabularyId = Service.Encrypt(score.VocabularyId.ToString()),
                                                 score.UserId,
                                                 score.Score,
                                                 score.TotalScore,
                                                 score.ModifiedDate,
                                                 score.CreateDate
                                             }).OrderByDescending(y => y.CreateDate);

                var vocabAssessmentScores = vocabAssessmentScore.AsEnumerable().Select(y => new
                {
                    RowIndex = scoreindex++,
                    ScoreId = y.ScoreId,
                    VocabularyId = y.VocabularyId,
                    UserId = y.UserId,
                    Score = y.Score,
                    TotalScore = y.TotalScore,
                    ModifiedDate = y.ModifiedDate,
                    CreatedDate = y.CreateDate,
                }).ToList();

                if (vocabAssessmentScores.Count > 0)
                {
                    AssessmentStatus = Service.ResultMessage("success", "1");
                    AssessmentStatus.AcceptChanges();
                    VocabAssessmentScore = Service.ConvertToDataTable(vocabAssessmentScores);
                    VocabAssessmentScore.TableName = "VocabAssessmentScore";
                }
                else
                {
                    //Table = Service.Message("success", "1");
                    //Table.AcceptChanges();
                    AssessmentStatus = Service.ResultMessage("No Data", "0");
                    AssessmentStatus.AcceptChanges();
                    //AssessmentStatus.TableName = "VocabAssessmentScore";
                }


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
                ds.Tables.Add(Vocab);
                ds.Tables.Add(VocabAssessmentScore);
                ds.Tables.Add(MyscoreDataStatus);
                ds.Tables.Add(Myscores);
                ds.Tables.Add(AssessmentStatus);
                ds.Tables.Add(VocabStatus);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetVocabByUserId");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }

        }

        public Stream GetVocabByUserIdDevice(Stream objStream)
        {
            try
            {
                VocabQuestion vocabQuestion = new VocabQuestion();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                var userLogin = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int userid = Convert.ToInt32(Service.Decrypt(userLogin["userid"]));
                DataTable AssessmentStatus = new DataTable();
                DataTable VocabAssessmentScore = new DataTable();
                DataTable VocabStatus = new DataTable();
                DataTable Vocab = new DataTable();
                int vocabindex = 1;
                int scoreindex = 1;

                var vocabdata = (from vocab in icanSpeakContext.Vocabularies
                                 where vocab.IsFree == true && vocab.DeletedDate == null
                                 select new
                                 {
                                     VocabularyId = Service.Encrypt(vocab.VocabularyId.ToString()),
                                     VocabEnglish = vocab.Text,
                                     VocabArabic = vocab.ArabicText,
                                     vocab.CreatedDate,
                                     IsSubCategory = vocab.IsSubCategory,
                                     SubCategory = (from subcategory in icanSpeakContext.VocabularySubCategories where subcategory.VocabularyId == vocab.VocabularyId select new { subcategory.SubCategoryName, VacabularySubId = Service.Encrypt(subcategory.VacabularySubId.ToString()) }).ToList()
                                 }).OrderByDescending(x => x.CreatedDate);

                var vocabdatas = vocabdata.AsEnumerable().Select(x => new
                {
                    RowIndex = vocabindex++,
                    VocabularyId = x.VocabularyId,
                    VocabEnglish = x.VocabEnglish,
                    VocabArabic = x.VocabArabic,
                    Date = x.CreatedDate,
                    IsSubCategory = x.IsSubCategory,
                    SubCategory = x.SubCategory
                }).ToList();

                if (vocabdatas.Count > 0)
                {
                    VocabStatus = Service.Message("success", "1");
                    VocabStatus.AcceptChanges();
                    Vocab = Service.ConvertToDataTable(vocabdatas);
                    Vocab.TableName = "Vocab";

                }
                else
                {
                    //Table = Service.Message("success", "1");
                    //Table.AcceptChanges();
                    VocabStatus = Service.Message("No Data", "0");
                    VocabStatus.TableName = "Dialog";
                }
                //Table = Service.Message("success", "1");
                //Table.AcceptChanges();
                //DataTable Vocab = Service.ConvertToDataTable(vocabdatas);
                //Vocab.TableName = "Vocab";


                var vocabAssessmentScore = (from score in icanSpeakContext.VocabAssessmentScores
                                            where score.UserId == userid && score.Type == "Vocab"
                                            select new
                                            {
                                                score.ScoreId,
                                                VocabularyId = Service.Encrypt(score.VocabularyId.ToString()),
                                                score.UserId,
                                                score.Score,
                                                score.TotalScore,
                                                score.ModifiedDate,
                                                score.CreateDate
                                            }).OrderByDescending(y => y.CreateDate);

                var vocabAssessmentScores = vocabAssessmentScore.AsEnumerable().Select(y => new
                {
                    RowIndex = scoreindex++,
                    ScoreId = y.ScoreId,
                    VocabularyId = y.VocabularyId,
                    UserId = y.UserId,
                    Score = y.Score,
                    TotalScore = y.TotalScore,
                    ModifiedDate = y.ModifiedDate,
                    CreatedDate = y.CreateDate,
                }).ToList();

                if (vocabAssessmentScores.Count > 0)
                {
                    AssessmentStatus = Service.ResultMessage("success", "1");
                    AssessmentStatus.AcceptChanges();
                    VocabAssessmentScore = Service.ConvertToDataTable(vocabAssessmentScores);
                    VocabAssessmentScore.TableName = "VocabAssessmentScore";
                }
                else
                {
                    //Table = Service.Message("success", "1");
                    //Table.AcceptChanges();
                    AssessmentStatus = Service.ResultMessage("No Data", "0");
                    AssessmentStatus.TableName = "VocabAssessmentScore";
                }



                ds.Tables.Add(Table);
                ds.Tables.Add(Vocab);
                //ds.Tables.Add(Vocab);
                ds.Tables.Add(VocabAssessmentScore);
                //ds.Tables.Add(MyscoreDataStatus);
                //ds.Tables.Add(Myscores);
                ds.Tables.Add(AssessmentStatus);
                ds.Tables.Add(VocabStatus);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetVocabByUserId");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }

        }




        public Stream AddVocabQuestionByVocabId(Stream objStream)
        {
            try
            {
                VocabQuestion vocabQuestion = new VocabQuestion();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                // requestString = "{\"vocabularyId\":\"5\",\"question\":\"Click the best definition of Persnickety\",\"optionsA\":\"Extensive\",\"optionsB\":\"Romantic\",\"optionsC\":\"Choosy\",\"optionsD\":\"Awesome\",\"correctAnswer\":\"Choosy\"}";
                var vocabData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var questionVocab = icanSpeakContext.VocabQuestions.Any(vocab => vocab.Question == vocabData["question"]);
                if (questionVocab == true)
                {

                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Please choose a different vocab question ! This one is already in use")));
                }
                else
                {
                    vocabQuestion.VocabularyId = Convert.ToInt32(vocabData["vocabularyId"]);
                    vocabQuestion.SelectSet = Convert.ToInt32(vocabData["SelectSet"]);
                    vocabQuestion.Question = vocabData["question"];
                   
                    vocabQuestion.IsActive = true;
                    vocabQuestion.OptionsA = vocabData["optionsA"];
                    vocabQuestion.OptionsB = vocabData["optionsB"];
                    vocabQuestion.OptionsC = vocabData["optionsC"];
                    vocabQuestion.OptionsD = vocabData["optionsD"];
                    vocabQuestion.CorrectAnswer = vocabData["correctAnswer"];
                    vocabQuestion.CreatedDate = System.DateTime.Now;
                    icanSpeakContext.VocabQuestions.InsertOnSubmit(vocabQuestion);
                    icanSpeakContext.SubmitChanges();

                    var questionId = icanSpeakContext.VocabQuestions.ToList<VocabQuestion>().LastOrDefault();
                    vocabQuestion.ImageUrl = questionId.QuestionId.ToString() + "_vocabquestionimage.jpg";
                    vocabQuestion.OptionAAudio = questionId.QuestionId.ToString() + "_vocaboptionA.mp3";
                    vocabQuestion.OptionBAudio = questionId.QuestionId.ToString() + "_vocaboptionB.mp3";
                    vocabQuestion.OptionCAudio = questionId.QuestionId.ToString() + "_vocaboptionC.mp3";
                    vocabQuestion.OptionDAudio = questionId.QuestionId.ToString() + "_vocaboptionD.mp3";
                    icanSpeakContext.SubmitChanges();
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(questionId.QuestionId.ToString())));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddQuestionByCategoryId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetVocabQuestionByQuestionId(Stream objStream)
        {
            try
            {
                Vocabulary objVocab = new Vocabulary();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);

                String requestString = reader.ReadToEnd();

                // requestString = "{\"text\":\"dfdf\",\"imageUrl\":\"img08.png\",\"audioUrl\":\"img05.jpg\",\"sampleSentance\":\"\",\"arabicText\":\"dfdf\"}";
                var questionid = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var questiondetail = (from question in icanSpeakContext.VocabQuestions
                                      where question.QuestionId == Convert.ToInt32(questionid["QuestionId"])
                                      select new
                                      {
                                          question.QuestionId,
                                          question.SelectSet,
                                          question.Question,
                                          question.CorrectAnswer,
                                          question.OptionsA,
                                          question.OptionsB,
                                          question.OptionsC,
                                          question.OptionsD,
                                          ImageUrl = Service.GetUrl() + "VocabQuestionImage/" + question.ImageUrl,
                                          Option1AudioUrl = Service.GetUrl() + "VocabQuestionAudio/" + question.OptionAAudio,
                                          Option2AudioUrl = Service.GetUrl() + "VocabQuestionAudio/" + question.OptionBAudio,
                                          Option3AudioUrl = Service.GetUrl() + "VocabQuestionAudio/" + question.OptionCAudio,
                                          Option4AudioUrl = Service.GetUrl() + "VocabQuestionAudio/" + question.OptionDAudio,
                                      }).ToList();

                if (questiondetail.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(questiondetail, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No Data")));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetVocabQuestionByQuestionId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }
        
        //Sanjay
         public Stream GetVocabQuestionBySet(Stream objStream)
         {
             StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
             String requestString = reader.ReadToEnd();
             var selectset = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
             try
             {
                 var vocabquestions = (from vocabquestion in icanSpeakContext.VocabQuestions
                                       where vocabquestion.SelectSet == Convert.ToInt32(selectset["SelectSet"])
                                       select new
                                       {
                                           vocabquestion.QuestionId,
                                           vocabquestion.Question,
                                           vocabquestion.CorrectAnswer,
                                           vocabquestion.OptionsA,
                                           vocabquestion.OptionsB,
                                           vocabquestion.OptionsC,
                                           vocabquestion.OptionsD,
                                           ImageUrl = Service.GetUrl() + "VocabQuestionImage/" + vocabquestion.ImageUrl,
                                           //Option1AudioUrl = Service.GetUrl() + "VocabQuestionAudio/" + vocabquestion.OptionAAudio,
                                           //Option2AudioUrl = Service.GetUrl() + "VocabQuestionAudio/" + vocabquestion.OptionBAudio,
                                           //Option3AudioUrl = Service.GetUrl() + "VocabQuestionAudio/" + vocabquestion.OptionCAudio,
                                           //Option4AudioUrl = Service.GetUrl() + "VocabQuestionAudio/" + vocabquestion.OptionDAudio,
                                           vocabquestion.CreatedDate
                                       }).OrderBy(x => x.CreatedDate).ToList();

                 if (vocabquestions.Count > 0)
                 {
                     Table = Service.Message("Success", "1");
                     Table.AcceptChanges();
                     DataTable Dialog = Service.ConvertToDataTable(vocabquestions);
                     Dialog.TableName = "VocabQuestion";
                     ds.Tables.Add(Table);
                     ds.Tables.Add(Dialog);
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
                 Helper.ErrorLog(ex, "GetVocabQuestionBySet");
                 Table = Service.Message(ex.Message, ex.HResult.ToString());
                 ds.Tables.Add(Table);
                 return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
             }
         }

        public Stream UpdateVocabQuestion(Stream objStream)
        {
            try
            {
                Vocabulary objVocab = new Vocabulary();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var questiondata = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var question = (from questions in icanSpeakContext.VocabQuestions
                                where questions.QuestionId == Convert.ToInt32(questiondata["QuestionId"])
                                select questions).FirstOrDefault();

                if (question.QuestionId > 0)
                {
                    question.Question = questiondata["Question"];
                    question.SelectSet = Convert.ToInt32(questiondata["SelectSet"]);
                    question.CorrectAnswer = questiondata["CorrectAnswer"];
                    question.OptionsA = questiondata["OptionsA"];
                    question.OptionsB = questiondata["OptionsB"];
                    question.OptionsC = questiondata["OptionsC"];
                    question.OptionsD = questiondata["OptionsD"];
                    icanSpeakContext.SubmitChanges();
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("VocabQuestion updated successfully")));
                }
                else
                {

                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Record not available for updation")));
                }


            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "UpdateVocabQuestion");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));

            }

        }

        public Stream DeleteVocabQuestionByQuestionId(Stream objStream)
        {

            try
            {

                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var question = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var questiondata = (from questions in icanSpeakContext.VocabQuestions
                                    where questions.QuestionId == Convert.ToInt32(question["QuestionId"])
                                    select questions).FirstOrDefault();



                if (question["softDelete"] == "true")
                {
                    if (questiondata.IsActive == true)
                    {
                        questiondata.IsActive = false;
                    }
                    else
                    {
                        questiondata.IsActive = true;
                    }
                }
                else
                {
                    icanSpeakContext.VocabQuestions.DeleteOnSubmit(questiondata);

                }

                icanSpeakContext.SubmitChanges();

                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
            }

            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteVocabByCategoryId");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));

            }
        }

        //============================================================================================================================================================//
        /// </summary>
        /// <param name="objStream"></param>
        /// <returns></returns>



        public Stream GetCorrectAnswerByQuestionId(Stream objStream)
        {
            try
            {
                VocabQuestion vocabQuestion = new VocabQuestion();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                // requestString = "{\"questionId\":\"3\"}";

                var vocabData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var correctAnswer = (from data in icanSpeakContext.VocabQuestions
                                     where data.QuestionId == Convert.ToInt32(vocabData["questionId"])
                                     select data.CorrectAnswer).FirstOrDefault();

                if (correctAnswer.Count() > 0)
                {
                    var js = JsonConvert.SerializeObject(correctAnswer, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    var js = JsonConvert.SerializeObject("Answer not available", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }


            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetCorrectAnswerByQuestionId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetAssementByVocabId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
            try
            {

                var results = (from question in icanSpeakContext.VocabQuestions
                               where question.VocabularyId == Convert.ToInt32(Service.Decrypt(userRequest["vocabId"]))
                               select new
                               {
                                   QuestionId = Service.Encrypt(question.QuestionId.ToString()),
                                   VocabularyId = Service.Encrypt(question.VocabularyId.ToString()),
                                   question.Question,
                                   question.CorrectAnswer,
                                   Picture = Service.GetUrl() + "VocabQuestionImage/" + question.ImageUrl,
                                   question.OptionsA,
                                   question.OptionsB,
                                   question.OptionsC,
                                   question.OptionsD,
                                   OptionAAudio = Service.GetUrl() + "VocabQuestionAudio/" + question.OptionAAudio,
                                   OptionBAudio = Service.GetUrl() + "VocabQuestionAudio/" + question.OptionBAudio,
                                   OptionCAudio = Service.GetUrl() + "VocabQuestionAudio/" + question.OptionCAudio,
                                   OptionDAudio = Service.GetUrl() + "VocabQuestionAudio/" + question.OptionDAudio,
                                   
                                   question.CreatedDate
                               }).OrderBy(x => x.CreatedDate).Take(1).ToList();

                if (results.Count > 0)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable Dialog = Service.ConvertToDataTable(results);
                    Dialog.TableName = "VocabQuestion";
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
                Helper.ErrorLog(ex, "GetAssementByVocabId");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }

        public Stream GetNextAssementByVocabId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
            int questionid = Convert.ToInt32(Service.Decrypt(userRequest["questionId"]));
            int userid = Convert.ToInt32(Service.Decrypt(userRequest["userId"]));
            int courseid = Convert.ToInt32(Service.Decrypt(userRequest["courseId"]));
            string coursetype = userRequest["coursetype"];
            //int subCategoryId = Convert.ToInt32(Service.Decrypt(userRequest["SubCategoryId"]));
            try
            {
                //=================================Assessement Score Table ENtry Starts==================================================================\\

                var check = (from score in icanSpeakContext.AssessmentScores
                             where score.UserId == userid && score.CourseType == "Vocab" && score.CourseId == courseid
                             && score.QuestionId == questionid
                             select new
                             {
                                 score.AssessmentScoreId

                             }).ToList();
                if (check.Count == 0)
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
                //AssessmentScore objscore = new AssessmentScore();
                //objscore.UserId = userid;
                //objscore.CourseId = Convert.ToInt32(Service.Decrypt(userRequest["courseId"]));
                //objscore.QuestionId = questionid;
                //objscore.CourseType = userRequest["coursetype"];
                ////objscore.SubCategoryId = subCategoryId;
                //if (userRequest["status"] == "true")
                //{
                //    objscore.IsQuestionCorrect = true;
                //}
                //else
                //{
                //    objscore.IsQuestionCorrect = false;
                //}
                //objscore.CreatedDate = System.DateTime.Now;
                //icanSpeakContext.AssessmentScores.InsertOnSubmit(objscore);
                //icanSpeakContext.SubmitChanges();
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
                UserData.TotalScore = UserData.TotalScore + 5;
                icanSpeakContext.SubmitChanges();

                //==============If Answer Correct, Increment User My Score Ends=======================================================================\\

                var results = (from question in icanSpeakContext.VocabQuestions
                               where question.QuestionId > questionid &&
                               question.VocabularyId == Convert.ToInt32(Service.Decrypt(userRequest["vocabId"]))
                               orderby question.QuestionId ascending
                               select new
                               {
                                   QuestionId = Service.Encrypt(question.QuestionId.ToString()),
                                   VocabularyId = Service.Encrypt(question.VocabularyId.ToString()),
                                   question.Question,
                                   question.CorrectAnswer,
                                   Picture = Service.GetUrl() + "VocabQuestionImage/" + question.ImageUrl,
                                   question.OptionsA,
                                   question.OptionsB,
                                   question.OptionsC,
                                   question.OptionsD,
                                   OptionAAudio = Service.GetUrl() + "VocabQuestionAudio/" + question.OptionAAudio,
                                   OptionBAudio = Service.GetUrl() + "VocabQuestionAudio/" + question.OptionBAudio,
                                   OptionCAudio = Service.GetUrl() + "VocabQuestionAudio/" + question.OptionCAudio,
                                   OptionDAudio = Service.GetUrl() + "VocabQuestionAudio/" + question.OptionDAudio,
                                   question.CreatedDate
                               }).OrderBy(x => x.CreatedDate).Take(1).ToList();

                if (results.Count > 0)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable Dialog = Service.ConvertToDataTable(results);
                    Dialog.TableName = "VocabQuestion";
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
                                   where score.UserId == userid && score.CourseType == userRequest["coursetype"]
                                   select score).Count();
                    if (result1 != null)
                    {
                        total = result1 * 5;

                        var result2 = (from score in icanSpeakContext.AssessmentScores
                                       where score.UserId == userid && score.CourseType == userRequest["coursetype"]
                                       && score.IsQuestionCorrect == true
                                       select score).Count();
                        scored = result2 * 5;

                        var checkScore = (from vocabAssesmentScore in icanSpeakContext.VocabAssessmentScores
                                          where vocabAssesmentScore.UserId == userid && vocabAssesmentScore.VocabularyId == courseid
                                          select vocabAssesmentScore).FirstOrDefault();
                        if (checkScore == null)
                        {
                            VocabAssessmentScore objVocabScore = new VocabAssessmentScore();
                            objVocabScore.Score = scored;
                            objVocabScore.TotalScore = total;
                            objVocabScore.VocabularyId = courseid;
                            objVocabScore.UserId = userid;
                            objVocabScore.Type = "Vocab";
                            objVocabScore.CreateDate = System.DateTime.Now;
                            icanSpeakContext.VocabAssessmentScores.InsertOnSubmit(objVocabScore);
                            icanSpeakContext.SubmitChanges();
                            //return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
                        }
                        else
                        {
                            VocabAssessmentScore objVocabScore = new VocabAssessmentScore();
                            objVocabScore = icanSpeakContext.VocabAssessmentScores.Single(key => key.ScoreId == checkScore.ScoreId);
                            objVocabScore.Score = scored;
                            objVocabScore.TotalScore = total;
                            objVocabScore.VocabularyId = courseid;
                            objVocabScore.UserId = userid;
                            objVocabScore.Type = "Vocab";
                            objVocabScore.ModifiedDate = System.DateTime.Now;
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
                Helper.ErrorLog(ex, "GetAssementByVocabId");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }
    }
}