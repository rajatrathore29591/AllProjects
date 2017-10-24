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
    public partial class edituser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserService oUserService = new UserService();
                UserModel objUserModel = new UserModel();
                string UserID = Request.QueryString["ID"];
                //string UserID = Request["UserID"];
                if (UserID != null)
                {
                    //get user details in model
                    objUserModel = oUserService.GetUserDetail("", UserID);

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
                        if (objUserModel.IsActive == "True")
                            chkStatus.Checked = true;
                        else
                            chkStatus.Checked = false;
                       // txtStatus.Text = objUserModel.IsActive== "True" ? "Active":"De-Active";
                        txtRegisteredDate.Text = objUserModel.RegisteredDate.ToString("MM/dd/yyyy");                      
                    }
                }
            }
        }

        protected void btnEditUser_Click(object sender, EventArgs e)
        {
            UserModel objUserModel = new UserModel();
            UserService oUserService = new UserService();
            string UserID = Request.QueryString["ID"];
            try
            {
                if (UserID != null)
                {

                    objUserModel.UserId = UserID;
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
                if (chkStatus.Checked == true)
                    objUserModel.IsActive = "True";
                else
                    objUserModel.IsActive = "False";
                oUserService.AddUser(objUserModel);
                lblSuccessmsg.Text = "User is updated successfully";
            }
            catch (Exception ex)
            {
            }

        }
    }
}