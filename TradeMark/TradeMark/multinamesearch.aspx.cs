
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradeMark.App_Data;
using Microsoft.VisualBasic.FileIO;
using TradeMark.BAL;
using TradeMark.Models;
using System.Text;

namespace TradeMark
{
    public partial class multinamesearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Server.HtmlEncode(Request.QueryString["value"]) != null)
            {
                string Value = Server.HtmlEncode(Request.QueryString["value"]);
                if (Value == "Goods")
                {
                    try
                    {
                        SearchService oService = new SearchService();
                        DataTable dtGoodsServicesData = oService.BindddlGoodsServices();

                        StringBuilder JSONString = new StringBuilder();
                        if (dtGoodsServicesData.Rows.Count > 0)
                        {
                            string strCell = string.Empty;
                            JSONString.Append("[");
                            for (int i = 0; i < dtGoodsServicesData.Rows.Count; i++)
                            {
                                JSONString.Append("{");
                                for (int j = 0; j < dtGoodsServicesData.Columns.Count; j++)
                                {
                                    strCell = dtGoodsServicesData.Rows[i][j].ToString().Replace(@"\", "/");
                                    if (j < dtGoodsServicesData.Columns.Count - 1)
                                    {
                                        JSONString.Append("\"" + dtGoodsServicesData.Columns[j].ColumnName.ToString() + "\":" + "\"" + strCell.Replace('"', ' ').Trim().Replace("    ", "") + "\",");
                                    }
                                    else if (j == dtGoodsServicesData.Columns.Count - 1)
                                    {
                                        JSONString.Append("\"" + dtGoodsServicesData.Columns[j].ColumnName.ToString() + "\":" + "\"" + strCell + "\"");
                                    }
                                }
                                if (i == dtGoodsServicesData.Rows.Count - 1)
                                {
                                    JSONString.Append("}");
                                }
                                else
                                {
                                    JSONString.Append("},");
                                }
                            }
                            JSONString.Append("]");
                        }

                        Response.Clear();

                        Response.Write(JSONString.ToString());
                        Response.End();
                    }
                    catch (Exception ex)
                    {
                        string strError = ex.Message.ToString();
                    }

                }

            }
        }

        /// <summary>
        /// uploading image in project folder and inserting data in DB using SqlBulkCopy	 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUpload1.HasFile)
                {
                    string FileName = new FileInfo(FileUpload1.PostedFile.FileName).Name;
                    FileName = FileName.Substring(0, FileName.LastIndexOf("."));
                    string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                    string FolderPath = "~/Uploads/";

                    // uploading file in folder
                    string FilePath = Server.MapPath(FolderPath + FileName + ".csv");
                    FileUpload1.SaveAs(FilePath);
                    var dt = Import_To_Grid(FilePath, Extension, FileName);

                    // take note of SqlBulkCopyOptions.KeepIdentity , you may or may not want to use this for your situation.  
                    using (var bulkCopy = new SqlBulkCopy(SqlHelper.GetConnectionString(), SqlBulkCopyOptions.KeepIdentity))
                    {
                        if (dt != null)
                        {
                            // my DataTable column names match my SQL Column names, so I simply made this loop. However if your column names don't match, just pass in which datatable name matches the SQL column name in Column Mappings
                            foreach (DataColumn col in dt.Columns)
                            {
                                bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                            }
                            bulkCopy.BulkCopyTimeout = 600;
                            bulkCopy.DestinationTableName = "MarkToBeSearch";
                            bulkCopy.WriteToServer(dt);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// To read CSV file
        /// </summary>
        /// <param name="csv_file_path"></param>
        /// <returns> return the datatable</returns>
        private DataTable Import_To_Grid(string csv_file_path, string Extension, string FileName)
        {
            DataTable csvData = new DataTable();
            try
            {
                BulkSearchRequestModel objBulkSearchRequestModel = new BulkSearchRequestModel();

                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    //read column names
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }

                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);

                    }

                    // adding 3 default columns in datatable
                    System.Data.DataColumn RequestId = new System.Data.DataColumn("RequestId", typeof(System.String));
                    RequestId.DefaultValue = "1";
                    csvData.Columns.Add(RequestId);
                    System.Data.DataColumn CreatedBy = new System.Data.DataColumn("CreatedBy", typeof(System.Guid));
                    CreatedBy.DefaultValue = Session["UserID"].ToString();
                    csvData.Columns.Add(CreatedBy);
                    System.Data.DataColumn Status = new System.Data.DataColumn("Status", typeof(System.String));
                    Status.DefaultValue = "Pending";
                    csvData.Columns.Add(Status);

                    var rowsWithoutParent = csvData.AsEnumerable().Where(r => r["Mark"].ToString() == "" || r["Mark"].ToString() == null);
                    if (rowsWithoutParent.Count() > 0)
                    {
                        lblMessage.Text = "Invalid excel please check and upload again ";
                        csvData = null;
                    }
                    else
                    {
                        // insert the filled data to the model class
                        objBulkSearchRequestModel.UserId = Session["UserID"].ToString();
                        objBulkSearchRequestModel.Email = "";
                        objBulkSearchRequestModel.Status = "Pending";
                        objBulkSearchRequestModel.ExcelName = FileName;

                        //adding record in BulkSearchRequest
                        MultiSearchService oMultiSearchService = new MultiSearchService();
                        var Respone = oMultiSearchService.AddBulkSearchRequest(objBulkSearchRequestModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return csvData;
        }

    }
}