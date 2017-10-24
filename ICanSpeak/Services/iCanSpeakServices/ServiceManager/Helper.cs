using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iCanSpeakServices.ServiceManager
{
    public class Helper
    {
       static iCanSpeakDataContext icanSpeakContext = new iCanSpeakDataContext();
        public static void ErrorLog(Exception ex, string EventId)
        {
            RX_Log errorLog = new RX_Log();
            errorLog.EventId = EventId;
            errorLog.Priority = 1;
            errorLog.Category = ex.Source;
            errorLog.Message = ex.Message;
            errorLog.InnerException = ex.StackTrace;
            icanSpeakContext.RX_Logs.InsertOnSubmit(errorLog);
            icanSpeakContext.SubmitChanges();

        }

        public static string RandomString(int Size)
        {
            Random random = new Random();
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, Size)
                                   .Select(x => input[random.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }
    }
}