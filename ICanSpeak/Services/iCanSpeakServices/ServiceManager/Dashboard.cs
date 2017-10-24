using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace iCanSpeakServices.ServiceManager
{
    public class Dashboard
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        public Stream DashboardData(Stream objStream)
        {
            try
            {
                Dictionary<String, object> userList = new Dictionary<String, object>();
                Dictionary<String, object> coursesList = new Dictionary<String, object>();
                Dictionary<String, object> paymentList = new Dictionary<String, object>();
                Dictionary<String, object> tutorList = new Dictionary<string, object>();
                Dictionary<String, object> VocabCategoryList = new Dictionary<string, object>();
                Dictionary<String, object> VocabSubCategoryList = new Dictionary<string, object>();
                Dictionary<String, object> WordList = new Dictionary<string, object>();
                Dictionary<String, object> vocabQuestionList = new Dictionary<string, object>();
                Dictionary<String, object> GrammerUnitList = new Dictionary<string, object>();
                Dictionary<String, object> GrammerAssessmentQuestionsList = new Dictionary<string, object>();
                Dictionary<String, object> DialogList = new Dictionary<string, object>();
                Dictionary<String, object> DialogKeyPhrasesList = new Dictionary<string, object>();
                Dictionary<String, object> DialogConversationList = new Dictionary<string, object>();
                Dictionary<String, object> DialogAssessmentQueList = new Dictionary<string, object>();

                List<object> mainList = new List<object>();

                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);

                var userResult = (from users in icanSpeakContext.Users
                                  select new
                                  {
                                      //  users.Email,
                                      users.IsActive
                                      //  users.IsSuggested,
                                      //  users.AboutMe,
                                      //  users.DOB
                                  }).ToList();

                var coursesResult = (from courses in icanSpeakContext.iCanSpeakCourses
                                     select new
                                     {
                                         // courses.CourseName,
                                         // courses.CourseDescription,
                                         // courses.Duration,
                                         courses.IsFree,
                                         //  courses.Price,
                                         //   courses.MaxScore,
                                         courses.IsActive
                                     }).ToList();

                var paymentResult = (from payment in icanSpeakContext.PaymentDetails
                                     select new
                                     {
                                         // payment.PaymentDetailId,
                                         // payment.PaidBy,
                                         payment.PaidAmount
                                         //  payment.PaidDate,
                                         //  payment.CoursePrice,
                                     }).ToList();

                var tutorResult = (from tutor in icanSpeakContext.SubAdmins
                                   where tutor.RoleId == 3
                                   select new
                                   {
                                       tutor.RoleId,
                                       tutor.IsActive
                                   }).ToList();

                var VocabResult = (from vocab in icanSpeakContext.Vocabularies
                                   where vocab.DeletedDate == null
                                   select new
                                   {
                                       vocab.IsFree,
                                       vocab.IsActive
                                   }).ToList();

                var VocabSubResult = (from vocab in icanSpeakContext.VocabularySubCategories
                                      where (icanSpeakContext.Vocabularies.Where(z=>z.DeletedDate==null).Select(z=>z.VocabularyId).Contains(vocab.VocabularyId))
                                      select new
                                      {
                                          vocab.IsActive,
                                          vocab.SubCategoryName
                                      }).ToList();
                //SELECT *  FROM [dbo].[VocabularySubCategory] where vocabularyid in (SELECT Vocabularyid  FROM [valens1_LLA].[dbo].[Vocabulary] where DeletedDate IS NULL) 


                var WordResult = (from word in icanSpeakContext.VocabularyWords where (icanSpeakContext.VocabularySubCategories.Where(z=>z.DeletedDate==null).Select(z=>z.VacabularySubId).Contains(word.VocabularySubId))
                                  && (icanSpeakContext.Vocabularies.Where(z => z.DeletedDate == null).Select(z => z.VocabularyId).Contains(Convert.ToInt32(word.VocabularyId)))                                
                                  where word.DeleteDate == null
                                  select new
                                  {
                                      word.IsActive,
                                      word.EnglishText
                                  }).ToList();
                
//SELECT *  FROM [dbo].[VocabularyWords] where VocabularySubId in(SELECT VocabularySubId  FROM [dbo].[VocabularySubCategory])
//and vocabularyid in (SELECT vocabularyid  FROM [dbo].[VocabularySubCategory] where vocabularyid in
//(SELECT Vocabularyid  FROM [valens1_LLA].[dbo].[Vocabulary] where DeletedDate IS NULL))

                var VocabQueResult = (from question in icanSpeakContext.VocabQuestions
                                      where question.DeletedDate == null
                                      select new
                                      {
                                          question.IsActive,
                                          question.Question
                                      }).ToList();

                var GrammerResult = (from grammer in icanSpeakContext.GrammerUnits
                                     where grammer.DeleteDate == null
                                     select new
                                     {
                                         grammer.IsFree,
                                         grammer.IsActive
                                     }).ToList();

                var GrammerQueResult = (from question in icanSpeakContext.GrammerAssessmentQuestions
                                        where (icanSpeakContext.GrammerUnits.Where(z => z.DeleteDate == null).Select(p => p.UnitId).Contains(Convert.ToInt32(question.UnitId)))
                                        select new
                                        {
                                            question.IsActive,
                                            question.SlotId
                                        }).ToList();

                var DialogResult = (from dialog in icanSpeakContext.Dialogs
                                    where dialog.DeleteDate == null
                                    select new
                                    {
                                        dialog.IsFree,
                                        dialog.IsActive
                                    }).ToList();

                var DialogKeyPhrasesResult = (from keyphrase in icanSpeakContext.DialogKeyPhrases
                                              where (icanSpeakContext.Dialogs.Where(z => z.DeleteDate == null).Select(p => p.DialogId).Contains(Convert.ToInt32(keyphrase.DialogId)))
                                             select new
                                             {
                                                 keyphrase.IsActive,
                                                 keyphrase.EnglishText
                                             }).ToList();

                var DialogConversationResult = (from conversation in icanSpeakContext.DialogConversations
                                                where (icanSpeakContext.Dialogs.Where(z => z.DeleteDate == null).Select(p => p.DialogId).Contains(Convert.ToInt32(conversation.DialogId)))
                                  select new
                                  {
                                      conversation.IsActive,
                                      conversation.DialogId
                                  }).ToList();
               
                var DialogAssessmentQueResult = (from question in icanSpeakContext.DialogAssessmentQuestions
                                                 where (icanSpeakContext.Dialogs.Where(z=>z.DeleteDate==null).Select(p=>p.DialogId).Contains(Convert.ToInt32(question.DialogId)))
                                                                                                 
                                      select new
                                      {
                                          question.IsActive,
                                          question.Question
                                      }).ToList();

                //SELECT *  FROM [dbo].[DialogAssessmentQuestions] where DialogId in (SELECT DialogId  FROM [valens1_LLA].[dbo].[Dialogs] where DeleteDate IS NULL)

                ///////////////////////// for users /////////////////////
                var totalUserCount = userResult.Count();

                var activeuserCount = userResult.Where(i => i.IsActive == true).Count();

                var inactiveUserCount = userResult.Where(i => i.IsActive == false).Count();

                userList.Add("userCount", totalUserCount);
                userList.Add("activeUserCount", activeuserCount);
                userList.Add("inactiveUserCount", inactiveUserCount);

                //////////////////////////// for payment///////////////
                var totalPaymentCount = paymentResult.Count();

                var totalPaidAmount = paymentResult.Sum(i => i.PaidAmount);

                paymentList.Add("totalPaymentCount", totalPaymentCount);
                paymentList.Add("totalPaidAmount", totalPaidAmount);

                ///////////////////////// for courses ///////////////////////////
                var totalCourseCount = coursesResult.Count();

                var activeCourseCount = coursesResult.Where(i => i.IsActive == true).Count();

                var inactiveCourseCount = coursesResult.Where(i => i.IsActive == false).Count();

                var totalCoursePaidCount = coursesResult.Where(i => i.IsFree == false).Count();

                var totalCourseFreeCount = coursesResult.Where(i => i.IsFree == true).Count();

                coursesList.Add("totalCourseCount", totalCourseCount);
                coursesList.Add("activeCourseCount", activeCourseCount);
                coursesList.Add("inactiveCourseCount", inactiveCourseCount);
                coursesList.Add("totalCoursePaidCount", totalCoursePaidCount);
                coursesList.Add("totalCourseFreeCount", totalCourseFreeCount);

                //////////////////////tutor count////////////////////////////
                var totaltutorcount = tutorResult.Count();

                var activetutorCount = tutorResult.Where(i => i.IsActive == true).Count();

                var inactivetutorCount = tutorResult.Where(i => i.IsActive == false).Count();

                tutorList.Add("totaltutorcount", totaltutorcount);
                tutorList.Add("activetutorCount", activetutorCount);
                tutorList.Add("inactivetutorCount", inactivetutorCount);

                ///////////////////////Vocabulary Count///////////////////
                var totalVocabCount = VocabResult.Count();

                var activeVocabCount = VocabResult.Where(i => i.IsActive == true).Count();

                var inactiveVocabCount = VocabResult.Where(i => i.IsActive == false).Count();

                var totalVocabPaidCount = VocabResult.Where(i => i.IsFree == false).Count();

                var totalVocabFreeCount = VocabResult.Where(i => i.IsFree == true).Count();

                VocabCategoryList.Add("totalVocabCount", totalVocabCount);
                VocabCategoryList.Add("activeVocabCount", activeVocabCount);
                VocabCategoryList.Add("inactiveVocabCount", inactiveVocabCount);
                VocabCategoryList.Add("totalVocabPaidCount", totalVocabPaidCount);
                VocabCategoryList.Add("totalVocabFreeCount", totalVocabFreeCount);
                //                 Vocabualry subcategory

                var totalVocabSubCount = VocabSubResult.Count();

                var activeVocabSubCount = VocabSubResult.Where(i => i.IsActive == true).Count();

                var inactiveVocabSubCount = VocabSubResult.Where(i => i.IsActive == false).Count();

                VocabSubCategoryList.Add("totalVocabSubCount", totalVocabSubCount);
                VocabSubCategoryList.Add("activeVocabSubCount", activeVocabSubCount);
                VocabSubCategoryList.Add("inactiveVocabSubCount", inactiveVocabSubCount);
                // Word Count WordList   

                var totalWordCount = WordResult.Count();

                var activeWordCount = WordResult.Where(i => i.IsActive == true).Count();

                var inactiveWordCount = WordResult.Where(i => i.IsActive == false).Count();

                WordList.Add("totalWordCount", totalWordCount);
                WordList.Add("activeWordCount", activeWordCount);
                WordList.Add("inactiveWordCount", inactiveWordCount);
                //Vocab Question list

                var totalVocabQueCount = VocabQueResult.Count();

                var activeVocabQueCount = VocabQueResult.Where(i => i.IsActive == true).Count();

                var inactiveVocabQueCount = VocabQueResult.Where(i => i.IsActive == false).Count();

                vocabQuestionList.Add("totalVocabQueCount", totalVocabQueCount);
                vocabQuestionList.Add("activeVocabQueCount", activeVocabQueCount);
                vocabQuestionList.Add("inactiveVocabQueCount", inactiveVocabQueCount);

                ///////////////////////Grammer Count///////////////////
                var totalGrammerCount = GrammerResult.Count();

                var activeGrammerCount = GrammerResult.Where(i => i.IsActive == 1).Count();

                var inactiveGrammerCount = GrammerResult.Where(i => i.IsActive == 0).Count();

                var totalGrammerPaidCount = GrammerResult.Where(i => i.IsFree == false).Count();

                var totalGrammerFreeCount = GrammerResult.Where(i => i.IsFree == true).Count();

                GrammerUnitList.Add("totalGrammerCount", totalGrammerCount);
                GrammerUnitList.Add("activeGrammerCount", activeGrammerCount);
                GrammerUnitList.Add("inactiveGrammerCount", inactiveGrammerCount);
                GrammerUnitList.Add("totalGrammerPaidCount", totalGrammerPaidCount);
                GrammerUnitList.Add("totalGrammerFreeCount", totalGrammerFreeCount);

                //Vocab Question list

                var totalGrammerQueCount = GrammerQueResult.Count();

                var activeGrammerQueCount = GrammerQueResult.Where(i => i.IsActive == 1).Count();

                var inactiveGrammerQueCount = GrammerQueResult.Where(i => i.IsActive == 0).Count();

                GrammerAssessmentQuestionsList.Add("totalGrammerQueCount", totalGrammerQueCount);
                GrammerAssessmentQuestionsList.Add("activeGrammerQueCount", activeGrammerQueCount);
                GrammerAssessmentQuestionsList.Add("inactiveGrammerQueCount", inactiveGrammerQueCount);

                ///////////////////////Dialog Count///////////////////
                var totalDialogCount = DialogResult.Count();

                var activeDialogCount = DialogResult.Where(i => i.IsActive == true).Count();

                var inactiveDialogCount = DialogResult.Where(i => i.IsActive == false).Count();

                var totalDialogPaidCount = DialogResult.Where(i => i.IsFree == false).Count();

                var totalDialogFreeCount = DialogResult.Where(i => i.IsFree == true).Count();

                DialogList.Add("totalDialogCount", totalDialogCount);
                DialogList.Add("activeDialogCount", activeDialogCount);
                DialogList.Add("inactiveDialogCount", inactiveDialogCount);
                DialogList.Add("totalDialogPaidCount", totalDialogPaidCount);
                DialogList.Add("totalDialogFreeCount", totalDialogFreeCount);
                //            Dialog     Key Phrases

                var totalKeyPhrasesCount = DialogKeyPhrasesResult.Count();

                var activeKeyPhrasesCount = DialogKeyPhrasesResult.Where(i => i.IsActive == true).Count();

                var inactiveKeyPhrasesCount = DialogKeyPhrasesResult.Where(i => i.IsActive == false).Count();

                DialogKeyPhrasesList.Add("totalKeyPhrasesCount", totalKeyPhrasesCount);
                DialogKeyPhrasesList.Add("activeKeyPhrasesCount", activeKeyPhrasesCount);
                DialogKeyPhrasesList.Add("inactiveKeyPhrasesCount", inactiveKeyPhrasesCount);
                // Dialog Conversation   

                var totalConversationCount = DialogConversationResult.Count();

                var activeConversationCount = DialogConversationResult.Where(i => i.IsActive == 1).Count();

                var inactiveConversationCount = DialogConversationResult.Where(i => i.IsActive == 0).Count();

                DialogConversationList.Add("totalConversationCount", totalConversationCount);
                DialogConversationList.Add("activeConversationCount", activeConversationCount);
                DialogConversationList.Add("inactiveConversationCount", inactiveConversationCount);
                //Dialog Assessment Question list

                var totalDialogAssiQueCount = DialogAssessmentQueResult.Count();

                var activeDialogAssiQueCount = DialogAssessmentQueResult.Where(i => i.IsActive == 1).Count();

                var inactiveDialogAssiQueCount = DialogAssessmentQueResult.Where(i => i.IsActive == 0).Count();

                DialogAssessmentQueList.Add("totalDialogAssiQueCount", totalDialogAssiQueCount);
                DialogAssessmentQueList.Add("activeDialogAssiQueCount", activeDialogAssiQueCount);
                DialogAssessmentQueList.Add("inactiveDialogAssiQueCount", inactiveDialogAssiQueCount);
                mainList.Add(userList);
                mainList.Add(coursesList);
                mainList.Add(paymentList);
                mainList.Add(tutorList);
                mainList.Add(VocabCategoryList);
                mainList.Add(VocabSubCategoryList);
                mainList.Add(WordList);
                mainList.Add(vocabQuestionList);
                mainList.Add(GrammerUnitList);
                mainList.Add(GrammerAssessmentQuestionsList);
                mainList.Add(DialogList);
                mainList.Add(DialogKeyPhrasesList);
                mainList.Add(DialogConversationList);
                mainList.Add(DialogAssessmentQueList);


                var js = JsonConvert.SerializeObject(mainList, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));


            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DashboardData");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }
    }
}