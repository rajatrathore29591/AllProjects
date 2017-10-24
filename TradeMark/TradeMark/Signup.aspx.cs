using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradeMark.BAL;
using TradeMark.Models;
using TradeMark.Utility;

namespace TradeMark
{
    public partial class Signup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// on submiting the signup form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSignup_Click(object sender, EventArgs e)
        {
            UserModel objUserModel = new UserModel();

            // insert the filled data to the model class
            EncryptDecrypt objEncryptDecrypt = new EncryptDecrypt();
            objUserModel.FirstName = txtFirstName.Text.Trim();
            objUserModel.Email =txtEmail.Text.Trim();
            objUserModel.UserName ="" ;
            objUserModel.ContactNo = txtContactNo.Text.Trim();
            objUserModel.Password = objEncryptDecrypt.Encrypt(txtPassword.Text);
            objUserModel.MediaProvider = "TM";
            objUserModel.SocialMediaUserId = "";
            objUserModel.LastName = txtLastName.Text.Trim();//Add more field into SignUp
            objUserModel.CompanyName = txtCompanyName.Text.Trim();
            objUserModel.Title = txtTitle.Text.Trim();
            objUserModel.StreetAddress = txtStreetAddress.Text.Trim();
            objUserModel.IsActive = "True";
            // pass model object to the Business access layer
            UserService oUserService = new UserService();
            if (oUserService.AddUser(objUserModel))
            {
                Response.Redirect("SuccessfullRegister.aspx");
            }
            else
            {
                lblMessage.Text = "Your are not register. please try again";
            }
            Response.Redirect("Login.aspx");
        }
        /// <summary>
        /// Method to check email
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string CheckEmail(string emailId)
        {
            UserService oUserService = new UserService();
            //Call method for duplicate email
            bool value = oUserService.CheckEmail(emailId);
            if (value == true)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }
    }

}
