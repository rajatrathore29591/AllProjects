using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iCanSpeakAdminPortal.App_Start
{
    public class HelperClass
    {
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;

        public string callservice(string jsonstring, string methodename)
        {
            string result = string.Empty;
            try
            {
                var data = javaScriptSerializer.DeserializeObject(jsonstring);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/" + methodename + "");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/" + methodename + "");//for local testing
               
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        result = sr.ReadToEnd();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                return result;
            }
        }
    }
}