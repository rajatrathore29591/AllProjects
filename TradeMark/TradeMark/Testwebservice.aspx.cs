using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TradeMark
{
    public partial class Testwebservice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string json = "aditi";
            Uri myUri = new Uri("http://10.10.10.170/TrademarkImportWebServices/Service1.svc/InsertTrademark/tmJsonData"+json);
            HttpWebRequest request = WebRequest.Create(myUri) as HttpWebRequest;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

        }
    }
}