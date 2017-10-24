using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TradeMark
{
    public partial class PaymentStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var PaymentStatus = Request.QueryString["PaymentStatus"].ToString();
            var Credits = Request.QueryString["Credits"].ToString();
            var TransactionId = Request.QueryString["TransactionId"].ToString();
            lblTransactionId.InnerText = TransactionId;
            lblCredits.InnerText = Credits;
            lblPaymentStatus.InnerText = PaymentStatus;
        }
    }
}