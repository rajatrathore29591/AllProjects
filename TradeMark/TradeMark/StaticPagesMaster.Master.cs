using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradeMark.BAL;

namespace TradeMark
{

    public partial class StaticPagesMaster : System.Web.UI.MasterPage
    {
        public bool IsAdminUser = false;
        public int userCredits = 0;
        public bool isUserTrans = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                lblUserName1.Text = "Hi, " + Session["UserName"].ToString();
                IsAdminUser = Convert.ToBoolean(Session["IsAdmin"]);
                
                
            }
            if (Session["UserId"] != null)
            {
                UserService oUserService = new UserService();
                userCredits = oUserService.GetUserCreditsById(Session["UserId"].ToString());
                lblUserCredits.Text = userCredits.ToString();
                var userTransaction = oUserService.GetLastTransactionDetail(Session["UserId"].ToString());
                if (userTransaction != null && userTransaction.Credits > 0)
                {
                    isUserTrans = true;
                }
                else { isUserTrans = false; }
            }
        }
    }
}