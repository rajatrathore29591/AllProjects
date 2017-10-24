using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradeMark.Models;

namespace TradeMark
{
    public partial class Payment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //Event for payment of package
        protected void btnPayment_Click(object sender, EventArgs e)
        {
            PaymentModel objPaymentModel = new PaymentModel();
            objPaymentModel.ssl_merchant_id = System.Configuration.ConfigurationManager.AppSettings["SslMerchantId"];
            objPaymentModel.ssl_user_id = System.Configuration.ConfigurationManager.AppSettings["SslUserId"];
            objPaymentModel.ssl_pin = System.Configuration.ConfigurationManager.AppSettings["SslPin"];
            try
            {
                //objPaymentModel.ssl_merchant_id = 
            }
            catch (Exception ex)
            {
            }
        }
    }
}