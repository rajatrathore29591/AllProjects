using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace iCanSpeakServices.ServiceManager
{
    public class Submmision
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        public Stream GetAllSubmmision(Stream objStream)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);

                var grammerAssessmentResult = (from grammerAssessment in icanSpeakContext.GrammerAssessmentScores
                                               select new
                                               {
                                                   grammerAssessment.User.Email,
                                                   grammerAssessment.Score.Value,
                                                   grammerAssessment.AssessmentNo,
                                                   grammerAssessment.GrammerUnit.CorrectAnswer,
                                                   grammerAssessment.CreateDate

                                               });

                var vocabAssessmentResult = (from vocabAssessment in icanSpeakContext.VocabAssessmentScores
                                             select new
                                             {
                                                 vocabAssessment.User.Email,
                                                 vocabAssessment.Score.Value,
                                                 vocabAssessment.AssessmentNo,

                                                 vocabAssessment.CreateDate

                                             });
                dic.Add("Vocab", vocabAssessmentResult);
                dic.Add("Grammer", grammerAssessmentResult);

                var js = JsonConvert.SerializeObject(dic, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAllSubmmision");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }
    }
}