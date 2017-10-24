using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
namespace TestApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        HttpWebRequest request;
        private void button1_Click(object sender, EventArgs e)
        {
           // {"email":"rahul@techvalens.com","password":"h"}

           // requestString = "{\"firstName\":\"rahul\",\"lastName\":\"pushpkar\",\"email\":\"rahul12@techvalens.com\",\"password\":\"h\",\"DOB\":\"03/06/1985\",\"gender\":\"Male\",\"nativeLanguage\":\"hindi\"}";
          //  string json = "{\"email\":\"svsvsvs@gmail.com\",\"DOB\":\"10/22/2014\",\"firstName\":\"svsvsv\",\"lastName\":\"svsvsvs\",\"gender\":\"Female\",\"Username\":\"svsvsvsvsv\",\"Country\":\"Barbados\",\"DeviceId\":\"\",\"profilePicture\":\"\"}";
            //string json = "{\"userid\":\"tgzp2orM2TekxkbQ2lJeiQ==\",\"bookmarkUrl\":\"\",\"courseType\":\"Grammer\",\"courseid\":\"IQhEoLP/dN3wEndxbtM3zw==\",\"courseName\":\"\"}";
            //string json = "{\"FacebookId\":\"1403744193284920\",\"email\":\"tvandroid31@gmail.com\",\"usertype\":\"1\",\"GoogleId\":\"\",\"TwitterId\":\"\",\"DeviceId\":\"\",\"imagebase64 \":\"\",\"DOB \":\"\",\"lastName \":\"Varma\",\"Username \":\"Amit\",\"gender \":\"female\",\"Country \":\"US\",\"firstName \":\"Amit\"}";
            //string json = "{\"TwitterId\":\"13\",\"email\":\"sadhuvishnu@gmail.com\",\"usertype\":\"3\",\"GoogleId\":\"110865292450540677401\",\"FacebookId\":\"\",\"DeviceId\":\"\",\"imagebase64 \":\"\",\"DOB \":\"\",\"lastName \":\"Vardhan\",\"Username \":\"Vishnu Vardhan\",\"gender \":\"\",\"Country \":\"\",\"firstName \":\"Vishnu\"}";
            string json = "{\"selectset\":\"3\"}";
            //string json = "{\"questionId\":\"tgzp2orM2TekxkbQ2lJeiQ==\",\"dialogId\":\"btTvQY/5RHrD9N79caWuAw==\",\"courseId\":\"btTvQY/5RHrD9N79caWuAw==\",\"coursetype\":\"dialog\",\"status\":\"true\",\"userId\":\"tgzp2orM2TekxkbQ2lJeiQ==\"}";
            request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetVocabQuestionBySet");
            //   request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/Services/Service.svc/MyActivityByUserId");
            var data = JsonConvert.DeserializeObject(json);
            string sb = JsonConvert.SerializeObject(data);
            request.Method = "POST";
            Byte[] bt = Encoding.UTF8.GetBytes(sb);
            Stream st = request.GetRequestStream();
            st.Write(bt, 0, bt.Length);
            st.Close();

            string strContent = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                string result = string.Empty;
                Stream responseStream = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(responseStream))
                {
                    strContent = sr.ReadToEnd();
                   
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string json = "{\"id\":42}";
            request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/EncryptId");
            //   request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/Services/Service.svc/GetVocabByUserIdDevice");
            var data = JsonConvert.DeserializeObject(json);
            string sb = JsonConvert.SerializeObject(data);
            request.Method = "POST";
            Byte[] bt = Encoding.UTF8.GetBytes(sb);
            Stream st = request.GetRequestStream();
            st.Write(bt, 0, bt.Length);
            st.Close();

            string strContent = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                string result = string.Empty;
                Stream responseStream = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(responseStream))
                {
                    strContent = sr.ReadToEnd();

                }
            }
        }
    }

    
}
