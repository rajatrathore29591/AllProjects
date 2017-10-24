using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using ICanSpeakWebsite.App_Start;
using ICanSpeakWebsite.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ICanSpeakWebsite.Controllers
{
    public class MessageController : Controller
    {
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataSet jsonDataSet = new DataSet();
        HelperClass objhelperclass = new HelperClass();
        string jsonstring = string.Empty;
        string resultjson = string.Empty;

        public ActionResult Messages()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError","Login");
            }
            try
            {
                jsonstring = "{\"UserId\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetMessageByUserId");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    ViewBag.Username = System.Web.HttpRuntime.Cache["Username"].ToString();
                    ViewBag.Country = System.Web.HttpRuntime.Cache["Country"].ToString();
                    ViewBag.BatchCount = jsonDataSet.Tables["UserMessages"].Rows[0]["BatchCount"].ToString();
                    ViewBag.Messages = jsonDataSet.Tables["UserMessages"].Rows[0]["Messages"].ToString();
                    JArray jsonArray = JArray.Parse(JsonConvert.SerializeObject(jsonDataSet.Tables["UserMessages"]));
                    List<UserMessage> message = jsonArray.ToObject<List<UserMessage>>();
                    return View(message);

                }
                else
                {
                    ViewBag.Username = System.Web.HttpRuntime.Cache["Username"].ToString();
                    ViewBag.Country = System.Web.HttpRuntime.Cache["Country"].ToString();
                    ViewBag.BatchCount = 0;
                    ViewBag.Messages = 0;
                    return View();
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }

        }

        public ActionResult MessageDetail(string id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            try
            {
                jsonstring = "{\"MessageId\":\"" + id + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetMessageDetailByMessageId");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                MessageDetail objModel = new MessageDetail();
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    ViewBag.Username = System.Web.HttpRuntime.Cache["Username"].ToString();
                    ViewBag.Country = System.Web.HttpRuntime.Cache["Country"].ToString();
                    ViewBag.BatchCount = jsonDataSet.Tables["UserMessages"].Rows[0]["BatchCount"].ToString();
                    ViewBag.Messages = jsonDataSet.Tables["UserMessages"].Rows[0]["Messages"].ToString();
                    objModel.SenderId = jsonDataSet.Tables["UserMessages"].Rows[0]["SenderId"].ToString();
                    objModel.Date = jsonDataSet.Tables["UserMessages"].Rows[0]["Date"].ToString();
                    objModel.MessageId = jsonDataSet.Tables["UserMessages"].Rows[0]["MessageId"].ToString();
                    objModel.Subject = jsonDataSet.Tables["UserMessages"].Rows[0]["Subject"].ToString();
                    objModel.ProfilePicture = jsonDataSet.Tables["UserMessages"].Rows[0]["ProfilePicture"].ToString();
                    objModel.Username = jsonDataSet.Tables["UserMessages"].Rows[0]["Username"].ToString();
                    objModel.Email = jsonDataSet.Tables["UserMessages"].Rows[0]["Email"].ToString();
                    objModel.DetailMessage = jsonDataSet.Tables["UserMessages"].Rows[0]["DetailMessage"].ToString();
                    return View(objModel);

                }
                else
                {
                    List<MessageDetail> message = new List<MessageDetail>();
                    return View(message);
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }

        }

        public JsonResult GetUserName(string prefix)
        {
           
            try
            {
                jsonstring = "{\"prefix\":\"" + prefix + "\",\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "SearchUser");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                List<string> list = new List<string>();
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {

                    for (int i = 0; i < jsonDataSet.Tables["Users"].Select().Length; i++)
                    {
                        list.Add(string.Format("{0}-{1}", jsonDataSet.Tables["Users"].Rows[i]["UserName"].ToString(), jsonDataSet.Tables["Users"].Rows[i]["UserId"]));
                    }
                    return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    list.Add(string.Format("{0}-{1}", "No result found.", 1));
                    return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Test()
        {
            return View();
        }


        public ActionResult SendMessage()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult SendMessage(SentMessage objModel)
        {
            try
            {
                jsonstring = "{\"senderid\":\"" + Session["UserId"] + "\",\"recieverid\":\"" + objModel.recieverid + "\",\"subject\":\"" + objModel.subject + "\",\"messagebody\":\"" + objModel.messagebody + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "SendMessage");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    ViewBag.Message = "success";
                    return View();
                }
                else
                {
                    ViewBag.Message = "error";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        public ActionResult MessageReply(string id, string username)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            ReplyMessage objModel = new ReplyMessage();
            objModel.RecieverId = id;
            objModel.UserName = username;
            return View(objModel);
        }

        [HttpPost]
        public ActionResult MessageReply(ReplyMessage objModel)
        {
            try
            {
                jsonstring = "{\"senderid\":\"" + Session["UserId"] + "\",\"recieverid\":\"" + objModel.RecieverId + "\",\"subject\":\"" + objModel.Subject + "\",\"messagebody\":\"" + objModel.Messagebody + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "SendMessage");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    ViewBag.Message = "success";
                    return View();
                }
                else
                {
                    ViewBag.Message = "error";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult DeleteMessage(string messageId)
        {
            try
            {
                jsonstring = "{\"MessageId\":\"" + messageId + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "DeleteMessageByMessageId");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    ViewBag.Message = "success";
                    return View();
                }
                else
                {
                    ViewBag.Message = "error";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult DeleteMultipleMessage(string MessageId)
        {
            try
            {
                jsonstring = "{\"MessageId\":\"" + MessageId + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "DeleteMultipleMessageByMessageId");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    //ViewBag.Message = "success";
                    
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }


        public ActionResult MessageForward(string id, string subject, string messagebody)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            ForwardMessage objModel = new ForwardMessage();
            objModel.RecieverId = id;
            objModel.Subject = subject;
            objModel.Messagebody = messagebody;
            return View(objModel);
        }

        [HttpPost]
        public ActionResult MessageForward(ForwardMessage objModel)
        {

            try
            {
                jsonstring = "{\"senderid\":\"" + Session["UserId"] + "\",\"recieverid\":\"" + objModel.RecieverId + "\",\"subject\":\"" + objModel.Subject + "\",\"messagebody\":\"" + objModel.Messagebody + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "SendMessage");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    ViewBag.Message = "success";
                    return View();
                }
                else
                {
                    ViewBag.Message = "error";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }
    }
}
