using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.Web.Hosting;
using System.Data;
using System.Reflection;
using System.ServiceModel.Web;
using System.Data.Objects.SqlClient;
using System.Globalization;

namespace iCanSpeakServices.ServiceManager
{
    public class Query
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataSet ds = new DataSet();
        DataTable Table = new DataTable();

        public Stream AddQuery(Stream objStream)
        {
            try
            {
                ContactUs contactUs = new ContactUs();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var contact = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                contactUs.Name = contact["name"];
                contactUs.EmailId = contact["email"];
                contactUs.ContactNo = contact["contactNo"];
                contactUs.Message = contact["message"];
                //contactUs.Subject = contact["subject"];

                icanSpeakContext.ContactUs.InsertOnSubmit(contactUs);
                icanSpeakContext.SubmitChanges();

                var js = JsonConvert.SerializeObject("Request sent", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));

                // return javaScriptSerializer.Serialize("Request sent..");

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "ContactUs");

                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
                //   var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            }
        }

        public Stream DeleteQueryById(Stream objStream)
        {
            try
            {
                ContactUs contactUs = new ContactUs();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                // requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var contact = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from user in icanSpeakContext.ContactUs
                              where user.QueryId == Convert.ToInt32(contact["queryId"])
                              select user).FirstOrDefault();

                if (contact["softDelete"] == "true")
                {
                    result.IsActive = false;
                }
                else
                {
                    icanSpeakContext.ContactUs.DeleteOnSubmit(result);
                }
                icanSpeakContext.SubmitChanges();
                //return javaScriptSerializer.Serialize("Query deleted successfully");

                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Query deleted successfully")));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteQueryById");
                // return javaScriptSerializer.Serialize(ex.Message.ToString());

                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream ReadQuery(Stream objStream)
        {
            try
            {
                ContactUs contactUs = new ContactUs();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                // requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var contact = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var userData = (from status in icanSpeakContext.ContactUs
                                where status.QueryId == Convert.ToInt32(contact["queryId"])
                                select status).FirstOrDefault();
                //userData.IsActive = false;
                userData.IsRead = true;
                icanSpeakContext.SubmitChanges();


                var result = (from data in icanSpeakContext.ContactUs
                              where data.QueryId == Convert.ToInt32(contact["queryId"])
                              select new { data.QueryId, data.Name, data.EmailId, data.ContactNo, data.Subject, data.Message, data.CreatedDate, data.IsRead }).SingleOrDefault();

                var js = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
                // return javaScriptSerializer.Serialize("Success");
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "ReadQuery");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetAllUserQueries(Stream objStream)
        {
            try
            {
                ContactUs contactUs = new ContactUs();

                var userData = (from data in icanSpeakContext.ContactUs
                                select new { data.QueryId, data.Name, data.EmailId, data.ContactNo, Subject = Service.TrimString(data.Subject, 30), Message = Service.TrimString(data.Message, 30), data.CreatedDate, data.IsRead }).OrderBy(c => c.IsRead).ThenByDescending(c => c.CreatedDate).ToList();

                if (userData.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(userData, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No data")));
                }


                // return javaScriptSerializer.Serialize("Success");
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "ReadQuery");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }


        public Stream GetAllUserQuery(Stream objStream)
        {
            try
            {
                ContactUs contactUs = new ContactUs();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                var userData = (from data in icanSpeakContext.ContactUs
                                select new
                                {
                                    data.QueryId,
                                    data.Name,
                                    data.EmailId,
                                    data.ContactNo,
                                    data.Message,
                                    data.IsActive,
                                    data.IsRead,
                                    data.CreatedDate
                                }).ToList();
                if (userData.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(userData, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No Data")));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "ReadQuery");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }

        public Stream AddQuery1(Stream objStream)
        {
            DataSet ds = new DataSet("Result");
            try
            {
                ContactUs contactUs = new ContactUs();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                var contact = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                contactUs.Name = contact["FullName"];
                contactUs.EmailId = contact["EmailId"];
                contactUs.ContactNo = contact["ContactNo"];
                contactUs.Message = contact["Message"];
                contactUs.CreatedDate = System.DateTime.Now;
                contactUs.IsActive = true;
                contactUs.IsRead = false;
                icanSpeakContext.ContactUs.InsertOnSubmit(contactUs);
                icanSpeakContext.SubmitChanges();
                Table = Service.Message("success", "1");
                Table.AcceptChanges();

                var js = JsonConvert.SerializeObject("Request sent", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "ReadQuery");
                //  var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }
    }
}