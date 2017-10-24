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
    public class RoleMapping
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();


        public Stream GetAllMenu(Stream objStream)
        {
            try
            {
                Menu menu = new Menu();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);

                var menuData = (from data in icanSpeakContext.Menus
                                select data).ToList();

                if (menuData.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(menuData, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    var js = JsonConvert.SerializeObject("No data", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAllMenu");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream UserRoleMapping(Stream objStream)
        {
            try
            {

                RoleMapper roleMapper = new RoleMapper();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                // requestString = "{\"userId\":\"2\",\"menuId\":\"2,3,4,5\"}";
               
                var param = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int userId = Convert.ToInt32(param["userId"]);

                var result = (from data in icanSpeakContext.RoleMappers
                              where data.UserId == userId
                              select data).ToList();

                Dictionary<string, string> dbItems = new Dictionary<string, string>();

                List<String> menuId = new List<String>(param["menuId"].Split(','));

              

                if (result.Count > 0)
                {
                    icanSpeakContext.RoleMappers.DeleteAllOnSubmit(result);
                    icanSpeakContext.SubmitChanges();

                    foreach (var data in menuId)
                    {
                        if (!string.IsNullOrEmpty(data))
                        {
                            RoleMapper roleMapper1 = new RoleMapper();
                            roleMapper1.UserId = Convert.ToInt32(param["userId"]);
                            roleMapper1.MenuId = Convert.ToInt32(data);
                            icanSpeakContext.RoleMappers.InsertOnSubmit(roleMapper1);
                            icanSpeakContext.SubmitChanges();
                        }
                        
                    }

                    var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));

                }
                else
                {

                    foreach (var data in menuId)
                    {
                        RoleMapper roleMapper1 = new RoleMapper();
                        roleMapper1.UserId = Convert.ToInt32(param["userId"]);
                        roleMapper1.MenuId = Convert.ToInt32(data);
                        icanSpeakContext.RoleMappers.InsertOnSubmit(roleMapper1);
                        icanSpeakContext.SubmitChanges();

                    }


                    var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }


            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "RoleMapping");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }


        }

        public Stream GetUserRoleByUserId(Stream objStream)
        {
            try
            {
                RoleMapper roleMapper = new RoleMapper();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                requestString = "{\"userId\":\"2\"}";
                //  requestString = "{\"helpId\":\"1\",\"title\":\"HelpContent\",\"screenName\":\"Profile\",\"description\":\"This is help content related to profile\"}";
                // requestString = "{\"helpId\":\"0\"}";
                var param = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from data in icanSpeakContext.RoleMappers
                              where data.UserId == Convert.ToInt32(param["userId"])
                              select new { data.MenuId }).ToList();



                if (result.Count > 0)
                {

                    var js = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    var js = JsonConvert.SerializeObject("No Role", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "RoleMapping");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }


        }

        public Stream GetMenuByUserId(Stream objStream)
        {
            try
            {
                RoleMapper roleMapper = new RoleMapper();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

               //requestString = "{\"userId\":\"56\"}";
                //  requestString = "{\"helpId\":\"1\",\"title\":\"HelpContent\",\"screenName\":\"Profile\",\"description\":\"This is help content related to profile\"}";
                // requestString = "{\"helpId\":\"0\"}";
                var param = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var menuData = (from data in icanSpeakContext.RoleMappers
                                join menuItem in icanSpeakContext.Menus on data.MenuId equals menuItem.MenuId
                                where data.UserId == Convert.ToInt32(param["userId"]) && menuItem.IsActivce == true
                                select new { menuItem.MenuId, menuItem.DisplayName, menuItem.URL }).ToList();

                var js = JsonConvert.SerializeObject(menuData, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetMenuByUserId");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }


    }
}