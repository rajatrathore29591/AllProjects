<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PromoCode.aspx.cs" Inherits="TradeMark.PromoCode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $("#Save").click(function() {
            var name = $("#<%= txtTitle.ClientID %>").val();
            if(name == "")
            {
                "#<%=lblTitleErrorMsg.ClientID %>").val()= "Please enter a Title";
         }        
        });
    </script>
  
    <!--Promocode Content Start-->
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="heading">
                    <h2><span>PromoCode</span></h2>
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
                    <!--Panel Body End-->
                    <div class="panel-body-content editprofile">
                        <div class="content-info form-content">
                            <form role="form" name="Promocode_form" runat="server">
                                <div class="row clearfix" id="divSuccess" style="display: none;">
                                    <div class="col-md-12">
                                        <div class="alert alert-success">
                                            <asp:Label ID="lblSuccess" class="successmsg" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>


                               &nbsp;<asp:Label ID="lblFailure" class="lbl-red" runat="server"></asp:Label>
                                <div class="row clearfix">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                          
                                            <asp:Label ID="lbl1" class="control-label" Visible="true" runat="server">Title<span class="text-danger">*</span>:</asp:Label>
                                            <asp:TextBox MaxLength="200" placeholder="Title" class="form-control" name="Title" ID="txtTitle" runat="server" />
                                            <asp:Label ID="lblTitleErrorMsg" class="lbl-red" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row clearfix">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <div class="form-group">
                                                <label class="control-label" for="username">PromoCode<span class="text-danger">*</span>:</label>
                                                <asp:TextBox MaxLength="100" placeholder="Promocode" class="form-control" name="Promo" ID="txtPromoCode" runat="server" />
                                                <asp:Label ID="lblRequiredPromocode" class="lbl-red" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row clearfix">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="control-label" for="Price">Price<span class="text-danger">*</span>:</label>
                                            <asp:TextBox MaxLength="6" placeholder="Price" class="form-control" name="Price" ID="txtPrice" runat="server" />
                                            <asp:Label ID="lblPrice" class="lbl-red" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                  <div class="row clearfix">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="control-label" for="Redeemlimit">Redeemlimit<span class="text-danger">*</span>:</label>
                                            <asp:TextBox MaxLength="3" placeholder="Redeemlimit" class="form-control" name="Redeemlimit" ID="txtRedeemlimit" runat="server" />
                                            <asp:Label ID="lblredeemlimit" class="lbl-red" runat="server"></asp:Label>

                                        </div>
                                    </div>
                                </div>
                                <div class="row clearfix">
                                    <div class="col-md-12">
                                        <div class=" text-right">
                                            <asp:Button ID="Save" title="save" runat="server" Text="Save" class="btn btn-primary  btn-big" OnClick="btnsave_Click"  />
                                            
                                              &nbsp;
                                            
                                              <asp:Button ID="Cancel" title="Cancel" runat="server" Text="Cancel" class="btn btn-primary  btn-big" OnClick="btncancel_Click"  />
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
    <!--Change Password Content End-->
</asp:Content>


