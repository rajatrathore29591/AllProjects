using iCanSpeakServices.HelperClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using iCanSpeakServices.ServiceManager;
using System.ServiceModel.Activation;
using System.Data;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Cryptography;

namespace iCanSpeakServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service : IService
    {

        UserProfile userProfile = new UserProfile();
        /// <summary>
        /// //////////////////////////////////////////////////////////////Rahul///////////////////////////////////////////
        /// </summary>
        /// <param name="objStream"></param>
        /// <returns></returns>


        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }


        public static string GetUrl()
        {
            string url = "http://lla.techvalens.net/";
            return url;
        }



        public static DataTable ConvertToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public static DataTable Message(string msg, string errorcode)
        {
            DataTable Table = new DataTable();
            Table.TableName = "Message";
            Table.Columns.Add("msg");
            Table.Columns.Add("code");
            DataRow drRownew = Table.NewRow();
            drRownew["msg"] = msg;
            drRownew["code"] = errorcode;
            Table.Rows.Add(drRownew);
            Table.AcceptChanges();
            return Table;
        }

        public static DataTable ResultMessage(string msg, string errorcode)
        {
            DataTable Table = new DataTable();
            Table.TableName = "ResultMessage";
            Table.Columns.Add("resultmsg");
            Table.Columns.Add("code");
            DataRow drRownew = Table.NewRow();
            drRownew["resultmsg"] = msg;
            drRownew["code"] = errorcode;
            Table.Rows.Add(drRownew);
            Table.AcceptChanges();
            return Table;
        }

        public static DataTable DataStatus(string msg, string errorcode)
        {
            DataTable Table = new DataTable();
            Table.Columns.Add("resultmsg");
            Table.Columns.Add("code");
            DataRow drRownew = Table.NewRow();
            drRownew["resultmsg"] = msg;
            drRownew["code"] = errorcode;
            Table.Rows.Add(drRownew);
            Table.AcceptChanges();
            return Table;
        }

        public static string TrimString(string yourString, int maxLength)
        {
            return (yourString.Length <= maxLength) ? yourString : yourString.Substring(0, maxLength);
        }

        public Stream UserRegistration(Stream objStream)
        {
            UserAccount userAccount = new UserAccount();
            return userAccount.UserRegistration(objStream);
        }

        public Stream EncryptId(Stream objStream)
        {
            Development development = new Development();
            return development.EncryptId(objStream);
        }

        public Stream AddFlashCardWord(Stream objStream)
        {
            FlashCardWord objflash = new FlashCardWord();
            return objflash.AddFlashCardWord(objStream);
        }

        public Stream RemoveFlashCardWord(Stream objStream)
        {
            FlashCardWord objflash = new FlashCardWord();
            return objflash.RemoveFlashCardWord(objStream);
        }

        public Stream FlashCardByUserId(Stream objStream)
        {
            FlashCardWord objflash = new FlashCardWord();
            return objflash.FlashCardByUserId(objStream);
        }

        public Stream NextFlashCardDetail(Stream objStream)
        {
            FlashCardWord objflash = new FlashCardWord();
            return objflash.NextFlashCardDetail(objStream);
        }

        public Stream BackFlashCardDetail(Stream objStream)
        {
            FlashCardWord objflash = new FlashCardWord();
            return objflash.BackFlashCardDetail(objStream);
        }


        public Stream SaveUserThirdParty(Stream objStream)
        {
            UserAccount userAccount = new UserAccount();
            return userAccount.SaveUserThirdParty(objStream);
        }

        public Stream Login(Stream objStream)
        {
            UserAccount userAccount = new UserAccount();

            return userAccount.Login(objStream);
        }

        public Stream GetUserCourseById(Stream objStream)
        {
            UserCourse userCourse = new UserCourse();

            return userCourse.GetUserCourseById(objStream);
        }

        public Stream ForgotPassword(Stream objStream)
        {
            UserAccount userAccount = new UserAccount();
            return userAccount.ForgotPassword(objStream);
        }

        public Stream GetUserProfileEdit(Stream objStream)
        {
            return userProfile.GetUserProfileEdit(objStream);
        }

        public Stream GetUserProfile(Stream objStream)
        {
            return userProfile.GetUserProfile(objStream);
        }

        public Stream EditUserProfile(Stream objStream)
        {
            return userProfile.EditUserProfile(objStream);
        }

        public Stream GetMessageByUserId(Stream objStream)
        {
            Messages message = new Messages();
            return message.GetMessageByUserId(objStream);
        }

        public Stream SentMessageByUserId(Stream objStream)
        {
            Messages message = new Messages();
            return message.SentMessageByUserId(objStream);
        }

        public Stream DeleteMessageByMessageId(Stream objStream)
        {
            Messages message = new Messages();
            return message.DeleteMessageByMessageId(objStream);
        }

        public Stream DeleteMultipleMessageByMessageId(Stream objStream)
        {
            Messages message = new Messages();
            return message.DeleteMultipleMessageByMessageId(objStream);
        }

        public Stream GetMessageDetailByMessageId(Stream objStream)
        {
            Messages message = new Messages();
            return message.GetMessageDetailByMessageId(objStream);
        }

        public Stream SendMessage(Stream objStream)
        {
            Messages message = new Messages();
            return message.SendMessage(objStream);
        }


        public Stream SearchFriendsByName(Stream objStream)
        {
            UserFriends userFriends = new UserFriends();
            return userFriends.SearchFriendsByName(objStream);

        }

        public Stream GetUnfriendsByUserId(Stream objStream)
        {
            UserFriends userFriends = new UserFriends();
            return userFriends.GetUnfriendsByUserId(objStream);

        }

        public Stream SendFreindRequest(Stream objStream)
        {
            UserFriends userFriends = new UserFriends();
            return userFriends.SendFreindRequest(objStream);

        }

        public Stream CancelSendfriendsRequestByUserId(Stream objStream)
        {
            UserFriends userFriends = new UserFriends();
            return userFriends.CancelSendfriendsRequestByUserId(objStream);

        }

        public Stream AcceptGetfriendsRequestByUserId(Stream objStream)
        {
            UserFriends userFriends = new UserFriends();
            return userFriends.AcceptGetfriendsRequestByUserId(objStream);

        }

        public Stream CancelGetfriendsRequestByUserId(Stream objStream)
        {
            UserFriends userFriends = new UserFriends();
            return userFriends.CancelGetfriendsRequestByUserId(objStream);

        }

        public Stream UnFriendRequestByUserId(Stream objStream)
        {
            UserFriends userFriends = new UserFriends();
            return userFriends.UnFriendRequestByUserId(objStream);

        }

        public Stream GetFriendByUserId(Stream objStream)
        {
            UserFriends userFriends = new UserFriends();
            return userFriends.GetFriendByUserId(objStream);
        }

        public Stream CreateSubAdmin(Stream objStream)
        {
            SubAdminAccount subAdmin = new SubAdminAccount();
            return subAdmin.CreateSubAdmin(objStream);
        }

        public Stream CreateTutorSubTutor(Stream objStream)
        {
            SubAdminAccount subAdmin = new SubAdminAccount();
            return subAdmin.CreateTutorSubTutor(objStream);
        }

        public Stream UpdateSubAdmin(Stream objStream)
        {
            SubAdminAccount subAdmin = new SubAdminAccount();
            return subAdmin.UpdateSubAdmin(objStream);
        }

        public Stream GetTutorSubTutorList(Stream objStream)
        {
            SubAdminAccount subAdmin = new SubAdminAccount();
            return subAdmin.GetTutorSubTutorList(objStream);
        }

        public Stream DeleteSubAdmin(Stream objStream)
        {
            SubAdminAccount subAdmin = new SubAdminAccount();
            return subAdmin.DeleteSubAdmin(objStream);
        }

        public Stream DeleteSubUserById(Stream objStream)
        {
            SubAdminAccount subAdmin = new SubAdminAccount();
            return subAdmin.DeleteSubUserById(objStream);
        }

        public Stream SaveStudentTutor(Stream objStream)
        {
            Tutors objtutor = new Tutors();
            return objtutor.SaveStudentTutor(objStream);
        }


        public Stream GetAllTutors(Stream objStream)
        {
            Tutors objtutor = new Tutors();
            return objtutor.GetAllTutors(objStream);
        }

        public Stream GetTutorsByUserId(Stream objStream)
        {
            Tutors objtutor = new Tutors();
            return objtutor.GetTutorsByUserId(objStream);
        }

        public Stream GetAllTutor(Stream objStream)
        {
            Tutors objtutor = new Tutors();
            return objtutor.GetAllTutor(objStream);
        }

        public Stream AddSubscribeType(Stream objStream)
        {
            Subscribes objsubscribe = new Subscribes();
            return objsubscribe.AddSubscribeType(objStream);
        }

        public Stream AddBookMark(Stream objStream)
        {
            BookMarks objbookmark = new BookMarks();
            return objbookmark.AddBookMark(objStream);
        }

        public Stream RemoveBookMark(Stream objStream)
        {
            BookMarks objbookmark = new BookMarks();
            return objbookmark.RemoveBookMark(objStream);
        }

        public Stream GetBookmarkById(Stream objStream)
        {
            BookMarks objbookmark = new BookMarks();
            return objbookmark.GetBookmarkById(objStream);
        }

        public Stream AddQuery(Stream objStream)
        {
            Query query = new Query();
            return query.AddQuery(objStream);
        }

        public Stream AddQuery1(Stream objStream)
        {
            Query query = new Query();
            return query.AddQuery1(objStream);
        }

        public Stream ReadQuery(Stream objStream)
        {
            Query query = new Query();
            return query.ReadQuery(objStream);
        }

        public Stream DeleteQueryById(Stream objStream)
        {
            Query query = new Query();
            return query.DeleteQueryById(objStream);
        }

        public Stream AddVocabCategory(Stream objStream)
        {
            VocabCategory vocab = new VocabCategory();
            return vocab.AddVocabCategory(objStream);
        }

        public Stream GetCategoryByVocabId(Stream objStream)
        {
            VocabCategory vocab = new VocabCategory();
            return vocab.GetCategoryByVocabId(objStream);
        }

        public Stream GetAllVocabCategory(Stream objStream)
        {
            VocabCategory vocab = new VocabCategory();
            return vocab.GetAllVocabCategory(objStream);
        }

        public Stream UpdateVocabCategory(Stream objStream)
        {
            VocabCategory vocab = new VocabCategory();
            return vocab.UpdateVocabCategory(objStream);
        }

        public Stream DeleteVocabByCategoryId(Stream objStream)
        {
            VocabCategory vocab = new VocabCategory();
            return vocab.DeleteVocabByCategoryId(objStream);
        }

        public Stream AddVocabSubCategory(Stream objStream)
        {
            VocabSubCategory vocabSub = new VocabSubCategory();
            return vocabSub.AddVocabSubCategory(objStream);
        }

        public Stream GetSubCategoryByVacabSubId(Stream objStream)
        {
            VocabSubCategory vocabSub = new VocabSubCategory();
            return vocabSub.GetSubCategoryByVacabSubId(objStream);
        }

        public Stream GetAllVocabSubCategory(Stream objStream)
        {
            VocabSubCategory vocabSub = new VocabSubCategory();
            return vocabSub.GetAllVocabSubCategory(objStream);
        }

        public Stream GetSubCategoryByVocabId(Stream objStream)
        {
            VocabSubCategory vocabSub = new VocabSubCategory();
            return vocabSub.GetSubCategoryByVocabId(objStream);
        }

        public Stream UpdateVocabSubCategoryById(Stream objStream)
        {
            VocabSubCategory vocabSubCategory = new VocabSubCategory();
            return vocabSubCategory.UpdateVocabSubCategoryById(objStream);
        }

        public Stream DeleteVocabSubCategoryById(Stream objStream)
        {
            VocabSubCategory vocabSubCategory = new VocabSubCategory();
            return vocabSubCategory.DeleteVocabSubCategoryById(objStream);
        }

        public Stream AddVocabWord(Stream objStream)
        {
            VocabWord nocabWord = new VocabWord();
            return nocabWord.AddVocabWord(objStream);
        }


        public Stream GetVocabWordByWordId(Stream objStream)
        {
            VocabWord nocabWord = new VocabWord();
            return nocabWord.GetVocabWordByWordId(objStream);
        }
        public Stream GetAllVocabWord(Stream objStream)
        {
            VocabWord nocabWord = new VocabWord();
            return nocabWord.GetAllVocabWord(objStream);
        }

        public Stream GetVocabSubCategoryWordsById(Stream objStream)
        {
            VocabWord nocabWord = new VocabWord();
            return nocabWord.GetVocabSubCategoryWordsById(objStream);
        }

        public Stream GetVocabCategoryWordsById(Stream objStream)
        {
            VocabWord nocabWord = new VocabWord();
            return nocabWord.GetVocabCategoryWordsById(objStream);
        }


        public Stream GetWordsDetailByWordId(Stream objStream)
        {
            VocabWord nocabWord = new VocabWord();
            return nocabWord.GetWordsDetailByWordId(objStream);
        }

        public Stream NextWordDetailById(Stream objStream)
        {
            VocabWord nocabWord = new VocabWord();
            return nocabWord.NextWordDetailById(objStream);
        }

        public Stream BackWordDetailById(Stream objStream)
        {
            VocabWord nocabWord = new VocabWord();
            return nocabWord.BackWordDetailById(objStream);
        }


        public Stream GetVocabWordsByCategoryId(Stream objStream)
        {
            VocabWord nocabWord = new VocabWord();
            return nocabWord.GetVocabWordsByCategoryId(objStream);
        }

        public Stream UpdateVocabWordById(Stream objStream)
        {
            VocabWord nocabWord = new VocabWord();
            return nocabWord.UpdateVocabWordById(objStream);
        }

        public Stream DeleteVocabWordById(Stream objStream)
        {
            VocabWord nocabWord = new VocabWord();
            return nocabWord.DeleteVocabWordById(objStream);
        }

        public Stream DeleteUserById(Stream objStream)
        {
            UserAccount userAccount = new UserAccount();
            return userAccount.DeleteUserById(objStream);
        }

        public Stream MakeSuggestedByUserId(Stream objStream)
        {
            UserAccount userAccount = new UserAccount();
            return userAccount.MakeSuggestedByUserId(objStream);
        }

        public Stream AddUpdateHelpContent(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.AddUpdateHelpContent(objStream);
        }

        public Stream GetHelpContent(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.GetHelpContent(objStream);
        }

        public Stream GetSettingContent(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.GetSettingContent(objStream);
        }


        public Stream AddUpdateAboutContent(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.AddUpdateAboutContent(objStream);
        }

        public Stream AddUpdateFaqContent(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.AddUpdateFaqContent(objStream);
        }

        public Stream GetAllFAQ1(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.GetAllFAQ1(objStream);
        }

        public Stream GetAllFAQ1s(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.GetAllFAQ1s(objStream);
        }

        public Stream AddFaq1(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.AddFaq1(objStream);
        }

        public Stream UpdateFAQ(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.UpdateFAQ(objStream);
        }

        public Stream GetFaqByFaqId(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.GetFaqByFaqId(objStream);
        }


        public Stream DeleteFAQByFaqId(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.DeleteFAQByFaqId(objStream);
        }

        public Stream AddUpdateWelcomeContent(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.AddUpdateWelcomeContent(objStream);
        }
        public Stream NewsLetter(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.NewsLetter(objStream);
        }

        public Stream UserRoleMapping(Stream objStream)
        {
            RoleMapping roleMapping = new RoleMapping();
            return roleMapping.UserRoleMapping(objStream);
        }

        public Stream GetMenuByUserId(Stream objStream)
        {
            RoleMapping roleMapping = new RoleMapping();
            return roleMapping.GetMenuByUserId(objStream);
        }

        public Stream GetAllMenu(Stream objStream)
        {
            RoleMapping roleMapping = new RoleMapping();
            return roleMapping.GetAllMenu(objStream);
        }



        public Stream GetUserRoleByUserId(Stream objStream)
        {
            RoleMapping roleMapping = new RoleMapping();
            return roleMapping.GetUserRoleByUserId(objStream);
        }

        public Stream GetAllUnitGrammer(Stream objStream)
        {
            Grammer objGrammer = new Grammer();
            return objGrammer.GetAllUnitGrammer(objStream);
        }

        public Stream GetUnitGrammerByUnitId(Stream objStream)
        {
            Grammer objGrammer = new Grammer();
            return objGrammer.GetUnitGrammerByUnitId(objStream);
        }

        public Stream NextGrammerUnitById(Stream objStream)
        {
            Grammer objGrammer = new Grammer();
            return objGrammer.NextGrammerUnitById(objStream);
        }

        public Stream BackGrammerUnitById(Stream objStream)
        {
            Grammer objGrammer = new Grammer();
            return objGrammer.BackGrammerUnitById(objStream);
        }

        public Stream GrammerUnitById(Stream objStream)
        {
            Grammer objGrammer = new Grammer();
            return objGrammer.GrammerUnitById(objStream);
        }

        public Stream AddCourse(Stream objStream)
        {
            AdminCourse adminCourse = new AdminCourse();
            return adminCourse.AddCourse(objStream);
        }

        public Stream UpdateCourse(Stream objStream)
        {
            AdminCourse adminCourse = new AdminCourse();
            return adminCourse.UpdateCourse(objStream);
        }

        public Stream DeleteCourse(Stream objStream)
        {
            AdminCourse adminCourse = new AdminCourse();
            return adminCourse.DeleteCourse(objStream);
        }

        public Stream GetCourseByCourseId(Stream objStream)
        {
            AdminCourse adminCourse = new AdminCourse();
            return adminCourse.GetCourseByCourseId(objStream);
        }

        public Stream GetAllCourse(Stream objStream)
        {
            AdminCourse adminCourse = new AdminCourse();
            return adminCourse.GetAllCourse(objStream);
        }

        public Stream DeleteGrammerUnitByUnitId(Stream objStream)
        {
            Grammer adminCourse = new Grammer();
            return adminCourse.DeleteGrammerUnitByUnitId(objStream);
        }


        public Stream DashboardData(Stream objStream)
        {
            Dashboard dashBoard = new Dashboard();
            return dashBoard.DashboardData(objStream);
        }

        public Stream GetAllPayment(Stream objStream)
        {
            Payment payment = new Payment();
            return payment.GetAllPayment(objStream);
        }


        public Stream AddAssessmentQuestionByUnitId(Stream objStream)
        {
            Grammer grammer = new Grammer();
            return grammer.AddAssessmentQuestionByUnitId(objStream);
        }

        public Stream GetAssessmentQuestionByUnitId(Stream objStream)
        {
            Grammer grammer = new Grammer();
            return grammer.GetAssessmentQuestionByUnitId(objStream);
        }

        public Stream GetAnswersBySlotId(Stream objStream)
        {
            Grammer grammer = new Grammer();
            return grammer.GetAnswersBySlotId(objStream);
        }

        public Stream SubmitAnswerBySlotId(Stream objStream)
        {
            Grammer grammer = new Grammer();
            return grammer.SubmitAnswerBySlotId(objStream);
        }

        public Stream GetAllSubmmision(Stream objStream)
        {
            Submmision submmision = new Submmision();
            return submmision.GetAllSubmmision(objStream);
        }

        public Stream GetAllDialog(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.GetAllDialog(objStream);
        }

        public Stream GetDialogDetail(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.GetDialogDetail(objStream);
        }

        public Stream DeleteDialogByDialogId(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.DeleteDialogByDialogId(objStream);
        }

        public Stream ManageDialog(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.ManageDialog(objStream);
        }

        public Stream DeleteConversationByConversationId(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.DeleteConversationByConversationId(objStream);
        }

        public Stream DeleteKeyPhrasesByKeyPhrasesId(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.DeleteKeyPhrasesByKeyPhrasesId(objStream);
        }

        public Stream AddVocabQuestionByVocabId(Stream objStream)
        {
            VocabCategory vocab = new VocabCategory();
            return vocab.AddVocabQuestionByVocabId(objStream);
        }

        public Stream GetVocabQuestionByQuestionId(Stream objStream)
        {
            VocabCategory vocab = new VocabCategory();
            return vocab.GetVocabQuestionByQuestionId(objStream);
        }

        public Stream GetVocabQuestionBySet(Stream objStream)
        {
            VocabCategory vocab = new VocabCategory();
            return vocab.GetVocabQuestionBySet(objStream);
        }

        public Stream UpdateVocabQuestion(Stream objStream)
        {
            VocabCategory vocab = new VocabCategory();
            return vocab.UpdateVocabQuestion(objStream);
        }

        public Stream DeleteVocabQuestionByQuestionId(Stream objStream)
        {
            VocabCategory vocab = new VocabCategory();
            return vocab.DeleteVocabQuestionByQuestionId(objStream);
        }


        public Stream GetAllVocabQuestion(Stream objStream)
        {
            VocabCategory vocab = new VocabCategory();
            return vocab.GetAllVocabQuestion(objStream);
        }

        public Stream GetVocabByUserId(Stream objStream)
        {
            VocabCategory vocab = new VocabCategory();
            return vocab.GetVocabByUserId(objStream);
        }

        public Stream GetVocabByUserIdDevice(Stream objStream)
        {
            VocabCategory vocab = new VocabCategory();
            return vocab.GetVocabByUserIdDevice(objStream);
        }

        public Stream GetVocabSubCategoryByVocabId(Stream objStream)
        {
            VocabSubCategory vocabsub = new VocabSubCategory();
            return vocabsub.GetVocabSubCategoryByVocabId(objStream);
        }


        public Stream GetCorrectAnswerByQuestionId(Stream objStream)
        {
            VocabCategory vocab = new VocabCategory();
            return vocab.GetCorrectAnswerByQuestionId(objStream);
        }

        public Stream GetAssementByVocabId(Stream objStream)
        {
            VocabCategory vocab = new VocabCategory();
            return vocab.GetAssementByVocabId(objStream);
        }

        public Stream GetNextAssementByVocabId(Stream objStream)
        {
            VocabCategory vocab = new VocabCategory();
            return vocab.GetNextAssementByVocabId(objStream);
        }

        public Stream GetKeyPhrasesByKeyPhrasesId(Stream objStream)
        {
            Dialogs dialogs = new Dialogs();
            return dialogs.GetKeyPhrasesByKeyPhrasesId(objStream);

        }

        public Stream TermsAndCondition(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.TermsAndCondition(objStream);
        }

        public Stream SuccessStoryList(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.SuccessStoryList(objStream);
        }

        public Stream AddSuccessStory(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.AddSuccessStory(objStream);
        }

        public Stream DeleteStoryByStoryId(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.DeleteStoryByStoryId(objStream);
        }

        public Stream GetStoryByStoryId(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.GetStoryByStoryId(objStream);
        }

        public Stream UpdateStoryByStoryId(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.UpdateStoryByStoryId(objStream);
        }

        public Stream GetAllStory(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.GetAllStory(objStream);
        }

        public Stream GetVideoDetail(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.GetVideoDetail(objStream);
        }

        public Stream UpdateVideo(Stream objStream)
        {
            PageManagement pageManagement = new PageManagement();
            return pageManagement.UpdateVideo(objStream);
        }

        public Stream AdminLogin(Stream objStream)
        {
            SubAdminAccount objSubAdmin = new SubAdminAccount();
            return objSubAdmin.AdminLogin(objStream);
        }

        public Stream ForgotPasswordAdmin(Stream objStream)
        {
            SubAdminAccount objSubAdmin = new SubAdminAccount();
            return objSubAdmin.ForgotPasswordAdmin(objStream);
        }

        public Stream GetConversationByConversationId(Stream objStream)
        {
            Dialogs dialogs = new Dialogs();
            return dialogs.GetConversationByConversationId(objStream);
        }



        /// <summary>
        /// //////////////////////////////////////////////////////////////// Rahul//////////////////////////////////////////////
        /// </summary>
        /// <param name="objStream"></param>
        /// <returns></returns>

        public Stream SendNotification(Stream objStream)
        {
            Notifications objnotification = new Notifications();
            return objnotification.SendNotification(objStream);
        }

        public Stream GetAllNotification(Stream objStream)
        {
            Notifications objnotification = new Notifications();
            return objnotification.GetAllNotification(objStream);
        }

        public Stream Test(Stream objStream)
        {
            Notifications objnotification = new Notifications();
            return objnotification.Test(objStream);
        }

        public Stream MyActivityByUserId(Stream objStream)
        {
            Activity objactivity = new Activity();
            return objactivity.MyActivityByUserId(objStream);
        }




        public Stream ChangePassword(Stream objStream)
        {
            return userProfile.ChangePassword(objStream);
        }

        public Stream ChangePasswordAdmin(Stream objStream)
        {
            return userProfile.ChangePasswordAdmin(objStream);
        }
        //public Stream DeleteUser(Stream objStream)
        //{
        //    return userProfile.DeleteUser(objStream);
        //}

        public Stream AddDialog(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.AddDialog(objStream);
        }

        public Stream UpdateDialog(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.UpdateDialog(objStream);
        }

        public Stream GetDialogByDialogId(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.GetDialogByDialogId(objStream);
        }

        public Stream GetDialogByUserId(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.GetDialogByUserId(objStream);
        }

        public Stream GetDialogDetails(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.GetDialogDetails(objStream);
        }

        public Stream BackDialogDetails(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.BackDialogDetails(objStream);
        }

        public Stream NextDialogDetails(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.NextDialogDetails(objStream);
        }

        public Stream GetAssementByDialogId(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.GetAssementByDialogId(objStream);
        }

        public Stream GetNextAssementByDialogId(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.GetNextAssementByDialogId(objStream);
        }


        public Stream GetConversationByDialogId(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.GetConversationByDialogId(objStream);
        }

        public Stream GetDialogConversation(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.GetDialogConversation(objStream);
        }

        public Stream GetKeyPhrasesByDialogId(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.GetKeyPhrasesByDialogId(objStream);
        }

        public Stream AddKeyPhrasesByDialogId(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.AddKeyPhrasesByDialogId(objStream);
        }

        public Stream AddConversationByDialogId(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.AddConversationByDialogId(objStream);
        }
        //////////////////////////// 04 sept 2014

        public Stream UpdateConversationByConversationId(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.UpdateConversationByConversationId(objStream);
        }

        public Stream UpdateKeyPhrasesByKeyPhrasesId(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.UpdateKeyPhrasesByKeyPhrasesId(objStream);
        }

        public Stream AddAssessmentQuestionByDialogId(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.AddAssessmentQuestionByDialogId(objStream);
        }

        public Stream GetAssessmentQuestionByDialogId(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.GetAssessmentQuestionByDialogId(objStream);
        }

        public Stream SubmitAnswerByQuestionId(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.SubmitAnswerByQuestionId(objStream);
        }

        public Stream GetAnswersByDialogId(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.GetAnswersByDialogId(objStream);
        }

        public Stream GetAllUsers(Stream objStream)
        {
            UserAccount objUserAccount = new UserAccount();
            return objUserAccount.GetAllUsers(objStream);

        }

        public Stream GetAllUsersAdmin(Stream objStream)
        {
            UserAccount objUserAccount = new UserAccount();
            return objUserAccount.GetAllUsersAdmin(objStream);

        }

        public Stream FilterByGender(Stream objStream)
        {
            UserAccount objUserAccount = new UserAccount();
            return objUserAccount.FilterByGender(objStream);

        }

        public Stream FilterByCountry(Stream objStream)
        {
            UserAccount objUserAccount = new UserAccount();
            return objUserAccount.FilterByCountry(objStream);

        }

        public Stream SearchUser(Stream objStream)
        {
            UserAccount objUserAccount = new UserAccount();
            return objUserAccount.SearchUser(objStream);

        }

        public Stream AddUnitToGrammer(Stream objStream)
        {
            Grammer objGrammer = new Grammer();
            return objGrammer.AddUnitToGrammer(objStream);

        }

        public Stream GetAllUserQueries(Stream objStream)
        {
            Query objQuery = new Query();
            return objQuery.GetAllUserQueries(objStream);
        }

        public Stream GetAllUserQuery(Stream objStream)
        {
            Query objQuery = new Query();
            return objQuery.GetAllUserQuery(objStream);
        }

        public Stream GetAllSubAdminUsersList()
        {
            SubAdminAccount objSubAdmin = new SubAdminAccount();
            return objSubAdmin.GetAllSubAdminUsersList();
        }

        public Stream GetSubAdminByUserId(Stream objStream)
        {
            SubAdminAccount objSubAdmin = new SubAdminAccount();
            return objSubAdmin.GetSubAdminByUserId(objStream);
        }

        public static string StringToJsonConvertor(string message)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Message");
            dt.Rows.Add(message);
            return JsonConvert.SerializeObject(dt);
        }

        public Stream UpdateGrammerUnit(Stream objStream)
        {
            Grammer objGrammer = new Grammer();
            return objGrammer.UpdateGrammerUnit(objStream);
        }

        public Stream GetAssessmentQuestionBySlotId(Stream objStream)
        {
            Grammer objGrammer = new Grammer();
            return objGrammer.GetAssessmentQuestionBySlotId(objStream);
        }

        public Stream UpdateAssessmentQuestionBySlotId(Stream objStream)
        {
            Grammer objGrammer = new Grammer();
            return objGrammer.UpdateAssessmentQuestionBySlotId(objStream);
        }

        public Stream DeleteAssessmentQuestionBySlotId(Stream objStream)
        {
            Grammer objGrammer = new Grammer();
            return objGrammer.DeleteAssessmentQuestionBySlotId(objStream);
        }

        public Stream GetDialogAssessmentQuestionByQuestionId(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.GetDialogAssessmentQuestionByQuestionId(objStream);
        }

        public Stream UpdateDialogAssessmentQuestion(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.UpdateDialogAssessmentQuestion(objStream);
        }

        public Stream DeleteDialogAssessmentQuestion(Stream objStream)
        {
            Dialogs objDialog = new Dialogs();
            return objDialog.DeleteDialogAssessmentQuestion(objStream);
        }

        public Stream GetLoginType(Stream objStream)
        {
            UserAccount objUser = new UserAccount();
            return objUser.GetLoginType(objStream);
        }

        public Stream Logout(Stream objStream)
        {
            UserAccount objUser = new UserAccount();
            return objUser.Logout(objStream);
        }

        public Stream GetSubscriptionDetail(Stream objStream)
        {
            Subscription Subscription = new Subscription();
            return Subscription.GetSubscriptionDetail(objStream);
        }

        public Stream GetNormalPlanSubscriptionDetail(Stream objStream)
        {
            Subscription Subscription = new Subscription();
            return Subscription.GetNormalPlanSubscriptionDetail(objStream);
        }

        public Stream GetPremiumSubscriptionDetail(Stream objStream)
        {
            Subscription Subscription = new Subscription();
            return Subscription.GetPremiumSubscriptionDetail(objStream);
        }
        public Stream GetGrammerBySubscriptionId(Stream objStream)
        {
            Subscription Subscription = new Subscription();
            return Subscription.GetGrammerBySubscriptionId(objStream);
        }

        public Stream GetDialogBySubscriptionId(Stream objStream)
        {
            Subscription Subscription = new Subscription();
            return Subscription.GetDialogBySubscriptionId(objStream);
        }

        public Stream GetVocabBySubscriptionId(Stream objStream)
        {
            Subscription Subscription = new Subscription();
            return Subscription.GetVocabBySubscriptionId(objStream);
        }

        public Stream GetGrammerUnitDetail(Stream objStream)
        {
            Subscription Subscription = new Subscription();
            return Subscription.GetGrammerUnitDetail(objStream);
        }

        public Stream GetVocabUnitDetail(Stream objStream)
        {
            Subscription Subscription = new Subscription();
            return Subscription.GetVocabUnitDetail(objStream);
        }

        public Stream GetDialogUnitDetail(Stream objStream)
        {
            Subscription Subscription = new Subscription();
            return Subscription.GetDialogUnitDetail(objStream);
        }

        public Stream AddGrammerBySubscriptionId(Stream objStream)
        {
            Subscription Subscription = new Subscription();
            return Subscription.AddGrammerBySubscriptionId(objStream);
        }

        public Stream AddDialogBySubscriptionId(Stream objStream)
        {
            Subscription Subscription = new Subscription();
            return Subscription.AddDialogBySubscriptionId(objStream);
        }

        public Stream AddVocabBySubscriptionId(Stream objStream)
        {
            Subscription Subscription = new Subscription();
            return Subscription.AddVocabBySubscriptionId(objStream);
        }

        public Stream DeleteBySubscriptionId(Stream objStream)
        {
            Subscription Subscription = new Subscription();
            return Subscription.DeleteBySubscriptionId(objStream);
        }

        public Stream GetStudentByUserId(Stream objStream)
        {
            Tutors objtutor = new Tutors();
            return objtutor.GetStudentByUserId(objStream);
        }

        public Stream AddStudentByTutorId(Stream objStream)
        {
            Tutors objtutor = new Tutors();
            return objtutor.AddStudentByTutorId(objStream);
        }

    }
}
