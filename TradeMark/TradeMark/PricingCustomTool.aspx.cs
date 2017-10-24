using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TradeMark
{
    public partial class PricingCustomTool : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["UserName"]!=null)
            {
                var userName = Session["UserName"].ToString();
            }
            //var userId = Session["UserId"].ToString();
            //if(userId>0)
            //{
            //    btnBuySingleSearch.Visible = true;
            //}
        }
    }
}