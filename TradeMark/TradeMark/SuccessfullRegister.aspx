<%@ Page Title="" Language="C#" MasterPageFile="~/StaticPagesMaster.Master" AutoEventWireup="true" CodeFile="SuccessfullRegister.aspx.cs" Inherits="TradeMark.SuccessfullRegister" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="color-line"></div>
<div class="container">

            <div class="row registered-msg">
            <div class="col-md-12">
                     <div class="alert alert-success  fade in alert-dismissable" >
                    <h1 class="text-center thanks-msg">  <i class="fa fa-thumbs-o-up" aria-hidden="true"></i>  <strong>Thanks for Registration</strong></h1>
                </div>
                <form role="form" name="reset_form" runat="server">
                    <p><a href="login.aspx">
                        <i aria-hidden="true" class="fa fa-arrow-circle-o-left"></i> Click here to Login</a></p>
                         <p>&nbsp;</p>
                    </form>
                </div>
                </div>




  
 </div>

</asp:Content>
