using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using TradeMark.Utility;

namespace TradeMark
{
    public partial class TradeMarkXml : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string applicationDate = "";
            string SerialNo = Request.QueryString["Sn"];
            string RegistrationNo = Request.QueryString["Rn"];
            ViewState["Rn"] = RegistrationNo;
            //string RegistrationNo = "1";
            //string SerialNo = "75084181";
            if (SerialNo != null && RegistrationNo != null)
            {

                //Uri myUri = new Uri("https://tsdrapi.uspto.gov/ts/cd/status66/sn" + SerialNo + "/info.xml");
                Uri myUri = new Uri("https://tsdrapi.uspto.gov/ts/cd/casestatus/sn" + SerialNo + "/info.xml");
                HttpWebRequest request = WebRequest.Create(myUri) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                XmlDocument xmlDoc = new XmlDocument();
                // load the xml document
                xmlDoc.Load(response.GetResponseStream());
                //create a xml namespace which we are using in our xml document
                var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                nsmgr.AddNamespace("ns1", "http://www.wipo.int/standards/XMLSchema/ST96/Common");
                nsmgr.AddNamespace("ns2", "http://www.wipo.int/standards/XMLSchema/ST96/Trademark");
                nsmgr.AddNamespace("ns3", "urn:us:gov:doc:uspto:trademark");

                // create a xml node list
                XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("./ns2:TrademarkTransactionBody/ns2:TransactionContentBag/ns2:TransactionData/ns2:TrademarkBag/ns2:Trademark", nsmgr);
                if (nodeList.Count > 0)
                {
                    lblSerialNumber.Text = SerialNo;
                    // Retrive the data from the xml node
                    foreach (XmlNode node in nodeList)
                    {

                        // General Information

                        //Strat of Trade mark child node
                        XmlNodeList generalNode = node.ChildNodes;
                        if (generalNode.Count > 0)
                        {
                            // ns2:MarkRepresentation/ns2:MarkReproduction/ns2:WordMarkSpecification/ns2:MarkVerbalElementText

                            // start of MarkVerbalElementText
                            foreach (XmlNode generalNodeItem in generalNode)
                            {
                                if (generalNodeItem.Name == "ns2:MarkRepresentation")
                                {
                                    XmlNodeList markReproductionNode = generalNodeItem.ChildNodes;
                                    foreach (XmlNode markReproductionNodeItem in markReproductionNode)
                                    {
                                        if (markReproductionNodeItem.Name == "ns2:MarkReproduction")
                                        {

                                            XmlNodeList wordMarkSpecificationNode = markReproductionNodeItem.ChildNodes;
                                            foreach (XmlNode wordMarkSpecificationNodeItem in wordMarkSpecificationNode)
                                            {
                                                if (wordMarkSpecificationNodeItem.Name == "ns2:WordMarkSpecification")
                                                {

                                                    XmlNodeList markVerbalElementTextNode = wordMarkSpecificationNodeItem.ChildNodes;
                                                    foreach (XmlNode markVerbalElementTextItem in markVerbalElementTextNode)
                                                    {
                                                        if (markVerbalElementTextItem.Name == "ns2:MarkVerbalElementText")
                                                        {

                                                            lblMark.Text = markVerbalElementTextItem.InnerText;
                                                            lblMarkLiteralElements.Text = markVerbalElementTextItem.InnerText;
                                                            // lblRegister.Text= markVerbalElementTextItem.InnerText;
                                                        }
                                                    }
                                                }
                                                if(wordMarkSpecificationNodeItem.Name == "ns2:MarkImageBag")
                                                {
                                                    StringBuilder markImageBagSB = new StringBuilder();
                                                    string markImageBag = "";
                                                    XmlNodeList MarkImageBagNode = wordMarkSpecificationNodeItem.ChildNodes;
                                                    foreach (XmlNode MarkImageBagNodeItem in MarkImageBagNode)
                                                    {
                                                        if(MarkImageBagNodeItem.Name== "ns2:MarkImage")
                                                        {
                                                            XmlNodeList MarkImageNode = MarkImageBagNodeItem.ChildNodes;
                                                            foreach(XmlNode markImageNodeItem in MarkImageNode)// ns2: NationalDesignSearchCodeBag
                                                            {
                                                                if(markImageNodeItem.Name== "ns2:NationalDesignSearchCodeBag")
                                                                {
                                                                    XmlNodeList nationalDesignSearchCodeBagNode = markImageNodeItem.ChildNodes;
                                                                    foreach(XmlNode nationalDesignSearchCodeBagNodeItem in nationalDesignSearchCodeBagNode)
                                                                    {
                                                                        
                                                                        if (nationalDesignSearchCodeBagNodeItem.Name == "ns2:NationalDesignSearchCode")
                                                                        {
                                                                            XmlNodeList nationalDesignSearchCodeNode = nationalDesignSearchCodeBagNodeItem.ChildNodes;

                                                                            foreach(XmlNode nationalDesignSearchCodeNodeItem in nationalDesignSearchCodeNode)
                                                                            {
                                                                                if(nationalDesignSearchCodeNodeItem.Name== "ns2:NationalDesignCode")
                                                                                {
                                                                                    markImageBag += nationalDesignSearchCodeNodeItem.InnerText +"-";
                                                                                }
                                                                                if (nationalDesignSearchCodeNodeItem.Name == "ns2:NationalDesignCodeDescriptionText")
                                                                                {
                                                                                    markImageBag += nationalDesignSearchCodeNodeItem.InnerText + ";";
                                                                                }
                                                                               
                                                                            }
                                                                        }
                                                                        markImageBagSB.Append("<div>" + markImageBag.TrimEnd(';') + "</div>");
                                                                        markImageBag = "";
                                                                    }
                                                                }

                                                                divDesignSearch.InnerHtml = Convert.ToString(markImageBagSB);
                                                            }
                                                        }
                                                      
                                                    }
                                                   
                                                }

                                            }

                                        }

                                        if (markReproductionNodeItem.Name == "ns2:MarkDescriptionBag")
                                        {
                                            XmlNodeList MarkDescriptionBagNode = markReproductionNodeItem.ChildNodes;
                                            foreach (XmlNode MarkDescriptionBagNodeItem in MarkDescriptionBagNode)
                                            {
                                                if (MarkDescriptionBagNodeItem.Name == "ns2:NationalMarkDescription")
                                                {
                                                    XmlNodeList nationalMarkDescriptionNode = MarkDescriptionBagNodeItem.ChildNodes;
                                                    foreach (XmlNode nationalMarkDescriptionNodeItem in nationalMarkDescriptionNode)
                                                    {
                                                        if (nationalMarkDescriptionNodeItem.Name == "ns2:MarkFeatureCode")
                                                        {
                                                            string markFeatureCode = nationalMarkDescriptionNodeItem.InnerText;
                                                            if (applicationDate != "")
                                                            {
                                                                string appYear = applicationDate.Substring(0, 4);
                                                                string appMonth = applicationDate.Substring(5, 2);
                                                                string appDay = applicationDate.Substring(8, 2);
                                                                string DateString = appYear + "-" + appMonth + "-" + appDay;

                                                                DateTime sourceDate = DateTime.ParseExact("2003-11-02", "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                                                DateTime dateVal = DateTime.ParseExact(DateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                                                                if (dateVal < sourceDate)
                                                                {
                                                                    lblMarkDrawingType.Text = MarkDrawing.PriorToNovember(markFeatureCode);
                                                                }
                                                                if (dateVal > sourceDate)
                                                                {
                                                                    lblMarkDrawingType.Text = MarkDrawing.AfterToNovember(markFeatureCode);
                                                                }

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            // ns2: NationalMarkDescription
                                        }
                                    } //end of ns2:MarkRepresentation foreach loop
                                } // end of ns2:MarkRepresentation if condition
                                // For basic Information tab
                                if (generalNodeItem.Name == "ns2:NationalFilingBasis")
                                {
                                    XmlNodeList NationalFilingBasisNode = generalNodeItem.ChildNodes;
                                    foreach (XmlNode NationalFilingBasisNodeItem in NationalFilingBasisNode)
                                    {
                                        if (NationalFilingBasisNodeItem.Name == "ns2:FilingBasis")
                                        {
                                            XmlNodeList NationalFilingBasisInnerNode = NationalFilingBasisNodeItem.ChildNodes;
                                            foreach (XmlNode NationalFilingBasisInnerNodeItem in NationalFilingBasisInnerNode)
                                            {
                                                if (NationalFilingBasisInnerNodeItem.Name == "ns2:BasisForeignApplicationIndicator")
                                                {
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "true")
                                                    {
                                                        lblFilingBasisFiled44D.Text = "Yes";
                                                    }
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "false")
                                                    {
                                                        lblFilingBasisFiled44D.Text = "Yes";
                                                    }
                                                }

                                                if (NationalFilingBasisInnerNodeItem.Name == "ns2:BasisForeignRegistrationIndicator")
                                                {
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "true")
                                                    {
                                                        lblFilingBasisFiled44E.Text = "Yes";
                                                    }
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "false")
                                                    {
                                                        lblFilingBasisFiled44E.Text = "No";
                                                    }
                                                }
                                                if (NationalFilingBasisInnerNodeItem.Name == "ns2:BasisUseIndicator")
                                                {
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "true")
                                                    {
                                                        lblFilingBasisFiledUse.Text = "Yes";
                                                    }
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "false")
                                                    {
                                                        lblFilingBasisFiledUse.Text = "Yes";
                                                    }
                                                }
                                                if (NationalFilingBasisInnerNodeItem.Name == "ns2:BasisIntentToUseIndicator")
                                                {
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "true")
                                                    {
                                                        lblFilingBasisFiledITU.Text = "Yes";
                                                    }
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "false")
                                                    {
                                                        lblFilingBasisFiledITU.Text = "No";
                                                    }
                                                }
                                                if (NationalFilingBasisInnerNodeItem.Name == "ns2:NoBasisIndicator")
                                                {
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "true")
                                                    {
                                                        lblFilingBasisFiledNoBasis.Text = "Yes";
                                                    }
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "false")
                                                    {
                                                        lblFilingBasisFiledNoBasis.Text = "No";
                                                    }
                                                }
                                                if (NationalFilingBasisInnerNodeItem.Name == "ns2:BasisInternationalRegistrationIndicator")
                                                {
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "true")
                                                    {
                                                        lblFilingBasisFiled66A.Text = "Yes";
                                                    }
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "false")
                                                    {
                                                        lblFilingBasisFiled66A.Text = "No";
                                                    }
                                                }
                                            }

                                        }

                                        if (NationalFilingBasisNodeItem.Name == "ns2:CurrentBasis")
                                        {
                                            XmlNodeList NationalFilingBasisInnerNode = NationalFilingBasisNodeItem.ChildNodes;
                                            foreach (XmlNode NationalFilingBasisInnerNodeItem in NationalFilingBasisInnerNode)
                                            {
                                                if (NationalFilingBasisInnerNodeItem.Name == "ns2:BasisForeignApplicationIndicator")
                                                {
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "true")
                                                    {
                                                        lblCurrently44D.Text = "Yes";
                                                    }
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "false")
                                                    {
                                                        lblCurrently44D.Text = "No";
                                                    }
                                                }

                                                if (NationalFilingBasisInnerNodeItem.Name == "ns2:BasisForeignRegistrationIndicator")
                                                {
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "true")
                                                    {
                                                        lblCurrently44E.Text = "Yes";
                                                    }
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "false")
                                                    {
                                                        lblCurrently44E.Text = "No";
                                                    }
                                                }
                                                if (NationalFilingBasisInnerNodeItem.Name == "ns2:BasisUseIndicator")
                                                {
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "true")
                                                    {
                                                        lblCurrentlyUse.Text = "Yes";
                                                        lblBasis.Text = "1(a)";
                                                    }
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "false")
                                                    {
                                                        lblCurrentlyUse.Text = "No";
                                                        lblBasis.Text = "1(b)";
                                                    }
                                                }
                                                if (NationalFilingBasisInnerNodeItem.Name == "ns2:BasisIntentToUseIndicator")
                                                {
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "true")
                                                    {
                                                        lblCurrentlyITU.Text = "Yes";
                                                    }
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "false")
                                                    {
                                                        lblCurrentlyITU.Text = "No";
                                                    }
                                                }
                                                if (NationalFilingBasisInnerNodeItem.Name == "ns2:NoBasisIndicator")
                                                {
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "true")
                                                    {
                                                        lblCurrentlyNoBasis.Text = "Yes";
                                                    }
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "false")
                                                    {
                                                        lblCurrentlyNoBasis.Text = "No";
                                                    }
                                                }
                                                if (NationalFilingBasisInnerNodeItem.Name == "ns2:BasisInternationalRegistrationIndicator")
                                                {
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "true")
                                                    {
                                                        lblCurrently66A.Text = "Yes";
                                                    }
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "false")
                                                    {
                                                        lblCurrently66A.Text = "No";
                                                    }
                                                }
                                            }
                                        }

                                        if (NationalFilingBasisNodeItem.Name == "ns2:AmendedBasis")
                                        {
                                            XmlNodeList NationalFilingBasisInnerNode = NationalFilingBasisNodeItem.ChildNodes;
                                            foreach (XmlNode NationalFilingBasisInnerNodeItem in NationalFilingBasisInnerNode)
                                            {
                                                if (NationalFilingBasisInnerNodeItem.Name == "ns2:BasisForeignApplicationIndicator")
                                                {
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "true")
                                                    {
                                                        lblAmended44D.Text = "Yes";
                                                    }
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "false")
                                                    {
                                                        lblAmended44D.Text = "No";
                                                    }
                                                }

                                                if (NationalFilingBasisInnerNodeItem.Name == "ns2:BasisForeignRegistrationIndicator")
                                                {
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "true")
                                                    {
                                                        lblAmended44E.Text = "Yes";
                                                    }
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "false")
                                                    {
                                                        lblAmended44E.Text = "No";
                                                    }
                                                }
                                                if (NationalFilingBasisInnerNodeItem.Name == "ns2:BasisUseIndicator")
                                                {
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "true")
                                                    {
                                                        lblAmendedUse.Text = "Yes";
                                                    }
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "false")
                                                    {
                                                        lblAmendedUse.Text = "No";
                                                    }
                                                }
                                                if (NationalFilingBasisInnerNodeItem.Name == "ns2:BasisIntentToUseIndicator")
                                                {
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "true")
                                                    {
                                                        lblAmendedITU.Text = "Yes";
                                                    }
                                                    if (NationalFilingBasisInnerNodeItem.InnerText == "false")
                                                    {
                                                        lblAmendedITU.Text = "No";
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }

                                if (generalNodeItem.Name == "ns2:ApplicationDate")
                                {
                                    lblApplicationFilingDate.Text = generalNodeItem.InnerText;
                                    applicationDate = generalNodeItem.InnerText;
                                    if (generalNodeItem.Name == "ns2:MarkDescriptionBag")
                                    {

                                    }

                                }
                                if (generalNodeItem.Name == "ns2:MarkCategory")
                                {
                                    lblMarkType.Text = generalNodeItem.InnerText;
                                }
                                if (generalNodeItem.Name == "ns1:RegistrationNumber")
                                {
                                    lblUSRegistrationNumber.Text = generalNodeItem.InnerText;
                                }
                                // Start looping for status
                                if (generalNodeItem.Name == "ns2:NationalTrademarkInformation")
                                {
                                    XmlNodeList markCurrentStatusExternalDescriptionTextNode = generalNodeItem.ChildNodes;
                                    foreach (XmlNode MarkCurrentStatusExternalDescriptionTextNodeItem in markCurrentStatusExternalDescriptionTextNode)
                                    {
                                        if (MarkCurrentStatusExternalDescriptionTextNodeItem.Name == "ns2:MarkCurrentStatusExternalDescriptionText")
                                        {
                                            // information for status
                                            lblStatus.Text = MarkCurrentStatusExternalDescriptionTextNodeItem.InnerText;
                                        }
                                        if (MarkCurrentStatusExternalDescriptionTextNodeItem.Name == "ns2:NationalCaseLocation")
                                        {
                                            XmlNodeList nationalCaseLocationNode = MarkCurrentStatusExternalDescriptionTextNodeItem.ChildNodes;
                                            foreach (XmlNode nationalCaseLocationNodeItem in nationalCaseLocationNode)
                                            {
                                                if (nationalCaseLocationNodeItem.Name == "ns2:LawOfficeAssignedText")
                                                {
                                                    lblLawOfficeAssigned.Text = nationalCaseLocationNodeItem.InnerText;
                                                }
                                            }
                                        }
                                        if (MarkCurrentStatusExternalDescriptionTextNodeItem.Name == "ns2:RegisterCategory")
                                        {
                                            // getting information for register
                                            lblRegister.Text = MarkCurrentStatusExternalDescriptionTextNodeItem.InnerText;
                                        }
                                    }


                                }// end looping for status
                                if (generalNodeItem.Name == "ns2:MarkCurrentStatusDate")
                                {
                                    lblStatusDate.Text = generalNodeItem.InnerText;
                                }
                                // start looping for publication date
                                if (generalNodeItem.Name == "ns2:PublicationBag")
                                {

                                    XmlNodeList publicationNode = generalNodeItem.ChildNodes;
                                    foreach (XmlNode publicationNodeItem in publicationNode)
                                    {
                                        if (publicationNodeItem.Name == "ns2:Publication")
                                        {
                                            XmlNodeList publicationDateNode = publicationNodeItem.ChildNodes;
                                            foreach (XmlNode publicationDateNodeItem in publicationDateNode)
                                            {
                                                if (publicationDateNodeItem.Name == "ns1:PublicationDate")
                                                {
                                                    // information for publication date
                                                    lblPublicationDate.Text = publicationDateNodeItem.InnerText;
                                                }
                                            }


                                        }
                                    }


                                }// end looping for publication date


                                // start looping for Notice of Allowance Date
                                if (generalNodeItem.Name == "ns2:NationalTrademarkInformation")
                                {
                                    XmlNodeList allowanceNoticeDateNode = generalNodeItem.ChildNodes;
                                    foreach (XmlNode allowanceNoticeDateNodeItem in allowanceNoticeDateNode)
                                    {
                                        if (allowanceNoticeDateNodeItem.Name == "ns2:AllowanceNoticeDate")
                                        {
                                            // Notice of Allowance Date information
                                            lblNoticeofAllowanceDate.Text = allowanceNoticeDateNodeItem.InnerText;
                                        }
                                        if (allowanceNoticeDateNodeItem.Name == "ns2:RenewalDate")
                                        {
                                            // renewal date's information
                                            lblRenewalDate.Text = allowanceNoticeDateNodeItem.InnerText;
                                        }
                                    }


                                }//  end loopin for Notice of Allowance Date

                                if (generalNodeItem.Name == "ns1:RegistrationDate")
                                {
                                    // date of registration information
                                    lblRegisterDate.Text = generalNodeItem.InnerText;
                                }

                                //Looping start for the Goods and services ns2:GoodsServicesBag
                                if (generalNodeItem.Name == "ns2:GoodsServicesBag")
                                {
                                    XmlNodeList goodsServicesBagNode = generalNodeItem.ChildNodes;
                                    foreach (XmlNode goodsServicesBagNodeItem in goodsServicesBagNode)
                                    {
                                        if (goodsServicesBagNodeItem.Name == "ns2:GoodsServices")
                                        {
                                            XmlNodeList goodsServicesClassificationBagNode = goodsServicesBagNodeItem.ChildNodes;
                                            foreach (XmlNode goodsServicesClassificationBagNodeNodeItem in goodsServicesClassificationBagNode)
                                            {
                                                if (goodsServicesClassificationBagNodeNodeItem.Name == "ns2:GoodsServicesClassificationBag")
                                                {
                                                    XmlNodeList goodsServicesClassificationNode = goodsServicesClassificationBagNodeNodeItem.ChildNodes;
                                                    int ClassNumber = 0;
                                                    string ClassNumberValue = "";
                                                    string USClasValue = "";
                                                    foreach (XmlNode goodsServicesClassificationNodeItem in goodsServicesClassificationNode)
                                                    {

                                                        if (goodsServicesClassificationNodeItem.Name == "ns2:GoodsServicesClassification")
                                                        {
                                                            XmlNodeList goodsServicesClassificationInnerNode = goodsServicesClassificationNodeItem.ChildNodes;
                                                            foreach (XmlNode goodsServicesClassificationInnerNodeItem in goodsServicesClassificationInnerNode)
                                                            {
                                                                if (goodsServicesClassificationInnerNodeItem.Name == "ns2:ClassificationKindCode")
                                                                {
                                                                    if (ClassNumber == 0)
                                                                    {
                                                                        ClassNumberValue += goodsServicesClassificationInnerNodeItem.InnerText + "-";
                                                                    }

                                                                }
                                                                if (goodsServicesClassificationInnerNodeItem.Name == "ns2:ClassNumber")
                                                                {
                                                                    if (ClassNumber == 0)
                                                                    {
                                                                        ClassNumberValue += goodsServicesClassificationInnerNodeItem.InnerText;
                                                                    }

                                                                }
                                                                if (goodsServicesClassificationInnerNodeItem.Name == "ns2:NationalClassNumber")
                                                                {
                                                                    USClasValue += goodsServicesClassificationInnerNodeItem.InnerText + ",";
                                                                }

                                                            }

                                                            lblInternationalClasses.Text = ClassNumberValue;

                                                        }
                                                        ClassNumber++;

                                                    }
                                                    lblUSClasses.Text = USClasValue.TrimEnd(',');

                                                }
                                                if (goodsServicesClassificationBagNodeNodeItem.Name == "ns2:NationalFilingBasis")
                                                {
                                                    XmlNodeList nationalFilingBasisInnerNode = goodsServicesClassificationBagNodeNodeItem.ChildNodes;
                                                    foreach (XmlNode nationalFilingBasisInnerNodeItem in nationalFilingBasisInnerNode)
                                                    {
                                                        if (nationalFilingBasisInnerNodeItem.Name == "ns2:FirstUsedDate")
                                                        {
                                                            lblFirstUse.Text = nationalFilingBasisInnerNodeItem.InnerText;
                                                        }
                                                        if (nationalFilingBasisInnerNodeItem.Name == "ns2:FirstUsedCommerceDate")
                                                        {
                                                            lblUseinCommerce.Text = nationalFilingBasisInnerNodeItem.InnerText;
                                                        }
                                                    }


                                                }
                                                if (goodsServicesClassificationBagNodeNodeItem.Name == "ns2:ClassDescriptionBag")
                                                {

                                                    XmlNodeList classDescriptionNode = goodsServicesClassificationBagNodeNodeItem.ChildNodes;
                                                    foreach (XmlNode classDescriptionNodeItem in classDescriptionNode)
                                                    {
                                                        if (classDescriptionNodeItem.Name == "ns2:ClassDescription")
                                                        {
                                                            XmlNodeList goodsServicesDescriptionTextNode = classDescriptionNodeItem.ChildNodes;


                                                            foreach (XmlNode goodsServicesDescriptionTextNodeItem in goodsServicesDescriptionTextNode)
                                                            {
                                                                if (goodsServicesDescriptionTextNodeItem.Name == "ns2:GoodsServicesDescriptionText")
                                                                {
                                                                    lblFor.Text = goodsServicesDescriptionTextNodeItem.InnerText;
                                                                }
                                                                if (goodsServicesDescriptionTextNodeItem.Name == "ns2:NationalStatusBag")
                                                                {
                                                                    XmlNodeList nationalStatusNode = goodsServicesDescriptionTextNodeItem.ChildNodes;
                                                                    foreach (XmlNode nationalStatusNodeNodeItem in nationalStatusNode)
                                                                    {
                                                                        if (nationalStatusNodeNodeItem.Name == "ns2:NationalStatus")
                                                                        {
                                                                            XmlNodeList nationalStatusInnerNode = nationalStatusNodeNodeItem.ChildNodes;
                                                                            foreach (XmlNode nationalStatusInnerNodeItem in nationalStatusInnerNode)
                                                                            {
                                                                                if (nationalStatusInnerNodeItem.Name == "ns2:NationalStatusExternalDescriptionText")
                                                                                {
                                                                                    lblClassStatus.Text = nationalStatusInnerNodeItem.InnerText;
                                                                                }
                                                                            }


                                                                        }
                                                                    }

                                                                }
                                                            }
                                                        }

                                                        //<ns2:NationalStatusExternalDescriptionText

                                                    }
                                                }
                                            }

                                        }

                                    }
                                }//Looping end for the Goods and services ns2:GoodsServices

                                // Start looping for current owner information
                                int currentOwner = 0;
                                if (generalNodeItem.Name == "ns2:ApplicantBag")
                                {
                                    XmlNodeList applicantNode = generalNodeItem.ChildNodes;
                                    foreach (XmlNode applicantNodeItem in applicantNode)
                                    {

                                        if (applicantNodeItem.Name == "ns2:Applicant")
                                        {

                                            if (currentOwner == 0)
                                            {
                                                XmlNodeList currentOwnerNode = applicantNodeItem.ChildNodes;
                                                foreach (XmlNode currentOwnerNodeItem in currentOwnerNode)
                                                {



                                                    if (currentOwnerNodeItem.Name == "ns1:Contact")
                                                    {

                                                        XmlNodeList applicantNameNode = currentOwnerNodeItem.ChildNodes;
                                                        foreach (XmlNode applicantNameNodeItem in applicantNameNode)
                                                        {
                                                            if (applicantNameNodeItem.Name == "ns1:Name")
                                                            {
                                                                XmlNodeList EntityNameNode = applicantNameNodeItem.ChildNodes;
                                                                foreach (XmlNode EntityNameNodeItem in EntityNameNode)
                                                                {
                                                                    if (EntityNameNodeItem.Name == "ns1:EntityName")
                                                                    {
                                                                        lblOwnerName.Text = EntityNameNodeItem.InnerText;
                                                                    }
                                                                }


                                                            }




                                                            //   ns2: ApplicantBag / ns2:Applicant / ns1:Contact / ns1:PostalAddressBag / ns1:PostalAddress / ns1:PostalStructuredAddress / ns1:AddressLineText
                                                            if (applicantNameNodeItem.Name == "ns1:PostalAddressBag")
                                                            {
                                                                XmlNodeList postalAddressNode = applicantNameNodeItem.ChildNodes;
                                                                foreach (XmlNode postalAddressNodeItem in postalAddressNode)
                                                                {
                                                                    if (postalAddressNodeItem.Name == "ns1:PostalAddress")
                                                                    {
                                                                        XmlNodeList postalStructuredAddressNode = postalAddressNodeItem.ChildNodes;
                                                                        foreach (XmlNode postalStructuredAddressNodeItem in postalStructuredAddressNode)
                                                                        {
                                                                            if (postalStructuredAddressNodeItem.Name == "ns1:PostalStructuredAddress")
                                                                            {
                                                                                XmlNodeList addressLineTextNode = postalStructuredAddressNodeItem.ChildNodes;
                                                                                foreach (XmlNode addressLineTextNodeItem in addressLineTextNode)
                                                                                {

                                                                                    if (addressLineTextNodeItem.Name == "ns1:AddressLineText")
                                                                                    {

                                                                                        lblOwnerAddress.Text += addressLineTextNodeItem.InnerText + " ";
                                                                                    }
                                                                                    if (addressLineTextNodeItem.Name == "ns1:CityName")
                                                                                    {
                                                                                        lblOwnerAddress.Text += addressLineTextNodeItem.InnerText + " ";
                                                                                    }
                                                                                    if (addressLineTextNodeItem.Name == "ns1:GeographicRegionName")
                                                                                    {
                                                                                        lblOwnerAddress.Text += addressLineTextNodeItem.InnerText + " ";
                                                                                    }
                                                                                    if (addressLineTextNodeItem.Name == "ns1:PostalCode")
                                                                                    {
                                                                                        lblOwnerAddress.Text += addressLineTextNodeItem.InnerText + " ";
                                                                                    }
                                                                                }
                                                                            }
                                                                        }

                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    //ns2:ApplicantBag/ns2:Applicant/ns1:LegalEntityName
                                                    if (currentOwnerNodeItem.Name == "ns1:LegalEntityName")
                                                    {
                                                        lblLegalEntityType.Text = currentOwnerNodeItem.InnerText;
                                                    } // end if condition for the LegalEntityName

                                                    //ns2:ApplicantBag/ns2:Applicant/ns1:IncorporationState
                                                    if (currentOwnerNodeItem.Name == "ns1:IncorporationState")
                                                    {
                                                        lblStateorCountryWhereOrganized.Text = currentOwnerNodeItem.InnerText;
                                                    }
                                                }
                                                currentOwner++;
                                            }

                                        }


                                    }

                                }   // end looping for current owner information


                                //ns2:NationalCorrespondent/ns1:Contact/ns1:Name/ns1:PersonName/ns1:PersonFullName
                                if (generalNodeItem.Name == "ns2:NationalCorrespondent")
                                {
                                    XmlNodeList NationalCorrespondentContactNode = generalNodeItem.ChildNodes;
                                    foreach (XmlNode NationalCorrespondentContactNodeItem in NationalCorrespondentContactNode)
                                    {
                                        if (NationalCorrespondentContactNodeItem.Name == "ns1:Contact")
                                        {

                                            XmlNodeList nationalCorrespondentPersonNameNode = NationalCorrespondentContactNodeItem.ChildNodes;
                                            foreach (XmlNode postalStructuredAddressNodeItem in nationalCorrespondentPersonNameNode)
                                            {
                                                if (postalStructuredAddressNodeItem.Name == "ns1:Name")
                                                {
                                                    XmlNodeList nationalCorrespondentNameNode = postalStructuredAddressNodeItem.ChildNodes;
                                                    foreach (XmlNode nationalCorrespondentNameNodeItem in nationalCorrespondentNameNode)
                                                    {


                                                        if (nationalCorrespondentNameNodeItem.Name == "ns1:PersonName")
                                                        {

                                                            XmlNodeList nationalCorrespondentPersonFullNameNode = nationalCorrespondentNameNodeItem.ChildNodes;
                                                            foreach (XmlNode nationalCorrespondentPersonFullNameNodeItem in nationalCorrespondentPersonFullNameNode)
                                                            {
                                                                if (nationalCorrespondentPersonFullNameNodeItem.Name == "ns1:PersonFullName")
                                                                {

                                                                    lblAttorneyName.Text = nationalCorrespondentPersonFullNameNodeItem.InnerText;
                                                                }
                                                            }

                                                        }
                                                        if (nationalCorrespondentNameNodeItem.Name == "ns1:OrganizationName")
                                                        {
                                                            XmlNodeList nationalCorrespondentOrganizationNameNode = nationalCorrespondentNameNodeItem.ChildNodes;
                                                            foreach (XmlNode nationalCorrespondentOrganizationNameNodeItem in nationalCorrespondentOrganizationNameNode)
                                                            {
                                                                if (nationalCorrespondentOrganizationNameNodeItem.Name == "ns1:OrganizationStandardName")
                                                                {

                                                                    lblAttorneyName.Text = nationalCorrespondentOrganizationNameNodeItem.InnerText;
                                                                }
                                                            }

                                                        }



                                                    }
                                                }
                                                // looping for postal address
                                                if (postalStructuredAddressNodeItem.Name == "ns1:PostalAddressBag")
                                                {
                                                    XmlNodeList postalAddressNode = postalStructuredAddressNodeItem.ChildNodes;
                                                    foreach (XmlNode postalAddressNodeItem in postalAddressNode)
                                                    {
                                                        if (postalAddressNodeItem.Name == "ns1:PostalAddress")
                                                        {
                                                            XmlNodeList postalStructuredAddressInnerNode = postalAddressNodeItem.ChildNodes;
                                                            foreach (XmlNode postalStructuredAddressNodeinnerItem in postalStructuredAddressInnerNode)
                                                            {
                                                                if (postalStructuredAddressNodeinnerItem.Name == "ns1:PostalStructuredAddress")
                                                                {
                                                                    XmlNodeList addressLineTextNode = postalStructuredAddressNodeinnerItem.ChildNodes;
                                                                    foreach (XmlNode addressLineTextNodeItem in addressLineTextNode)
                                                                    {

                                                                        if (addressLineTextNodeItem.Name == "ns1:AddressLineText")
                                                                        {

                                                                            lblNameAddress.Text += addressLineTextNodeItem.InnerText + " ";
                                                                        }
                                                                        if (addressLineTextNodeItem.Name == "ns1:CityName")
                                                                        {
                                                                            lblNameAddress.Text += addressLineTextNodeItem.InnerText + " ";
                                                                        }
                                                                        if (addressLineTextNodeItem.Name == "ns1:GeographicRegionName")
                                                                        {
                                                                            lblNameAddress.Text += addressLineTextNodeItem.InnerText + " ";
                                                                        }
                                                                        if (addressLineTextNodeItem.Name == "ns1:PostalCode")
                                                                        {
                                                                            lblNameAddress.Text += addressLineTextNodeItem.InnerText + " ";
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                        }
                                                    }
                                                }// postal address looping end here

                                                if (postalStructuredAddressNodeItem.Name == "ns1:FaxNumberBag")
                                                {
                                                    XmlNodeList faxNumberBagNode = postalStructuredAddressNodeItem.ChildNodes;
                                                    foreach (XmlNode faxNumberBagNodeItem in faxNumberBagNode)
                                                    {
                                                        if (faxNumberBagNodeItem.Name == "ns1:FaxNumber")
                                                        {

                                                            lblFax.Text = faxNumberBagNodeItem.InnerText;


                                                        }
                                                    }

                                                }
                                                if (postalStructuredAddressNodeItem.Name == "ns1:EmailAddressBag")
                                                {
                                                    XmlNodeList emailAddressTextNode = postalStructuredAddressNodeItem.ChildNodes;
                                                    foreach (XmlNode emailAddressTextNodeItem in emailAddressTextNode)
                                                    {
                                                        if (emailAddressTextNodeItem.Name == "ns1:EmailAddressText")
                                                        {

                                                            lblCorrespondntEmail.Text = emailAddressTextNodeItem.InnerText;

                                                        }
                                                    }

                                                }
                                                if (postalStructuredAddressNodeItem.Name == "ns1:PhoneNumberBag")
                                                {
                                                    XmlNodeList phoneNumberNode = postalStructuredAddressNodeItem.ChildNodes;
                                                    foreach (XmlNode phoneNumberNodeItem in phoneNumberNode)
                                                    {
                                                        if (phoneNumberNodeItem.Name == "ns1:PhoneNumber")
                                                        {
                                                            lblPhone.Text = phoneNumberNodeItem.InnerText;

                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }

                                }


                                if (generalNodeItem.Name == "ns1:StaffBag")
                                {
                                    XmlNodeList staffBagNode = generalNodeItem.ChildNodes;
                                    foreach (XmlNode staffBagNodeItem in staffBagNode)
                                    {
                                        if (staffBagNodeItem.Name == "ns1:Staff")
                                        {
                                            XmlNodeList StaffNode = generalNodeItem.ChildNodes;
                                            foreach (XmlNode StaffNodeItem in StaffNode)
                                            {
                                                if (StaffNodeItem.Name == "ns1:StaffName")
                                                {
                                                    lblTMAttorney.Text = StaffNodeItem.InnerText;
                                                }
                                            }
                                        }
                                    }
                                }




                                // start looping for
                                if (generalNodeItem.Name == "ns2:RecordAttorney")
                                {
                                    //ns2:RecordAttorney/ns1:Contact/ns1:Name/ns1:PersonName/ns1:PersonFullName

                                    XmlNodeList RecordAttorneyDocketTextNode = generalNodeItem.ChildNodes;
                                    foreach (XmlNode RecordAttorneyDocketTextNodeItem in RecordAttorneyDocketTextNode)
                                    {
                                        if (RecordAttorneyDocketTextNodeItem.Name == "ns2:DocketText")
                                        {
                                            lblDocketNumber.Text = RecordAttorneyDocketTextNodeItem.InnerText;
                                        }
                                        if (RecordAttorneyDocketTextNodeItem.Name == "ns1:Contact")
                                        {

                                            XmlNodeList RecordAttorneyPersonNameNode = RecordAttorneyDocketTextNodeItem.ChildNodes;
                                            foreach (XmlNode RecordAttorneyPersonNameNodeItem in RecordAttorneyPersonNameNode)
                                            {

                                                if (RecordAttorneyPersonNameNodeItem.Name == "ns1:Name")
                                                {

                                                    XmlNodeList RecordAttorneyPersonFullNameNode = RecordAttorneyPersonNameNodeItem.ChildNodes;

                                                    foreach (XmlNode RecordAttorneyPersonFullNameNodeItem in RecordAttorneyPersonFullNameNode)
                                                    {
                                                        if (RecordAttorneyPersonFullNameNodeItem.Name == "ns1:PersonName")
                                                        {
                                                            XmlNodeList RecordAttorneyNameNode = RecordAttorneyPersonFullNameNodeItem.ChildNodes;

                                                            foreach (XmlNode RecordAttorneyNameNodeItem in RecordAttorneyNameNode)
                                                            {
                                                                if (RecordAttorneyNameNodeItem.Name == "ns1:PersonFullName")
                                                                {
                                                                    lblCorrespondent.Text = RecordAttorneyNameNodeItem.InnerText;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }

                                // looping for prosecution history

                                StringBuilder sb = new StringBuilder();

                                sb.Append("<table border='1' cellpadding='0' cellspacing='0' class='table table-striped table-border'>");
                                sb.Append("<tr>");

                                sb.Append("<td>Description</td>");
                                sb.Append("<td>Proceeding Number</td>");
                                sb.Append("<td>Date	</td>");

                                sb.Append("</tr>");
                                if (generalNodeItem.Name == "ns2:MarkEventBag")
                                {
                                    XmlNodeList markEventBagNode = generalNodeItem.ChildNodes;
                                    foreach (XmlNode markEventBagNodeItem in markEventBagNode)
                                    {
                                        sb.Append("<tr>");
                                        if (markEventBagNodeItem.Name == "ns2:MarkEvent")
                                        {
                                            XmlNodeList markEventNode = markEventBagNodeItem.ChildNodes;
                                            foreach (XmlNode markEventNodeItem in markEventNode)
                                            {


                                                //XmlNodeList MarkEventInnerNode = markEventNodeItem.ChildNodes;
                                                //foreach(XmlNode markEventNodeItem in markEventNode)
                                                //{
                                                if (markEventNodeItem.Name == "ns2:NationalMarkEvent")
                                                {
                                                    //ns2:MarkEvent

                                                    XmlNodeList nationalMarkEventNode = markEventNodeItem.ChildNodes;
                                                    foreach (XmlNode nationalMarkEventNodeItem in nationalMarkEventNode)
                                                    {
                                                        if (nationalMarkEventNodeItem.Name == "ns2:MarkEventDescriptionText")
                                                        {
                                                            sb.Append("<td>" + nationalMarkEventNodeItem.InnerText + "</td>");
                                                        }
                                                        if (nationalMarkEventNodeItem.Name == "ns2:MarkEventAdditionalText")
                                                        {
                                                            sb.Append("<td>" + nationalMarkEventNodeItem.InnerText + "</td>");
                                                        }
                                                    }

                                                }
                                                if (markEventNodeItem.Name == "ns2:MarkEventDate")
                                                {
                                                    sb.Append("<td>" + markEventNodeItem.InnerText + "</td>");
                                                }

                                            }

                                            sb.Append("</tr>");
                                        }


                                    }
                                    sb.Append("</table>");
                                    Grd1.InnerHtml = Convert.ToString(sb);
                                }

                                // Maintenance Filings or Post Registration Information
                                if (generalNodeItem.Name == "ns2:NationalTrademarkInformation")
                                {
                                    int MaintenanceFiling = 0;
                                    XmlNodeList nationalTrademarkInformationNode = generalNodeItem.ChildNodes;
                                    foreach (XmlNode nationalTrademarkInformationNodeItem in nationalTrademarkInformationNode)
                                    {
                                        if (nationalTrademarkInformationNodeItem.Name == "ns2:MaintenanceFilingBag")
                                        {
                                            XmlNodeList MaintenanceFilingBagNode = nationalTrademarkInformationNodeItem.ChildNodes;
                                            foreach (XmlNode MaintenanceFilingBagNodeItem in MaintenanceFilingBagNode)
                                            {
                                                if (MaintenanceFilingBagNodeItem.Name == "ns2:MaintenanceFiling")
                                                {
                                                    XmlNodeList MaintenanceFilingNode = MaintenanceFilingBagNodeItem.ChildNodes;
                                                    foreach (XmlNode MaintenanceFilingNodeItem in MaintenanceFilingNode)
                                                    {
                                                        if (MaintenanceFilingNodeItem.Name == "ns2:SectionText")
                                                        {
                                                            if (MaintenanceFiling == 0)
                                                            {
                                                                lblAffidavitofContinuedUse.Text += "Section" + MaintenanceFilingNodeItem.InnerText + "-";
                                                            }
                                                            else
                                                            {
                                                                lblAffidavitofIncontestability.Text += "Section" + MaintenanceFilingNodeItem.InnerText + "-";
                                                            }

                                                        }
                                                        if (MaintenanceFilingNodeItem.Name == "ns2:DecisionStatusCategory")
                                                        {
                                                            if (MaintenanceFiling == 0)
                                                                lblAffidavitofContinuedUse.Text += MaintenanceFilingNodeItem.InnerText;
                                                            else
                                                                lblAffidavitofIncontestability.Text += MaintenanceFilingNodeItem.InnerText;
                                                        }
                                                        MaintenanceFiling++;
                                                    }
                                                }//ns2:SectionText
                                            }
                                        }
                                        if (nationalTrademarkInformationNodeItem.Name == "ns2:RenewalDate")
                                        {
                                            lblRenewalDate.Text = nationalTrademarkInformationNodeItem.InnerText;
                                        }
                                        if (nationalTrademarkInformationNodeItem.Name == "ns2:NationalCaseLocation")
                                        {
                                            XmlNodeList nationalCaseLocationNode = nationalTrademarkInformationNodeItem.ChildNodes;
                                            foreach (XmlNode nationalCaseLocationNodeItem in nationalCaseLocationNode)
                                            {
                                                if (nationalCaseLocationNodeItem.Name == "ns2:CurrentLocationText")
                                                {
                                                    lblCurrentLocation.Text = nationalCaseLocationNodeItem.InnerText;
                                                }
                                                if (nationalCaseLocationNodeItem.Name == "ns2:CurrentLocationDate")
                                                {
                                                    lblDateinLocation.Text = nationalCaseLocationNodeItem.InnerText.TrimEnd('-');
                                                }
                                            }

                                        }
                                    }
                                }

                                // Assignment Abstract Of Title Information

                                // reference xml <ns2:AssignmentBag>
                                //< ns2:Assignment >
                                // < ns2:AssignmentIdentifier > 28720331 </ ns2:AssignmentIdentifier >
                                //< ns2:AssignmentConveyanceCategory > CHANGE OF NAME</ ns2:AssignmentConveyanceCategory >
                                //  < ns1:PageTotalQuantity > 4 </ ns1:PageTotalQuantity >
                                //  < ns2:AssignmentReceivedDate > 2004 - 06 - 10 </ ns2:AssignmentReceivedDate >
                                // < ns2:AssignmentMailedDate > 2004 - 06 - 17 </ ns2:AssignmentMailedDate >
                                //    < ns2:AssignmentRecordedDate > 2004 - 06 - 10 </ ns2:AssignmentRecordedDate >
                                //< ns2:AssignmentExecutedDate > 2002 - 01 - 10 </ ns2:AssignmentExecutedDate >
                                //< ns2:AssignorBag >
                                //<< ns2:Assignor >
                                //< < ns1:LegalEntityName > CORPORATION </ ns1:LegalEntityName >
                                // < ns1:NationalLegalEntityCode > 3 </ ns1:NationalLegalEntityCode >
                                //  < ns1:IncorporationCountryCode > US </ ns1:IncorporationCountryCode >
                                // < ns1:IncorporationState > ARKANSAS </ ns1:IncorporationState >
                                // < ns1:Contact >
                                //  < ns1:Name >
                                // < ns1:EntityName > ARKAT FEEDS, INC.</ ns1:EntityName >
                                //     </ ns1:Name >
                                // </ ns1:Contact >
                                //  </ ns2:Assignor >
                                //   </ ns2:AssignorBag >
                                //    < ns2:AssigneeBag >
                                //< ns2:Assignee >
                                // < ns1:LegalEntityName > CORPORATION </ ns1:LegalEntityName >
                                // < ns1:NationalLegalEntityCode > 3 </ ns1:NationalLegalEntityCode >
                                //  < ns1:IncorporationCountryCode > US </ ns1:IncorporationCountryCode >
                                // < ns1:IncorporationState > ARKANSAS </ ns1:IncorporationState >
                                // < ns1:Contact >
                                //  < ns1:Name >
                                // < ns1:OrganizationName >
                                // < ns1:OrganizationStandardName > ARKAT NUTRITION, INC.</ ns1:OrganizationStandardName >
                                //</ ns1:OrganizationName >
                                //< ns1:EntityName > ARKAT NUTRITION, INC.</ ns1:EntityName >
                                // </ ns1:Name >
                                //< ns1:PostalAddressBag >
                                //<< ns1:PostalAddress >
                                //< < ns1:PostalAddressText ns1:sequenceNumber = "1" > 800 SOUTH MAIN STREET </ ns1:PostalAddressText >
                                //<        < ns1:PostalAddressText ns1:sequenceNumber = "1" > DUMAS, ARKANSAS 71639 </ ns1:PostalAddressText >
                                // </ ns1:PostalAddress >
                                //  </ ns1:PostalAddressBag >
                                // </ ns1:Contact >
                                //  </ ns2:Assignee >
                                // </ ns2:AssigneeBag >
                                //  < ns2:Registrant >
                                //   < ns1:Contact >
                                //    < ns1:Name >
                                //     < ns1:EntityName > Arkat Feeds, Inc.</ ns1:EntityName >
                                //  </ ns1:Name >
                                //   </ ns1:Contact >
                                //    </ ns2:Registrant >
                                //     < ns2:NationalRepresentative />
                                //  < ns2:NationalCorrespondent >
                                // < ns1:CommentText ns1:sequenceNumber = "1" > Correspondent </ ns1:CommentText >
                                //  < ns1:Contact >
                                //   < ns1:Name >
                                //   < ns1:EntityName > J.CHARLES DOUGHERTY </ ns1:EntityName >
                                //  </ ns1:Name >
                                //  < ns1:PostalAddressBag >
                                //  < ns1:PostalAddress >
                                //< ns1:PostalAddressText ns1:sequenceNumber = "1" > 200 W.CAPITOL AVE.</ ns1:PostalAddressText >
                                //   < ns1:PostalAddressText ns1:sequenceNumber = "1" > SUITE 2300 </ ns1:PostalAddressText >
                                //   < ns1:PostalAddressText ns1:sequenceNumber = "1" > LITTLE ROCK, AR 72201 - 3699 </ ns1:PostalAddressText >
                                //     </ ns1:PostalAddress >
                                //          </ ns1:PostalAddressBag >
                                //           </ ns1:Contact >
                                //            </ ns2:NationalCorrespondent >
                                //             < ns2:AssignmentGroupCategory > Ownership and Name Change </ ns2:AssignmentGroupCategory >
                                //                  < ns2:AssignmentDocumentBag >
                                //                   < ns2:TrademarkDocument >
                                //                                                                                < ns2:DocumentCreatedDate > 2004 - 06 - 10 </ ns2:DocumentCreatedDate >
                                //                                                                                         < ns2:TrademarkDocumentDescriptionText >
                                //                                                                                          http://assignments.uspto.gov/assignments/assignment-tm-2872-0331.pdf
                                //</ ns2:TrademarkDocumentDescriptionText >
                                // < ns1:PageTotalQuantity > 4 </ ns1:PageTotalQuantity >
                                //      < ns1:DocumentIdentifier >
                                //       http://assignments.uspto.gov/assignments/assignment-tm-2872-0331.pdf
                                //</ ns1:DocumentIdentifier >
                                // </ ns2:TrademarkDocument >
                                //  </ ns2:AssignmentDocumentBag >
                                //   </ ns2:Assignment >
                                if (generalNodeItem.Name == "ns2:AssignmentBag")
                                {
                                    int AssignmentNo = 0;
                                    StringBuilder stringBuilder = new StringBuilder();
                                    // stringBuilder.Append("<div>");

                                    XmlNodeList assignmentBagNodes = generalNodeItem.ChildNodes;

                                    foreach (XmlNode assignmentBagNodesItem in assignmentBagNodes)
                                    {
                                        stringBuilder.Append("<div>");
                                        string convenyance = "";
                                        string reel = "";
                                        string daterecorded = "";
                                        string pages = "";
                                        string supportingDoc = "";
                                        string assignorName = "";
                                        string assignorLegalEntity = "";
                                        string assignorState = "";
                                        string assigneeName = "";
                                        string assigneeLegalEntity = "";
                                        string assigneeAddress = "";
                                        string correspondentName = "";
                                        string correspondentAdress = "";
                                        string assigneeState = "";
                                        if (assignmentBagNodesItem.Name == "ns2:Assignment")
                                        {
                                            XmlNodeList assignmentNode = assignmentBagNodesItem.ChildNodes;

                                            foreach (XmlNode assignmentNodeItem in assignmentNode)
                                            {

                                                if (assignmentNodeItem.Name == "ns2:NationalAssignmentTotalQuantity")
                                                {

                                                }
                                                if (assignmentNodeItem.Name == "ns1:PageTotalQuantity")
                                                {


                                                    pages = assignmentNodeItem.InnerText;

                                                }
                                                if (assignmentNodeItem.Name == "ns2:AssignmentConveyanceCategory")
                                                {

                                                    convenyance = assignmentNodeItem.InnerText;

                                                }
                                                if (assignmentNodeItem.Name == "ns2:AssignmentIdentifier")
                                                {

                                                    // stringBuilder.Append("<div> Reel/Frame:" + assignmentNodeItem.InnerText + "</div>");
                                                    reel = assignmentNodeItem.InnerText;
                                                    //Conveyance:	ns2:AssignmentReceivedDate

                                                }
                                                if (assignmentNodeItem.Name == "ns2:AssignmentReceivedDate")
                                                {

                                                    // stringBuilder.Append("<div> Date Recorded:" + assignmentNodeItem.InnerText + "</div>");
                                                    daterecorded = assignmentNodeItem.InnerText;
                                                    //Supporting Documents:

                                                }
                                                if (assignmentNodeItem.Name == "ns2:AssignmentDocumentBag")
                                                {
                                                    XmlNodeList assignmentDocumentBagNode = assignmentNodeItem.ChildNodes;
                                                    foreach (XmlNode assignmentDocumentBagNodeItem in assignmentDocumentBagNode)
                                                    {
                                                        if (assignmentDocumentBagNodeItem.Name == "ns2:TrademarkDocument")
                                                        {
                                                            XmlNodeList trademarkDocumentNode = assignmentDocumentBagNodeItem.ChildNodes;

                                                            foreach (XmlNode trademarkDocumentNodeItem in trademarkDocumentNode)
                                                            {
                                                                if (trademarkDocumentNodeItem.Name == "ns2:TrademarkDocumentDescriptionText")
                                                                {
                                                                    //  stringBuilder.Append("<div>Supporting Documents:" + trademarkDocumentNodeItem.InnerText + "<div>");

                                                                    supportingDoc = trademarkDocumentNodeItem.InnerText;
                                                                }
                                                            }
                                                        }
                                                    }


                                                    //Supporting Documents: ns2:TrademarkDocumentDescriptionText

                                                }
                                                // Start looping for Assignor data
                                                if (assignmentNodeItem.Name == "ns2:AssignorBag")
                                                {
                                                    XmlNodeList assignorBagNode = assignmentNodeItem.ChildNodes;

                                                    foreach (XmlNode assignorBagNodeItem in assignorBagNode)
                                                    {
                                                        if (assignorBagNodeItem.Name == "ns2:Assignor")
                                                        {
                                                            XmlNodeList assignorNode = assignorBagNodeItem.ChildNodes;

                                                            foreach (XmlNode assignorNodeItem in assignorNode)
                                                            {
                                                                if (assignorNodeItem.Name == "ns1:Contact")
                                                                {
                                                                    XmlNodeList assignmentContactNode = assignorNodeItem.ChildNodes;
                                                                    foreach (XmlNode assignmentContactNodeItem in assignmentContactNode)
                                                                    {
                                                                        if (assignmentContactNodeItem.Name == "ns1:Name")
                                                                        {
                                                                            XmlNodeList assignmentAssignorName = assignmentContactNodeItem.ChildNodes;
                                                                            foreach (XmlNode assignmentAssignorNameItem in assignmentAssignorName)
                                                                            {
                                                                                if (assignmentAssignorNameItem.Name == "ns1:EntityName")
                                                                                {
                                                                                    assignorName = assignmentAssignorNameItem.InnerText;
                                                                                    //  stringBuilder.Append("<div>Assignor Name:" + assignmentAssignorNameItem.InnerText + "<div>");
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (assignorNodeItem.Name == "ns1:IncorporationState")
                                                                {

                                                                    assignorState = assignorNodeItem.InnerText;
                                                                }
                                                                if (assignorNodeItem.Name == "ns1:LegalEntityName")
                                                                {
                                                                    // get the assignor 
                                                                    assignorLegalEntity = assignorNodeItem.InnerText;
                                                                }
                                                            }
                                                        }

                                                    }
                                                }// end of assignorBag
                                                if (assignmentNodeItem.Name == "ns2:AssigneeBag")
                                                {

                                                    XmlNodeList assigneeBagNode = assignmentNodeItem.ChildNodes;
                                                    foreach (XmlNode assigneeBagNodeItem in assigneeBagNode)
                                                    {
                                                        if (assigneeBagNodeItem.Name == "ns2:Assignee")
                                                        {
                                                            XmlNodeList assigneeNode = assigneeBagNodeItem.ChildNodes;
                                                            foreach (XmlNode assigneeNodeItem in assigneeNode)
                                                            {
                                                                if (assigneeNodeItem.Name == "ns1:LegalEntityName")
                                                                {
                                                                    assigneeLegalEntity = assigneeNodeItem.InnerText;
                                                                    //stringBuilder.Append("<div>Legal Entity Type:" + assigneeNodeItem.InnerText + "<div>");
                                                                }
                                                                if (assigneeNodeItem.Name == "ns1:IncorporationState")
                                                                {
                                                                    assigneeState = assigneeNodeItem.InnerText;
                                                                    // stringBuilder.Append("<div>State or Country Where Organized:	" + assigneeNodeItem.InnerText + "<div>");
                                                                }
                                                                if (assigneeNodeItem.Name == "ns1:Contact")
                                                                {
                                                                    XmlNodeList assigneeContactNode = assigneeNodeItem.ChildNodes;
                                                                    foreach (XmlNode assigneeContactNodeItem in assigneeContactNode)
                                                                    {
                                                                        if (assigneeContactNodeItem.Name == "ns1:Name")
                                                                        {
                                                                            XmlNodeList assigneeContactNameNode = assigneeContactNodeItem.ChildNodes;
                                                                            foreach (XmlNode assigneeContactNameNodeItem in assigneeContactNameNode)
                                                                            {
                                                                                if (assigneeContactNameNodeItem.Name == "ns1:OrganizationName")
                                                                                {
                                                                                    XmlNodeList assigneeContactOrganizationNameNode = assigneeContactNameNodeItem.ChildNodes;
                                                                                    foreach (XmlNode assigneeContactOrganizationNameNodeItem in assigneeContactOrganizationNameNode)
                                                                                    {
                                                                                        if (assigneeContactOrganizationNameNodeItem.Name == "ns1:OrganizationStandardName")
                                                                                        {
                                                                                            assigneeName = assigneeContactOrganizationNameNodeItem.InnerText;
                                                                                            //stringBuilder.Append("<div>Name:	" + assigneeContactOrganizationNameNodeItem.InnerText + "<div>");
                                                                                            //XmlNodeList assigneeContactOrganizationNameInnerNode = assigneeContactOrganizationNameNodeItem.ChildNodes;
                                                                                            //foreach (XmlNode assigneeContactOrganizationNameInnerNodeItem in assigneeContactOrganizationNameInnerNode)
                                                                                            //{
                                                                                            //    if (assigneeContactOrganizationNameInnerNodeItem.Name == "ns1:OrganizationStandardName")
                                                                                            //    {
                                                                                            //        stringBuilder.Append("<div>Name:	" + assigneeContactOrganizationNameInnerNodeItem.InnerText + "<div>");
                                                                                            //    }
                                                                                            //}
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        if (assigneeContactNodeItem.Name == "ns1:PostalAddressBag")
                                                                        {
                                                                            XmlNodeList assigneePostalAddressBag = assigneeContactNodeItem.ChildNodes;
                                                                            foreach (XmlNode assigneePostalAddressBagItem in assigneePostalAddressBag)
                                                                            {
                                                                                if (assigneePostalAddressBagItem.Name == "ns1:PostalAddress")
                                                                                {
                                                                                    XmlNodeList assigneePostalAddressNode = assigneePostalAddressBagItem.ChildNodes;
                                                                                    foreach (XmlNode assigneePostalAddressNodeItem in assigneePostalAddressNode)
                                                                                    {
                                                                                        if (assigneePostalAddressNodeItem.Name == "ns1:PostalAddressText")
                                                                                        {
                                                                                            assigneeAddress += assigneePostalAddressNodeItem.InnerText + " ";
                                                                                            //stringBuilder.Append("<div>Address:" + assigneePostalAddressNodeItem.InnerText + " <div>");
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                        }
                                                    }

                                                }// end looping for Assignee
                                                if (assignmentNodeItem.Name == "ns2:NationalCorrespondent")
                                                {


                                                    XmlNodeList assigneeNode = assignmentNodeItem.ChildNodes;
                                                    foreach (XmlNode assigneeNodeItem in assigneeNode)
                                                    {
                                                        if (assigneeNodeItem.Name == "ns1:Contact")
                                                        {
                                                            XmlNodeList assigneeContactNode = assigneeNodeItem.ChildNodes;
                                                            foreach (XmlNode assigneeContactNodeItem in assigneeContactNode)
                                                            {
                                                                if (assigneeContactNodeItem.Name == "ns1:Name")
                                                                {
                                                                    XmlNodeList assigneeContactNameNode = assigneeContactNodeItem.ChildNodes;
                                                                    foreach (XmlNode assigneeContactNameNodeItem in assigneeContactNameNode)
                                                                    {
                                                                        if (assigneeContactNameNodeItem.Name == "ns1:EntityName")
                                                                        {
                                                                            // get assignee name
                                                                            correspondentName = assigneeContactNameNodeItem.InnerText;
                                                                            //  stringBuilder.Append("<div>Address:" + assigneeContactNameNodeItem.InnerText + " <div>");
                                                                        }
                                                                    }
                                                                }
                                                                if (assigneeContactNodeItem.Name == "ns1:PostalAddressBag")
                                                                {
                                                                    XmlNodeList assigneePostalAddressBag = assigneeContactNodeItem.ChildNodes;
                                                                    foreach (XmlNode assigneePostalAddressBagItem in assigneePostalAddressBag)
                                                                    {
                                                                        if (assigneePostalAddressBagItem.Name == "ns1:PostalAddress")
                                                                        {
                                                                            XmlNodeList assigneePostalAddressNode = assigneePostalAddressBagItem.ChildNodes;
                                                                            foreach (XmlNode assigneePostalAddressNodeItem in assigneePostalAddressNode)
                                                                            {
                                                                                if (assigneePostalAddressNodeItem.Name == "ns1:PostalAddressText")
                                                                                {
                                                                                    // get the assignee address
                                                                                    correspondentAdress += assigneePostalAddressNodeItem.InnerText + "";
                                                                                    //stringBuilder.Append("<div>Address:" + assigneePostalAddressNodeItem.InnerText + " <div>");
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }




                                            }// end of assignment tag
                                            // build the assignments here
                                            stringBuilder.Append("<div>");
                                            stringBuilder.Append("<div>");
                                            stringBuilder.Append("<h3>Assignment</h3>");
                                            stringBuilder.Append("<div> Conveyance:" + convenyance + "</div>");
                                            stringBuilder.Append("<div> Reel/Frame:" + reel + "</div>");
                                            stringBuilder.Append("<div> Date Recorded:" + daterecorded + "</div>");
                                            stringBuilder.Append("<div> Supporting Document:" + " " + supportingDoc + "</div>");
                                            stringBuilder.Append("</div>");
                                            stringBuilder.Append("</div>");
                                            stringBuilder.Append("</div>");
                                            stringBuilder.Append("<div>");
                                            stringBuilder.Append("<div>");
                                            stringBuilder.Append("<h4>Assignor</h4>");
                                            stringBuilder.Append("<div> Name:" + assignorName + "</div>");
                                            stringBuilder.Append("<div> Legal Entity Type" + assignorLegalEntity + "</div>");
                                            stringBuilder.Append("<div> State or Country Where Organized:" + assignorState + "</div>");
                                            stringBuilder.Append("</div>");
                                            stringBuilder.Append("</div>");
                                            stringBuilder.Append("<div>");
                                            stringBuilder.Append("<div>");
                                            stringBuilder.Append("<h4>Assignor</h4>");
                                            stringBuilder.Append("<div> Name:" + assigneeName + "</div>");
                                            stringBuilder.Append("<div> Legal Entity Type" + assigneeLegalEntity + "</div>");
                                            stringBuilder.Append("<div>Address:" + assigneeState + "</div>");
                                            stringBuilder.Append("<div> State or Country Where Organized:" + assigneeName + "</div>");
                                            stringBuilder.Append("</div>");
                                            stringBuilder.Append("</div>");
                                            stringBuilder.Append("</div>");
                                            stringBuilder.Append("<div>");
                                            stringBuilder.Append("<div>");
                                            stringBuilder.Append("<h4>Correspondent</h4>");
                                            stringBuilder.Append("<div>Correspondent Name:" + correspondentName + "</div>");
                                            stringBuilder.Append("<div>Correspondent Address" + correspondentAdress + "</div>");

                                            stringBuilder.Append("</div>");
                                            stringBuilder.Append("</div>");
                                            stringBuilder.Append("</div>");
                                        }
                                    }
                                    divAssignmentAbstract.InnerHtml = Convert.ToString(stringBuilder);
                                }// end of assignment loop
                                //Start looping  for Proceeding
                                if (generalNodeItem.Name == "ns2:LegalProceedingsBag")
                                {
                                    XmlNodeList legalProceedingsBagNode = generalNodeItem.ChildNodes;
                                    foreach (XmlNode legalProceedingsBagNodeItem in legalProceedingsBagNode)
                                    {
                                        if (legalProceedingsBagNodeItem.Name == "ns2:LegalProceedings")
                                        {

                                            XmlNodeList legalProceedingsNode = legalProceedingsBagNodeItem.ChildNodes;
                                            foreach (XmlNode legalProceedingsNodeItem in legalProceedingsNode)
                                            {
                                                if (legalProceedingsNodeItem.Name == "ns2:OppositionProceedingBag")
                                                {

                                                    XmlNodeList oppositionProceedingBagNode = legalProceedingsNodeItem.ChildNodes;
                                                    foreach (XmlNode oppositionProceedingBagNodeItem in oppositionProceedingBagNode)
                                                    {
                                                        if (oppositionProceedingBagNodeItem.Name == "ns2:ProceedingStatus")
                                                        {

                                                            XmlNodeList proceedingStatusNode = oppositionProceedingBagNodeItem.ChildNodes;
                                                            foreach (XmlNode proceedingStatusItem in proceedingStatusNode)
                                                            {
                                                                if (proceedingStatusItem.Name == "ns2:NationalStatusDate")
                                                                {

                                                                    lblStatusDateProceeding.Text = proceedingStatusItem.InnerText;
                                                                }
                                                                if (proceedingStatusItem.Name == "ns2:NationalStatusExternalDescriptionText")
                                                                {
                                                                    lblStatusProceeding.Text = proceedingStatusItem.InnerText;
                                                                }
                                                            }
                                                            //lblStatusProceeding
                                                        }
                                                        if (oppositionProceedingBagNodeItem.Name == "ns1:OppositionDate")
                                                        {
                                                            lblFilingDate.Text = oppositionProceedingBagNodeItem.InnerText;
                                                        }
                                                        if (oppositionProceedingBagNodeItem.Name == "ns1:OppositionIdentifier")
                                                        {
                                                            lblProceedingNumber.Text = oppositionProceedingBagNodeItem.InnerText;
                                                        }
                                                        // getting defendent block data
                                                        if (oppositionProceedingBagNodeItem.Name == "ns2:Defendant")
                                                        {

                                                            XmlNodeList defendantNode = oppositionProceedingBagNodeItem.ChildNodes;
                                                            foreach (XmlNode defendantNodeItem in defendantNode)
                                                            {
                                                                if (defendantNodeItem.Name == "ns1:Contact")
                                                                {

                                                                    XmlNodeList defendantContactNode = defendantNodeItem.ChildNodes;
                                                                    foreach (XmlNode defendantContactNodeItem in defendantContactNode)
                                                                    {
                                                                        if (defendantContactNodeItem.Name == "ns1:Name")
                                                                        {

                                                                            XmlNodeList defendantContactNameNode = defendantContactNodeItem.ChildNodes;
                                                                            foreach (XmlNode defendantContactNameNodeItem in defendantContactNameNode)
                                                                            {
                                                                                if (defendantContactNameNodeItem.Name == "ns1:EntityName")
                                                                                {
                                                                                    lblName.Text = defendantContactNameNodeItem.InnerText;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                // getting correspondent data
                                                                if (defendantNodeItem.Name == "ns2:Correspondent")
                                                                {

                                                                    XmlNodeList defendantCorrespondentNode = defendantNodeItem.ChildNodes;
                                                                    foreach (XmlNode defendantCorrespondentNodeItem in defendantCorrespondentNode)
                                                                    {
                                                                        if (defendantCorrespondentNodeItem.Name == "ns1:Contact")
                                                                        {

                                                                            XmlNodeList defendantCorrespondentContactNode = defendantCorrespondentNodeItem.ChildNodes;
                                                                            foreach (XmlNode defendantCorrespondentContactNodeItem in defendantCorrespondentContactNode)
                                                                            {
                                                                                if (defendantCorrespondentContactNodeItem.Name == "ns1:Name")
                                                                                {

                                                                                    XmlNodeList defendantCorrespondentNameNode = defendantCorrespondentContactNodeItem.ChildNodes;
                                                                                    foreach (XmlNode defendantCorrespondentNameNodeItem in defendantCorrespondentNameNode)
                                                                                    {
                                                                                        if (defendantCorrespondentNameNodeItem.Name == "ns1:PersonName")

                                                                                        {

                                                                                            XmlNodeList defendantCorrespondentPersonNameNode = defendantCorrespondentNameNodeItem.ChildNodes;
                                                                                            foreach (XmlNode defendantCorrespondentPersonNameNodeItem in defendantCorrespondentPersonNameNode)
                                                                                            {
                                                                                                if (defendantCorrespondentPersonNameNodeItem.Name == "ns1:PersonFullName")
                                                                                                {
                                                                                                    lblCorrespondentAddress.Text += defendantCorrespondentPersonNameNodeItem.InnerText;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        if (defendantCorrespondentNameNodeItem.Name == "ns1:OrganizationName")
                                                                                        {
                                                                                            XmlNodeList defendantCorrespondentOrganizationNameNode = defendantCorrespondentNameNodeItem.ChildNodes;
                                                                                            foreach (XmlNode defendantCorrespondentOrganizationNameNodeItem in defendantCorrespondentOrganizationNameNode)
                                                                                            {
                                                                                                if (defendantCorrespondentOrganizationNameNodeItem.Name == "ns1:OrganizationStandardName")
                                                                                                {
                                                                                                    lblCorrespondentAddress.Text += defendantCorrespondentOrganizationNameNodeItem.InnerText;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                if (defendantCorrespondentContactNodeItem.Name == "ns1:PostalAddressBag")
                                                                                {

                                                                                    XmlNodeList defendantPostalAddressBagNode = defendantCorrespondentContactNodeItem.ChildNodes;
                                                                                    foreach (XmlNode defendantPostalAddressBagNodeItem in defendantPostalAddressBagNode)
                                                                                    {
                                                                                        if (defendantPostalAddressBagNodeItem.Name == "ns1:PostalAddress")
                                                                                        {

                                                                                            XmlNodeList defendantPostalAddressNode = defendantPostalAddressBagNodeItem.ChildNodes;
                                                                                            foreach (XmlNode defendantPostalAddressNodeItem in defendantPostalAddressNode)
                                                                                            {
                                                                                                if (defendantPostalAddressNodeItem.Name == "ns1:PostalStructuredAddress")
                                                                                                {

                                                                                                    XmlNodeList defendantPostalStructuredAddressNode = defendantPostalAddressNodeItem.ChildNodes;
                                                                                                    foreach (XmlNode defendantPostalStructuredAddressNodeItem in defendantPostalStructuredAddressNode)
                                                                                                    {
                                                                                                        if (defendantPostalStructuredAddressNodeItem.Name == "ns1:AddressLineText")
                                                                                                        {
                                                                                                            lblCorrespondentAddress.Text += defendantPostalStructuredAddressNodeItem.InnerText;
                                                                                                        }
                                                                                                        if (defendantPostalStructuredAddressNodeItem.Name == "ns1:CityName")
                                                                                                        {
                                                                                                            lblCorrespondentAddress.Text += defendantPostalStructuredAddressNodeItem.InnerText;
                                                                                                        }
                                                                                                        if (defendantPostalStructuredAddressNodeItem.Name == "ns1:GeographicRegionName")
                                                                                                        {
                                                                                                            lblCorrespondentAddress.Text += defendantPostalStructuredAddressNodeItem.InnerText;
                                                                                                        }
                                                                                                        if (defendantPostalStructuredAddressNodeItem.Name == "ns1:PostalCode")
                                                                                                        {
                                                                                                            lblCorrespondentAddress.Text += defendantPostalStructuredAddressNodeItem.InnerText;
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            //
                                                                        }

                                                                    }
                                                                }
                                                                if (defendantNodeItem.Name == "ns2:AssociatedMarkBag")
                                                                {

                                                                    XmlNodeList defendantAssociatedMarkBagNode = defendantNodeItem.ChildNodes;
                                                                    foreach (XmlNode defendantAssociatedMarkBagNodeItem in defendantAssociatedMarkBagNode)
                                                                    {
                                                                        if (defendantAssociatedMarkBagNodeItem.Name == "ns2:AssociatedMark")
                                                                        {

                                                                            XmlNodeList defendantAssociatedMarkNode = defendantAssociatedMarkBagNodeItem.ChildNodes;
                                                                            foreach (XmlNode defendantAssociatedMarkNodeItem in defendantAssociatedMarkNode)
                                                                            {
                                                                                if (defendantAssociatedMarkNodeItem.Name == "ns2:WordMarkSpecification")
                                                                                {

                                                                                    XmlNodeList defendantWordMarkSpecificationNode = defendantAssociatedMarkNodeItem.ChildNodes;
                                                                                    foreach (XmlNode defendantWordMarkSpecificationNodeItem in defendantWordMarkSpecificationNode)
                                                                                    {
                                                                                        if (defendantWordMarkSpecificationNodeItem.Name == "ns2:MarkVerbalElementText")
                                                                                        {
                                                                                            lblMarkProceeding.Text = defendantWordMarkSpecificationNodeItem.InnerText;
                                                                                        }
                                                                                        if (defendantWordMarkSpecificationNodeItem.Name == "ns2:MarkStandardCharacterIndicator")
                                                                                        {
                                                                                            if (defendantWordMarkSpecificationNodeItem.InnerText == "false")
                                                                                                lblStandardCharacterClaim.Text = "No";
                                                                                            else
                                                                                                lblStandardCharacterClaim.Text = "Yes";
                                                                                        }
                                                                                    }
                                                                                }
                                                                                if (defendantAssociatedMarkNodeItem.Name == "ns2:NationalStatus")
                                                                                {

                                                                                    XmlNodeList defendantNationalStatusNode = defendantAssociatedMarkNodeItem.ChildNodes;
                                                                                    foreach (XmlNode defendantNationalStatusNodeItem in defendantNationalStatusNode)
                                                                                    {
                                                                                        if (defendantNationalStatusNodeItem.Name == "ns2:NationalStatusExternalDescriptionText")
                                                                                        {
                                                                                            lblApplicationStatus.Text = defendantNationalStatusNodeItem.InnerText;
                                                                                            lblSerialNumberProceeding.Text = SerialNo;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    //ns2:AssociatedMark
                                                                }

                                                            }
                                                        }

                                                        if (oppositionProceedingBagNodeItem.Name == "ns2:Plaintiff")
                                                        {

                                                            XmlNodeList PlaintiffNode = oppositionProceedingBagNodeItem.ChildNodes;
                                                            foreach (XmlNode PlaintiffNodeItem in PlaintiffNode)
                                                            {
                                                                if (PlaintiffNodeItem.Name == "ns2:Correspondent")
                                                                {
                                                                    XmlNodeList defendantCorrespondentNode = PlaintiffNodeItem.ChildNodes;
                                                                    foreach (XmlNode defendantCorrespondentNodeItem in defendantCorrespondentNode)
                                                                    {
                                                                        if (defendantCorrespondentNodeItem.Name == "ns1:Contact")
                                                                        {

                                                                            XmlNodeList defendantCorrespondentContactNode = defendantCorrespondentNodeItem.ChildNodes;
                                                                            foreach (XmlNode defendantCorrespondentContactNodeItem in defendantCorrespondentContactNode)
                                                                            {
                                                                                if (defendantCorrespondentContactNodeItem.Name == "ns1:Name")
                                                                                {

                                                                                    XmlNodeList defendantCorrespondentNameNode = defendantCorrespondentContactNodeItem.ChildNodes;
                                                                                    foreach (XmlNode defendantCorrespondentNameNodeItem in defendantCorrespondentNameNode)
                                                                                    {
                                                                                        if (defendantCorrespondentNameNodeItem.Name == "ns1:PersonName")

                                                                                        {

                                                                                            XmlNodeList defendantCorrespondentPersonNameNode = defendantCorrespondentNameNodeItem.ChildNodes;
                                                                                            foreach (XmlNode defendantCorrespondentPersonNameNodeItem in defendantCorrespondentPersonNameNode)
                                                                                            {
                                                                                                if (defendantCorrespondentPersonNameNodeItem.Name == "ns1:PersonFullName")
                                                                                                {
                                                                                                    lblCorrespondentAddressPlaintiff.Text += defendantCorrespondentPersonNameNodeItem.InnerText;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        if (defendantCorrespondentNameNodeItem.Name == "ns1:OrganizationName")
                                                                                        {
                                                                                            XmlNodeList defendantCorrespondentOrganizationNameNode = defendantCorrespondentNameNodeItem.ChildNodes;
                                                                                            foreach (XmlNode defendantCorrespondentOrganizationNameNodeItem in defendantCorrespondentOrganizationNameNode)
                                                                                            {
                                                                                                if (defendantCorrespondentOrganizationNameNodeItem.Name == "ns1:OrganizationStandardName")
                                                                                                {
                                                                                                    lblCorrespondentAddressPlaintiff.Text += defendantCorrespondentOrganizationNameNodeItem.InnerText;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                if (defendantCorrespondentContactNodeItem.Name == "ns1:PostalAddressBag")
                                                                                {

                                                                                    XmlNodeList defendantPostalAddressBagNode = defendantCorrespondentContactNodeItem.ChildNodes;
                                                                                    foreach (XmlNode defendantPostalAddressBagNodeItem in defendantPostalAddressBagNode)
                                                                                    {
                                                                                        if (defendantPostalAddressBagNodeItem.Name == "ns1:PostalAddress")
                                                                                        {

                                                                                            XmlNodeList defendantPostalAddressNode = defendantPostalAddressBagNodeItem.ChildNodes;
                                                                                            foreach (XmlNode defendantPostalAddressNodeItem in defendantPostalAddressNode)
                                                                                            {
                                                                                                if (defendantPostalAddressNodeItem.Name == "ns1:PostalStructuredAddress")
                                                                                                {

                                                                                                    XmlNodeList defendantPostalStructuredAddressNode = defendantPostalAddressNodeItem.ChildNodes;
                                                                                                    foreach (XmlNode defendantPostalStructuredAddressNodeItem in defendantPostalStructuredAddressNode)
                                                                                                    {
                                                                                                        if (defendantPostalStructuredAddressNodeItem.Name == "ns1:AddressLineText")
                                                                                                        {
                                                                                                            lblCorrespondentAddressPlaintiff.Text += defendantPostalStructuredAddressNodeItem.InnerText;
                                                                                                        }
                                                                                                        if (defendantPostalStructuredAddressNodeItem.Name == "ns1:CityName")
                                                                                                        {
                                                                                                            lblCorrespondentAddressPlaintiff.Text += defendantPostalStructuredAddressNodeItem.InnerText;
                                                                                                        }
                                                                                                        if (defendantPostalStructuredAddressNodeItem.Name == "ns1:GeographicRegionName")
                                                                                                        {
                                                                                                            lblCorrespondentAddressPlaintiff.Text += defendantPostalStructuredAddressNodeItem.InnerText;
                                                                                                        }
                                                                                                        if (defendantPostalStructuredAddressNodeItem.Name == "ns1:PostalCode")
                                                                                                        {
                                                                                                            lblCorrespondentAddressPlaintiff.Text += defendantPostalStructuredAddressNodeItem.InnerText;
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            //
                                                                        }

                                                                    }
                                                                }


                                                                if (PlaintiffNodeItem.Name == "ns1:Contact")
                                                                {

                                                                    XmlNodeList defendantContactNode = PlaintiffNodeItem.ChildNodes;
                                                                    foreach (XmlNode defendantContactNodeItem in defendantContactNode)
                                                                    {
                                                                        if (defendantContactNodeItem.Name == "ns1:Name")
                                                                        {

                                                                            XmlNodeList defendantContactNameNode = defendantContactNodeItem.ChildNodes;
                                                                            foreach (XmlNode defendantContactNameNodeItem in defendantContactNameNode)
                                                                            {
                                                                                if (defendantContactNameNodeItem.Name == "ns1:EntityName")
                                                                                {
                                                                                    lblNamePlaintiff.Text = defendantContactNameNodeItem.InnerText;
                                                                                }
                                                                            }
                                                                        }
                                                                    }


                                                                }
                                                                if (PlaintiffNodeItem.Name == "ns2:AssociatedMarkBag")
                                                                {
                                                                    XmlNodeList defendantAssociatedMarkBagNode = PlaintiffNodeItem.ChildNodes;
                                                                    foreach (XmlNode defendantAssociatedMarkBagNodeItem in defendantAssociatedMarkBagNode)
                                                                    {
                                                                        if (defendantAssociatedMarkBagNodeItem.Name == "ns2:AssociatedMark")
                                                                        {

                                                                            XmlNodeList defendantAssociatedMarkNode = defendantAssociatedMarkBagNodeItem.ChildNodes;
                                                                            foreach (XmlNode defendantAssociatedMarkNodeItem in defendantAssociatedMarkNode)
                                                                            {
                                                                                if (defendantAssociatedMarkNodeItem.Name == "ns2:WordMarkSpecification")
                                                                                {

                                                                                    XmlNodeList defendantWordMarkSpecificationNode = defendantAssociatedMarkNodeItem.ChildNodes;
                                                                                    foreach (XmlNode defendantWordMarkSpecificationNodeItem in defendantWordMarkSpecificationNode)
                                                                                    {
                                                                                        if (defendantWordMarkSpecificationNodeItem.Name == "ns2:MarkVerbalElementText")
                                                                                        {
                                                                                            lblPlaintiffMark.Text = defendantWordMarkSpecificationNodeItem.InnerText;
                                                                                        }
                                                                                    }
                                                                                }
                                                                                if (defendantAssociatedMarkNodeItem.Name == "ns2:NationalStatus")
                                                                                {

                                                                                    XmlNodeList defendantNationalStatusNode = defendantAssociatedMarkNodeItem.ChildNodes;
                                                                                    foreach (XmlNode defendantNationalStatusNodeItem in defendantNationalStatusNode)
                                                                                    {
                                                                                        if (defendantNationalStatusNodeItem.Name == "ns2:NationalStatusExternalDescriptionText")
                                                                                        {
                                                                                            lblPlaintiffApplicationStatus.Text = defendantNationalStatusNodeItem.InnerText;
                                                                                            lblPlaintiffSerialNumber.Text = SerialNo;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        StringBuilder sbProceeding = new StringBuilder();

                                                        sbProceeding.Append("<table border='1' cellpadding='0' cellspacing='0' class='table - striped'>");
                                                        sbProceeding.Append("<tr>");

                                                        sbProceeding.Append("<td>Description</td>");
                                                        // sbProceeding.Append("<td>Proceeding Number</td>");
                                                        sbProceeding.Append("<td>Date	</td>");

                                                        sbProceeding.Append("</tr>");
                                                        if (oppositionProceedingBagNodeItem.Name == "ns2:ProceedingEventBag")
                                                        {
                                                            XmlNodeList markEventBagNode = oppositionProceedingBagNodeItem.ChildNodes;
                                                            foreach (XmlNode markEventBagNodeItem in markEventBagNode)
                                                            {
                                                                sbProceeding.Append("<tr>");
                                                                if (markEventBagNodeItem.Name == "ns2:ProceedingEvent")
                                                                {
                                                                    XmlNodeList markEventNode = markEventBagNodeItem.ChildNodes;
                                                                    foreach (XmlNode markEventNodeItem in markEventNode)
                                                                    {


                                                                        //XmlNodeList MarkEventInnerNode = markEventNodeItem.ChildNodes;
                                                                        //foreach(XmlNode markEventNodeItem in markEventNode)
                                                                        //{
                                                                        if (markEventNodeItem.Name == "ns2:NationalMarkEvent")
                                                                        {
                                                                            //ns2:MarkEvent

                                                                            XmlNodeList nationalMarkEventNode = markEventNodeItem.ChildNodes;
                                                                            foreach (XmlNode nationalMarkEventNodeItem in nationalMarkEventNode)
                                                                            {
                                                                                if (nationalMarkEventNodeItem.Name == "ns2:MarkEventDescriptionText")
                                                                                {
                                                                                    sbProceeding.Append("<td>" + nationalMarkEventNodeItem.InnerText + "</td>");
                                                                                }
                                                                                if (nationalMarkEventNodeItem.Name == "ns2:MarkEventAdditionalText")
                                                                                {
                                                                                    sbProceeding.Append("<td>" + nationalMarkEventNodeItem.InnerText + "</td>");
                                                                                }
                                                                            }

                                                                        }
                                                                        if (markEventNodeItem.Name == "ns2:MarkEventDate")
                                                                        {
                                                                            sbProceeding.Append("<td>" + markEventNodeItem.InnerText + "</td>");
                                                                        }

                                                                    }

                                                                    sbProceeding.Append("</tr>");
                                                                }
                                                            }
                                                            sbProceeding.Append("</table>");
                                                            tblProceeding.InnerHtml = Convert.ToString(sbProceeding);
                                                        }
                                                    }
                                                }
                                            }
                                            // Bind dynamic table here for proceeding history
                                        }// end loop for proceeding


                                    }// end of assignment loop
                                }// end of Trade mark child node  foreach loop

                            }

                        } //end of node foreach loop
                    }//end of node if condition
                }
            }
        }
    }
}