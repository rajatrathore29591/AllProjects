using iCanSpeakServices.HelperClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using System.Data;
using System.Reflection;
using System.ServiceModel.Web;
using System.Data.Objects.SqlClient;
using System.Globalization;
namespace iCanSpeakServices.ServiceManager
{
    public class UserAccount
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataSet ds = new DataSet();
        DataTable Table = new DataTable();
        DataTable LoginType = new DataTable();

        public Stream UserRegistration(Stream objStream)
        {
            DataSet ds = new DataSet("Result");
            try
            {

                User objUser = new User();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var userRegistration = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var emailCheck = icanSpeakContext.Users.Any(email => email.Email == userRegistration["email"]);
                if (emailCheck == true)
                {
                    Table = Service.Message("emailexist", "0");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));

                }
                else
                {
                    int age = Convert.ToInt32(Math.Round(DateTime.Now.Subtract(Convert.ToDateTime(userRegistration["DOB"])).TotalDays * 0.00273790926));

                    var newPassword = Helper.RandomString(6);

                    objUser.FirstName = userRegistration["firstName"];
                    objUser.LastName = userRegistration["lastName"];
                    objUser.Age = age;
                    objUser.Email = userRegistration["email"];
                    objUser.Password = newPassword;
                    objUser.DOB = Convert.ToDateTime(userRegistration["DOB"]);
                    objUser.IsActive = true;
                    objUser.DeviceId = userRegistration["DeviceId"];
                    objUser.RoleId = 4;
                    objUser.AccessToken = Helper.RandomString(6);
                    objUser.IsSuggested = 0;
                    objUser.Username = userRegistration["Username"];
                    objUser.Country = userRegistration["Country"];
                    //objUser.City = userRegistration["City"];
                    //objUser.ZipCode = userRegistration["Zipcode"];
                    objUser.CreatedDate = System.DateTime.Now;
                    objUser.LoginStatus = "1";
                    objUser.FlashCard = 0;
                    objUser.MyScore = 0;
                    objUser.TotalScore = 0;
                    objUser.VocabularyBookmark = "";
                    objUser.DialogBookmark = "";
                    objUser.GrammerBookmark = "";
                    if (userRegistration["profilePicture"] != "")
                    {
                        objUser.ProfilePicture = userRegistration["profilePicture"];
                    }
                    objUser.Messages = 0;
                    objUser.BatchCount = 0;
                    if (userRegistration["gender"] == "Male")
                    {
                        if (age > 18)
                        {
                            objUser.Gender = "Male";
                        }
                        else
                        {
                            objUser.Gender = "Child Male";
                        }
                    }

                    else
                    {
                        if (age > 18)
                        {
                            objUser.Gender = "Female";
                        }
                        else
                        {
                            objUser.Gender = "Child Female";
                        }
                    }
                    icanSpeakContext.Users.InsertOnSubmit(objUser);
                    icanSpeakContext.SubmitChanges();
                    var userId = icanSpeakContext.Users.ToList().Max(U => U.UserId);

                    objUser.AboutMe = userId + "_aboutme.mp3";
                    if (userRegistration["profilePicture"] == "")
                    {
                        objUser.ProfilePicture = userId + "_profilepic.png";
                    }
                    icanSpeakContext.SubmitChanges();

                    if (!string.IsNullOrEmpty(userRegistration["aboutmeaudio"]))
                    {
                        string filePath = HostingEnvironment.MapPath("/AboutMeAudio/" + userId + "_aboutme.mp3");
                        byte[] bytes = System.Convert.FromBase64String(userRegistration["aboutmeaudio"]);
                        FileStream fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Close();
                    }

                    if (userRegistration["profilePicture"] == "")
                    {
                        if (!string.IsNullOrEmpty(userRegistration["imagebase64"]))
                        {
                            string filePath = HostingEnvironment.MapPath("/ProfilePictures/" + userId + "_profilepic.png");
                            byte[] bytes = System.Convert.FromBase64String(userRegistration["imagebase64"]);
                            FileStream fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                            fs.Write(bytes, 0, bytes.Length);
                            fs.Close();
                        }
                    }

                    WelcomeMail(userRegistration["email"], userRegistration["firstName"] + " " + userRegistration["lastName"], newPassword, userId.ToString());

                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable Profile = new DataTable();
                    Profile.TableName = "Profile";
                    Profile.Columns.Add("Username");
                    Profile.Columns.Add("Country");
                    Profile.Columns.Add("ProfilePicture");
                    Profile.Columns.Add("UserId");
                    //Profile.Columns.Add("GrammerBookmark");
                    //Profile.Columns.Add("VocabularyBookmark");
                    //Profile.Columns.Add("DialogBookmark");
                    //Profile.Columns.Add("BatchCount");
                    //Profile.Columns.Add("VocabId");                   
                    //Profile.Columns.Add("VocabSubId");

                    DataRow usernewrow = Profile.NewRow();
                    usernewrow["Username"] = userRegistration["Username"];
                    usernewrow["Country"] = userRegistration["Country"];                   
                    //usernewrow["GrammerBookmark"] = objUser.GrammerBookmark ;
                    //usernewrow["VocabularyBookmark"] = objUser.VocabularyBookmark ;
                    //usernewrow["DialogBookmark"] = objUser.DialogBookmark;
                    //usernewrow["BatchCount"] = objUser.BatchCount;
                    //usernewrow["VocabId"] = objUser.VocabId;
                    //usernewrow["VocabSubId"] = objUser.VocabSubId;
                    if (userRegistration["profilePicture"] != "")
                    {
                        usernewrow["ProfilePicture"] = Service.GetUrl() + "ProfilePictures/" + userRegistration["profilePicture"];
                    }
                    else
                    {
                        usernewrow["ProfilePicture"] = Service.GetUrl() + "ProfilePictures/" + userId + "_profilepic.png";
                    }
                    usernewrow["UserId"] = Service.Encrypt(userId.ToString());
                    Profile.Rows.Add(usernewrow);
                    Profile.AcceptChanges();
                    ds.Tables.Add(Table);
                    ds.Tables.Add(Profile);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));

                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "UserRegistraction");
                //  var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }

        public Stream SaveUserThirdParty(Stream objStream)
        {
            DataSet ds = new DataSet("Result");
            int age = 0;
            try
            {
                User objUser = new User();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                int userid = 0;
                // requestString = "{\"usertype\":\"3\",\"FacebookId\":\"\",\"TwitterId\":\"\",\"GoogleId\":\"114602483333102436937\",\"email\":\"dkapafdgbria@gmail.com\",\"DOB\":\"\",\"firstName\":\"Dheerendra\",\"lastName\":\"Kapariya\",\"gender\":\"male\",\"Username\":\"hgfndra\",\"Country\":\"\",\"DeviceId\":\"\",\"imagebase64\":\"/9j/4AAQSkZJRgABAQEAYABgAAD/4QCmRXhpZgAATU0AKgAAAAgABFEAAAQAAAABAAAAAFEBAAMAAAABAAEAAFECAAEAAABgAAAAPlEDAAEAAAABAAAAAAAAAACzs7O3t7e8vLzBwcHGxsbKysrPz8/U1NTZ2dnd3d3h4eHi4uLk5OTm5ubn5+fo6Ojq6urs7Ozu7u7w8PDx8fHz8/P19fX39/f5+fn6+vr7+/v9/f3///8AAAAAAAAAAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wAARCAGQAZADASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD2GiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACilALHABJPYUsxt7Nd13OsZ6hByx/CgBtKqM33VJ+grOm8QxocWlop/25jk/kKoy63qMvBuWQeiALj8qAOjFtMf+WTflS/ZZx/yzNci13cuctcSsfUuTTVnmX7ssg+jGgDrWhkX70bD8KZXOR6pfxfcu5voWJH61ei8R3PS5hinHckbW/Mf4UAatFRW+o6fd4VZDbyH+GX7p/H/GrEkTx43Dg9COhoAZRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAU9YwUaSRhHEv3nboKP3cULXFw2yFOp7sfQVzmo6nLqEmD8kC/cjHQf/AF6AL17r20GLTwY16GUj5m+npWIzM7FmYsx5JJyTSUUAFFFFABRRRQAUUUUAFX7HVrmx+VW8yHvE/I/D0qhRQB11vPb6hGXtSRIBloW6j6eoorlIpZIZVkico6nIYHpXT2F+mqpsbCXijkdpPce9AElFHQ4NFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAU+NVIZ5G2xINzsewpoBYgAZJ4FZuvXoUDT4W+VOZSO7en4UAUdU1Fr+fgbYE4jT0Hr9aoUUUAFFFFABRRRQAUUUUAFFFFABRRRQAU5HaORXRirKcgjsabRQB1lndrqdqZeBcxj96o7/7Qp1czZXcljdpPH1U8j+8O4rqZNjqk8JzDKNy+3tQAyiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigBzTrZWkt42MoNsYPdj0rkGZnYsxJZjkk9zW34in2tBZKeI13v/ALx/+t/OsOgAooooAKKKKACiiigAooooAKKKKACiiigAooooAK3tAufNjksHPX95F9e4/wA+9YNS207WtzHOn3kYMPegDqaKln2lxJHzHIodT7GoqACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKlt033CKemcmoqVn8myu5u6wkD6ngUAcxfXBur6afP33JH07fpVeiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKAOo02Xz9FiJ+9C5jP06ipqzvDr5ivIPVQ4/A8/zrRoAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAqLUW2aHc/wC2yL+ualqvq3/IDb3mXP5GgDmaKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooA1/DrY1Nk/vxMv9f6Vr1i+Hj/xO4B6hgf++TW1QAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABUGpjdoc3+zIp/pU9JcJ5ulXsf8A0z3/APfJzQByVFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAavh0Z1ZT/dRifyrZrM8OJ++upuyQ7fxJ/wDrVp0AFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAVNbbTNsb7rgofxqGlBKkEdQc0AcnLG0MzxN95GKn6imVreIIPL1Dz1HyXChx9e/8An3rJoAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiilALMFAyScACgDo9Fj8rSJJD1mkwPov/wBfNWqe0QtoILUf8sUAP+8eTTKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKAItQt/tmlOoGZYD5ie47iuWrsYpDFIrjtXP6zYizu98Y/cTfNGfT1H4UAZ1FFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABWtoFqJb03Eg/dW43n3bsP8+lZSqzuqKCWY4AHc11sNuLCyjtBgv8AflI7t6fhQArMXYsepOTSUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFK8Md7ataTHAbmN/7rUlFAHLXFvJazvDKu10OCKirq72zTVIAhIW6QYjkP8AEPQ1y80MkErRSoVdTgg0AMooooAKKKKACiiigAooooAKKKKACiitnSdJDhby7XEA5RD1kP8AhQBY0Wx+zRi/nX52H7hD/wChVeJJJJOSetOkkaV9x+gA6AUygAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKUAscAEn0FPkRLdd11NHAv8AtHk/QUAR0XFtDqcYjnBWYDCTKMkex9RVSbW7GHIghknb+852r/jVGbxBfSZEbJAvpEuP1oAq32nXOnybZ0+U/dcfdaqlSSzzTnMsryH1ZiajoAKKKKACiiigAooooAKVVZ3CIpZmOAAMk0lFAHR2GhC3UXF+hZuqwjkf8CP9KvSStK25j9AOgrmYNSvbbHlXUigdAWyPyPFaEXiKQ8XVtFKP7y/K1AGnRUUOoadc4CzmBz/DMOPz6VZeCRF3Y3IeQynIoAjooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiinyeVbReddyeUh6D+JvoKAGqrO21QSfQUlxPaWI/0qbMn/PKPlvx9KyLzXpZQYrRfs8J7j77fU1kdTk0Aa9z4guHBS1RbaP8A2eWP41lO7SOXdizHqWOSabRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFWbW/urNswTMg7rng/hVaigDobfXreb5b2Hy2/56xDj8RWj5YeLzYXWaI/xIc/nXG1NbXc9pJ5kErI3t0P1HegDqKKrWms215hLoC3mP8Ay0H3G+vpVySJ4iNw4PQjoaAGUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABTkRpGCqMk0scZkYgYAAyzHoBWVqWsgK1tYMQnR5u7fT0FAFy91SDTsxw7Z7odT/Cn+JrnJ7iW6mMs0hdz1JqKigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACtHT9Ymsh5Tjzrc9Y2PT6elZ1FAHYRmG6g8+0fen8Sn7yfUU2uXtbuaznE0DlWH5Eehrp7S7h1SMtEAlwoy8Xr7igBaKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKfHGZCeQqqMsx6AURxtK4Rep/SsjWNTVwbK1b9wp+dx/y0P8AhQAzVdW88fZbUlbZTye8h9T7VkUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABT4pXhlWSNirqcgjtTKKAOrsr2PVYsgBLtR86Do/uKfXKRSvDKssbFXU5BHauqtbpNTtjMgCzoP3qD/0Ie1AC0UUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFA5OBRSy3CafZtduAX+7Ep7t6/QUAU9YvvscBsoW/fSD96w/hH92ucp0jtLI0jsWdjkk9zTaACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACp7O7lsrlJ4jhl6jsR6GoKKAOx3xXECXUH+qk6j+6e4plYui6gLW4MMx/0ebhv9k9jW7LGYpCjdv1oAZRRRQAUUUUAFFFFABRRRQAUUUUAFFFFAD4ozLIqDv1PpXP6xfC8vMRn9xF8kY9ff8a2NRufsWlsQcTXGUX2Xua5agAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACuo0u6+3adsY5ntxg/7Sdvyrl6t6beGxvo5v4M7XHqp60AdHRUkyCOQhTlTyp9QajoAKKKKACiiigAooooAKKKKACnRoZJFQdScU2nNN9ls7i67xphf948CgDB1u6FzqTqh/dQ/u0/Dr+tZtFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFAHUaZP9r0hQTmS2Ow/wC6en+H4VNWN4fnEeo+Sx+SdTGfr2/z71tEFWKnqDg0AJRRRQAUUUUAFFFFABRRRQAVS12XytOt4B1lcyN9BwP51drH8RSbtU8oHiGNUH5Z/rQBk0UUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAOjkaKVJF4ZGDD6iuxuCrusqfdlUOPxFcZXVWMnnaNaueqFoz+B4/SgCSiiigAooooAKKKKACiiigCSBd08Y/2hXLajJ5upXL+srY+ma6y0wLlSegBP6VxZJZiT1JzQAlFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABXQ6E+7TLmP/nnIr/mMf0rnq3PDrf8fiesYb8j/wDXoA0qKKKACiiigAooooAKKKKAJYf+Wh/6Zt/KuMrs4f8Alr/1zb+VcZQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFbPhz/j7uR2+zt/MVjVs+HP+Pu5Pb7Ow/UUAatFFFABRRRQAUUUUAFFFFAE9pg3KqehBH6VxbAqxU9QcGuxgbbPGf8AaFcvqMfk6lcx+kjY+meKAKtFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABW54dX5b2T0jC/mf/AK1YddLo0DW+lSSOMG4cbR6qO9AFmiiigAooooAKKKKACiiigAqrqumnUAbu2H+kAfvIx/FjuPerVKrFWDKSCOhFAHIkEEgjBHUGkrrLq1tNQ5uEMc3/AD2jHX6jvWVP4eu0y0BS4T1Q4P5GgDIoqWW1uITiWCRMf3kIqKgAooooAKKKKACiiigAooooAKKKUIzDIUn6CgBKKd5cn9xvyo8uT+435UANop3lyf3G/Kjy5P7jflQA2ineXJ/cb8qPLk/uN+VADaKd5cn9xvyo8uT+435UANop3lyf3G/Kl8qT/nm35UAMoqX7NP8A88ZP++DR9mn/AOeMn/fBoAioqX7NP/zxk/74NH2af/njJ/3waAIqKl+zT/8APGT/AL4NH2af/njJ/wB8GgCKipfs0/8Azxk/74NH2af/AJ4yf98GgCKipfs0/wDzxk/74NH2af8A54yf98GgCKiphaXLfdt5T9ENPTTr2Q4W0nP/AGzNAFaitaLw9evgzeXAvrI3P5CtO202xssNg3Mw/icYUfQUAZul6OZttzdgpbDkA9ZPYe3vW1LIZGzgBQMKo6AUSSPK25zk/wAqZQAUUUUAFFFFABRRRQAUUUUAFFFFABSgkHIODSUUASrczr0kb8ead9tuP+en/joqCigCf7bcf89P/HRR9tuP+en/AI6KgooAn+23H/PT/wAdFH224/56f+OioKKAJ/ttx/z0/wDHRR9tuP8Anp/46KgooAn+23H/AD0/8dFH224/56f+OioKKAJ/ttx/z0/8dFH2y4P/AC0P5CoKKAJ/tc//AD0P5Cj7XP8A89D+QqCigCf7XP8A89D+Qo+1z/8APQ/kKgooAn+1z/8APQ/kKPtc/wDz0P5CoKKAJ/tc/wDz0P5Cj7XP/wA9D+QqCigCf7XP/wA9D+Qo+1z/APPQ/lUFFAE32qf/AJ6NR9qn/wCejVDRQBN9qn/56NR9qn/56NUNFAE32qf/AJ6NR9qn/wCejVDRQBN9qn/56NR9qn/56NUNFAE32qf/AJ6NR9qn/wCejVDRQBL9pmP/AC1b86QzynrI/wCdR0UABJJyaKKKACiiigAooooAKKKKAP/Z\"}";
                var userRegistration = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                //Removing empty whitespaces from keyname
                userRegistration = userRegistration.ToDictionary(x => x.Key.Trim(), x => x.Value);

                if (userRegistration["usertype"] == "1")
                {

                    var checkid = (from user in icanSpeakContext.Users
                                   where user.FacebookId == userRegistration["FacebookId"] &&
                                   user.Email == userRegistration["email"]
                                   select user
                             ).Any();


                    if (checkid == true)
                    {
                        var userId = (from user in icanSpeakContext.Users
                                      where user.FacebookId == userRegistration["FacebookId"] &&
                                       user.Email == userRegistration["email"]
                                      select user
                                     ).FirstOrDefault();

                        userId.LoginStatus = "1";
                        icanSpeakContext.SubmitChanges();

                        userid = userId.UserId;
                        Table = Service.Message("success", "1");
                        Table.AcceptChanges();
                        DataTable Profile = new DataTable();
                        Profile.TableName = "Profile";
                        Profile.Columns.Add("Username");
                        Profile.Columns.Add("Country");
                        Profile.Columns.Add("ProfilePicture");
                        Profile.Columns.Add("UserId");
                        Profile.Columns.Add("BatchCount");
                        Profile.Columns.Add("DialogBookmark");
                        Profile.Columns.Add("GrammerBookmark");
                        Profile.Columns.Add("VocabularyBookmark");
                        Profile.Columns.Add("VocabId");
                        Profile.Columns.Add("VocabSubId");
                        DataRow usernewrow = Profile.NewRow();
                        usernewrow["Username"] = userId.Username;
                        usernewrow["Country"] = userId.Country;
                        usernewrow["ProfilePicture"] = Service.GetUrl() + "ProfilePictures/" + userid + "_profilepic.png";
                        usernewrow["UserId"] = Service.Encrypt(userid.ToString());
                        usernewrow["BatchCount"] = userId.BatchCount;
                        if (userId.DialogBookmark != "")
                        {
                            usernewrow["DialogBookmark"] = Service.Encrypt(userId.DialogBookmark);
                        }
                        else
                        {
                            usernewrow["DialogBookmark"] = userId.DialogBookmark;
                        }
                        if (userId.GrammerBookmark != "")
                        {
                            usernewrow["GrammerBookmark"] = Service.Encrypt(userId.GrammerBookmark);
                        }
                        else
                        {
                            usernewrow["GrammerBookmark"] = userId.GrammerBookmark;
                        }
                        if (userId.VocabularyBookmark != "")
                        {
                            usernewrow["VocabularyBookmark"] = Service.Encrypt(userId.VocabularyBookmark);
                        }
                        else
                        {
                            usernewrow["VocabularyBookmark"] = userId.VocabularyBookmark;
                        }
                        if (userId.VocabId != "" && userId.VocabId != null)
                        {
                            usernewrow["VocabId"] = Service.Encrypt(userId.VocabId);
                        }
                        else
                        {
                            usernewrow["VocabId"] = userId.VocabId;
                        }
                        if (userId.VocabSubId != "" && userId.VocabSubId != null)
                        {
                            usernewrow["VocabSubId"] = Service.Encrypt(userId.VocabSubId);
                        }
                        else
                        {
                            usernewrow["VocabSubId"] = userId.VocabSubId;
                        }
                        Profile.Rows.Add(usernewrow);
                        Profile.AcceptChanges();
                        ds.Tables.Add(Table);
                        ds.Tables.Add(Profile);
                        string js1 = JsonConvert.SerializeObject(ds);
                        WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                        return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                    }
                    else
                    {
                        var emailCheck = icanSpeakContext.Users.Any(email => email.Email == userRegistration["email"]);
                        if (emailCheck == true)
                        {
                            //Table = Service.Message("emailexist", "0");
                            //Table.AcceptChanges();
                            //LoginType = Service.ResultMessage("LoginType","1");
                            //LoginType.AcceptChanges();
                            //ds.Tables.Add(Table);
                            //ds.Tables.Add(LoginType);
                            //string js1 = JsonConvert.SerializeObject(ds);
                            //WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                            //return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                            var result = (from user in icanSpeakContext.Users
                                          where user.Email == userRegistration["email"]
                                          select user).FirstOrDefault();
                            Table = Service.Message("emailexist", "0");
                            Table.AcceptChanges();
                            DataTable LoginType = new DataTable();
                            LoginType.TableName = "LoginType";
                            LoginType.Columns.Add("loginType");
                            DataRow usernewrow = LoginType.NewRow();
                            usernewrow["loginType"] = result.LoginType;
                            LoginType.Rows.Add(usernewrow);
                            LoginType.AcceptChanges();
                            ds.Tables.Add(Table);
                            ds.Tables.Add(LoginType);
                            string js1 = JsonConvert.SerializeObject(ds);
                            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                            return new MemoryStream(Encoding.UTF8.GetBytes(js1));

                        }
                        else
                        {
                            //string returnVal = "";

                            //if (userRegistration["DOB"] != null && userRegistration["DOB"] != "")
                            //if (userRegistration.TryGetValue("DOB ",out returnVal))
                            //{
                            //    age = Convert.ToInt32(Math.Round(DateTime.Now.Subtract(Convert.ToDateTime(userRegistration["DOB"])).TotalDays * 0.00273790926));
                            //}

                            /*
                            objUser.FirstName = userRegistration["firstName"];
                            objUser.LastName = userRegistration["lastName"];
                            objUser.Email = userRegistration["email"];
                            if (userRegistration["DOB"] != "")
                            {
                                objUser.DOB = Convert.ToDateTime(userRegistration["DOB"]);
                            }
                            objUser.IsActive = true;
                            objUser.DeviceId = userRegistration["DeviceId"];
                            objUser.RoleId = 4;
                            objUser.AccessToken = Helper.RandomString(6);
                            objUser.IsSuggested = 0;
                            objUser.Username = userRegistration["Username"];
                            objUser.Country = userRegistration["Country"];
                            objUser.CreatedDate = System.DateTime.Now;
                            objUser.Messages = 0;
                            objUser.BatchCount = 10;
                            objUser.LoginStatus = "1";
                            objUser.FlashCard = 0;
                            objUser.MyScore = 0;
                            objUser.TotalScore = 0;
                            objUser.LoginType = userRegistration["usertype"];
                            objUser.FacebookId = userRegistration["FacebookId"];
                             * */
                            // objUser.FirstName = (!(userRegistration.TryGetValue("firstName", out returnVal))?"":userRegistration["firstName"]);


                            objUser.FirstName = userRegistration["firstName"];
                            objUser.LastName = userRegistration["lastName"];
                            objUser.Email = userRegistration["email"];
                            if (userRegistration["DOB"] != "")
                            {
                                objUser.DOB = Convert.ToDateTime(userRegistration["DOB"]);
                            }
                            objUser.IsActive = true;
                            objUser.DeviceId = userRegistration["DeviceId"];
                            objUser.RoleId = 4;
                            objUser.AccessToken = Helper.RandomString(6);
                            objUser.IsSuggested = 0;
                            objUser.Username = userRegistration["Username"];
                            objUser.Country = userRegistration["Country"];
                            objUser.CreatedDate = System.DateTime.Now;
                            objUser.Messages = 0;
                            objUser.BatchCount = 10;
                            objUser.LoginStatus = "1";
                            objUser.FlashCard = 0;
                            objUser.MyScore = 0;
                            objUser.TotalScore = 0;
                            objUser.LoginType = userRegistration["usertype"];
                            objUser.FacebookId = userRegistration["FacebookId"];
                            objUser.DialogBookmark = "";
                            objUser.GrammerBookmark = "";
                            objUser.VocabularyBookmark = "";
                            objUser.VocabId = "";
                            objUser.VocabSubId = "";

                            if (userRegistration["gender"] == "Male")
                            {
                                if (age > 18)
                                {
                                    objUser.Gender = "Male";
                                }
                                else
                                {
                                    objUser.Gender = "Child Male";
                                }
                            }
                            else
                            {
                                if (age > 18)
                                {
                                    objUser.Gender = "Female";
                                }
                                else
                                {
                                    objUser.Gender = "Child Female";
                                }
                            }
                            icanSpeakContext.Users.InsertOnSubmit(objUser);
                            icanSpeakContext.SubmitChanges();
                            var userId = icanSpeakContext.Users.ToList().Max(U => U.UserId);
                            objUser.ProfilePicture = userId + "_profilepic.png";
                            icanSpeakContext.SubmitChanges();




                            if (!string.IsNullOrEmpty(userRegistration["imagebase64"]))
                            {
                                string filePath = HostingEnvironment.MapPath("/ProfilePictures/" + userId + "_profilepic.png");
                                byte[] bytes = System.Convert.FromBase64String(userRegistration["imagebase64"]);
                                FileStream fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                                fs.Write(bytes, 0, bytes.Length);
                                fs.Close();
                            }


                            ThirdPartyWelcomeMail(userRegistration["email"], userRegistration["firstName"] + " " + userRegistration["lastName"], userId.ToString());

                            Table = Service.Message("success", "1");
                            Table.AcceptChanges();
                            DataTable Profile = new DataTable();
                            Profile.TableName = "Profile";
                            Profile.Columns.Add("Username");
                            Profile.Columns.Add("Country");
                            Profile.Columns.Add("ProfilePicture");
                            Profile.Columns.Add("UserId");
                            Profile.Columns.Add("BatchCount");
                            Profile.Columns.Add("DialogBookmark");
                            Profile.Columns.Add("GrammerBookmark");
                            Profile.Columns.Add("VocabularyBookmark");
                            Profile.Columns.Add("VocabId");
                            Profile.Columns.Add("VocabSubId");
                            DataRow usernewrow = Profile.NewRow();
                            usernewrow["Username"] = userRegistration["Username"];
                            usernewrow["Country"] = userRegistration["Country"];
                            usernewrow["ProfilePicture"] = Service.GetUrl() + "ProfilePictures/" + userId + "_profilepic.png";
                            usernewrow["UserId"] = Service.Encrypt(userid.ToString());
                            usernewrow["BatchCount"] = objUser.BatchCount;
                            usernewrow["DialogBookmark"] = objUser.DialogBookmark;
                            usernewrow["GrammerBookmark"] = objUser.GrammerBookmark;
                            usernewrow["VocabularyBookmark"] = objUser.VocabularyBookmark;
                            usernewrow["VocabId"] = objUser.VocabId;
                            usernewrow["VocabSubId"] = objUser.VocabSubId;


                            Profile.Rows.Add(usernewrow);
                            Profile.AcceptChanges();
                            ds.Tables.Add(Table);
                            ds.Tables.Add(Profile);
                            string js1 = JsonConvert.SerializeObject(ds);
                            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                            return new MemoryStream(Encoding.UTF8.GetBytes(js1));

                        }
                    }
                }
                else if (userRegistration["usertype"] == "2")
                {
                    var checkid = (from user in icanSpeakContext.Users
                                   where user.TwitterId == userRegistration["TwitterId"]
                                   //&&
                                   //user.Email == userRegistration["email"]
                                   select user
                             ).Any();

                    if (checkid == true)
                    {
                        var userId = (from user in icanSpeakContext.Users
                                      where user.TwitterId == userRegistration["TwitterId"]
                                      //user.Email == userRegistration["email"]
                                      select user
                                     ).FirstOrDefault();

                        userId.LoginStatus = "1";
                        icanSpeakContext.SubmitChanges();

                        userid = userId.UserId;
                        Table = Service.Message("success", "1");
                        Table.AcceptChanges();
                        DataTable Profile = new DataTable();
                        Profile.TableName = "Profile";
                        Profile.Columns.Add("Username");
                        Profile.Columns.Add("Country");
                        Profile.Columns.Add("ProfilePicture");
                        Profile.Columns.Add("UserId");
                        Profile.Columns.Add("BatchCount");
                        Profile.Columns.Add("DialogBookmark");
                        Profile.Columns.Add("GrammerBookmark");
                        Profile.Columns.Add("VocabularyBookmark");
                        Profile.Columns.Add("VocabId");
                        Profile.Columns.Add("VocabSubId");
                        DataRow usernewrow = Profile.NewRow();
                        usernewrow["Username"] = userId.Username;
                        usernewrow["Country"] = userId.Country;
                        usernewrow["ProfilePicture"] = Service.GetUrl() + "ProfilePictures/" + userid + "_profilepic.png";
                        usernewrow["UserId"] = Service.Encrypt(userid.ToString());
                        usernewrow["BatchCount"] = userId.BatchCount;
                        if (userId.DialogBookmark != "")
                        {
                            usernewrow["DialogBookmark"] = Service.Encrypt(userId.DialogBookmark);
                        }
                        else
                        {
                            usernewrow["DialogBookmark"] = userId.DialogBookmark;
                        }
                        if (userId.GrammerBookmark != "")
                        {
                            usernewrow["GrammerBookmark"] = Service.Encrypt(userId.GrammerBookmark);
                        }
                        else
                        {
                            usernewrow["GrammerBookmark"] = userId.GrammerBookmark;
                        }
                        if (userId.VocabularyBookmark != "")
                        {
                            usernewrow["VocabularyBookmark"] = Service.Encrypt(userId.VocabularyBookmark);
                        }
                        else
                        {
                            usernewrow["VocabularyBookmark"] = userId.VocabularyBookmark;
                        }

                        if (userId.VocabId != "" && userId.VocabId != null)
                        {
                            usernewrow["VocabId"] = Service.Encrypt(userId.VocabId);
                        }
                        else
                        {
                            usernewrow["VocabId"] = userId.VocabId;
                        }
                        if (userId.VocabSubId != "" && userId.VocabSubId != null)
                        {
                            usernewrow["VocabSubId"] = Service.Encrypt(userId.VocabSubId);
                        }
                        else
                        {
                            usernewrow["VocabSubId"] = userId.VocabSubId;
                        }

                        Profile.Rows.Add(usernewrow);
                        Profile.AcceptChanges();
                        ds.Tables.Add(Table);
                        ds.Tables.Add(Profile);
                        string js1 = JsonConvert.SerializeObject(ds);
                        WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                        return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                    }
                    else
                    {
                        var emailCheck = false;
                        if (userRegistration["email"] != "")
                        {
                            emailCheck = icanSpeakContext.Users.Any(email => email.Email == userRegistration["email"]);
                        }
                        if (emailCheck == true)
                        {
                            //Table = Service.Message("emailexist", "0");
                            //Table.AcceptChanges();
                            //ds.Tables.Add(Table);
                            //string js1 = JsonConvert.SerializeObject(ds);
                            //WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                            //return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                            var result = (from user in icanSpeakContext.Users
                                          where user.Email == userRegistration["email"]
                                          select user).FirstOrDefault();
                            Table = Service.Message("emailexist", "0");
                            Table.AcceptChanges();
                            DataTable LoginType = new DataTable();
                            LoginType.TableName = "LoginType";
                            LoginType.Columns.Add("loginType");
                            DataRow usernewrow = LoginType.NewRow();
                            usernewrow["loginType"] = result.LoginType;
                            LoginType.Rows.Add(usernewrow);
                            LoginType.AcceptChanges();
                            ds.Tables.Add(Table);
                            ds.Tables.Add(LoginType);
                            string js1 = JsonConvert.SerializeObject(ds);
                            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                            return new MemoryStream(Encoding.UTF8.GetBytes(js1));

                        }
                        else
                        {


                            if (userRegistration["DOB"] != "")
                            {
                                age = Convert.ToInt32(Math.Round(DateTime.Now.Subtract(Convert.ToDateTime(userRegistration["DOB"])).TotalDays * 0.00273790926));
                            }

                            objUser.FirstName = userRegistration["firstName"];
                            objUser.LastName = userRegistration["lastName"];
                            objUser.Email = userRegistration["email"];
                            if (userRegistration["DOB"] != "")
                            {
                                objUser.DOB = Convert.ToDateTime(userRegistration["DOB"]);
                            }
                            objUser.IsActive = true;
                            objUser.DeviceId = userRegistration["DeviceId"];
                            objUser.RoleId = 4;
                            objUser.AccessToken = Helper.RandomString(6);
                            objUser.IsSuggested = 0;
                            objUser.Username = userRegistration["Username"];
                            objUser.Country = userRegistration["Country"];
                            objUser.CreatedDate = System.DateTime.Now;
                            objUser.Messages = 0;
                            objUser.BatchCount = 10;
                            objUser.LoginStatus = "1";
                            objUser.FlashCard = 0;
                            objUser.MyScore = 0;
                            objUser.TotalScore = 0;
                            objUser.LoginType = userRegistration["usertype"];
                            objUser.TwitterId = userRegistration["TwitterId"];
                            objUser.DialogBookmark = "";
                            objUser.GrammerBookmark = "";
                            objUser.VocabularyBookmark = "";
                            objUser.VocabId = "";
                            objUser.VocabSubId = "";
                            if (userRegistration["gender"] == "Male")
                            {
                                if (age > 18)
                                {
                                    objUser.Gender = "Male";
                                }
                                else
                                {
                                    objUser.Gender = "Child Male";
                                }
                            }
                            else
                            {
                                if (age > 18)
                                {
                                    objUser.Gender = "Female";
                                }
                                else
                                {
                                    objUser.Gender = "Child Female";
                                }
                            }
                            icanSpeakContext.Users.InsertOnSubmit(objUser);
                            icanSpeakContext.SubmitChanges();
                            var userId = icanSpeakContext.Users.ToList().Max(U => U.UserId);


                            objUser.ProfilePicture = userId + "_profilepic.png";

                            icanSpeakContext.SubmitChanges();




                            if (!string.IsNullOrEmpty(userRegistration["imagebase64"]))
                            {
                                string filePath = HostingEnvironment.MapPath("/ProfilePictures/" + userId + "_profilepic.png");
                                byte[] bytes = System.Convert.FromBase64String(userRegistration["imagebase64"]);
                                FileStream fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                                fs.Write(bytes, 0, bytes.Length);
                                fs.Close();
                            }


                            ThirdPartyWelcomeMail(userRegistration["email"], userRegistration["firstName"] + " " + userRegistration["lastName"], userId.ToString());

                            Table = Service.Message("success", "1");
                            Table.AcceptChanges();
                            DataTable Profile = new DataTable();
                            Profile.TableName = "Profile";
                            Profile.Columns.Add("Username");
                            Profile.Columns.Add("Country");
                            Profile.Columns.Add("ProfilePicture");
                            Profile.Columns.Add("UserId");
                            Profile.Columns.Add("BatchCount");
                            Profile.Columns.Add("DialogBookmark");
                            Profile.Columns.Add("GrammerBookmark");
                            Profile.Columns.Add("VocabularyBookmark");
                            Profile.Columns.Add("VocabId");
                            Profile.Columns.Add("VocabSubId");
                            DataRow usernewrow = Profile.NewRow();
                            usernewrow["Username"] = userRegistration["Username"];
                            usernewrow["Country"] = userRegistration["Country"];
                            usernewrow["ProfilePicture"] = Service.GetUrl() + "ProfilePictures/" + userId + "_profilepic.png";
                            usernewrow["UserId"] = Service.Encrypt(userid.ToString());
                            usernewrow["BatchCount"] = objUser.BatchCount;
                            usernewrow["DialogBookmark"] = objUser.DialogBookmark;
                            usernewrow["GrammerBookmark"] = objUser.GrammerBookmark;
                            usernewrow["VocabularyBookmark"] = objUser.VocabularyBookmark;
                            usernewrow["VocabId"] = objUser.VocabId;
                            usernewrow["VocabSubId"] = objUser.VocabSubId;
                            Profile.Rows.Add(usernewrow);
                            Profile.AcceptChanges();
                            ds.Tables.Add(Table);
                            ds.Tables.Add(Profile);
                            string js1 = JsonConvert.SerializeObject(ds);
                            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                            return new MemoryStream(Encoding.UTF8.GetBytes(js1));

                        }
                    }
                }
                else
                {
                    var checkid = (from user in icanSpeakContext.Users
                                   where user.GoogleId == userRegistration["GoogleId"] &&
                                   user.Email == userRegistration["email"]
                                   select user
                             ).Any();

                    if (checkid == true)
                    {
                        var userId = (from user in icanSpeakContext.Users
                                      where user.GoogleId == userRegistration["GoogleId"] &&
                                      user.Email == userRegistration["email"]
                                      select user
                                     ).FirstOrDefault();

                        userId.LoginStatus = "1";
                        icanSpeakContext.SubmitChanges();

                        userid = userId.UserId;
                        Table = Service.Message("success", "1");
                        Table.AcceptChanges();
                        DataTable Profile = new DataTable();
                        Profile.TableName = "Profile";
                        Profile.Columns.Add("Username");
                        Profile.Columns.Add("Country");
                        Profile.Columns.Add("ProfilePicture");
                        Profile.Columns.Add("UserId");
                        Profile.Columns.Add("BatchCount");
                        Profile.Columns.Add("DialogBookmark");
                        Profile.Columns.Add("GrammerBookmark");
                        Profile.Columns.Add("VocabularyBookmark");
                        Profile.Columns.Add("VocabId");
                        Profile.Columns.Add("VocabSubId");
                        DataRow usernewrow = Profile.NewRow();
                        usernewrow["Username"] = userId.Username;
                        usernewrow["Country"] = userId.Country;
                        usernewrow["ProfilePicture"] = Service.GetUrl() + "ProfilePictures/" + userid + "_profilepic.png";
                        usernewrow["UserId"] = Service.Encrypt(userid.ToString());
                        usernewrow["BatchCount"] = userId.BatchCount;
                        if (userId.DialogBookmark != "")
                        {
                            usernewrow["DialogBookmark"] = Service.Encrypt(userId.DialogBookmark);
                        }
                        else
                        {
                            usernewrow["DialogBookmark"] = userId.DialogBookmark;
                        }
                        if (userId.GrammerBookmark != "")
                        {
                            usernewrow["GrammerBookmark"] = Service.Encrypt(userId.GrammerBookmark);
                        }
                        else
                        {
                            usernewrow["GrammerBookmark"] = userId.GrammerBookmark;
                        }
                        if (userId.VocabularyBookmark != "")
                        {
                            usernewrow["VocabularyBookmark"] = Service.Encrypt(userId.VocabularyBookmark);
                        }
                        else
                        {
                            usernewrow["VocabularyBookmark"] = userId.VocabularyBookmark;
                        }
                        if (userId.VocabId != "" && userId.VocabId != null)
                        {
                            usernewrow["VocabId"] = Service.Encrypt(userId.VocabId);
                        }
                        else
                        {
                            usernewrow["VocabId"] = userId.VocabId;
                        }
                        if (userId.VocabSubId != "" && userId.VocabSubId != null)
                        {
                            usernewrow["VocabSubId"] = Service.Encrypt(userId.VocabSubId);
                        }
                        else
                        {
                            usernewrow["VocabSubId"] = userId.VocabSubId;
                        }
                        Profile.Rows.Add(usernewrow);
                        Profile.AcceptChanges();
                        ds.Tables.Add(Table);
                        ds.Tables.Add(Profile);
                        string js1 = JsonConvert.SerializeObject(ds);
                        WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                        return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                    }
                    else
                    {
                        var emailCheck = icanSpeakContext.Users.Any(email => email.Email == userRegistration["email"]);
                        if (emailCheck == true)
                        {
                            var result = (from user in icanSpeakContext.Users
                                          where user.Email == userRegistration["email"]
                                          select user).FirstOrDefault();
                            Table = Service.Message("emailexist", "0");
                            Table.AcceptChanges();
                            DataTable LoginType = new DataTable();
                            LoginType.TableName = "LoginType";
                            LoginType.Columns.Add("loginType");
                            DataRow usernewrow = LoginType.NewRow();
                            usernewrow["loginType"] = result.LoginType;
                            LoginType.Rows.Add(usernewrow);
                            LoginType.AcceptChanges();
                            ds.Tables.Add(Table);
                            ds.Tables.Add(LoginType);
                            string js1 = JsonConvert.SerializeObject(ds);
                            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                            return new MemoryStream(Encoding.UTF8.GetBytes(js1));

                        }
                        else
                        {


                            if (userRegistration["DOB"] != "")
                            {
                                age = Convert.ToInt32(Math.Round(DateTime.Now.Subtract(Convert.ToDateTime(userRegistration["DOB"])).TotalDays * 0.00273790926));
                            }


                            objUser.FirstName = userRegistration["firstName"];
                            objUser.LastName = userRegistration["lastName"];
                            objUser.Email = userRegistration["email"];
                            if (userRegistration["DOB"] != "")
                            {
                                objUser.DOB = Convert.ToDateTime(userRegistration["DOB"]);
                            }
                            objUser.IsActive = true;
                            objUser.DeviceId = userRegistration["DeviceId"];
                            objUser.RoleId = 4;
                            objUser.AccessToken = Helper.RandomString(6);
                            objUser.IsSuggested = 0;
                            objUser.Username = userRegistration["Username"];
                            objUser.Country = userRegistration["Country"];
                            objUser.CreatedDate = System.DateTime.Now;
                            objUser.Messages = 0;
                            objUser.BatchCount = 10;
                            objUser.LoginStatus = "1";
                            objUser.FlashCard = 0;
                            objUser.MyScore = 0;
                            objUser.TotalScore = 0;
                            objUser.LoginType = userRegistration["usertype"];
                            objUser.GoogleId = userRegistration["GoogleId"];
                            objUser.DialogBookmark = "";
                            objUser.GrammerBookmark = "";
                            objUser.VocabularyBookmark = "";
                            objUser.VocabId = "";
                            objUser.VocabSubId = "";
                            if (userRegistration["gender"] == "Male")
                            {
                                if (age > 18)
                                {
                                    objUser.Gender = "Male";
                                }
                                else
                                {
                                    objUser.Gender = "Child Male";
                                }
                            }
                            else
                            {
                                if (age > 18)
                                {
                                    objUser.Gender = "Female";
                                }
                                else
                                {
                                    objUser.Gender = "Child Female";
                                }
                            }
                            icanSpeakContext.Users.InsertOnSubmit(objUser);
                            icanSpeakContext.SubmitChanges();
                            var userId = icanSpeakContext.Users.ToList().Max(U => U.UserId);


                            objUser.ProfilePicture = userId + "_profilepic.png";
                            icanSpeakContext.SubmitChanges();




                            if (!string.IsNullOrEmpty(userRegistration["imagebase64"]))
                            {
                                string filePath = HostingEnvironment.MapPath("/ProfilePictures/" + userId + "_profilepic.png");
                                byte[] bytes = System.Convert.FromBase64String(userRegistration["imagebase64"]);
                                FileStream fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                                fs.Write(bytes, 0, bytes.Length);
                                fs.Close();
                            }

                            ThirdPartyWelcomeMail(userRegistration["email"], userRegistration["firstName"] + " " + userRegistration["lastName"], userId.ToString());


                            Table = Service.Message("success", "1");
                            Table.AcceptChanges();
                            DataTable Profile = new DataTable();
                            Profile.TableName = "Profile";
                            Profile.Columns.Add("Username");
                            Profile.Columns.Add("Country");
                            Profile.Columns.Add("ProfilePicture");
                            Profile.Columns.Add("UserId");
                            Profile.Columns.Add("BatchCount");
                            Profile.Columns.Add("DialogBookmark");
                            Profile.Columns.Add("GrammerBookmark");
                            Profile.Columns.Add("VocabularyBookmark");
                            Profile.Columns.Add("VocabId");
                            Profile.Columns.Add("VocabSubId");
                            DataRow usernewrow = Profile.NewRow();
                            usernewrow["Username"] = userRegistration["Username"];
                            usernewrow["Country"] = userRegistration["Country"];
                            usernewrow["ProfilePicture"] = Service.GetUrl() + "ProfilePictures/" + userId + "_profilepic.png";
                            usernewrow["UserId"] = Service.Encrypt(userid.ToString());
                            usernewrow["BatchCount"] = objUser.BatchCount;
                            usernewrow["DialogBookmark"] = objUser.DialogBookmark;
                            usernewrow["GrammerBookmark"] = objUser.GrammerBookmark;
                            usernewrow["VocabularyBookmark"] = objUser.VocabularyBookmark;
                            usernewrow["VocabId"] = objUser.VocabId;
                            usernewrow["VocabSubId"] = objUser.VocabSubId;
                            Profile.Rows.Add(usernewrow);
                            Profile.AcceptChanges();
                            ds.Tables.Add(Table);
                            ds.Tables.Add(Profile);
                            string js1 = JsonConvert.SerializeObject(ds);
                            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                            return new MemoryStream(Encoding.UTF8.GetBytes(js1));

                        }
                    }
                }




            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "UserRegistraction");
                //  var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }


        public string WelcomeMail(string email, string name, string newPassword, string userId)
        {
            try
            {
                //////////////////////  PAssword Send mail start //////////////////
                MailMessage message = new MailMessage();
                string Subject = string.Empty;
                string Body = string.Empty;
                bool check;

                var userName = from user in icanSpeakContext.Users
                               where user.Email == email
                               select user.FirstName + " " + user.LastName;

                message = new MailMessage();
                Subject = "Welcome to iCanSpeak Lanaguage E-Learing";
                Body = @"<table border='0' cellpadding='0' cellspacing='0'><tr><td>Hello  " + name +
                  "<br/><br/> Your autogenerated password is <b>" + newPassword + "</b><br/><br/>" +
                  "Once you have logged in, please head on over to profile/reset password to change your temporary password.</td></tr></table>";
                message.Subject = Subject;
                message.IsBodyHtml = true;
                message.Body = Body;
                message.To.Add(email);
                EmailSendUtility SendEmail = new EmailSendUtility();
                check = SendEmail.SendMail(Subject, Body, email);
                return check.ToString();
                //if (check != true)
                //{
                //    SubAdmin objSubAdmin = new SubAdmin();
                //    var result = (from user in icanSpeakContext.SubAdmins
                //                  where user.UserId == Convert.ToInt32(userId)
                //                  select user).FirstOrDefault();

                //    icanSpeakContext.SubAdmins.DeleteOnSubmit(result);
                //    icanSpeakContext.SubmitChanges();
                //}
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "UserRegistraction Email Service");
                return "false";
                //SubAdmin objSubAdmin = new SubAdmin();
                //var result = (from user in icanSpeakContext.SubAdmins
                //              where user.UserId == Convert.ToInt32(userId)
                //              select user).FirstOrDefault();

                //icanSpeakContext.SubAdmins.DeleteOnSubmit(result);
                //icanSpeakContext.SubmitChanges();

                //Helper.ErrorLog(ex, "UserRegistraction Email Service");
                //Table = Service.Message(ex.Message, ex.HResult.ToString());
                //ds.Tables.Add(Table);
                //return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
            }
        }

        public string ThirdPartyWelcomeMail(string email, string name, string userId)
        {
            try
            {
                //////////////////////  PAssword Send mail start //////////////////
                MailMessage message = new MailMessage();
                string Subject = string.Empty;
                string Body = string.Empty;
                bool check;

                var userName = from user in icanSpeakContext.Users
                               where user.Email == email
                               select user.FirstName + " " + user.LastName;

                message = new MailMessage();
                Subject = "Welcome to iCanSpeak Lanaguage E-Learing";
                Body = @"<table border='0' cellpadding='0' cellspacing='0'><tr><td>Hello  " + name +
                  "<br/><br/>Thank you for creating new account on iCanSpeak. For help and customer support you can always open a new thread in the support section.</td></tr></table>";
                message.Subject = Subject;
                message.IsBodyHtml = true;
                message.Body = Body;
                message.To.Add(email);
                EmailSendUtility SendEmail = new EmailSendUtility();
                check = SendEmail.SendMail(Subject, Body, email);
                return check.ToString();
                //if (check != true)
                //{
                //    SubAdmin objSubAdmin = new SubAdmin();
                //    var result = (from user in icanSpeakContext.SubAdmins
                //                  where user.UserId == Convert.ToInt32(userId)
                //                  select user).FirstOrDefault();

                //    icanSpeakContext.SubAdmins.DeleteOnSubmit(result);
                //    icanSpeakContext.SubmitChanges();
                //}
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "UserRegistraction Email Service");
                return "false";

            }
        }

        public Stream Login(Stream objStream)
        {

            //Exception ex1 = new Exception();
            //Helper.ErrorLog(ex1, requestString);
            DataSet ds = new DataSet("Result");
            String requestString = string.Empty;
            try
            {
                User objUser = new User();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                requestString = reader.ReadToEnd();

                // requestString = {\"email\":\"testuser@gmail.com\",\"password\":\"123456\"}";
                var userLogin = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);


                var emailStatus = (from user in icanSpeakContext.Users
                                   where user.Email == userLogin["email"]
                                   select user.Email).FirstOrDefault();

                if (emailStatus == null)
                {
                    Table = Service.Message("Invalid email", "0");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }



                var passwordStatus = (from user in icanSpeakContext.Users
                                      where (user.Email == userLogin["email"]) && (user.Password == userLogin["password"])
                                      select user.Email).FirstOrDefault();

                if (passwordStatus == null)
                {
                    Table = Service.Message("Invalid password", "0");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }

                var activeStatus = (from user in icanSpeakContext.Users
                                    where user.Email == userLogin["email"] && user.Password == userLogin["password"]
                                    select user.IsActive).FirstOrDefault();




                if (activeStatus.Value == false)
                {
                    Table = Service.Message("Account inactive contact to admin", "0");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));

                }

                var updatedata = (from user in icanSpeakContext.Users
                                  where user.Email == userLogin["email"]
                                  select user
                                 ).FirstOrDefault();


                updatedata.LoginStatus = "1";
                icanSpeakContext.SubmitChanges();


                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                var result = (from user in icanSpeakContext.Users
                              where user.Email == userLogin["email"]
                              && user.Password == userLogin["password"]
                              select new
                             {
                                 ProfilePicture = Service.GetUrl() + "ProfilePictures/" + user.ProfilePicture,
                                 UserId = Service.Encrypt(user.UserId.ToString()),
                                 Username = textInfo.ToTitleCase(user.Username),
                                 user.Country,
                                 user.BatchCount,
                                 DialogBookmark = user.DialogBookmark.ToString(),
                                 GrammerBookmark = user.GrammerBookmark.ToString(),
                                 VocabularyBookmark = user.VocabularyBookmark.ToString(),
                                 VocabId = user.VocabId,
                                 VocabSubId = user.VocabSubId,

                             }).ToList();

                if (result != null)
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();

                    //DataTable Profile = new DataTable();
                    //Profile.TableName = "Profile";
                    //Profile.Columns.Add("Username");
                    //Profile.Columns.Add("Country");
                    //Profile.Columns.Add("ProfilePicture");
                    //Profile.Columns.Add("UserId");
                    //Profile.Columns.Add("BatchCount");
                    //Profile.Columns.Add("DialogBookmark");
                    //Profile.Columns.Add("GrammerBookmark");
                    //Profile.Columns.Add("VocabularyBookmark");
                    //Profile.Columns.Add("VocabId");
                    //Profile.Columns.Add("VocabSubId");
                    //DataRow usernewrow = Profile.NewRow();
                    //usernewrow["Username"] = result.Username;
                    //usernewrow["Country"] = result.Country;
                    //usernewrow["ProfilePicture"] = Service.GetUrl() + "ProfilePictures/" + result.ProfilePicture;
                    //usernewrow["UserId"] = Service.Encrypt(result.ToString());
                    //usernewrow["BatchCount"] = result.BatchCount;
                    //if (result.DialogBookmark != "" && result.DialogBookmark != null)
                    //{
                    //    usernewrow["DialogBookmark"] = Service.Encrypt(result.DialogBookmark);
                    //}
                    //else
                    //{
                    //    usernewrow["DialogBookmark"] = result.DialogBookmark;
                    //}
                    //if (result.GrammerBookmark != "" && result.GrammerBookmark != null)
                    //{
                    //    usernewrow["GrammerBookmark"] = Service.Encrypt(result.GrammerBookmark);
                    //}
                    //else
                    //{
                    //    usernewrow["GrammerBookmark"] = result.GrammerBookmark;
                    //}
                    //if (result.VocabularyBookmark != "" && result.VocabularyBookmark != null)
                    //{
                    //    usernewrow["VocabularyBookmark"] = Service.Encrypt(result.VocabularyBookmark);
                    //}
                    //else
                    //{
                    //    usernewrow["VocabularyBookmark"] = result.VocabId;
                    //}
                    //if (result.VocabId != "" && result.VocabId != null)
                    //{
                    //    usernewrow["VocabId"] = Service.Encrypt(result.VocabId);
                    //}
                    //else
                    //{
                    //    usernewrow["VocabId"] = result.VocabId;
                    //}
                    //if (result.VocabSubId != "" && result.VocabSubId != null)
                    //{
                    //    usernewrow["VocabSubId"] = Service.Encrypt(result.VocabSubId);
                    //}
                    //else
                    //{
                    //    usernewrow["VocabSubId"] = result.VocabSubId;
                    //}


                    DataTable dt = Service.ConvertToDataTable(result);
                    // dt.Columns["GrammarBookmark"] = ((dt.Columns["GrammarBookmark"].ToString()) == "" ? "" : Service.Encrypt(dt.Columns["GrammarBookmark"].ToString()));

                    dt.TableName = "Profile";

                    //Edit columns
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["GrammerBookmark"] = dr["GrammerBookmark"].ToString() == "" ? "" : Service.Encrypt(dr["GrammerBookmark"].ToString());
                        dr["VocabularyBookmark"] = dr["VocabularyBookmark"].ToString() == "" ? "" : Service.Encrypt(dr["VocabularyBookmark"].ToString());
                        dr["DialogBookmark"] = dr["DialogBookmark"].ToString() == "" ? "" : Service.Encrypt(dr["DialogBookmark"].ToString());
                        dr["VocabId"] = dr["VocabId"].ToString() == "" ? "" : Service.Encrypt(dr["VocabId"].ToString());
                        dr["VocabSubId"] = dr["VocabSubId"].ToString() == "" ? "" : Service.Encrypt(dr["VocabSubId"].ToString());
                    }

                    //dt.Columns.Add("DialogBookmark1");
                    //dt.Columns.Add("VocabularyBookmark1");
                    //dt.Columns.Add("GrammerBookmark1");

                    //DataRow usernewrow = dt.NewRow();

                    //// usernewrow["Dialog"] = Service.Encrypt(result[0].GrammerBookmark);
                    //if (result[0].DialogBookmark != "")
                    //{
                    //    usernewrow["DialogBookmark1"] = Service.Encrypt(result[0].DialogBookmark);
                    //}
                    //else
                    //{
                    //    usernewrow["DialogBookmark1"] = result[0].DialogBookmark;
                    //}
                    //if (result[0].GrammerBookmark != "")
                    //{
                    //    usernewrow["GrammerBookmark1"] = Service.Encrypt(result[0].GrammerBookmark);
                    //}
                    //else
                    //{
                    //    usernewrow["GrammerBookmark1"] = result[0].GrammerBookmark;
                    //}
                    //if (result[0].VocabularyBookmark != "")
                    //{
                    //    usernewrow["VocabularyBookmark1"] = Service.Encrypt(result[0].VocabularyBookmark);
                    //}
                    //else
                    //{
                    //    usernewrow["VocabularyBookmark1"] = result[0].VocabularyBookmark;
                    //}
                    //dt.Columns["GrammarBookmark"] = usernewrow["DialogBookmark1"];
                    //dt.Rows.Add(usernewrow);
                    // Profile.Rows.Add(usernewrow);
                    dt.AcceptChanges();
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

                Helper.ErrorLog(ex, requestString);
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));// javaScriptSerializer.Serialize(ex.Message.ToString());


            }

        }

        public Stream Logout(Stream objStream)
        {

            //Exception ex1 = new Exception();
            //Helper.ErrorLog(ex1, requestString);
            DataSet ds = new DataSet("Result");
            String requestString = string.Empty;
            try
            {
                User objUser = new User();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                requestString = reader.ReadToEnd();

                // requestString = {\"email\":\"testuser@gmail.com\",\"password\":\"123456\"}";
                var userLogin = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var UserData = (from user in icanSpeakContext.Users
                                where user.UserId == Convert.ToInt32(Service.Decrypt(userLogin["userid"]))
                                select user
                                ).FirstOrDefault();

                UserData.DeviceId = "";
                UserData.LoginStatus = "0";
                icanSpeakContext.SubmitChanges();

                Table = Service.Message("success", "1");
                Table.AcceptChanges();
                DataTable dt = Service.ResultMessage("updated", "1");
                dt.TableName = "Profile";
                ds.Tables.Add(Table);
                ds.Tables.Add(dt);
                string js1 = JsonConvert.SerializeObject(ds);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(js1));


            }
            catch (Exception ex)
            {

                Helper.ErrorLog(ex, requestString);
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));// javaScriptSerializer.Serialize(ex.Message.ToString());


            }

        }
        public Stream GetLoginType(Stream objStream)
        {

            DataSet ds = new DataSet("Result");
            String requestString = string.Empty;
            try
            {
                User objUser = new User();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                requestString = reader.ReadToEnd();

                // requestString = {\"email\":\"testuser@gmail.com\",\"password\":\"123456\"}";
                var userLogin = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                int userid = 0;


                if (userLogin["userid"] != "")
                {
                    userid = Convert.ToInt32(Service.Decrypt(userLogin["userid"]));
                    var logintype = (from user in icanSpeakContext.Users
                                     where user.UserId == userid
                                     select new
                                     {
                                         usertype = user.LoginType,
                                         user.LoginStatus
                                     });

                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(logintype); ;
                    dt.TableName = "LoginType";
                    ds.Tables.Add(Table);
                    ds.Tables.Add(dt);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
                }
                else
                {
                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = new DataTable();
                    dt.TableName = "LoginType";
                    dt.Columns.Add("LoginType");
                    dt.Columns.Add("LoginStatus");
                    DataRow drRownew = dt.NewRow();
                    drRownew["LoginType"] = "4";
                    drRownew["LoginStatus"] = "0";
                    dt.Rows.Add(drRownew);
                    dt.AcceptChanges();
                    ds.Tables.Add(Table);
                    ds.Tables.Add(dt);
                    string js1 = JsonConvert.SerializeObject(ds);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
                }
            }
            catch (Exception ex)
            {

                Helper.ErrorLog(ex, requestString);
                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));// javaScriptSerializer.Serialize(ex.Message.ToString());
            }
        }

        public Stream ForgotPassword(Stream objStream)
        {

            User objUser = new User();
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String requestString = reader.ReadToEnd();
            DataSet ds = new DataSet("Result");
            var forgotpassword = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
            try
            {
                MailMessage message = new MailMessage();
                string Subject = string.Empty;
                string Body = string.Empty;
                bool check;
                var newPassword = Helper.RandomString(6);

                var userName = (from user in icanSpeakContext.Users
                                where user.Email == forgotpassword["email"]
                                select new { Name = user.FirstName + " " + user.LastName }).FirstOrDefault();

                if (userName != null)
                {
                    message = new MailMessage();
                    Subject = "Your  password has been changed.";
                    Body = @"<table border='0' cellpadding='0' cellspacing='0'><tr><td>Hello  " + userName.Name +
                        ",<br/><br/> Your  password has been changed.Your new password is <b>" + newPassword + "</b><br/><br/>" +
                      "Once you have logged in, please head on over to profile/reset password to change your temporary password.</td></tr></table>";
                    message.Subject = Subject;
                    message.IsBodyHtml = true;
                    message.Body = Body;
                    message.To.Add(forgotpassword["email"]);
                    EmailSendUtility SendEmail = new EmailSendUtility();
                    check = SendEmail.SendMail(Subject, Body, forgotpassword["email"]);
                    if (check != true)
                    {

                        DataTable Table = Service.Message("error", "0");
                        Table.AcceptChanges();
                        ds.Tables.Add(Table);
                        string js1 = JsonConvert.SerializeObject(ds);
                        return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                    }
                    else
                    {
                        User objuser = icanSpeakContext.Users.Single(u => u.Email == forgotpassword["email"]);
                        objuser.Password = newPassword;
                        icanSpeakContext.SubmitChanges();
                        DataTable Table = Service.Message("Password sent", "1");
                        Table.AcceptChanges();
                        ds.Tables.Add(Table);
                        string js1 = JsonConvert.SerializeObject(ds);
                        return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                    }
                }
                else
                {
                    DataTable Table = Service.Message("incorrect email", "0");
                    Table.AcceptChanges();
                    ds.Tables.Add(Table);
                    string js1 = JsonConvert.SerializeObject(ds);
                    return new MemoryStream(Encoding.UTF8.GetBytes(js1));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "ForgotPassword");

                Table = Service.Message(ex.Message, ex.HResult.ToString());
                ds.Tables.Add(Table);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ds)));
                //   var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            }

        }

        public Stream GetAllUsers(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String responseString = reader.ReadToEnd();
            try
            {
                // responseString = "{\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                var userRequest = javaScriptSerializer.Deserialize<Dictionary<string, string>>(responseString);

                var checkUserAuthentication = from user in icanSpeakContext.Users
                                              where user.UserId == Convert.ToInt32(userRequest["LoginUserId"]) &&
                                              user.AccessToken == userRequest["AccessToken"]
                                              select user.Email;

                if (Convert.ToString(checkUserAuthentication.FirstOrDefault()) != "" && Convert.ToString(checkUserAuthentication.FirstOrDefault()) != null)
                {
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                    var checkUsers = (from users in icanSpeakContext.Users
                                      select new { users.UserId, users.Email, users.CreatedDate, FirstName = textInfo.ToTitleCase(users.FirstName), users.Gender, LastName = textInfo.ToTitleCase(users.LastName), Country = textInfo.ToTitleCase(users.Country), users.IsActive, users.IsSuggested });


                    // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(checkUsers)).Replace("\\/", "/")));

                }
                else
                {
                    // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize("User Authentication Failed")).Replace("\\/", "/")));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAllUsers");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }


        public Stream GetAllUsersAdmin(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String responseString = reader.ReadToEnd();
            try
            {
                // responseString = "{\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\"}";

                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                //1 var checkUsers = icanSpeakContext.Users.GroupJoin(icanSpeakContext.TutorSubTutorMappings, a => a.UserId, b => b.StudentUserId, (a, b) => new { a, b }).ToList()
                //2 var checkUsers = (from users in icanSpeakContext.Users
                //                 join student in icanSpeakContext.TutorSubTutorMappings on users.UserId equals student.StudentUserId into usr from y in usr.DefaultIfEmpty()
                //                 join tutor in icanSpeakContext.SubAdmins on users.UserId equals tutor.UserId into tut from x in tut.DefaultIfEmpty()
                //                 where users.RoleId == 4            
                var checkUsers = (from users in icanSpeakContext.Users
                                  join student in icanSpeakContext.TutorSubTutorMappings on users.UserId equals student.StudentUserId into US
                                  from USs in US.DefaultIfEmpty()
                                  join tutor in icanSpeakContext.SubAdmins on USs.TutorId equals tutor.UserId into UST
                                  from USTs in UST.DefaultIfEmpty()
                                  select new
                                  {
                                      UserId = users.UserId,
                                      Email = users.Email,
                                      FirstName = users.FirstName,
                                      LastName = users.LastName,
                                      Gender = users.Gender,
                                      Country = users.Country,
                                      CreatedDate = users.CreatedDate,
                                      IsActive = users.IsActive,
                                      IsSuggested = users.IsSuggested,
                                      TutFirstName = USTs.FirstName,
                                      TutLastName = USTs.LastName,
                                  });

                //3 var checkUsers = (from users in icanSpeakContext.Users
                //                  join student in icanSpeakContext.TutorSubTutorMappings on users.UserId equals student.StudentUserId
                //                  join tutor in icanSpeakContext.SubAdmins on student.TutorId equals tutor.UserId
                //                  let FullName = tutor.FirstName+" "+tutor.LastName
                //                  where tutor.RoleId == 3
                //                  select new
                //                  {
                //                      users.Email,
                //                      users.CreatedDate,
                //                      FirstName = textInfo.ToTitleCase(users.FirstName),
                //                      users.Gender,
                //                      LastName = textInfo.ToTitleCase(users.LastName),
                //                      Country = textInfo.ToTitleCase(users.Country),
                //                      users.City,
                //                      users.IsActive,
                //                      users.IsSuggested,
                //                      //FullName
                //                  });

                //var checkUsers = (from users in icanSpeakContext.Users
                //                  where users.RoleId == 4
                //                  select new
                //                  {
                //                      users.UserId,
                //                      users.Email,
                //                      users.CreatedDate,
                //                      FirstName = textInfo.ToTitleCase(users.FirstName),                                     
                //                      LastName = textInfo.ToTitleCase(users.LastName),
                //                      users.Gender,
                //                      Country = textInfo.ToTitleCase(users.Country),
                //                      users.IsActive,
                //                      users.IsSuggested
                //                  });
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(checkUsers)).Replace("\\/", "/")));


            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAllUsers");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }

        public Stream FilterByGender(Stream objStream)
        {
            try
            {
                iCanSpeakCourse objiCanSpeakCourse = new iCanSpeakCourse();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                var filter = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                var checkUsers = (from users in icanSpeakContext.Users
                                  join student in icanSpeakContext.TutorSubTutorMappings on users.UserId equals student.StudentUserId into US
                                  from USs in US.DefaultIfEmpty()
                                  join tutor in icanSpeakContext.SubAdmins on USs.TutorId equals tutor.UserId into UST
                                  from USTs in UST.DefaultIfEmpty()
                                  where users.Gender == filter["Gender"]
                                  select new
                                  {
                                      UserId = users.UserId,
                                      Email = users.Email,
                                      FirstName = users.FirstName,
                                      LastName = users.LastName,
                                      Gender = users.Gender,
                                      Country = users.Country,
                                      CreatedDate = users.CreatedDate,
                                      IsActive = users.IsActive,
                                      IsSuggested = users.IsSuggested,
                                      TutFirstName = USTs.FirstName,
                                      TutLastName = USTs.LastName,
                                  }).ToList();
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(checkUsers)).Replace("\\/", "/")));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAllUsers");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }



        //public Stream FilterByGender(Stream objStream)
        //{
        //    StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
        //    String responseString = reader.ReadToEnd();
        //    try
        //    {
        //        var filter = javaScriptSerializer.Deserialize<Dictionary<string, string>>(responseString);
        //        String requestString = reader.ReadToEnd();
        //        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        //        var checkUsers = (from users in icanSpeakContext.Users
        //                          join student in icanSpeakContext.TutorSubTutorMappings on users.UserId equals student.StudentUserId into US
        //                          from USs in US.DefaultIfEmpty()
        //                          join tutor in icanSpeakContext.SubAdmins on USs.TutorId equals tutor.UserId into UST
        //                          from USTs in UST.DefaultIfEmpty()
        //                          where users.Gender == filter["Gender"]
        //                          select new
        //                          {
        //                              UserId = users.UserId,
        //                              Email = users.Email,
        //                              FirstName = users.FirstName,
        //                              LastName = users.LastName,
        //                              Gender = users.Gender,
        //                              Country = users.Country,
        //                              CreatedDate = users.CreatedDate,
        //                              IsActive = users.IsActive,
        //                              IsSuggested = users.IsSuggested,
        //                              TutFirstName = USTs.FirstName,
        //                              TutLastName = USTs.LastName,
        //                          });
        //        if (checkUsers.Count() > 0 && checkUsers != null)
        //        {
        //            Table = Service.Message("success", "1");
        //            Table.AcceptChanges();
        //            DataTable dt = Service.ConvertToDataTable(checkUsers); ;
        //            dt.TableName = "Users";
        //            ds.Tables.Add(Table);
        //            ds.Tables.Add(dt);
        //            string js1 = JsonConvert.SerializeObject(ds);
        //            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
        //            return new MemoryStream(Encoding.UTF8.GetBytes(js1));
        //        }
        //        else
        //        {
        //            DataTable Table = Service.Message("No Data", "0");
        //            Table.AcceptChanges();
        //            ds.Tables.Add(Table);
        //            string js1 = JsonConvert.SerializeObject(ds);
        //            return new MemoryStream(Encoding.UTF8.GetBytes(js1));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Helper.ErrorLog(ex, "GetAllTutors");

        //        // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
        //        return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
        //    }
        //}


        public Stream FilterByCountry(Stream objStream)
        {
            try
            {
                iCanSpeakCourse objiCanSpeakCourse = new iCanSpeakCourse();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                var filter = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                var checkUsers = (from users in icanSpeakContext.Users
                                  join student in icanSpeakContext.TutorSubTutorMappings on users.UserId equals student.StudentUserId into US
                                  from USs in US.DefaultIfEmpty()
                                  join tutor in icanSpeakContext.SubAdmins on USs.TutorId equals tutor.UserId into UST
                                  from USTs in UST.DefaultIfEmpty()
                                  where users.Country == filter["Country"]
                                  select new
                                  {
                                      UserId = users.UserId,
                                      Email = users.Email,
                                      FirstName = users.FirstName,
                                      LastName = users.LastName,
                                      Gender = users.Gender,
                                      Country = users.Country,
                                      CreatedDate = users.CreatedDate,
                                      IsActive = users.IsActive,
                                      IsSuggested = users.IsSuggested,
                                      TutFirstName = USTs.FirstName,
                                      TutLastName = USTs.LastName,
                                  });
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(checkUsers)).Replace("\\/", "/")));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "GetAllUsers");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }

        public Stream SearchUser(Stream objStream)
        {
            StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
            String responseString = reader.ReadToEnd();
            var parameters = javaScriptSerializer.Deserialize<Dictionary<string, string>>(responseString);
            DataSet ds = new DataSet();
            try
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                var result = (from users in icanSpeakContext.Users
                              where users.RoleId == 4 &&
                              users.Username.StartsWith(parameters["prefix"]) &&
                              users.UserId != Convert.ToInt32(Service.Decrypt(parameters["userid"]))
                              select new
                              {
                                  UserId = Service.Encrypt(users.UserId.ToString()),
                                  users.Email,
                                  UserName = textInfo.ToTitleCase(users.Username)
                              }).ToList();

                if (result.Count > 0)
                {

                    Table = Service.Message("success", "1");
                    Table.AcceptChanges();
                    DataTable dt = Service.ConvertToDataTable(result); ;
                    dt.TableName = "Users";
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
                Helper.ErrorLog(ex, "GetAllUsers");

                // WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes((javaScriptSerializer.Serialize(ex.Message.ToString())).Replace("\\/", "/")));
            }
        }

        public Stream DeleteUserById(Stream objStream)
        {
            User objUser = new User();

            try
            {
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"userId\":\"10\",\"softDelete\":\"false\"}";
                var userString = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var result = (from user in icanSpeakContext.Users
                              where user.UserId == Convert.ToInt32(userString["userId"])
                              select user).FirstOrDefault();

                if (userString["softDelete"] == "true")
                {
                    if (result.IsActive == true)
                    {
                        result.IsActive = false;
                    }
                    else
                    {
                        result.IsActive = true;
                    }

                }

                else
                {
                    //var result = (from user in icanSpeakContext.Users
                    //              where user.UserId == Convert.ToInt32(userString["userId"])
                    //              select user).FirstOrDefault();

                    icanSpeakContext.Users.DeleteOnSubmit(result);
                }
                icanSpeakContext.SubmitChanges();
                var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));


            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "DeleteUserById");

                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }

        public Stream MakeSuggestedByUserId(Stream objStream)
        {
            User objUser = new User();

            try
            {
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();


                //  requestString = "{\"email\":\"rahul@techvalens.com\",\"password\":\"h\"}";
                //  requestString = "{\"userId\":\"1\",\"status\":\"1\"}";
                var userString = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                // int userId = Convert.ToInt32("userId");
                var status = (from userStatus in icanSpeakContext.Users
                              where userStatus.UserId == Convert.ToInt32(userString["userId"])
                              select userStatus).FirstOrDefault();

                if (status.IsSuggested == Convert.ToInt32(userString["status"]))
                {
                    status.IsSuggested = 1;
                }
                else
                {
                    status.IsSuggested = 0;
                }

                //  icanSpeakContext.Users.InsertOnSubmit(status);
                icanSpeakContext.SubmitChanges();
                var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "MakeSuggestedByUserId");

                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
        }
    }
}