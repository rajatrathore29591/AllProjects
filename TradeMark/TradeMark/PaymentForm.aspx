<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PaymentForm.aspx.cs" Inherits="TradeMark.PaymentForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="https://js.stripe.com/v2/"></script>
    <script src="Scripts/Payment.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <div >
        <form id="form1" runat="server">

            <div class="col-lg-8">
                <div class="col-lg-4">
                    <asp:Label ID="Label1" runat="server" Text="Label">Card No.</asp:Label>

                </div>
                <div class="col-lg-4">
                    <asp:TextBox ID="txtCard" runat="server"></asp:TextBox>
                    <span id="spnErrorCard"></span>
                </div>
            </div>
            <div class="col-lg-8">
                <div class="col-lg-4">
                    <asp:Label ID="Label2" runat="server" Text="Label">Expiration</asp:Label>

                </div>
                <div class="col-lg-4">
                    <asp:TextBox ID="txtExpiration" runat="server"></asp:TextBox>
                    <span id="spnErrorExpiration"  style="margin-top:60px"></span>
                   
                </div>
            </div>
            <div class="col-lg-8">
                <div class="col-lg-4">
                    <asp:Label ID="Label3" runat="server" Text="Label">CVC</asp:Label>

                </div>
                <div class="col-lg-4">
                    <asp:TextBox ID="txtCVC" runat="server" ></asp:TextBox>
                     <span id="spnErrorCVC"></span>
                </div>
            </div>
            <div class="col-lg-8">
                <input type="submit" value="Submit" />
            </div>
        </form>
    </div>--%>

        <div class="container">
        <input type="hidden" id="hdnAmount" runat="server" />
        <input type="hidden" id="hdnCredits" runat="server" />
        <input type="hidden" id="hdnPromoCode" runat="server" />
        <input type="hidden" id="hdnToken" runat="server" />
        <div class="row">
            <div class="col-md-12">
                <div class="heading">
                    <h2><span>BOB Payment Form</span></h2>
                </div>

                <form role="form" name="add_factory_form" runat="server">
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
                            <div class="row clearfix">
                            <div class="col-md-6">
                                <label for="txtCardNumber">Card Number<span class="text-danger">*</span>:</label>
                                <div class="form-group">
                                    <input class="form-control" type="text" id="txtCardNumber" placeholder="Card Number e.g 1234..." maxlength="16" />
                                    <span id="spnErrorCard" class="lbl-red"></span>
                                </div>
                            </div>
                                </div>
                             <div class="row clearfix">
                            <div class="col-md-6">
                                <label for="txtCvc">CVC<span class="text-danger">*</span>:</label>
                                <div class="form-group">
                                    <input class="form-control" type="password" id="txtCvc" placeholder="Security Code e.g 987" maxlength="3"/>
                                    <span id="spnErrorCVC" class="lbl-red"></span>
                                </div>
                            </div>
                                 </div>
                             <div class="row clearfix">
                            <div class="col-md-6">
                                <label for="txtExpiryMonth">Expiry<span class="text-danger">*</span>:</label>
                                <div class="form-group">
                                    <input class="form-control" type="text" id="txtExpiry" placeholder="MM/YY" />
                                    <span id="spnErrorExpiry" class="lbl-red"></span>
                                </div>
                            </div>
                                 </div>
                             <div class="row clearfix">
                            <div class="col-md-6">
                                <label for="txtCredits">Credits:</label>
                                <div class="form-group">
                                    <input class="form-control" type="text" id="txtCredits" readonly="readonly" runat="server" />
                                </div>
                            </div>
                                 </div>
                             <div class="row clearfix">
                            <div class="col-md-6">
                                <label for="txtAmount">Amount</label>
                                <div class="form-group">
                                    <input class="form-control" type="text" id="txtAmount" placeholder="MM/YY" readonly="readonly" runat="server" />
                                </div>
                            </div>
                                 </div>
                             <div class="row clearfix">
                            <div class="col-md-6 no-padding chkStyle">
                                <div class="form-group common-checkbox m-t0 chkStyle">
                                    <%--<label class="control-label" for="CheckBox">--%>
                                        <%--<input type="checkbox" id="CheckBoxSubscriptionAgreement" class="common-checkbox-item " name="checkbox-1" /> --%>
                                        <%--<asp:CheckBox ID="CheckBoxSubscriptionAgreement" class="common-checkbox-item" Text="" runat="server" />--%>
                                        
                                        <%--<a href="SubscriptionAgreement.aspx" target="_blank">Subscription Agreement</a> --%>
                                    <%--</label>--%>
                                    <span id="spnSubscriptionAgreement" class="lbl-red"></span>
                                </div>

                                 <div class="form-group chkStyle">
                                                <div class="common-checkbox-item m-t0">
                                                    <asp:CheckBox ID="CheckBoxSubscriptionAgreement" Text="Subscription Agreement" runat="server" />
                                                    <%--<a href="SubscriptionAgreement.aspx" target="_blank">Subscription Agreement</a> --%>
                                                </div>
                                            </div>
                               </div>
                                 </div>
                             <div class="row clearfix">
                            <div class="col-md-12">
                                <div class="text-right">
                                    <input type="button" id="btnCharge" value="Submit" class="btn btn-primary btn-big" />
                                </div>
                            </div>
                                 </div>

                 
                        </div>
                    </div>
                    <!--Panel Body End-->
                </div>
                <!--Panel Shadow Box End-->
                    </form>

                </div>
            </div>
            </div>


    <div>

       
    </div>
</asp:Content>
