using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BorgCivil.MVCWeb.App_Start
{
    public class HelperClass
    {
       // JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;


        //public string callservice(string jsonstring, string methodename)
        //{
        //    string result = string.Empty;
        //    try
        //    {
        //        var data = javaScriptSerializer.DeserializeObject(jsonstring);
        //        //request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/" + methodename + "");
        //        request = (HttpWebRequest)WebRequest.Create("http://localhost:64479/api/" + methodename + "");//for local testing
        //                                                                                                      //  request = (HttpWebRequest)WebRequest.Create("http://localhost:1311/Service.svc/" + methodename + "");

        //        string sb = JsonConvert.SerializeObject(data);
        //        request.Method = "POST";
        //        request.ContentType = "application/json";
        //        Byte[] bt = Encoding.UTF8.GetBytes(sb);
        //        Stream st = request.GetRequestStream();
        //        st.Write(bt, 0, bt.Length);
        //        st.Close();
        //        using (response = request.GetResponse() as HttpWebResponse)
        //        {
        //            if (response.StatusCode != HttpStatusCode.OK)
        //            {
        //                var message = response.StatusCode.ToString();
        //                return message;
        //                throw new Exception(String.Format(
        //                    "Server error (HTTP {0}: {1}).", response.StatusCode,
        //                    response.StatusDescription));
        //            }
        //            Stream responseStream = response.GetResponseStream();
        //            using (StreamReader sr = new StreamReader(responseStream))
        //            {
        //                result = sr.ReadToEnd();
        //                return result;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.ToString();
        //    }
        //}
    }
}