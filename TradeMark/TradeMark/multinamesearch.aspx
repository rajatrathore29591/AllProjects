<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeBehind="multinamesearch.aspx.cs" Inherits="TradeMark.multinamesearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="//code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script src="Scripts/Search.js"></script>
    <link href="//code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" rel="Stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!--Admin Setting Content Start-->
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="heading">
                    <h2><span>Multi Search</span></h2>
                </div>
                <!--Panel Shadow Box Start-->
                <div class="panel-shadow-box">
                    <!-- .panel-heading -->
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-md-6">
                                <span class="notify">All fields with a * are required. </span>
                            </div>
                        </div>
                    </div>
                    <!-- .panel-heading -->
                    <!--Panel Body Start-->
                    <div class="panel-body-content editprofile">
                        <div class="content-info form-content">
                            <form role="form" name="add_factory_form" id="form2" runat="server">
                                <asp:Panel ID="Panel2" runat="server">
                                    <div class="row clearfix" id="divSuccess">
                                        <div class="col-md-12">
                                            <div class="alert alert-success">
                                                <asp:Label CssClass="lbl-red" ID="lblMessage" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row clearfix">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <div class="common-checkbox-item m-t0">
                                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                                    <br />
                                                    <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="FileUpload1"
                                                        runat="server" Display="Dynamic" ForeColor="Red" />
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.xls|.xlsx|.csv)$"
                                                        ControlToValidate="FileUpload1" runat="server" ForeColor="Red" ErrorMessage="Please select a valid Excel file."
                                                        Display="Dynamic" />
                                                    <br />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row clearfix">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Email:</label>
                                                <asp:TextBox ID="TextBox1" runat="server" class="form-control"></asp:TextBox>
                                                <%--  <span id="spnEmail" class="spanstyle"></span>
                                                <span class="lbl-red" id="spnEmailError" style="display: none;">Required!</span>--%>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row clearfix">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Enter Goods or Services Description<span class="text-danger">*</span>:</label>
                                                <div id="divGoodServices">
                                                    <%--  <asp:DropDownList ID="ddlGoodsServices" runat="server" data-placeholder="Choose a Application type..." class="form-control  chosen-select" onclick="">
                                            </asp:DropDownList>--%>

                                                    <input id="autoGoodsData" placeholder="Enter Goods or Services" class="float-left form-control input-12 text-box single-line ui-autocomplete-input valid" type="text" value="" title="Enter Goods & Services name" name="autoGoodsData" autocomplete="off" />
                                                    <asp:HiddenField ID="HiddenField2" runat="server" />
                                                    <asp:HiddenField ID="HiddenField3" runat="server" />
                                                    <asp:HiddenField ID="HiddenField4" runat="server" Value="" />
                                                    <span class="lbl-red" id="spnGoodsDatatxtError" style="display: none;">Required!</span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row clearfix">
                                        <div class="col-md-12">
                                            <div class=" text-right">
                                                <asp:Button ID="Button1" Class="btn btn-primary btn-big" runat="server" Text="Import" OnClick="btnImport_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </form>
                        </div>
                    </div>
                    <!--Panel Body End-->
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                </div>
                <!--Panel Shadow Box End-->
            </div>
        </div>
    </div>
    <!--Admin Setting Content End-->
</asp:Content>

