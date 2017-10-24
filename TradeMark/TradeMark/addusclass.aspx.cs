using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradeMark.BAL;
using TradeMark.Models;

namespace TradeMark
{
    public partial class addusclass : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUSClassDescriptions_Click(object sender, EventArgs e)
        {
            USClassModel objUSClassModel = new USClassModel();

            // insert the filled data to the model class         
           objUSClassModel.USClassDescriptions = txtUSClassDescriptions.Text.Trim();
           objUSClassModel.ClassNo = txtClassNo.Text.Trim();
        
            // pass model object to the Business access layer
            USClassService oUSClassService = new USClassService();
            if (oUSClassService.AddUSClass(objUSClassModel))
            {
                lblSuccessMsg.Attributes["class"] = "successmsg";
                lblSuccessMsg.Text = "The US Class Description has been successfully inserted";
                txtUSClassDescriptions.Text = "";
                txtClassNo.Text = "";
            }
            else
            {
                lblSuccessMsg.Attributes["class"] = "lbl-red";
                lblSuccessMsg.Text = "The US Class Description are not register. please try again";
            }           
        }    
    }
}