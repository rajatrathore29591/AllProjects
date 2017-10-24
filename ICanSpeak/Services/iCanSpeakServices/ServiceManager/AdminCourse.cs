using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Globalization;

namespace iCanSpeakServices.ServiceManager
{
    public class AdminCourse
    {
        iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        public Stream AddCourse(Stream objStream)
        {
            try
            {
                iCanSpeakCourse objiCanSpeakCourse = new iCanSpeakCourse();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                //  requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var courseData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var courseName = icanSpeakContext.iCanSpeakCourses.Any(course => course.CourseName == courseData["courseName"]);
                if (courseName == true)
                {

                    //var js = JsonConvert.SerializeObject("Please choose a different course name ! This one is already in use", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Please choose a different course name ! This one is already in use")));
                }
                else
                {
                    objiCanSpeakCourse.CourseName = courseData["courseName"];
                    objiCanSpeakCourse.CourseDescription = courseData["courseDescription"];
                    objiCanSpeakCourse.CourseType = courseData["courseType"];
                    objiCanSpeakCourse.Duration = courseData["duration"];
                    objiCanSpeakCourse.RewardPoints = Convert.ToInt32(courseData["rewardPoints"]);
                    objiCanSpeakCourse.Price = Convert.ToInt32(courseData["price"]);
                    //objiCanSpeakCourse.ImageUrl = courseData["imageUrl"];
                    //objiCanSpeakCourse.AudioUrl = courseData["audioUrl"];
                    objiCanSpeakCourse.MaxScore = Convert.ToInt32(courseData["maxScore"]);
                    objiCanSpeakCourse.IsActive = true;
                    objiCanSpeakCourse.IsFree = Convert.ToBoolean(courseData["isFree"]);
                    objiCanSpeakCourse.Unit = Convert.ToInt32(courseData["unit"]);
                    icanSpeakContext.iCanSpeakCourses.InsertOnSubmit(objiCanSpeakCourse);
                    icanSpeakContext.SubmitChanges();

                    var courseId = icanSpeakContext.iCanSpeakCourses.ToList().Max(U => U.CourseId);
                    if (courseData["courseType"] == "grammer")
                    {
                        objiCanSpeakCourse.AudioUrl = courseId + "_video_" + courseData["audioUrl"];
                    }
                    if (courseData["courseType"] == "vocabulary")
                    {
                        objiCanSpeakCourse.ImageUrl = courseId + "_image_" + courseData["imageUrl"];
                        objiCanSpeakCourse.AudioUrl = courseId + "_audio_" + courseData["audioUrl"];
                    }
                    if (courseData["courseType"] == "dialog")
                    {
                        objiCanSpeakCourse.AudioUrl = courseId + "_video_" + courseData["audioUrl"];
                    }
                    
                    icanSpeakContext.SubmitChanges();
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(courseId.ToString())));


                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddCourse");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));

            }

        }

        public Stream UpdateCourse(Stream objStream)
        {
            try
            {
                iCanSpeakCourse objiCanSpeakCourse = new iCanSpeakCourse();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                //  requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var courseData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var courseResult = (from data in icanSpeakContext.iCanSpeakCourses
                                    where data.CourseId == Convert.ToInt32(courseData["courseId"])
                                    select data).FirstOrDefault();

                if (courseResult.CourseId > 0)
                {

                    courseResult.CourseName = courseData["courseName"];
                    courseResult.CourseDescription = courseData["courseDescription"];
                    courseResult.CourseType = courseData["courseType"];
                    courseResult.Duration = courseData["duration"];
                    courseResult.RewardPoints = Convert.ToInt32(courseData["rewardPoints"]);
                    courseResult.Price = Convert.ToInt32(courseData["price"]);
                    if (courseData["courseType"] == "grammer")
                    {
                        courseResult.AudioUrl = courseResult.CourseId + "_video_" + courseData["audioUrl"];
                        courseResult.ImageUrl = "";
                    }
                    if (courseData["courseType"] == "vocabulary")
                    {
                        courseResult.ImageUrl = courseResult.CourseId + "_image_" + courseData["imageUrl"];
                        courseResult.AudioUrl = courseResult.CourseId + "_audio_" + courseData["audioUrl"];
                    }
                    if (courseData["courseType"] == "dialog")
                    {
                        courseResult.AudioUrl = courseResult.CourseId + "_video_" + courseData["audioUrl"];
                        courseResult.ImageUrl = "";
                    }
                    //if (courseData["imageUrl"] != "")
                    //{
                    //    courseResult.ImageUrl = courseResult.CourseId + "_image_" + courseData["imageUrl"];
                    //}
                    //if (courseData["audioUrl"] != "")
                    //{
                    //    courseResult.AudioUrl = courseResult.CourseId + "_audio_" + courseData["audioUrl"];
                    //}
                    courseResult.MaxScore = Convert.ToInt32(courseData["maxScore"]);
                     courseResult.IsActive = true;
                    courseResult.IsFree = Convert.ToBoolean(courseData["isFree"]);
                    courseResult.Unit = Convert.ToInt32(courseData["unit"]);
                    icanSpeakContext.SubmitChanges();
                }

               // var js = JsonConvert.SerializeObject("Success", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddCourse");
                var js = JsonConvert.SerializeObject(ex.Message.ToString(), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));

            }

        }

        public Stream DeleteCourse(Stream objStream)
        {
            try
            {
                iCanSpeakCourse objiCanSpeakCourse = new iCanSpeakCourse();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();
                //  requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var courseData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var courseResult = (from data in icanSpeakContext.iCanSpeakCourses
                                    where data.CourseId == Convert.ToInt32(courseData["courseId"])
                                    select data).FirstOrDefault();

                if (courseData["softDelete"] == "true")
                {
                    if (courseResult.IsActive == true)
                    {

                        courseResult.IsActive = false;
                    }
                    else
                    {
                        courseResult.IsActive = true;
                    }
                }
                else
                {

                    icanSpeakContext.iCanSpeakCourses.DeleteOnSubmit(courseResult);
                }

                icanSpeakContext.SubmitChanges();



                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("Success")));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddCourse");
                
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));

            }

        }

        public Stream GetAllCourse(Stream objStream)
        {
            try
            {
                iCanSpeakCourse objiCanSpeakCourse = new iCanSpeakCourse();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var courseData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);
                string str;
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                var courseResult = (from data in icanSpeakContext.iCanSpeakCourses
                                    select new { data.CourseDescription, data.AudioUrl, data.CourseId,CourseName=textInfo.ToTitleCase(data.CourseName),CourseType= textInfo.ToTitleCase(data.CourseType), data.CreatedDate, data.Duration, ImageUrl = Service.GetUrl() + "CourseImages/" + data.ImageUrl, data.IsActive, data.IsFree, data.MaxScore, data.Price, data.RewardPoints, data.Unit }).ToList();
                 
                var js = JsonConvert.SerializeObject(courseResult, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                return new MemoryStream(Encoding.UTF8.GetBytes(js));
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddCourse");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));
            }

        }

        public Stream GetCourseByCourseId(Stream objStream)
        {
            try
            {
                iCanSpeakCourse objiCanSpeakCourse = new iCanSpeakCourse();
                StreamReader reader = new StreamReader(objStream, Encoding.UTF8);
                String requestString = reader.ReadToEnd();

                //  requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
                var courseData = javaScriptSerializer.Deserialize<Dictionary<string, string>>(requestString);

                var courseResult = (from data in icanSpeakContext.iCanSpeakCourses
                                    where data.CourseId == Convert.ToInt32(courseData["courseId"])
                                    select new { data.CourseId, data.CourseName, data.CourseDescription, data.Duration, AudioUrl = Service.GetUrl() + "CourseImages/" + data.AudioUrl, data.CourseType, data.CreatedDate, ImageUrl = Service.GetUrl() + "CourseImages/" + data.ImageUrl, data.IsFree, data.MaxScore, data.Price, data.RewardPoints, data.Unit }).ToList();

                if (courseResult.Count > 0)
                {
                    var js = JsonConvert.SerializeObject(courseResult, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(js));
                }
                else
                {
                    var js = JsonConvert.SerializeObject("No data", Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor("No data")));
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog(ex, "AddCourse");
                return new MemoryStream(Encoding.UTF8.GetBytes(Service.StringToJsonConvertor(ex.Message)));

            }

        }
    }
}