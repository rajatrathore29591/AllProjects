using System;
using System.Configuration;
using TradeMark.App_Data;
using System.Net.Mail;
using System.Data;
using System.Data.SqlClient;
using TradeMark.BAL;



namespace TradeMark.Utility
{
    public static class Utility
    {

        public static bool SendEmail(string subject,string body, string To)
        {
            var password =GeneratePassword();
            MailMessage mailMessage = new MailMessage();
            // Setup the mail configuration.
            mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);
            mailMessage.Subject = subject;
            
            if (subject == "Account Recovery Password")
            {
                mailMessage.Body = body + "<b>" + " " + password + "</b>";
                mailMessage.Body += "<br>Have Fabulous Day";
            }else
            {
                mailMessage.Body = body;
            }
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(new MailAddress(To));
            try
            {
                if (mailMessage.To != null && mailMessage.To.Count > 0)
                {
                    // Initiat smtp configuration.
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["Host"];
                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                    // Pass the sender credentials.
                    NetworkCred.UserName = ConfigurationManager.AppSettings["UserName"];
                    NetworkCred.Password = ConfigurationManager.AppSettings["Password"];
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);

                    smtp.Send(mailMessage);
                    if (subject == "Account Recovery Password")
                    {
                        //update password in db
                        UserService oUserService = new UserService();
                        EncryptDecrypt objEncryptDecrypt = new EncryptDecrypt();
                        oUserService.ResetPassword(To, objEncryptDecrypt.Encrypt(password));
                    }
                    return true;
                }
            }catch(Exception ex)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Generate auto/rendom password to REset pwd
        /// </summary>
        /// <returns></returns>
        public static string GeneratePassword()
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;
            
                characters += alphabets + small_alphabets + numbers;
            
            int length = 8;
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            
            return otp;

        }

        /// <summary>
        /// Method to delete saved search, 1 year old searches
        /// </summary>
        public static void DeleteSavedSearch()
        {
            // SQL Data access process here, 
            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();
            SqlCommand sqlcom = new SqlCommand();
            sqlcon.ConnectionString = connectionstring;
            try
            {
                sqlcon.Open();
                sqlcom.Connection = sqlcon;
                //Put the sql stored procedure ........
                sqlcom.CommandText = "[Usp_DeleteSavedSearches]";
                
                sqlcom.CommandType = CommandType.StoredProcedure;
                sqlcom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                sqlcon.Close();
            }
        }
    }
}
