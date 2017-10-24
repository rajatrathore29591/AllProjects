using System;
using System.Collections.Generic;
using System.Data;
using TradeMark.DAL;
using TradeMark.Models;

namespace TradeMark.BAL
{

    public class UserService
    {
        UserProvider oProvider = new UserProvider();
        public bool AddUser(UserModel oUser)
        {
            return oProvider.AddUser(oUser);
        }
        public List<UserModel> GetUser(string whereCondition, string OrderBy)
        {
            // SQL Data access process here
            var data = oProvider.GetUser(whereCondition, OrderBy);

            return new List<UserModel>();
        }
        /// <summary>
        /// Check email is available at time signup
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public bool CheckEmail(string emailId)
        {
            UserProvider oProvider = new UserProvider();
            bool emailAvailability = oProvider.CheckEmailAvailability(emailId);
            if (emailAvailability == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        ///  Check userid and password is valid or not
        /// </summary>
        /// <param name="objUserModel"></param>
        /// <returns></returns>
        public int UserLogin(UserModel objUserModel)
        {
            UserProvider oProvider = new UserProvider();
            int userDetail = oProvider.IsValidUser(objUserModel);

            return userDetail;

        }
        /// <summary>
        /// get the user name in respect to emailId
        /// </summary>
        /// <param name="email">email address of user</param>
        /// <returns>username</returns>
        public string GetUserNameByEmail(string email)
        {
            UserModel objUserModel = new UserModel();
            UserProvider oProvider = new UserProvider();
            string userName = oProvider.GetUserNameByEmail(email);
            return userName;
        }

        /// <summary>
        ///  check valid email id at the time of reset password
        /// </summary>
        /// <returns></returns>
        public bool ValidUser(UserModel objUserModel)
        {
            UserProvider oProvider = new UserProvider();
            bool userDetail = oProvider.IsValidEmailId(objUserModel);
            if (userDetail == true)
            {

                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Get user details by email id
        /// </summary>
        /// <param name="emailId">email id </param>
        /// <returns></returns>
        public UserModel GetUserDetail(string emailId = "", string id = null)
        {
            UserModel objUserModel = new UserModel();
            UserProvider oProvider = new UserProvider();
            objUserModel = oProvider.GetUserDetail(emailId, new Guid(id));
            return objUserModel;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUserModel"></param>
        /// <returns></returns>
        public bool LoginWithFacebook(UserModel objUserModel)
        {
            UserProvider oProvider = new UserProvider();
            bool userDetail = oProvider.IsValidEmailId(objUserModel);
            if (userDetail == true)
            {

                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Method to reset / change password
        /// </summary>
        /// <param name="emailId">email id </param>
        /// <param name="password">new password</param>
        public void ResetPassword(string emailId, string password)
        {

            UserProvider oProvider = new UserProvider();
            oProvider.ResetPassword(emailId, password);
            // return objUserModel;
        }
        public void EditProfile(UserModel objUserModel)
        {
            UserProvider oProvider = new UserProvider();
            oProvider.AddUser(objUserModel);
        }
        public int GetUserId(string emailId, string id)
        {
            UserProvider oProvider = new UserProvider();
            int userId = oProvider.GetUserId(emailId, id);
            return userId;
        }
        /// <summary>
        ///  here saving the search result
        /// </summary>
        public bool SaveSearchresult(string markIndex, string componentFullForm, int pointofContact, string Title, string goodsServices, string userId, string fullForm)
        {
            UserProvider oProvider = new UserProvider();
            bool value = oProvider.SaveSearchresult(markIndex, componentFullForm, pointofContact, Title, goodsServices, userId, fullForm);
            return value;
        }
        /// <summary>
        ///  here saving the search result
        /// </summary>
        public bool SaveSearchLog(string markIndex, string componentFullForm, string pointofContact, string goodsServices, string userId)
        {
            UserProvider oProvider = new UserProvider();
            bool value = oProvider.SaveSearchLog(markIndex, componentFullForm, pointofContact, goodsServices, userId);
            return value;
        }
        public DataTable GetUserSearchResult(string userId)
        {
            UserProvider oProvider = new UserProvider();
            List<MarkSearchModel> searchMarkList = new List<MarkSearchModel>();
            // searchMarkList = oProvider.GetUserSearchResult(userId);
            return oProvider.GetUserSearchResult(userId);
        }
        /// <summary>
        ///  Check is password field in database is empty or not
        /// </summary>
        /// <param name="UserId">Loged in user's id</param>
        /// <returns>return bool value</returns>
        public string PasswordAvailable(string UserId)
        {
            UserProvider oProvider = new UserProvider();
            string password = oProvider.PasswordAvailable(UserId);
            if (!String.IsNullOrEmpty(password))
            {
                return password;
            }
            return "";
        }
        /// <summary>
        ///  Check if feeded password is correct or not 
        /// </summary>
        /// <param name="UserId">Logged in user's id</param>
        /// <param name="password"> password feed in the textbox</param>
        /// <returns></returns>
        public bool PasswordCheck(string userId, string password)
        {
            string oldPassword = oProvider.PasswordAvailable(userId);
            if (oldPassword == password)
            {
                return true;
            }
            return false;

        }
        /// <summary>
        /// Change password of logged in user here
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public bool SetNewPassword(string userId, string password)
        {
            UserProvider oProvider = new UserProvider();
            bool Newpassword = oProvider.SetNewPassword(userId, password);
            if (Newpassword == true)
                return true;
            return false;
        }

        /// <summary>
        ///  Check userid and password is valid or not
        /// </summary>
        /// <param name="objUserModel"></param>
        /// <returns>UserModel</returns>
        public UserModel UserAuthentication(UserModel objUserModel)
        {
            UserProvider oProvider = new UserProvider();
            UserModel userDetail = oProvider.UserAuthentication(objUserModel);

            return userDetail;

        }
        /// <summary>
        /// Method to save the fileimporting status
        /// </summary>
        /// <param name="importFileStatus">file importing is in progress or Stop</param>
        /// <returns></returns>
        public string SaveAdminSetting(int importFileStatus, string email)
        {
            UserProvider oProvider = new UserProvider();
            return oProvider.SaveAdminSetting(importFileStatus, email);

        }
        /// <summary>
        ///  Method to get admin settings 
        /// </summary>
        /// <returns>AdminSettingModel</returns>
        public AdminSettingModel GetAdminSetting()
        {
            UserProvider oProvider = new UserProvider();
            AdminSettingModel adminSetting = oProvider.GetAdminSetting();

            return adminSetting;

        }
        /// <summary>
        /// Method to get All Users 
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllUsers()
        {
            UserProvider oProvider = new UserProvider();
            return oProvider.GetAllUsers();
        }
        /// <summary>
        /// Method for delete user by user id 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteUser(string userId)
        {
            return oProvider.DeleteUser(userId);
        }

        /// <summary>
        /// Method for edit user status by user id 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool EditUserStatus(string userId, bool userStatus)
        {
            return oProvider.EditUserStatus(userId, userStatus);
        }
        /// <summary>
        /// user buy the search credit from here
        /// </summary>
        /// <param name="token">stripe token</param>
        /// <param name="amount">amount pay to buy search credits</param>
        public PaymentOutputModel MakePayment(UserTransactionModel objUserTransactionModel)
        {
            var objPaymentOutputModel = oProvider.MakePayment(objUserTransactionModel);
            return objPaymentOutputModel;
        }
        /// <summary>
        /// Get PromoCode Detail
        /// </summary>
        /// <param name="promocode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public PromoCodeModel CheckPromoCode(string promocode, string userId)
        {
            var promoCodeDetail = oProvider.CheckPromoCode(promocode, userId);

            return promoCodeDetail;
        }
        /// <summary>
        /// Get user's current credits
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetUserCreditsById(string userid)
        {
            UserProvider oProvider = new UserProvider();
            int userCredits = oProvider.GetUserCreditsById(userid);
            return userCredits;
        }

        /// <summary>
        /// Get user's last transaction details to auto reload
        /// </summary>
        /// <param name="userid">userid </param>
        /// <returns></returns>
        public UserTransactionModel GetLastTransactionDetail(string userid)
        {
            UserProvider oProvider = new UserProvider();
            var objUserModel = oProvider.GetLastTransactionDetail(userid);
            return objUserModel;
        }

        /// <summary>
        /// Method to save promocode 
        /// </summary>
        /// <param name="objPromoCodeModel"> model param</param>
        public void SavePromoCode(PromoCodeModel objPromoCodeModel)
        {
            UserProvider oProvider = new UserProvider();
             oProvider.SavePromoCode(objPromoCodeModel);
            
        }
        
        public PromoCodeModel GetPromoCodeDetailsById(int promocodeId)
        {
            UserProvider oProvider = new UserProvider();
            return oProvider.GetPromoCodeDetailsById(promocodeId);

        }
        

    }
}