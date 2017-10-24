using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TradeMark
{
    public partial class PaymentForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var amount = Request.QueryString["amount"].ToString();
            var Credits = Request.QueryString["credits"].ToString();
            var promoCode = Request.QueryString["promoCode"].ToString();
            hdnAmount.Value = amount;
            hdnPromoCode.Value = promoCode;
            hdnCredits.Value = Credits;
            txtCredits.Value = Credits;
            txtAmount.Value = amount;
        }
    }
}