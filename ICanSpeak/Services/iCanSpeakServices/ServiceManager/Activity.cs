using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Data.Objects;
using System.Data.Entity;

namespace iCanSpeakServices.ServiceManager
{
    public class Activity
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataTable Table = new DataTable();
        DataSet ds = new DataSet();

        public Stream MyActivityByUserId(Stream objStream)
        {
           
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            var searchResult = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
          

            try
            {
                int userid = Convert.ToInt32(Service.Decrypt(searchResult["UserId"]));
                var result = (from activity in icanSpeakContext.MyActivities
                              where activity.UserId == userid
                              orderby activity.MyActivityId descending
                              select new 
                                        { 
                                        MyActivityId = Service.Encrypt(activity.MyActivityId.ToString()),
                                        activity.Message,
                                        CreatedDate = activity.CreatedDate.Value.Date
                                        //EntityFunctions.TruncateTime(activity.CreatedDate)
                                        //CreatedDate = activity.CreatedDate.Value.Date
                                        //activity.CreatedDate.ToString("M/d/yyyy")
                                        //
                                        
                                        //EntityFunctions.TruncateTime(xx.CREATED_DATE);
                                        }
                             ).ToList();

                if (result.Count > 0)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable FlashCard = Service.ConvertToDataTable(result);
                    FlashCard.TableName = "MyActivity";
                    //foreach (DataColumn column in FlashCard.Columns)
                    //{
                    //    if(column.ColumnName == "CreatedDate")
                    //        column.DataType = System.Type.GetType("System.String");
                    //}
                    //foreach (DataRow row in FlashCard.Rows)
                    //{
                    //    row["CreatedDate"] = string.Format(Convert.ToDateTime(row["CreatedDate"]).Date.ToString(),"MM/dd/YY");
                    //}
                    ds.Tables.Add(Table);
                    ds.Tables.Add(FlashCard);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
                else
                {
                    Table = Service.Message("No Data", "0");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "MyActivityByUserId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }
    }
}