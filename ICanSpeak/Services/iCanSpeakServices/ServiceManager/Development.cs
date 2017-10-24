using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace iCanSpeakServices.ServiceManager
{
    public class Development
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        public Stream EncryptId(Stream objStream)
        {
            try
            {
                VocabularyWord vocabularyWord = new VocabularyWord();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                var parameters = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                string id = Service.Encrypt(parameters["id"]);

                string result = "{\"id\":\""+id+"\"}";

                return new MemoryStream(Encoding.UTF8.GetBytes(result));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetVocabWordBySubCategoryId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }

        }

    }
}