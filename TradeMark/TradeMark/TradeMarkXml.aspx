<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="TradeMarkXml.aspx.cs" Inherits="TradeMark.TradeMarkXml" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    
    <div class="container">
    <div class="content">
                <!-- <div class="panel-heading">TradeMark Detail Page</div> -->
                <div class="panel-body">

                    <form role="form" name="TMDetail_form" runat="server">
                        <%-- <input type="button" value="Status" onclick="HideDocumentDiv('divStatus', 'divDocument')" />
                        <input type="button" value="Document" id="btnDocument" onclick="    HideStatusDiv('divStatus', 'divDocument')" />--%>
                        <div id="exTab2">
                            <ul class="nav nav-tabs">
                                <li class="active">
                                    <a href="#1" data-toggle="tab">Status</a>
                                </li>
                                <li><a href="#2" data-toggle="tab">Document</a>
                                </li>

                            </ul>

                            <%--<div id="divStatus" style="display: block">--%>
                            <div class="tab-content ">
                                <div class="tab-pane active" id="1">
                                    <p>

                                        <label class="control-label" for="Generatedon"><b>Generated on:</b></label>

                                        <asp:Label ID="lblGeneratedon" runat="server"></asp:Label>

                                    </p>
                                    <p> 
                                            <label class="control-label" for="Mark"><b>Mark:</b></label>
                                            <asp:Label ID="lblMark" runat="server"></asp:Label>
                                    </p>
                                    <p>
                                        
                                            <label class="control-label" for="ApplicationFilingDate"><b>Application Filing Date:</b></label>
                                       
                                            <asp:Label ID="lblApplicationFilingDate" runat="server"></asp:Label>
                                      
                                    </p>
                                    <p>
                                        <label class="control-label" for="USRegistrationNumber"><b>US Registration Number:</b></label>

                                        <asp:Label ID="lblUSRegistrationNumber" runat="server"></asp:Label>
                                    </p>

                                    <p>
                                        <label class="control-label" for="MarkType"><b>Mark Type:</b></label>

                                        <asp:Label ID="lblMarkType" runat="server"></asp:Label>
                                    </p>

                                    <p>
                                        <label class="control-label" for="Register"><b>Register:</b></label>

                                        <asp:Label ID="lblRegister" runat="server"></asp:Label>
                                    </p>
                                    <p>
                                        <label class="control-label" for="RegisterDate"><b>Registration Date:</b></label>

                                        <asp:Label ID="lblRegisterDate" runat="server"></asp:Label>
                                    </p>
                                    <p>
                                        <label class="control-label" for="US SerialNumber"><b>US Serial Number:</b></label>

                                        <asp:Label ID="lblSerialNumber" runat="server"></asp:Label>
                                    </p>
                                    <%--<div class="form-group">
                                        <label class="control-label" for="TM5CommonStatusDescriptor"><b>TM5 Common Status Descriptor:</b></label>

                                        <asp:Label ID="lblTM5CommonStatusDescriptor" runat="server"></asp:Label>
                                    </div>--%>

                                    <p>
                                        <label class="control-label" for="Status"><b>Status:</b></label>

                                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                    </p>
                                    <p>
                                        <label class="control-label" for="StatusDate"><b>Status Date:	</b></label>

                                        <asp:Label ID="lblStatusDate" runat="server"></asp:Label>
                                    </p>
                                    <p>
                                        <label class="control-label" for="PublicationDate"><b>Publication Date:</b></label>

                                        <asp:Label ID="lblPublicationDate" runat="server"></asp:Label>
                                    </p>
                                    <p>
                                        <label class="control-label" for="NoticeofAllowanceDate"><b>Notice of Allowance Date:</b></label>

                                        <asp:Label ID="lblNoticeofAllowanceDate" runat="server"></asp:Label>
                                    </p>

                                    <%--   -----------------%>
                                    <div class="panel-group page-penal" id="accordion">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseMarkInformation"><b>Mark Information</b></a>
                                                </h4>
                                            </div>
                                            <div id="collapseMarkInformation" class="panel-collapse collapse panel-body in">
                                                
                                                    <p>
                                                        <label class="control-label" for="MarkLiteralElements"><b>Mark Literal Elements:</b></label>

                                                        <asp:Label ID="lblMarkLiteralElements" runat="server"></asp:Label>
                                                    </p>
                                                    <p>
                                                        <label class="control-label" for="StandardCharacterClaim"><b>Standard Character Claim:</b></label>

                                                        <asp:Label ID="lblStandardCharacterClaim" runat="server"></asp:Label>
                                                    </p>
                                                    <p>
                                                        <label class="control-label" for="MarkDrawingType"><b>Mark Drawing Type:</b></label>

                                                        <asp:Label ID="lblMarkDrawingType" runat="server"></asp:Label>
                                                    </p>
                                                   <p>
                                                        <label class="control-label" for="DesignSearch"><b>Design Search Code(s):</b></label>
                                                       <div id="divDesignSearch" runat="server"></div>
                                                       <%-- <asp:Label ID="lblDesignSearch" runat="server"></asp:Label>--%>
                                                    </p>
                                                
                                            </div>
                                        </div>
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseGoodsandServices"><b>Goods and Services</b></a>
                                                </h4>
                                            </div>
                                            <div id="collapseGoodsandServices" class="panel-collapse collapse panel-body">
                                                <p>
                                                    <label class="control-label" for="For"><b>For:</b></label>

                                                    <asp:Label ID="lblFor" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="InternationalClasses"><b>International Class(es):</b></label>

                                                    <asp:Label ID="lblInternationalClasses" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="USClasses"><b>U.S Class(es):</b></label>

                                                    <asp:Label ID="lblUSClasses" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="ClassStatus"><b>Class Status:</b></label>

                                                    <asp:Label ID="lblClassStatus" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="Basis"><b>Basis:</b></label>

                                                    <asp:Label ID="lblBasis" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="FirstUse"><b>First Use:</b></label>

                                                    <asp:Label ID="lblFirstUse" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="UseinCommerce"><b>Use in Commerce:</b></label>

                                                    <asp:Label ID="lblUseinCommerce" runat="server"></asp:Label>
                                                </p>
                                            </div>
                                        </div>

                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseasisInformation"><b>Basis Information (Case Level)</b></a>
                                                </h4>
                                            </div>
                                            <div id="collapseasisInformation" class="panel-collapse collapse panel-body">
                                               
                                                  <p>
                                                    
                                                    <label class="control-label" for="OwnerName"><b>Filed Use:	</b></label>

                                                    <asp:Label ID="lblFilingBasisFiledUse" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="OwnerAddress"><b>Filed ITU:</b></label>

                                                    <asp:Label ID="lblFilingBasisFiledITU" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="LegalEntityType"><b>Filed 44D:</b></label>

                                                    <asp:Label ID="lblFilingBasisFiled44D" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="StateorCountryWhereOrganized"><b>Filed 44E:</b></label>

                                                    <asp:Label ID="lblFilingBasisFiled44E" runat="server"></asp:Label>
                                                </p>
                                                 <p>
                                                    <label class="control-label" for="LegalEntityType"><b>Filed 66A:</b></label>

                                                    <asp:Label ID="lblFilingBasisFiled66A" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="StateorCountryWhereOrganized"><b>Filed No Basis:</b></label>

                                                    <asp:Label ID="lblFilingBasisFiledNoBasis" runat="server"></asp:Label>
                                                    </p>
                                                      <%------------------------------------------------------------------------------%>
                                                     <p>
                                                    <label class="control-label" for="OwnerName"><b>Currently Use:	</b></label>

                                                    <asp:Label ID="lblCurrentlyUse" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="OwnerAddress"><b>Currently ITU:</b></label>

                                                    <asp:Label ID="lblCurrentlyITU" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="LegalEntityType"><b>Currently 44D:</b></label>

                                                    <asp:Label ID="lblCurrently44D" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="StateorCountryWhereOrganized"><b>Currently 44E:	</b></label>

                                                    <asp:Label ID="lblCurrently44E" runat="server"></asp:Label>
                                                </p>
                                                 <p>
                                                    <label class="control-label" for="LegalEntityType"><b>Currently 66A:</b></label>

                                                    <asp:Label ID="lblCurrently66A" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="StateorCountryWhereOrganized"><b>Currently No Basis:</b></label>

                                                    <asp:Label ID="lblCurrentlyNoBasis" runat="server"></asp:Label>

                                                </p>

                                                 <%------------------------------------------------------------------------------%>
                                               <p>
                                                    <label class="control-label" for="OwnerName"><b>Amended Use:	</b></label>

                                                    <asp:Label ID="lblAmendedUse" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="OwnerAddress"><b>Amended ITU:</b></label>

                                                    <asp:Label ID="lblAmendedITU" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="LegalEntityType"><b>Amended 44D:</b></label>

                                                    <asp:Label ID="lblAmended44D" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="StateorCountryWhereOrganized"><b>Amended 44E:</b></label>

                                                    <asp:Label ID="lblAmended44E" runat="server"></asp:Label>
                                                </p>
                                           
                                        </div>

                                        </div>
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseCurrentOwner"><b>Current Owner(s) Information</b></a>
                                                </h4>
                                            </div>
                                            <div id="collapseCurrentOwner" class="panel-collapse collapse panel-body">
                                                <p>
                                                    <label class="control-label" for="OwnerName"><b>Owner Name:	</b></label>

                                                    <asp:Label ID="lblOwnerName" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="OwnerAddress"><b>Owner Address:</b></label>

                                                    <asp:Label ID="lblOwnerAddress" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="LegalEntityType"><b>Legal Entity Type:</b></label>

                                                    <asp:Label ID="lblLegalEntityType" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="StateorCountryWhereOrganized"><b>State or Country Where Organized:</b></label>

                                                    <asp:Label ID="lblStateorCountryWhereOrganized" runat="server"></asp:Label>
                                                </p>
                                            </div>
                                        </div>


                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseCorrespondenceInformation"><b>Attorney/Correspondence Information</b></a>
                                                </h4>
                                            </div>
                                            <div id="collapseCorrespondenceInformation" class="panel-collapse collapse panel-body">
                                                <p>
                                                    <label class="control-label" for="AttorneyName"><b>Attorney Name:</b></label>

                                                    <asp:Label ID="lblAttorneyName" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="DocketNumber"><b>Docket Number:</b></label>

                                                    <asp:Label ID="lblDocketNumber" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="Correspondent "><b>Correspondent:</b></label>

                                                    <asp:Label ID="lblCorrespondent" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="nameaddress"><b>Name/Address:</b></label>

                                                    <asp:Label ID="lblNameAddress" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="phone"><b>Phone:</b></label>

                                                    <asp:Label ID="lblPhone" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="Fax"><b>Fax:</b></label>

                                                    <asp:Label ID="lblFax" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="correspondentemail"><b>Correspondent E-mail:</b></label>

                                                    <asp:Label ID="lblCorrespondntEmail" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <label class="control-label" for="correspondentemailauthorized:"><b>Correspondent E-mail Authorized:</b></label>

                                                    <asp:Label ID="lblCorrespondntEmailAuthorized" runat="server"></asp:Label>
                                                </p>
                                            </div>
                                        </div>

                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseProsecutionHistory"><b>Prosecution History</b></a>
                                                </h4>
                                            </div>
                                            <div id="collapseProsecutionHistory" class="panel-collapse collapse panel-body">
                                                <div id="Grd1" class="table-responsive" runat="server"></div>
                                            </div>
                                        </div>
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseMaintenanceFilings"><b>Maintenance Filings or Post Registration Information</b></a>
                                                </h4>
                                            </div>
                                            <div id="collapseMaintenanceFilings" class="panel-collapse collapse panel-body">

                                                    <p>
                                                        <label class="control-label" for="AffidavitofContinuedUse"><b>Affidavit of Continued Use:</b></label>

                                                        <asp:Label ID="lblAffidavitofContinuedUse" runat="server"></asp:Label>
                                                    </p>
                                                    <p>
                                                        <label class="control-label" for="AffidavitofIncontestability"><b>Affidavit of Incontestability:</b></label>

                                                        <asp:Label ID="lblAffidavitofIncontestability" runat="server"></asp:Label>
                                                    </p>
                                                    <p>
                                                        <label class="control-label" for="RenewalDate"><b>Renewal Date:</b></label>

                                                        <asp:Label ID="lblRenewalDate" runat="server"></asp:Label>
                                                    </p>

                                            </div>
                                        </div>
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseTMStaff"><b>TM Staff and Location Information</b></a>
                                                </h4>
                                            </div>
                                            <div id="collapseTMStaff" class="panel-collapse collapse panel-body">
                                     
                                                    <h3>TM Staff Information</h3>
                                                    <p>
                                                        <label class="control-label" for="TMAttorney"><b>TM Attorney:</b></label>

                                                        <asp:Label ID="lblTMAttorney" runat="server"></asp:Label>
                                                    </p>
                                                    <p>
                                                        <label class="control-label" for="LawOfficeAssigned"><b>Law Office Assigned:</b></label>

                                                        <asp:Label ID="lblLawOfficeAssigned" runat="server"></asp:Label>
                                                    </p>
                                                    <h3>File Location</h3>
                                                    <p>
                                                        <label class="control-label" for="CurrentLocation"><b>Current Location</b></label>

                                                        <asp:Label ID="lblCurrentLocation" runat="server"></asp:Label>
                                                    </p>
                                                    <p>
                                                        <label class="control-label" for="DateinLocation"><b>Date in Location:</b></label>

                                                        <asp:Label ID="lblDateinLocation" runat="server"></asp:Label>
                                                    </p>
                                               
                                            </div>
                                        </div>
                                         <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseAssignmentAbstract"><b>Assignment Abstract Of Title Information</b></a>
                                                </h4>
                                            </div>
                                            <div id="collapseAssignmentAbstract" class="panel-collapse collapse panel-body">
                                        
                                                  <table class="table-striped">
                                                        <tr>
                                                            <td id="divAssignmentAbstract" class="table-striped" runat="server"></td>
                                                        </tr>
                                                    </table>
                                                
                                            </div>
                                        </div>
                                          <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseProceedings"><b>Proceedings</b></a>
                                                </h4>
                                            </div>
                                            <div id="collapseProceedings" class="panel-collapse collapse panel-body">
                                            
                                                    <h3>Type of proceeding: Opposition</h3>
                                                   <p>
                                                        <label class="control-label" for="ProceedingNumber"><b>Proceeding Number:</b></label>

                                                        <asp:Label ID="lblProceedingNumber" runat="server"></asp:Label>
                                                    </p>
                                                    <p>
                                                        <label class="control-label" for="Status"><b>Status:</b></label>

                                                        <asp:Label ID="lblStatusProceeding" runat="server"></asp:Label>
                                                    </p>
                                                   
                                                    <p>
                                                        <label class="control-label" for="FilingDate"><b>Filing Date:</b></label>

                                                        <asp:Label ID="lblFilingDate" runat="server"></asp:Label>
                                                    </p>
                                                    <p>
                                                        <label class="control-label" for="StatusDate"><b>Status Date:</b></label>

                                                        <asp:Label ID="lblStatusDateProceeding" runat="server"></asp:Label>
                                                    </p>

                                                     <h4>Defendant</h4>
                                                   <p>
                                                        <label class="control-label" for="Name"><b>Name:</b></label>

                                                        <asp:Label ID="lblName" runat="server"></asp:Label>
                                                    </p>
                                                    <p>
                                                        <label class="control-label" for="CorrespondentAddress"><b>Correspondent Address:</b></label>

                                                        <asp:Label ID="lblCorrespondentAddress" runat="server"></asp:Label>
                                                    </p>
                                                   <h4>Associated marks</h4>
                                                    <p>
                                                        <label class="control-label" for="Mark"><b>Mark:</b></label>

                                                        <asp:Label ID="lblMarkProceeding" runat="server"></asp:Label>
                                                    </p>
                                                    <p>
                                                        <label class="control-label" for="ApplicationStatus"><b>Application Status:</b></label>

                                                        <asp:Label ID="lblApplicationStatus" runat="server"></asp:Label>
                                                    </p>
                                                    <p>
                                                        <label class="control-label" for="SerialNumber"><b>Serial Number:</b></label>

                                                        <asp:Label ID="lblSerialNumberProceeding" runat="server"></asp:Label>
                                                    </p>
                                                    <p>
                                                        <label class="control-label" for="RegistrationNumber"><b>Registration Number:</b></label>

                                                        <asp:Label ID="lblRegistrationNumber" runat="server"></asp:Label>
                                                    </p>
                                                   <h3>Plaintiff(s)</h3>
                                                    <p>
                                                        <label class="control-label" for="SerialNumber"><b>Name:</b></label>

                                                        <asp:Label ID="lblNamePlaintiff" runat="server"></asp:Label>
                                                    </p>
                                                    <p>
                                                        <label class="control-label" for="CorrespondentAddress"><b>Correspondent Address:</b></label>

                                                        <asp:Label ID="lblCorrespondentAddressPlaintiff" runat="server"></asp:Label>
                                                    </p>
                                                     <h4>Associated marks</h4>
                                                    <p>
                                                        <label class="control-label" for="Mark"><b>Mark:</b></label>

                                                        <asp:Label ID="lblPlaintiffMark" runat="server"></asp:Label>
                                                    </p>
                                                    <p>
                                                        <label class="control-label" for="ApplicationStatus"><b>Application Status:</b></label>

                                                        <asp:Label ID="lblPlaintiffApplicationStatus" runat="server"></asp:Label>
                                                    </p>
                                                    <p>
                                                        <label class="control-label" for="SerialNumber"><b>Serial Number:</b></label>

                                                        <asp:Label ID="lblPlaintiffSerialNumber" runat="server"></asp:Label>
                                                    </p>
                                                    <p>
                                                        <label class="control-label" for="RegistrationNumber"><b>Registration Number:</b></label>

                                                        <asp:Label ID="lblPlaintiffRegistrationNumber" runat="server"></asp:Label>
                                                    </p>
                                                  <table class="table-striped">
                                                        <tr>
                                                            <td id="tblProceeding" class="table-striped" runat="server"></td>
                                                        </tr>
                                                    </table>
                                         
                                            </div>
                                        </div>
                                    </div>
                            </div>

                            <%--  <div id="divDocument" style="display: none">--%>
                            <div class="tab-pane" id="2">
                                <span style="color: red"><a href="https://tsdrapi.uspto.gov/ts/cd/casedocs/bundle.pdf?rn=<%= Request.QueryString["Rn"].ToString()%>&category=RC" target="_blank">For document please click here</a></span>

                            </div>
                        </div>
                </div>
                </form>
            </div>
        </div>

    </div>
</asp:Content>
