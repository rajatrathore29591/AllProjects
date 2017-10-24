<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="SearchDemo.aspx.cs" Inherits="TradeMark.SearchDemo" ValidateRequest="false" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">



    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>

    <script src="Scripts/Search.js"></script>
    <link href="http://code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" rel="Stylesheet" />

    <script>
        $(document).ready(function () {

            var status = true;
            $(window).keydown(function (event) {

            });

        })
        //function to set the Radio button value in Hidden
        function setRadioValue()
        {
            $("#ContentPlaceHolder1_hdntalktoAttoneycheck").val("2");
            if (document.getElementById('rdokHelp').checked) {
                $("#ContentPlaceHolder1_hdntalktoAttoneycheck").val("1");
            }
            
            if (document.getElementById('rdoAssistance').checked) {
                $("#ContentPlaceHolder1_hdntalktoAttoneycheck").val("2");
            }
            if (document.getElementById('rdoBoth').checked) {
                $("#ContentPlaceHolder1_hdntalktoAttoneycheck").val("3");
            }
            //alert($("#ContentPlaceHolder1_hdntalktoAttoneycheck").val());
            $('#TalktoanAttorneyModal').modal('hide');
            return true;
        }

    </script>

</asp:Content>
<asp:Content ID="Contentbody" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <section class="inner-content">
        <div class="container">
            <div class="row">
                <div class="col-md-12">



                    <%if (importInporocess)
                        { %>
                    <div class="lbl-red alert alert-danger" id="divUpdateMsg"><strong>Database is updating…</strong></div>
                    <%}
                        else
                        { %>

                    <div id="divDateMsg" class="alert alert-success m-b0 m-t20 text-center">Database updated through <%=latestFileImportDate %></div>
                    <%} %>

                    <div class="heading">
                        <h2><span>Search</span></h2>
                    </div>
                    <div class="panel-shadow-box">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-md-6">
                                    <span class="notify">All fields with a * are required. </span>
                                </div>
                                <div class="col-md-6 text-right"><h2><i class="fa fa-lightbulb-o" aria-hidden="true"></i><a href="#"  data-toggle="modal" data-target="#searchmodal">SEARCH TIPS</a></h2></div>
                            </div>
                            
                        </div>
                        <div class="row clearfix" id="divSuccess" style="display: none;">
                                    <div class="col-md-12">
                                        <div class="alert alert-success">
                                            <asp:Label ID="lblSuccessmsg" runat="server" CssClass="successmsg"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                        <!-- .panel-body -->
                        <div class="panel-body-content search-content">


                            <form role="form" name="add_factory_form" runat="server">
                                <div class="field-box search-fields">

                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btndefault">
                                        <div class="row">
                                            <asp:HiddenField ID="hdnSearchoption" Value="0" runat="server" />
                                            <asp:HiddenField ID="hdnSearchCount" Value="0" runat="server" />
                                            <asp:HiddenField ID="hdnSearchRslt" Value="false" runat="server" />
                                            <div class="col-md-6 col-sm-6">
                                                <div class="form-group ">
                                                    <label>Enter Mark<span class="text-danger">*</span>:</label>
                                                    <asp:Button ID="btnCheck" Style="display: none" runat="server" OnClientClick=" TestFunction()" />
                                                    <asp:TextBox ID="txtMark" runat="server" class="form-control"></asp:TextBox>
                                                    <span id="spnEnterMark" class="spanstyle"></span>
                                                    <span class="lbl-red" id="spnTMtxtError" style="display: none;">Required!</span>


                                                    <div class="m-t10" style="display: none;" id="divComponentFullForm">
                                                        <asp:TextBox ID="txtComponentFullForm" runat="server" class="form-control"></asp:TextBox>
                                                        <span id="spnComponentFullForm" class="spanstyle"></span>
                                                    </div>
                                                    <div class="m-t10" style="display: none;" id="divHybride">
                                                    <asp:TextBox ID="txtFullForm" runat="server" class="form-control"></asp:TextBox>
                                                    <span id="spnFullForm" class="spanstyle"></span>
                                                </div>
                                                </div>
                                                <div class="form-group ">
                                                    <label>Enter Goods or Services Description<span class="text-danger">*</span>:</label>
                                                    <div id="divGoodServices">
                                                        <%--  <asp:DropDownList ID="ddlGoodsServices" runat="server" data-placeholder="Choose a Application type..." class="form-control  chosen-select" onclick="">
                                            </asp:DropDownList>--%>

                                                        <input id="autoGoodsData" placeholder="Enter Goods or Services" class="float-left form-control input-12 text-box single-line ui-autocomplete-input valid" type="text" value="" title="Enter Goods & Services name" name="autoGoodsData" autocomplete="off" />
                                                        <asp:HiddenField ID="hdnGoodsID" runat="server" />
                                                        <asp:HiddenField ID="hdnGoodsName" runat="server" />
                                                        <asp:HiddenField ID="hdnSearchGuid" runat="server" Value="" />
                                                        <span class="lbl-red" id="spnGoodsDatatxtError" style="display: none;">Required!</span>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="col-md-6 col-sm-6">
                                                <div class="form-fgroup chkStyle">

                                                    <div class="common-checkbox-item">
                                                        <asp:CheckBoxList ID="CheckBoxList1" runat="server">
                                                            <asp:ListItem Text="Enter Mark" Value="1" onClick="MutExChkList(this);" style="display: none;" />
                                                            <asp:ListItem Text="Check the box if the mark you entered has one or more components." Value="2" onclick="MutExChkList(this);" />
                                                            <asp:ListItem Text="Check the box if the mark you entered is an abbreviation, nickname, or acronym." Value="3" onclick="MutExChkList(this);" />
                                                            <asp:ListItem Text="Check the box if the mark you entered has one or more components and one or more of the components is an abbreviation, nickname, or acronym." Value="4" onclick="MutExChkList(this);" />

                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="button-block text-right col-sm-12 col-xs-12">
                                                <asp:Button ID="btndefault" Text="Search" runat="server" Class="btn btn-primary btn-big m-t10" OnClientClick="return ValidateTMtext();" title="Search" />
                                                <button type="button" id="btnOpenalertModel" class="btn btn-primary" data-toggle="modal" data-target="#myAlertModal" style="display: none;">Search</button>
                                                <button type="button" id="btnOpenModel" class="btn btn-primary" data-toggle="modal" data-target="#myModal" style="display: none;">Save Search Result</button>
                                                <button type="button" id="btnOpenAttorneytModel" class="btn btn-primary" data-toggle="modal" data-target="#TalktoanAttorneyModal" style="display: none;">Attorney</button>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <asp:HiddenField ID="hdnRdoOption" Value="1" runat="server" />
                                <div id="myModal" class="modal fade" role="dialog">
                                    <div class="modal-dialog">

                                        <!-- Modal content Save search-->
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                                <h4 class="modal-title">Save search</h4>
                                            </div>

                                            <div class="modal-body">

                                                <span id="spnMsg" style="display: none;" class="successmsg">Your search is saved</span><br />
                                                <label>Title:<span class="error">*</span></label>

                                                <asp:TextBox ID="txtTitle" runat="server" class="form-control" MaxLength="150"></asp:TextBox>
                                                <asp:Label ID="lblTitle" runat="server"></asp:Label>

                                            </div>

                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                                <button type="button" id="btnSaveSearch" class="btn btn-default" onclick="SaveSearchData()">Save</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                 <div id="TalktoanAttorneyModal" class="modal fade" role="dialog">
                                    <div class="modal-dialog">

                                        <!-- Modal content Talk to an attorney-->
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                                <h4 class="modal-title">Do you need!</h4>
                                            </div>

                                            <div class="modal-body">
                                                
                                                <div class="common-checkbox">
                                                    <input id="rdokHelp" class="checkbox-custom"  name="checkbox-1" checked type="radio" />
                                                    <label for="rdokHelp" class="checkbox-custom-label">Help reviewing the search results</label>
                                                </div>
                                                <div class="common-checkbox">
                                                    <input id="rdoAssistance" class="checkbox-custom" name="checkbox-1" type="radio" />
                                                    <label for="rdoAssistance" class="checkbox-custom-label">Assistance with the filing of a trademark application</label>
                                                </div>
                                                  <div class="common-checkbox">
                                                    <input id="rdoBoth" class="checkbox-custom" name="checkbox-1" type="radio" />
                                                    <label for="rdoBoth" class="checkbox-custom-label">Both</label>
                                                </div>

                                            </div>

                                            <div class="modal-footer">
                                                <%--<button title="Send" type="button" class="btn btn-primary btn-small" data-dismiss="modal" onclick="return showpopup();">Send</button>--%>
                                                <asp:Button title="Send" ID="btnSendEmail" Text="Send" runat="server" class="btn btn-default btn-small"  OnClientClick="return setRadioValue();" OnClick="btnSendEmail_Click" />

                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div id="myAlertModal" class="modal fade" role="dialog">
                                    <div class="modal-dialog">

                                        <!-- Modal content-->
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                                <h4 class="modal-title">Alert!</h4>
                                            </div>

                                            <div class="modal-body">
                                                <p>Does the new mark you entered have components or is it an acronym, nickname, or abbreviation? If so, click yes and check the appropriate box. If no, click no to proceed with your new search.</p>
                                                <div class="common-checkbox">
                                                    <input id="checkbox" class="checkbox-custom" name="checkbox-1" type="checkbox" />
                                                    <label for="checkbox" class="checkbox-custom-label">Don’t show this alert again.</label>
                                                </div>


                                            </div>

                                            <div class="modal-footer">
                                                <button title="Yes" type="button" class="btn btn-primary btn-small" data-dismiss="modal">Yes</button>
                                                <asp:Button title="No" ID="btnSearch" Text="No" runat="server" class="btn btn-default btn-small" OnClientClick="showloader('show');" OnClick="btnSearch_Click" />

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="loader" id="divloader" style="display: none;">
                                    <img src="Images/loader.gif" alt="Loading" />
                                </div>
                                <div id="divSearchResult" runat="server">
                                </div>
                                <div runat="server" id="divResultDuplicate">
                                </div>
                                <div class="button-block text-right col-sm-12 col-xs-12 p-r0">

                                    <asp:Button ID="btnsearchsave" Text=" " runat="server" Visible="false" class="btn btn-success btn-mrn btn-save btn-icons" title="Save Search Result" OnClientClick="return ValidateTMtextSaveSearch();" />
                                    <asp:Button ID="btnSavePdf" Text=" " runat="server" Visible="false" class="btn btn-danger btn-mrn btn-pdf-o btn-icons" title="View Result in PDF" OnClick="btnSavePdf_Click" />
                                    <%--<asp:Button ID="btnSavedoc" Text=" " runat="server" Visible="false" class="btn btn-primary btn-mrn btn-doc btn-icons" title="View Result in Word Doc" OnClientClick="funSetDocContent(); downloadDoc()" />--%>
                                    <input type="button" id="btnSavedoc" runat="server" visible="false" class="btn btn-primary btn-mrn btn-doc btn-icons" title="View Result in Word Doc" onclick="generateFinalHtmlToExport(); downloadDoc(); " />
                                    <asp:Button ID="btnTalkToAttorney" Text="Talk to an Attorney" runat="server" Visible="false" class="btn btn-primary btn-mrn btn-email-o btn-icons" title="Talk to an Attorney" OnClientClick="$('#btnOpenAttorneytModel').click(); generateFinalHtmlToExport(); return false;"/>

                                </div>
                                 <%--<div class="row clearfix" id="divSuccess" style="display: none;">
                                    <div class="col-md-12">
                                        <div class="alert alert-success">
                                            <asp:Label ID="lblSuccessmsg" runat="server" CssClass="successmsg"></asp:Label>
                                        </div>
                                    </div>
                                </div>--%>

                                
                                <asp:HiddenField ID="hdnSearchResult" Value="" runat="server" />
                                <asp:HiddenField ID="hdnSearchResultCopy" Value="" runat="server" />
                                <asp:HiddenField ID="hdnCurrentDate" Value="" runat="server" />
                                <asp:HiddenField ID="hdnApplicationUrl" Value="" runat="server" />
                                <asp:HiddenField ID="hdntalktoAttoneycheck" Value="1" runat="server" />
                            </form>

                            <span class="clearfix"></span>
                            <asp:Literal runat="server" ID="litScript"></asp:Literal>
                        </div>
                        <!-- .panel-body -->
                    </div>
                </div>
            </div>
        </div>
    </section>



    <!-- Main Wrapper -->

    <div id="message_model" class="modal fade">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body" id="modelViewlog">
                    <p class="text-center" id="viewLogDetail">
                        Sent email to an Attorney.
                    </p>

                </div>
            </div>
        </div>
    </div>
   
    <!-- Modal -->
    <!-- Search Tip Model -->
        <div id="searchmodal" class="modal fade" role="dialog">
          <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">BOB’S SEARCH TIPS</h4>
              </div>
              <div class="modal-body">
                <p>The following suggested tips are intended to help you get the most out of your preliminary search.</p>
                  <ul class="arrow">
                      <li>If your proposed mark can logically be broken into two or more pieces, it is better to use BOB’s component search option. For example, “CALIBURGER” has two components “CALI” and “BURGER.” “D’AMICO & SONS” has two components “D’AMICO” and “SONS.” “KNOCKOUT THE PAIN” has two components “KNOCKOUT” and “PAIN.”</li>
                      <li>It is better to use a broad goods or services description than a narrow one. For example, if you offer “Japanese restaurant services” search for only “restaurant services.” If you offer “medical devices for treating pulmonary disease” search for only “medical devices.” If you offer “computer software for managing mobile devices and networks,” search for only “computer software.”</li>
                      <li>Review the individual records in the search results to see if the detailed goods or services descriptions are like your goods or services. If they are like your goods or services, rethink adopting that mark or consult with a trademark attorney about it.</li>
                      <li>Search Google for any identical marks located in your geographic area.</li>
                  </ul>
                  <p>Don’t get discouraged if the first search returns a Warning light. Coming up with an available name can take time and patience.</p>
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
              </div>
            </div>

          </div>
        </div>
     <!-- Search Tip Model End-->
    <style>
        #ui-id-1 {
            max-width: 500px;
            width: 77% !important;
            margin-right: 10px;
        }
    </style>

</asp:Content>