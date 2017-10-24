using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradeMark.BAL;
using TradeMark.Models;

namespace TradeMark
{
    public partial class EditProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserService oUserService = new UserService();
                UserModel objUserModel = new UserModel();
                if (Session["UserId"] != null)
                {
                    //get user details in model
                    objUserModel = oUserService.GetUserDetail("", Session["UserId"].ToString());

                    if (objUserModel != null)
                    {                        
                        //details fill on text box for profile details
                        txtEFirstName.Text = objUserModel.FirstName;
                        txtELastName.Text = objUserModel.LastName;
                        txtEEmail.Text = objUserModel.Email;
                        txtEContactNo.Text = objUserModel.ContactNo;
                        txtECompanyName.Text = objUserModel.CompanyName;
                        txtETitle.Text = objUserModel.Title;
                        txtEStreetAddress.Text = objUserModel.StreetAddress;
                        txtRegisteredDate.Text = objUserModel.RegisteredDate.ToString("MM/dd/yyyy");
                        //apply read only property of email field 
                        if (!string.IsNullOrEmpty(objUserModel.Email) && objUserModel.Email != "undefined")
                        {
                            txtEEmail.ReadOnly = true;
                        }
                    }
                }
            }
        }
        /// <summary>   
        /// submit the edit data here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditProfile_Click(object sender, EventArgs e)
        {
            UserModel objUserModel = new UserModel();
            UserService oUserService = new UserService();
            try
            {
                if (Session["UserId"] != null)
                {

                    objUserModel.UserId = Session["UserId"].ToString();
                }
                objUserModel.Password = "";

                objUserModel.ContactNo = txtEContactNo.Text.Trim();
                objUserModel.FirstName = txtEFirstName.Text.Trim();
                objUserModel.Email = txtEEmail.Text.Trim();
                objUserModel.UserName = "";
                objUserModel.ProfilePic = "";
                objUserModel.SocialMediaUserId = "";
                objUserModel.MediaProvider = "";
                objUserModel.LastName = txtELastName.Text.Trim();//Add more field into SignUp
                objUserModel.CompanyName = txtECompanyName.Text.Trim();
                objUserModel.Title = txtETitle.Text.Trim();
                objUserModel.StreetAddress = txtEStreetAddress.Text.Trim();
                objUserModel.IsActive = "True";
                oUserService.AddUser(objUserModel);
                lblSuccessmsg.Text = "Profile is updated successfully";
                litScript.Text = "<script>showEditProfileSuccessmsg();</script>";
            }
            catch(Exception ex)
            {
            }

        }
    }
}