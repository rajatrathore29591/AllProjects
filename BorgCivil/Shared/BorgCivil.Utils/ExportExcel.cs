using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BorgCivil.Utils
{
    public class ExportExcel
    {
        public static void GetExportExcel(string filename, string excelHeader, DataTable dtExcel)
        {
            string attachment = string.Format("attachment; filename={0}", filename);
            var tw = new StringWriter();
            var hw = new HtmlTextWriter(tw);
            var dgGrid = new DataGrid { DataSource = dtExcel };
            dgGrid.DataBind();  // Report Header
            dgGrid.CssClass = "confr_table";
            hw.WriteLine("<b><u><font size='4'> " + excelHeader + " </font></u></b><br/><br/>");    //Get the HTML for the control.
            dgGrid.RenderControl(hw);  //Write the HTML back to the browser.
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", attachment);
            //this.EnableViewState = false;
            System.Web.HttpContext.Current.Response.Write(tw.ToString());
            System.Web.HttpContext.Current.Response.End();
        }


    }
}
