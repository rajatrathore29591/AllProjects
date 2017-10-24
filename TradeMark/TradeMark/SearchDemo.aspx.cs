using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.IO;
using System.Web.Hosting;
using System.Web.UI;
using TradeMark.BAL;
using TradeMark.Models;
using System.Text;
using System.Linq;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Configuration;

namespace TradeMark
{
    public partial class SearchDemo : System.Web.UI.Page
    {
        public string htmlResult = string.Empty;
        public string htmlResultFinal = string.Empty;
        const int nearX = 6;
        string grpscore = string.Empty;
        string gridColor = string.Empty;
        string groupName = string.Empty;
        static iTextSharp.text.Font HeaderFont = FontFactory.GetFont("gothic", 24, iTextSharp.text.Font.NORMAL, BaseColor.WHITE);
        public string cautionMessage = string.Empty;
        public string searchedScored = string.Empty;
        public bool importInporocess = false;
        public string latestFileImportDate = string.Empty;
        DataTable pdfDataTable = new DataTable();
        DataTable dtcalculateGrouping = new DataTable();
        string associatedUSclassNear5Condition = string.Empty;

        string[] goodsServicearray1;
        string[] goodsServicearray2;
        string[] goodsServicearray3;
        string[] goodsServicearray4;
        string[] goodsServicearray5;
        string[] goodsServicearray6;
        string[] goodsServicearray7;
        string[] goodsServicearray8;
        string[] goodsServicearray9;
        string[] goodsServicearray10;
        public string applicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"];
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                hdnApplicationUrl.Value = applicationUrl;
                if (Server.HtmlEncode(Request.QueryString["Guid"]) != null)
                {
                    hdnSearchGuid.Value = Server.HtmlEncode(Request.QueryString["Guid"]).ToString();
                    SearchService oSearchService = new SearchService();
                    System.Data.DataTable searchParamdt = oSearchService.GetSeachParams(Server.HtmlEncode(Request.QueryString["Guid"]).ToString());

                    string searOption = string.Empty;
                    string trademarkText = string.Empty;
                    string ComponentFullFormText = string.Empty;
                    string goodsServicesId = string.Empty;
                    string fullForm = string.Empty;

                    if (searchParamdt != null && searchParamdt.Rows.Count > 0)
                    {
                        foreach (DataRow drsearch in searchParamdt.Rows)
                        {
                            trademarkText = drsearch["SearchText"].ToString();
                            goodsServicesId = drsearch["UsClassDescriptionId"].ToString();
                            ComponentFullFormText = drsearch["ComponentsFullForm"].ToString();
                            searOption = drsearch["FilterOption"].ToString();
                            fullForm = drsearch["FullForm"].ToString();
                            hdnSearchoption.Value = searOption;
                            //Call method to get us class description
                            hdnGoodsName.Value = GetUSClassDescriptionByDescriptionId(goodsServicesId.Split(',')[1]);
                            hdnGoodsName.Value = hdnGoodsName.Value.Replace("'", "''");
                        }
                    }
                    hdnGoodsID.Value = goodsServicesId;
                    BindddlGoodsServices(goodsServicesId);
                    TradeMarkSearch(trademarkText.Trim(), goodsServicesId, ComponentFullFormText.Trim(), fullForm.Trim(), searOption);
                }
                else
                {
                    BindddlGoodsServices("");

                }

                //Call method to get the current status of importing and last imported xml file date
                UserService oUserService = new UserService();
                var adminSetting = oUserService.GetAdminSetting();
                if (adminSetting != null)
                {
                    importInporocess = adminSetting.StatusFlag;
                    latestFileImportDate = adminSetting.LatestFileImportDate;
                }

            }

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
        /// Click event of search button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            hdnCurrentDate.Value = DateTime.Now.ToString("MMMM dd, yyyy");
            string selectedUsClassId = hdnGoodsID.Value;
            string selectedUsClassDescription = hdnGoodsName.Value;
            string usClassId = selectedUsClassId.Split(',')[0].ToString();
            string uSClassDescriptionId = selectedUsClassId.Split(',')[1].ToString();

            if (!string.IsNullOrEmpty(txtMark.Text))
            {
                TradeMarkSearch(txtMark.Text.Trim(), selectedUsClassId, txtComponentFullForm.Text.Trim(), txtFullForm.Text.Trim(), hdnRdoOption.Value);
            }
        }

        /// <summary>
        /// Search the entered trademark 
        /// </summary>
        /// <param name="trademarkText"> trademark text </param>
        /// <param name="goodsServicesId">selected goods & services</param>
        /// <param name="ComponentFullFormText">component text or Full text search text </param>
        /// <param name="searchOption">Component or Full text</param>
        public void TradeMarkSearch(string trademarkText, string goodsServicesId, string ComponentFullFormText, string FullFormText, string searchOption)
        {
            btnSavedoc.Visible = false;
            btnSavePdf.Visible = false;
            btnsearchsave.Visible = false;
            btnTalkToAttorney.Visible = false;


            //Call method to get the current status of importing and last imported xml file date
            htmlResultFinal = string.Empty;
            string sqlQuery = string.Empty;
            DataSet objDataSet = new DataSet();
            DataSet dtResultdataSet = new DataSet();

            hdnGoodsName.Value = hdnGoodsName.Value.Replace("'", "''");

            UserService oUserService = new UserService();
            PluralizationService pluralservice = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));
            var adminSetting = oUserService.GetAdminSetting();
            if (adminSetting != null)
            {
                importInporocess = adminSetting.StatusFlag;
                latestFileImportDate = adminSetting.LatestFileImportDate;
            }
            if (trademarkText.Contains("'s"))
            {
                trademarkText = trademarkText.Replace("'s", "");
            }
            string mark = string.Empty;
            string seachWhereQuery = string.Empty;
            string selectedUsClassId = goodsServicesId;

            txtMark.Text = trademarkText;
            if (trademarkText.Contains("'"))
            {
                trademarkText = trademarkText.Replace("'", "''");
            }
            txtComponentFullForm.Text = ComponentFullFormText;
            txtFullForm.Text = FullFormText;
            if (ComponentFullFormText.Contains("'"))
            {
                ComponentFullFormText = ComponentFullFormText.Replace("'", "''");
            }

            string usClassId = selectedUsClassId.Split(',')[0].ToString();
            string uSClassDescriptionId = selectedUsClassId.Split(',')[1].ToString();
            string uSAssociatedClassIds = string.Empty;
            SearchService oService = new SearchService();

            string selectedUsClassDescription = hdnGoodsName.Value.Replace(", ", " ");

            if (Server.HtmlEncode(Request.QueryString["Guid"]) != null && selectedUsClassDescription == "")
            {
                //Get Us class description
                selectedUsClassDescription = oService.GetUsclassdescription(uSClassDescriptionId);
            }

            string[] arrayStringSAMEGS = hdnGoodsName.Value.Split(' ');
            string whereConditionSAMEGS = string.Empty;

            //Apply SAME if GS is single word 
            if (arrayStringSAMEGS.Length == 1)
            {
                for (int gs = 0; gs <= arrayStringSAMEGS.Length - 1; gs++)
                {

                    whereConditionSAMEGS = whereConditionSAMEGS + "(Casefilestatementstext like '%" + pluralservice.Singularize(arrayStringSAMEGS[gs]) + "%') OR";
                    whereConditionSAMEGS = whereConditionSAMEGS + "(Casefilestatementstext like '%" + pluralservice.Pluralize(arrayStringSAMEGS[gs]) + "%') OR";
                }
                if (!string.IsNullOrEmpty(whereConditionSAMEGS))
                {
                    whereConditionSAMEGS = whereConditionSAMEGS.Substring(0, whereConditionSAMEGS.Length - 2);
                }
            }
            //End logic

            var arryselectedUsClassDescription = selectedUsClassDescription.Split(' ');
            string near5ConditionString = string.Empty;

            DataTable dtResult = new DataTable();
            DataTable dtAssociatedUSClassIds = new DataTable();

            string near5Innerstring = string.Empty;

            if (!string.IsNullOrEmpty(trademarkText))
            {
                //trademark search , first option or default process to perfom initial search
                //NEAR 5 Search query
                if (arryselectedUsClassDescription.Length > 1)
                {
                    near5ConditionString = " AND (" + GetNear5String(selectedUsClassDescription) + ")";
                }
                //code start Checkbox: Abbreviation (INCREDIBLE PROTEIN SNACKS)
                var searchMark = string.Empty;

                if (!string.IsNullOrEmpty(ComponentFullFormText) && searchOption == "3")
                {
                    searchMark = "MarkIdentification LIKE '% " + trademarkText + " %' OR MarkIdentification LIKE '" + trademarkText + " %' OR MarkIdentification LIKE '" + pluralservice.Pluralize(trademarkText) + "" + " %' OR MarkIdentification LIKE '% " + pluralservice.Pluralize(trademarkText) + "' OR MarkIdentification LIKE '" + trademarkText + "''s" + "" + "%' OR MarkIdentification LIKE '" + trademarkText + "''s" + "" + " %' OR MarkIdentification LIKE '" + trademarkText + "' OR MarkIdentification LIKE '%-" + trademarkText + "' OR MarkIdentification LIKE '% " + trademarkText + "' OR MarkIdentification LIKE '%" + ComponentFullFormText + "%";
                }
                else if (!string.IsNullOrEmpty(FullFormText) && searchOption == "4")
                {
                    searchMark = "MarkIdentification LIKE '% " + trademarkText + " %' OR MarkIdentification LIKE '" + trademarkText + " %' OR MarkIdentification LIKE '" + pluralservice.Pluralize(trademarkText) + "" + " %' OR MarkIdentification LIKE '% " + pluralservice.Pluralize(trademarkText) + "" + " %' OR MarkIdentification LIKE '" + trademarkText + "''s" + " %' OR MarkIdentification LIKE '" + trademarkText + "''s" + "%'  OR MarkIdentification LIKE '" + trademarkText + "' OR MarkIdentification LIKE '%-" + trademarkText + "' OR MarkIdentification LIKE '% " + trademarkText + "' OR MarkIdentification LIKE '%" + FullFormText + "%";
                }
                else
                {
                    searchMark = "MarkIdentification LIKE '% " + trademarkText + " %' OR MarkIdentification LIKE '" + trademarkText + " %' OR MarkIdentification LIKE '" + pluralservice.Pluralize(trademarkText) + "" + " %' OR MarkIdentification LIKE '% " + pluralservice.Pluralize(trademarkText) + "" + " %' OR MarkIdentification LIKE '" + trademarkText + "''s" + " %' OR MarkIdentification LIKE '" + trademarkText + "''s" + "%'  OR MarkIdentification LIKE '" + trademarkText + "' OR MarkIdentification LIKE '%-" + trademarkText + "' OR MarkIdentification LIKE '% " + trademarkText;
                }
                //code end 

                seachWhereQuery = "(" + searchMark + "')  AND (USClassId LIKE'%" + usClassId + "%') " + near5ConditionString;
                if (!string.IsNullOrEmpty(whereConditionSAMEGS))
                {
                    seachWhereQuery = "(" + searchMark + "')  AND (USClassId LIKE'%" + usClassId + "%') AND (" + whereConditionSAMEGS + ")";
                }

                //Result Method to get trade marks
                dtResult = oService.GetTradeMark_EnterMark(seachWhereQuery);
                dtResult.DefaultView.Sort = "RegistrationNo" + " " + "desc";
                var dtResultDistinct = dtResult.DefaultView.ToTable(true, "OwnerName").Rows.Count;
                dtResult = dtResult.DefaultView.ToTable();
                //testing
                //dtResult = null;
                //Scoring, grouping, coloring for Group 1

                if (dtResult != null && dtResultDistinct > 0)
                {
                    //Red color
                    groupName = ScoringColoringModel.Group1;
                    gridColor = ScoringColoringModel.Red;

                    if (dtResultDistinct <= 2)
                    {
                        grpscore = ScoringColoringModel.grpscoreA5;
                    }
                    else { grpscore = ScoringColoringModel.grpscoreA4; }

                    htmlResultFinal = string.Empty;
                    htmlResultFinal = "<div class='table - responsive'> <table class='table table-bordered table - hover'><thead><tr>";
                    htmlResultFinal = htmlResultFinal + "<th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Serial Number</th> <th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Registration Number</th><th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Word Mark</th>";
                    htmlResultFinal = htmlResultFinal + "</tr></thead><tbody>";
                    if (dtResult != null || dtResult.Rows.Count > 0)
                    {
                        //call method to draw result HTML 
                        htmlResultFinal += GenerateHTML(dtResult, grpscore, gridColor, searchOption, ComponentFullFormText);
                    }
                }
                //search level 2 Component/fulltext text+ US class+ NEAR5/SAME
                //if no result then Components + associated US-class ids

                bool isRed = false;

                if ((dtResult == null || dtResult.Rows.Count == 0) && (searchOption == "2" || searchOption == "3" || searchOption == "4"))
                {

                    sqlQuery = string.Empty;
                    if (searchOption == "2" || searchOption == "4")
                    {
                        DataTable mainDatatable = new DataTable();

                        string qeryWithComponents = string.Empty;

                        if (!string.IsNullOrEmpty(ComponentFullFormText))
                        {
                            //needs to split with commas and make the query 
                            var arryComponents = txtComponentFullForm.Text.Split(',');
                            string[] queryWhereArray = new string[arryComponents.Length];
                            for (int component = 0; component < arryComponents.Length; component++)
                            {
                                qeryWithComponents = "";

                                qeryWithComponents = qeryWithComponents + " MarkIdentification LIKE '%" + pluralservice.Singularize(arryComponents[component].Trim()) + "%' OR";

                                qeryWithComponents = qeryWithComponents + " MarkIdentification LIKE '%" + pluralservice.Pluralize(arryComponents[component].Trim()) + " %' OR";

                                qeryWithComponents = qeryWithComponents.Substring(0, qeryWithComponents.Length - 2);

                                seachWhereQuery = " (" + qeryWithComponents + " )  AND (USClassId LIKE'%" + usClassId + "%')" + near5ConditionString;

                                if (!string.IsNullOrEmpty(whereConditionSAMEGS))
                                {
                                    seachWhereQuery = " (" + qeryWithComponents + " )  AND (USClassId LIKE'%" + usClassId + "%') AND (" + whereConditionSAMEGS + ")";
                                }

                                queryWhereArray[component] = seachWhereQuery;
                                sqlQuery += "select Top 10 SerialNo, Registrationno,Status,MarkIdentification,OwnerName from TradeMark where " + seachWhereQuery + " Order by Registrationno desc, SerialNo desc ";
                                sqlQuery += " ";
                            }//end for (int component = 0;
                        }//end if (!string.IsNullOrEmpty(ComponentFullFormText))

                        //Both option
                        if (searchOption == "4" && (!string.IsNullOrEmpty(FullFormText)))
                        {

                            var arryFullForm = txtFullForm.Text.Split(' ');
                            string qeryWithFullform = string.Empty;
                            //VD

                            for (int full = 0; full < arryFullForm.Length; full++)
                            {
                                qeryWithFullform = string.Empty;
                                if (full == 0)
                                {
                                    //For first index

                                    qeryWithFullform = qeryWithFullform + " MarkIdentification LIKE '" + arryFullForm[0].Replace(",", "") + "%'";
                                }
                                else if (full == arryFullForm.Length - 1)
                                {
                                    //For last words *Snacks  
                                    qeryWithFullform = qeryWithFullform + " MarkIdentification LIKE '%" + arryFullForm[arryFullForm.Length - 1].Replace(",", "") + "'";
                                }
                                else
                                {
                                    //For middle words *Protein*
                                    qeryWithFullform = qeryWithFullform + " MarkIdentification LIKE '%" + arryFullForm[full].Replace(",", "") + "%' ";
                                }

                                seachWhereQuery = " (" + qeryWithFullform + ")  AND  (USClassId LIKE '%" + usClassId + "%')" + near5ConditionString;
                                //Call method to get the condition string for Associated us class description with SAME concept

                                if (!string.IsNullOrEmpty(whereConditionSAMEGS))
                                {
                                    seachWhereQuery = " ('" + qeryWithFullform + ")  AND  (USClassId LIKE '%" + usClassId + "%') AND(" + whereConditionSAMEGS + ")";
                                }

                                sqlQuery += "select Top 10 SerialNo, Registrationno,Status,MarkIdentification,OwnerName from TradeMark where " + seachWhereQuery + " Order by Registrationno desc, SerialNo desc ";
                                sqlQuery += " ";
                            }

                        }

                        //end both
                        dtResultdataSet = null;
                        //execute query with dataset
                        dtResultdataSet = oService.GetTradeMark_EnterMarkDataSet(sqlQuery);

                        if (dtResultdataSet != null && dtResultdataSet.Tables.Count > 0)
                        {
                            var arryComponentsNew = txtComponentFullForm.Text.Split(',');
                            for (int component = 0; component < arryComponentsNew.Length; component++)
                            {
                                if (dtResultdataSet.Tables[component] != null && dtResultdataSet.Tables[component].Rows.Count > 0)
                                {
                                    if (component == 0)
                                    {
                                        htmlResultFinal = "<div class='table - responsive'> <table class='table table-bordered table - hover'><thead><tr>";
                                        htmlResultFinal = htmlResultFinal + "<th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Serial Number</th> <th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Registration Number</th><th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Word Mark</th>";
                                        htmlResultFinal = htmlResultFinal + "</tr></thead><tbody>";
                                    }
                                    if (string.IsNullOrEmpty(htmlResultFinal))
                                    {
                                        htmlResultFinal = "<div class='table - responsive'> <table class='table table-bordered table - hover'><thead><tr>";
                                        htmlResultFinal = htmlResultFinal + "<th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Serial Number</th> <th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Registration Number</th><th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Word Mark</th>";
                                        htmlResultFinal = htmlResultFinal + "</tr></thead><tbody>";
                                    }

                                    dtcalculateGrouping = null;
                                    objDataSet = dtResultdataSet;
                                    dtResult = dtResultdataSet.Tables[component];

                                    if (objDataSet != null && objDataSet.Tables.Count > 0)
                                    {
                                        dtcalculateGrouping = objDataSet.Tables[component];
                                        dtcalculateGrouping.DefaultView.Sort = "RegistrationNo" + " " + "desc";
                                        dtcalculateGrouping = dtcalculateGrouping.DefaultView.ToTable();
                                        dtResultDistinct = dtcalculateGrouping.DefaultView.ToTable(true, "OwnerName").Rows.Count;
                                    }

                                    groupName = ScoringColoringModel.Group2;

                                    if (dtResult != null && dtResultDistinct > 0 && dtResultDistinct <= 2)
                                    {
                                        //Scored a3 if 1 or 2 

                                        grpscore = ScoringColoringModel.grpscoreA3;
                                        gridColor = ScoringColoringModel.Red;
                                        isRed = true;
                                    }
                                    else
                                    { //if 3+

                                        grpscore = ScoringColoringModel.grpscoreA2;
                                        gridColor = ScoringColoringModel.Yellow;
                                    }
                                    //testing
                                    //dtResult = null;
                                    if (dtcalculateGrouping != null && dtcalculateGrouping.Rows.Count > 0)
                                    {
                                        htmlResultFinal += GenerateHTML(dtcalculateGrouping, grpscore, gridColor, searchOption, ComponentFullFormText);
                                    }
                                }//end for 
                                 //isRed = false then go to search level 3

                                if (isRed == false && searchOption == "4")
                                {
                                    DataTable dtresult3 = null;
                                    dtresult3 = SearchLevel3Hybride(null, uSClassDescriptionId, searchMark, searchOption, ComponentFullFormText, FullFormText, pluralservice);

                                }
                                else if (isRed == false)
                                {
                                    DataTable dtresult3 = null;
                                    dtresult3 = SearchLevel3(null, uSClassDescriptionId, searchMark, searchOption, ComponentFullFormText, FullFormText, pluralservice);

                                }
                            }
                        }
                    }//End option 2

                    //option 3, FullForm, Group 2, Full form+ Us class 
                    if (searchOption == "3" && (dtResult == null || dtResult.Rows.Count == 0))
                    {
                        DataTable mainDatatable = new DataTable();

                        //For Example: IPS, Intelligent Protein Snacks
                        //Intelligent* *Protein* *Snacks
                        string qeryWithFullform = string.Empty;
                        bool isRedColor = false;
                        if (!string.IsNullOrEmpty(ComponentFullFormText))
                        {
                            sqlQuery = string.Empty;
                            var fullFormarray = ComponentFullFormText.Split(' ');
                            for (int loop = 0; loop <= fullFormarray.Length - 1; loop++)
                            {
                                qeryWithFullform = string.Empty;
                                if (loop == 0)
                                {
                                    //For first index

                                    qeryWithFullform = qeryWithFullform + " MarkIdentification LIKE '" + fullFormarray[0].Replace(",", "") + "%'";
                                }
                                else if (loop == fullFormarray.Length - 1)
                                {
                                    //For last words *Snacks  
                                    qeryWithFullform = qeryWithFullform + " MarkIdentification LIKE '%" + fullFormarray[fullFormarray.Length - 1].Replace(",", "") + "'";
                                }
                                else
                                {
                                    //For middle words *Protein*
                                    qeryWithFullform = qeryWithFullform + " MarkIdentification LIKE '%" + fullFormarray[loop].Replace(",", "") + "%' ";
                                }

                                seachWhereQuery = " (" + qeryWithFullform + ")  AND (USClassId LIKE '%" + usClassId + "%')" + near5ConditionString;
                                if (!string.IsNullOrEmpty(whereConditionSAMEGS))
                                {
                                    seachWhereQuery = " (" + qeryWithFullform + ")  AND (USClassId LIKE '%" + usClassId + "%') AND(" + whereConditionSAMEGS + ")" + near5ConditionString;
                                }

                                sqlQuery += "Select Top 10 SerialNo, Registrationno,Status,MarkIdentification,OwnerName from TradeMark where " + seachWhereQuery + " Order by Registrationno desc, SerialNo desc";
                                sqlQuery += " ";
                            }
                        }

                        //execute query dtResultdataSet
                        dtResultdataSet = oService.GetTradeMark_EnterMarkDataSet(sqlQuery);

                        //Scoring, grouping, coloring for Group 1

                        if (dtResultdataSet != null && dtResultdataSet.Tables.Count > 0)
                        {

                            htmlResultFinal = "<div class='table - responsive'> <table class='table table-bordered table - hover'><thead><tr>";
                            htmlResultFinal = htmlResultFinal + "<th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Serial Number</th> <th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Registration Number</th><th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Word Mark</th>";
                            htmlResultFinal = htmlResultFinal + "</tr></thead><tbody>";

                            var arryComponentsAssociateNew = ComponentFullFormText.Split(' ');
                            objDataSet = null;
                            objDataSet = dtResultdataSet;

                            for (int component = 0; component < arryComponentsAssociateNew.Length; component++)
                            {

                                dtcalculateGrouping = null;

                                dtResult = dtResultdataSet.Tables[component];

                                if (objDataSet != null && objDataSet.Tables.Count > 0)
                                {

                                    dtcalculateGrouping = objDataSet.Tables[component];
                                    dtcalculateGrouping.DefaultView.Sort = "RegistrationNo" + " " + "desc";
                                    dtcalculateGrouping = dtcalculateGrouping.DefaultView.ToTable();
                                    dtResultDistinct = dtcalculateGrouping.DefaultView.ToTable(true, "OwnerName").Rows.Count;
                                }

                                groupName = ScoringColoringModel.Group2;
                                if (dtResult != null && dtResultDistinct <= 2)
                                {
                                    //Scored a3 if 1 or 2 

                                    grpscore = ScoringColoringModel.grpscoreA4;
                                    gridColor = ScoringColoringModel.Red;
                                    isRedColor = true;
                                }
                                else
                                { //if 3+

                                    grpscore = ScoringColoringModel.grpscoreA3;
                                    gridColor = ScoringColoringModel.Yellow;
                                }
                                htmlResultFinal += GenerateHTML(dtcalculateGrouping, grpscore, gridColor, searchOption, ComponentFullFormText);
                            }//end for 
                            //isRed = false then go to search level 3
                            if (isRedColor == false)
                            {
                                DataTable dtresult3 = null;
                                dtresult3 = SearchLevel3(null, uSClassDescriptionId, searchMark, searchOption, ComponentFullFormText, FullFormText, pluralservice);
                            }
                        }
                    }//End option 3

                }//End  option 2 & 3

                //Search level 3: MI+ Associate us class decription + Near5 + SAME
                //Testing
                //dtResult = null;
                string whereConditionSAMEASSOCIATED = string.Empty;
                if (dtResult == null || dtResult.Rows.Count == 0)
                {
                    DataTable dtresult3 = null;
                    dtresult3 = SearchLevel3(null, uSClassDescriptionId, searchMark, searchOption, ComponentFullFormText, FullFormText, pluralservice);
                }

                //option 2 , Search 4, MI+ Associate us class decription + Near5 + Components + SAME 
                if (searchOption == "2" && (dtResult == null || dtResult.Rows.Count == 0))
                {
                    DataTable mainDatatable = new DataTable();
                    //search by Components + Us-Class id
                    //if no result then Components + associated US-class ids 
                    sqlQuery = string.Empty;
                    string qeryWithComponents = string.Empty;

                    if (!string.IsNullOrEmpty(ComponentFullFormText))
                    {
                        //needs to split with commas and make the query 
                        var arryComponents = txtComponentFullForm.Text.Split(',');
                        for (int component = 0; component < arryComponents.Length; component++)
                        {
                            qeryWithComponents = qeryWithComponents + " OR MarkIdentification LIKE '%" + pluralservice.Singularize(arryComponents[component].Trim()) + "%' ";
                            qeryWithComponents = qeryWithComponents + " OR MarkIdentification LIKE '%" + pluralservice.Pluralize(arryComponents[component].Trim()) + "%' ";

                            qeryWithComponents = qeryWithComponents.Substring(0, qeryWithComponents.Length - 1);
                            //Scoring, grouping, coloring for Group 3:Mark Components + Associated Us-Class
                            //Search in the associate class
                            if ((dtResult == null || dtResult.Rows.Count == 0) && (!string.IsNullOrEmpty(uSAssociatedClassIds)))
                            {
                                seachWhereQuery = " (" + searchMark + "'" + qeryWithComponents + ")  AND (" + uSAssociatedClassIds + ")" + associatedUSclassNear5Condition;
                                //Call method to get the condition string for Associated us class description with SAME concept
                                whereConditionSAMEASSOCIATED = GenerateConditionSAMEForAssociated(Convert.ToInt32(uSClassDescriptionId), pluralservice);

                                if (!string.IsNullOrEmpty(whereConditionSAMEASSOCIATED))
                                {
                                    seachWhereQuery = " (" + searchMark + "'" + qeryWithComponents + ")  AND (" + uSAssociatedClassIds + ") AND (" + whereConditionSAMEASSOCIATED + ")" + associatedUSclassNear5Condition;
                                }
                            }
                            sqlQuery += "Select Top 10 SerialNo, Registrationno,Status,MarkIdentification,OwnerName from TradeMark where " + seachWhereQuery + " Order by Registrationno desc, SerialNo desc";
                            sqlQuery += " ";
                        }

                        //Get result with Associated US-Class 

                        //execute query
                        dtResultdataSet = oService.GetTradeMark_EnterMarkDataSet(sqlQuery);
                        objDataSet = null;
                        //if (dtResult != null && dtResult.Rows.Count > 0)
                        if (dtResultdataSet != null && dtResultdataSet.Tables.Count > 0)
                        {

                            objDataSet = dtResultdataSet;

                            var arryComponentsAssociateNew = txtComponentFullForm.Text.Split(',');
                            for (int component = 0; component < arryComponentsAssociateNew.Length; component++)
                            {
                                if (dtResultdataSet.Tables[component] != null && dtResultdataSet.Tables[component].Rows.Count > 0)
                                {
                                    if (component == 0)
                                    {
                                        htmlResultFinal = "<div class='table - responsive'> <table class='table table-bordered table - hover'><thead><tr>";
                                        htmlResultFinal = htmlResultFinal + "<th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Serial Number</th> <th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Registration Number</th><th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Word Mark</th>";
                                        htmlResultFinal = htmlResultFinal + "</tr></thead><tbody>";
                                    }
                                    if (string.IsNullOrEmpty(htmlResultFinal))
                                    {
                                        htmlResultFinal = "<div class='table - responsive'> <table class='table table-bordered table - hover'><thead><tr>";
                                        htmlResultFinal = htmlResultFinal + "<th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Serial Number</th> <th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Registration Number</th><th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Word Mark</th>";
                                        htmlResultFinal = htmlResultFinal + "</tr></thead><tbody>";
                                    }
                                    dtcalculateGrouping = null;

                                    dtResult = dtResultdataSet.Tables[component];

                                    if (objDataSet != null && objDataSet.Tables.Count > 0)
                                    {
                                        dtcalculateGrouping = objDataSet.Tables[component];
                                        dtcalculateGrouping.DefaultView.Sort = "RegistrationNo" + " " + "desc";
                                        dtcalculateGrouping = dtcalculateGrouping.DefaultView.ToTable();
                                        dtResultDistinct = dtcalculateGrouping.DefaultView.ToTable(true, "OwnerName").Rows.Count;
                                    }

                                    //Scoring, grouping, coloring for Group 4:Mark Components + Associated Us-class
                                    groupName = ScoringColoringModel.Group4;
                                    if (dtResult != null && dtResultDistinct <= 2)
                                    {
                                        //Scored a3 if 1 or 2 
                                        grpscore = ScoringColoringModel.grpscoreA2;
                                        gridColor = ScoringColoringModel.Yellow;
                                    }
                                    else
                                    { //if 3+
                                        grpscore = ScoringColoringModel.grpscoreA1;
                                        gridColor = ScoringColoringModel.Green;
                                    }
                                    htmlResultFinal += GenerateHTML(dtcalculateGrouping, grpscore, gridColor, searchOption, ComponentFullFormText);
                                }
                            }
                        }
                    }

                }

                //option 3 Search 4, FullForm + MI+Associated US class description + Near 5+ SAME
                if (searchOption == "3" && (dtResult == null || dtResult.Rows.Count == 0) && !string.IsNullOrEmpty(uSAssociatedClassIds))
                {

                    //For Example: IPS, Intelligent Protein Snacks
                    //Intelligent* *Protein* *Snacks
                    string qeryWithFullform = string.Empty;

                    sqlQuery = string.Empty;

                    searchMark = "MarkIdentification LIKE '% " + trademarkText + " %' OR MarkIdentification LIKE '" + trademarkText + " %' OR MarkIdentification LIKE '" + trademarkText + "s" + "%' OR MarkIdentification LIKE '" + trademarkText + "es" + "%' OR MarkIdentification LIKE '" + trademarkText + "''s" + "%' OR MarkIdentification LIKE '" + trademarkText + "ies" + " %' OR MarkIdentification LIKE '" + trademarkText + "' OR MarkIdentification LIKE '% " + trademarkText;
                    if (!string.IsNullOrEmpty(ComponentFullFormText))
                    {
                        var fullFormarray = ComponentFullFormText.Split(' ');

                        whereConditionSAMEASSOCIATED = GenerateConditionSAMEForAssociated(Convert.ToInt32(uSClassDescriptionId), pluralservice);
                        for (int loop = 0; loop <= fullFormarray.Length - 1; loop++)
                        {
                            qeryWithFullform = string.Empty;
                            if (loop == 0)
                            {
                                //For first index

                                qeryWithFullform = qeryWithFullform + " MarkIdentification LIKE '" + fullFormarray[0].Replace(",", "") + "%'";
                            }
                            else if (loop == fullFormarray.Length - 1)
                            {
                                //For last words *Snacks  
                                qeryWithFullform = qeryWithFullform + " MarkIdentification LIKE '%" + fullFormarray[fullFormarray.Length - 1].Replace(",", "") + "'";
                            }
                            else
                            {
                                //For middle words *Protein*
                                qeryWithFullform = qeryWithFullform + " MarkIdentification LIKE '%" + fullFormarray[loop].Replace(",", "") + "%' ";
                            }

                            seachWhereQuery = " (" + searchMark + "'" + qeryWithFullform + ")  AND  (" + uSAssociatedClassIds + ")" + associatedUSclassNear5Condition;
                            //Call method to get the condition string for Associated us class description with SAME concept

                            if (!string.IsNullOrEmpty(whereConditionSAMEASSOCIATED))
                            {
                                seachWhereQuery = " (" + searchMark + "'" + qeryWithFullform + ")  AND  (" + uSAssociatedClassIds + ") AND(" + whereConditionSAMEASSOCIATED + ")" + associatedUSclassNear5Condition;
                            }

                            sqlQuery += "Select Top 10 SerialNo, Registrationno,Status,MarkIdentification,OwnerName from TradeMark where " + seachWhereQuery + " Order by Registrationno desc, SerialNo desc";
                            sqlQuery += " ";
                        }
                    }
                    //Search in the associate class

                    //Get result with Associated US-Class 
                    dtResultdataSet = oService.GetTradeMark_EnterMarkDataSet(sqlQuery);

                    //Scoring, grouping, coloring for Group 2
                    if (dtResultdataSet != null && dtResultdataSet.Tables.Count > 0)
                    {
                        htmlResultFinal = "<div class='table - responsive'> <table class='table table-bordered table - hover'><thead><tr>";
                        htmlResultFinal = htmlResultFinal + "<th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Serial Number</th> <th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Registration Number</th><th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Word Mark</th>";
                        htmlResultFinal = htmlResultFinal + "</tr></thead><tbody>";

                        objDataSet = null;
                        objDataSet = dtResultdataSet;
                        var arryComponentsAssociateNew = ComponentFullFormText.Split(' ');
                        for (int component = 0; component < arryComponentsAssociateNew.Length; component++)
                        {
                            dtResult = dtResultdataSet.Tables[component];
                            dtcalculateGrouping = null;

                            objDataSet = dtResultdataSet;
                            if (objDataSet != null && objDataSet.Tables.Count > 0)
                            {
                                dtcalculateGrouping = objDataSet.Tables[component];
                                dtcalculateGrouping.DefaultView.Sort = "RegistrationNo" + " " + "desc";
                                dtcalculateGrouping = dtcalculateGrouping.DefaultView.ToTable();
                                dtResultDistinct = dtcalculateGrouping.DefaultView.ToTable(true, "OwnerName").Rows.Count;
                            }

                            groupName = ScoringColoringModel.Group4;
                            if (dtResult != null && dtResultDistinct <= 2)
                            {
                                //Scored a3 if 1 or 2 
                                grpscore = ScoringColoringModel.grpscoreA2;
                                gridColor = ScoringColoringModel.Yellow;
                            }
                            else
                            { //if 3+
                                grpscore = ScoringColoringModel.grpscoreA1;
                                gridColor = ScoringColoringModel.Green;
                            }
                            htmlResultFinal += GenerateHTML(dtcalculateGrouping, grpscore, gridColor, searchOption, ComponentFullFormText);
                        }
                    }
                }

                //option 4 with Level 4
                else if (searchOption == "4" && (dtResult == null || dtResult.Rows.Count == 0) && !string.IsNullOrEmpty(uSAssociatedClassIds))
                {

                    sqlQuery = string.Empty;
                    string qeryWithComponents = string.Empty;


                    if (!string.IsNullOrEmpty(ComponentFullFormText))
                    {
                        //needs to split with commas and make the query 
                        var arryComponents = txtComponentFullForm.Text.Split(',');
                        for (int component = 0; component < arryComponents.Length; component++)
                        {
                            qeryWithComponents = qeryWithComponents + " OR MarkIdentification LIKE '%" + pluralservice.Singularize(arryComponents[component].Trim()) + "%' ";
                            qeryWithComponents = qeryWithComponents + " OR MarkIdentification LIKE '%" + pluralservice.Pluralize(arryComponents[component].Trim()) + "%' ";

                            qeryWithComponents = qeryWithComponents.Substring(0, qeryWithComponents.Length - 1);
                            //Scoring, grouping, coloring for Group 3:Mark Components + Associated Us-Class
                            //Search in the associate class
                            if ((dtResult == null || dtResult.Rows.Count == 0) && (!string.IsNullOrEmpty(uSAssociatedClassIds)))
                            {
                                seachWhereQuery = " (" + searchMark + "'" + qeryWithComponents + ")  AND (" + uSAssociatedClassIds + ")" + associatedUSclassNear5Condition;
                                //Call method to get the condition string for Associated us class description with SAME concept
                                whereConditionSAMEASSOCIATED = GenerateConditionSAMEForAssociated(Convert.ToInt32(uSClassDescriptionId), pluralservice);

                                if (!string.IsNullOrEmpty(whereConditionSAMEASSOCIATED))
                                {
                                    seachWhereQuery = " (" + searchMark + "'" + qeryWithComponents + ")  AND (" + uSAssociatedClassIds + ") AND (" + whereConditionSAMEASSOCIATED + ")" + associatedUSclassNear5Condition;
                                }
                            }
                            sqlQuery += "Select Top 10 SerialNo, Registrationno,Status,MarkIdentification,OwnerName from TradeMark where " + seachWhereQuery + " Order by Registrationno desc, SerialNo desc";
                            sqlQuery += " ";
                        }
                    }


                    if (!string.IsNullOrEmpty(FullFormText))
                    {
                        var fullFormarray = FullFormText.Split(' ');
                        var qeryWithFullform = string.Empty;
                        whereConditionSAMEASSOCIATED = GenerateConditionSAMEForAssociated(Convert.ToInt32(uSClassDescriptionId), pluralservice);
                        for (int loop = 0; loop <= fullFormarray.Length - 1; loop++)
                        {
                            qeryWithFullform = string.Empty;
                            if (loop == 0)
                            {
                                //For first index

                                qeryWithFullform = qeryWithFullform + " MarkIdentification LIKE '" + fullFormarray[0].Replace(",", "") + "%'";
                            }
                            else if (loop == fullFormarray.Length - 1)
                            {
                                //For last words *Snacks  
                                qeryWithFullform = qeryWithFullform + " MarkIdentification LIKE '%" + fullFormarray[fullFormarray.Length - 1].Replace(",", "") + "'";
                            }
                            else
                            {
                                //For middle words *Protein*
                                qeryWithFullform = qeryWithFullform + " MarkIdentification LIKE '%" + fullFormarray[loop].Replace(",", "") + "%' ";
                            }

                            seachWhereQuery = " (" + searchMark + "'" + qeryWithFullform + ")  AND  (" + uSAssociatedClassIds + ")" + associatedUSclassNear5Condition;
                            //Call method to get the condition string for Associated us class description with SAME concept

                            if (!string.IsNullOrEmpty(whereConditionSAMEASSOCIATED))
                            {
                                seachWhereQuery = " (" + searchMark + "'" + qeryWithFullform + ")  AND  (" + uSAssociatedClassIds + ") AND(" + whereConditionSAMEASSOCIATED + ")" + associatedUSclassNear5Condition;
                            }

                            sqlQuery += "Select Top 10 SerialNo, Registrationno,Status,MarkIdentification,OwnerName from TradeMark where " + seachWhereQuery + " Order by Registrationno desc, SerialNo desc";
                            sqlQuery += " ";
                        }
                    }



                    dtResultdataSet = oService.GetTradeMark_EnterMarkDataSet(sqlQuery);

                    //Scoring, grouping, coloring for Group 2
                    if (dtResultdataSet != null && dtResultdataSet.Tables.Count > 0)
                    {
                        htmlResultFinal = "<div class='table - responsive'> <table class='table table-bordered table - hover'><thead><tr>";
                        htmlResultFinal = htmlResultFinal + "<th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Serial Number</th> <th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Registration Number</th><th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Word Mark</th>";
                        htmlResultFinal = htmlResultFinal + "</tr></thead><tbody>";

                        objDataSet = null;
                        objDataSet = dtResultdataSet;
                        var arryComponentsAssociateNew = ComponentFullFormText.Split(' ');
                        for (int component = 0; component < arryComponentsAssociateNew.Length; component++)
                        {
                            dtResult = dtResultdataSet.Tables[component];
                            dtcalculateGrouping = null;

                            objDataSet = dtResultdataSet;
                            if (objDataSet != null && objDataSet.Tables.Count > 0)
                            {
                                dtcalculateGrouping = objDataSet.Tables[component];
                                dtcalculateGrouping.DefaultView.Sort = "RegistrationNo" + " " + "desc";
                                dtcalculateGrouping = dtcalculateGrouping.DefaultView.ToTable();
                                dtResultDistinct = dtcalculateGrouping.DefaultView.ToTable(true, "OwnerName").Rows.Count;
                            }

                            groupName = ScoringColoringModel.Group4;
                            if (dtResult != null && dtResultDistinct <= 2)
                            {
                                //Scored a3 if 1 or 2 
                                grpscore = ScoringColoringModel.grpscoreA2;
                                gridColor = ScoringColoringModel.Yellow;
                            }
                            else
                            { //if 3+
                                grpscore = ScoringColoringModel.grpscoreA1;
                                gridColor = ScoringColoringModel.Green;
                            }
                            htmlResultFinal += GenerateHTML(dtcalculateGrouping, grpscore, gridColor, searchOption, ComponentFullFormText);
                        }
                    }

                }



                if (dtResult == null || dtResult.Rows.Count == 0)
                {
                    htmlResultFinal += GenerateHTML(dtResult, grpscore, gridColor, searchOption, ComponentFullFormText);
                }
                //Method to save search log
                oUserService.SaveSearchLog(trademarkText, ComponentFullFormText, searchOption, uSClassDescriptionId, Session["UserId"].ToString());
            }
            htmlResultFinal += "</tbody></table></div>";

            divSearchResult.InnerHtml = cautionMessage + htmlResultFinal;
        }
        /// <summary>
        /// method for search level 3
        /// </summary>
        /// <param name="dtResultGroup2"></param>
        /// <param name="uSClassDescriptionId"></param>
        /// <param name="searchMark"></param>
        /// <param name="searchOption"></param>
        /// <param name="ComponentFullFormText"></param>
        /// <returns></returns>
        public DataTable SearchLevel3(DataTable dtResultGroup2, string uSClassDescriptionId, string searchMark, string searchOption, string ComponentFullFormText, string FullFormText, PluralizationService pluralservice)
        {
            //Search level 3: MI+ Associate us class decription + Near5 + SAME
            //Testing
            //dtResult = null;
            string whereConditionSAMEASSOCIATED = string.Empty;
            DataTable dtResultLevel3 = null;
            if (dtResultGroup2 == null || dtResultGroup2.Rows.Count == 0)
            {
                string uSAssociatedClassIds = string.Empty;
                string seachWhereQuery = string.Empty;

                SearchService oService = new SearchService();
                //method to get associated us-class ids in string format
                uSAssociatedClassIds = GetAssociatedUsclassIdsString(Convert.ToInt32(uSClassDescriptionId));
                int dtResultDistinct = 0;
                if (!string.IsNullOrEmpty(uSAssociatedClassIds))
                {
                    seachWhereQuery = "(" + searchMark + "')  AND (" + uSAssociatedClassIds + ")" + associatedUSclassNear5Condition;
                    //Call method to get the condition string for Associated us class description with SAME concept
                    whereConditionSAMEASSOCIATED = GenerateConditionSAMEForAssociated(Convert.ToInt32(uSClassDescriptionId), pluralservice);
                    if (!string.IsNullOrEmpty(whereConditionSAMEASSOCIATED))
                    {
                        seachWhereQuery = "(" + searchMark + "')  AND (" + uSAssociatedClassIds + ") AND (" + whereConditionSAMEASSOCIATED + ")" + associatedUSclassNear5Condition;
                    }
                    //Get result with Associated US-Class ids
                    dtResultLevel3 = oService.GetTradeMark_EnterMark(seachWhereQuery);
                    dtResultLevel3.DefaultView.Sort = "RegistrationNo" + " " + "desc";
                    dtResultLevel3 = dtResultLevel3.DefaultView.ToTable();
                    dtResultDistinct = dtResultLevel3.DefaultView.ToTable(true, "OwnerName").Rows.Count;
                }

                //Scoring, grouping, coloring for Group 2
                if (dtResultLevel3 != null && dtResultDistinct > 0)
                {

                    groupName = ScoringColoringModel.Group3;

                    if (dtResultDistinct <= 2)
                    {
                        //Scored a3 if 1 or 2 
                        grpscore = ScoringColoringModel.grpscoreA3;
                        gridColor = ScoringColoringModel.Red;
                    }
                    else
                    { //if 3+
                        grpscore = ScoringColoringModel.grpscoreA2;
                        gridColor = ScoringColoringModel.Yellow;
                    }
                    if (gridColor == ScoringColoringModel.Red)
                    {
                        htmlResultFinal = string.Empty;
                        htmlResultFinal = "<div class='table - responsive'> <table class='table table-bordered table - hover'><thead><tr>";
                        htmlResultFinal = htmlResultFinal + "<th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Serial Number</th> <th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Registration Number</th><th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Word Mark</th>";
                        htmlResultFinal = htmlResultFinal + "</tr></thead><tbody>";
                        //call method to draw result HTML
                        htmlResultFinal += GenerateHTML(dtResultLevel3, grpscore, gridColor, searchOption, ComponentFullFormText);
                        dtResultLevel3 = null;
                    }
                }

            }
            return dtResultLevel3;
        }
        /// <summary>
        /// method for hybride option level 3
        /// </summary>
        /// <param name="dtResultGroup2"></param>
        /// <param name="uSClassDescriptionId"></param>
        /// <param name="searchMark"></param>
        /// <param name="searchOption"></param>
        /// <param name="ComponentFullFormText"></param>
        /// <param name="FullFormText"></param>
        /// <param name="pluralservice"></param>
        /// <returns></returns>
        public DataTable SearchLevel3Hybride(DataTable dtResultGroup2, string uSClassDescriptionId, string searchMark, string searchOption, string ComponentFullFormText, string FullFormText, PluralizationService pluralservice)
        {
            //Search level 3: MI+ Associate us class decription + Near5 or SAME + full form
            //Testing
            //dtResult = null;
            string whereConditionSAMEASSOCIATED = string.Empty;
            DataSet dtResultLevel3 = null;
            DataTable dtResult = null;

            if (dtResultGroup2 == null || dtResultGroup2.Rows.Count == 0)
            {
                string uSAssociatedClassIds = string.Empty;
                string seachWhereQuery = string.Empty;
                string sqlQuery = string.Empty;

                SearchService oService = new SearchService();
                //method to get associated us-class ids in string format
                uSAssociatedClassIds = GetAssociatedUsclassIdsString(Convert.ToInt32(uSClassDescriptionId));
                int dtResultDistincount = 0;
                var arryFullForm = FullFormText.Split(' ');
                if (!string.IsNullOrEmpty(uSAssociatedClassIds))
                {
                    //seachWhereQuery = "(" + searchMark + "')  AND (" + uSAssociatedClassIds + ")" + associatedUSclassNear5Condition;
                    //Call method to get the condition string for Associated us class description with SAME concept
                    whereConditionSAMEASSOCIATED = GenerateConditionSAMEForAssociated(Convert.ToInt32(uSClassDescriptionId), pluralservice);
                }
                //if (!string.IsNullOrEmpty(whereConditionSAMEASSOCIATED))
                //{
                //    seachWhereQuery = "(" + searchMark + ")  AND (" + uSAssociatedClassIds + ") AND (" + whereConditionSAMEASSOCIATED + ")" + associatedUSclassNear5Condition;
                //}
                //Both option
                if (!string.IsNullOrEmpty(FullFormText))
                {

                    string qeryWithFullform = string.Empty;

                    seachWhereQuery = "(" + searchMark + "') ";

                    for (int full = 0; full < arryFullForm.Length; full++)
                    {
                        qeryWithFullform = string.Empty;
                        if (full == 0)
                        {
                            //For first index

                            qeryWithFullform = qeryWithFullform + " MarkIdentification LIKE '" + arryFullForm[0].Replace(",", "") + "%'";
                        }
                        else if (full == arryFullForm.Length - 1)
                        {
                            //For last words *Snacks  
                            qeryWithFullform = qeryWithFullform + " MarkIdentification LIKE '%" + arryFullForm[arryFullForm.Length - 1].Replace(",", "") + "'";
                        }
                        else
                        {
                            //For middle words *Protein*
                            qeryWithFullform = qeryWithFullform + " MarkIdentification LIKE '%" + arryFullForm[full].Replace(",", "") + "%' ";
                        }

                        seachWhereQuery = "(" + searchMark + "' OR " + qeryWithFullform + ")   AND (" + whereConditionSAMEASSOCIATED + ")" + associatedUSclassNear5Condition;
                        //Call method to get the condition string for Associated us class description with SAME concept
                        sqlQuery += "select Top 10 SerialNo, Registrationno,Status,MarkIdentification,OwnerName from TradeMark where " + seachWhereQuery + " Order by Registrationno desc, SerialNo desc ";
                        sqlQuery += " ";
                    }

                }

                //end both
                //Get result with Associated US-Class ids

                dtResultLevel3 = oService.GetTradeMark_EnterMarkDataSet(sqlQuery);

                //Scoring, grouping, coloring for Group 1

                if (dtResultLevel3 != null && dtResultLevel3.Tables.Count > 0)
                {

                    htmlResultFinal = "<div class='table - responsive'> <table class='table table-bordered table - hover'><thead><tr>";
                    htmlResultFinal = htmlResultFinal + "<th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Serial Number</th> <th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Registration Number</th><th style='background-color:#eaeaea;font-size:14px; padding: 5px; text-align: center; vertical-align: middle; color:#000;'>Word Mark</th>";
                    htmlResultFinal = htmlResultFinal + "</tr></thead><tbody>";


                    DataSet objDataSet = null;
                    objDataSet = dtResultLevel3;

                    for (int full = 0; full < arryFullForm.Length; full++)
                    {

                        dtcalculateGrouping = null;
                        dtResult = dtResultLevel3.Tables[full];

                        if (objDataSet != null && objDataSet.Tables.Count > 0)
                        {
                            dtcalculateGrouping = objDataSet.Tables[full];
                            dtcalculateGrouping.DefaultView.Sort = "RegistrationNo" + " " + "desc";
                            dtcalculateGrouping = dtcalculateGrouping.DefaultView.ToTable();
                            dtResultDistincount = dtcalculateGrouping.DefaultView.ToTable(true, "OwnerName").Rows.Count;
                        }

                        groupName = ScoringColoringModel.Group2;
                        if (dtResult != null && dtResultDistincount <= 2)
                        {
                            //Scored a3 if 1 or 2 
                            grpscore = ScoringColoringModel.grpscoreA4;
                            gridColor = ScoringColoringModel.Red;
                        }
                        else
                        { //if 3+
                            grpscore = ScoringColoringModel.grpscoreA3;
                            gridColor = ScoringColoringModel.Yellow;
                        }
                        htmlResultFinal += GenerateHTML(dtcalculateGrouping, grpscore, gridColor, searchOption, ComponentFullFormText);
                    }//end for 
                }
            }
            return dtResult;
        }


        /// <summary>
        /// Filter result table as per entered component and full text i.e. Cali, Burger
        /// </summary>
        /// <param name="mainDatatable">Final result table</param>
        /// <param name="arryComponentsNew">Component or full text array</param>
        /// <returns></returns>
        public DataSet FilterDatatable(DataTable mainDatatable, string arryComponentsNew)
        {
            DataRow[] drFilter = mainDatatable.Select("MarkIdentification like '%" + arryComponentsNew.Trim() + "%'");
            DataSet objDataSet = new DataSet();

            DataTable dtResult = new DataTable();
            if (drFilter.Count() > 0)
            {
                dtResult = drFilter.CopyToDataTable();

                var row = mainDatatable.AsEnumerable().Except(dtResult.AsEnumerable(), DataRowComparer.Default);
                if (row.Count() > 0)
                {
                    mainDatatable = row.CopyToDataTable();
                    objDataSet.Tables.Add(mainDatatable);
                }

                objDataSet.Tables.Add(dtResult);

            }
            return objDataSet;
        }
        /// <summary>
        /// return US-Class ids by usClassDescriptionid
        /// </summary>
        /// <param name="uSClassDescriptionId"> us calss decription id</param>
        /// <returns></returns>
        protected string GetAssociatedUsclassIdsString(int uSClassDescriptionId)
        {
            SearchService oService = new SearchService();
            string uSAssociatedClassIds = string.Empty;
            string usClassAssociatedIds = string.Empty;
            //method to get associated us-class ids in datatable
            DataTable dtAssociatedUSClassIds = oService.GetAssociatedUsclassIds(Convert.ToInt32(uSClassDescriptionId));

            //generate associated us-class ids string with comma separated to execute in the query to get Associated result
            if (dtAssociatedUSClassIds != null && dtAssociatedUSClassIds.Rows.Count > 0)
            {
                foreach (DataRow dr in dtAssociatedUSClassIds.Rows)
                {
                    uSAssociatedClassIds = uSAssociatedClassIds + " USClassId like  '%" + dr["ClassNo"].ToString() + "%' OR";
                    usClassAssociatedIds = usClassAssociatedIds + dr["ClassNo"].ToString() + ",";
                }
                uSAssociatedClassIds = uSAssociatedClassIds.Substring(0, uSAssociatedClassIds.Length - 2);
                usClassAssociatedIds = usClassAssociatedIds.Substring(0, usClassAssociatedIds.Length - 1);
                GetNear5ConditionStringForAssociatedUsClass(Convert.ToInt32(uSClassDescriptionId));
            }
            return uSAssociatedClassIds;
        }

        /// <summary>
        /// method to generate the Near 5 Condition string for Associated US-Class ids
        /// </summary>
        /// <param name="uSClassDescriptionId"></param>
        /// <returns></returns>
        protected void GetNear5ConditionStringForAssociatedUsClass(int usClassDescriptionId)
        {
            //NEAR 5 where Search query for associated us class ids

            associatedUSclassNear5Condition = string.Empty;

            //method to get associated us-class ids in datatable
            SearchService oService = new SearchService();
            DataTable dtAssociatedUSClassDescription = oService.GetAssociatedUsclassDescriptions(usClassDescriptionId, "Yes");
            string near5Innerstring = string.Empty;
            string usDescription = string.Empty;

            if (dtAssociatedUSClassDescription != null && dtAssociatedUSClassDescription.Rows.Count > 0)
            {
                foreach (DataRow dr in dtAssociatedUSClassDescription.Rows)
                {

                    usDescription = dr["USClassDescription"].ToString().Replace(",", " ");

                    var arrayDescription = usDescription.Split(' ');

                    if (arrayDescription.Length > 1)
                    {
                        near5Innerstring = GetNear5String(usDescription);
                        if (!string.IsNullOrEmpty(near5Innerstring))
                        {
                            associatedUSclassNear5Condition = associatedUSclassNear5Condition + near5Innerstring + " OR";
                            near5Innerstring = string.Empty;
                        }
                    }
                    //-------
                }
                associatedUSclassNear5Condition = associatedUSclassNear5Condition.Substring(0, associatedUSclassNear5Condition.Length - 2);
                associatedUSclassNear5Condition = " AND (" + associatedUSclassNear5Condition + ")";

            }
        }
        /// <summary>
        /// Method to generate the SAME condition string for Associated Us Class description ids
        /// </summary>
        /// <param name="usClassDescriptionId"></param>
        protected string GenerateConditionSAMEForAssociated(int usClassDescriptionId, PluralizationService pluralservice)
        {
            //method to get associated us-class ids in datatable
            SearchService oService = new SearchService();
            DataTable dtAssociatedUSClassDescription = oService.GetAssociatedUsclassDescriptions(usClassDescriptionId, "No");
            string whereConditionSAMEGS = string.Empty;
            string description = string.Empty;

            //Logic to generate the equation for GS example: SAME Battery connectors)[GS]: like Battery or like connetors
            if (dtAssociatedUSClassDescription != null && dtAssociatedUSClassDescription.Rows.Count > 0)
            {

                foreach (DataRow dr in dtAssociatedUSClassDescription.Rows)
                {
                    description = dr["USClassDescription"].ToString();
                    string[] arrayStringSAMEGS = description.Split(' ');

                    //Apply SAME if GS is single word 
                    if (arrayStringSAMEGS.Length == 1)
                    {
                        for (int gs = 0; gs <= arrayStringSAMEGS.Length - 1; gs++)
                        {
                            whereConditionSAMEGS = whereConditionSAMEGS + "(Casefilestatementstext like '%" + pluralservice.Singularize(arrayStringSAMEGS[gs]) + "%') OR";
                            whereConditionSAMEGS = whereConditionSAMEGS + "(Casefilestatementstext like '%" + pluralservice.Pluralize(arrayStringSAMEGS[gs]) + "%') OR";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(whereConditionSAMEGS))
                {
                    whereConditionSAMEGS = whereConditionSAMEGS.Substring(0, whereConditionSAMEGS.Length - 2);
                }
            }
            //End logic
            return whereConditionSAMEGS;
        }

        /// <summary>
        /// Generate result HTML grid
        /// </summary>
        /// <param name="dtResult"> dtata table with result</param>
        /// <param name="grpScore">Group score</param>
        /// <param name="cautionColor"> color </param>
        /// <param name="searchOption">component or full form</param>
        /// <param name="componentFullText">text component or full form</param>
        /// <returns>result string</returns>
        protected string GenerateHTML(DataTable dtResult, string grpScore, string cautionColor, string searchOption, string componentFullText)
        {
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                if (pdfDataTable != null && pdfDataTable.Rows.Count > 0)
                {
                    if (!dtResult.Columns.Contains("CellColor"))
                    { dtResult.Columns.Add("CellColor", typeof(string)); }


                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        dtResult.Rows[i]["CellColor"] = cautionColor;
                    }
                    pdfDataTable.Merge(dtResult);
                }
                if (pdfDataTable == null || pdfDataTable.Rows.Count == 0)
                {
                    if (!dtResult.Columns.Contains("CellColor"))
                    { dtResult.Columns.Add("CellColor", typeof(string)); }

                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        dtResult.Rows[i]["CellColor"] = cautionColor;
                    }

                    pdfDataTable = dtResult;

                }

                Session["Datatable"] = pdfDataTable;
                DataTable objDataTable = (DataTable)Session["Datatable"];
                htmlResult = string.Empty;
            }


            if (dtResult != null && dtResult.Rows.Count > 0)
            {


                if (cautionColor == ScoringColoringModel.Yellow)
                {
                    foreach (DataRow dr in dtResult.Rows)
                    {

                        //htmlResult = htmlResult + "<tr><td class='searh-td-right yellowLightMsg' style='padding: 15px; margin-bottom: 20px; border: 1px solid transparent; border-radius: 4px; color: #8a6d3b;background-color: #fff1a7;border-color: #faebcc;'> <a href='TradeMarkDetailPage.aspx?Sn=" + dr["SerialNo"].ToString() + "&Rn=" + dr["Registrationno"].ToString() + "'>" + dr["SerialNo"].ToString() + "</a></td>";
                        htmlResult = htmlResult + "<tr><td class='searh-td-right' style='line-height: 2rem; padding: 15px; margin-bottom: 20px; border: 1px solid transparent; border-radius: 4px; color: #8a6d3b;background-color: #fff1a7;border-color: #faebcc;'> <a href='TradeMarkDetailPage.aspx?Sn=" + dr["SerialNo"].ToString() + "&Rn=" + dr["Registrationno"].ToString() + "'>" + dr["SerialNo"].ToString() + "</a></td>";

                        //htmlResult = htmlResult + "<td class='searh-td-right yellowLightMsg' style='padding: 15px; margin-bottom: 20px; border: 1px solid transparent; border-radius: 4px; color: #8a6d3b;background-color: #fff1a7;border-color: #faebcc;'> " + dr["Registrationno"].ToString() + "</td>";
                        htmlResult = htmlResult + "<td class='searh-td-right' style='line-height: 2rem;padding: 15px; margin-bottom: 20px; border: 1px solid transparent; border-radius: 4px; color: #8a6d3b;background-color: #fff1a7;border-color: #faebcc;'> " + dr["Registrationno"].ToString() + "</td>";
                        //htmlResult = htmlResult + "<td class='yellowLightMsg' style='padding: 15px; margin-bottom: 20px; border: 1px solid transparent; border-radius: 4px; color: #8a6d3b;background-color: #fff1a7;border-color: #faebcc;'>" + dr["MarkIdentification"].ToString() + "</td>";
                        htmlResult = htmlResult + "<td style='line-height: 2rem;padding: 15px; margin-bottom: 20px; border: 1px solid transparent; border-radius: 4px; color: #8a6d3b;background-color: #fff1a7;border-color: #faebcc;'>" + dr["MarkIdentification"].ToString() + "</td>";

                        htmlResult = htmlResult + "</tr>";
                    }

                }
                else
                {
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        htmlResult = htmlResult + "<tr><td class='searh-td-right' style='color:" + cautionColor + ";'> <a href='TradeMarkDetailPage.aspx?Sn=" + dr["SerialNo"].ToString() + "&Rn=" + dr["Registrationno"].ToString() + "'>" + dr["SerialNo"].ToString() + " </a></td>";

                        htmlResult = htmlResult + "<td class='searh-td-right' style='color:" + cautionColor + ";'> " + dr["Registrationno"].ToString() + "</td>";
                        htmlResult = htmlResult + "<td style='color:" + cautionColor + ";'>" + dr["MarkIdentification"].ToString() + "</td>";

                        htmlResult = htmlResult + "</tr>";
                    }
                }
                btnSavedoc.Visible = true;
                btnSavePdf.Visible = true;
                btnsearchsave.Visible = true;
                btnTalkToAttorney.Visible = true;
            }
            else
            {
                btnsearchsave.Visible = false;
                htmlResult = "<div class='table - responsive'>No result found</div>";
                cautionMessage = ScoringColoringModel.GreenLightMsg;
                return htmlResult;
            }
            Session["htmlResult"] = htmlResult;
            //Caution Message
            DataRow[] drFilterRed = pdfDataTable.Select("CellColor like '%Red%'");
            {
                if (drFilterRed.Count() > 0)
                {
                    cautionMessage = ScoringColoringModel.RedLightMsg;
                }
                else
                {
                    DataRow[] drFilterYellow = pdfDataTable.Select("CellColor like '%Yellow%'");
                    if (drFilterYellow.Count() > 0)
                    {
                        cautionMessage = ScoringColoringModel.YellowLightMsg;
                    }
                    else
                    {
                        cautionMessage = ScoringColoringModel.GreenLightMsg;
                    }
                }
            }

            if (!string.IsNullOrEmpty(grpscore))
            {
                //searchedScored = "<b>Scored: " + grpscore + "</b>";
            }
            if (searchOption == "2" || searchOption == "3")
            {
                litScript.Text = "<script>showloader('hide');showComponentFullText();</script>";
            }
            else { litScript.Text = "<script>showloader('hide');</script>"; }
            return htmlResult;
        }

        /// <summary>
        /// bind the good and services drop down
        /// </summary>
        protected void BindddlGoodsServices(string goodsServicesId)
        {
            SearchService oService = new SearchService();
            DataTable dtUsClass = oService.BindddlGoodsServices();

        }
        /// <summary>
        /// Get the usclass description
        /// </summary>
        /// <param name="goodsServicesId=UsclassDescriptionId"></param>
        /// <returns></returns>
        public string GetUSClassDescriptionByDescriptionId(string usClassDescriptionId)
        {
            SearchService oService = new SearchService();
            string usClassDescription = oService.GetUSClassDescription(usClassDescriptionId);
            return usClassDescription;
        }

        /// <summary>
        /// Generate near 5 where condition string with all possible combination
        /// </summary>
        /// <param name="goodsService">selected goods and services</param>
        /// <returns></returns>
        public string GetNear5String(string goodsService)
        {

            string near5Innerstring = string.Empty;

            string[] arrOrignal = goodsService.Split(' ');
            int length = arrOrignal.Length;
            PluralizationService ps = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));

            string near5ConditionString = string.Empty;

            switch (length)
            {
                case 2:

                    goodsServicearray1 = new string[] { "", "" };
                    goodsServicearray2 = new string[] { "", "" };

                    RecursionNear5Array(0, arrOrignal, ps);

                    var crossproduct1 = new CartesianProduct<string>(
                    goodsServicearray1
                    , goodsServicearray2
                    );


                    foreach (var item in crossproduct1.Get())
                    {
                        near5Innerstring = string.Format("{0}{1}", item[0], item[1]);
                        near5Innerstring = near5Innerstring.Substring(0, near5Innerstring.Length - 1);
                        near5ConditionString = near5ConditionString + "( CONTAINS(Casefilestatementstext,'NEAR((" + near5Innerstring.Replace("'", "''") + "), " + nearX + ")')) OR";

                    }
                    near5ConditionString = near5ConditionString.Replace(",,", ",");
                    break;

                case 3:
                    goodsServicearray1 = new string[] { "", "" };
                    goodsServicearray2 = new string[] { "", "" };
                    goodsServicearray3 = new string[] { "", "" };

                    RecursionNear5Array(0, arrOrignal, ps);

                    var crossproduct2 = new CartesianProduct<string>(
                    goodsServicearray1
                    , goodsServicearray2
                    , goodsServicearray3
                    );

                    foreach (var item in crossproduct2.Get())
                    {
                        near5Innerstring = string.Format("{0}{1}{2}", item[0], item[1], item[2]);
                        near5Innerstring = near5Innerstring.Substring(0, near5Innerstring.Length - 1);
                        near5ConditionString = near5ConditionString + "( CONTAINS(Casefilestatementstext,'NEAR((" + near5Innerstring.Replace("'", "''") + "), " + nearX + ")')) OR";
                    }
                    near5ConditionString = near5ConditionString.Replace(",,", ",");
                    break;

                case 4:
                    goodsServicearray1 = new string[] { "", "" };
                    goodsServicearray2 = new string[] { "", "" };
                    goodsServicearray3 = new string[] { "", "" };
                    goodsServicearray4 = new string[] { "", "" };

                    RecursionNear5Array(0, arrOrignal, ps);

                    var crossproduct3 = new CartesianProduct<string>(
                    goodsServicearray1
                    , goodsServicearray2
                    , goodsServicearray3
                    , goodsServicearray4
                    );

                    foreach (var item in crossproduct3.Get())
                    {
                        near5Innerstring = string.Format("{0}{1}{2}{3}", item[0], item[1], item[2], item[3]);
                        near5Innerstring = near5Innerstring.Substring(0, near5Innerstring.Length - 1);
                        near5ConditionString = near5ConditionString + "( CONTAINS(Casefilestatementstext,'NEAR((" + near5Innerstring.Replace("'", "''") + "), " + nearX + ")')) OR";
                    }
                    near5ConditionString = near5ConditionString.Replace(",,", ",");
                    break;

                case 5:
                    goodsServicearray1 = new string[] { "", "" };
                    goodsServicearray2 = new string[] { "", "" };
                    goodsServicearray3 = new string[] { "", "" };
                    goodsServicearray4 = new string[] { "", "" };
                    goodsServicearray5 = new string[] { "", "" };

                    RecursionNear5Array(0, arrOrignal, ps);

                    var crossproduct4 = new CartesianProduct<string>(
                    goodsServicearray1
                    , goodsServicearray2
                    , goodsServicearray3
                    , goodsServicearray4
                    , goodsServicearray5
                    );

                    foreach (var item in crossproduct4.Get())
                    {
                        near5Innerstring = string.Format("{0}{1}{2}{3}{4}", item[0], item[1], item[2], item[3], item[4]);
                        near5Innerstring = near5Innerstring.Substring(0, near5Innerstring.Length - 1);
                        near5ConditionString = near5ConditionString + "( CONTAINS(Casefilestatementstext,'NEAR((" + near5Innerstring.Replace("'", "''") + "), " + nearX + ")')) OR";
                    }
                    near5ConditionString = near5ConditionString.Replace(",,", ",");
                    break;
                case 6:
                    goodsServicearray1 = new string[] { "", "" };
                    goodsServicearray2 = new string[] { "", "" };
                    goodsServicearray3 = new string[] { "", "" };
                    goodsServicearray4 = new string[] { "", "" };
                    goodsServicearray5 = new string[] { "", "" };
                    goodsServicearray6 = new string[] { "", "" };

                    RecursionNear5Array(0, arrOrignal, ps);

                    var crossproduct5 = new CartesianProduct<string>(
                    goodsServicearray1
                    , goodsServicearray2
                    , goodsServicearray3
                    , goodsServicearray4
                    , goodsServicearray5
                    , goodsServicearray6
                    );

                    foreach (var item in crossproduct5.Get())
                    {
                        near5Innerstring = string.Format("{0}{1}{2}{3}{4}{5}", item[0], item[1], item[2], item[3], item[4], item[5]);
                        near5Innerstring = near5Innerstring.Substring(0, near5Innerstring.Length - 1);
                        near5ConditionString = near5ConditionString + "( CONTAINS(Casefilestatementstext,'NEAR((" + near5Innerstring.Replace("'", "''") + "), " + nearX + ")')) OR";
                    }
                    near5ConditionString = near5ConditionString.Replace(",,", ",");
                    break;
                case 7:
                    goodsServicearray1 = new string[] { "", "" };
                    goodsServicearray2 = new string[] { "", "" };
                    goodsServicearray3 = new string[] { "", "" };
                    goodsServicearray4 = new string[] { "", "" };
                    goodsServicearray5 = new string[] { "", "" };
                    goodsServicearray6 = new string[] { "", "" };
                    goodsServicearray7 = new string[] { "", "" };

                    RecursionNear5Array(0, arrOrignal, ps);

                    var crossproduct6 = new CartesianProduct<string>(
                    goodsServicearray1
                    , goodsServicearray2
                    , goodsServicearray3
                    , goodsServicearray4
                    , goodsServicearray5
                    , goodsServicearray6
                    , goodsServicearray7
                    );

                    foreach (var item in crossproduct6.Get())
                    {
                        near5Innerstring = string.Format("{0}{1}{2}{3}{4}{5}{6}", item[0], item[1], item[2], item[3], item[4], item[5], item[6]);
                        near5Innerstring = near5Innerstring.Substring(0, near5Innerstring.Length - 1);
                        near5ConditionString = near5ConditionString + "( CONTAINS(Casefilestatementstext,'NEAR((" + near5Innerstring.Replace("'", "''") + "), " + nearX + ")')) OR";
                    }
                    near5ConditionString = near5ConditionString.Replace(",,", ",");
                    break;

                case 8:
                    goodsServicearray1 = new string[] { "", "" };
                    goodsServicearray2 = new string[] { "", "" };
                    goodsServicearray3 = new string[] { "", "" };
                    goodsServicearray4 = new string[] { "", "" };
                    goodsServicearray5 = new string[] { "", "" };
                    goodsServicearray6 = new string[] { "", "" };
                    goodsServicearray7 = new string[] { "", "" };
                    goodsServicearray8 = new string[] { "", "" };

                    RecursionNear5Array(0, arrOrignal, ps);

                    var crossproduct7 = new CartesianProduct<string>(
                    goodsServicearray1
                    , goodsServicearray2
                    , goodsServicearray3
                    , goodsServicearray4
                    , goodsServicearray5
                    , goodsServicearray6
                    , goodsServicearray7
                    , goodsServicearray8
                    );

                    foreach (var item in crossproduct7.Get())
                    {
                        near5Innerstring = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", item[0], item[1], item[2], item[3], item[4], item[5], item[6], item[7]);
                        near5Innerstring = near5Innerstring.Substring(0, near5Innerstring.Length - 1);
                        near5ConditionString = near5ConditionString + "( CONTAINS(Casefilestatementstext,'NEAR((" + near5Innerstring.Replace("'", "''") + "), " + nearX + ")')) OR";
                    }
                    near5ConditionString = near5ConditionString.Replace(",,", ",");
                    break;
                case 9:
                    goodsServicearray1 = new string[] { "", "" };
                    goodsServicearray2 = new string[] { "", "" };
                    goodsServicearray3 = new string[] { "", "" };
                    goodsServicearray4 = new string[] { "", "" };
                    goodsServicearray5 = new string[] { "", "" };
                    goodsServicearray6 = new string[] { "", "" };
                    goodsServicearray7 = new string[] { "", "" };
                    goodsServicearray8 = new string[] { "", "" };
                    goodsServicearray9 = new string[] { "", "" };

                    RecursionNear5Array(0, arrOrignal, ps);

                    var crossproduct8 = new CartesianProduct<string>(
                    goodsServicearray1
                    , goodsServicearray2
                    , goodsServicearray3
                    , goodsServicearray4
                    , goodsServicearray5
                    , goodsServicearray6
                    , goodsServicearray7
                    , goodsServicearray8
                    , goodsServicearray9
                    );

                    foreach (var item in crossproduct8.Get())
                    {
                        near5Innerstring = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", item[0], item[1], item[2], item[3], item[4], item[5], item[6], item[7], item[8]);
                        near5Innerstring = near5Innerstring.Substring(0, near5Innerstring.Length - 1);
                        near5ConditionString = near5ConditionString + "( CONTAINS(Casefilestatementstext,'NEAR((" + near5Innerstring.Replace("'", "''") + "), " + nearX + ")')) OR";
                    }
                    near5ConditionString = near5ConditionString.Replace(",,", ",");
                    break;

                case 10:
                    goodsServicearray1 = new string[] { "", "" };
                    goodsServicearray2 = new string[] { "", "" };
                    goodsServicearray3 = new string[] { "", "" };
                    goodsServicearray4 = new string[] { "", "" };
                    goodsServicearray5 = new string[] { "", "" };
                    goodsServicearray6 = new string[] { "", "" };
                    goodsServicearray7 = new string[] { "", "" };
                    goodsServicearray8 = new string[] { "", "" };
                    goodsServicearray9 = new string[] { "", "" };
                    goodsServicearray10 = new string[] { "", "" };

                    RecursionNear5Array(0, arrOrignal, ps);

                    var crossproduct9 = new CartesianProduct<string>(
                    goodsServicearray1
                    , goodsServicearray2
                    , goodsServicearray3
                    , goodsServicearray4
                    , goodsServicearray5
                    , goodsServicearray6
                    , goodsServicearray7
                    , goodsServicearray8
                    , goodsServicearray9
                    , goodsServicearray10
                    );

                    foreach (var item in crossproduct9.Get())
                    {
                        near5Innerstring = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", item[0], item[1], item[2], item[3], item[4], item[5], item[6], item[7], item[8], item[9]);
                        near5Innerstring = near5Innerstring.Substring(0, near5Innerstring.Length - 1);
                        near5ConditionString = near5ConditionString + "( CONTAINS(Casefilestatementstext,'NEAR((" + near5Innerstring.Replace("'", "''") + "), " + nearX + ")')) OR";
                    }
                    near5ConditionString = near5ConditionString.Replace(",,", ",");
                    break;

            }
            near5ConditionString = near5ConditionString.Substring(0, near5ConditionString.Length - 2);
            return near5ConditionString;
        }

        /// <summary>
        /// recursive method to generate the near 5 condition string
        /// </summary>
        /// <param name="next">next count</param>
        /// <param name="gsArray">Goods service string array</param>
        public void RecursionNear5Array(int next, string[] gsArray, PluralizationService ps)
        {
            int totalSize = gsArray.Length;

            if (next < totalSize)
            {
                switch (next)
                {
                    case 0:
                        goodsServicearray1[0] = ps.Singularize(gsArray[0].ToString()) + ",";
                        goodsServicearray1[1] = ps.Pluralize(gsArray[0].ToString()) + ",";
                        break;

                    case 1:
                        goodsServicearray2[0] = ps.Singularize(gsArray[1].ToString()) + ",";
                        goodsServicearray2[1] = ps.Pluralize(gsArray[1].ToString()) + ",";
                        break;

                    case 2:
                        goodsServicearray3[0] = ps.Singularize(gsArray[2].ToString()) + ",";
                        goodsServicearray3[1] = ps.Pluralize(gsArray[2].ToString()) + ",";
                        break;

                    case 3:
                        goodsServicearray4[0] = ps.Singularize(gsArray[3].ToString()) + ",";
                        goodsServicearray4[1] = ps.Pluralize(gsArray[3].ToString()) + ",";
                        break;

                    case 4:
                        goodsServicearray5[0] = ps.Singularize(gsArray[4].ToString()) + ",";
                        goodsServicearray5[1] = ps.Pluralize(gsArray[4].ToString()) + ",";
                        break;

                    case 5:
                        goodsServicearray6[0] = ps.Singularize(gsArray[5].ToString()) + ",";
                        goodsServicearray6[1] = ps.Pluralize(gsArray[5].ToString()) + ",";
                        break;

                    case 6:
                        goodsServicearray7[0] = ps.Singularize(gsArray[6].ToString()) + ",";
                        goodsServicearray7[1] = ps.Pluralize(gsArray[6].ToString()) + ",";
                        break;

                    case 7:
                        goodsServicearray8[0] = ps.Singularize(gsArray[7].ToString()) + ",";
                        goodsServicearray8[1] = ps.Pluralize(gsArray[7].ToString()) + ",";
                        break;

                    case 8:
                        goodsServicearray9[0] = ps.Singularize(gsArray[8].ToString()) + ",";
                        goodsServicearray9[1] = ps.Pluralize(gsArray[8].ToString()) + ",";
                        break;

                    case 9:
                        goodsServicearray10[0] = ps.Singularize(gsArray[9].ToString()) + ",";
                        goodsServicearray10[1] = ps.Pluralize(gsArray[9].ToString()) + ",";
                        break;
                }
                RecursionNear5Array(++next, gsArray, ps);
            }
        }
        /// <summary>
        /// to generate the pdf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSavePdf_Click(object sender, EventArgs e)
        {
            byte[] bPDF = null;
            Paragraph objParagraph = new Paragraph();


            //AV
            Chunk searchMark = new Chunk("Mark Searched: ");
            Chunk searchGoods = new Chunk("Goods/Services Searched: ");
            Chunk searchDate = new Chunk("Date Searched: ");
            Chunk searchTextMark = new Chunk(txtMark.Text.Trim() + "\n\n", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD));
            Chunk searchGoodsText = new Chunk(hdnGoodsName.Value.Trim() + "\n\n", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD));
            string searchDateFormat = DateTime.Now.ToString("MMMM dd, yyyy");
            Chunk searchDateText = new Chunk(searchDateFormat + "\n\n\n", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD));

            objParagraph.Add(searchMark);
            objParagraph.Add(searchTextMark);
            objParagraph.Add(searchGoods);
            objParagraph.Add(searchGoodsText);
            objParagraph.Add(searchDate);
            objParagraph.Add(searchDateText);

            Font color = FontFactory.GetFont(ScoringColoringModel.pdfFontSet, 12, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#CC0000").ToArgb())); ;
            if ((DataTable)Session["Datatable"] != null)
            {
                var imagePath = HostingEnvironment.MapPath("~/Images/WarningImage.JPG");
                DataTable objDataTable = (DataTable)Session["Datatable"];
                //Caution Message
                DataRow[] drFilterRed = objDataTable.Select("CellColor like '%Red%'");
                {
                    if (drFilterRed.Count() > 0)
                    {
                        color = FontFactory.GetFont(ScoringColoringModel.pdfFontSet, 12, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#CC0000").ToArgb()));
                        Chunk paraData = new Chunk(ScoringColoringModel.pdfRedLightHeadingMsg, color);
                        Chunk paraDataText = new Chunk(ScoringColoringModel.pdfRedLightMsg, color);
                        Chunk emptyChunk = new Chunk("\n");
                        objParagraph.Add(paraData);
                        objParagraph.Add(paraDataText);
                        objParagraph.Add(emptyChunk);
                    }
                    else
                    {
                        DataRow[] drFilterYellow = objDataTable.Select("CellColor like '%Yellow%'");
                        if (drFilterYellow.Count() > 0)
                        {
                            imagePath = HostingEnvironment.MapPath("~/Images/cautionImage.JPG");
                            color = FontFactory.GetFont(ScoringColoringModel.pdfFontSet, 12, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fff1a7").ToArgb()));
                            Chunk paraData = new Chunk(ScoringColoringModel.pdfYellowLightHeadingMsg, color);
                            Chunk paraDataText = new Chunk(ScoringColoringModel.pdfYellowLightMsg, color);
                            Chunk emptyChunk = new Chunk("\n");
                            objParagraph.Add(paraData);
                            objParagraph.Add(paraDataText);
                            objParagraph.Add(emptyChunk);
                        }
                        else
                        {
                            imagePath = HostingEnvironment.MapPath("~/Images/congratulationImage.JPG");
                            color = FontFactory.GetFont(ScoringColoringModel.pdfFontSet, 12, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#008000").ToArgb()));
                            Chunk paraData = new Chunk(ScoringColoringModel.pdfGreenLightHeadingMsg, color);
                            Chunk paraDataText = new Chunk(ScoringColoringModel.pdfGreenLightMsg, color);
                            Chunk emptyChunk = new Chunk("\n");
                            objParagraph.Add(paraData);
                            objParagraph.Add(paraDataText);
                            objParagraph.Add(emptyChunk);
                        }
                    }
                }

                MemoryStream ms = new MemoryStream();
                Document document = new Document(); //pdf document to write

                var originalpath = HostingEnvironment.MapPath("~/PDFs/");
                if (!System.IO.Directory.Exists(originalpath))
                    Directory.CreateDirectory(originalpath);
                // Create a new PdfWriter object, specifying the output stream

                var pdfwriter = PdfWriter.GetInstance(document, ms);


                // Open the Document for writing
                document.Open();
                PdfPTable ParentTable = new PdfPTable(1);

                ParentTable.TotalWidth = 500f;
                ParentTable.LockedWidth = true;
                ParentTable.HorizontalAlignment = 0;
                ParentTable.ExtendLastRow = false;
                PdfPCell heading = new PdfPCell(new Phrase("", HeaderFont));
                heading.PaddingBottom = 0f;
                heading.PaddingTop = 0f;

                heading.Border = 1;

                ParentTable.AddCell(heading);
                PdfPTable dataTableCellHeaderTable = new PdfPTable(3);
                dataTableCellHeaderTable.HorizontalAlignment = 0;

                float[] widths = new float[] { 2f, 2f, 5f };
                dataTableCellHeaderTable.SetWidths(widths);

                Font tableHeaderCellFont = FontFactory.GetFont(ScoringColoringModel.pdfFontSet, 10, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF").ToArgb()));

                PdfPCell cellSerialNumber = new PdfPCell(new Phrase(ScoringColoringModel.pdfFirstCellHeading, tableHeaderCellFont)) { Border = 0 };
                cellSerialNumber.PaddingTop = 7.5f;
                cellSerialNumber.PaddingBottom = 7.5f;
                cellSerialNumber.BorderColor = BaseColor.WHITE;
                cellSerialNumber.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3").ToArgb());
                dataTableCellHeaderTable.AddCell(cellSerialNumber);

                PdfPCell cellRegistration = new PdfPCell(new Phrase(ScoringColoringModel.pdfSecondCellHeading, tableHeaderCellFont)) { Border = PdfPCell.LEFT_BORDER };
                cellRegistration.PaddingTop = 7.5f;
                cellRegistration.PaddingBottom = 7.5f;
                cellRegistration.BorderColor = BaseColor.WHITE;
                cellRegistration.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3").ToArgb());
                dataTableCellHeaderTable.AddCell(cellRegistration);

                PdfPCell cellwordMark = new PdfPCell(new Phrase(ScoringColoringModel.pdfThirdCellHeading, tableHeaderCellFont)) { Border = PdfPCell.LEFT_BORDER };
                cellwordMark.PaddingTop = 7.5f;
                cellwordMark.PaddingBottom = 7.5f;
                cellwordMark.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3").ToArgb());
                cellwordMark.BorderColor = BaseColor.WHITE;
                dataTableCellHeaderTable.AddCell(cellwordMark);
                // to append more data create one table
                PdfPTable datatable = new PdfPTable(3);

                Font cellColor = new Font();

                foreach (DataRow dr in objDataTable.Rows)
                {
                    if (dr.ItemArray[5].ToString() == "Yellow")
                    {
                        cellColor = FontFactory.GetFont(ScoringColoringModel.pdfFontSet, 12, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fff1a7").ToArgb())); ;
                    }
                    else
                    {
                        cellColor = FontFactory.GetFont(ScoringColoringModel.pdfFontSet, 12, new BaseColor(System.Drawing.ColorTranslator.FromHtml(dr.ItemArray[5].ToString()).ToArgb())); ;
                    }

                    dataTableCellHeaderTable.AddCell(new PdfPCell(new Phrase(dr.ItemArray[0].ToString(), cellColor)) { PaddingBottom = 5, Border = 1, PaddingTop = 5 });
                    dataTableCellHeaderTable.AddCell(new PdfPCell(new Phrase(dr.ItemArray[1].ToString(), cellColor)) { PaddingBottom = 5, Border = 1, PaddingTop = 5 });
                    dataTableCellHeaderTable.AddCell(new PdfPCell(new Phrase(dr.ItemArray[3].ToString(), cellColor)) { PaddingBottom = 5, Border = 1, PaddingTop = 5 });
                }
                PdfPCell nestDataTableCellCreationTable = new PdfPCell(dataTableCellHeaderTable);

                nestDataTableCellCreationTable.Border = iTextSharp.text.Image.LEFT_BORDER;

                nestDataTableCellCreationTable.Border = iTextSharp.text.Image.RIGHT_BORDER;
                nestDataTableCellCreationTable.Border = iTextSharp.text.Image.BOTTOM_BORDER;
                ParentTable.AddCell(nestDataTableCellCreationTable);
                //ImageTable

                // Get instance of iTextSharp Image BOB Report top image

                var fileStream = File.ReadAllBytes(imagePath);

                Image imageReport = Image.GetInstance(imagePath);
                imageReport.Alignment = Image.ALIGN_CENTER;
                imageReport.ScalePercent(60);
                //

                document.Add(imageReport);
                document.Add(objParagraph);
                document.Add(ParentTable);

                document.Close();
                bPDF = ms.ToArray();
                // Close the writer instance

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=TrademarkSearchResult.pdf");
                Response.BinaryWrite(bPDF);
                Response.End();
            }
        }
        /// <summary>
        /// to generate the word doc 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveDoc_Click(object sender, EventArgs e)
        {

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
                "attachment;filename=TrademarkSearchResult.doc");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-word ";

            StringWriter sw = new StringWriter();
            if (!string.IsNullOrEmpty(hdnSearchResult.Value.ToString()))
            {
                string strSearchResult = hdnSearchResult.Value.ToString();
                strSearchResult = strSearchResult.Replace("class=\"redLightMsg", " Style=\"color:red;");
                strSearchResult = strSearchResult.Replace("class=\"yellowLightMsg alert alert-warning", " Style=\"color: #fff1a7;");
                strSearchResult = strSearchResult.Replace("class=\"searh-td-right yellowLightMsg", "Style=\"color: #fff1a7;");
                strSearchResult = strSearchResult.Replace("class=\"GreenLightMsg", " Style=\"color:green;");
                sw.WriteLine(strSearchResult);
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        /// <summary>
        /// to send email to Attorney
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            UserService oUserService = new UserService();
            AdminSettingModel adminSetting = oUserService.GetAdminSetting();
            if (adminSetting != null)
            {
                importInporocess = adminSetting.StatusFlag;
                latestFileImportDate = adminSetting.LatestFileImportDate;
            }
            UserModel objUserModel = new UserModel();
            string emailBody = string.Empty;
            //get customer details
            if (Session["UserId"] != null)
            {
                //get user details in model
                objUserModel = oUserService.GetUserDetail("", Session["UserId"].ToString());
                string talktoAttoneyText = string.Empty;

                if (hdntalktoAttoneycheck.Value == "1")
                {
                    talktoAttoneyText = ScoringColoringModel.talkToAttorneyHelp;
                }
                else if (hdntalktoAttoneycheck.Value == "2")
                {
                    talktoAttoneyText = ScoringColoringModel.talkToAttorneyAssistance;
                }
                else { talktoAttoneyText = ScoringColoringModel.talkToAttorneyBoth; }

                if (objUserModel != null)
                {
                    emailBody = "<p>Hello, <br/><br/>" + talktoAttoneyText + ScoringColoringModel.talktoAttorneyBody;
                    emailBody = emailBody + "<p><b>Customer Details:</b></p><table><tr><td> First Name: </td><td><b> " + objUserModel.FirstName + " </b></td></tr><tr><td> Last Name: </td><td><b> " + objUserModel.LastName + " </b></td></tr><tr><td> Email: </td><td><b>" + objUserModel.Email + " </b></td></tr><tr><td> ContactNo: </td><td><b>" + objUserModel.ContactNo + " </b></td></tr><tr><td> Title: </td><td><b>" + objUserModel.Title + " </b></td></tr><tr><td>CompanyName: </td><td><b> " + objUserModel.CompanyName + "</b></td></tr></table><br/><br/><br/>";
                }
            }

            if (adminSetting != null)
            {
                emailBody = emailBody + hdnSearchResult.Value;

                emailBody = emailBody.Replace("alert alert-danger", "line-height: 2rem;margin: 15px auto; border:1px solid transparent;border-radius:4px;margin-bottom:20px;padding:15px;background-color: #f2dede;border-color:#ebccd1;color:#a94442;");
                //emailBody = emailBody.Replace("", "line-height: 2rem;margin: 15px auto; border:1px solid transparent;border-radius:4px;margin-bottom:20px;padding:15px;background-color: #f2dede;border-color:#ebccd1;color:#a94442;");
                emailBody = emailBody + ScoringColoringModel.talktoAttorneyThanks;

                if (Utility.Utility.SendEmail("BOB - Talk to an Attorney Request.", emailBody, adminSetting.AdminEmail))
                {
                    lblSuccessmsg.Text = "Your request has been sent to an attorney.";
                    litScript.Text = "<script>showSuccessmsg();</script>";
                }
            }
            divSearchResult.InnerHtml = "";
            hdnSearchResultCopy.Value = hdnSearchResultCopy.Value.Replace("alert alert-danger", "line-height: 2rem;margin: 15px auto; border:1px solid transparent;border-radius:4px;margin-bottom:20px;padding:15px;background-color: #f2dede;border-color:#ebccd1;color:#a94442;");
            divSearchResult.InnerHtml = hdnSearchResultCopy.Value.ToString();



        }
    }
}
