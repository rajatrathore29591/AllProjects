using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TradeMark.Models;
using TradeMark.App_Data;
using System.Data.SqlClient;
using System.Data;
using TradeMark.Utility;
using System.Configuration;

namespace TradeMark.DAL
{
    public class UserProvider
    {
        /// <summary>
        /// Insert and update user's data 
        /// </summary>
        /// <param name="oUser"></param>
        /// <returns></returns>
        public bool AddUser(UserModel oUser)
        {
            // SQL Data access process here, 
            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();
            SqlCommand sqlcom = new SqlCommand();
            sqlcon.ConnectionString = connectionstring;
            try
            {
                Guid userId;
                sqlcon.Open();

                sqlcom.Connection = sqlcon;
                //Put the sql stored procedure ........
                sqlcom.CommandText = "[sp_SaveUpdateUserRegistraion]";
                //add parametre    

                if (oUser.UserId == null)
                {
                    userId = Guid.NewGuid();
                }
                else
                {
                    userId = new Guid(oUser.UserId);
                }
                sqlcom.Parameters.AddWithValue("@userId", userId);
                sqlcom.Parameters.AddWithValue("@firstName", oUser.FirstName);
                sqlcom.Parameters.AddWithValue("@emailId", oUser.Email);
                sqlcom.Parameters.AddWithValue("@userName", oUser.UserName);
                sqlcom.Parameters.AddWithValue("@password", oUser.Password);
                sqlcom.Parameters.AddWithValue("@socialMediaUser", oUser.MediaProvider);
                sqlcom.Parameters.AddWithValue("@profilePic", "");
                sqlcom.Parameters.AddWithValue("@contactNo", oUser.ContactNo);
                sqlcom.Parameters.AddWithValue("@socialMediaUserId", oUser.SocialMediaUserId);
                sqlcom.Parameters.AddWithValue("@lastName", oUser.LastName);
                sqlcom.Parameters.AddWithValue("@companyName", oUser.CompanyName);
                sqlcom.Parameters.AddWithValue("@title", oUser.Title);
                sqlcom.Parameters.AddWithValue("@streetaddress", oUser.StreetAddress);

                sqlcom.Parameters.AddWithValue("@isActive", oUser.IsActive);

                sqlcom.CommandType = CommandType.StoredProcedure;
                sqlcom.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }



        }

        /// <summary>
        /// Get all user
        /// </summary>
        /// <param name="whereCondition"></param>
        /// <param name="OrderBy"></param>
        /// <returns></returns>
        public List<UserModel> GetUser(string whereCondition, string OrderBy)
        {
            // SQL Data access process here


            return new List<UserModel>();

        }

        /// <summary>
        /// Email Id is available or notm
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public bool CheckEmailAvailability(string emailId)
        {
            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();

            sqlcon.ConnectionString = connectionstring;

            sqlcon.Open();
            SqlCommand cmd = new SqlCommand("select EmailId from Registration where EmailId=@emailId", sqlcon);
            cmd.Parameters.AddWithValue("@EmailId", emailId);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        /// <summary>
        /// Check during login user is register or not 
        /// if user login with social media then if it already register 
        /// then we do'nt insert its information and vise versa
        /// </summary>
        /// <param name="objUserModel"></param>
        /// <returns></returns>
        public int IsValidUser(UserModel objUserModel)
        {
            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();

            sqlcon.ConnectionString = connectionstring;
            sqlcon.Open();
            int userId = 0;
            if (objUserModel == null)
                return userId;
            if (string.IsNullOrEmpty(objUserModel.SocialMediaUserId))
            {
                SqlCommand cmd = new SqlCommand("select UserId from Registration where EmailId=@EmailId and Password=@Password", sqlcon);
                cmd.Parameters.AddWithValue("@EmailId", objUserModel.Email);
                cmd.Parameters.AddWithValue("@Password", objUserModel.Password);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr != null && dr.HasRows)
                {
                    while (dr.Read())
                    {
                        userId = dr.GetInt32(0);

                    }
                    return userId;
                }
                else
                {
                    return userId;
                }
            }
            if (objUserModel.SocialMediaUserId != null)
            {
                SqlCommand cmd = new SqlCommand("select UserId from Registration where SocialMediaUserId=@SocialMediaUserId", sqlcon);
                cmd.Parameters.AddWithValue("@SocialMediaUserId", objUserModel.SocialMediaUserId);

                SqlDataReader dr = cmd.ExecuteReader();
                //if (dr != null && dr.HasRows)
                //{
                //    userId = dr.GetInt32(0);
                //    return userId;
                //}
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        userId = dr.GetInt32(0);
                    }
                    //return userId;
                }
                else
                {
                    // save user in the db if not exits
                    AddUser(objUserModel);
                    return userId;
                }
            }

            return userId;
        }

        /// <summary>
        /// Check Valid email id
        /// </summary>
        /// <param name="objUserModel"></param>
        /// <returns></returns>
        public bool IsValidEmailId(UserModel objUserModel)
        {
            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();

            SqlCommand sqlcom = new SqlCommand();
            sqlcon.ConnectionString = connectionstring;
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand("select EmailId from Registration where EmailId=@EmailId ", sqlcon);
            cmd.Parameters.AddWithValue("@EmailId", objUserModel.Email);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///  Get the user detail regarding specific EmailId
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public UserModel GetUserDetail(string emailId, Guid userId)
        {
            UserModel objUserModel = new UserModel();
            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();

            sqlcon.ConnectionString = connectionstring;
            sqlcon.Open();
            if (!String.IsNullOrEmpty(emailId))
            {
                SqlCommand cmd = new SqlCommand("select  * from Registration where EmailId=@EmailId ", sqlcon);
                cmd.Parameters.AddWithValue("@EmailId", emailId);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        objUserModel.UserId = dr.GetSqlValue(0).ToString();
                        objUserModel.FirstName = dr.GetSqlValue(1).ToString();
                        objUserModel.Email = dr.GetSqlValue(6).ToString();
                        objUserModel.UserName = dr.GetSqlValue(8).ToString();
                        objUserModel.LastName = dr.GetSqlValue(2).ToString();
                        objUserModel.CompanyName = dr.GetSqlValue(3).ToString();
                        objUserModel.Title = dr.GetSqlValue(4).ToString();
                        objUserModel.StreetAddress = dr.GetSqlValue(5).ToString();
                        objUserModel.ContactNo = dr.GetSqlValue(15).ToString();
                        objUserModel.RegisteredDate = Convert.ToDateTime(dr.GetSqlValue(12).ToString());
                        objUserModel.IsActive = dr.GetSqlValue(11).ToString();
                        objUserModel.StripeCustomerId = dr.GetSqlValue(17).ToString();
                    }
                }
            }
            else
            {
                //select user data from the Registration table
                if (userId != null)
                {
                    SqlCommand cmd = new SqlCommand("select * from Registration where UserId=@UserId ", sqlcon);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            //fill data in UserModel from the Registration table 
                            objUserModel.FirstName = dr.GetSqlValue(1).ToString();
                            objUserModel.Email = dr.GetSqlValue(6).ToString();
                            objUserModel.UserName = dr.GetSqlValue(8).ToString();
                            objUserModel.LastName = dr.GetSqlValue(2).ToString();
                            objUserModel.CompanyName = dr.GetSqlValue(3).ToString();
                            objUserModel.Title = dr.GetSqlValue(4).ToString();
                            objUserModel.StreetAddress = dr.GetSqlValue(5).ToString();
                            objUserModel.ContactNo = dr.GetSqlValue(15).ToString();
                            objUserModel.RegisteredDate = Convert.ToDateTime(dr.GetSqlValue(12).ToString());
                            objUserModel.IsActive = dr.GetSqlValue(11).ToString();
                            objUserModel.StripeCustomerId = dr.GetSqlValue(17).ToString();
                        }
                    }
                }
            }
            return objUserModel;
        }

        /// <summary>
        /// get user name by its email Id
        /// </summary>
        /// <param name="email"> email address of user</param>
        /// <returns>user name</returns>
        public string GetUserNameByEmail(string email)
        {
            var connectionstring = SqlHelper.GetConnectionString();
            string userName = "";
            SqlConnection sqlcon = new SqlConnection();

            SqlCommand sqlcom = new SqlCommand();
            sqlcon.ConnectionString = connectionstring;
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand("select * from Registration where EmailId=@EmailId ", sqlcon);
            cmd.Parameters.AddWithValue("@EmailId", email);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    userName = dr.GetString(1);
                }

            }
            return userName;


        }

        /// <summary>
        /// method to reset/change password
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="newPassword"></param>
        public void ResetPassword(string emailId, string newPassword)
        {

            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();
            SqlCommand sqlcom = new SqlCommand();
            sqlcon.ConnectionString = connectionstring;

            try
            {
                sqlcon.Open();

                sqlcom.Connection = sqlcon;
                //Put the sql stored procedure ........
                sqlcom.CommandText = "[sp_ResetChangeUserpassword]";
                //add parametre
                sqlcom.Parameters.AddWithValue("@emailId", emailId);
                sqlcom.Parameters.AddWithValue("@newPassword", newPassword);
                sqlcom.CommandType = CommandType.StoredProcedure;

                sqlcom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        ///  get the user Id 
        ///  /Id get whether the user is login from the Tm or from other social media
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetUserId(string emailId, string id)
        {

            Console.WriteLine(id);
            UserModel objUserModel = new UserModel();
            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();
            int userId = 0;
            sqlcon.ConnectionString = connectionstring;
            sqlcon.Open();
            if (!string.IsNullOrEmpty(emailId))
            {
                SqlCommand cmd = new SqlCommand("select * from Registration where EmailId=@EmailId ", sqlcon);
                cmd.Parameters.AddWithValue("@EmailId", emailId);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        userId = dr.GetInt32(0);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(id))
                {
                    SqlCommand cmd = new SqlCommand("select * from Registration where SocialMediaUserId=@SocialMediaUserId", sqlcon);//@SocialMediaUserId
                    cmd.Parameters.AddWithValue("@SocialMediaUserId", id);
                    SqlDataReader dr = cmd.ExecuteReader();
                    Console.WriteLine(dr);
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            userId = dr.GetInt32(0);
                        }
                    }
                }
            }
            return userId;
        }

        /// <summary>
        ///save the search data
        /// </summary>
        /// <param name="markIndex">searchTMText</param>
        /// <param name="componentFullForm">componentsFullForm</param>
        /// <param name="pointofContact">filterOption</param>
        /// <param name="Title">searchTitle</param>
        /// <param name="goodsServices">usClassDescriptionId</param>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public bool SaveSearchresult(string markIndex, string componentFullForm, int pointofContact, string Title, string goodsServices, string userId, string fullForm)
        {

            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();
            SqlCommand sqlcom = new SqlCommand();
            sqlcon.ConnectionString = connectionstring;
            var guid = Guid.NewGuid();

            try
            {
                sqlcon.Open();

                sqlcom.Connection = sqlcon;
                //Put the sql stored procedure ........
                sqlcom.CommandText = "[sp_SaveMarkSearch]";
                //add parametre
                sqlcom.Parameters.AddWithValue("@searchTMText", markIndex);
                sqlcom.Parameters.AddWithValue("@filterOption", pointofContact);
                sqlcom.Parameters.AddWithValue("@userId", userId);
                sqlcom.Parameters.AddWithValue("@componentsFullForm", componentFullForm);
                sqlcom.Parameters.AddWithValue("@usClassDescriptionId", goodsServices);
                sqlcom.Parameters.AddWithValue("@searchTitle", Title);
                sqlcom.Parameters.AddWithValue("@searchGuid", guid);
                sqlcom.Parameters.AddWithValue("@fullForm", fullForm);


                sqlcom.CommandType = CommandType.StoredProcedure;
                sqlcom.ExecuteNonQuery();
                return true;

            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
                sqlcon.Close();
            }

        }

        /// <summary>
        ///save the search data
        /// </summary>
        /// <param name="markIndex">searchTMText</param>
        /// <param name="componentFullForm">componentsFullForm</param>
        /// <param name="pointofContact">filterOption</param>
        /// <param name="Title">searchTitle</param>
        /// <param name="goodsServices">usClassDescriptionId</param>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public bool SaveSearchLog(string markIndex, string componentFullForm, string pointofContact, string goodsServices, string userId)
        {

            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();
            SqlCommand sqlcom = new SqlCommand();
            sqlcon.ConnectionString = connectionstring;
            var guid = Guid.NewGuid();

            try
            {
                sqlcon.Open();

                sqlcom.Connection = sqlcon;
                //Put the sql stored procedure ........
                sqlcom.CommandText = "[sp_SaveSearchLog]";
                //add parametre
                sqlcom.Parameters.AddWithValue("@searchTMText", markIndex);
                sqlcom.Parameters.AddWithValue("@filterOption", pointofContact);
                sqlcom.Parameters.AddWithValue("@userId", userId);
                sqlcom.Parameters.AddWithValue("@componentsFullForm", componentFullForm);
                sqlcom.Parameters.AddWithValue("@usClassDescriptionId", goodsServices);
                sqlcom.Parameters.AddWithValue("@searchGuid", guid);

                sqlcom.CommandType = CommandType.StoredProcedure;
                sqlcom.ExecuteNonQuery();
                return true;

            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
                sqlcon.Close();
            }

        }

        /// <summary>
        /// Method to get the save Search list 
        /// </summary>
        /// <param name="userId">User id </param>
        /// <returns>List<MarkSearchModel></returns>
        public DataTable GetUserSearchResult(string userId)
        {

            List<MarkSearchModel> searchMarkList = new List<MarkSearchModel>();
            DataTable dtMakSearch = null;
            if (userId != null && userId != "")
            {

                string query = "select * from SaveMarkSearch where UserId= '" + userId + "'";
                dtMakSearch = SqlHelper.FillDataTable(query);


                searchMarkList = (from DataRow dr in dtMakSearch.Rows
                                  select new MarkSearchModel()
                                  {
                                      SearchId = Convert.ToInt32(dr["SearchId"]),
                                      SearchText = dr["SearchText"].ToString(),
                                      SearchDate = Convert.ToDateTime(dr["SearchDate"]),
                                      FilterOption = Convert.ToInt16(dr["FilterOption"]),
                                      UserId = dr["UserId"].ToString(),
                                      ComponentsFullForm = dr["ComponentsFullForm"].ToString(),
                                      UsClassDescriptionId = dr["UsClassDescriptionId"].ToString(),
                                      Title = dr["Title"].ToString(),
                                      SearchGuid = dr["SearchGuid"].ToString(),
                                  }).ToList();
            }

            return dtMakSearch;
        }

        /// <summary>
        /// Get the user password
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string PasswordAvailable(string userId)
        {
            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();
            sqlcon.ConnectionString = connectionstring;
            sqlcon.Open();
            try
            {

                SqlCommand cmd = new SqlCommand("select * from Registration where UserId=@userId ", sqlcon);
                cmd.Parameters.AddWithValue("@userId", userId);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        string password = dr.GetString(9);
                        if (!String.IsNullOrEmpty(password))
                        {
                            return password;
                        }
                        else
                        {
                            return "";
                        }

                    }
                }
                return "";

            }
            catch (Exception ex)
            {
                return "";
                throw ex;
            }
            finally
            {
                sqlcon.Close();
            }

        }

        /// <summary>
        /// Change password of logged in user here
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public bool SetNewPassword(string userId, string password)
        {
            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();
            SqlCommand sqlcom = new SqlCommand();
            sqlcon.ConnectionString = connectionstring;

            try
            {
                sqlcon.Open();

                sqlcom.Connection = sqlcon;
                //Put the sql stored procedure ........
                sqlcom.CommandText = "[sp_ChangeUserpassword]";
                //add parametre
                sqlcom.Parameters.AddWithValue("@userId", userId);
                sqlcom.Parameters.AddWithValue("@newPassword", password);
                sqlcom.CommandType = CommandType.StoredProcedure;

                sqlcom.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Method to authenticat user 
        /// </summary>
        /// <param name="objUserModel"></param>
        /// <returns> UserModel</returns>
        public UserModel UserAuthentication(UserModel objModel)
        {
            UserModel objUserModel = new UserModel();
            SqlConnection sqlcon = new SqlConnection();
            sqlcon.ConnectionString = SqlHelper.GetConnectionString();
            sqlcon.Open();
            if (!String.IsNullOrEmpty(objModel.Email))
            {
                SqlCommand cmd = new SqlCommand("select * from Registration where EmailId=@EmailId and Password=@Password", sqlcon);
                cmd.Parameters.AddWithValue("@EmailId", objModel.Email);
                cmd.Parameters.AddWithValue("@Password", objModel.Password);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        objUserModel.UserId = dr.GetSqlValue(0).ToString();
                        objUserModel.FirstName = dr.GetSqlValue(1).ToString();
                        objUserModel.LastName = dr.GetSqlValue(2).ToString();
                        objUserModel.Email = dr.GetSqlValue(2).ToString();
                        objUserModel.UserName = dr.GetSqlValue(4).ToString();
                        objUserModel.ContactNo = dr.GetSqlValue(15).ToString();
                        objUserModel.IsActive = dr.GetSqlValue(11).ToString();
                        objUserModel.IsAdmin = Convert.ToBoolean(dr.GetSqlValue(16).ToString());
                    }
                }


            }
            return objUserModel;
        }

        /// <summary>
        /// Method to update admin setting for file import
        /// </summary>
        /// <param name="importFileStatus">status of file import (importin/not importing)</param>
        /// <returns></returns>
        public string SaveAdminSetting(int importFileStatus, string email)
        {
            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();
            SqlCommand sqlcom = new SqlCommand();
            sqlcon.ConnectionString = connectionstring;
            //var filedate = getLatestImportedFileDate();

            try
            {
                sqlcon.Open();

                sqlcom.Connection = sqlcon;
                //Put the sql stored procedure ........
                sqlcom.CommandText = "[sp_UpdateAdminSetting]";
                //add parametre
                sqlcom.Parameters.AddWithValue("@statusFlag", importFileStatus);
                sqlcom.Parameters.AddWithValue("@email", email);
                sqlcom.CommandType = CommandType.StoredProcedure;
                sqlcom.ExecuteNonQuery();
                return "Sucessfuly Save";
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }


        /// <summary>
        /// method to get the Date from the imported last file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string getLatestImportedFileDate(string filename)
        {
            string fileDate = string.Empty;
            if (!string.IsNullOrEmpty(filename))
            {
                string substringFilename = string.Empty;
                string year = "20";
                string month = string.Empty;
                string date = string.Empty;
                //apc160131
                filename = filename.Split('.')[0];

                substringFilename = filename.Substring(3, 6);
                //160131:= 15 Jan 2016
                year = year + substringFilename.Substring(0, 2);
                month = substringFilename.Substring(2, 2);
                date = substringFilename.Substring(4, 2);
                fileDate = ReturnMonthName(month) + " " + date + ", " + year;
            }
            return fileDate;
        }

        public string ReturnMonthName(string monthnumber)
        {
            string monthName = string.Empty;
            switch (monthnumber)
            {
                case "01":
                    monthName = "January";
                    break;
                case "02":
                    monthName = "February";
                    break;
                case "03":
                    monthName = "March";
                    break;
                case "04":
                    monthName = "April";
                    break;
                case "05":
                    monthName = "May";
                    break;
                case "06":
                    monthName = "June";
                    break;
                case "07":
                    monthName = "July";
                    break;
                case "08":
                    monthName = "August";
                    break;
                case "09":
                    monthName = "September";
                    break;
                case "10":
                    monthName = "October";
                    break;
                case "11":
                    monthName = "November";
                    break;
                case "12":
                    monthName = "December";
                    break;
            }
            return monthName;


        }

        public AdminSettingModel GetAdminSetting()
        {

            string query = "Select statusFlag,LatestFileNameImport,AdminEmail from AdminSettings";
            DataTable dtAdminSettings = SqlHelper.FillDataTable(query);

            //SqlCommand cmd = new SqlCommand("Select statusFlag,LatestFileImportDate from AdminSettings", sqlcon);

            AdminSettingModel oAdminSettingModel = new AdminSettingModel();
            if (dtAdminSettings != null && dtAdminSettings.Rows.Count > 0)
            {
                oAdminSettingModel.StatusFlag = Convert.ToBoolean(dtAdminSettings.Rows[0]["statusFlag"]);
                oAdminSettingModel.LatestFileImportDate = getLatestImportedFileDate(Convert.ToString(dtAdminSettings.Rows[0]["LatestFileNameImport"]));
                oAdminSettingModel.AdminEmail = dtAdminSettings.Rows[0]["AdminEmail"].ToString();
            }
            return oAdminSettingModel;
        }

        /// <summary>
        /// Select all users from the "Registration" table
        /// </summary>
        /// <param name="objModel"></param>
        /// <returns></returns>
        public DataTable GetAllUsers()
        {
            string query = "usp_GetAllUsers";
            DataTable dtUsers = SqlHelper.FillDataTable(query);
            return dtUsers;
        }

        /// <summary>
        /// Method for delete user by uesr id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>retuen boolean result </returns>
        public bool DeleteUser(string userId)
        {
            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();
            SqlCommand sqlcom = new SqlCommand();
            sqlcon.ConnectionString = connectionstring;
            try
            {
                sqlcon.Open();
                sqlcom.Connection = sqlcon;
                //Put the sql stored procedure ........
                sqlcom.CommandText = "[usp_DeleteUser]";
                //add parametre
                sqlcom.Parameters.AddWithValue("@userId", new Guid(userId));

                sqlcom.CommandType = CommandType.StoredProcedure;
                sqlcom.ExecuteNonQuery();
                return true;

            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        /// <summary>
        ///Method for edit user status by user id 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>retuen boolean result </returns>
        public bool EditUserStatus(string userId, bool userStatus)
        {
            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();
            SqlCommand sqlcom = new SqlCommand();
            sqlcon.ConnectionString = connectionstring;
            try
            {
                sqlcon.Open();
                sqlcom.Connection = sqlcon;
                //Put the sql stored procedure ........
                sqlcom.CommandText = "[usp_UpdateUserStatus]";
                //add parametre
                sqlcom.Parameters.AddWithValue("@userId", new Guid(userId));
                sqlcom.Parameters.AddWithValue("@IsActive", userStatus);
                sqlcom.CommandType = CommandType.StoredProcedure;
                sqlcom.ExecuteNonQuery();
                return true;

            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        /// <summary>
        /// user buy the search credit from here
        /// </summary>
        /// <param name="token">stripe token</param>
        /// <param name="amount">amount pay to buy search credits</param>
        public PaymentOutputModel MakePayment(UserTransactionModel objUserTransactionModel)
        {
            PaymentOutputModel objPaymentOutputModel = new PaymentOutputModel();

            if (objUserTransactionModel.PromoCode != "")
            {
                var promoCodeDetail = GetPromoCodeDetail(objUserTransactionModel.PromoCode, objUserTransactionModel.UserId);
                objUserTransactionModel.PromocodeId = promoCodeDetail.PromoCodeId;
                objUserTransactionModel.Amount = promoCodeDetail.Amount * 100;
                objUserTransactionModel.Limit = promoCodeDetail.Limit;

            }
            StripePayment oStripePayment = new StripePayment();
            Stripe.StripeCustomer myCustomer = new Stripe.StripeCustomer();

            string stripeCustomerId = string.Empty;
            //need to check is customer exists in stripe// get customer id from registration table//if blank then need to create customer and pass
            //create customer and makepayment

            var userDetails = GetUserDetail(string.Empty, new Guid(objUserTransactionModel.UserId));
            if (userDetails != null && userDetails.StripeCustomerId != "Null")
            {
                stripeCustomerId = userDetails.StripeCustomerId;
            }

            if (stripeCustomerId == "Null" || string.IsNullOrEmpty(stripeCustomerId))
            {
                //create customer method 
                myCustomer = oStripePayment.CreateCutomer(userDetails.Email, objUserTransactionModel.Token);
                stripeCustomerId = myCustomer.Id;

            }
            objUserTransactionModel.StripeCustomerId = stripeCustomerId;


            var chargePayment = oStripePayment.MakePayement(objUserTransactionModel);
            if (chargePayment != null)
            {
                if (chargePayment.Id != null)
                {
                    var connectionstring = SqlHelper.GetConnectionString();
                    SqlConnection sqlcon = new SqlConnection();
                    SqlCommand sqlcom = new SqlCommand();
                    sqlcon.ConnectionString = connectionstring;
                    try
                    {
                        sqlcon.Open();
                        sqlcom.Connection = sqlcon;
                        //Put the sql stored procedure ........to save the user transaction
                        sqlcom.CommandText = "[sp_SaveUserTransaction]";
                        //add parametre
                        sqlcom.Parameters.AddWithValue("@userId", new Guid(objUserTransactionModel.UserId));
                        sqlcom.Parameters.AddWithValue("@TransactionId", chargePayment.Id);

                        sqlcom.Parameters.AddWithValue("@Amount", (objUserTransactionModel.Amount / 100));

                        sqlcom.Parameters.AddWithValue("@Credits", objUserTransactionModel.Credits);
                        sqlcom.Parameters.AddWithValue("@IsActive", true);
                        sqlcom.Parameters.AddWithValue("@CreatDate", DateTime.Now);
                        sqlcom.Parameters.AddWithValue("@EditedDate", DateTime.Now);
                        sqlcom.Parameters.AddWithValue("@PromocodeId", objUserTransactionModel.PromocodeId);
                        sqlcom.Parameters.AddWithValue("@EditedBy", new Guid(objUserTransactionModel.UserId));
                        sqlcom.Parameters.AddWithValue("@CreateBy", new Guid(objUserTransactionModel.UserId));
                        sqlcom.Parameters.AddWithValue("@IsSubscriptionAgreement", true);
                        sqlcom.Parameters.AddWithValue("@subscriptionDate", DateTime.Now);
                        sqlcom.Parameters.AddWithValue("@stripeCustomerId", objUserTransactionModel.StripeCustomerId);

                        sqlcom.CommandType = CommandType.StoredProcedure;
                        sqlcom.ExecuteNonQuery();
                        //retrive the data available in usercredit table or not

                        SqlCommand newSqlcom = new SqlCommand();
                        newSqlcom.Connection = sqlcon;
                        var userCredits = GetUserCredits(objUserTransactionModel.UserId);
                        objUserTransactionModel.Credits = objUserTransactionModel.Credits + userCredits.credits;
                        //Put the sql stored procedure ........to save the user credits
                        newSqlcom.CommandText = "[sp_CreateOrUpdateUserCredits]";
                        //add parametre
                        newSqlcom.Parameters.AddWithValue("@id", userCredits.Id);
                        newSqlcom.Parameters.AddWithValue("@userId", new Guid(objUserTransactionModel.UserId));
                        newSqlcom.Parameters.AddWithValue("@Credits", objUserTransactionModel.Credits);
                        newSqlcom.Parameters.AddWithValue("@IsActive", true);
                        newSqlcom.Parameters.AddWithValue("@CreatDate", DateTime.Now);
                        newSqlcom.Parameters.AddWithValue("@EditedDate", DateTime.Now);
                        newSqlcom.Parameters.AddWithValue("@EditedBy", new Guid(objUserTransactionModel.UserId));
                        newSqlcom.Parameters.AddWithValue("@CreateBy", new Guid(objUserTransactionModel.UserId));
                        newSqlcom.CommandType = CommandType.StoredProcedure;
                        newSqlcom.ExecuteNonQuery();
                        if (objUserTransactionModel.PromoCode != "")
                        {
                            var limit = objUserTransactionModel.Limit - 1;
                            UpdatePromoLimit(limit, objUserTransactionModel.PromocodeId);
                        }
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    finally
                    {
                        sqlcon.Close();
                    }
                }
                objPaymentOutputModel.Credits = objUserTransactionModel.Credits;
                objPaymentOutputModel.PaymentStatus = chargePayment.Status;
                objPaymentOutputModel.TransactionId = chargePayment.Id;
            }
            else
            {
                objPaymentOutputModel.Credits = 0;
                objPaymentOutputModel.PaymentStatus = "Sorry something went wrrong";
                objPaymentOutputModel.TransactionId = "";
            }
            objPaymentOutputModel.UserEmailid = userDetails.Email;
            return objPaymentOutputModel;
        }
        /// <summary>
        /// Method to get the User credits
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns></returns>
        public UserCreditsModel GetUserCredits(string userId)
        {
            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();
            SqlCommand sqlcom = new SqlCommand();
            sqlcon.ConnectionString = connectionstring;
            UserCreditsModel objUserCreditsModel = new UserCreditsModel();
            try
            {
                sqlcon.Open();
                sqlcom.Connection = sqlcon;
                SqlCommand cmd = new SqlCommand("select * from UserCredits where UserId=@UserId", sqlcon);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@UserId", new Guid(userId));

                SqlDataReader Newdr = cmd.ExecuteReader();
                DataTable objDt = new DataTable();
                //objDt=Newdr.
                if (Newdr.HasRows)
                {
                    while (Newdr.Read())
                    {
                        var x = Newdr.GetSqlValue(0).ToString();
                        objUserCreditsModel.Id = Convert.ToInt32(Newdr.GetSqlValue(0).ToString());
                        objUserCreditsModel.credits = Convert.ToInt16(Newdr.GetSqlValue(2).ToString());
                    }
                }
            }
            catch (Exception err)
            {

            }
            finally
            {
                sqlcon.Close();
            }

            return objUserCreditsModel;
        }
        /// <summary>
        /// Check promocode whether is valid or not
        /// </summary>
        /// <param name="promocode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public PromoCodeModel CheckPromoCode(string promocode, string userId)
        {
            var promoCodeDetail = GetPromoCodeDetail(promocode, userId);
            return promoCodeDetail;
        }
        /// <summary>
        /// Get PromoCode Detail
        /// </summary>
        /// <param name="promocode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public PromoCodeModel GetPromoCodeDetail(string promocode, string userId)
        {

            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();
            SqlCommand sqlcom = new SqlCommand();
            sqlcon.ConnectionString = connectionstring;
            sqlcon.Open();
            sqlcom.Connection = sqlcon;
            SqlCommand cmd = new SqlCommand("select * from vwPromoCode_PromoCodeUser where Code=@PromoCode and UserId=@UserId and RemainingRedeemLimit!=0", sqlcon);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@PromoCode", promocode);
            cmd.Parameters.AddWithValue("@UserId", new Guid(userId));

            SqlDataReader Newdr = cmd.ExecuteReader();
            DataTable objDt = new DataTable();
            PromoCodeModel objPromoCodeModel = new PromoCodeModel();

            if (Newdr.HasRows)
            {
                while (Newdr.Read())
                {
                    var x = Newdr.GetSqlValue(5).ToString();
                    objPromoCodeModel.PromoCodeId = Convert.ToInt32(Newdr.GetSqlValue(0).ToString());
                    objPromoCodeModel.Amount = Convert.ToDecimal(Newdr.GetSqlValue(5).ToString());
                    objPromoCodeModel.Limit = Convert.ToInt16(Newdr.GetSqlValue(7).ToString());
                }

            }
            return objPromoCodeModel;
        }
        /// <summary>
        /// Update Promo code Limit according to user.
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="promoCodeUserId"></param>
        public void UpdatePromoLimit(int limit, long promoCodeUserId)
        {
            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();

            sqlcon.ConnectionString = connectionstring;
            sqlcon.Open();
            string sql = "UPDATE PromoCodeUser SET RemainingRedeemLimit='" + limit + "'where UserPromoId=" + promoCodeUserId;
            SqlCommand sqlcom = new SqlCommand(sql, sqlcon);
            //OleDbCommand cmd = new OleDbCommand(sql, cn);
            sqlcom.ExecuteNonQuery();

        }

        /// <summary>
        /// method to return user credits 
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetUserCreditsById(string userid)
        {
            var creditModal = GetUserCredits(userid);
            int userCredits = 0;
            if (creditModal != null)
            {
                userCredits = creditModal.credits;
            }
            return userCredits;
        }
        /// <summary>
        /// Method 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserTransactionModel GetLastTransactionDetail(string userId)
        {
            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();
            SqlCommand sqlcom = new SqlCommand();
            sqlcon.ConnectionString = connectionstring;
            sqlcon.Open();
            sqlcom.Connection = sqlcon;
            SqlCommand cmd = new SqlCommand("select top 1 Amount,Credit from [Transaction] where UserId=@UserId Order By Id desc", sqlcon);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@UserId", new Guid(userId));

            SqlDataReader Newdr = cmd.ExecuteReader();
            UserTransactionModel objUserTransactionModel = new UserTransactionModel();

            if (Newdr.HasRows)
            {
                decimal amt = 0;
                while (Newdr.Read())
                {
                    objUserTransactionModel.Credits = Convert.ToInt16(Newdr.GetSqlValue(1).ToString());
                    if (objUserTransactionModel.Credits == 1)
                    {
                        //Single credit
                        //objUserTransactionModel.Amount = Convert.ToDecimal(ConfigurationManager.AppSettings["SingleSearchCost"]) ;
                        objUserTransactionModel.Amount = Convert.ToInt32(Convert.ToDecimal(ConfigurationManager.AppSettings["SingleSearchCost"]) * 100);
                    }
                    else
                    {
                        amt = Convert.ToDecimal(Newdr.GetSqlValue(0).ToString());
                        objUserTransactionModel.Amount = Convert.ToInt32(amt * 100);
                    }
                    objUserTransactionModel.PromoCode = string.Empty;
                    objUserTransactionModel.UserId = userId;
                }
            }
            return objUserTransactionModel;
            //Amount * 100
        }
        /// <summary>
        /// Method to save promocode into
        /// </summary>
        /// <param name="objPromoCode">Promo Code Model </param>
        public void SavePromoCode(PromoCodeModel objPromoCode)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "usp_SavePromoCode";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@title", objPromoCode.Title);
                cmd.Parameters.AddWithValue("@promocode", objPromoCode.PromoCode);
                cmd.Parameters.AddWithValue("@price", objPromoCode.Price);
                cmd.Parameters.AddWithValue("@redeemlimit", objPromoCode.Redeemlimit);
                cmd.ExecuteNonQuery();
            }
            catch (Exception exce)
            {
            }
            finally { con.Close(); }
        }

        /// <summary>
        /// Methode to get promocode details by Id 
        /// </summary>
        /// <param name="promocodeId">Promo code Id</param>
        /// <returns></returns>
        public PromoCodeModel GetPromoCodeDetailsById(int promocodeId)
        {
            string query = "Select Title,Code,Price,RedeemLimit from Promocode";
            DataTable dtPromoCode = SqlHelper.FillDataTable(query);

            //SqlCommand cmd = new SqlCommand("Select statusFlag,LatestFileImportDate from AdminSettings", sqlcon);

            PromoCodeModel oPromoCodeModel = new PromoCodeModel();
            if (dtPromoCode != null && dtPromoCode.Rows.Count > 0)
            {
                oPromoCodeModel.Title = dtPromoCode.Rows[0]["Title"].ToString();
                oPromoCodeModel.PromoCode = dtPromoCode.Rows[0]["Code"].ToString();
                oPromoCodeModel.Price =Convert.ToDecimal(dtPromoCode.Rows[0]["Price"].ToString());
                oPromoCodeModel.Redeemlimit = Convert.ToInt32(dtPromoCode.Rows[0]["RedeemLimit"].ToString());
            }
            return oPromoCodeModel;
        }
    }

}
