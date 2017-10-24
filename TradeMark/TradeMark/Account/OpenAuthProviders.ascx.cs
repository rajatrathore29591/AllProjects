using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Text;

namespace TradeMark.Account
{
    public partial class OpenAuthProviders : System.Web.UI.UserControl
    {
        string _accessToken;
        string _appId = "967388133384678";
        string _appSecret = "987a13a265b7b5e27851adf01030de6e";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                var provider = Request.Form["provider"];
                if (provider == null)
                {
                    return;
                }
                // Request a redirect to the external login provider
                string redirectUrl = ResolveUrl(String.Format(CultureInfo.InvariantCulture, "~/Account/RegisterExternalLogin?{0}={1}&returnUrl={2}", IdentityHelper.ProviderNameKey, provider, ReturnUrl));
                var properties = new AuthenticationProperties() { RedirectUri = redirectUrl };
                GetAccessToken();
                // Add xsrf verification when linking accounts
                if (Context.User.Identity.IsAuthenticated)
                {
                    properties.Dictionary[IdentityHelper.XsrfKey] = Context.User.Identity.GetUserId();
                }
                Context.GetOwinContext().Authentication.Challenge(properties, provider);
                Response.StatusCode = 401;
                Response.End();
            }
        }

        public string ReturnUrl { get; set; }

        public IEnumerable<string> GetProviderNames()
        {
            return Context.GetOwinContext().Authentication.GetExternalAuthenticationTypes().Select(t => t.AuthenticationType);
        }

        public string GetAccessToken()
        {
            var facebookCookie = HttpContext.Current.Request.Cookies["fbsr_" + _appId];
            if (facebookCookie != null && facebookCookie.Value != null)
            {
                string jsoncode = System.Text.ASCIIEncoding.ASCII.GetString(FromBase64ForUrlString(facebookCookie.Value.Split(new char[] { '.' })[1]));
                var tokenParams = HttpUtility.ParseQueryString(GetAccessToken((string)JObject.Parse(jsoncode)["code"]));
                _accessToken = tokenParams["access_token"];
                return _accessToken;
            }
            else
                return null;

            // return DBLoginCall(username, passwordHash, cookieToken, cookieTokenExpires, args.LoginType == LoginType.Logout, null);
        }

        private string GetAccessToken(string code)
        {
            //Notice the empty redirect_uri! And the replace on the code we get from the cookie.
            string url = string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}", _appId, "", _appSecret, code.Replace("\"", ""));

            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as System.Net.HttpWebRequest;
            System.Net.HttpWebResponse response = null;

            try
            {
                using (response = request.GetResponse() as System.Net.HttpWebResponse)
                {
                    System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream());

                    string retVal = reader.ReadToEnd();
                    return retVal;
                }
            }
            catch
            {
                return null;
            }
        }

        private byte[] FromBase64ForUrlString(string base64ForUrlInput)
        {
            int padChars = (base64ForUrlInput.Length % 4) == 0 ? 0 : (4 - (base64ForUrlInput.Length % 4));
            StringBuilder result = new StringBuilder(base64ForUrlInput, base64ForUrlInput.Length + padChars);
            result.Append(String.Empty.PadRight(padChars, '='));
            result.Replace('-', '+');
            result.Replace('_', '/');
            return Convert.FromBase64String(result.ToString());
        }
    }
}