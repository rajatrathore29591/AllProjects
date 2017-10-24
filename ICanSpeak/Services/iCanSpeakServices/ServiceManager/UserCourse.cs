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

namespace iCanSpeakServices.ServiceManager
{
    public class UserCourse
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataTable Table = new DataTable();

        public Stream GetUserCourseById(Stream objStream)
        {

            DataSet ds = new DataSet("Result");
            User objUser = new User();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();

            DataTable MyscoreDataStatus = new DataTable();
            DataTable TotalFlashDataStatus = new DataTable();
            DataTable Myscores = new DataTable();

            int grammerindex = 1;

            try
            {
                var userLogin = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int userid = Convert.ToInt32(Service.Decrypt(userLogin["userid"]));

                //--------------------------------------------Grammer Section-----------------------------------------------------------//
                var grammerdata = (from grammer in icanSpeakContext.GrammerUnits
                                   where grammer.IsFree == true && grammer.DeleteDate == null
                                   select new
                                   {
                                       UnitId = Service.Encrypt(grammer.UnitId.ToString()),
                                       grammer.UnitNameEnglish,
                                       grammer.UnitNameArabic,
                                       grammer.CreateDate
                                   }).OrderByDescending(x => x.CreateDate);

                var grammerdatas = grammerdata.AsEnumerable().Select(x => new
                {
                    RowIndex = grammerindex++,
                    UnitId = x.UnitId,
                    UnitNameEnglish = x.UnitNameEnglish,
                    UnitNameArabic = x.UnitNameArabic,
                    Date = x.CreateDate
                }).ToList();

                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                DataTable Grammer = Service.ConvertToDataTable(grammerdatas);
                Grammer.TableName = "Grammer";

                //=======================
                var Myscore = (from user in icanSpeakContext.Users
                               where user.UserId == userid
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

                }
                else
                {
                    MyscoreDataStatus = Service.DataStatus("No Data", "0");
                    MyscoreDataStatus.TableName = "MyscoreDataStatus";
                }
                //=========================

                ds.Tables.Add(Table);
                ds.Tables.Add(Grammer);
                ds.Tables.Add(MyscoreDataStatus);
                ds.Tables.Add(Myscores);

                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetUserCourseById");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));// javaScriptSerializer.Serialize(ex.Message.ToString());
            }

        }
    }
}