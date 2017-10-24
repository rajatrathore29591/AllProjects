<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="TradeMark.Payment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="normalheader transition">
        <div class="hpanel">
            <div class="panel-body">
                <div class="container">
                    <h2 class="font-light m-b-xs">Payment </h2>
                </div>
                <!--/.container-->
            </div>
        </div>
    </div>
    <div class="container">
        <div class="login-container signup-container">
            <div>
                <div class="panel-body">
                    <form role="form" name="" runat="server">
                        <asp:HiddenField ID="hndStatus" runat="server" Value="true" />
                        <asp:Label ID="lblSuccessmsg" runat="server" class="successmsg"></asp:Label>
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="control-label" for="name">Card Number:</label>
                                    <asp:TextBox MaxLength="16" placeholder="Card Number" class="form-control" name="ssl_card_number" ID="ssl_card_number" runat="server" onkeydown = "return (!(event.keyCode>=65) && event.keyCode!=32);" />
                                    <span id="spnCardNumber" class="lbl-red"></span>

                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="control-label" for="contact">Expire Date:</label>
                                    <asp:TextBox MaxLength="30" placeholder="MMYY" class="form-control" name="ssl_exp_date" ID="ssl_exp_date" runat="server" />
                                    <span id="spnExpireDate" class="lbl-red"></span>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="control-label" for="username">CVV:</label>
                                    <asp:TextBox MaxLength="3" placeholder="CVV" class="form-control" name="ssl_cvv2cvc2" ID="ssl_cvv2cvc2" runat="server" onkeydown = "return (!(event.keyCode>=65) && event.keyCode!=32);"/>
                                    <span id="spnCVV"></span>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="control-label" for="username">Transaction Type:</label>
                                    <asp:TextBox MaxLength="30" placeholder="Transaction Type" class="form-control" name="ssl_transaction_type" ID="ssl_transaction_type" runat="server"/>
                                </div>
                            </div>
                        </div>                 
                        
                        <span class="clr"></span>
                        <div class=" text-center">
                            <button title="Cancel" class="btn btn-success fr mrg" onclick="return redirect();">Cancel</button>
                            <asp:Button ID="btnPayment" runat="server" Text="Submit" class="btn btn-success fr" OnClick="btnPayment_Click" />
                            <span class="clr"></span>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
