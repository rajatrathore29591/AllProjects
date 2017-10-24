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
    public class Payment
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        public Stream SavePaymentDetails(Stream objStream)
        {
            try
            {
                PaymentDetail payment = new PaymentDetail();
                 StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                 String requestString = reader.ReadToEnd();
                // requestString = "{\"text\":\"dfdf\",\"imageUrl\":\"img08.png\",\"audioUrl\":\"img05.jpg\",\"sampleSentance\":\"\",\"arabicText\":\"dfdf\"}";
                var paymentData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);


                var paymentResult = (from data in icanSpeakContext.PaymentDetails
                                     where data.TranctionId == Convert.ToInt32(paymentData["tranctionId"])
                                     select data).FirstOrDefault();

                if (paymentResult.TranctionId > 0)
                {
                    var js = JsonConvert.SerializeObject("TranctionId already exists", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    payment.UserId = Convert.ToInt32(paymentData["userId"]);
                    payment.TranctionId = Convert.ToInt32(paymentData["TranctionId"]);
                    payment.CourseId = Convert.ToInt32(paymentData["courseId"]);
                    payment.CoursePrice = Convert.ToInt32(paymentData["coursePrice"]);
                    payment.Discription = paymentData["discription"];
                    payment.PaidAmount = Convert.ToInt32(paymentData["paidAmount"]);
                    payment.PaidBy = paymentData["PaidBy"];
                    payment.PaidDate = DateTime.UtcNow;

                    var js = JsonConvert.SerializeObject("success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));


                }

             
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "SavePaymentDetails");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetAllPayment(Stream objStream)
        {
            try
            {
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
             
                var userResult = (from payment in icanSpeakContext.PaymentDetails
                                  join users in icanSpeakContext.Users on payment.UserId equals users.UserId
                                  join course in icanSpeakContext.Courses on payment.UserId equals course.UserId
                                  select new
                                  {
                                      payment.UserId,
                                      users.Email,
                                      payment.CoursePrice,
                                      payment.Discription,
                                      payment.PaidAmount,
                                      payment.PaidBy,
                                      payment.PaidDate,
                                      payment.PaymentDetailId,
                                      payment.TranctionId
                                  }).ToList();


                var js = JsonConvert.SerializeObject(userResult, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));


            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAllPayment");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }
    }
}