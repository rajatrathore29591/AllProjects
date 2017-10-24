using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace iCanSpeakServices.ServiceManager
{
    public class Messages
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataSet ds = new DataSet("Result");
        DataTable Table = new DataTable();

        public Stream GetMessageByUserId(Stream objStream)
        {
            User objUser = new User();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();

            try
            {
                //requestString = "{\"userId\":\"8\"}";
                var userProfile = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                int userid = Convert.ToInt32(Service.Decrypt(userProfile["UserId"]));
                var query =    (from message in icanSpeakContext.Messages
                               join user in icanSpeakContext.Users on message.SenderId equals user.UserId
                               join reciever in icanSpeakContext.Users on message.RecieverId equals reciever.UserId
                               where message.RecieverId == userid
                               select new 
                                         {
                                         Username = textInfo.ToTitleCase(user.Username),
                                         MessageId = Service.Encrypt(message.MessageId.ToString()),
                                         message.IsRead,
                                         reciever.BatchCount,
                                         reciever.Messages,
                                         message.Subject,
                                         Date = message.CreatedDate.ToShortDateString()
                                         }).ToList();


                if (query.Count>0)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(query); ;
                    dt.TableName = "UserMessages";
                    ds.Tables.Add(Table);
                    ds.Tables.Add(dt);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
                else
                {
                    DataTable Table = Service.Message("No Data", "0");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetMessageDetailByMessageId");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }

        public Stream SentMessageByUserId(Stream objStream)
        {
            User objUser = new User();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();

            try
            {
                //requestString = "{\"userId\":\"8\"}";
                var userProfile = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                int userid = Convert.ToInt32(Service.Decrypt(userProfile["UserId"]));
                var query = (from message in icanSpeakContext.Messages
                             join user in icanSpeakContext.Users on message.SenderId equals user.UserId
                             join reciever in icanSpeakContext.Users on message.RecieverId equals reciever.UserId
                             where message.SenderId == userid
                             select new
                             {
                                 Username = textInfo.ToTitleCase(user.Username),
                                 MessageId = Service.Encrypt(message.MessageId.ToString()),
                                 message.IsRead,
                                 reciever.BatchCount,
                                 reciever.Messages,
                                 message.Subject,
                                 Date = message.CreatedDate.ToShortDateString()
                             }).ToList();


                if (query.Count > 0)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(query); ;
                    dt.TableName = "UserMessages";
                    ds.Tables.Add(Table);
                    ds.Tables.Add(dt);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
                else
                {
                    DataTable Table = Service.Message("No Data", "0");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetMessageDetailByMessageId");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }


        public Stream DeleteMessageByMessageId(Stream objStream)
        {

            try
            {
                Message message = new Message();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"userId\":\"8\",\"softDelete\":\"true\"}";
                var requestData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from messeges in icanSpeakContext.Messages
                              where messeges.MessageId == Convert.ToInt32(Service.Decrypt(requestData["MessageId"]))
                              select messeges).FirstOrDefault();


                icanSpeakContext.Messages.DeleteOnSubmit(result);
                icanSpeakContext.SubmitChanges();
                // return javaScriptSerializer.Serialize("User deleted successfully");
                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                ds.Tables.Add(Table);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteMessage");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream DeleteMultipleMessageByMessageId(Stream objStream)
        {

            try
            {
                Message message = new Message();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                int i = 0;
                var requestData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                string temp = requestData["MessageId"];
                string[] messageIds = temp.Split(',');

                List<string> list = new List<string>();
                string temparray;
                for (i = 0; i < messageIds.Length; i++)
                {
                    temparray = Service.Decrypt(messageIds[i]);
                    //string temparray = Service.Decrypt(messageIds[i]);
                    list.Add(temparray);
                   
                 }
                for (int j = 0; j < list.Count; j++)
                {
                    int id = Convert.ToInt32(list[j]);
                    var result = (from messeges in icanSpeakContext.Messages
                                  where messeges.MessageId == id
                                  select messeges).FirstOrDefault();


                    icanSpeakContext.Messages.DeleteOnSubmit(result);
                    icanSpeakContext.SubmitChanges();
                }
                // return javaScriptSerializer.Serialize("User deleted successfully");
                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                ds.Tables.Add(Table);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteMessage");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }
        public Stream GetMessageDetailByMessageId(Stream objStream)
        {
            User objUser = new User();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();

            try
            {
               
                var data = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                int messageid = Convert.ToInt32(Service.Decrypt(data["MessageId"]));

                var query = (from message in icanSpeakContext.Messages
                             join user in icanSpeakContext.Users on message.SenderId equals user.UserId
                             join reciever in icanSpeakContext.Users on message.RecieverId equals reciever.UserId
                             where message.MessageId == messageid
                             select new
                             {
                                 MessageId = Service.Encrypt(message.MessageId.ToString()),
                                 message.Subject,
                                 ProfilePicture = Service.GetUrl() + "ProfilePictures/" + user.ProfilePicture,
                                 Username = textInfo.ToTitleCase(user.Username),
                                 user.Email,
                                 SenderId = Service.Encrypt(user.UserId.ToString()),
                                 Date = message.CreatedDate.ToShortDateString(),
                                 DetailMessage = message.Message1,
                                 message.IsRead,
                                 reciever.BatchCount,
                                 reciever.Messages
                             }).ToList();


                if (query.Count > 0)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(query); ;
                    dt.TableName = "UserMessages";
                    ds.Tables.Add(Table);
                    ds.Tables.Add(dt);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
                else
                {
                    DataTable Table = Service.Message("No Data", "0");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetMessageDetailByMessageId");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }

        public Stream SendMessage(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String responseString = reader.ReadToEnd();
            var parameters = javaScriptSerializer.Deserialize<Dictionary<string, string>>(responseString);

            DataSet ds = new DataSet();
            try
            {

                  Message objMessage = new Message();
                  objMessage.SenderId = Convert.ToInt32(Service.Decrypt(parameters["senderid"]));
                  objMessage.RecieverId = Convert.ToInt32(Service.Decrypt(parameters["recieverid"]));
                  objMessage.Subject = parameters["subject"];
                  objMessage.Message1 = parameters["messagebody"];
                  objMessage.CreatedDate = System.DateTime.Now;
                  icanSpeakContext.Messages.InsertOnSubmit(objMessage);
                  icanSpeakContext.SubmitChanges();

                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ResultMessage("saved","1") ;
                    ds.Tables.Add(Table);
                    ds.Tables.Add(dt);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "SendMessage");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }
    }
}