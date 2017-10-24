using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BorgCivil.Utils
{
    public static class ServiceHelper
    {
        /// <summary>
        /// This method for check is valid GUID.
        /// </summary>
        /// <param name="value">raw guid/id</param>
        /// <returns>result</returns>
        public static bool IsGuid(string value)
        {
            try
            {
                Guid parseFormat;
                return Guid.TryParse(value, out parseFormat);
            }
            catch (FormatException ex)
            {
                LogHelper.CommonLog log = new LogHelper.CommonLog() { Method = MethodBase.GetCurrentMethod().Name, Exception = ex.Message };
                return false;
            }
        }

        /// <summary>
        /// check is empty
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmpty(string value)
        {
            return string.IsNullOrEmpty(value) ? true : false;
        }

        /// <summary>
        /// This method for convert string to boolen.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool BooleanConversion(object input)
        {
            bool output = false;
            String[] values = { null, String.Empty, "true", "True", "TrueString", "False", "false", "-1", "0" };
            foreach (var value in values)
            {
                try
                {
                    output = Convert.ToBoolean(input);
                }
                catch (FormatException ex)
                {
                    // Handel Exception Log
                    LogHelper.CommonLog log = new LogHelper.CommonLog() { Method = MethodBase.GetCurrentMethod().Name, Exception = ex.Message };
                    output = false;
                }
            }
            return output;
        }
        
        /// <summary>
        /// This method for Get List of ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static List<Guid> GetIds(List<string> ids)
        {
            ////define var
            var collection = new List<Guid>();

            ////get valid id
            foreach (var id in ids)
            {
                ////check id is valid
                if (!IsGuid(id))
                    ////set id in list.
                    collection.Add(new Guid(id));
            }
            ////return ids collection
            return collection;
        }

        /// <summary>
        /// This method use for file/image delete from folder.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FileDeleteFromFolder(string filePath)
        {
            try
            {
                ////if check path is valid
                if (!string.IsNullOrEmpty(filePath))
                {
                    ////check file is exist
                    if ((System.IO.File.Exists(filePath)))
                    {
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                        System.IO.File.Delete(filePath);
                        return true;    
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }

        // code for genrating image to base64
        public static string ImageToBase64(string filePath)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            return base64ImageRepresentation;
        }

        // code for genrating thumbnail from image
        //public static string GenerateThumbnail(string path)
        //{
        //    string base64String = string.Empty;
        //    Size size = new Size();
        //    size.Height = 100;
        //    size.Width = 100;
        //    /// assigning path to Image
        //    Image img = Image.FromFile(path);
        //    var resizedImage = ImageHelper.ResizeImage(img, size);
        //    Image thumbnail = resizedImage;
        //    //using (System.Drawing.Image thumbnail = image.GetThumbnailImage(100, 100, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero))
        //    //{
        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            thumbnail.Save(memoryStream, ImageFormat.Png);
        //            Byte[] bytes = new Byte[memoryStream.Length];
        //            memoryStream.Position = 0;
        //            memoryStream.Read(bytes, 0, (int)bytes.Length);
        //            base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
        //            memoryStream.Dispose();
        //            memoryStream.Close();
        //        }
        //    //}
        //    return base64String;
        //}

        public static bool ThumbnailCallback()
        {
            return false;
        }


        /// <summary>
        /// This method use for get web config file value by key name.
        /// </summary>
        /// <param name="appSettingsKey"></param>
        /// <returns>AppSettingsValue</returns>
        //public static string GetAppSettingsValue(string appSettingsKey)
        //{
        //    //TODO: handle errors, maybe add some overloads for numbers, etc..**??
        //    return System.Configuration.ConfigurationManager.AppSettings.Get(appSettingsKey);
        //}
    }


    public class NameValueCollectionMapper<T> where T : new()
    {
        private static readonly System.Text.RegularExpressions.Regex _regex = new System.Text.RegularExpressions.Regex(@"\[(?<value>.*?)\]", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.Singleline);
        public static T Map(System.Collections.Specialized.NameValueCollection nvc, string rootObjectName)
        {
            var result = new T();
            foreach (string kvp in nvc.AllKeys)
            {
                if (!kvp.StartsWith(rootObjectName))
                    throw new Exception("All keys should start with " + rootObjectName);
                var match = _regex.Match(kvp.Remove(0, rootObjectName.Length));

                if (match.Success)
                {
                    // build path in a form of [Documents, 0, DocumentID]-like array
                    var path = new List<string>();
                    while (match.Success)
                    {
                        path.Add(match.Groups["value"].Value);
                        match = match.NextMatch();
                    }
                    // this is object we currently working on                                      
                    object currentObject = result;
                    for (int i = 0; i < path.Count; i++)
                    {
                        bool last = i == path.Count - 1;
                        var propName = path[i];
                        int index;
                        if (int.TryParse(propName, out index))
                        {
                            // index access, like [0]
                            var list = currentObject as System.Collections.IList;
                            if (list == null)
                                throw new Exception("Invalid index access expression"); // more info here
                                                                                        // get the type of item in that list (i.e. Document)
                            var args = list.GetType().GetGenericArguments();
                            var listItemType = args[0];
                            if (last)
                            {
                                // may need more sophisticated conversion from string to target type
                                list[index] = Convert.ChangeType(nvc[kvp], Nullable.GetUnderlyingType(listItemType) ?? listItemType);
                            }
                            else
                            {
                                // if not initialized - initalize
                                var next = index < list.Count ? list[index] : null;
                                if (next == null)
                                {
                                    // handle IList case in a special way here, since you cannot create instance of interface                                    
                                    next = Activator.CreateInstance(listItemType);
                                    // fill with nulls if not enough items yet
                                    while (index >= list.Count)
                                    {
                                        list.Add(null);
                                    }
                                    list[index] = next;
                                }
                                currentObject = next;
                            }
                        }
                        else
                        {
                            var prop = currentObject.GetType().GetProperty(propName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                            if (last)
                            {
                                // may need more sophisticated conversion from string to target type
                                prop.SetValue(currentObject, Convert.ChangeType(nvc[kvp], Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType));
                            }
                            else
                            {
                                // if not initialized - initalize
                                var next = prop.GetValue(currentObject);
                                if (next == null)
                                {
                                    // TODO: handle IList case in a special way here, since you cannot create instance of interface                                    
                                    next = Activator.CreateInstance(prop.PropertyType);
                                    prop.SetValue(currentObject, next);
                                }
                                currentObject = next;
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
