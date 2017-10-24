using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradeMark.BAL;
using TradeMark.Utility;
using System.Data.SqlClient;
using System.Configuration;
using TradeMark.Models;

namespace TradeMark
{
    public partial class PromoCode : System.Web.UI.Page
    {
        UserService objUserService = new UserService();
        public int promocodeId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if(Server.HtmlEncode(Request.QueryString["Id"]) != null)
                {
                    GetPromoCodeDetailsById(Server.HtmlEncode(Request.QueryString["Id"]));
                }

            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            PromoCodeModel objPromoCodeModel = new PromoCodeModel();
            objPromoCodeModel.Title = txtTitle.Text;
            objPromoCodeModel.PromoCode = txtPromoCode.Text;
            objPromoCodeModel.Price =Convert.ToDecimal(txtPrice.Text);
            objPromoCodeModel.Redeemlimit =Convert.ToInt32(txtRedeemlimit.Text);
            objUserService.SavePromoCode(objPromoCodeModel);
        }

        /// <summary>
        /// Method to get promo code details
        /// </summary>
        public void GetPromoCodeDetailsById(string promoCodeId)
        {
            PromoCodeModel objPromoCodeModel = new PromoCodeModel();
            objPromoCodeModel=objUserService.GetPromoCodeDetailsById(Convert.ToInt32(promoCodeId));
            txtTitle.Text = objPromoCodeModel.Title;
            txtPromoCode.Text = objPromoCodeModel.PromoCode;
            txtPrice.Text = objPromoCodeModel.Price.ToString();
            txtRedeemlimit.Text = objPromoCodeModel.Redeemlimit.ToString();

        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("search.aspx");
        }
    }
} 