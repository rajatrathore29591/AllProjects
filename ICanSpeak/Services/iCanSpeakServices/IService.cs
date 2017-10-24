    using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace iCanSpeakServices
{

    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "UserRegistration", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream UserRegistration(Stream objStream);

        [WebInvoke(UriTemplate = "Login", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream Login(Stream objStream);

        [WebInvoke(UriTemplate = "GetUserCourseById", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetUserCourseById(Stream objStream);

        [WebInvoke(UriTemplate = "ForgotPassword", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream ForgotPassword(Stream objStream);

        [WebInvoke(UriTemplate = "GetUserProfileEdit", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetUserProfileEdit(Stream objStream);

        [WebInvoke(UriTemplate = "GetUserProfile", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetUserProfile(Stream objStream);

        [WebInvoke(UriTemplate = "GetMessageByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetMessageByUserId(Stream objStream);

        [WebInvoke(UriTemplate = "GetMessageDetailByMessageId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetMessageDetailByMessageId(Stream objStream);

        [WebInvoke(UriTemplate = "SendNotification", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream SendNotification(Stream objStream);

        [WebInvoke(UriTemplate = "EditUserProfile", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream EditUserProfile(Stream objStream);

        //[WebInvoke(UriTemplate = "SaveUserRelation", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        //Stream SaveUserRelation(Stream objStream);

        [WebInvoke(UriTemplate = "SearchFriendsByName", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream SearchFriendsByName(Stream objStream);

        [WebInvoke(UriTemplate = "GetFriendByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetFriendByUserId(Stream objStream);

        [WebInvoke(UriTemplate = "CreateSubAdmin", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream CreateSubAdmin(Stream objStream);

        [WebInvoke(UriTemplate = "CreateTutorSubTutor", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream CreateTutorSubTutor(Stream objStream);

        [WebInvoke(UriTemplate = "UpdateSubAdmin", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream UpdateSubAdmin(Stream objStream);

        [WebInvoke(UriTemplate = "GetTutorSubTutorList", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetTutorSubTutorList(Stream objStream);

        [WebInvoke(UriTemplate = "SaveStudentTutor", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream SaveStudentTutor(Stream objStream);

        [WebInvoke(UriTemplate = "GetAllTutors", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllTutors(Stream objStream);

        [WebInvoke(UriTemplate = "GetTutorsByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetTutorsByUserId(Stream objStream);



        [WebInvoke(UriTemplate = "DeleteSubAdmin", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteSubAdmin(Stream objStream);

        [WebInvoke(UriTemplate = "DeleteSubUserById", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteSubUserById(Stream objStream);

        [WebInvoke(UriTemplate = "AddQuery", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddQuery(Stream objStream);

        [WebInvoke(UriTemplate = "AddQuery1", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddQuery1(Stream objStream);

        [WebInvoke(UriTemplate = "ReadQuery", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream ReadQuery(Stream objStream);

        [WebInvoke(UriTemplate = "DeleteQueryById", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteQueryById(Stream objStream);

        [WebInvoke(UriTemplate = "AddVocabCategory", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddVocabCategory(Stream objStream);

        [WebInvoke(UriTemplate = "GetAllVocabCategory", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllVocabCategory(Stream objStream);

        [WebInvoke(UriTemplate = "GetCategoryByVocabId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetCategoryByVocabId(Stream objStream);

        [WebInvoke(UriTemplate = "UpdateVocabCategory", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream UpdateVocabCategory(Stream objStream);

        [WebInvoke(UriTemplate = "DeleteVocabByCategoryId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteVocabByCategoryId(Stream objStream);


        [WebInvoke(UriTemplate = "AddVocabSubCategory", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddVocabSubCategory(Stream objStream);

        [WebInvoke(UriTemplate = "GetSubCategoryByVacabSubId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetSubCategoryByVacabSubId(Stream objStream);

        [WebInvoke(UriTemplate = "GetAllVocabSubCategory", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllVocabSubCategory(Stream objStream);

        [WebInvoke(UriTemplate = "UpdateVocabSubCategoryById", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream UpdateVocabSubCategoryById(Stream objStream);

        [WebInvoke(UriTemplate = "DeleteVocabSubCategoryById", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteVocabSubCategoryById(Stream objStream);


        [WebInvoke(UriTemplate = "AddVocabWord", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddVocabWord(Stream objStream);

        [WebInvoke(UriTemplate = "GetVocabWordByWordId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetVocabWordByWordId(Stream objStream);

        [WebInvoke(UriTemplate = "GetAllVocabWord", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllVocabWord(Stream objStream);

        [WebInvoke(UriTemplate = "UpdateVocabWordById", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream UpdateVocabWordById(Stream objStream);

        [WebInvoke(UriTemplate = "DeleteVocabWordById", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteVocabWordById(Stream objStream);

        [WebInvoke(UriTemplate = "DeleteUserById", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteUserById(Stream objStream);

        [WebInvoke(UriTemplate = "MakeSuggestedByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream MakeSuggestedByUserId(Stream objStream);

        [WebInvoke(UriTemplate = "GetAllUserQueries", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllUserQueries(Stream objStream);

        [WebInvoke(UriTemplate = "GetAllUserQuery", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllUserQuery(Stream objStream);

        [WebInvoke(UriTemplate = "FilterByGender", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream FilterByGender(Stream objStream);

        [WebInvoke(UriTemplate = "FilterByCountry", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream FilterByCountry(Stream objStream);

        [WebInvoke(UriTemplate = "AddUpdateHelpContent", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddUpdateHelpContent(Stream objStream);

        [WebInvoke(UriTemplate = "GetHelpContent", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetHelpContent(Stream objStream);

        [WebInvoke(UriTemplate = "AddUpdateAboutContent", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddUpdateAboutContent(Stream objStream);

        [WebInvoke(UriTemplate = "AddUpdateFaqContent", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddUpdateFaqContent(Stream objStream);

        [WebInvoke(UriTemplate = "GetAllFAQ1", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllFAQ1(Stream objStream);

        [WebInvoke(UriTemplate = "GetAllFAQ1s", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllFAQ1s(Stream objStream);

        [WebInvoke(UriTemplate = "AddFaq1", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddFaq1(Stream objStream);

        [WebInvoke(UriTemplate = "UpdateFAQ", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream UpdateFAQ(Stream objStream);

            [WebInvoke(UriTemplate = "GetFaqByFaqId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetFaqByFaqId(Stream objStream);

            [WebInvoke(UriTemplate = "DeleteFAQByFaqId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteFAQByFaqId(Stream objStream);

        [WebInvoke(UriTemplate = "AddUpdateWelcomeContent", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddUpdateWelcomeContent(Stream objStream);

        [WebInvoke(UriTemplate = "NewsLetter", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream NewsLetter(Stream objStream);

        [WebInvoke(UriTemplate = "UserRoleMapping", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream UserRoleMapping(Stream objStream);

        [WebInvoke(UriTemplate = "GetMenuByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetMenuByUserId(Stream objStream);


        [WebInvoke(UriTemplate = "GetAllUnitGrammer", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllUnitGrammer(Stream objStream);

        [WebInvoke(UriTemplate = "GetUnitGrammerByUnitId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetUnitGrammerByUnitId(Stream objStream);

        [WebInvoke(UriTemplate = "GrammerUnitById", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GrammerUnitById(Stream objStream);

        [WebInvoke(UriTemplate = "NextGrammerUnitById", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream NextGrammerUnitById(Stream objStream);

        [WebInvoke(UriTemplate = "BackGrammerUnitById", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream BackGrammerUnitById(Stream objStream);

        [WebInvoke(UriTemplate = "DeleteGrammerUnitByUnitId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteGrammerUnitByUnitId(Stream objStream);

        [WebInvoke(UriTemplate = "AddCourse", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddCourse(Stream objStream);

        [WebInvoke(UriTemplate = "UpdateCourse", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream UpdateCourse(Stream objStream);

        [WebInvoke(UriTemplate = "DeleteCourse", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteCourse(Stream objStream);

        [WebInvoke(UriTemplate = "GetAllCourse", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllCourse(Stream objStream);

        [WebInvoke(UriTemplate = "GetCourseByCourseId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetCourseByCourseId(Stream objStream);

        [WebInvoke(UriTemplate = "GetUserRoleByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetUserRoleByUserId(Stream objStream);

        [WebInvoke(UriTemplate = "GetSubAdminByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetSubAdminByUserId(Stream objStream);

        [WebInvoke(UriTemplate = "GetAllMenu", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllMenu(Stream objStream);

        [WebInvoke(UriTemplate = "DashboardData", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DashboardData(Stream objStream);

        [WebInvoke(UriTemplate = "GetAllPayment", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllPayment(Stream objStream);

        [WebInvoke(UriTemplate = "AddAssessmentQuestionByUnitId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddAssessmentQuestionByUnitId(Stream objStream);

        [WebInvoke(UriTemplate = "GetAssessmentQuestionByUnitId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAssessmentQuestionByUnitId(Stream objStream);

        [WebInvoke(UriTemplate = "GetAnswersBySlotId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAnswersBySlotId(Stream objStream);

        [WebInvoke(UriTemplate = "SubmitAnswerBySlotId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream SubmitAnswerBySlotId(Stream objStream);

        [WebInvoke(UriTemplate = "GetAllSubmmision", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllSubmmision(Stream objStream);

        [WebInvoke(UriTemplate = "GetAllDialog", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllDialog(Stream objStream);

        [WebInvoke(UriTemplate = "GetDialogDetail", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetDialogDetail(Stream objStream);

        [WebInvoke(UriTemplate = "DeleteDialogByDialogId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteDialogByDialogId(Stream objStream);

        [WebInvoke(UriTemplate = "ManageDialog", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream ManageDialog(Stream objStream);

        [WebInvoke(UriTemplate = "DeleteConversationByConversationId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteConversationByConversationId(Stream objStream);

        [WebInvoke(UriTemplate = "DeleteKeyPhrasesByKeyPhrasesId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteKeyPhrasesByKeyPhrasesId(Stream objStream);

        [WebInvoke(UriTemplate = "AddVocabQuestionByVocabId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddVocabQuestionByVocabId(Stream objStream);

        [WebInvoke(UriTemplate = "GetVocabQuestionByQuestionId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetVocabQuestionByQuestionId(Stream objStream);

        [WebInvoke(UriTemplate = "GetVocabQuestionBySet", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetVocabQuestionBySet(Stream objStream);

        [WebInvoke(UriTemplate = "UpdateVocabQuestion", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream UpdateVocabQuestion(Stream objStream);

        [WebInvoke(UriTemplate = "DeleteVocabQuestionByQuestionId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteVocabQuestionByQuestionId(Stream objStream);


        [WebInvoke(UriTemplate = "GetAllVocabQuestion", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllVocabQuestion(Stream objStream);

        [WebInvoke(UriTemplate = "GetCorrectAnswerByQuestionId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetCorrectAnswerByQuestionId(Stream objStream);

        [WebInvoke(UriTemplate = "GetKeyPhrasesByKeyPhrasesId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetKeyPhrasesByKeyPhrasesId(Stream objStream);

          [WebInvoke(UriTemplate = "TermsAndCondition", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream TermsAndCondition(Stream objStream);

          [WebInvoke(UriTemplate = "SuccessStoryList", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
          Stream SuccessStoryList(Stream objStream);

          [WebInvoke(UriTemplate = "AddSuccessStory", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
          Stream AddSuccessStory(Stream objStream);

          [WebInvoke(UriTemplate = "DeleteStoryByStoryId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
          Stream DeleteStoryByStoryId(Stream objStream);

          [WebInvoke(UriTemplate = "GetStoryByStoryId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
          Stream GetStoryByStoryId(Stream objStream);

          [WebInvoke(UriTemplate = "UpdateStoryByStoryId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
          Stream UpdateStoryByStoryId(Stream objStream);

          [WebInvoke(UriTemplate = "GetAllStory", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
          Stream GetAllStory(Stream objStream);

          [WebInvoke(UriTemplate = "GetVideoDetail", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
          Stream GetVideoDetail(Stream objStream);

          [WebInvoke(UriTemplate = "UpdateVideo", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
          Stream UpdateVideo(Stream objStream);

            [WebInvoke(UriTemplate = "AdminLogin", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AdminLogin(Stream objStream);

            [WebInvoke(UriTemplate = "ForgotPasswordAdmin", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream ForgotPasswordAdmin(Stream objStream);

             [WebInvoke(UriTemplate = "GetConversationByConversationId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetConversationByConversationId(Stream objStream);

     
        ////////////////////////////////////////////////////Implemanted by Mahendra///////////////////////////////////

        //[WebInvoke(UriTemplate = "DeleteUser", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        //Stream DeleteUser(Stream objStream);

        [WebInvoke(UriTemplate = "ChangePassword", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream ChangePassword(Stream objStream);

        [WebInvoke(UriTemplate = "ChangePasswordAdmin", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream ChangePasswordAdmin(Stream objStream);

        [WebInvoke(UriTemplate = "AddDialog", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddDialog(Stream objStream);

        [WebInvoke(UriTemplate = "UpdateDialog", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream UpdateDialog(Stream objStream);

        [WebInvoke(UriTemplate = "GetDialogByDialogId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetDialogByDialogId(Stream objStream);

        [WebInvoke(UriTemplate = "GetDialogDetails", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetDialogDetails(Stream objStream);

        [WebInvoke(UriTemplate = "GetConversationByDialogId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetConversationByDialogId(Stream objStream);

        [WebInvoke(UriTemplate = "GetDialogConversation", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetDialogConversation(Stream objStream);

        [WebInvoke(UriTemplate = "GetKeyPhrasesByDialogId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetKeyPhrasesByDialogId(Stream objStream);

        [WebInvoke(UriTemplate = "AddConversationByDialogId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Stream AddConversationByDialogId(Stream objStream);

        /////////////////////////// 04-Sept-2014

        [WebInvoke(UriTemplate = "AddKeyPhrasesByDialogId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddKeyPhrasesByDialogId(Stream objStream);

        [WebInvoke(UriTemplate = "UpdateConversationByConversationId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream UpdateConversationByConversationId(Stream objStream);

        [WebInvoke(UriTemplate = "UpdateKeyPhrasesByKeyPhrasesId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream UpdateKeyPhrasesByKeyPhrasesId(Stream objStream);

        [WebInvoke(UriTemplate = "AddAssessmentQuestionByDialogId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddAssessmentQuestionByDialogId(Stream objStream);

        [WebInvoke(UriTemplate = "GetAssessmentQuestionByDialogId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAssessmentQuestionByDialogId(Stream objStream);

        [WebInvoke(UriTemplate = "SubmitAnswerByQuestionId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream SubmitAnswerByQuestionId(Stream objStream);

        [WebInvoke(UriTemplate = "GetAnswersByDialogId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAnswersByDialogId(Stream objStream);

        [WebInvoke(UriTemplate = "GetAllUsers", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllUsers(Stream objStream);

        [WebInvoke(UriTemplate = "GetAllUsersAdmin", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllUsersAdmin(Stream objStream);

        [WebInvoke(UriTemplate = "AddUnitToGrammer", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddUnitToGrammer(Stream objStream);

        [WebInvoke(UriTemplate = "GetAllSubAdminUsersList", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllSubAdminUsersList();

        [WebInvoke(UriTemplate = "UpdateGrammerUnit", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream UpdateGrammerUnit(Stream objStream);

        [WebInvoke(UriTemplate = "GetAssessmentQuestionBySlotId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAssessmentQuestionBySlotId(Stream objStream);

        [WebInvoke(UriTemplate = "UpdateAssessmentQuestionBySlotId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream UpdateAssessmentQuestionBySlotId(Stream objStream);

        [WebInvoke(UriTemplate = "DeleteAssessmentQuestionBySlotId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteAssessmentQuestionBySlotId(Stream objStream);

        [WebInvoke(UriTemplate = "GetDialogAssessmentQuestionByQuestionId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetDialogAssessmentQuestionByQuestionId(Stream objStream);

        [WebInvoke(UriTemplate = "UpdateDialogAssessmentQuestion", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream UpdateDialogAssessmentQuestion(Stream objStream);

        [WebInvoke(UriTemplate = "DeleteDialogAssessmentQuestion", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteDialogAssessmentQuestion(Stream objStream);

        [WebInvoke(UriTemplate = "GetVocabByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetVocabByUserId(Stream objStream);

        [WebInvoke(UriTemplate = "GetDialogByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetDialogByUserId(Stream objStream);

        [WebInvoke(UriTemplate = "SearchUser", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream SearchUser(Stream objStream);

        [WebInvoke(UriTemplate = "SendMessage", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream SendMessage(Stream objStream);

        [WebInvoke(UriTemplate = "GetAllTutor", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllTutor(Stream objStream);

        [WebInvoke(UriTemplate = "GetSettingContent", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetSettingContent(Stream objStream);

        [WebInvoke(UriTemplate = "GetSubCategoryByVocabId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetSubCategoryByVocabId(Stream objStream);

        [WebInvoke(UriTemplate = "GetAllNotification", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAllNotification(Stream objStream);

        [WebInvoke(UriTemplate = "Test", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream Test(Stream objStream);

        [WebInvoke(UriTemplate = "GetLoginType", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetLoginType(Stream objStream);

        [WebInvoke(UriTemplate = "SaveUserThirdParty", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream SaveUserThirdParty(Stream objStream);

        [WebInvoke(UriTemplate = "Logout", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream Logout(Stream objStream);

        [WebInvoke(UriTemplate = "GetVocabSubCategoryByVocabId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetVocabSubCategoryByVocabId(Stream objStream);

        [WebInvoke(UriTemplate = "GetVocabSubCategoryWordsById", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetVocabSubCategoryWordsById(Stream objStream);

        [WebInvoke(UriTemplate = "GetVocabCategoryWordsById", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetVocabCategoryWordsById(Stream objStream);

        [WebInvoke(UriTemplate = "EncryptId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream EncryptId(Stream objStream);

        [WebInvoke(UriTemplate = "GetVocabWordsByCategoryId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetVocabWordsByCategoryId(Stream objStream);

        [WebInvoke(UriTemplate = "GetWordsDetailByWordId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetWordsDetailByWordId(Stream objStream);

        [WebInvoke(UriTemplate = "NextWordDetailById", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream NextWordDetailById(Stream objStream);

        [WebInvoke(UriTemplate = "BackWordDetailById", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream BackWordDetailById(Stream objStream);

        [WebInvoke(UriTemplate = "NextDialogDetails", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream NextDialogDetails(Stream objStream);

        [WebInvoke(UriTemplate = "BackDialogDetails", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream BackDialogDetails(Stream objStream);

        [WebInvoke(UriTemplate = "GetVocabByUserIdDevice", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetVocabByUserIdDevice(Stream objStream);

        [WebInvoke(UriTemplate = "AddFlashCardWord", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddFlashCardWord(Stream objStream);

        [WebInvoke(UriTemplate = "RemoveFlashCardWord", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream RemoveFlashCardWord(Stream objStream);

        [WebInvoke(UriTemplate = "FlashCardByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream FlashCardByUserId(Stream objStream);

        [WebInvoke(UriTemplate = "NextFlashCardDetail", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream NextFlashCardDetail(Stream objStream);

        [WebInvoke(UriTemplate = "BackFlashCardDetail", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream BackFlashCardDetail(Stream objStream);

        [WebInvoke(UriTemplate = "SentMessageByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream SentMessageByUserId(Stream objStream);

        [WebInvoke(UriTemplate = "DeleteMessageByMessageId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteMessageByMessageId(Stream objStream);

        [WebInvoke(UriTemplate = "DeleteMultipleMessageByMessageId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteMultipleMessageByMessageId(Stream objStream);

        [WebInvoke(UriTemplate = "GetAssementByDialogId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAssementByDialogId(Stream objStream);

        [WebInvoke(UriTemplate = "GetNextAssementByDialogId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetNextAssementByDialogId(Stream objStream);

        [WebInvoke(UriTemplate = "GetAssementByVocabId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAssementByVocabId(Stream objStream);

        [WebInvoke(UriTemplate = "GetNextAssementByVocabId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetNextAssementByVocabId(Stream objStream);

        [WebInvoke(UriTemplate = "GetUnfriendsByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetUnfriendsByUserId(Stream objStream);

        [WebInvoke(UriTemplate = "SendFreindRequest", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream SendFreindRequest(Stream objStream);

        [WebInvoke(UriTemplate = "CancelSendfriendsRequestByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream CancelSendfriendsRequestByUserId(Stream objStream);

        [WebInvoke(UriTemplate = "AcceptGetfriendsRequestByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AcceptGetfriendsRequestByUserId(Stream objStream);

        [WebInvoke(UriTemplate = "CancelGetfriendsRequestByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream CancelGetfriendsRequestByUserId(Stream objStream);

        [WebInvoke(UriTemplate = "UnFriendRequestByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream UnFriendRequestByUserId(Stream objStream);

        [WebInvoke(UriTemplate = "GetStudentByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetStudentByUserId(Stream objStream);

        [WebInvoke(UriTemplate = "AddBookMark", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddBookMark(Stream objStream);

        [WebInvoke(UriTemplate = "RemoveBookMark", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream RemoveBookMark(Stream objStream);

        [WebInvoke(UriTemplate = "GetBookmarkById", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetBookmarkById(Stream objStream);

        [WebInvoke(UriTemplate = "MyActivityByUserId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream MyActivityByUserId(Stream objStream);


        [WebInvoke(UriTemplate = "GetSubscriptionDetail", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetSubscriptionDetail(Stream objStream);

        [WebInvoke(UriTemplate = "GetNormalPlanSubscriptionDetail", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetNormalPlanSubscriptionDetail(Stream objStream);

        [WebInvoke(UriTemplate = "GetPremiumSubscriptionDetail", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetPremiumSubscriptionDetail(Stream objStream);

        [WebInvoke(UriTemplate = "GetGrammerBySubscriptionId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetGrammerBySubscriptionId(Stream objStream);

        [WebInvoke(UriTemplate = "GetDialogBySubscriptionId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetDialogBySubscriptionId(Stream objStream);

        [WebInvoke(UriTemplate = "GetVocabBySubscriptionId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetVocabBySubscriptionId(Stream objStream);

        [WebInvoke(UriTemplate = "GetGrammerUnitDetail", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetGrammerUnitDetail(Stream objStream);

        [WebInvoke(UriTemplate = "GetVocabUnitDetail", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetVocabUnitDetail(Stream objStream);

        [WebInvoke(UriTemplate = "GetDialogUnitDetail", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetDialogUnitDetail(Stream objStream);

        [WebInvoke(UriTemplate = "AddGrammerBySubscriptionId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddGrammerBySubscriptionId(Stream objStream);

        [WebInvoke(UriTemplate = "AddDialogBySubscriptionId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddDialogBySubscriptionId(Stream objStream);

        [WebInvoke(UriTemplate = "AddVocabBySubscriptionId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddVocabBySubscriptionId(Stream objStream);

        [WebInvoke(UriTemplate = "DeleteBySubscriptionId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream DeleteBySubscriptionId(Stream objStream);

        [WebInvoke(UriTemplate = "AddStudentByTutorId", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddStudentByTutorId(Stream objStream);

        [WebInvoke(UriTemplate = "AddSubscribeType", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AddSubscribeType(Stream objStream);
    }
}
