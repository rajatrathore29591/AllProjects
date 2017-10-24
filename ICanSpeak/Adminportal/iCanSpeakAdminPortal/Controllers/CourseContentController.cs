using iCanSpeakAdminPortal.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace iCanSpeakAdminPortal.Controllers
{
    public class CourseContentController : Controller
    {
        //
        // GET: /CourseContent/
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;

        [Authorize]
        public ActionResult VocabularyList()
        {
            try
            {
                string json = "";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetAllVocabCategory");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetAllVocabCategory");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                        if (table.Rows[0][0].ToString() == "No data")
                        {
                            List<VocabularyModel> objList = new List<VocabularyModel>();
                            return View(objList);
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(result);
                            List<VocabularyModel> vocab = jsonArray.ToObject<List<VocabularyModel>>();
                            return View(vocab.ToList());
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult VocabDetail(int? vocabid)
        {
            try
            {
                string json = "{\"vocabularyId\":\"" + vocabid + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetCategoryByVocabId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetCategoryByVocabId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                        VocabularyModel objVocabularyModel = new VocabularyModel();
                        objVocabularyModel.Text = table.Rows[0]["Text"].ToString();
                        objVocabularyModel.ArabicText = table.Rows[0]["ArabicText"].ToString();
                        objVocabularyModel.Duration = table.Rows[0]["Duration"].ToString();
                        objVocabularyModel.IsActive = Convert.ToBoolean(table.Rows[0]["IsActive"].ToString());
                        objVocabularyModel.IsFree = Convert.ToBoolean(table.Rows[0]["IsFree"].ToString());
                        objVocabularyModel.issubcategory = table.Rows[0]["IsSubCategory"].ToString();
                        objVocabularyModel.MaxScore = table.Rows[0]["MaxScore"].ToString();
                        objVocabularyModel.Price = Convert.ToInt32(table.Rows[0]["Price"].ToString());
                        objVocabularyModel.RewardPoints = table.Rows[0]["RewardPoints"].ToString();
                        objVocabularyModel.SampleSentance = table.Rows[0]["SampleSentance"].ToString();
                        return View(objVocabularyModel);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult VocabularySubCategoryList(int vocabid)
        {
            try
            {
                string json = "{\"vocabularyId\":\"" + vocabid.ToString() + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetAllVocabSubCategory");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);


                        TempData["WordCount"] = result; //sjy
                        if (table.Rows[0][0].ToString() == "No data")
                        {
                            List<VocabularySubCategoryModel> vocabsubcategory = new List<VocabularySubCategoryModel>();
                            return View(vocabsubcategory);
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(result);

                            List<VocabularySubCategoryModel> vocabsubcategory = jsonArray.ToObject<List<VocabularySubCategoryModel>>();
                            vocabsubcategory.ForEach(z => z.SetCount = Convert.ToString(Convert.ToInt32(z.WordCount) / 5 + (Convert.ToInt32(z.WordCount) % 5 > 0 ? 1 : 0)));
                            return View(vocabsubcategory);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult VocabularyWordList(int? subcatid, int vocabid)
        {
            try
            {
                string json = "{\"vocabularySubId\":\"" + subcatid.ToString() + "\",\"vocabid\":\"" + vocabid.ToString() + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetAllVocabWord");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();

                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                        if (table.Rows[0][0].ToString() == "No data")
                        {
                            List<VocabularyWordModel> vocabword = new List<VocabularyWordModel>();
                            return View(vocabword);
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(result);
                            List<VocabularyWordModel> vocab = jsonArray.ToObject<List<VocabularyWordModel>>();
                            return View(vocab.ToList());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult AddVocabularyCategoryName(int vocabid = 0)
        {
            try
            {
                VocabularyModel objModel = new VocabularyModel();
                if (vocabid != 0)
                {
                    string json = "{\"vocabularyId\":\"" + vocabid.ToString() + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetCategoryByVocabId");
                    //request = (HttpWebRequest)WebRequest.Create("http://localhost:3160/Service.svc/GetCategoryByVocabId");
                    string sb = JsonConvert.SerializeObject(data);
                    request.Method = "POST";
                    Byte[] bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                    using (response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).", response.StatusCode,
                            response.StatusDescription));
                        Stream responseStream = response.GetResponseStream();
                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            string result = sr.ReadToEnd();
                            var table = JsonConvert.DeserializeObject<DataTable>(result);
                            //JArray jsonArray = JArray.Parse(result);
                            //List<VocabularyModel> vocab = jsonArray.ToObject<List<VocabularyModel>>();

                            objModel.ArabicText = table.Rows[0]["ArabicText"].ToString();
                            objModel.Text = table.Rows[0]["Text"].ToString();
                            objModel.SampleSentance = table.Rows[0]["SampleSentance"].ToString();
                            objModel.Price = Convert.ToInt32(table.Rows[0]["Price"].ToString());
                            objModel.Duration = table.Rows[0]["Duration"].ToString();
                            objModel.RewardPoints = table.Rows[0]["RewardPoints"].ToString();
                            objModel.MaxScore = table.Rows[0]["MaxScore"].ToString();
                            objModel.SampleSentance = table.Rows[0]["IsFree"].ToString();
                            objModel.issubcategory = table.Rows[0]["IsSubCategory"].ToString();
                            objModel.VocabularyId = vocabid;

                            return View(objModel);
                        }
                    }
                }
                return View(objModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVocabularyCategoryName(VocabularyModel objModel)
        {
            try
            {

                if (objModel.VocabularyId == 0 || Convert.ToString(objModel.VocabularyId) == "")
                {
                    if (objModel.issubcategory == "0")
                    {
                        objModel.subcategory = "";
                    }
                    string json = "{\"text\":\"" + objModel.Text + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\",\"subcategory\":\"" + objModel.subcategory + "\",\"Duration\":\"" + objModel.Duration + "\",\"Price\":\"" + objModel.Price + "\",\"RewardPoints\":\"" + objModel.RewardPoints + "\",\"MaxScore\":\"" + objModel.MaxScore + "\",\"IsFree\":\"" + objModel.IsFree + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddVocabCategory");
                    // request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/AddVocabCategory");
                    string sb = JsonConvert.SerializeObject(data);
                    request.Method = "POST";
                    Byte[] bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                    using (response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).", response.StatusCode,
                            response.StatusDescription));
                        Stream responseStream = response.GetResponseStream();
                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            string result = sr.ReadToEnd();
                            int vocabid = Convert.ToInt32(result);

                            //if (objModel.Image != null)
                            //{

                            //    string filepath = Server.MapPath("../../VocabularyImages/");
                            //    objModel.Image.SaveAs(filepath + vocabid.ToString() + "_vocabimage.jpeg");
                            //}
                            //if (objModel.Audio != null)
                            //{
                            //    string filepath = Server.MapPath("../../VocabularyAudios/");
                            //    objModel.Image.SaveAs(filepath + vocabid.ToString() + "_vocabaudio.mp3");
                            //}
                            return RedirectToAction("VocabularyList", "CourseContent");
                        }
                    }
                }
                else
                {

                    string json = "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\",\"Duration\":\"" + objModel.Duration + "\",\"Price\":\"" + objModel.Price + "\",\"RewardPoints\":\"" + objModel.RewardPoints + "\",\"MaxScore\":\"" + objModel.MaxScore + "\",\"IsFree\":\"" + objModel.IsFree + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/UpdateVocabCategory");
                    //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/UpdateVocabCategory");
                    string sb = JsonConvert.SerializeObject(data);
                    request.Method = "POST";
                    Byte[] bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                    using (response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).", response.StatusCode,
                            response.StatusDescription));
                        Stream responseStream = response.GetResponseStream();
                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            string result = sr.ReadToEnd();

                            //if (objModel.Image != null)
                            //{
                            //    string filepath = Server.MapPath("/Uploads/VocabularyImages/");
                            //    objModel.Image.SaveAs(filepath + objModel.VocabularyId.ToString() + "_image_" + objModel.Image.FileName);
                            //}
                            //if (objModel.Audio != null)
                            //{
                            //    string filepath = Server.MapPath("/Uploads/VocabularyAudios/");
                            //    objModel.Image.SaveAs(filepath + objModel.VocabularyId.ToString() + "_audio_" + objModel.Image.FileName);
                            //}
                            return RedirectToAction("VocabularyList", "CourseContent");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        public JsonResult ManageVocabCategoryStatus(string vocabid, string softDelete)
        {
            try
            {
                string json = "{\"vocabularyId\":\"" + vocabid + "\",\"softDelete\":\"" + softDelete + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/DeleteVocabByCategoryId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        return Json(result.ToString(), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult AddVocabularySubCategoryName(int vocabid, int subcatid)
        {
            try
            {
                if (Convert.ToString(TempData["SubCatResult"]) != "")
                {
                    ViewBag.Result = TempData["SubCatResult"];
                }
                if (subcatid != 0)
                {
                    string json = "{\"vacabularySubId\":\"" + subcatid.ToString() + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetSubCategoryByVacabSubId");
                    //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetSubCategoryByVacabSubId");
                    string sb = JsonConvert.SerializeObject(data);
                    request.Method = "POST";
                    Byte[] bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                    using (response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).", response.StatusCode,
                            response.StatusDescription));
                        Stream responseStream = response.GetResponseStream();
                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            string result = sr.ReadToEnd();
                            var table = JsonConvert.DeserializeObject<DataTable>(result);
                            VocabularySubCategoryModel objModel = new VocabularySubCategoryModel();
                            objModel.VacabularySubId = Convert.ToInt32(table.Rows[0]["VacabularySubId"].ToString());
                            objModel.SubCategoryName = table.Rows[0]["SubCategoryName"].ToString();
                            objModel.VocabularyId = Convert.ToInt32(table.Rows[0]["VocabularyId"].ToString());
                            return View(objModel);
                        }
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult UpdateVocabularySubCategoryName(int vocabid, int subcatid)
        {
            try
            {
                if (Convert.ToString(TempData["SubCatResult"]) != "")
                {
                    ViewBag.Result = TempData["SubCatResult"];
                }
                if (subcatid != 0)
                {
                    string json = "{\"vacabularySubId\":\"" + subcatid.ToString() + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetSubCategoryByVacabSubId");
                    //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetSubCategoryByVacabSubId");
                    string sb = JsonConvert.SerializeObject(data);
                    request.Method = "POST";
                    Byte[] bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                    using (response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).", response.StatusCode,
                            response.StatusDescription));
                        Stream responseStream = response.GetResponseStream();
                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            string result = sr.ReadToEnd();
                            var table = JsonConvert.DeserializeObject<DataTable>(result);
                            VocabularySubCategoryModel objModel = new VocabularySubCategoryModel();
                            objModel.VacabularySubId = Convert.ToInt32(table.Rows[0]["VacabularySubId"].ToString());
                            objModel.SubCategoryName = table.Rows[0]["SubCategoryName"].ToString();
                            objModel.VocabularyId = Convert.ToInt32(table.Rows[0]["VocabularyId"].ToString());
                            return View(objModel);
                        }
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVocabularySubCategoryName(VocabularySubCategoryModel objModel, int? VocabularyId, int? VacabularySubId)
        {
            try
            {
                if (objModel.VacabularySubId == 0 || Convert.ToString(objModel.VacabularySubId) == "")
                {
                    string json = "{\"vocabularyId\":\"" + VocabularyId + "\",\"subcategory\":\"" + objModel.subcategory + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddVocabSubCategory");
                    //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/AddVocabSubCategory");
                    string sb = JsonConvert.SerializeObject(data);
                    request.Method = "POST";
                    Byte[] bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                    using (response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).", response.StatusCode,
                            response.StatusDescription));
                        Stream responseStream = response.GetResponseStream();
                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            string result = sr.ReadToEnd();



                            TempData["SubCatResult"] = "Success";
                            return RedirectToAction("VocabularySubCategoryList", "CourseContent", new { vocabid = VocabularyId });

                            //return View(new { vocabid = vocabid });
                        }
                    }
                }
                else
                {
                    string json = "{\"VacabularySubId\":\"" + VacabularySubId + "\",\"subcategory\":\"" + objModel.subcategory + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/UpdateVocabSubCategoryById");
                    //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/UpdateVocabSubCategoryById");
                    string sb = JsonConvert.SerializeObject(data);
                    request.Method = "POST";
                    Byte[] bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                    using (response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).", response.StatusCode,
                            response.StatusDescription));
                        Stream responseStream = response.GetResponseStream();
                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            string result = sr.ReadToEnd();

                            //int vocabid = Convert.ToInt32(result);
                            //JArray jsonArray = JArray.Parse(result);
                            //var table = JsonConvert.DeserializeObject<DataTable>(result);

                            return RedirectToAction("VocabularySubCategoryList", "CourseContent", new { vocabid = VocabularyId });
                            //return View();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TempData["SubCatResult"] = ex.Message.ToString();
                ////ViewBag.Result = ex.Message.ToString();
                //return RedirectToAction("AddVocabularySubCategoryName", "CourseContent", new { vocabid = objModel.VocabularyId });
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateVocabularySubCategoryName(VocabularySubCategoryModel objModel, int? VocabularyId, int? VacabularySubId)
        {
            try
            {
                if (objModel.VacabularySubId == 0 || Convert.ToString(objModel.VacabularySubId) == "")
                {
                    string json = "{\"vocabularyId\":\"" + VocabularyId + "\",\"subcategory\":\"" + objModel.subcategory + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddVocabSubCategory");
                    //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/AddVocabSubCategory");
                    string sb = JsonConvert.SerializeObject(data);
                    request.Method = "POST";
                    Byte[] bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                    using (response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).", response.StatusCode,
                            response.StatusDescription));
                        Stream responseStream = response.GetResponseStream();
                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            string result = sr.ReadToEnd();



                            TempData["SubCatResult"] = "Success";
                            return RedirectToAction("VocabularySubCategoryList", "CourseContent", new { vocabid = VocabularyId });

                            //return View(new { vocabid = vocabid });
                        }
                    }
                }
                else
                {
                    string json = "{\"VacabularySubId\":\"" + VacabularySubId + "\",\"subcategory\":\"" + objModel.subcategory + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/UpdateVocabSubCategoryById");
                    //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/UpdateVocabSubCategoryById");
                    string sb = JsonConvert.SerializeObject(data);
                    request.Method = "POST";
                    Byte[] bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                    using (response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).", response.StatusCode,
                            response.StatusDescription));
                        Stream responseStream = response.GetResponseStream();
                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            string result = sr.ReadToEnd();

                            //int vocabid = Convert.ToInt32(result);
                            //JArray jsonArray = JArray.Parse(result);
                            //var table = JsonConvert.DeserializeObject<DataTable>(result);

                            return RedirectToAction("VocabularySubCategoryList", "CourseContent", new { vocabid = VocabularyId });
                            //return View();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TempData["SubCatResult"] = ex.Message.ToString();
                ////ViewBag.Result = ex.Message.ToString();
                //return RedirectToAction("AddVocabularySubCategoryName", "CourseContent", new { vocabid = objModel.VocabularyId });
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult AddVocabularyWord(int subcatid, int vocabid, int wordid = 0)
        {
            try
            {
                if (Convert.ToString(TempData["wordResult"]) != "")
                {
                    ViewBag.Result = TempData["wordResult"];
                }
                if (wordid != 0)
                {
                    string json = "{\"wordId\":\"" + wordid.ToString() + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetVocabWordByWordId");
                    string sb = JsonConvert.SerializeObject(data);
                    request.Method = "POST";
                    Byte[] bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                    using (response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).", response.StatusCode,
                            response.StatusDescription));
                        Stream responseStream = response.GetResponseStream();
                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            string result = sr.ReadToEnd();
                            var table = JsonConvert.DeserializeObject<DataTable>(result);
                            //JArray jsonArray = JArray.Parse(result);
                            //List<VocabularyModel> vocab = jsonArray.ToObject<List<VocabularyModel>>();
                            VocabularyWordModel objModel = new VocabularyWordModel();
                            objModel.ArabicText = table.Rows[0]["ArabicText"].ToString();
                            objModel.AudioUrl = table.Rows[0]["AudioUrl"].ToString();
                            objModel.PictureUrl = table.Rows[0]["PictureUrl"].ToString();
                            objModel.EnglishText = table.Rows[0]["EnglishText"].ToString();

                            return View(objModel);
                        }
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVocabularyWord(VocabularyWordModel objModel)
        {
            try
            {
                if (objModel.WordId == 0 || Convert.ToString(objModel.WordId) == "")
                {
                    string wordnames = string.Empty;
                    string arabicwordnames = string.Empty;
                    foreach (var wordname in objModel.EnglishTexts)
                    {
                        wordnames = wordnames + wordname + "|||";
                    }
                    wordnames = wordnames.Substring(0, wordnames.Length - 3);
                    foreach (var wordname in objModel.ArabicTexts)
                    {
                        arabicwordnames = arabicwordnames + wordname + "|||";
                    }
                    arabicwordnames = arabicwordnames.Substring(0, arabicwordnames.Length - 3);


                    string json = "{\"vacabularySubId\":\"" + objModel.VocabularySubId + "\",\"vocabid\":\"" + objModel.vocabid + "\",\"wordname\":\"" + wordnames + "\",\"arabicwordnames\":\"" + arabicwordnames + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddVocabWord");
                    //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/AddVocabWord");
                    string sb = JsonConvert.SerializeObject(data);
                    request.Method = "POST";
                    Byte[] bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                    using (response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).", response.StatusCode,
                            response.StatusDescription));
                        Stream responseStream = response.GetResponseStream();
                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            string result = sr.ReadToEnd();
                            var table = JsonConvert.DeserializeObject<DataTable>(result);

                            int finalid = Convert.ToInt32(table.Rows[0][0].ToString());


                            if (objModel.Image != null && objModel.Audio != null)
                            {
                                string imagepath = Server.MapPath("../../VocabularyImages/");
                                string audiopath = Server.MapPath("../../VocabularyAudios/");
                                int filecount = objModel.Image.Count() - 1;
                                for (int i = filecount; i >= 0; i--)
                                {
                                    if (objModel.Image[i].FileName != null)
                                    {
                                        objModel.Image[i].SaveAs(imagepath + finalid.ToString() + "_wordimage.jpg");
                                    }
                                    if (objModel.Audio[i].FileName != null)
                                    {
                                        objModel.Audio[i].SaveAs(audiopath + finalid.ToString() + "_wordaudio.mp3");
                                    }
                                    finalid--;
                                }
                            }



                            return RedirectToAction("VocabularyWordList", "CourseContent", new { subcatid = objModel.VocabularySubId, vocabid = objModel.vocabid });

                            //return View(new { vocabid = vocabid });
                        }
                    }
                }
                else
                {
                    string json = "{\"arabicText\":\"" + objModel.ArabicText + "\",\"englishText\":\"" + objModel.EnglishText + "\",\"vocabWordId\":\"" + objModel.WordId + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.SubCategoryName + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/UpdateVocabWordById");
                    string sb = JsonConvert.SerializeObject(data);
                    request.Method = "POST";
                    Byte[] bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                    using (response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).", response.StatusCode,
                            response.StatusDescription));
                        Stream responseStream = response.GetResponseStream();
                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            string result = sr.ReadToEnd();
                            if (objModel.ImageFile != null)
                            {
                                string filepath = Server.MapPath("../../VocabularyImages/");
                                objModel.ImageFile.SaveAs(filepath + objModel.WordId.ToString() + "_wordimage.jpg");
                            }
                            if (objModel.AudioFile != null)
                            {
                                string filepath = Server.MapPath("../../VocabularyAudios/");
                                objModel.AudioFile.SaveAs(filepath + objModel.WordId.ToString() + "_wordaudio.mp3");
                            }
                            return RedirectToAction("VocabularyWordList", "CourseContent", new { subcatid = objModel.VocabularySubId, vocabid = objModel.vocabid });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TempData["wordResult"] = ex.Message.ToString();
                ////ViewBag.Result = ex.Message.ToString();
                //return RedirectToAction("AddVocabularyWord", "CourseContent", new { subcatid = objModel.VocabularySubId });
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        public JsonResult ManageVocabSubCategoryStatus(string vocabsubcategoryid, string VocabularyId, string softDelete)
        {
            try
            {
                string json = "{\"vocabSubCategoryId\":\"" + vocabsubcategoryid + "\",\"vocabid\":\"" + VocabularyId + "\",\"softDelete\":\"" + softDelete + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/DeleteVocabSubCategoryById");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/DeleteVocabSubCategoryById");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        return Json(result.ToString(), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ManageVocabWordStatus(string wordid, string subcategoryid, string vocabid, string softDelete)
        {
            try
            {
                string json = "{\"vocabWordId\":\"" + wordid + "\",\"subcategoryid\":\"" + subcategoryid + "\",\"vocabid\":\"" + vocabid + "\",\"softDelete\":\"" + softDelete + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/DeleteVocabWordById");
                //  request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/DeleteVocabWordById");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        return Json(result.ToString(), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        //===============================================Grammer Section======================================================================================================//
        [Authorize]
        public ActionResult GrammerList()
        {
            try
            {
                string json = "";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetAllUnitGrammer");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);

                        if (table.Rows[0][0].ToString() == "No data")
                        {
                            List<GrammerModel> objList = new List<GrammerModel>();
                            return View(objList);
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(result);
                            List<GrammerModel> grammer = jsonArray.ToObject<List<GrammerModel>>();
                            return View(grammer.ToList());
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult GrammarDetail(int? grammerid)
        {
            try
            {
                string json = "{\"unitId\":\"" + grammerid + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetUnitGrammerByUnitId");
                // request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetUnitGrammerByUnitId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                        GrammerModel objGrammerModel = new GrammerModel();
                        objGrammerModel.UnitId = Convert.ToInt32(table.Rows[0]["UnitId"].ToString());
                        objGrammerModel.DescriptionArabic = table.Rows[0]["DescriptionArabic"].ToString();
                        objGrammerModel.DescriptionEnglish = table.Rows[0]["DescriptionEnglish"].ToString();
                        objGrammerModel.PPTUrl = table.Rows[0]["PPTUrl"].ToString();
                        objGrammerModel.UnitNameArabic = table.Rows[0]["UnitNameArabic"].ToString();
                        objGrammerModel.UnitNameEnglish = table.Rows[0]["UnitNameEnglish"].ToString();
                        objGrammerModel.VideoUrl = table.Rows[0]["VideoUrl"].ToString();
                        objGrammerModel.Duration = table.Rows[0]["Duration"].ToString();
                        objGrammerModel.RewardPoints = table.Rows[0]["RewardPoints"].ToString();
                        objGrammerModel.Price = Convert.ToInt32(table.Rows[0]["Price"].ToString());
                        objGrammerModel.MaxScore = table.Rows[0]["MaxScore"].ToString();
                        objGrammerModel.IsFree = Convert.ToBoolean(table.Rows[0]["IsFree"].ToString());
                        return View(objGrammerModel);



                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult AddGrammerUnit(int unitid = 0)
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGrammerUnit(GrammerModel objModel)
        {
            try
            {

                string json = "{\"UnitNameEnglish\":\"" + objModel.UnitNameEnglish + "\",\"UnitNameArabic\":\"" + objModel.UnitNameArabic + "\",\"DescriptionArabic\":\"" + objModel.DescriptionArabic + "\",\"DescriptionEnglish\":\"" + objModel.DescriptionEnglish + "\",\"Duration\":\"" + objModel.Duration + "\",\"Price\":\"" + objModel.Price + "\",\"RewardPoints\":\"" + objModel.RewardPoints + "\",\"MaxScore\":\"" + objModel.MaxScore + "\",\"IsFree\":\"" + objModel.IsFree + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddUnitToGrammer");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/AddUnitToGrammer");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string id = sr.ReadToEnd();
                        
                        if (objModel.Video != null)
                        {
                            string filepath = Server.MapPath("../../GrammerVideos/");
                            objModel.Video.SaveAs(filepath + id.ToString() + "_video1.mp4");
                        }
                        if (objModel.Video2 != null)
                        {
                            string filepath = Server.MapPath("../../GrammerVideos/");
                            objModel.Video2.SaveAs(filepath + id.ToString() + "_video2.mp4");
                        }
                        if (objModel.PPT != null)
                        {
                            string filepath = Server.MapPath("../../GrammerPPT/");
                            objModel.PPT.SaveAs(filepath + id.ToString() + "_grammerppt.ppt");
                        }

                        return RedirectToAction("GrammerList", "CourseContent");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["wordResult"] = ex.Message.ToString();
                ViewBag.ErrorName = ex.Message;
                return View("Error"); //RedirectToAction("AddVocabularyWord", "CourseContent", new { subcatid = objModel.VocabularySubId });
            }
        }

        [Authorize]
        public ActionResult UpdateGrammerUnit(int unitid)
        {
            try
            {
                GrammerModel objGrammerModel = new GrammerModel();

                string json = "{\"unitId\":\"" + unitid + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetUnitGrammerByUnitId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetUnitGrammerByUnitId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);

                        //objGrammerModel.AudioUrl = table.Rows[0]["AudioUrl"].ToString();
                        objGrammerModel.UnitId = Convert.ToInt32(table.Rows[0]["UnitId"].ToString());
                        objGrammerModel.DescriptionArabic = table.Rows[0]["DescriptionArabic"].ToString();
                        objGrammerModel.DescriptionEnglish = table.Rows[0]["DescriptionEnglish"].ToString();
                        objGrammerModel.UnitNameArabic = table.Rows[0]["UnitNameArabic"].ToString();
                        objGrammerModel.UnitNameEnglish = table.Rows[0]["UnitNameEnglish"].ToString();
                        objGrammerModel.VideoUrl = table.Rows[0]["VideoUrl"].ToString();
                        objGrammerModel.Duration = table.Rows[0]["Duration"].ToString();
                        objGrammerModel.RewardPoints = table.Rows[0]["RewardPoints"].ToString();
                        objGrammerModel.Price = Convert.ToInt32(table.Rows[0]["Price"].ToString());
                        objGrammerModel.MaxScore = table.Rows[0]["MaxScore"].ToString();
                        objGrammerModel.IsFree = Convert.ToBoolean(table.Rows[0]["IsFree"].ToString());
                        objGrammerModel.PPTUrl = table.Rows[0]["PPTUrl"].ToString();
                        objGrammerModel.AudioUrl = table.Rows[0]["AudioUrl"].ToString();
                        objGrammerModel.VideoUrl = table.Rows[0]["VideoUrl"].ToString();
                        return View(objGrammerModel);
                        //TempData["wordResult"] = "Success";
                        //return RedirectToAction("AddVocabularyWord", "CourseContent", new { subcatid = objModel.VocabularySubId });
                    }
                }


            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }


        [Authorize]
        [HttpPost]

        public ActionResult UpdateGrammerUnit(GrammerModel objModel)
        {
            string json = "{\"UnitId\":\"" + objModel.UnitId + "\",\"UnitNameEnglish\":\"" + objModel.UnitNameEnglish + "\",\"UnitNameArabic\":\"" + objModel.UnitNameEnglish + "\",\"DescriptionArabic\":\"" + objModel.DescriptionArabic + "\",\"DescriptionEnglish\":\"" + objModel.DescriptionEnglish + "\",\"Duration\":\"" + objModel.Duration + "\",\"Price\":\"" + objModel.Price + "\",\"RewardPoints\":\"" + objModel.RewardPoints + "\",\"MaxScore\":\"" + objModel.MaxScore + "\",\"IsFree\":\"" + objModel.IsFree + "\"}";
            var data = javaScriptSerializer.DeserializeObject(json);
            request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/UpdateGrammerUnit");
            //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/UpdateGrammerUnit");
            string sb = JsonConvert.SerializeObject(data);
            request.Method = "POST";
            Byte[] bt = Encoding.UTF8.GetBytes(sb);
            Stream st = request.GetRequestStream();
            st.Write(bt, 0, bt.Length);
            st.Close();
            using (response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format(
                    "Server error (HTTP {0}: {1}).", response.StatusCode,
                    response.StatusDescription));
                Stream responseStream = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(responseStream))
                {
                    string result = sr.ReadToEnd();
                    var table = JsonConvert.DeserializeObject<DataTable>(result);
                    if (table.Rows[0][0].ToString() == "Success")
                    {
                        if (objModel.PPT != null)
                        {
                            string filepath = Server.MapPath("../../GrammerPPT/");
                            objModel.PPT.SaveAs(filepath + objModel.UnitId.ToString() + "_grammerppt.ppt");
                        }
                        if (objModel.Video != null)
                        {
                            string filepath = Server.MapPath("../../GrammerVideos/");
                            objModel.Video.SaveAs(filepath + objModel.UnitId.ToString() + "_video1.mp4");
                        }
                        if (objModel.Video2 != null)
                        {
                            string filepath = Server.MapPath("../../GrammerVideos/");
                            objModel.Video2.SaveAs(filepath + objModel.UnitId.ToString() + "_video2.mp4");
                        }

                        return RedirectToAction("GrammerList", "CourseContent");
                    }
                    else
                    {
                        objModel.errormsg = "A unit with this name already exists.";
                        return View(objModel);
                    }

                }
            }
        }


        public JsonResult ManageGrammerUnitStatus(string unitId, string softDelete)
        {
            try
            {
                string json = "{\"unitId\":\"" + unitId + "\",\"softDelete\":\"" + softDelete + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/DeleteGrammerUnitByUnitId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        var table = JsonConvert.DeserializeObject<DataTable>(sr.ReadToEnd());
                        return Json(table.Rows[0][0].ToString(), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult AddGrammerAssessmentQuestionSlots()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddGrammerAssessmentQuestionSlots(GrammerAssessmentQuestionSlotsModel objModel)
        {
            try
            {
                string json = "{\"unitId\":\"" + objModel.UnitId + "\",\"SlotValues\":" + JsonConvert.SerializeObject(objModel.slotvaluearray) + ",\"CorrectAnswer\":" + JsonConvert.SerializeObject(objModel.answerarray) + "}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddAssessmentQuestionByUnitId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        return RedirectToAction("GetAllSlots", "CourseContent", new { unitId = objModel.UnitId });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetAllSlots(int unitId = 0)
        {
            try
            {
                string json = "{\"unitId\":\"" + unitId + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetAssessmentQuestionByUnitId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);

                        if (table.Rows[0][0].ToString() == "No Question Available")
                        {
                            List<GrammerAssessmentQuestionSlotsModel> objList = new List<GrammerAssessmentQuestionSlotsModel>();
                            return View(objList);
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(result);
                            List<GrammerAssessmentQuestionSlotsModel> grammer = jsonArray.ToObject<List<GrammerAssessmentQuestionSlotsModel>>();
                            return View(grammer.ToList());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        public JsonResult ManageGrammerAssessmentQuestionSlotStatus(string SlotId, string softDelete)
        {
            try
            {
                string json = "{\"SlotId\":\"" + SlotId + "\",\"softDelete\":\"" + softDelete + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/DeleteAssessmentQuestionBySlotId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                        return Json(table.Rows[0][0].ToString(), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult EditSlotValue(int SlotId)
        {
            try
            {
                string json = "{\"SlotId\":\"" + SlotId + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetAssessmentQuestionBySlotId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        result = "[" + result + "]";
                        var table = JsonConvert.DeserializeObject<DataTable>(result);

                        GrammerAssessmentQuestionSlotsModel objModel = new GrammerAssessmentQuestionSlotsModel();
                        objModel.SlotId = SlotId;
                        // objModel.Question = table.Rows[0]["Question"].ToString();
                        objModel.SlotPointValue = table.Rows[0]["SlotPointValue"].ToString();
                        objModel.CorrectAnswer = table.Rows[0]["CorrectAnswer"].ToString();
                        return View(objModel);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult EditSlotValue(GrammerAssessmentQuestionSlotsModel objModel)
        {
            try
            {
                string json = "{\"SlotId\":\"" + objModel.SlotId + "\",\"SlotPointValue\":\"" + objModel.SlotPointValue + "\",\"Question\":\" \",\"CorrectAnswer\":\"" + objModel.CorrectAnswer + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/UpdateAssessmentQuestionBySlotId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                        //return View(objModel);
                        return RedirectToAction("GetAllSlots", "CourseContent", new { unitId = objModel.UnitId });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }
        //===============================================Dialog Section======================================================================================================//



        //===============================================Dialog Section======================================================================================================//

        [Authorize]
        public ActionResult DialogList()
        {
            try
            {
                string json = "";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetAllDialog");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetAllDialog");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                        if (table.Rows[0][0].ToString() == "No Data")
                        {
                            List<DialogsModel> dialog = new List<DialogsModel>();
                            return View(dialog);
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(result);
                            List<DialogsModel> dialog = jsonArray.ToObject<List<DialogsModel>>();
                            return View(dialog);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult DialogDetail(int? dialogid)
        {
            try
            {
                string json = "{\"dialogid\":\"" + dialogid + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetDialogDetail");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetDialogDetail");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);

                        DialogsModel objDialog = new DialogsModel();
                        objDialog.DialogId = Convert.ToInt32(table.Rows[0]["DialogId"]);
                        objDialog.EnglishName = table.Rows[0]["EnglishName"].ToString();
                        objDialog.DescriptionEngilsh = table.Rows[0]["DescriptionEnglish"].ToString();
                        objDialog.Duration = table.Rows[0]["Duration"].ToString();
                        objDialog.RewardPoints = table.Rows[0]["RewardPoints"].ToString();
                        objDialog.Price = Convert.ToInt32(table.Rows[0]["Price"].ToString());
                        objDialog.MaxScore = table.Rows[0]["MaxScore"].ToString();
                        objDialog.IsFree = Convert.ToBoolean(table.Rows[0]["IsFree"].ToString());

                        return View(objDialog);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult AddDialog()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddDialog(DialogsModel objModel)
        {
            try
            {
                string json = "{\"EnglishName\":\"" + objModel.EnglishName + "\",\"ArabicName\":\"" + objModel.ArabicName + "\",\"StoryArabic\":\"" + objModel.StoryArabic + "\",\"StoryEnglish\":\"" + objModel.StoryEnglish + "\",\"DescriptionArabic\":\"" + objModel.DescriptionArabic + "\",\"DescriptionEnglish\":\"" + objModel.DescriptionEngilsh + "\",\"DialogGender\":\"" + objModel.DialogGender + "\",\"Duration\":\"" + objModel.Duration + "\",\"Price\":\"" + objModel.Price + "\",\"RewardPoints\":\"" + objModel.RewardPoints + "\",\"MaxScore\":\"" + objModel.MaxScore + "\",\"IsFree\":\"" + objModel.IsFree + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddDialog");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/AddDialog");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        int dialogid = Convert.ToInt32(sr.ReadToEnd());
                        if (objModel.Audio1 != null)
                        {
                            string filepath = Server.MapPath("../../DialogAudio/");
                            if (objModel.Audio1.FileName.Contains(".wav"))
                            {
                                objModel.Audio1.SaveAs(filepath + dialogid.ToString() + "_dialogaudio1.wav");
                            }
                            else
                            {
                                objModel.Audio1.SaveAs(filepath + dialogid.ToString() + "_dialogaudio1.mp3");
                            }
                        }
                        if (objModel.Audio2 != null)
                        {
                            string filepath = Server.MapPath("../../DialogAudio/");
                            if (objModel.Audio2.FileName.Contains(".wav"))
                            {
                                objModel.Audio2.SaveAs(filepath + dialogid.ToString() + "_dialogaudio2.wav");
                            }
                            else
                            {
                                objModel.Audio2.SaveAs(filepath + dialogid.ToString() + "_dialogaudio2.mp3");
                            }
                        }
                        if (objModel.Video != null)
                        {
                            string filepath = Server.MapPath("../../DialogVideo/");
                            objModel.Video.SaveAs(filepath + dialogid.ToString() + "_dialogvideo.mp4");
                        }
                        //srt files
                        if (objModel.EnglishSubtitle != null)
                        {
                            string filepath = Server.MapPath("../../DialogSubtitle/");
                            objModel.EnglishSubtitle.SaveAs(filepath + dialogid.ToString() + "_englishsubtitle.vtt");
                        }

                        if (objModel.ArabicSubtitle != null)
                        {
                            string filepath = Server.MapPath("../../DialogSubtitle/");
                            objModel.ArabicSubtitle.SaveAs(filepath + dialogid.ToString() + "_arabicsubtitle.vtt");
                        }

                        if (objModel.BothSubtitle != null)
                        {
                            string filepath = Server.MapPath("../../DialogSubtitle/");
                            objModel.BothSubtitle.SaveAs(filepath + dialogid.ToString() + "_bothsubtitle.vtt");
                        }

                        return RedirectToAction("DialogList", "CourseContent");
                        //if (objModel.Audio != null)
                        //{
                        //    string filepath = Server.MapPath("/Uploads/GrammerAudios/");
                        //    objModel.Audio.SaveAs(filepath + id.ToString() + "_audio_" + objModel.Audio.FileName);
                        //}
                        //TempData["wordResult"] = "Success";
                        //return RedirectToAction("AddVocabularyWord", "CourseContent", new { subcatid = objModel.VocabularySubId });
                        return View(objModel);
                    }


                }

            }
            catch (Exception ex)
            {
                TempData["wordResult"] = ex.Message.ToString();
                ViewBag.ErrorName = ex.Message;
                return View("Error"); //RedirectToAction("AddVocabularyWord", "CourseContent", new { subcatid = objModel.VocabularySubId });
            }
        }

        [Authorize]
        public ActionResult UpdateDialog(string dialogid)
        {
            try
            {
                string json = "{\"dialogid\":\"" + dialogid + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetDialogDetail");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetDialogDetail");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);

                        DialogsModel objDialog = new DialogsModel();
                        objDialog.DialogId = Convert.ToInt32(table.Rows[0]["DialogId"]);
                        objDialog.EnglishName = table.Rows[0]["EnglishName"].ToString();
                        objDialog.DescriptionEngilsh = table.Rows[0]["DescriptionEnglish"].ToString();
                        objDialog.StoryEnglish = table.Rows[0]["StoryEnglish"].ToString();
                        objDialog.ArabicName = table.Rows[0]["ArabicName"].ToString();
                        objDialog.DescriptionArabic = table.Rows[0]["DescriptionArabic"].ToString();
                        objDialog.StoryArabic = table.Rows[0]["StoryArabic"].ToString();
                        objDialog.Duration = table.Rows[0]["Duration"].ToString();
                        objDialog.RewardPoints = table.Rows[0]["RewardPoints"].ToString();
                        objDialog.Price = Convert.ToInt32(table.Rows[0]["Price"].ToString());
                        objDialog.MaxScore = table.Rows[0]["MaxScore"].ToString();
                        objDialog.IsFree = Convert.ToBoolean(table.Rows[0]["IsFree"].ToString());
                        objDialog.AudioUrl = table.Rows[0]["AudioUrl"].ToString();
                        objDialog.Audio2Url = table.Rows[0]["Audio2Url"].ToString();
                        objDialog.VideoUrl = table.Rows[0]["VideoUrl"].ToString();
                        objDialog.EnglishSubtitleUrl = table.Rows[0]["EnglishSubtitleUrl"].ToString();
                        objDialog.ArabicSubtitleUrl = table.Rows[0]["ArabicSubtitleUrl"].ToString();
                        objDialog.BothSubtitleUrl = table.Rows[0]["BothSubtitleUrl"].ToString();

                        return View(objDialog);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }

        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateDialog(DialogsModel objModel)
        {

            string json = "{\"DialogId\":\"" + objModel.DialogId + "\",\"EnglishName\":\"" + objModel.EnglishName + "\",\"ArabicName\":\"" + objModel.ArabicName + "\",\"StoryEnglish\":\"" + objModel.StoryEnglish + "\",\"StoryArabic\":\"" + objModel.StoryArabic + "\",\"DescriptionEngilsh\":\"" + objModel.DescriptionEngilsh + "\",\"DescriptionArabic\":\"" + objModel.DescriptionArabic + "\",\"DialogGender\":\"" + objModel.DialogGender + "\",\"Duration\":\"" + objModel.Duration + "\",\"Price\":\"" + objModel.Price + "\",\"RewardPoints\":\"" + objModel.RewardPoints + "\",\"MaxScore\":\"" + objModel.MaxScore + "\",\"IsFree\":\"" + objModel.IsFree + "\"}";
            var data = javaScriptSerializer.DeserializeObject(json);
            request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/UpdateDialog");
            //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/UpdateDialog");
            string sb = JsonConvert.SerializeObject(data);
            request.Method = "POST";
            Byte[] bt = Encoding.UTF8.GetBytes(sb);
            Stream st = request.GetRequestStream();
            st.Write(bt, 0, bt.Length);
            st.Close();
            using (response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format(
                    "Server error (HTTP {0}: {1}).", response.StatusCode,
                    response.StatusDescription));
                Stream responseStream = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(responseStream))
                {
                    string result = sr.ReadToEnd();
                    var table = JsonConvert.DeserializeObject<DataTable>(result);
                    if (table.Rows[0][0].ToString() == "success")
                    {
                        int dialogid = objModel.DialogId;
                        if (objModel.Audio1 != null)
                        {
                            string filepath = Server.MapPath("../../DialogAudio/");
                            if (objModel.Audio1.FileName.Contains(".wav"))
                            {
                                objModel.Audio1.SaveAs(filepath + dialogid.ToString() + "_dialogaudio1.wav");
                            }
                            else
                            {
                                objModel.Audio1.SaveAs(filepath + dialogid.ToString() + "_dialogaudio1.mp3");
                            }
                        }
                        if (objModel.Audio2 != null)
                        {
                            string filepath = Server.MapPath("../../DialogAudio/");
                            if (objModel.Audio2.FileName.Contains(".wav"))
                            {
                                objModel.Audio2.SaveAs(filepath + dialogid.ToString() + "_dialogaudio2.wav");
                            }
                            else
                            {
                                objModel.Audio2.SaveAs(filepath + dialogid.ToString() + "_dialogaudio2.mp3");
                            }
                        }
                        if (objModel.Video != null)
                        {
                            string filepath = Server.MapPath("../../DialogVideo/");
                            objModel.Video.SaveAs(filepath + dialogid.ToString() + "_dialogvideo.mp4");
                        }
                        //srt files
                        if (objModel.EnglishSubtitle != null)
                        {
                            string filepath = Server.MapPath("../../DialogSubtitle/");
                            objModel.EnglishSubtitle.SaveAs(filepath + dialogid.ToString() + "_englishsubtitle.vtt");
                        }

                        if (objModel.ArabicSubtitle != null)
                        {
                            string filepath = Server.MapPath("../../DialogSubtitle/");
                            objModel.ArabicSubtitle.SaveAs(filepath + dialogid.ToString() + "_arabicsubtitle.vtt");
                        }

                        if (objModel.BothSubtitle != null)
                        {
                            string filepath = Server.MapPath("../../DialogSubtitle/");
                            objModel.BothSubtitle.SaveAs(filepath + dialogid.ToString() + "_bothsubtitle.vtt");
                        }

                        return RedirectToAction("DialogList", "CourseContent");
                    }
                    else
                    {
                        objModel.errormsg = "Dialog with this name already exists.";
                        return View(objModel);
                    }
                }
            }
        }

        [Authorize]
        public JsonResult DeleteDialogByDialogId(string dialogid)
        {
            try
            {
                string json = "{\"dialogid\":\"" + dialogid + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/DeleteDialogByDialogId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/DeleteDialogByDialogId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ManageDialogStatus(int DialogId, string softDelete)
        {
            try
            {
                string json = "{\"DialogId\":\"" + DialogId + "\",\"softDelete\":\"" + softDelete + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/ManageDialog");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/ManageDialog");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        return Json(result.ToString(), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        //===========================================Dialog KeyPhrase Section Starts===============================================================================================//
        [Authorize]
        public ActionResult KeyPhraseList(string dialogid)
        {
            try
            {
                string json = "{\"DialogId\":\"" + dialogid + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetKeyPhrasesByDialogId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                        TempData["DialogId"] = dialogid;
                        if (table.Rows[0][0].ToString() == "Invalid dialog id")
                        {
                            List<KeyPhraseModel> objList = new List<KeyPhraseModel>();
                            return View(objList);
                        }
                        else
                        {

                            JArray jsonArray = JArray.Parse(result);
                            List<KeyPhraseModel> keyphrase = jsonArray.ToObject<List<KeyPhraseModel>>();
                            return View(keyphrase.ToList());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult AddKeyPhrase(string dialogid)
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddKeyPhrases(AddKeyPhraseModel objAddKeyPhraseModel)
        {
            try
            {
                string json = "{\"DialogId\":\"" + objAddKeyPhraseModel.DialogId + "\",\"Arabictxt\":" + JsonConvert.SerializeObject(objAddKeyPhraseModel.Arabictxt) + ",\"Englishtxt\":" + JsonConvert.SerializeObject(objAddKeyPhraseModel.Englishtxt) + "}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddKeyPhrasesByDialogId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                    }
                }
                return RedirectToAction("KeyPhraseList", "CourseContent", new { dialogid = objAddKeyPhraseModel.DialogId });
            }
            catch (Exception ex)
            {
                TempData["Message"] = "An error occured, please try again";
                //return View("AddKeyPhrase");
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }

        }

        [Authorize]
        public ActionResult EditKeyPhrases(string keyphraseid)
        {
            try
            {
                string json = "{\"keyPhrasesId\":\"" + keyphraseid + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);

                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetKeyPhrasesByKeyPhrasesId");

                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        JArray jsonArray = JArray.Parse(result);
                        List<KeyPhraseModel> keyphrase = jsonArray.ToObject<List<KeyPhraseModel>>();
                        return View(keyphrase.ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "An error occured, please try again";
                //return View("EditKeyPhrases");
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult UpdateKeyPhrases(EditKeyPhrasemodel objEditKeyPhrasemodel)
        {
            try
            {
                string json = "{\"KeyPhrasesId\":\"" + objEditKeyPhrasemodel.KeyPhrasesId + "\",\"EnglishText\":\"" + objEditKeyPhrasemodel.EnglishText + "\",\"ArabicText\":\"" + objEditKeyPhrasemodel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/UpdateKeyPhrasesByKeyPhrasesId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                    }
                }
                return RedirectToAction("KeyPhraseList", "CourseContent", new { DialogId = objEditKeyPhrasemodel.DialogId });
            }
            catch (Exception ex)
            {
                TempData["Message"] = "An error occured, please try again";
                //return View("AddKeyPhrase");
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }

        }
        public JsonResult DeleteKeyPhrase(string keyphraseid, string status)
        {
            try
            {
                string json = "{\"keyPhrasesId\":\"" + keyphraseid + "\",\"softDelete\":\"" + status + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/Services/Service.svc/DeleteKeyPhrasesByKeyPhrasesId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        //=====================================Dialog KeyPhrase Section Ends=======================================================================================================//    


        //=====================================Dialog Conversection Section Starts=======================================================================================================//

        [Authorize]
        public ActionResult ConversationList(string dialogid, string gender)
        {
            try
            {
                string json = "{\"DialogId\":\"" + dialogid + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetConversationByDialogId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                        if (table.Rows[0][0].ToString() == "Invalid dialog id")
                        {
                            List<ConversationModel> emptyModel = new List<ConversationModel>();
                            return View(emptyModel);
                        }
                        JArray jsonArray = JArray.Parse(result);
                        TempData["DialogId"] = table.Rows[0]["DialogId"].ToString();
                        List<ConversationModel> vocab = jsonArray.ToObject<List<ConversationModel>>();
                        return View(vocab.ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult AddConversation(string dialogid, string gender)
        {
            TempData["DialogId"] = dialogid;
            TempData["gender"] = gender;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddConversations(AddConversation objModel)
        {
            try
            {
                string arbtxt = JsonConvert.SerializeObject(objModel.onearbtxt);
                string json = "{\"DialogId\":\"" + objModel.DialogId + "\",\"oneengtxt\":" + JsonConvert.SerializeObject(objModel.oneengtxt) + ",\"onearbtxt\":" + JsonConvert.SerializeObject(objModel.onearbtxt) + ",\"twoengtxt\":" + JsonConvert.SerializeObject(objModel.twoengtxt) + ",\"twoarbtxt\":" + JsonConvert.SerializeObject(objModel.twoarbtxt) + "}";
                //var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddConversationByDialogId");
                // request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/AddConversationByDialogId");
                //   string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(json);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        //return Json(result.ToString(), JsonRequestBehavior.AllowGet);
                        return RedirectToAction("ConversationList", "CourseContent", new { dialogid = objModel.DialogId, gender = objModel.gender });
                    }
                }

            }
            catch (Exception ex)
            {
                TempData["Message"] = "An error occured, please try again";
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }



        public JsonResult AddConversations(string dialogid, string conversationtxt)
        {
            try
            {
                string json = "{\"DialogId\":\"" + dialogid + "\",\"ConversationText\":\"" + conversationtxt + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddConversationByDialogId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        return Json(result.ToString(), JsonRequestBehavior.AllowGet);
                    }
                }

            }
            catch (Exception ex)
            {
                TempData["Message"] = "An error occured, please try again";
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult DeleteConversation(string conversationid, string status)
        {
            try
            {
                string json = "{\"conversationId\":\"" + conversationid + "\",\"softDelete\":\"" + status + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/Services/Service.svc/DeleteConversationByConversationId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult EditConversation(string conversationid, string gender)
        {
            try
            {
                string json = "{\"conversationId\":\"" + conversationid + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetConversationByConversationId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        JArray jsonArray = JArray.Parse(result);
                        List<ConversationModel> keyphrase = jsonArray.ToObject<List<ConversationModel>>();
                        return View(keyphrase.ToList());
                    }
                }

            }
            catch (Exception ex)
            {
                TempData["Message"] = "An error occured, please try again";
                //return View("EditKeyPhrases");
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }

        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateConversation(string dialogids, string conversationid, string oneengtxt, string onearbtxt, string twoengtxt, string twoarbtxt, string DialogGender)
        {
            try
            {
                string json = "{\"ConversationId\":\"" + conversationid + "\",\"Person1Text\":\"" + oneengtxt + "\",\"Person2Text\":\"" + twoengtxt + "\",\"onearbtxt\":\"" + onearbtxt + "\",\"twoarbtxt\":\"" + twoarbtxt + "\",\"LanguageName\":\"English\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/UpdateConversationByConversationId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        if (result.Contains("Conversation Updated Successfully"))
                        {
                            return RedirectToAction("ConversationList", "CourseContent", new { dialogid = dialogids, gender = DialogGender });
                        }
                        else
                        {
                            TempData["Message"] = "An error occured, please try again";
                        }
                    }
                }
                return View("EditConversation");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "An error occured, please try again";
                //return View("EditKeyPhrases");
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }

        }
        //=====================================Dialog Conversection Section Ends=========================================================================== Anu============================//
        [Authorize]
        public ActionResult DialogAssessmentQuestions(int dialogid = 0)
        {
            try
            {
                string json = "{\"DialogId\":\"" + dialogid + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetAssessmentQuestionByDialogId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                        if (table.Rows[0][0].ToString() == "No Question Available")
                        {
                            List<DialogAssessmentQuestionsModel> emptyModel = new List<DialogAssessmentQuestionsModel>();
                            return View(emptyModel);
                        }
                        JArray jsonArray = JArray.Parse(result);
                        List<DialogAssessmentQuestionsModel> queires = jsonArray.ToObject<List<DialogAssessmentQuestionsModel>>();
                        return View(queires.ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }


        //====================================Dialog Assements Question Starts================================================================================================//    
        [Authorize]
        public ActionResult AddDialogAssessmentQuestions(int dialogid)
        {
            try
            {
                ViewBag.dialogid = dialogid;
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }


        [HttpPost]
        public ActionResult AddDialogAssessmentQuestions(DialogAssessmentQuestionsModel objModel)
        {
            try
            {

                string json = "{\"DialogId\":\"" + objModel.DialogId + "\",\"Question\":\"" + objModel.Question + "\",\"QuestionType\":\"" + objModel.QuestionType + "\",\"FillAnsText\":\"" + objModel.FillAnsText + "\",\"TrueFalseType\":\"" + objModel.TrueFalseType + "\",\"ObjOpt1txt\":\"" + objModel.ObjOpt1txt + "\",\"ObjOpt2txt\":\"" + objModel.ObjOpt2txt + "\",\"ObjOpt3txt\":\"" + objModel.ObjOpt3txt + "\",\"OptionCorrectAnswer\":\"" + objModel.OptionCorrectAnswer + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddAssessmentQuestionByDialogId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/AddAssessmentQuestionByDialogId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                        if (table.Rows[0][0].ToString() == "Already")
                        {
                            ViewBag.errormsg = "Assessment Question Already Exists";
                            return View();
                        }
                        if (objModel.QuestionType == "Objective")
                        {
                            int questionid = Convert.ToInt32(table.Rows[0][0].ToString());
                            if (objModel.OptionAudioUrl1 != null)
                            {
                                string filepath = Server.MapPath("../../DialogAssessmentQuestionsAudio/");
                                objModel.OptionAudioUrl1.SaveAs(filepath + questionid.ToString() + "_questionoption1.mp3");
                            }
                            if (objModel.OptionAudioUrl2 != null)
                            {
                                string filepath = Server.MapPath("../../DialogAssessmentQuestionsAudio/");
                                objModel.OptionAudioUrl2.SaveAs(filepath + questionid.ToString() + "_questionoption2.mp3");
                            }
                            if (objModel.OptionAudioUrl3 != null)
                            {
                                string filepath = Server.MapPath("../../DialogAssessmentQuestionsAudio/");
                                objModel.OptionAudioUrl3.SaveAs(filepath + questionid.ToString() + "_questionoption3.mp3");
                            }
                            return RedirectToAction("DialogAssessmentQuestions", "CourseContent", new { dialogid = objModel.DialogId });
                        }
                        else
                        {
                            return RedirectToAction("DialogAssessmentQuestions", "CourseContent", new { dialogid = objModel.DialogId });
                        }
                    }
                }


                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult UpdateDialogAssessmentQuestions(int questionId, int dialogId)
        {
            try
            {
                DialogAssessmentQuestionsModel objModel = new DialogAssessmentQuestionsModel();
                string json = "{\"QuestionId\":\"" + questionId + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetDialogAssessmentQuestionByQuestionId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetDialogAssessmentQuestionByQuestionId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                        objModel.QuestionId = Convert.ToInt32(table.Rows[0]["QuestionId"].ToString());
                        objModel.DialogId = dialogId;
                        objModel.Question = table.Rows[0]["Question"].ToString();
                        objModel.QuestionType = table.Rows[0]["QuestionType"].ToString();

                        objModel.OptionCorrectAnswer = table.Rows[0]["OptionCorrectAnswer"].ToString();
                        objModel.FillAnsText = table.Rows[0]["FillAnswerText"].ToString();
                        objModel.TrueFalseTypes = table.Rows[0]["TrueFalseAnswer"].ToString();

                        objModel.ObjOpt1txt = table.Rows[0]["OptionText1"].ToString();
                        objModel.ObjOpt2txt = table.Rows[0]["OptionText2"].ToString();
                        objModel.ObjOpt3txt = table.Rows[0]["OptionText3"].ToString();
                        objModel.OptionAudio1 = table.Rows[0]["OptionAudio1"].ToString();
                        objModel.OptionAudio2 = table.Rows[0]["OptionAudio2"].ToString();
                        objModel.OptionAudio3 = table.Rows[0]["OptionAudio3"].ToString();
                        return View(objModel);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateDialogAssessmentQuestions(DialogAssessmentQuestionsModel objModel)
        {
            try
            {
                string json = "{\"QuestionId\":\"" + objModel.QuestionId + "\",\"Question\":\"" + objModel.Question + "\",\"OptionCorrectAnswer\":\"" + objModel.OptionCorrectAnswer + "\",\"QuestionType\":\"" + objModel.QuestionType + "\",\"FillAnsText\":\"" + objModel.FillAnsText + "\",\"TrueFalseType\":\"" + objModel.TrueFalseType + "\",\"ObjOpt1txt\":\"" + objModel.ObjOpt1txt + "\",\"ObjOpt2txt\":\"" + objModel.ObjOpt2txt + "\",\"ObjOpt3txt\":\"" + objModel.ObjOpt3txt + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/UpdateDialogAssessmentQuestion");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/UpdateDialogAssessmentQuestion");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                        if (table.Rows[0][0].ToString() == "error")
                        {
                            ViewBag.errormsg = "An error occurred, please try again.";
                            return View();
                        }
                        if (objModel.QuestionType == "Objective")
                        {
                            int questionid = Convert.ToInt32(table.Rows[0][0].ToString());
                            if (objModel.OptionAudioUrl1 != null)
                            {
                                string filepath = Server.MapPath("../../DialogAssessmentQuestionsAudio/");
                                objModel.OptionAudioUrl1.SaveAs(filepath + questionid.ToString() + "_questionoption1.mp3");
                            }
                            if (objModel.OptionAudioUrl2 != null)
                            {
                                string filepath = Server.MapPath("../../DialogAssessmentQuestionsAudio/");
                                objModel.OptionAudioUrl2.SaveAs(filepath + questionid.ToString() + "_questionoption2.mp3");
                            }
                            if (objModel.OptionAudioUrl3 != null)
                            {
                                string filepath = Server.MapPath("../../DialogAssessmentQuestionsAudio/");
                                objModel.OptionAudioUrl3.SaveAs(filepath + questionid.ToString() + "_questionoption3.mp3");
                            }
                            return RedirectToAction("DialogAssessmentQuestions", "CourseContent", new { dialogid = objModel.DialogId });
                        }
                        else
                        {
                            return RedirectToAction("DialogAssessmentQuestions", "CourseContent", new { dialogid = objModel.DialogId });
                        }
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult ViewDialogAssessmentQuestionsInfo(int QuestionId = 0)
        {
            try
            {
                if (QuestionId != 0)
                {
                    string json = "{\"QuestionId\":\"" + QuestionId + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetDialogAssessmentQuestionByQuestionId");
                    //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetDialogAssessmentQuestionByQuestionId");
                    string sb = JsonConvert.SerializeObject(data);
                    request.Method = "POST";
                    Byte[] bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                    using (response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).", response.StatusCode,
                            response.StatusDescription));
                        Stream responseStream = response.GetResponseStream();
                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            string result = sr.ReadToEnd();
                            //int id = Convert.ToInt32(result);

                            var table = JsonConvert.DeserializeObject<DataTable>(result);
                            DialogAssessmentQuestionsModel objModel = new DialogAssessmentQuestionsModel();
                            objModel.QuestionId = Convert.ToInt32(table.Rows[0]["QuestionId"].ToString());
                            objModel.Question = table.Rows[0]["Question"].ToString();
                            objModel.QuestionType = table.Rows[0]["QuestionType"].ToString();

                            objModel.OptionCorrectAnswer = table.Rows[0]["OptionCorrectAnswer"].ToString();
                            objModel.FillAnsText = table.Rows[0]["FillAnswerText"].ToString();
                            objModel.TrueFalseTypes = table.Rows[0]["TrueFalseAnswer"].ToString();

                            objModel.ObjOpt1txt = table.Rows[0]["OptionText1"].ToString();
                            objModel.ObjOpt2txt = table.Rows[0]["OptionText2"].ToString();
                            objModel.ObjOpt3txt = table.Rows[0]["OptionText3"].ToString();
                            objModel.OptionAudio1 = table.Rows[0]["OptionAudio1"].ToString();
                            objModel.OptionAudio2 = table.Rows[0]["OptionAudio2"].ToString();
                            objModel.OptionAudio3 = table.Rows[0]["OptionAudio3"].ToString();
                            if (objModel.OptionAudioUrl1 != null)
                            {
                                string filepath = Server.MapPath("../../DialogAssessmentQuestionsAudio/");
                                objModel.OptionAudioUrl1.SaveAs(filepath + objModel.DialogId.ToString() + "_audio_" + objModel.OptionAudioUrl1.FileName);
                            }
                            if (objModel.OptionAudioUrl2 != null)
                            {
                                string filepath = Server.MapPath("../../DialogAssessmentQuestionsAudio/");
                                objModel.OptionAudioUrl2.SaveAs(filepath + objModel.DialogId.ToString() + "_audio_" + objModel.OptionAudioUrl2.FileName);
                            }
                            if (objModel.OptionAudioUrl3 != null)
                            {
                                string filepath = Server.MapPath("../../DialogAssessmentQuestionsAudio/");
                                objModel.OptionAudioUrl3.SaveAs(filepath + objModel.DialogId.ToString() + "_audio_" + objModel.OptionAudioUrl3.FileName);
                            }
                            return View(objModel);
                            //return RedirectToAction("AddCourse", "Courses", new { UnitId = UnitId });
                        }
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        public JsonResult DeleteDialogAssessmentQuestionsStatus(int QuestionId, string softDelete)
        {
            try
            {
                string json = "{\"QuestionId\":\"" + QuestionId + "\",\"softDelete\":\"" + softDelete + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/DeleteDialogAssessmentQuestion");
                request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/DeleteDialogAssessmentQuestion");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        return Json(result.ToString(), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        //=====================================Dialog Assements Question Ends=======================================================================================================//


        //=====================================Vocab Assements Qurestion Starts==============================================================================================//    

        public ActionResult VocabAssessmentQuestions(int VocabularyId = 0)
        {
            try
            {
                string json = "{\"VocabularyId\":\"" + VocabularyId + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetAllVocabQuestion");
                //  request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetAllVocabQuestion");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                        if (table.Rows[0][0].ToString() == "No Data")
                        {
                            List<VocabAssessmentQuestionsModel> vocabquestion = new List<VocabAssessmentQuestionsModel>();
                            return View(vocabquestion);
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(result);
                            List<VocabAssessmentQuestionsModel> vocabquestion = jsonArray.ToObject<List<VocabAssessmentQuestionsModel>>();
                            return View(vocabquestion.ToList());
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                List<UsersListModel> users = new List<UsersListModel>();
                return View(users);
            }
            return View();

        }

        [Authorize]
        public ActionResult AddVocabAssessmentQuestions(int questionId = 0, int VocabularyId = 0)
        {
            try
            {
                VocabAssessmentQuestionsModel objModel = new VocabAssessmentQuestionsModel();
                objModel.VocabularyId = VocabularyId;
                objModel.QuestionId = questionId;
                JavaScriptSerializer js = new JavaScriptSerializer();//sjy
                string WordCount = Convert.ToString(TempData["WordCount"]);
                VocabAssessmentQuestionsModel[] VocabAssessmentQuestionsModel = js.Deserialize<VocabAssessmentQuestionsModel[]>(WordCount);
                int WordCountvalue = Convert.ToInt32(VocabAssessmentQuestionsModel.Select(x => x.WordCount).FirstOrDefault().ToString());
                ViewData["setword"] = WordCountvalue;//sjy
                return View(objModel);

            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult AddVocabAssessmentQuestions(VocabAssessmentQuestionsModel objModel)
        {

            //  string json = "{\"Alert\":\"" + " hello" + "\"," + "\"ClientMessageID\":\"" + "5" + "\"}";

            string json = "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"question\":\"" + objModel.Question + "\",\"SelectSet\":\"" + objModel.SelectSet + "\",\"optionsA\":\"" + objModel.OptionsA + "\",\"optionsB\":\"" + objModel.OptionsB + "\",\"optionsC\":\"" + objModel.OptionsC + "\",\"optionsD\":\"" + objModel.OptionsD + "\",\"correctAnswer\":\"" + objModel.CorrectAnswer + "\"}";
            var data = javaScriptSerializer.DeserializeObject(json);
            request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddVocabQuestionByVocabId");
            //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/AddVocabQuestionByVocabId");
            string sb = JsonConvert.SerializeObject(data);
            request.Method = "POST";
            Byte[] bt = Encoding.UTF8.GetBytes(sb);
            Stream st = request.GetRequestStream();
            st.Write(bt, 0, bt.Length);
            st.Close();
            using (response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format(
                    "Server error (HTTP {0}: {1}).", response.StatusCode,
                    response.StatusDescription));
                Stream responseStream = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(responseStream))
                {
                    string result = sr.ReadToEnd();
                    var table = JsonConvert.DeserializeObject<DataTable>(result);
                    if (table.Rows[0][0].ToString() == "Please choose a different vocab question ! This one is already in use")
                    {
                        ViewBag.Result = table.Rows[0][0].ToString();
                        List<DialogAssessmentQuestionsModel> emptyModel = new List<DialogAssessmentQuestionsModel>();
                        return View();
                    }
                    else
                    {
                        int QuestionId = Convert.ToInt32(table.Rows[0]["Message"].ToString());
                        if (objModel.Image != null)
                        {
                            string filepath = Server.MapPath("../../VocabQuestionImage/");
                            objModel.Image.SaveAs(filepath + QuestionId.ToString() + "_vocabquestionimage.jpg");
                        }
                        if (objModel.optaud1 != null)
                        {
                            string filepath = Server.MapPath("../../VocabQuestionAudio/");
                            objModel.optaud1.SaveAs(filepath + QuestionId.ToString() + "_vocaboptionA.mp3");
                        }
                        if (objModel.optaud2 != null)
                        {
                            string filepath = Server.MapPath("../../VocabQuestionAudio/");
                            objModel.optaud2.SaveAs(filepath + QuestionId.ToString() + "_vocaboptionB.mp3");
                        }
                        if (objModel.optaud3 != null)
                        {
                            string filepath = Server.MapPath("../../VocabQuestionAudio/");
                            objModel.optaud3.SaveAs(filepath + QuestionId.ToString() + "_vocaboptionC.mp3");
                        }
                        if (objModel.optaud4 != null)
                        {
                            string filepath = Server.MapPath("../../VocabQuestionAudio/");
                            objModel.optaud4.SaveAs(filepath + QuestionId.ToString() + "_vocaboptionD.mp3");
                        }
                    }
                    return RedirectToAction("VocabAssessmentQuestions", "CourseContent", new { dialogid = objModel.VocabularyId });
                }
            }

        }

        public ActionResult UpdateVocabAssessmentQuestions(int questionId = 0, int VocabularyId = 0)
        {
            try
            {
                VocabAssessmentQuestionsModel objModel = new VocabAssessmentQuestionsModel();

                string json = "{\"QuestionId\":\"" + questionId + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetVocabQuestionByQuestionId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetVocabQuestionByQuestionId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                        objModel.SelectSet = Convert.ToInt32(table.Rows[0]["SelectSet"].ToString());
                        objModel.Question = table.Rows[0]["Question"].ToString();
                        objModel.ImageURL = table.Rows[0]["ImageURL"].ToString();
                        objModel.CorrectAnswer = table.Rows[0]["CorrectAnswer"].ToString();
                        objModel.QuestionId = Convert.ToInt32(table.Rows[0]["QuestionId"].ToString());
                        objModel.VocabularyId = VocabularyId;
                        objModel.OptionsA = table.Rows[0]["OptionsA"].ToString();
                        objModel.OptionsB = table.Rows[0]["OptionsB"].ToString();
                        objModel.OptionsC = table.Rows[0]["OptionsC"].ToString();
                        objModel.OptionsD = table.Rows[0]["OptionsD"].ToString();
                        objModel.Option1AudioUrl = table.Rows[0]["Option1AudioUrl"].ToString();
                        objModel.Option2AudioUrl = table.Rows[0]["Option2AudioUrl"].ToString();
                        objModel.Option3AudioUrl = table.Rows[0]["Option3AudioUrl"].ToString();
                        objModel.Option4AudioUrl = table.Rows[0]["Option4AudioUrl"].ToString();
                        return View(objModel);
                    }
                }
                return View();
            }

            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult UpdateVocabAssessmentQuestions(VocabAssessmentQuestionsModel objModel)
        {
            try
            {
                string json = "{\"QuestionId\":\"" + objModel.QuestionId + "\",\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"SelectSet\":\"" + objModel.SelectSet + "\",\"Question\":\"" + objModel.Question + "\",\"OptionsA\":\"" + objModel.OptionsA + "\",\"OptionsB\":\"" + objModel.OptionsB + "\",\"OptionsC\":\"" + objModel.OptionsC + "\",\"OptionsD\":\"" + objModel.OptionsD + "\",\"CorrectAnswer\":\"" + objModel.CorrectAnswer + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/UpdateVocabQuestion");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/UpdateVocabQuestion");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                        string tablemsg = table.Rows[0][0].ToString();
                        if (tablemsg == "VocabQuestion updated successfully")
                        {
                            int QuestionId = objModel.QuestionId;
                            if (objModel.Image != null)
                            {
                                string filepath = Server.MapPath("../../VocabQuestionImage/");
                                objModel.Image.SaveAs(filepath + QuestionId.ToString() + "_vocabquestionimage.jpg");
                            }
                            if (objModel.optaud1 != null)
                            {
                                string filepath = Server.MapPath("../../VocabQuestionAudio/");
                                objModel.optaud1.SaveAs(filepath + QuestionId.ToString() + "_vocaboptionA.mp3");
                            }
                            if (objModel.optaud2 != null)
                            {
                                string filepath = Server.MapPath("../../VocabQuestionAudio/");
                                objModel.optaud2.SaveAs(filepath + QuestionId.ToString() + "_vocaboptionB.mp3");
                            }
                            if (objModel.optaud3 != null)
                            {
                                string filepath = Server.MapPath("../../VocabQuestionAudio/");
                                objModel.optaud3.SaveAs(filepath + QuestionId.ToString() + "_vocaboptionC.mp3");
                            }
                            if (objModel.optaud4 != null)
                            {
                                string filepath = Server.MapPath("../../VocabQuestionAudio/");
                                objModel.optaud4.SaveAs(filepath + QuestionId.ToString() + "_vocaboptionD.mp3");
                            }

                        }
                        return RedirectToAction("VocabAssessmentQuestions", "CourseContent", new { VocabularyId = objModel.VocabularyId });

                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                List<VocabAssessmentQuestionsModel> users = new List<VocabAssessmentQuestionsModel>();
                return View(users);
            }

        }
        [Authorize]
        public ActionResult ViewVocabAssessmentQuestionsInfo(int QuestionId = 0)
        {
            try
            {
                if (QuestionId != 0)
                {
                    string json = "{\"QuestionId\":\"" + QuestionId + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetVocabQuestionByQuestionId");
                    //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetVocabQuestionByQuestionId");
                    string sb = JsonConvert.SerializeObject(data);
                    request.Method = "POST";
                    Byte[] bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                    using (response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).", response.StatusCode,
                            response.StatusDescription));
                        Stream responseStream = response.GetResponseStream();
                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            string result = sr.ReadToEnd();
                            //int id = Convert.ToInt32(result);

                            var table = JsonConvert.DeserializeObject<DataTable>(result);
                            VocabAssessmentQuestionsModel objModel = new VocabAssessmentQuestionsModel();
                            objModel.SelectSet = Convert.ToInt32(table.Rows[0]["SelectSet"].ToString());
                            objModel.Question = table.Rows[0]["Question"].ToString();
                            objModel.ImageURL = table.Rows[0]["ImageURL"].ToString();
                            objModel.CorrectAnswer = table.Rows[0]["CorrectAnswer"].ToString();
                            objModel.QuestionId = Convert.ToInt32(table.Rows[0]["QuestionId"].ToString());
                            // objModel.VocabularyId = VocabularyId;
                            objModel.OptionsA = table.Rows[0]["OptionsA"].ToString();
                            objModel.OptionsB = table.Rows[0]["OptionsB"].ToString();
                            objModel.OptionsC = table.Rows[0]["OptionsC"].ToString();
                            objModel.OptionsD = table.Rows[0]["OptionsD"].ToString();
                            if (objModel.Image != null)
                            {
                                string filepath = Server.MapPath("../../DialogAssessmentQuestionsAudio/");
                                objModel.Image.SaveAs(filepath + objModel.QuestionId.ToString() + "audio" + objModel.Image.FileName);
                            }

                            return View(objModel);
                            //return RedirectToAction("AddCourse", "Courses", new { UnitId = UnitId });
                        }
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public JsonResult DeleteVocabAssessmentQuestions(int QuestionId, string softDelete)
        {
            try
            {
                string json = "{\"QuestionId\":\"" + QuestionId + "\",\"softDelete\":\"" + softDelete + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/DeleteVocabQuestionByQuestionId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetVocabQuestionByQuestionId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        return Json(result.ToString(), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("VocabAssessmentQuestions", JsonRequestBehavior.AllowGet);
            }
        }
        //snjy
        [Authorize]
        public ActionResult Viewassismentquestioninfobyset(int SelectSet = 0)
        {
            try
            {
                if (SelectSet != 0)
                {
                    string json = "{\"SelectSet\":\"" + SelectSet + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetVocabQuestionBySet");
                    // request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetVocabQuestionBySet");
                    string sb = JsonConvert.SerializeObject(data);
                    request.Method = "POST";
                    Byte[] bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                    using (response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).", response.StatusCode,
                            response.StatusDescription));
                        Stream responseStream = response.GetResponseStream();
                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            string result = sr.ReadToEnd();
                            //int id = Convert.ToInt32(result);

                            var table = JsonConvert.DeserializeObject<DataTable>(result);
                            VocabAssessmentQuestionsModel objModel = new VocabAssessmentQuestionsModel();
                            objModel.Question = table.Rows[0]["Question"].ToString();
                            objModel.ImageURL = table.Rows[0]["ImageURL"].ToString();
                            objModel.CorrectAnswer = table.Rows[0]["CorrectAnswer"].ToString();
                            objModel.QuestionId = Convert.ToInt32(table.Rows[0]["QuestionId"].ToString());
                            // objModel.VocabularyId = VocabularyId;
                            objModel.OptionsA = table.Rows[0]["OptionsA"].ToString();
                            objModel.OptionsB = table.Rows[0]["OptionsB"].ToString();
                            objModel.OptionsC = table.Rows[0]["OptionsC"].ToString();
                            objModel.OptionsD = table.Rows[0]["OptionsD"].ToString();
                            if (objModel.Image != null)
                            {
                                string filepath = Server.MapPath("../../DialogAssessmentQuestionsAudio/");
                                objModel.Image.SaveAs(filepath + objModel.QuestionId.ToString() + "audio" + objModel.Image.FileName);
                            }

                            return View(objModel);
                            //return RedirectToAction("AddCourse", "Courses", new { UnitId = UnitId });
                        }
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        //=====================================Vocab Assements Qurestion Ends======================================================================== Anu======================//    

    }
}
//===============================================================//
