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
    public class VocabWord
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataTable Table = new DataTable();
        DataTable ResultMessage = new DataTable();
        DataSet ds = new DataSet();


        public Stream AddVocabWord(Stream objStream)
        {
            try
            {
                
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var vocabworddata = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                string wordnames = vocabworddata["wordname"];
                string[] wordnamesarray = new string[0];
                wordnamesarray = Regex.Split(wordnames, @"\|\|\|");
                string arabicwordnames = vocabworddata["arabicwordnames"];
                string[] arabicwordnamesarray = new string[0];
                arabicwordnamesarray = Regex.Split(arabicwordnames, @"\|\|\|");
                int wordcount = wordnamesarray.Count() - 1; 
                string returnid = string.Empty;
                for(int i=0;i<=wordcount;i++)
                {
                    
                    VocabularyWord vocabularyWord = new VocabularyWord();
                    vocabularyWord.VocabularyId = Convert.ToInt32(vocabworddata["vocabid"]);
                    vocabularyWord.VocabularySubId = Convert.ToInt32(vocabworddata["vacabularySubId"]);
                    vocabularyWord.EnglishText = wordnamesarray[i].ToString();
                    vocabularyWord.ArabicText = arabicwordnamesarray[i].ToString();
                    vocabularyWord.CreateDate = System.DateTime.Now;
                    vocabularyWord.IsActive = true;
                    icanSpeakContext.VocabularyWords.InsertOnSubmit(vocabularyWord);
                    icanSpeakContext.SubmitChanges();

                    var vocabwordid = icanSpeakContext.VocabularyWords.ToList().Max(U => U.WordId);
                    vocabularyWord.PictureUrl = vocabwordid + "_wordimage.jpg";
                    vocabularyWord.AudioUrl = vocabwordid + "_wordaudio.mp3";
                    icanSpeakContext.SubmitChanges();
                    returnid = vocabwordid.ToString();
                    
                }

                if (vocabworddata["vacabularySubId"] != "0")
                {
                    var vocabSubCategory = (from voc in icanSpeakContext.VocabularySubCategories
                                            where voc.VacabularySubId == Convert.ToInt32(vocabworddata["vacabularySubId"])
                                            select voc).FirstOrDefault();
                    vocabSubCategory.WordCount = vocabSubCategory.WordCount + wordnamesarray.Count();
                    icanSpeakContext.SubmitChanges();
                }
                var vocabDatas = (from voc in icanSpeakContext.Vocabularies
                                  where voc.VocabularyId == Convert.ToInt32(vocabworddata["vocabid"])
                                  select voc).FirstOrDefault();
                vocabDatas.WordCount = vocabDatas.WordCount + wordnamesarray.Count();
                icanSpeakContext.SubmitChanges();
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(returnid)));

            }
            catch (Exception ex)
            {
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetVocabWordByWordId(Stream objStream)
        {
            try
            {
                VocabularyWord vocabularyWord = new VocabularyWord();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                var vocabWordData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var vocabWords = (from vocabWord in icanSpeakContext.VocabularyWords
                                  where vocabWord.WordId == Convert.ToInt32(vocabWordData["wordId"])
                                  select new { vocabularyWord.VocabularyId, vocabWord.VocabularySubId, vocabWord.WordId, vocabWord.ArabicText, vocabWord.EnglishText, vocabWord.IsActive, AudioUrl = Service.GetUrl() + "VocabularyAudios/" + vocabWord.AudioUrl, PictureUrl = Service.GetUrl() + "VocabularyImages/" + vocabWord.PictureUrl, vocabWord.CreateDate }).ToList();

                if (vocabWords.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(vocabWords, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
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

        public Stream GetAllVocabWord(Stream objStream)
        {
            try
            {
                VocabularyWord vocabularyWord = new VocabularyWord();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                var vocabWordData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int subcategoryid = Convert.ToInt32(vocabWordData["vocabularySubId"]);
                if(subcategoryid==0)
                {
                    var vocabWords = (from vocabWord in icanSpeakContext.VocabularyWords
                                      where vocabWord.VocabularyId == Convert.ToInt32(vocabWordData["vocabid"])
                                      select new { vocabWord.VocabularySubId, vocabWord.WordId, vocabWord.ArabicText, vocabWord.EnglishText, vocabWord.IsActive, 
                                                  AudioUrl = Service.GetUrl() + "VocabularyAudios/" + vocabWord.AudioUrl,
                                                  PictureUrl = Service.GetUrl() + "VocabularyImages/" + vocabWord.PictureUrl,
                                                  vocabWord.CreateDate }).ToList();
                    if (vocabWords.Count > 0)
                    {
                        var js = JsonConvert.SerializeObject(vocabWords, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                        return new MemoryStream(Encoding.UTF8.GetBytes(js));
                    }
                    else
                    {
                        return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No data")));
                    }
                }
                else
                {
                var vocabWords = (from vocabWord in icanSpeakContext.VocabularyWords
                                  where vocabWord.VocabularySubId == Convert.ToInt32(vocabWordData["vocabularySubId"])
                                  select new { vocabWord.VocabularySubId, vocabWord.WordId, vocabWord.ArabicText, vocabWord.EnglishText, vocabWord.IsActive, AudioUrl = Service.GetUrl() + "VocabularyAudios/" + vocabWord.AudioUrl, PictureUrl = Service.GetUrl() + "VocabularyImages/" + vocabWord.PictureUrl, vocabWord.CreateDate }).ToList();
                if (vocabWords.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(vocabWords, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No data")));
                }
                }
                

                
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAllVocabCategory");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }

        }

        public Stream UpdateVocabWordById(Stream objStream)
        {
            try
            {
                VocabularyWord vocabularyWord = new VocabularyWord();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var vocabData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var vocabWordId = (from voc in icanSpeakContext.VocabularyWords
                                        where voc.WordId == Convert.ToInt32(vocabData["vocabWordId"])
                                         select voc).FirstOrDefault();

                if (vocabWordId.WordId > 0)
                {

                    vocabWordId.EnglishText = vocabData["englishText"];
                    vocabWordId.ArabicText = vocabData["arabicText"];
                    vocabWordId.ModifyDate = System.DateTime.Now;
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
                Helper.ErrorLog(ex, "UpdateVocabWordById");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));

            }

        }

        public Stream DeleteVocabWordById(Stream objStream)
        {
            try
            {
                VocabularyWord vocabularyWord = new VocabularyWord();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var vocabData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var vocabWord = (from voc in icanSpeakContext.VocabularyWords
                                 where voc.WordId == Convert.ToInt32(vocabData["vocabWordId"])
                                 select voc).FirstOrDefault();

             
                if (vocabData["softDelete"] == "true")
                {
                    if (vocabWord.IsActive == true)
                    {
                        vocabWord.IsActive = false;
                    }
                    else
                    {
                        vocabWord.IsActive = true;
                    }
                    icanSpeakContext.SubmitChanges();
                }

                else
                {
                    icanSpeakContext.VocabularyWords.DeleteOnSubmit(vocabWord);
                    icanSpeakContext.SubmitChanges();

                    var vocabdata = (from voc in icanSpeakContext.Vocabularies
                                     where voc.VocabularyId == Convert.ToInt32(vocabData["vocabid"])
                                     select voc).FirstOrDefault();
                    vocabdata.WordCount = vocabdata.WordCount - 1;
                    icanSpeakContext.SubmitChanges();

                    if(vocabData["subcategoryid"]!="0")
                    {
                    var vocabSubCategories = (from vocsub in icanSpeakContext.VocabularySubCategories
                                              where vocsub.VacabularySubId == Convert.ToInt32(vocabData["subcategoryid"])
                                              select vocsub).FirstOrDefault();
                    vocabSubCategories.WordCount = vocabSubCategories.WordCount - 1;
                    icanSpeakContext.SubmitChanges();
                    }
                }
                

                var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteVocabWordById");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));

            }
        }

        public Stream GetVocabSubCategoryWordsById(Stream objStream) // VocabSubCategoryWordsList
        {
            try
            {
                VocabularyWord vocabularyWord = new VocabularyWord();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                int vocabindex = 1;
                var vocabWordData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int subcategoryId = Convert.ToInt32(Service.Decrypt(vocabWordData["subcategoryId"]));

                var vocabWords = (from vocabWord in icanSpeakContext.VocabularyWords
                                  join category in icanSpeakContext.Vocabularies on vocabWord.VocabularyId equals Convert.ToInt32(category.VocabularyId)
                                  join subcategory in icanSpeakContext.VocabularySubCategories on vocabWord.VocabularySubId equals Convert.ToInt32(subcategory.VacabularySubId)
                                  where vocabWord.VocabularySubId == subcategoryId
                                  select new
                                  {
                                              VocabularyId = Service.Encrypt(vocabWord.VocabularyId.ToString()),
                                              WordId = Service.Encrypt(vocabWord.WordId.ToString()),
                                              vocabWord.ArabicText,
                                              vocabWord.EnglishText,
                                              vocabWord.CreateDate,
                                              category.Text,
                                              subcategory.SubCategoryName
                                  }).OrderByDescending(x => x.CreateDate).ToList();

                if (vocabWords.Count()>0)
                {

                    var vocabWordss = vocabWords.AsEnumerable().Select(x => new
                    {
                        RowIndex = vocabindex++,
                        VocabularyId = x.VocabularyId,
                        WordId = x.WordId,
                        ArabicText = x.ArabicText,
                        EnglishText = x.EnglishText,
                        Date = x.CreateDate,
                        CategoryName=x.Text,
                        SubCategoryName=x.SubCategoryName
                    }).ToList();
                    
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(vocabWordss); ;
                    dt.TableName = "Words";
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

        public Stream GetVocabCategoryWordsById(Stream objStream)   //Vocab Catory Word List
        {
            try
            {
                VocabularyWord vocabularyWord = new VocabularyWord();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                int vocabindex = 1;
                var vocabWordData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int categoryId = Convert.ToInt32(Service.Decrypt(vocabWordData["categoryId"]));

                var vocabWords = (from vocabWord in icanSpeakContext.VocabularyWords
                                  where vocabWord.VocabularyId == categoryId
                                  select new
                                  {
                                      VocabularyId = Service.Encrypt(vocabWord.VocabularyId.ToString()),
                                      WordId = Service.Encrypt(vocabWord.WordId.ToString()),
                                      vocabWord.ArabicText,
                                      vocabWord.EnglishText,
                                      vocabWord.CreateDate
                                  }).OrderByDescending(x => x.CreateDate).ToList();

                if (vocabWords.Count() > 0)
                {

                    var vocabWordss = vocabWords.AsEnumerable().Select(x => new
                    {
                        RowIndex = vocabindex++,
                        VocabularyId = x.VocabularyId,
                        WordId = x.WordId,
                        ArabicText = x.ArabicText,
                        EnglishText = x.EnglishText,
                        Date = x.CreateDate,
                    }).ToList();

                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(vocabWordss); ;
                    dt.TableName = "Words";
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

        public Stream GetVocabWordsByCategoryId(Stream objStream)
        {
            try
            {
                VocabularyWord vocabularyWord = new VocabularyWord();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                var vocabWordData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int vocabId = Convert.ToInt32(Service.Decrypt(vocabWordData["vocabId"]));
                var vocabWords = (from vocabWord in icanSpeakContext.VocabularyWords
                                  where vocabWord.VocabularyId == vocabId
                                  select new
                                  {
                                      WordId = Service.Encrypt(vocabWord.WordId.ToString()),
                                      vocabWord.ArabicText,
                                      vocabWord.EnglishText
                                  }).ToList();

                if (vocabWords != null)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(vocabWords); ;
                    dt.TableName = "Words";
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

        public Stream GetWordsDetailByWordId(Stream objStream)   //WorDetail
        {
            try
            {
                VocabularyWord vocabularyWord = new VocabularyWord();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                var vocabWordData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int wordId =  Convert.ToInt32(Service.Decrypt(vocabWordData["wordId"]));
                int vocabid = Convert.ToInt32(Service.Decrypt(vocabWordData["vocabid"]));
                int userid = Convert.ToInt32(Service.Decrypt(vocabWordData["userid"]));


                string subcategoryname = string.Empty;
                var subcategorys = (from subcategory in icanSpeakContext.VocabularySubCategories where subcategory.VocabularyId == vocabid select new { subcategory.SubCategoryName}).FirstOrDefault();
                if (subcategorys != null)
                {
                    subcategoryname = subcategorys.SubCategoryName; 
                }
                else
                {
                    subcategoryname = ""; 
                }
                
                var vocabWords = (from vocabWord in icanSpeakContext.VocabularyWords
                                  join vocab in icanSpeakContext.Vocabularies on vocabWord.VocabularyId equals Convert.ToInt32(vocab.VocabularyId)
                                  where vocabWord.WordId == wordId
                                  select new
                                  {
                                      WordId = Service.Encrypt(vocabWord.WordId.ToString()),
                                      VocabularyId = Service.Encrypt(vocab.VocabularyId.ToString()),
                                      VocabularySubId = Service.Encrypt(vocabWord.VocabularySubId.ToString()),
                                      SubCategory = subcategoryname ,
                                      vocabWord.ArabicText,
                                      vocabWord.EnglishText,
                                      AudioUrl = Service.GetUrl() + "VocabularyAudios/" + vocabWord.AudioUrl,
                                      PictureUrl = Service.GetUrl() + "VocabularyImages/" + vocabWord.PictureUrl,
                                      vocab.Text,
                                      DescriptionEnglish = vocab.SampleSentance,
                                      DescriptionArabic = vocab.ArabicText,
                                      FlashCardStatus = FlashCardStatus(Convert.ToInt32(vocabWord.WordId), userid),
                                      BookMarkStatus = BookMarkStatus(Convert.ToInt32(vocabWord.WordId), userid)
                                  }).ToList();

                if (vocabWords != null)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(vocabWords); ;
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

        public string BookMarkStatus(int wordId, int userid)
        {
            string result = string.Empty;
            var status = (from Book in icanSpeakContext.BookMarks
                          where Book.CourseId == wordId && Book.UserId == userid
                          select new { Book.BookmarkId}
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

        public Stream NextWordDetailById(Stream objStream)  //Next Word Detail
        {
            try
            {
                VocabularyWord vocabularyWord = new VocabularyWord();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                var vocabWordData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int wordID = Convert.ToInt32(Service.Decrypt(vocabWordData["wordId"]));
                int vocabid = Convert.ToInt32(Service.Decrypt(vocabWordData["vocabid"]));
                int userid = Convert.ToInt32(Service.Decrypt(vocabWordData["userid"]));

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

                var lastVocabWordId = (from vocabWord in icanSpeakContext.VocabularyWords
                                       where vocabWord.VocabularyId == vocabid
                                       orderby vocabWord.WordId
                                       select new { vocabWord.WordId }).Take(1).ToList();

                var vocabWordId = (from vocabWord in icanSpeakContext.VocabularyWords
                                   join vocab in icanSpeakContext.Vocabularies on vocabWord.VocabularyId equals Convert.ToInt32(vocab.VocabularyId)
                                   where vocabWord.WordId < wordID && vocabWord.VocabularyId == vocabid
                                   orderby vocabWord.WordId descending

                                   select new
                                   {
                                       WordId = Service.Encrypt(vocabWord.WordId.ToString()),
                                       
                                   }).Take(1).ToList();

                int wordId = Convert.ToInt32(Service.Decrypt(vocabWordId[0].WordId));

                var vocabWords = (from vocabWord in icanSpeakContext.VocabularyWords
                                  join vocab in icanSpeakContext.Vocabularies on vocabWord.VocabularyId equals Convert.ToInt32(vocab.VocabularyId)
                                  where vocabWord.WordId < wordID && vocabWord.VocabularyId == vocabid
                                  orderby vocabWord.WordId descending
                                  
                                  select new
                                  {
                                      WordId = Service.Encrypt(vocabWord.WordId.ToString()),
                                      vocabWord.ArabicText,
                                      vocabWord.EnglishText,
                                      VocabularyId = Service.Encrypt(vocabWord.VocabularyId.ToString()),
                                      SubCategory = subcategoryname,
                                      AudioUrl = Service.GetUrl() + "VocabularyAudios/" + vocabWord.AudioUrl,
                                      PictureUrl = Service.GetUrl() + "VocabularyImages/" + vocabWord.PictureUrl,
                                      vocab.Text,
                                      FlashCardStatus = FlashCardStatus(Convert.ToInt32(vocabWord.WordId), userid),
                                      BookMarkStatus = BookMarkStatus(wordId, userid),
                                      thisWordId = vocabWord.WordId
                                  }).Take(1).ToList();


                if (vocabWords.Count > 0)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(vocabWords);

                    DataColumn newCol = new DataColumn("isLastData", typeof(string));
                    newCol.AllowDBNull = true;
                    dt.Columns.Add(newCol);

                    if (vocabWords.ElementAtOrDefault(0).thisWordId == lastVocabWordId.ElementAtOrDefault(0).WordId)
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

        public Stream BackWordDetailById(Stream objStream)
        {
            try
            {
                VocabularyWord vocabularyWord = new VocabularyWord();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                var vocabWordData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int wordID = Convert.ToInt32(Service.Decrypt(vocabWordData["wordId"]));
                int vocabid = Convert.ToInt32(Service.Decrypt(vocabWordData["vocabid"]));
                int userid = Convert.ToInt32(Service.Decrypt(vocabWordData["userid"]));

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

                var firstVocabWordId = (from vocabWord in icanSpeakContext.VocabularyWords
                                       where vocabWord.VocabularyId == vocabid
                                        orderby vocabWord.WordId descending
                                       select new { vocabWord.WordId }).Take(1).ToList();
                var vocabWordId = (from vocabWord in icanSpeakContext.VocabularyWords
                                  join vocab in icanSpeakContext.Vocabularies on vocabWord.VocabularyId equals Convert.ToInt32(vocab.VocabularyId)
                                  where vocabWord.WordId > wordID && vocabWord.VocabularyId == vocabid
                                  orderby vocabWord.WordId
                                  select new
                                  {
                                      WordId = Service.Encrypt(vocabWord.WordId.ToString()),
                                      
                                  }).Take(1).ToList();
                int wordId = Convert.ToInt32(Service.Decrypt(vocabWordId[0].WordId));

                var vocabWords = (from vocabWord in icanSpeakContext.VocabularyWords
                                  join vocab in icanSpeakContext.Vocabularies on vocabWord.VocabularyId equals Convert.ToInt32(vocab.VocabularyId)
                                  where vocabWord.WordId > wordID && vocabWord.VocabularyId == vocabid
                                  orderby vocabWord.WordId 
                                  select new
                                  {
                                      WordId = Service.Encrypt(vocabWord.WordId.ToString()),
                                      vocabWord.ArabicText,
                                      vocabWord.EnglishText,
                                      VocabularyId = Service.Encrypt(vocabWord.VocabularyId.ToString()),
                                      SubCategory = subcategoryname,
                                      AudioUrl = Service.GetUrl() + "VocabularyAudios/" + vocabWord.AudioUrl,
                                      PictureUrl = Service.GetUrl() + "VocabularyImages/" + vocabWord.PictureUrl,
                                      vocab.Text,
                                      FlashCardStatus = FlashCardStatus(Convert.ToInt32(vocabWord.WordId), userid),
                                      BookMarkStatus = BookMarkStatus(wordId, userid),
                                      //BookMarkStatus = BookMarkStatus(Convert.ToInt32(vocabWord.WordId), userid),
                                      thisWordId = vocabWord.WordId
                                  }).Take(1).ToList();

                if (vocabWords.Count > 0)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(vocabWords);

                    DataColumn newCol = new DataColumn("isLastData", typeof(string));
                    newCol.AllowDBNull = true;
                    dt.Columns.Add(newCol);

                    if (vocabWords.ElementAtOrDefault(0).thisWordId == firstVocabWordId.ElementAtOrDefault(0).WordId)
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