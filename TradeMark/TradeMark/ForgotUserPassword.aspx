<%@ Page Title="" Language="C#" MasterPageFile="~/StaticPagesMaster.Master" AutoEventWireup="true" CodeBehind="ForgotUserPassword.aspx.cs" Inherits="TradeMark.forgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function Validation() {

            var isValid = "1";
            $("#<%=lblEmailmsg.ClientID%>").html("");
            var email = $("#<%=txtemail.ClientID%>")[0].value;


             var re = /^\w+([-+.'][^\s]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
             // var re = /^([a-zA-Z0-9.])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
             var emailFormat = re.test($("#<%=txtemail.ClientID%>")[0].value);


             if ($("#<%=txtemail.ClientID%>").val() == "") {
                 $("#<%=lblEmailmsg.ClientID%>").append("Enter your email address to retrieve your password.");
                isValid = "0";

            }
            else {
                if (emailFormat) {

                }
                else {
                    setTimeout("", 4000);
                    $("#<%=lblEmailmsg.ClientID%>").append("Invalid email format");
                    isValid = "0";
                }
            }

            if (isValid == "1") { return true; } else { return false; }
        }
        $(document).ready(function () {

            setTimeout(hideMessage, 3000);

        });
        function hideMessage() {
            $("#<%=lblSuccessmsg.ClientID%>").html("");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!--Forgot Password Content Start-->
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="heading">
                    <h2><span>Forgot Password</span></h2>
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
                    <div class="panel-body-content forgotpassword">
                        <div class="content-info form-content">
                            <form role="form" name="reset_form" runat="server">
                                <asp:Label ID="lblSuccessmsg" class="successmsg" runat="server"></asp:Label>
                                <asp:Label ID="lblFailuremsg" class="lbl-red" runat="server"></asp:Label>
                                <div class="row clearfix">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="control-label" for="username">Email<span class="text-danger">*</span>:</label>
                                            <asp:TextBox placeholder="example@example.com" class="form-control" name="email" ID="txtemail" runat="server" TextMode="email" />
                                            <asp:Label ID="lblEmailmsg" class="lbl-red" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row clearfix">
                                    <div class="col-md-6">
                                        <a href="login.aspx" class="btn p-l0">
                                            <i aria-hidden="true" class="fa fa-arrow-circle-o-left"></i> Back to Login</a>
                                    </div>
                                    <div class="col-md-6">
                                        <div class=" text-right">

                                            <asp:Button ID="btnResetPassword" runat="server" ToolTip="Reset Password" Text="Reset password" class="btn btn-primary btn-big" OnClick="btnResetPassword_Click" OnClientClick="return Validation()" />
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
    <!--Forgot Password Content End-->



</asp:Content>
