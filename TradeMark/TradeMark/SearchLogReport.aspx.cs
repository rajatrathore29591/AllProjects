using System;
using System.IO;
using System.Data;

namespace TradeMark
{
    public partial class SearchLogReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Convert.ToBoolean(Session["IsAdmin"]))
            {
                Response.Redirect("Search.aspx");
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
                "attachment;filename=SearchLog.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            StringWriter sw = new StringWriter();

            if (Session["dtlog"] != null)
            {
                // string strSearchResult = Session["htmlResult"].ToString();
                DataTable dtSeachLog = Session["dtlog"] as DataTable;
                
                
                string strSearchResult = "No Record Found!";

                if (dtSeachLog != null && dtSeachLog.Rows.Count > 0)
                {
                    strSearchResult = "<div class='table - responsive'> <table border='1' class='table table-bordered table - hover'><thead><tr>";
                    strSearchResult = strSearchResult + "<th class='header'>User Name</th> <th class='header'>Trade Mark</th><th class='header'>Date</th><th class='header'>Us Class Description</th>";
                    strSearchResult = strSearchResult + "</tr></thead><tbody>";
                    foreach (DataRow dr in dtSeachLog.Rows)
                    {

                        strSearchResult = strSearchResult + "<tr><td>" + dr["FullName"].ToString() + " </ a ></td>";

                        strSearchResult = strSearchResult + "<td align='center'> " + dr["SearchText"].ToString().Replace("''","'") + "</td>";
                        strSearchResult = strSearchResult + "<td>" +Convert.ToDateTime(dr["SearchDate"]).ToShortDateString() + "</td>";
                        strSearchResult = strSearchResult + "<td align='center'>" + dr["UsClassDescription"].ToString() + "</td>";
                        strSearchResult = strSearchResult + "</tr>";
                    }
                    strSearchResult = strSearchResult + "</tbody></table></div>";
                }
                    sw.WriteLine(strSearchResult);
                //HtmlTextWriter hw = new HtmlTextWriter(sw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        
    }
}