
using Ipm.Hub.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using TradeSystem.MVCWeb.Models;
using TradeSystem.Utils.Models;

namespace TradeSystem.MVCWeb.App_Helper
{
    public static class AuthToken
    {
        public static string CONTENT_TYPE = @"application/x-www-form-urlencoded";
        public static string POST_METHOD = "POST";
        public static string GET_METHOD = "GET";
        public static string PUT_METHOD = "PUT";

        public static AccessTokenModel authToken;
        public static string physmodoAccessToken;

        //public static bool connectHTTP()
        //{
        //    bool rtnFlag = false;

        //    var tokenUrl = PhysmodoURL + "token";
        //    var userName = "webdev@physmodo.com";
        //    var userPassword = "PhysmodoDev123#";
        //    // var userPassword = "Neetu1234#";  
        //    var request = string.Format("grant_type=password&username={0}&password={1}", HttpUtility.UrlEncode(userName), HttpUtility.UrlEncode(userPassword));
        //    authToken = HttpPost(tokenUrl, request);
        //    if (authToken != null)
        //    {
        //        physmodoAccessToken = authToken.access_token;
        //        //HttpContext.Current.Session["accessToken"] = physmodoAccessToken;
        //        rtnFlag = true;
        //        Console.WriteLine("Sucessful log into physmodo web site with " + userName);
        //    }
        //    else
        //    {
        //        physmodoAccessToken = null;
        //    }
        //    return rtnFlag;
        //}
        public static AccessTokenModel GetLoingInfo(LoginDataModel model)
        {
            var myrul = HttpContext.Current.Request.Url.AbsoluteUri;
            var domain = AppHelper.GetAppSetting("Domain");
            var ProjectName = AppHelper.GetAppSetting("ProjectName");
            //var tokenUrl = myrul.Replace(HttpContext.Current.Request.Url.AbsolutePath, "/") + "token";
            var tokenUrl = domain + ProjectName + "/token";

            var userName = model.Email;
            var userPassword = model.Password;
            //var request = string.Format("grant_type=password&username={0}&password={1}", HttpUtility.UrlEncode(userName), HttpUtility.UrlEncode(userPassword));
            var request = string.Format("grant_type=password&username={0}&password={1}", userName, userPassword);

            authToken = HttpPost(tokenUrl, request);
            return authToken;
        }


        //public static AccessToken GetLoingInfo(LoginViewModel model)
        //{
        //    var myrul = HttpContext.Current.Request.Url.AbsoluteUri;
        //    Console.WriteLine("myrul: " + myrul);
        //    var tokenUrl = myrul.Replace(HttpContext.Current.Request.Url.AbsolutePath, "/TangoBMS/") + "token";
        //    Console.WriteLine("tokenUrl: " + tokenUrl);
        //    var userName = model.Email;
        //    var userPassword = model.Password;
        //    var request = string.Format("grant_type=password&username={0}&password={1}", HttpUtility.UrlEncode(userName), HttpUtility.UrlEncode(Encrypt.GetValue(userPassword)));
        //    authToken = HttpPost(tokenUrl, request);
        //    return authToken;
        //}
        //public static AccessToken HttpPost(string tokenUrl, string requestDetails)
        //{
        //    AccessToken token = null;
        //    try
        //    {
        //        WebRequest webRequest = WebRequest.Create(tokenUrl);
        //        Console.WriteLine("Web Request created.");
        //        webRequest.ContentType = CONTENT_TYPE;
        //        webRequest.Method = POST_METHOD;
        //        byte[] bytes = Encoding.ASCII.GetBytes(requestDetails);
        //        webRequest.ContentLength = bytes.Length;
        //        Console.WriteLine("value of web request: " + webRequest);
        //        using (Stream outputStream = webRequest.GetRequestStream())
        //        {
        //            outputStream.Write(bytes, 0, bytes.Length);
        //        }
        //        using (WebResponse webResponse = webRequest.GetResponse())
        //        {
        //            StreamReader newstreamreader = new StreamReader(webResponse.GetResponseStream());
        //            string newresponsefromserver = newstreamreader.ReadToEnd();
        //            newresponsefromserver = newresponsefromserver.Replace(".expires", "expires").Replace(".issued", "issued");
        //            token = Newtonsoft.Json.JsonConvert.DeserializeObject<AccessToken>(newresponsefromserver);// new JavaScriptSerializer().Deserialize<AccessToken>(newresponsefromserver);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        token = null;
        //    }

        //    return token;
        //}

        public static CookieHeaderValue Get()
        {
            //var resp = new HttpResponseMessage();
            //var resp = CookieHeaderValue;
            var cookie = new CookieHeaderValue("session-id", "12345");
            cookie.Expires = DateTimeOffset.Now.AddDays(1);
            cookie.Domain = HttpContext.Current.Request.Url.AbsoluteUri;
            cookie.Path = "/";

            //resp.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            //resp = cookie;
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