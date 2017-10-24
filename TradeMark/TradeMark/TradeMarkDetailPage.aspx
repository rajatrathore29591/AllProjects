<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="TradeMarkDetailPage.aspx.cs" Inherits="TradeMark.TradeMarkDetailPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <!-- <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
      <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" /> -->
   <!-- <link href="Content/Site.css" rel="stylesheet" />
      <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
      <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
      <style type="text/css">
          .bs-example {
              margin: 20px;
          }
          #tooltip {
      background: #eee;
      border: 1px solid #bbb;
      padding: 5px;
      position: absolute;
      }
         
      </style> -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <!--TradeMark Detail Page Content Start-->
   <div class="container">
      <div class="row">
         <div class="col-md-12">
            <div class="heading">
               <h2><span>Trademark Detail Page</span></h2>
            </div>
            <!--Panel Shadow Box Start-->
            <div class="panel-shadow-box">
               <!--Panel Heading Start-->
               <%--<div class="panel-heading">
                  <h3>Disclaimer</h3>
                  </div>--%>
               <!--Panel Heading End-->
               <!--Panel Body End-->
               <div class="panel-body-content trademarkdetail">
                  <div class="content-info">
                     <form>
                        <a href="https://tsdrapi.uspto.gov/ts/cd/casestatus/sn<%= Request.QueryString["Sn"].ToString()%>/download.pdf" title="Download Status Pdf" class="btn btn-danger btn-sm pull-right btn-pdf"><i class="fa fa-file-pdf-o"></i></a>
                        <ul class="nav nav-tabs">
                           <li class="active"><a data-toggle="tab" href="#Status">Status</a></li>
                           <li><a data-toggle="tab" href="#Document">Document</a></li>
                        </ul>
                        <div class="tab-content ">
                           <div class="tab-pane active" id="Status">
                              <div class="form-group">
                                 <div id="divsummary" runat="server" class="sectionContainer table-responsive"></div>
                              </div>
                              <div class="panel-group" id="accordion">
                                 <div class="panel panel-default">
                                    <div class="panel-heading">
                                       <h4 class="panel-title">
                                          <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseMarkInformation"><b>Mark Information</b></a>
                                       </h4>
                                    </div>
                                    <div id="collapseMarkInformation" class="panel-collapse collapse">
                                       <div class="panel-body">
                                          <div class="form-group">
                                             <div id="divMarkInfo" runat="server" class="sectionContainer table-responsive "></div>
                                          </div>
                                       </div>
                                    </div>
                                 </div>
                                 <%--For goods and services--%>
                                 <div class="panel panel-default">
                                    <div class="panel-heading">
                                       <h4 class="panel-title">
                                          <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseGoodsandServices"><b>Goods and Services</b></a>
                                       </h4>
                                    </div>
                                    <div id="collapseGoodsandServices" class="panel-collapse collapse">
                                       <div class="panel-body">
                                          <div id="divGoodsandServices" runat="server" class="sectionContainer table-responsive "></div>
                                       </div>
                                    </div>
                                 </div>
                                 <div class="panel panel-default">
                                    <div class="panel-heading">
                                       <h4 class="panel-title">
                                          <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseBasisInformation"><b>Basis Information (Case Level)</b></a>
                                       </h4>
                                    </div>
                                    <div id="collapseBasisInformation" class="panel-collapse collapse">
                                       <div class="panel-body">
                                          <div id="divBasisInformation" runat="server" class="sectionContainer table-responsive "></div>
                                       </div>
                                    </div>
                                 </div>
                                 <%--for current owner--%>
                                 <div class="panel panel-default">
                                    <div class="panel-heading">
                                       <h4 class="panel-title">
                                          <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseCurrentOwner"><b>Current Owner(s) Information</b></a>
                                       </h4>
                                    </div>
                                    <div id="collapseCurrentOwner" class="panel-collapse collapse">
                                       <div class="panel-body">
                                          <div id="divCurrentOwner" runat="server" class="sectionContainer table-responsive "></div>
                                       </div>
                                    </div>
                                 </div>
                                 <%-- for Attorney/Correspondence Information--%>
                                 <div class="panel panel-default">
                                    <div class="panel-heading">
                                       <h4 class="panel-title">
                                          <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseCorrespondenceInformation"><b>Attorney/Correspondence Information</b></a>
                                       </h4>
                                    </div>
                                    <div id="collapseCorrespondenceInformation" class="panel-collapse collapse">
                                       <div class="panel-body">
                                          <div id="divCorrespondenceInformation" runat="server" class="sectionContainer table-responsive "></div>
                                       </div>
                                    </div>
                                 </div>
                                 <%--for Prosecution History--%>
                                 <div class="panel panel-default">
                                    <div class="panel-heading">
                                       <h4 class="panel-title">
                                          <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseProsecutionHistory"><b>Prosecution History</b></a>
                                       </h4>
                                    </div>
                                    <div id="collapseProsecutionHistory" class="panel-collapse collapse">
                                       <div class="panel-body">
                                          <div id="divProsecutionHistory" runat="server" class="sectionContainer table-responsive "></div>
                                       </div>
                                    </div>
                                 </div>
                                 <%-- for Maintenance Filings or Post Registration Information--%>
                                 <div class="panel panel-default">
                                    <div class="panel-heading">
                                       <h4 class="panel-title">
                                          <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseMaintenanceFilings"><b>Maintenance Filings or Post Registration Information</b></a>
                                       </h4>
                                    </div>
                                    <div id="collapseMaintenanceFilings" class="panel-collapse collapse">
                                       <div class="panel-body">
                                          <div id="divMaintenanceFilings" runat="server" class="sectionContainer table-responsive "></div>
                                       </div>
                                    </div>
                                 </div>
                                 <%--TM Staff and Location Information--%>
                                 <div class="panel panel-default">
                                    <div class="panel-heading">
                                       <h4 class="panel-title">
                                          <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseTMStaff"><b>TM Staff and Location Information</b></a>
                                       </h4>
                                    </div>
                                    <div id="collapseTMStaff" class="panel-collapse collapse">
                                       <div class="panel-body">
                                          <div id="divTMStaff" runat="server" class="sectionContainer table-responsive "></div>
                                       </div>
                                    </div>
                                 </div>
                                 <%-- for Assignment Abstract Of Title Information--%>
                                 <div class="panel panel-default">
                                    <div class="panel-heading">
                                       <h4 class="panel-title">
                                          <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseAssignment"><b>Assignment Abstract Of Title Information</b></a>
                                       </h4>
                                    </div>
                                    <div id="collapseAssignment" class="panel-collapse collapse">
                                       <div class="panel-body">
                                          <div id="divAssignment" runat="server" class="sectionContainer table-responsive "></div>
                                       </div>
                                    </div>
                                 </div>
                                 <%--  for proceeding--%>
                                 <div class="panel panel-default">
                                    <div class="panel-heading">
                                       <h4 class="panel-title">
                                          <a data-toggle="collapse" class="accordion-toggle" data-parent="#accordion" href="#collapseproceeding"><b>Proceeding</b></a>
                                       </h4>
                                    </div>
                                    <div id="collapseproceeding" class="panel-collapse collapse">
                                       <div class="panel-body">
                                          <div id="divProceeding" runat="server" class="sectionContainer table-responsive "></div>
                                       </div>
                                    </div>
                                 </div>
                              </div>
                           </div>
                           <div class="tab-pane txt-center" id="Document">
                              <div class="bs-example">
                                 <span><a href="https://tsdrapi.uspto.gov/ts/cd/casedocs/bundle.pdf?rn=<%= Request.QueryString["Rn"].ToString()%>&category=RC" target="_blank">Download Registration Certificate</a></span>
                              </div>
                           </div>
                           <div class="tab-pane txt-center" id="Download">
                              <div class="bs-example">
                                 <span><a href="https://tsdrapi.uspto.gov/ts/cd/casestatus/sn<%= Request.QueryString["Sn"].ToString()%>/download.pdf" target="_blank">For Download please click here</a></span>
                              </div>
                           </div>
                        </div>
                     </form>
                  </div>
               </div>
               <!--Panel Body End-->
            </div>
            <!--Panel Shadow Box End-->
         </div>
      </div>
   </div>
   <!--Disclimer Content End-->
</asp:Content>