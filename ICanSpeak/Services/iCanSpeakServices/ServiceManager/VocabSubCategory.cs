using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Data;
using System.ServiceModel.Web;

namespace iCanSpeakServices.ServiceManager
{
    public class VocabSubCategory
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataTable Table = new DataTable();
        DataTable ResultMessage = new DataTable();
        DataSet ds = new DataSet();



        public Stream AddVocabSubCategory(Stream objStream)
        {
            try
            {
                VocabularySubCategory objVocabSubCategory = new VocabularySubCategory();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var vocabData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                string subcategory = vocabData["subcategory"];
                string[] subcategoryarray = new string[0];

                if (subcategory != "")
                {
                    Vocabulary objVocab = new Vocabulary();

                    var vocabdata = (from voc in icanSpeakContext.Vocabularies
                                   where voc.VocabularyId == Convert.ToInt32(vocabData["vocabularyId"])
                                   select voc).FirstOrDefault();

                    

                    subcategoryarray = Regex.Split(subcategory, @"\|\|\|");
                    vocabdata.SubCategoryCount = vocabdata.SubCategoryCount + subcategoryarray.Count();
                    icanSpeakContext.SubmitChanges();

                    foreach (string subcategoryvalue in subcategoryarray)
                    {

                        VocabularySubCategory objvocabsubcategory = new VocabularySubCategory();
                        objvocabsubcategory.VocabularyId = Convert.ToInt32(vocabData["vocabularyId"]);
                        objvocabsubcategory.SubCategoryName = subcategoryvalue;
                        objvocabsubcategory.IsActive = true;
                        objvocabsubcategory.CreatedDate = System.DateTime.Now;
                        objvocabsubcategory.WordCount = 0;
                        icanSpeakContext.VocabularySubCategories.InsertOnSubmit(objvocabsubcategory);
                        icanSpeakContext.SubmitChanges();
                    }
                    var js1 = JsonConvert.SerializeObject("success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
                else
                {
                    var js1 = JsonConvert.SerializeObject("error", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
      
            }
            catch (Exception ex)
            {
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetSubCategoryByVacabSubId(Stream objStream)
        {
            try
            {
                VocabularySubCategory objVocabSubCategory = new VocabularySubCategory();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                var vocabSubData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var vocabSubCategories = (from vocabSub in icanSpeakContext.VocabularySubCategories
                                          where vocabSub.VacabularySubId == Convert.ToInt32(vocabSubData["vacabularySubId"])
                                          select new { vocabSub.VocabularyId, vocabSub.VacabularySubId, vocabSub.SubCategoryName, vocabSub.ArabicText, vocabSub.AudioUrl, vocabSub.ImageUrl, vocabSub.CreatedDate, vocabSub.IsActive}).ToList();

                if (vocabSubCategories.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(vocabSubCategories, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
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

        public Stream GetAllVocabSubCategory(Stream objStream)
        {
            try
            {
                VocabularySubCategory objVocabSubCategory = new VocabularySubCategory();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                var vocabSubData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                

                var vocabSubCategories = (from vocabSub in icanSpeakContext.VocabularySubCategories
                                          where vocabSub.VocabularyId == Convert.ToInt32(vocabSubData["vocabularyId"])
                                          select new { vocabSub.VocabularyId, vocabSub.VacabularySubId, 
                                                      vocabSub.SubCategoryName, vocabSub.CreatedDate, 
                                                      vocabSub.IsActive, vocabSub.WordCount}).ToList();

                if (vocabSubCategories.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(vocabSubCategories, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
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


        public Stream GetSubCategoryByVocabId(Stream objStream)
        {
            try
            {
                VocabularySubCategory objVocabSubCategory = new VocabularySubCategory();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                var vocabSubData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int vocabid = Convert.ToInt32(Service.Decrypt(vocabSubData["vocabularyId"]));

                var vocabSubCategories = (from vocabSub in icanSpeakContext.VocabularySubCategories
                                          where vocabSub.VocabularyId == vocabid
                                          select new
                                          {
                                              vocabSub.VocabularyId,
                                              vocabSub.VacabularySubId,
                                              vocabSub.SubCategoryName,
                                              vocabSub.CreatedDate,
                                              vocabSub.IsActive,
                                              vocabSub.WordCount
                                          }).ToList();

                if (vocabSubCategories != null)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(vocabSubCategories); ;
                    dt.TableName = "SubCategory";
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
                Helper.ErrorLog(ex, "GetSubCategoryByVocabId");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }

        }


        public Stream UpdateVocabSubCategoryById(Stream objStream)
        {
            try
            {
                VocabularySubCategory objVocabSubCategory = new VocabularySubCategory();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                //  requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var vocabData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                var vocaSubCategory = (from voc in icanSpeakContext.VocabularySubCategories
                                       where voc.VacabularySubId == Convert.ToInt32(vocabData["VacabularySubId"])
                                       select voc).FirstOrDefault();
                if (vocaSubCategory.VacabularySubId > 0)
                {
                    vocaSubCategory.SubCategoryName = vocabData["subcategory"];
                    vocaSubCategory.VacabularySubId = Convert.ToInt32(vocabData["VacabularySubId"]);
                    //vocaSubCategory.ImageUrl = vocabData["imageUrl"];
                    //vocaSubCategory.AudioUrl = vocabData["audioUrl"];
                    //vocaSubCategory.ArabicText = vocabData["arabicText"];
                   // var vacabularySubId = icanSpeakContext.VocabularySubCategories.ToList().Max(U => U.VacabularySubId); sjy
                    //objVocabSubCategory.ImageUrl = vacabularySubId + "_subimage_" + vocabData["imageUrl"];
                   // objVocabSubCategory.AudioUrl = vacabularySubId + "_subaudio_" + vocabData["audioUrl"];
                    vocaSubCategory.ModifiedDate = System.DateTime.Now;
                    icanSpeakContext.SubmitChanges();
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


        public Stream DeleteVocabSubCategoryById(Stream objStream)
        {
            try
            {
                VocabularySubCategory objVocabSub = new VocabularySubCategory();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var vocabData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var vocabSubCategory = (from voc in icanSpeakContext.VocabularySubCategories
                                        where voc.VacabularySubId == Convert.ToInt32(vocabData["vocabSubCategoryId"])
                                        select voc).FirstOrDefault();

                var vocabWord = (from voc in icanSpeakContext.VocabularyWords
                                 where voc.VocabularySubId == vocabSubCategory.VacabularySubId
                                 select voc).ToList();


                if (vocabData["softDelete"] == "true")
                {
                    if (vocabSubCategory.IsActive == true)
                    {
                        vocabSubCategory.IsActive = false;
                    }
                    else
                    {
                        vocabSubCategory.IsActive = true;
                    }
                    icanSpeakContext.SubmitChanges();
                }

                else
                {
                    icanSpeakContext.VocabularyWords.DeleteAllOnSubmit(vocabWord);
                    icanSpeakContext.VocabularySubCategories.DeleteOnSubmit(vocabSubCategory);
                    icanSpeakContext.SubmitChanges();
                   var vocabDatas = (from voc in icanSpeakContext.Vocabularies
                                     where voc.VocabularyId == Convert.ToInt32(vocabData["vocabid"])
                                            select voc).FirstOrDefault();
                   vocabDatas.SubCategoryCount = vocabDatas.SubCategoryCount - 1;
                   icanSpeakContext.SubmitChanges();
                    
                }
                

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


        public Stream GetVocabSubCategoryByVocabId(Stream objStream)    //SubCatagory List
        {
            try
            {
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                DataTable AssessmentStatus = new DataTable();
                int vocabindex = 1;
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var vocabsubcategory = ((from subcategory in icanSpeakContext.VocabularySubCategories 
                                         join category in icanSpeakContext.Vocabularies on subcategory.VocabularyId equals category.VocabularyId
                                         where subcategory.VocabularyId == Convert.ToInt32(Service.Decrypt(userRequest["vocabid"]))
                                         select new
                                         {
                                             VacabularySubId = Service.Encrypt(subcategory.VacabularySubId.ToString()),
                                             SubCategoryName = subcategory.SubCategoryName,
                                             SubCategoryArabic = subcategory.ArabicText,
                                             subcategory.CreatedDate,
                                             CategoryName = category.Text,
                                             WordCount=category.WordCount
                                         }).OrderByDescending(x => x.CreatedDate).ToList());

                if (vocabsubcategory.Count > 0)
                {
                    var vocabsubcategorys = vocabsubcategory.AsEnumerable().Select(x => new
                    {
                        RowIndex = vocabindex++,
                        VacabularySubId = x.VacabularySubId,
                        SubCategoryName = x.SubCategoryName,
                        SubCategoryArabic = x.SubCategoryArabic,
                        Date = x.CreatedDate,
                        CategoryName=x.CategoryName,
                        WordCount=x.WordCount
                    }).ToList();

                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable VocabSubCategory = Service.ConvertToDataTable(vocabsubcategorys);
                    VocabSubCategory.TableName = "VocabSubCategory";



                    //var vocabAssessmentScore = (from score in icanSpeakContext.VocabAssessmentScores
                    //                            where score.VocabularyId == Convert.ToInt32(Service.Decrypt(userRequest["vocabid"])) && score.Type == "Vocab"
                    //                            select new
                    //                            {
                    //                                score.ScoreId,
                    //                                VocabularyId = Service.Encrypt(score.VocabularyId.ToString()),
                    //                                score.UserId,
                    //                                score.Score,
                    //                                score.TotalScore,
                    //                                score.ModifiedDate,
                    //                                score.CreateDate
                    //                            }).OrderByDescending(y => y.CreateDate);

                    //var vocabAssessmentScores = vocabAssessmentScore.AsEnumerable().Select(y => new
                    //{
                    //    RowIndex = scoreindex++,
                    //    ScoreId = y.ScoreId,
                    //    DialogId = y.VocabularyId,
                    //    UserId = y.UserId,
                    //    Score = y.Score,
                    //    TotalScore = y.TotalScore,
                    //    ModifiedDate = y.ModifiedDate,
                    //    CreatedDate = y.CreateDate,
                    //}).ToList();

                    //if (vocabAssessmentScores.Count > 0)
                    //{
                    //    AssessmentStatus = Service.Message("success", "1");
                    //    AssessmentStatus.AcceptChanges();
                    //    VocabAssessmentScore = Service.ConvertToDataTable(vocabAssessmentScores);
                    //    VocabAssessmentScore.TableName = "VocabAssessmentScore";
                    //}
                    //else
                    //{
                    //    //Table = Service.Message("success", "1");
                    //    //Table.AcceptChanges();
                    //    AssessmentStatus = Service.Message("No Data", "0");
                    //    AssessmentStatus.TableName = "VocabAssessmentScore";
                    //}



                    ds.Tables.Add(Table);
                    ds.Tables.Add(VocabSubCategory);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
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
                Helper.ErrorLog(ex, "GetVocabSubCategoryByVocabId");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }

        }
        public Stream GetVocabQuestionBySetId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            var selectset = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
            try
            {

                var results = (from question in icanSpeakContext.VocabQuestions
                               where question.VocabularyId == Convert.ToInt32(Service.Decrypt(selectset["vocabId"]))
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

    }
}