using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace TradeMark
{
    public partial class TradeMarkDetailPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string SerialNo = Request.QueryString["Sn"];
            string RegistrationNo = Request.QueryString["Rn"];
            ViewState["Rn"] = RegistrationNo;

            if (SerialNo != null && RegistrationNo != null)
            {
                string summary = "";
                string markInfo = "";
                string BasisInfo = "";
                string goodsandServices = "";
                string currentOwnerInfo = "";
                string assignment = "";
                string nodeName = "";
                string prosecutionHistory = "";
                string tMStaff = "";
                string maintenanceFilings = "";
                string correspondenceInformation = "";
                string proceeding = "";


                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load("https://tsdrapi.uspto.gov/ts/cd/casestatus/sn" + SerialNo + "/content.html");

                HtmlNode[] summeryNodes = doc.DocumentNode.SelectNodes("//div").Where(x => x.InnerHtml.Contains("summary")).ToArray();
                int summaryval = 1;
                // for getting general detail from the html file.
                foreach (HtmlNode summeryNodesItem in summeryNodes)
                {
                    if (summaryval == 1)
                    {
                        StringBuilder objSummarySB = new StringBuilder();
                        summary = summeryNodesItem.InnerHtml;
                        objSummarySB.Append(summary);
                        divsummary.InnerHtml = Convert.ToString(objSummarySB);
                    }
                    summaryval++;
                }//end foreach  summeryNodes
                HtmlNode[] accordialNode = doc.DocumentNode.SelectNodes("//div").Where(x => x.InnerHtml.Contains("expand_wrapper default_hide")).ToArray();
                // start looping to get the data which we will show in accordial 
                foreach (HtmlNode accordialNodeItem in accordialNode)
                {
                    HtmlNode[] accordialInnerNode = accordialNodeItem.ChildNodes.ToArray();
                    foreach (HtmlNode accordialInnerNodeItem in accordialInnerNode)
                    {
                        if (accordialInnerNodeItem.Name == "div")
                        {
                            HtmlNode[] accordialGetInnerNode = accordialInnerNodeItem.ChildNodes.ToArray();
                            foreach (HtmlNode accordialGetInnerNodeItem in accordialGetInnerNode)
                            {
                                if (accordialGetInnerNodeItem.Name == "h2")
                                {
                                    HtmlNode[] accordialh2Node = accordialGetInnerNodeItem.ChildNodes.ToArray();
                                    foreach (HtmlNode accordialh2NodeItem in accordialh2Node)
                                    {
                                        if (accordialh2NodeItem.Name == "span")
                                        {
                                            // here strore name of tab by which we apply the check in below code
                                            nodeName = accordialh2NodeItem.InnerText;
                                        }
                                    }
                                }
                                if (accordialGetInnerNodeItem.Name == "div")
                                {
                                    // apply check here and according to condition get the data.
                                    switch (nodeName)
                                    {
                                        case "Mark Information":
                                            {
                                                StringBuilder objmarkInfoSB = new StringBuilder();
                                                markInfo = accordialGetInnerNodeItem.InnerHtml;
                                                objmarkInfoSB.Append(markInfo);
                                                divMarkInfo.InnerHtml = Convert.ToString(objmarkInfoSB);
                                                break;
                                            }
                                        case "Goods and Services":
                                            {
                                                StringBuilder objGoodsandServicesSB = new StringBuilder();
                                                goodsandServices = accordialGetInnerNodeItem.InnerHtml;
                                                objGoodsandServicesSB.Append(goodsandServices);
                                                divGoodsandServices.InnerHtml = Convert.ToString(objGoodsandServicesSB);
                                                break;
                                            }
                                        case "Basis Information (Case Level)":
                                            {
                                                StringBuilder objBasisInformationSB = new StringBuilder();
                                                BasisInfo = accordialGetInnerNodeItem.InnerHtml;
                                                objBasisInformationSB.Append(BasisInfo);
                                                divBasisInformation.InnerHtml = Convert.ToString(objBasisInformationSB);
                                                break;
                                            }
                                        case "Current Owner(s) Information":
                                            {
                                                StringBuilder objCurrentOwnerSB = new StringBuilder();
                                                currentOwnerInfo = accordialGetInnerNodeItem.InnerHtml;
                                                objCurrentOwnerSB.Append(currentOwnerInfo);
                                                divCurrentOwner.InnerHtml = Convert.ToString(objCurrentOwnerSB);
                                                break;
                                            }

                                        case "Attorney/Correspondence Information":
                                            {
                                                StringBuilder objCorrespondenceInformationSB = new StringBuilder();
                                                correspondenceInformation = accordialGetInnerNodeItem.InnerHtml;
                                                objCorrespondenceInformationSB.Append(correspondenceInformation);
                                                divCorrespondenceInformation.InnerHtml = Convert.ToString(objCorrespondenceInformationSB);
                                                break;
                                            }
                                        case "Prosecution History":
                                            {
                                                StringBuilder objProsecutionHistorySB = new StringBuilder();
                                                prosecutionHistory = accordialGetInnerNodeItem.InnerHtml;
                                                objProsecutionHistorySB.Append(prosecutionHistory);
                                                divProsecutionHistory.InnerHtml = Convert.ToString(objProsecutionHistorySB);
                                                break;
                                            }
                                        case "TM Staff and Location Information":
                                            {
                                                StringBuilder objTMStaffSB = new StringBuilder();
                                                tMStaff = accordialGetInnerNodeItem.InnerHtml;
                                                objTMStaffSB.Append(tMStaff);
                                                divTMStaff.InnerHtml = Convert.ToString(objTMStaffSB);
                                                break;
                                            }
                                        case "Maintenance Filings or Post Registration Information":
                                            {
                                                StringBuilder objMaintenanceFilingsSB = new StringBuilder();
                                                maintenanceFilings = accordialGetInnerNodeItem.InnerHtml;
                                                objMaintenanceFilingsSB.Append(maintenanceFilings);
                                                divMaintenanceFilings.InnerHtml = Convert.ToString(objMaintenanceFilingsSB);
                                                break;
                                            }

                                        case "Assignment Abstract Of Title Information":
                                            {
                                                StringBuilder objAssignmentSB = new StringBuilder();
                                                assignment = accordialGetInnerNodeItem.InnerHtml;
                                                objAssignmentSB.Append(assignment);
                                                divAssignment.InnerHtml = Convert.ToString(objAssignmentSB);
                                                break;
                                            }

                                        case "Proceedings":

                                            {
                                                StringBuilder objProceedingSB = new StringBuilder();
                                                proceeding = accordialGetInnerNodeItem.InnerHtml;
                                                objProceedingSB.Append(proceeding);
                                                divProceeding.InnerHtml = Convert.ToString(objProceedingSB);
                                                break;
                                            }

                                    } // end of switch case
                                } // end of if condition for div
                            } //end of loop accordialGetInnerNode
                        }
                    } // end of loop accordialInnerNode

                }//end of loop accordialNode

            }


        }
    }
}
