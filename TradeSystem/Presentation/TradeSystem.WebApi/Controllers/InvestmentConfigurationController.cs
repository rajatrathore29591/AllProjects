using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradeSystem.Framework.Entities;
using TradeSystem.Utils.Models;

namespace TradeSystem.WebApi.Controllers
{
    public class InvestmentConfigurationController : BaseController
    {
       
        //The URL of the WEB API Service
        string url = WebConfigurationManager.AppSettings["url"];

        /// <summary>
        /// Constructor of Investment Configuration Controller 
        /// </summary>
        public InvestmentConfigurationController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        //For Redirect Create New Investment
        public ActionResult Index()
        {
            return RedirectToAction("CreateNewInvestment");
        }

        /// <summary>
        /// Get method for add product
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> CreateNewInvestment()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            ViewBag.Country = "";
            ViewBag.Investments = "";
            ViewBag.UserName = "";
            Session["ByCountry"] = "";
            Session["ByUserName"] = "";
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/customer/GetAllCountry");
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                ViewBag.Country = JsonConvert.DeserializeObject<List<SelectListModel>>(responseData);
                System.Web.HttpRuntime.Cache["Country"] = ViewBag.Country;
            }

            HttpResponseMessage customerResponse = await client.GetAsync(url + "/customer/GetAllCustomerSelectList");
            if (customerResponse.StatusCode == HttpStatusCode.OK)
            {

                new List<SelectListModel>();
                var responseData = customerResponse.Content.ReadAsStringAsync().Result;

                ViewBag.UserName = JsonConvert.DeserializeObject<List<SelectListModel>>(responseData);
                System.Web.HttpRuntime.Cache["UserName"] = ViewBag.UserName;
            }

            HttpResponseMessage response = await client.GetAsync(url + "/product/GetProductSelectList");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseData = response.Content.ReadAsStringAsync().Result;
                ViewBag.Investments = JsonConvert.DeserializeObject<List<SelectListModel>>(responseData);
                System.Web.HttpRuntime.Cache["Investments"] = ViewBag.Investments;
            }
            ProductDataModel ProductData = new ProductDataModel();
            ProductData.DurationWithdraw = "DurationWithdrawCheck";
            return View(ProductData);
        }

        /// <summary>
        /// Post method for add Product
        /// </summary>
        /// <param name="ProductData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateNewInvestment(ProductDataModel productData, HttpPostedFileBase file)
        {
            productData.DurationWithdraw = Request.Form["DurationWithdrawCheck"];

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            if (productData.ByCountry != null)
            {
                Session["ByCountry"] = productData.ByCountry;
            }
            if (productData.ByUserName != null)
            {
                Session["ByUserName"] = productData.ByUserName;
            }
            if (file != null)
            {

                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Pictures/"), pic);
                // file is uploaded
                file.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] bytes = ms.GetBuffer();
                    string base64String = System.Convert.ToBase64String(bytes);
                    productData.ImageBase64 = base64String;
                }
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

            }
            string myjson = JsonConvert.SerializeObject(productData);
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/product/Addproduct", productData);

            if (responseMessage.IsSuccessStatusCode)
            {
                ProductDataModel obj = new ProductDataModel();
                return RedirectToAction("Inventory", "InventoryManagement");
            }
            return RedirectToAction("Error");
        }

    }
}