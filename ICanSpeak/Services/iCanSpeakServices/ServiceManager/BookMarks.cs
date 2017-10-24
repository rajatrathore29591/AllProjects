using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace iCanSpeakServices.ServiceManager
{
    public class BookMarks
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataSet ds = new DataSet();
        DataTable Table = new DataTable();
        BookMark objbookmark = new BookMark();
        User objUser = new User();


        public Stream AddBookMark(Stream objStream)
        {
            try
            {

                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                var data = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int userId = Convert.ToInt32(Service.Decrypt(data["userid"]));

                string courseType = data["courseType"];


                var check1 = (from bookmark in icanSpeakContext.BookMarks
                              where bookmark.UserId == userId && bookmark.CourseType == courseType
                              select new
                              {

                                  bookmark.BookmarkId
                              }).ToList();


                if (check1.Count == 0)
                {

                    objbookmark.BookMarkURL = data["bookmarkUrl"];
                    objbookmark.CourseId = Convert.ToInt32(Service.Decrypt(data["courseid"]));
                    objbookmark.UserId = Convert.ToInt32(Service.Decrypt(data["userid"]));
                    objbookmark.CourseType = data["courseType"];
                    objbookmark.VocabId = Service.Decrypt(data["vocabId"]);
                    objbookmark.VocabSubId = Service.Decrypt(data["vocabSubId"]);
                    objbookmark.CreatedDate = System.DateTime.Now;
                    icanSpeakContext.BookMarks.InsertOnSubmit(objbookmark);
                    icanSpeakContext.SubmitChanges();

                    MyActivity objmyactivity = new MyActivity();
                    objmyactivity.UserId = Convert.ToInt32(Service.Decrypt(data["userid"]));
                    objmyactivity.Message = "You have book marked a Dialog " + " " + data["courseName"] + " ";
                    objmyactivity.CreatedDate = System.DateTime.Now;
                    icanSpeakContext.MyActivities.InsertOnSubmit(objmyactivity);
                    icanSpeakContext.SubmitChanges();


                }

                else
                {
                    int temp = check1[0].BookmarkId;
                    BookMark objbookmark = new BookMark();
                    objbookmark = icanSpeakContext.BookMarks.Single(key => key.BookmarkId == temp);
                    objbookmark.BookMarkURL = data["bookmarkUrl"];
                    objbookmark.CourseId = Convert.ToInt32(Service.Decrypt(data["courseid"]));
                    objbookmark.UserId = Convert.ToInt32(Service.Decrypt(data["userid"]));
                    objbookmark.CourseType = data["courseType"];
                    objbookmark.VocabId = Service.Decrypt(data["vocabId"]);
                    objbookmark.VocabSubId = Service.Decrypt(data["vocabSubId"]);
                    objbookmark.CreatedDate = System.DateTime.Now;
                    icanSpeakContext.SubmitChanges();

                }
                if (courseType == "Grammer")
                {
                    var checkGrammer = (from user in icanSpeakContext.Users
                                        where user.UserId == userId
                                        //&& user.GrammerBookmark == Service.Decrypt(data["courseid"])
                                        select new
                                        {
                                            user.UserId,
                                            user.DialogBookmark,
                                            user.VocabularyBookmark

                                        }).ToList();

                    objUser = icanSpeakContext.Users.Single(key => key.UserId == userId);
                    objUser.GrammerBookmark = Service.Decrypt(data["courseid"]);
                    //objUser.VocabId = "";
                    //objUser.VocabSubId = "";
                    if (checkGrammer[0].DialogBookmark == "")
                    {
                        objUser.DialogBookmark = "";
                    }
                    if (checkGrammer[0].VocabularyBookmark == "")
                    {
                        objUser.VocabularyBookmark = "";
                    }
                    icanSpeakContext.SubmitChanges();

                }

                if (courseType == "Dialog")
                {
                    var checkDialog = (from user in icanSpeakContext.Users
                                       where user.UserId == userId
                                       //&& user.DialogBookmark == Service.Decrypt(data["courseid"])
                                       select new
                                       {

                                           user.UserId,
                                           user.VocabularyBookmark,
                                           user.GrammerBookmark

                                       }).ToList();

                    objUser = icanSpeakContext.Users.Single(key => key.UserId == userId);
                    objUser.DialogBookmark = Service.Decrypt(data["courseid"]);
                    //objUser.VocabId = "";
                    //objUser.VocabSubId = "";
                    if (checkDialog[0].GrammerBookmark == "")
                    {
                        objUser.GrammerBookmark = "";
                    }
                    if (checkDialog[0].VocabularyBookmark == "")
                    {
                        objUser.VocabularyBookmark = "";
                    }
                    icanSpeakContext.SubmitChanges();

                }



                if (courseType == "Vocabulary")
                {
                    var checkVocab = (from user in icanSpeakContext.Users
                                      where user.UserId == userId
                                      select new
                                      {

                                          user.UserId,
                                          user.DialogBookmark,
                                          user.GrammerBookmark
                                      }).ToList();

                    objUser = icanSpeakContext.Users.Single(key => key.UserId == userId);
                    objUser.VocabularyBookmark = Service.Decrypt(data["courseid"]);
                    objUser.VocabId = Service.Decrypt(data["vocabId"]); ;
                    objUser.VocabSubId = Service.Decrypt(data["vocabSubId"]); ;
                    if (checkVocab[0].GrammerBookmark == "")
                    {
                        objUser.GrammerBookmark = "";
                    }
                    if (checkVocab[0].DialogBookmark == "")
                    {
                        objUser.DialogBookmark = "";
                    }
                    icanSpeakContext.SubmitChanges();

                }

                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                DataTable ResultMessage = Service.ResultMessage("Marked", "1");
                ds.Tables.Add(Table);
                ds.Tables.Add(ResultMessage);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));

            }

            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddBookMark");
                //  var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }


        public Stream RemoveBookMark(Stream objStream)
        {
            try
            {

                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                var data = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int userId = Convert.ToInt32(Service.Decrypt(data["userid"]));

                string courseType = data["courseType"];

                var check = (from bookmark in icanSpeakContext.BookMarks
                             where bookmark.UserId == userId && bookmark.CourseType == courseType && bookmark.CourseId == Convert.ToInt32(Service.Decrypt(data["courseid"]))
                             select bookmark).FirstOrDefault();

                icanSpeakContext.BookMarks.DeleteOnSubmit(check);
                icanSpeakContext.SubmitChanges();



                if (courseType == "Grammer")
                {
                    var checkGrammer = (from user in icanSpeakContext.Users
                                        where user.UserId == userId && user.GrammerBookmark == Service.Decrypt(data["courseid"])
                                        select new
                                        {
                                            user.UserId
                                        }).ToList();

                    objUser = icanSpeakContext.Users.Single(key => key.UserId == userId);
                    objUser.GrammerBookmark = null;
                    icanSpeakContext.SubmitChanges();

                }


                if (courseType == "Dialog")
                {

                    var checkDialog = (from user in icanSpeakContext.Users
                                       where user.UserId == userId && user.DialogBookmark == Service.Decrypt(data["courseid"])
                                       select new
                                       {

                                           user.UserId
                                       }).ToList();

                    objUser = icanSpeakContext.Users.Single(key => key.UserId == userId);
                    objUser.DialogBookmark = null;
                    icanSpeakContext.SubmitChanges();

                }


                if (courseType == "Vocabulary")
                {
                    var checkVocab = (from user in icanSpeakContext.Users
                                      where user.UserId == userId && user.VocabularyBookmark == Service.Decrypt(data["courseid"])
                                      select new
                                      {

                                          user.UserId
                                      }).ToList();

                    objUser = icanSpeakContext.Users.Single(key => key.UserId == userId);
                    objUser.VocabularyBookmark = null;
                    icanSpeakContext.SubmitChanges();

                }

                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                DataTable ResultMessage = Service.ResultMessage("Marked", "1");
                ds.Tables.Add(Table);
                ds.Tables.Add(ResultMessage);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));

            }

            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddBookMark");
                //  var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }


        public Stream GetBookmarkById(Stream objStream)
        {
            //Dialog objDialog = new Dialog();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
            int userId = Convert.ToInt32(Service.Decrypt(userRequest["userid"]));


            try
            {
                var results = (from grammer in icanSpeakContext.GrammerUnits
                               join bookmark in icanSpeakContext.BookMarks on Convert.ToInt32(grammer.UnitId) equals bookmark.CourseId

                               where bookmark.UserId == userId && bookmark.CourseType == "Grammer"
                               select new
                               {

                                   grammer.UnitId,
                                   grammer.UnitNameEnglish,
                                   grammer.UnitNameArabic,
                                   grammer.VideoUrl,
                                   grammer.DescriptionArabic,
                                   grammer.DescriptionEnglish,
                                   bookmark.CourseType,
                                   bookmark.CourseId,
                                   bookmark.BookmarkId

                               }).ToList();



                var results1 = (from vocabsubcategory in icanSpeakContext.VocabularySubCategories
                                join vocabword in icanSpeakContext.VocabularyWords on vocabsubcategory.VacabularySubId equals vocabword.VocabularySubId
                                join bookmark in icanSpeakContext.BookMarks on Convert.ToInt32(vocabword.WordId) equals bookmark.CourseId
                                join vocabcategory in icanSpeakContext.Vocabularies on vocabword.VocabularyId equals Convert.ToInt32(vocabcategory.VocabularyId)

                                where bookmark.UserId == userId && bookmark.CourseType == "Vocabulary"
                                select new
                                {

                                    vocabword.ArabicText,
                                    vocabword.AudioUrl,
                                    vocabword.EnglishText,
                                    vocabword.WordId,
                                    vocabsubcategory.SubCategoryName,
                                    vocabsubcategory.VacabularySubId,
                                    vocabcategory.Text,
                                    vocabcategory.VocabularyId,
                                    bookmark.CourseType,
                                    bookmark.CourseId

                                }).ToList();



                var results2 = (from dialog in icanSpeakContext.Dialogs
                                join bookmark in icanSpeakContext.BookMarks on Convert.ToInt32(dialog.DialogId) equals bookmark.CourseId
                                where bookmark.UserId == userId && bookmark.CourseType == "Dialog"
                                select new
                                {

                                    dialog.DialogId,
                                    dialog.ArabicName,
                                    dialog.EnglishName,
                                    dialog.DescriptionArabic,
                                    dialog.DescriptionEnglish,
                                    dialog.VideoUrl,
                                    dialog.DialogKeyPhrases,
                                    dialog.DialogConversations,
                                    bookmark.CourseType,
                                    bookmark.CourseId

                                }).ToList();



                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                DataTable BookMarkGrammer = Service.ConvertToDataTable(results);
                BookMarkGrammer.TableName = "BookmarkGrammerDetail";
                DataTable BookMarkVocab = Service.ConvertToDataTable(results1);
                BookMarkVocab.TableName = "BookmarkVocabDetail";
                DataTable BookMarkDialog = Service.ConvertToDataTable(results2);
                BookMarkDialog.TableName = "BookmarkDialogDetail";
                ds.Tables.Add(Table);
                ds.Tables.Add(BookMarkGrammer);
                ds.Tables.Add(BookMarkVocab);
                ds.Tables.Add(BookMarkDialog);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));

            }

            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetBookmarkById");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }

    }
}