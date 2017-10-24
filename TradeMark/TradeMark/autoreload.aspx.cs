using System;
using TradeMark.BAL;

namespace TradeMark
{
    public partial class autoreload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                UserService oUserService = new UserService();
                //Check if credits are 1 or less then auto reload can apply
                var currentCredits = oUserService.GetUserCreditsById(Session["UserId"].ToString());
                if (currentCredits <= 1)
                {
                    //var userId = new Guid(Session["UserId"].ToString());
                    //Get last transaction details 
                    var userTransaction = oUserService.GetLastTransactionDetail(Session["UserId"].ToString());
                    if (userTransaction != null && userTransaction.Credits > 0)
                    {
                        //Payment method
                        var charge = oUserService.MakePayment(userTransaction);
                        Response.Redirect("PaymentStatus.aspx?PaymentStatus=" + charge.PaymentStatus + "&Credits=" + charge.Credits + "&TransactionId=" + charge.TransactionId);
                    }
                    else
                    {
                        Response.Redirect("Search.aspx");
                    }
                }
                else
                {
                    Response.Redirect("Search.aspx");
                }
            }
            catch (Exception exc)
            {

            }
        }
    }
}