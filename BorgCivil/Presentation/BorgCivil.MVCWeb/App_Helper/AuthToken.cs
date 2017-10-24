
using BorgCivil.Utils.Models;
using Ipm.Hub.Utilities;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace BorgCivil.MVCWeb.App_Helper
{
    public static class AuthToken
    {
        public static string CONTENT_TYPE = @"application/x-www-form-urlencoded";
        public static string POST_METHOD = "POST";
        public static string GET_METHOD = "GET";
        public static string PUT_METHOD = "PUT";

        public static AccessTokenModel authToken;
        public static string physmodoAccessToken;

        public static AccessTokenModel GetLoingInfo(EmployeeDataModel model)
        {
            var myrul = HttpContext.Current.Request.Url.AbsoluteUri;
            var domain = AppHelper.GetAppSetting("Domain");
            var ProjectName = AppHelper.GetAppSetting("ProjectName");
            var tokenUrl = domain + ProjectName + "/token";
            var userName = model.Email;
            var userPassword = model.Password;
            var request = string.Format("grant_type=password&username={0}&password={1}", userName, userPassword);

            authToken = HttpPost(tokenUrl, request);
            return authToken;
        }


        public static CookieHeaderValue Get()
        {
            var cookie = new CookieHeaderValue("session-id", "12345");
            cookie.Expires = DateTimeOffset.Now.AddDays(1);
            cookie.Domain = HttpContext.Current.Request.Url.AbsoluteUri;
            cookie.Path = "/";

            return cookie;
        }

        public static AccessTokenModel HttpPost(string tokenUrl, string requestDetails)
        {
            AccessTokenModel token = null;
            try
            {
                WebRequest webRequest = WebRequest.Create(tokenUrl);
                webRequest.ContentType = CONTENT_TYPE;
                webRequest.Method = POST_METHOD;
                byte[] bytes = Encoding.ASCII.GetBytes(requestDetails);
                webRequest.ContentLength = bytes.Length;
                using (Stream outputStream = webRequest.GetRequestStream())
                {
                    outputStream.Write(bytes, 0, bytes.Length);
                }
                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    StreamReader newstreamreader = new StreamReader(webResponse.GetResponseStream());
                    string newresponsefromserver = newstreamreader.ReadToEnd();
                    newresponsefromserver = newresponsefromserver.Replace(".expires", "expires").Replace(".issued", "issued");
                    token = Newtonsoft.Json.JsonConvert.DeserializeObject<AccessTokenModel>(newresponsefromserver);// new JavaScriptSerializer().Deserialize<AccessToken>(newresponsefromserver);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                token = null;
            }

            return token;
        }
    }
}