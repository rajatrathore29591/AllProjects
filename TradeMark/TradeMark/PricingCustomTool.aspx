<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PricingCustomTool.aspx.cs" MasterPageFile="~/StaticPagesMaster.Master" Inherits="TradeMark.PricingCustomTool" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>PricingCustomTool- Trademark</title>
    <script src="Scripts/PriceCustomTool.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Pricing Bob Content Start-->
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="heading" style="padding: 1.375rem 0">
                    <h2><span>Pricing</span></h2>
                </div>
                <input type="hidden" id="hdnAmount" runat="server">
                <!--Panel Shadow Box Start-->
                <form id="form1" runat="server">
                    <div class="panel-shadow-box-white">

                        <div class="panel-body-content p-0">
                            <div class="content-info pricing">

                                <p><b class="first-word-head">BOB</b> is an on demand, flexible alternative to traditional preliminary search options. Pay for a single search or pre-pay for multiple searches. You choose whatever works best with your budget. In its 2015 Economic Survey, the American Intellectual Property Association (“AILPA”) reported that the average cost for a trademark clearance search, analysis, and opinion is <strong>&#36;1,486</strong>.  </p>

                                <div class="box-shadow p-t20 p-b20 text-center m-b15 m-t15">
                                    <p><span class="highlight">One Name -</span> <b class="text-capitalize subheading">$99.99 THIS IS A 93% DISCOUNT TO THE AVERAGE COST OF A TRADEMARK SEARCH REPORTED BY AILPA!</b></p>
                                </div>
                                
                                <p>Searching more than one name? Create a custom package of up to 100 names.[ Search credits expire two years after the date of purchase, and are non-transferable.] The more you search the more you save. Enter the total number of names you will search and <strong>BOB</strong> will tell you what the total cost of your custom package will be.</p>
                                <%if (Session["UserName"] != null)
                                    { %>
              
                               <div class="clearfix promobox">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <div class="form-group form-content">
                                              <asp:Label ID="lblPromoCode" runat="server" CssClass="control-label" Text="Promo Code"></asp:Label>
                            <asp:TextBox MaxLength="100" placeholder="Apply Promo Code" class="form-control" name="firstName" ID="txtApplyPromoCode" runat="server" />
                                        <span id="spnErrortxtPromocode" class="lbl-red"></span>
                                        <span id="spnsuccessMsg" class="success"></span>
                                            </div>
                                        </div>
                                    </div>
                                

                                <div class="col-lg-12 form-content">                   
                                        <input type="hidden" id="hdnApplyCoupan" runat="server" value="true" class="form-control">
                                         <input type="button" class="btn btn-primary  btn-big" value="Apply Coupan" id="btnApply" title="Apply Coupan" onclick="ApplyCoupan()">
                                    <input type="button" class="btn btn-primary  btn-big" title="Buy Single Search" value="Buy Single Search" id="btnBuySingleSearch" onclick="MakePaymentforsingleSearch()">
             
                                </div>
                                </div>
                       
                                <%--<asp:Button ID="btnBuySingleSearch" runat="server" Text="Buy Single Search" />--%>
                                <%}%>
                                <p class="text-capitalize subheading"><b>Custom Price Tool</b></p>


                                 <div class="row clearfix">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <div class="form-content">
                                                <asp:TextBox MaxLength="4" placeholder="Enter the total number of names" class="form-control" name="" ID="txtNoOFName" runat="server" onchange="PriceCalculationTotalNumber()" />
                                        <span id="spnErrortxtNoOFName" class="lbl-red"></span>
                                                </div>
                                     
                                            </div>
                                        </div>
                                     </div>
                                <div class="row clearfix">
                                    <div class="col-md-6">
                                           <div class="form-group form-content">
                                                <asp:TextBox MaxLength="100" placeholder="Total Cost of the Custom Package" class="form-control" name="firstName" ID="txtTotalCost" runat="server" ReadOnly="true" />
                                                </div>
                                    </div>
                                        
                                    </div>
                                <%if (Session["UserName"] != null)
                                    { %>
                                <input type="button" title="Buy" value="Buy" id="btnBuy" class="hide btn btn-primary btn-big m-b10" onclick="MakePayment()">
                                <%}%>

                                <%--<asp:Button ID="btnBuy" runat="server" Text="Buy" />--%>

                                <p>Don’t want to be bothered with loading new searches on your account when you run out? Select auto renew and BOB will load the number of searches you previously purchased to your account after the last search credit is used.</p>

                                <p class="Center"><span class="subheading"><b>High-Volume Searching</b></span></p>
                                <p>Searching more than 100 names? Purchase a custom, high-volume package[ Search credits in a high-volume package expire two years after the date of purchase, and are non-transferable.] from BOB. The minimum high-volume package starts at 1,000 names for only $19.99 a name. Use all 1,000 searches in two years from the date of purchase, and your effective per name cost will be $19.99. Use less than all 1,000 search credits and your effective per name cost will be higher than $19.99. Use the Effective Cost Calculator to determine your effective per name cost after determining the total cost of your custom package using the Custom Price Tool</p>

                                     <div class="row clearfix">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <div class="form-content">
                                                <asp:TextBox MaxLength="4" placeholder="Enter the total number of names over a two-year period" class="form-control" name="TotalNoSearchUsed" ID="txtTotalNoSearchUsed" runat="server" onchange="PriceCalculationTotalNoSearchUsed()" />
                                        <span id="spnErrortxtTotalNoSearchUsed" class="lbl-red"></span>
                                                </div>
                                     
                                            </div>
                                        </div>
                                     </div>
                                <div class="row clearfix">
                                    <div class="col-md-6">
                                           <div class="form-group form-content">
                                               <asp:TextBox MaxLength="100" placeholder="Effect Per Name Cost" class="form-control" name="SearchPerCost" ID="txtSearchPerCost" runat="server" ReadOnly="true" />
                                                </div>
                                    </div>
                                        
                                    </div>
                                <p>Use the Custom Price Tool to determine the total cost of your high-volume package,<strong>then contact BOB to discuss payment schedule options.</strong></p>

                            </div>
                            <div>
                            </div>

                        </div>

                        <!--Panel Body End-->
                    </div>
                    <!--Panel Shadow Box End-->
                </form>
            </div>
        </div>
    </div>
    <!--Pricing Content End-->
    <script>

        
    </script>
</asp:Content>


