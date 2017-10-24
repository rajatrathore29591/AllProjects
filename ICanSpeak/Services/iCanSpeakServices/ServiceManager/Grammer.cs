using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Data;
using System.ServiceModel.Web;

namespace iCanSpeakServices.ServiceManager
{
    public class Grammer
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataSet ds = new DataSet("Result");
        DataTable Table = new DataTable();


        public Stream AddUnitToGrammer(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                //  requestString = "{\"UnitNameEnglish\":\"test 1\",\"UnitNameArabic\":\"test 1\",\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\",\"PPTUrl\":\"www.google.com\",\"VideoUrl\":\"www.google.com\",\"Price\":\"3\",\"Duration\":\"1 Month\",\"AssessmentSlots\":\"2.35,4,5,6,8.2,9\",\"DescriptionEnglish\":\"test english\",\"DescriptionArabic\":\"test arabic\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var checkUnit = (from unit in icanSpeakContext.GrammerUnits
                                 where unit.UnitNameEnglish == userRequest["UnitNameEnglish"]
                                 select new { unit.UnitId }).ToList();

                if (checkUnit.Count == 0)
                {
                    GrammerUnit objGrammerUnit = new GrammerUnit();

                    objGrammerUnit.UnitNameEnglish = userRequest["UnitNameEnglish"];
                    objGrammerUnit.UnitNameArabic = userRequest["UnitNameArabic"];
                    objGrammerUnit.DescriptionArabic = userRequest["DescriptionArabic"];
                    objGrammerUnit.DescriptionEnglish = userRequest["DescriptionEnglish"];
                    objGrammerUnit.IsActive = 1;
                    objGrammerUnit.Price = Convert.ToInt32(userRequest["Price"]);
                    objGrammerUnit.Duration = userRequest["Duration"];
                    objGrammerUnit.RewardPoints = Convert.ToInt32(userRequest["RewardPoints"]);
                    objGrammerUnit.MaxScore = Convert.ToInt32(userRequest["MaxScore"]);
                    objGrammerUnit.IsFree = Convert.ToBoolean(userRequest["IsFree"]);
                    objGrammerUnit.CreateDate = System.DateTime.Now;

                    icanSpeakContext.GrammerUnits.InsertOnSubmit(objGrammerUnit);
                    icanSpeakContext.SubmitChanges();
                    var UnitId = icanSpeakContext.GrammerUnits.ToList().Max(U => U.UnitId);
                    objGrammerUnit.PPTUrl = UnitId + "_grammerppt.ppt";
                    objGrammerUnit.VideoUrl = UnitId + "_video1.mp4";
                    objGrammerUnit.Video2Url = UnitId + "_video2.mp4";
                    icanSpeakContext.SubmitChanges();

                    icanSpeakContext.ExecuteCommand("update Users set batchcount=batchcount+1");

                    Notification objNotification = new Notification();
                    objNotification.UserId = 102;
                    objNotification.Message = "Added a new course";
                    objNotification.CreatedDate = System.DateTime.Now;
                    icanSpeakContext.Notifications.InsertOnSubmit(objNotification);
                    icanSpeakContext.SubmitChanges();

                    var js = JsonConvert.SerializeObject(UnitId, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize("Unit with this name already exists.")).Replace("\\/", "/")));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddKeyPhrasesByDialogId");
                //WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message)).Replace("\\/", "/")));
            }
        }

        public Stream UpdateGrammerUnit(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                //  requestString = "{\"UnitNameEnglish\":\"test 1\",\"UnitNameArabic\":\"test 1\",\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\",\"PPTUrl\":\"www.google.com\",\"VideoUrl\":\"www.google.com\",\"Price\":\"3\",\"Duration\":\"1 Month\",\"AssessmentSlots\":\"2.35,4,5,6,8.2,9\",\"DescriptionEnglish\":\"test english\",\"DescriptionArabic\":\"test arabic\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var checkUnit = (from unit in icanSpeakContext.GrammerUnits
                                 where unit.UnitNameEnglish == userRequest["UnitNameEnglish"] && unit.UnitId != Convert.ToInt32(userRequest["UnitId"])
                                 select new { unit.UnitId }).ToList();

                if (checkUnit.Count == 0)
                {
                    GrammerUnit objGrammerUnit = new GrammerUnit();
                    objGrammerUnit = icanSpeakContext.GrammerUnits.Single(key => key.UnitId == Convert.ToInt32(userRequest["UnitId"]));
                    objGrammerUnit.UnitNameEnglish = userRequest["UnitNameEnglish"];
                    objGrammerUnit.UnitNameArabic = userRequest["UnitNameArabic"];
                    objGrammerUnit.DescriptionArabic = userRequest["DescriptionArabic"];
                    objGrammerUnit.DescriptionEnglish = userRequest["DescriptionEnglish"];
                    //objGrammerUnit.PPTUrl = userRequest["PPTUrl"];
                   // objGrammerUnit.VideoUrl = userRequest["VideoUrl"];
                  //  objGrammerUnit.IsActive = 1;
                    objGrammerUnit.Price = Convert.ToInt32(userRequest["Price"]);
                    objGrammerUnit.Duration = userRequest["Duration"];
                    objGrammerUnit.RewardPoints = Convert.ToInt32(userRequest["RewardPoints"]);
                    objGrammerUnit.MaxScore = Convert.ToInt32(userRequest["MaxScore"]);
                    objGrammerUnit.IsFree = Convert.ToBoolean(userRequest["IsFree"]);
                    objGrammerUnit.ModifyDate = System.DateTime.Now;
                    icanSpeakContext.SubmitChanges();
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("already")));
                    
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddKeyPhrasesByDialogId");
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message))));
            }
        }

        public Stream GetAllUnitGrammer(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {

                var grammerUnits = (from grammer in icanSpeakContext.GrammerUnits
                                    where grammer.DeleteDate==null
                                    select new { grammer.UnitId, grammer.UnitNameEnglish, grammer.UnitNameArabic, grammer.VideoUrl, grammer.PPTUrl, grammer.Duration, grammer.AssessmentSlots, grammer.DescriptionEnglish, grammer.DescriptionArabic, grammer.CreateDate, grammer.IsActive,grammer.Price }).ToList();

                if (grammerUnits.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(grammerUnits, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No data")));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAllUnitGrammer");
                //WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message)).Replace("\\/", "/")));
            }
        }

        public Stream GetUnitGrammerByUnitId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();

            var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
            try
            {
                var grammerUnits = (from grammer in icanSpeakContext.GrammerUnits
                                    where grammer.UnitId == Convert.ToInt32(userRequest["unitId"])
                                    select new { grammer.UnitId, 
                                                 grammer.UnitNameEnglish,
                                                 grammer.UnitNameArabic,
                                                 AudioUrl = Service.GetUrl() + "GrammerVideos/" + grammer.VideoUrl,
                                                 VideoUrl = Service.GetUrl() + "GrammerVideos/" + grammer.VideoUrl,
                                                 PPTUrl = Service.GetUrl() + "GrammerPPT/" + grammer.PPTUrl,
                                                 grammer.AssessmentSlots,
                                                 grammer.DescriptionEnglish,
                                                 grammer.DescriptionArabic,
                                                 grammer.CreateDate, 
                                                 grammer.IsActive,
                                                 grammer.Duration,
                                                 grammer.RewardPoints,
                                                 grammer.MaxScore,
                                                 grammer.IsFree,
                                                 grammer.Price,
                                                 
                                                 }).ToList();

                if (grammerUnits.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(grammerUnits, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No data")));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetUnitGrammerByUnitId");
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message)).Replace("\\/", "/")));
            }
        }

        public string BookMarkStatus(int wordId, int userid)
        {
            string result = string.Empty;
            var status = (from Book in icanSpeakContext.BookMarks
                          where Book.CourseId == wordId && Book.UserId == userid
                          select new { Book.BookmarkId }
                         ).ToList();

            if (status.Count > 0)
            {
                result = "1";
            }
            else
            {
                result = "0";
            }

            return result;
        }

        public Stream GrammerUnitById(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();

            var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
            try
            {
                int unitid = Convert.ToInt32(Service.Decrypt(userRequest["unitId"]));
                int userid = Convert.ToInt32(Service.Decrypt(userRequest["userid"]));

                var grammerUnits = (from grammer in icanSpeakContext.GrammerUnits
                                    where grammer.UnitId == unitid
                                    select new
                                    {
                                        Unitid = userRequest["unitId"],
                                        grammer.UnitNameEnglish,
                                        grammer.UnitNameArabic,
                                        grammer.DescriptionEnglish,
                                        grammer.DescriptionArabic,
                                        VideoUrl = Service.GetUrl() + "GrammerVideos/" + grammer.VideoUrl,
                                        BookMarkStatus = BookMarkStatus(Convert.ToInt32(grammer.UnitId), userid)
                                    }).ToList();

                if (grammerUnits.Count > 0)
                {
                    var slots = (from slotpoint in icanSpeakContext.GrammerAssessmentQuestions
                                 where slotpoint.UnitId == unitid
                                 select new { slotpoint.SlotPointValue,slotpoint.CorrectAnswer }
                                ).ToList();

                    if (slots.Count > 0)
                    {
                        Table = Service.Message("success", "1");
                        Table.AcceptChanges();
                        DataTable Grammer = Service.ConvertToDataTable(grammerUnits); ;
                        Grammer.TableName = "Grammer";
                        DataTable DataStatus = Service.DataStatus("success", "1");
                        DataStatus.TableName = "SlotDataStatus";
                        DataTable SlotPoint = Service.ConvertToDataTable(slots); ;
                        SlotPoint.TableName = "SlotPoint";
                        ds.Tables.Add(Table);
                        ds.Tables.Add(Grammer);
                        ds.Tables.Add(SlotPoint);
                        ds.Tables.Add(DataStatus);
                        string js1 = JsonConvert.SerializeObject(ds);
                        WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                        return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                    }
                    else
                    {
                        Table = Service.Message("success", "1");
                        Table.AcceptChanges();
                        DataTable Grammer = Service.ConvertToDataTable(grammerUnits); ;
                        Grammer.TableName = "Grammer";
                        DataTable DataStatus = Service.DataStatus("No Data", "0");
                        DataStatus.TableName = "SlotDataStatus";
                        ds.Tables.Add(Table);
                        ds.Tables.Add(Grammer);
                        ds.Tables.Add(DataStatus);
                        string js1 = JsonConvert.SerializeObject(ds);
                        WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                        return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                    }


                    
                }
                else
                {
                    Table = Service.Message("No Data", "1");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }  
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "Login");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));// javaScriptSerializer.Serialize(ex.Message.ToString());

            }
        }

        public Stream NextGrammerUnitById(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();

            var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
            try
            {
                int unitid = Convert.ToInt32(Service.Decrypt(userRequest["unitId"]));
                int userid = Convert.ToInt32(Service.Decrypt(userRequest["userid"]));

                var LastGrammerID = (from grammer in icanSpeakContext.GrammerUnits
                                      where grammer.IsFree == true && grammer.DeleteDate == null
                                      orderby grammer.UnitId 
                                      select new { grammer.UnitId }).Take(1).ToList();

                var grammerUnits = (from grammer in icanSpeakContext.GrammerUnits
                                    where grammer.UnitId < unitid && grammer.IsFree == true && grammer.DeleteDate == null
                                    orderby grammer.UnitId descending
                                    select new
                                    {
                                        Unitid = Service.Encrypt(grammer.UnitId.ToString()),
                                        grammer.UnitNameEnglish,
                                        grammer.UnitNameArabic,
                                        grammer.DescriptionEnglish,
                                        grammer.DescriptionArabic,
                                        VideoUrl = Service.GetUrl() + "GrammerVideos/" + grammer.VideoUrl,
                                        thisUnitid = grammer.UnitId,
                                        BookMarkStatus = BookMarkStatus(Convert.ToInt32(grammer.UnitId), userid)
                                    }).Take(1).ToList();

                if (grammerUnits.Count > 0)
                {
                    var slots = (from slotpoint in icanSpeakContext.GrammerAssessmentQuestions
                                 where slotpoint.UnitId == unitid
                                 select new { slotpoint.SlotPointValue }
                                ).ToList();

                    if (slots.Count > 0)
                    {
                        Table = Service.Message("success", "1");
                        Table.AcceptChanges();
                        DataTable Grammer = Service.ConvertToDataTable(grammerUnits); ;

                        DataColumn newCol = new DataColumn("isLastData", typeof(string));
                        newCol.AllowDBNull = true;
                        Grammer.Columns.Add(newCol);

                        if (grammerUnits.ElementAtOrDefault(0).thisUnitid == LastGrammerID.ElementAtOrDefault(0).UnitId)
                        {
                            foreach (DataRow row in Grammer.Rows)
                            {
                                row["isLastData"] = "1";
                            }
                        }

                        else
                        {
                            foreach (DataRow row in Grammer.Rows)
                            {
                                row["isLastData"] = "0";
                            }
                        }

                        Grammer.TableName = "Grammer";
                        DataTable DataStatus = Service.DataStatus("success", "1");
                        DataStatus.TableName = "SlotDataStatus";
                        DataTable SlotPoint = Service.ConvertToDataTable(slots); ;
                        SlotPoint.TableName = "SlotPoint";
                        ds.Tables.Add(Table);
                        ds.Tables.Add(Grammer);
                        ds.Tables.Add(SlotPoint);
                        ds.Tables.Add(DataStatus);
                        string js1 = JsonConvert.SerializeObject(ds);
                        WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                        return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                    }
                    else
                    {
                        Table = Service.Message("success", "1");
                        Table.AcceptChanges();
                        DataTable Grammer = Service.ConvertToDataTable(grammerUnits);

                        DataColumn newCol = new DataColumn("isLastData", typeof(string));
                        newCol.AllowDBNull = true;
                        Grammer.Columns.Add(newCol);

                        if (grammerUnits.ElementAtOrDefault(0).thisUnitid == LastGrammerID.ElementAtOrDefault(0).UnitId)
                        {
                            foreach (DataRow row in Grammer.Rows)
                            {
                                row["isLastData"] = "1";
                            }
                        }

                        else
                        {
                            foreach (DataRow row in Grammer.Rows)
                            {
                                row["isLastData"] = "0";
                            }
                        }

                        Grammer.TableName = "Grammer";
                        DataTable DataStatus = Service.DataStatus("No Data", "0");
                        DataStatus.TableName = "SlotDataStatus";
                        ds.Tables.Add(Table);
                        ds.Tables.Add(Grammer);
                        ds.Tables.Add(DataStatus);
                        string js1 = JsonConvert.SerializeObject(ds);
                        WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                        return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                    }
                }
                else
                {
                    Table = Service.Message("No Data", "1");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "Login");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));// javaScriptSerializer.Serialize(ex.Message.ToString());

            }
        }

        public Stream BackGrammerUnitById(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();

            var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
            try
            {
                int unitid = Convert.ToInt32(Service.Decrypt(userRequest["unitId"]));
                int userid = Convert.ToInt32(Service.Decrypt(userRequest["userid"]));

                var FirstGrammerID = (from grammer in icanSpeakContext.GrammerUnits
                                      where grammer.IsFree == true && grammer.DeleteDate == null
                                      orderby grammer.UnitId descending
                                      select new { grammer.UnitId }).Take(1).ToList();

                var grammerUnits = (from grammer in icanSpeakContext.GrammerUnits
                                    where grammer.UnitId > unitid && grammer.IsFree == true && grammer.DeleteDate == null
                                    orderby grammer.UnitId 
                                    select new
                                    {
                                        Unitid = Service.Encrypt(grammer.UnitId.ToString()),
                                        grammer.UnitNameEnglish,
                                        grammer.UnitNameArabic,
                                        grammer.DescriptionEnglish,
                                        grammer.DescriptionArabic,
                                        VideoUrl = Service.GetUrl() + "GrammerVideos/" + grammer.VideoUrl,
                                        thisUnitid = grammer.UnitId,
                                        BookMarkStatus = BookMarkStatus(Convert.ToInt32(grammer.UnitId), userid)
                                    }).Take(1).ToList();

                if (grammerUnits.Count > 0)
                {
                    var slots = (from slotpoint in icanSpeakContext.GrammerAssessmentQuestions
                                 where slotpoint.UnitId == unitid
                                 select new { slotpoint.SlotPointValue }
                                ).ToList();

                    if (slots.Count > 0)
                    {
                        Table = Service.Message("success", "1");
                        Table.AcceptChanges();
                        DataTable Grammer = Service.ConvertToDataTable(grammerUnits);
                        
                        DataColumn newCol = new DataColumn("isLastData", typeof(string));
                        newCol.AllowDBNull = true;
                        Grammer.Columns.Add(newCol);

                        if (grammerUnits.ElementAtOrDefault(0).thisUnitid == FirstGrammerID.ElementAtOrDefault(0).UnitId)
                        {
                            foreach (DataRow row in Grammer.Rows)
                            {
                                row["isLastData"] = "1";
                            }
                        }
                        else
                        {
                            foreach (DataRow row in Grammer.Rows)
                            {
                                row["isLastData"] = "0";
                            }
                        }

                        Grammer.TableName = "Grammer";
                        DataTable DataStatus = Service.DataStatus("success", "1");
                        DataStatus.TableName = "SlotDataStatus";
                        DataTable SlotPoint = Service.ConvertToDataTable(slots); ;
                        SlotPoint.TableName = "SlotPoint";
                        ds.Tables.Add(Table);
                        ds.Tables.Add(Grammer);
                        ds.Tables.Add(SlotPoint);
                        ds.Tables.Add(DataStatus);
                        string js1 = JsonConvert.SerializeObject(ds);
                        WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                        return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                    }
                    else
                    {
                        Table = Service.Message("success", "1");
                        Table.AcceptChanges();
                        DataTable Grammer = Service.ConvertToDataTable(grammerUnits);

                        DataColumn newCol = new DataColumn("isLastData", typeof(string));
                        newCol.AllowDBNull = true;
                        Grammer.Columns.Add(newCol);

                        if (grammerUnits.ElementAtOrDefault(0).thisUnitid == FirstGrammerID.ElementAtOrDefault(0).UnitId)
                        {
                            foreach (DataRow row in Grammer.Rows)
                            {
                                row["isLastData"] = "1";
                            }
                        }
                        else
                        {
                            foreach (DataRow row in Grammer.Rows)
                            {
                                row["isLastData"] = "0";
                            }
                        }

                        Grammer.TableName = "Grammer";
                        DataTable DataStatus = Service.DataStatus("No Data", "0");
                        DataStatus.TableName = "SlotDataStatus";
                        ds.Tables.Add(Table);
                        ds.Tables.Add(Grammer);
                        ds.Tables.Add(DataStatus);
                        string js1 = JsonConvert.SerializeObject(ds);
                        WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                        return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                    }



                }
                else
                {
                    Table = Service.Message("No Data", "1");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }



            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "Login");
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));// javaScriptSerializer.Serialize(ex.Message.ToString());

            }
        }

        public Stream DeleteGrammerUnitByUnitId(Stream objStream)
        {
            try
            {
                GrammerUnit objGrammerUnit = new GrammerUnit();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"userId\":\"8\",\"softDelete\":\"true\"}";
                var grammerUnit = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from user in icanSpeakContext.GrammerUnits
                              where user.UnitId == Convert.ToInt32(grammerUnit["unitId"])
                              select user).FirstOrDefault();

                if (grammerUnit["softDelete"] == "true")
                {
                    if (result.IsActive == 1)
                    {
                        result.IsActive = 0;
                    }
                    else
                    {
                        result.IsActive = 1;
                    }
                }
                else
                {
                    result.DeleteDate = System.DateTime.Now;
                }
                icanSpeakContext.SubmitChanges();
                // return javaScriptSerializer.Serialize("User deleted successfully");
                //var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteGrammerUnitByUnitId");
                //var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message.ToString())));
            }
        }

        public Stream AddAssessmentQuestionByUnitId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                //  requestString = "{\"unitId\":\"1\",\"UserId\":\"1\",\"AccessToken\":\"xdoykz\",\"Question\":\"hi, how _______ you ?\",\"QuestionType\":\"FillInTheBlanks\",\"AnswerOptions\":\"is,are,am\",\"CorrectAnswer\":\"are\"}";
              //  var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                var userRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<AssessmentQuestionModel>(requestString);
                string UnitId = userRequest.unitId;
                string[] SlotValues = userRequest.SlotValues.ToArray();
                string[] CorrectAnswer = userRequest.CorrectAnswer.ToArray();
                int finalcount = SlotValues.Count() - 1;
                for (int i = 0; i <= finalcount; i++)
                {
                    GrammerAssessmentQuestion objGrammerAssessmentQuestion = new GrammerAssessmentQuestion();
                    objGrammerAssessmentQuestion.Question = "";
                    objGrammerAssessmentQuestion.SlotPointValue = SlotValues[i];
                    objGrammerAssessmentQuestion.IsActive = 1;
                    objGrammerAssessmentQuestion.CorrectAnswer = CorrectAnswer[i];
                    objGrammerAssessmentQuestion.UnitId = Convert.ToInt32(UnitId);
                    icanSpeakContext.GrammerAssessmentQuestions.InsertOnSubmit(objGrammerAssessmentQuestion);
                    icanSpeakContext.SubmitChanges();
                }
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Assessment Question Added Successfully")));
                // return javaScriptSerializer.Serialize("Assessment Question Added Successfully");
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddAssessmentQuestionByUnitId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }


        public class AssessmentQuestionModel
        {
            public List<string> SlotValues { get; set; }
            public List<string> CorrectAnswer { get; set; }
            public string unitId { get; set; }
        }

        public Stream GetAssessmentQuestionByUnitId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"UserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var checkQuestion = (from grammerQuestion in icanSpeakContext.GrammerAssessmentQuestions
                                     where grammerQuestion.UnitId == Convert.ToInt32(userRequest["unitId"])
                                     select new { grammerQuestion.SlotId, grammerQuestion.Question, grammerQuestion.CorrectAnswer, grammerQuestion.SlotPointValue,grammerQuestion.CreateDate, grammerQuestion.IsActive }).ToList();

                if (checkQuestion.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(checkQuestion, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No Question Available")));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAssessmentQuestionByUnitId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream GetAssessmentQuestionBySlotId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);


                var GetAssessmentQuestion = (from question in icanSpeakContext.GrammerAssessmentQuestions
                                             where question.SlotId == Convert.ToInt32(userRequest["SlotId"])
                                             select new { question.SlotId, question.Question,question.SlotPointValue, question.CorrectAnswer }).SingleOrDefault();

                // return javaScriptSerializer.Serialize(checkGetAnswers);
                var js = JsonConvert.SerializeObject(GetAssessmentQuestion, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAssessmentQuestionBySlotId");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }

        public Stream UpdateAssessmentQuestionBySlotId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                GrammerAssessmentQuestion objgrmrassesmentquestn = new GrammerAssessmentQuestion();
                objgrmrassesmentquestn = icanSpeakContext.GrammerAssessmentQuestions.Single(key=>key.SlotId==Convert.ToInt32(userRequest["SlotId"]));
                objgrmrassesmentquestn.Question = userRequest["Question"];
                objgrmrassesmentquestn.SlotPointValue = userRequest["SlotPointValue"];
                objgrmrassesmentquestn.CorrectAnswer = userRequest["CorrectAnswer"];
                icanSpeakContext.SubmitChanges();


                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAssessmentQuestionBySlotId");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }
        }

        public Stream DeleteAssessmentQuestionBySlotId(Stream objStream)
        {
            try
            {
                GrammerAssessmentQuestion objgrmrassesmentquestn = new GrammerAssessmentQuestion();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"SlotId\":\"1\",\"softDelete\":\"true\"}";
                var AssessmentQuestion = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from user in icanSpeakContext.GrammerAssessmentQuestions
                              where user.SlotId == Convert.ToInt32(AssessmentQuestion["SlotId"])
                              select user).FirstOrDefault();

                if (AssessmentQuestion["softDelete"] == "true")
                {
                    if (result.IsActive == 1)
                    {
                        result.IsActive = 0;
                    }
                    else
                    {
                        result.IsActive = 1;
                    }
                }
                else
                {
                    icanSpeakContext.GrammerAssessmentQuestions.DeleteOnSubmit(result);
                }
                icanSpeakContext.SubmitChanges();
                // return javaScriptSerializer.Serialize("User deleted successfully");
                //var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteAssessmentQuestionBySlotId");
                //var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message.ToString())));
            }
        }
        public Stream SubmitAnswerBySlotId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"QuestionId\":\"5\",\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\",\"UserId\":\"2\",\"Answer\":\"are\",\"IsCorrectAnswer\":\"1\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                

                var checkSubmitAnswer = (from submitanswer in icanSpeakContext.GrammerAssessmentAnswers
                                         where submitanswer.SlotId == Convert.ToInt32(userRequest["slotId"]) && submitanswer.UserId == Convert.ToInt32(userRequest["userId"])
                                         select new { submitanswer.SlotId }).ToList();

                if (checkSubmitAnswer.Count > 0)
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("You already answered this question")));
                }
                else
                {
                    GrammerAssessmentAnswer objAnswer = new GrammerAssessmentAnswer();
                    objAnswer.SlotId = Convert.ToInt32(userRequest["slotId"]);
                    objAnswer.UserId = Convert.ToInt32(userRequest["userId"]);
                    objAnswer.UserAnswer = userRequest["answer"];
                    icanSpeakContext.GrammerAssessmentAnswers.InsertOnSubmit(objAnswer);
                    icanSpeakContext.SubmitChanges();

                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Answer Submitted Successfully")));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "SubmitAnswerBySlotId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        
       

       



        public Stream GetAnswersBySlotId(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            try
            {
                // requestString = "{\"DialogId\":\"5\",\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);


                var checkGetAnswers = (from question in icanSpeakContext.GrammerAssessmentQuestions
                                       join answers in icanSpeakContext.GrammerAssessmentAnswers on question.UnitId equals answers.UnitId
                                       where question.UnitId == Convert.ToInt32(userRequest["unitId"])
                                       select new { question.SlotId, 
                                                    question.Question,
                                                    question.UnitId,
                                                    answers.AnswerId,
                                                    answers.UserAnswer }).ToList();

                // return javaScriptSerializer.Serialize(checkGetAnswers);
                var js = JsonConvert.SerializeObject(checkGetAnswers, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAnswersBySlotId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }


    }
}