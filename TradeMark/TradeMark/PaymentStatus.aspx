<%@ Page Title="" Language="C#" MasterPageFile="~/StaticPagesMaster.Master" AutoEventWireup="true" CodeBehind="PaymentStatus.aspx.cs" Inherits="TradeMark.PaymentStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="color-line"></div>
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="heading">
                    <h2><span>Payment Status</span></h2>
                </div>

                <div class="alert alert-success fade in alert-dismissable" >
                    


                   <form role="form" name="reset_form" runat="server">
                <dl class="dl-horizontal">
                    <dt>
                        <label for="txtCardNumber"><strong>Payment Status:</strong></label>
                    </dt>

                    <dd>
                        <label id="lblPaymentStatus" runat="server"></label>
                    </dd>

                    <dt>
                        <label for="txtCvc"><strong>Total Credits:</strong></label>
                    </dt>

                    <dd>
                        <label id="lblCredits" runat="server"></label>
                    </dd>


                    <dt>
                        <label for="txtExpiryMonth"><strong>Transaction Id:</strong></label>
                    </dt>
                    <dd>
                        <label id="lblTransactionId" runat="server"></label>
                    </dd>
                    <dt>


                    <dd></dd>
                </dl>
                    </form>
                    
                     
                </div>
                <span class="clearfix"></span>
                <p><a href="Search.aspx">
                        <i aria-hidden="true" class="fa fa-arrow-circle-o-left"></i> Click here to go at search</a></p>
                <p>&nbsp;</p>
                
            </div>
        </div>

    </div>

</asp:Content>
