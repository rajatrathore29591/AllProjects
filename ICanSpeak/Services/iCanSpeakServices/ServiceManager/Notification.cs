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
    public class Notifications
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataSet ds = new DataSet();
        DataTable Table = new DataTable();

        public Stream SendNotification(Stream objStream)
        {
            User objUser = new User();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                //  requestString = "{\"Email\":\"mahendra@techvalens.com\",\"Password\":\"h\",\"NewPassword\":\"123456\"}";
                var userLogin = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);


                icanSpeakContext.ExecuteCommand("update Users set batchcount=batchcount+1");
               
                    //var js = JsonConvert.SerializeObject("Password Changed Successfully", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
              
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "Login");

                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetAllNotification(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String responseString = reader.ReadToEnd();
            DataTable MyscoreDataStatus = new DataTable();
            DataTable Myscores = new DataTable();
            
            try
            {
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(responseString);

                var notificationList = (from notification in icanSpeakContext.Notifications 
                                        join user in icanSpeakContext.Users on notification.UserId equals user.UserId
                                select new {
                                    NotificationId = Service.Encrypt(notification.NotificationId.ToString()),
                                    ProfilePicture = Service.GetUrl() + "ProfilePictures/" + user.ProfilePicture,
                                    user.Username,
                                    notification.Message,
                                    Date = DateTime.Parse(notification.CreatedDate.Value.ToShortDateString()).ToString("dd-MM-yyyy"),
                                    notification.CreatedDate ,
                                   Time = notification.CreatedDate.Value.ToShortTimeString() 
                                  }).OrderByDescending(x=>x.CreatedDate).ToList();

                var Myscore = (from user in icanSpeakContext.Users
                               where user.UserId == Convert.ToInt32(Service.Decrypt(userRequest["userid"]))
                               select new
                               {
                                   user.MyScore,
                                   user.TotalScore,
                                   user.FlashCard
                               }).ToList();

                if (Myscore.Count > 0)
                {
                    MyscoreDataStatus = Service.DataStatus("success", "1");
                    MyscoreDataStatus.TableName = "MyscoreDataStatus";
                    Myscores = Service.ConvertToDataTable(Myscore);
                    Myscores.TableName = "Myscores";
                    ds.Tables.Add(Myscores);
                    ds.Tables.Add(MyscoreDataStatus);
                }
                else
                {
                    MyscoreDataStatus = Service.DataStatus("No Data", "0");
                    MyscoreDataStatus.TableName = "MyscoreDataStatus";
                }

                if (notificationList.Count() > 0 && notificationList != null)
                {

                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(notificationList); ;
                    dt.TableName = "Notification";
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
                Helper.ErrorLog(ex, "GetAllNotification");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }

        public Stream Test(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String responseString = reader.ReadToEnd();
            DataTable MyscoreDataStatus = new DataTable();
            DataTable Myscores = new DataTable();
            string js1 = "";

            try
            {
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(responseString);

                var check = (from user in icanSpeakContext.Users
                             where user.Username == userRequest["UserName"]
                             select new
                             {
                                 user.Username
                             }).ToList();
                if (String.Equals(check[0].Username, userRequest["UserName"], StringComparison.OrdinalIgnoreCase))
                {
                    DataTable Table = Service.Message("No Data", "0");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    js1 = JsonConvert.SerializeObject(ds);
                   
                }


                return new MemoryStream(Encoding.UTF8.GetBytes(js1));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAllNotification");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }
    }
}