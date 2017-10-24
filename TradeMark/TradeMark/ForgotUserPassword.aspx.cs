using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradeMark.BAL;
using TradeMark.Models;

namespace TradeMark
{
    public partial class forgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            UserModel objUserModel = new UserModel();
            objUserModel.Email = txtemail.Text;
            UserService oUserService = new UserService();
            lblEmailmsg.Text = "";
            string userName = oUserService.GetUserNameByEmail(txtemail.Text);
            try
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    var subject = "Account Recovery Password";

                    StringBuilder objSBEmailBody = new StringBuilder();
                    objSBEmailBody.Append("<div> Hello" + " " + userName + "</div>");
                    objSBEmailBody.Append("We have rececived request for password reset for your account<br>");
                    objSBEmailBody.Append("Your temporary password is");
                    var body = objSBEmailBody.ToString();
                    var to = objUserModel.Email;
                    if (Utility.Utility.SendEmail(subject, body, to))
                    {
                        lblSuccessmsg.Text = "Email is sent to you, Please check your email.";
                        lblFailuremsg.Text = "";
                    }
                }
                else
                {
                    lblSuccessmsg.Text = "";
                    lblFailuremsg.Text = "The email address you provided does not match our records. Please try again.";
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }

        }
    }
}