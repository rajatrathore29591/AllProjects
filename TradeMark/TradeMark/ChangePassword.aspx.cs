using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradeMark.BAL;
using TradeMark.Utility;

namespace TradeMark
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            EncryptDecrypt objEncryptDecrypt = new EncryptDecrypt();
            UserService objUserService = new UserService();
            string passwordAvailable = objUserService.PasswordAvailable(Session["UserId"].ToString());
            if(!String.IsNullOrEmpty(passwordAvailable))
            {
                lblOldPassword.Visible = true;
                txtOldPassword.Visible = true;
                hdnOldPassword.Value = "true";
                HdnOldpasswordtxt.Value = objEncryptDecrypt.Decrypt(passwordAvailable);
            }

        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            string newPassword = txtNewPassword.Text;
            if (!String.IsNullOrEmpty(newPassword))
            {
                string userId = Session["UserId"].ToString();
                UserService objUserService = new UserService();
                EncryptDecrypt objEncryptDecrypt = new EncryptDecrypt();
                bool oldPassword = objUserService.PasswordCheck(Session["UserId"].ToString(), objEncryptDecrypt.Encrypt(txtOldPassword.Text));
                if (oldPassword == true)
                {
                   bool password = objUserService.SetNewPassword(userId, objEncryptDecrypt.Encrypt(newPassword));
                   
                    if (password == true)
                    {
                        lblSuccess.Text = "Password has been changed successfully";
                        litScript.Text = "<script>showSuccessmsg();</script>";
                    }
                    else
                        lblFailure.Text = "Sorry something went wrong ,please try again";
                }
                else
                    lblFailure.Text = "your old password is not correct";
            }
        }
    }
}