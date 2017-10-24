using System;
using TradeMark.BAL;
using TradeMark.DAL;
using TradeMark.Models;

namespace TradeMark
{
    public partial class AdminSettings : System.Web.UI.Page
    {
        public bool importInporocess = false;
        public string latestFileImportDate = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Convert.ToBoolean(Session["IsAdmin"]))
            {
                Response.Redirect("Search.aspx");
            }

            if (!IsPostBack)
            {
                //Call method to get the current status of importing and last imported xml file date
                
                UserService oUserService = new UserService();
                AdminSettingModel adminSetting = oUserService.GetAdminSetting();
                if (adminSetting != null)
                {
                    importInporocess = adminSetting.StatusFlag;
                    chkIsProgress.Checked = importInporocess;
                    txtEmail.Text = adminSetting.AdminEmail;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var isProgress = chkIsProgress.Checked;
            var email = txtEmail.Text.Trim();
            UserProvider oUserProvider = new UserProvider();
            var result = string.Empty;
            if (isProgress)
                
             result =oUserProvider.SaveAdminSetting(1, email);
            else
                result = oUserProvider.SaveAdminSetting(0, email);

            litScript.Text = "<script>ShowHide();</script>";

        }
    }
}