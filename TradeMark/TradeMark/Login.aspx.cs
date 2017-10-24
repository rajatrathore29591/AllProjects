using System;
using TradeMark.BAL;
using TradeMark.Models;
using System.Configuration;
using System.Data;
using ASPSnippets.TwitterAPI;
using ASPSnippets.FaceBookAPI;
using System.Web.Script.Serialization;
using System.Web;
using TradeMark.Utility;

namespace TradeMark
{
    public partial class Login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["logout"] != null)
                {
                    if (Request.Cookies["CheckboxCheck"] != null)
                    {
                        Response.Cookies["CheckboxCheck"].Expires = DateTime.Now.AddDays(-1);
                    }

                    if (Request.Cookies["CheckboxCheck"] != null)
                    {
                        Response.Cookies["CheckboxCheck"].Expires = DateTime.Now.AddDays(-1);
                    }

                    if (Request.Cookies["visited"] != null)
                    {
                        Response.Cookies["visited"].Expires = DateTime.Now.AddDays(-1);
                    }

                    Session.Clear();
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    //Connect with twitter API
                    GetTwitterRegisteredDetail();
                    //Connect with facebook API
                    GetFBRegisteredDetail();
                }

            }


        }
        /// <summary>
        /// used to login user on button click
        /// Redirect the valid user to the home page
        /// else user stay to the login page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLogin_Click(object sender, EventArgs e)

        {
            if (chkRememberMe.Checked)
            {
                Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(30);
                Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);
                Response.Cookies["UserName"].Value = txtEmail.Text.Trim();
                Response.Cookies["Password"].Value = txtPassword.Text.Trim();
            }

            EncryptDecrypt objEncryptDecrypt = new EncryptDecrypt();
            UserModel objUserModel = new UserModel();
            objUserModel.Email = txtEmail.Text.Trim();
            objUserModel.Password = objEncryptDecrypt.Encrypt(txtPassword.Text);
            UserService oUserService = new UserService();
            var userDetail = oUserService.UserAuthentication(objUserModel);
            //Condition check if user is active then this will able to login or not
            if(userDetail.UserId == null)
            {
                lblMessage.Text = "The email or password you entered is incorrect."; divMsg.Visible = true;
            }
            else if (userDetail!=null && Convert.ToBoolean(userDetail.IsActive) == true) { 
                if (userDetail.UserId != null)
                {
                    Session["UserId"] = userDetail.UserId;
                    //Session["UserName"] = userDetail.FirstName + " " + userDetail.LastName;
                    Session["UserName"] = userDetail.FirstName;
                    Session["IsAdmin"] = userDetail.IsAdmin;
                    Response.Redirect("Search.aspx");
                }
                else
                { lblMessage.Text = "The email or password you entered is incorrect."; divMsg.Visible = true; }
            }
            else
            { lblMessage.Text = "Your account is De-Active please contact to admin."; divMsg.Visible = true; }
        }

        /// <summary>
        /// Twitter login event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTwitterLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TwitterConnect.IsAuthorized)
                {
                    TwitterConnect twitter = new TwitterConnect();
                    twitter.Authorize(Request.Url.AbsoluteUri.Split('?')[0]);
                }
            }
            catch (Exception ex) { throw ex; }
        }
        protected void btnFacebookLogin_Click(object sender, EventArgs e)
        {
            FaceBookConnect.Authorize("user_photos,email", Request.Url.AbsoluteUri.Split('?')[0]);
        }

        /// <summary>
        /// Get details of user after performing Login with Twitter
        /// </summary>

        protected void GetTwitterRegisteredDetail()
        {
            //Get API key and secret key of Twitter
            TwitterConnect.API_Key = ConfigurationManager.AppSettings["TwitterappId"];
            TwitterConnect.API_Secret = ConfigurationManager.AppSettings["TwitterappSecret"];
            var userIdByTWitId = 0;
            try
            {
                if (!IsPostBack)
                {
                    TwitterConnect twitter = new TwitterConnect();
                    if (TwitterConnect.IsAuthorized)
                    {
                        //LoggedIn User Twitter Profile Details
                        DataTable dtTwit = twitter.FetchProfile();
                        //Get value in user model from twitter account
                        UserModel objUserModel = new UserModel();
                        objUserModel.Email = "";
                        objUserModel.Password = "";
                        objUserModel.FirstName = dtTwit.Rows[0]["name"].ToString();
                        objUserModel.ProfilePic = "";
                        objUserModel.MediaProvider = "Tw";
                        objUserModel.SocialMediaUserId = dtTwit.Rows[0]["Id"].ToString();

                        objUserModel.ContactNo = "";
                        objUserModel.UserName = dtTwit.Rows[0]["screen_name"].ToString();
                        UserService oUserService = new UserService();
                        oUserService.UserLogin(objUserModel);
                        // Check if user is registered or not
                        userIdByTWitId = oUserService.GetUserId("", dtTwit.Rows[0]["Id"].ToString());
                        var userNameByTwitName = dtTwit.Rows[0]["name"].ToString();
                        if (userIdByTWitId > 0 && userNameByTwitName != null)
                        {
                            Session["UserId"] = userIdByTWitId;
                            Session["UserName"] = userNameByTwitName;
                            Response.Redirect("Search.aspx");
                        }
                        else
                        { lblMessage.Text = "The email or password you entered is incorrect."; }


                        btnLogin.Enabled = false;
                    }
                    if (TwitterConnect.IsDenied)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "key", "alert('User has denied access.')", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Get details of user after performing Login with FB
        /// </summary>
        protected void GetFBRegisteredDetail()
        {
            FaceBookConnect.API_Key = ConfigurationManager.AppSettings["FBappId"];
            FaceBookConnect.API_Secret = ConfigurationManager.AppSettings["FBappSecret"];
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["error"] == "access_denied")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('User has denied access.')", true);
                        return;
                    }

                    string code = Request.QueryString["code"];
                    if (!string.IsNullOrEmpty(code))
                    {

                        string data = FaceBookConnect.Fetch(code, "me");
                        FacebookModel faceBookUser = new JavaScriptSerializer().Deserialize<FacebookModel>(data);
                        UserModel objUserModel = new UserModel();
                        objUserModel.Email = "";
                        objUserModel.Password = "";
                        objUserModel.FirstName = faceBookUser.Name;
                        objUserModel.ProfilePic = "";
                        objUserModel.MediaProvider = "Fb";
                        objUserModel.SocialMediaUserId = faceBookUser.Id;
                        objUserModel.ContactNo = "";
                        objUserModel.UserName = "";
                        UserService oUserService = new UserService();
                        oUserService.UserLogin(objUserModel);
                        // user id into session
                        var userIdByFBId = oUserService.GetUserId("", faceBookUser.Id);
                        if (userIdByFBId > 0 && faceBookUser != null)
                        {
                            Session["UserId"] = userIdByFBId.ToString();
                            Session["UserName"] = faceBookUser.Name;
                            Response.Redirect("Search.aspx");
                        }
                        else
                        { lblMessage.Text = "The email or password you entered is incorrect."; }
                        btnLogin.Enabled = false;
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

    }
}