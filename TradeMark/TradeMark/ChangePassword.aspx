<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="TradeMark.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function showSuccessmsg() {

            $('#divSuccess').show();
            setTimeout(hideSuccessmsg, 5000);
        }
        function hideSuccessmsg(Id) {

            $('#divSuccess').hide();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function CheckPassword() {
            $("#<%=lblOldPwdErrorMsg.ClientID%>").html("");

            $("#<%=lblOldPwdErrorMsg.ClientID%>").html("");
            var getVal = $("#<%=hdnOldPassword.ClientID%>").val();
            var password = $("#<%=txtOldPassword.ClientID%>").val();

            if (getVal == "true") {
                if ($("#<%=HdnOldpasswordtxt.ClientID%>").val() == $("#<%=txtOldPassword.ClientID%>").val()) {

                    $("#<%=hdnStatus.ClientID%>").val("true");
                }
                else {

                    $("#<%=hdnStatus.ClientID%>").val("false");
                    $("#<%=lblOldPwdErrorMsg.ClientID%>").html("Password is invalid")
                }

            }

        }

        function validation() {
            var isValid = "1";
            if ($("#<%=lblOldPwdErrorMsg.ClientID%>").val() == "Required!") {
                $("#<%=lblOldPwdErrorMsg.ClientID%>").html("");
            }
            $("#<%=lblRequiredConfirmPwd.ClientID%>").html("");
            $("#<%=lblRequiredNewPwd.ClientID%>").html("");
            var getstatus = $("#<%=hdnStatus.ClientID%>").val();

            var getOldPasswordVal = $("#<%=hdnOldPassword.ClientID%>").val();

            if (getOldPasswordVal == "true") {

                if ($("#<%=txtOldPassword.ClientID%>").val() == "") {
                    $("#<%=lblOldPwdErrorMsg.ClientID%>").html("Required!");

                    isValid = "0";
                }
                else
                    if (getstatus == "false") {
                        isValid = "0";
                    }
            }

            if ($("#<%=txtNewPassword.ClientID%>").val() == "") {
                $("#<%=lblRequiredNewPwd.ClientID%>").html("Required!");
                isValid = "0";
            }
            else {
                var regex = /^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$/;
                var passwordFormat = regex.test($("#<%=txtNewPassword.ClientID %>")[0].value);
                if (passwordFormat) {
                }
                else {

                    $("#<%=lblRequiredNewPwd.ClientID%>").html("Please enter alpha numeric with special character password with minimum 8 characters");
                    isValid = "0";
                }
            }
            if ($("#<%=txtConfirmPassword.ClientID%>").val() == "") {
                $("#<%=lblRequiredConfirmPwd.ClientID%>").append("Required!");
                isValid = "0";

            }
            else {
                if ($("#<%=txtNewPassword.ClientID%>").val() != $("#<%=txtConfirmPassword.ClientID%>").val()) {
                    $("#<%=lblRequiredConfirmPwd.ClientID%>").html("New password and confirm password does not match");
                     isValid = "0";
                 }
             }

             if (isValid == "1") { return true; } else { return false; }
         }
         $(document).ready(function () {

             //setTimeout(hideMessage, 3000);

         });
         function hideMessage() {

             $("#<%=lblSuccess.ClientID%>").html("");
            }

    </script>


    <!--Change Password Content Start-->
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="heading">
                    <h2><span>Change Password</span></h2>
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
                            <form role="form" name="ChangePassword_form" runat="server">
                                <div class="row clearfix" id="divSuccess" style="display: none;">
                                    <div class="col-md-12">
                                        <div class="alert alert-success">
                                            <asp:Label ID="lblSuccess" class="successmsg" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>


                                <asp:Label ID="lblFailure" class="lbl-red" runat="server"></asp:Label>
                                <div class="row clearfix">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:HiddenField ID="hdnOldPassword" runat="server" Value="" />
                                            <asp:HiddenField ID="hdnStatus" runat="server" />
                                            <asp:HiddenField ID="HdnOldpasswordtxt" runat="server" Value="" />
                                            <asp:Label ID="lblOldPassword" class="control-label" Visible="false" runat="server">Old Password<span class="text-danger">*</span>:</asp:Label>
                                            <asp:TextBox MaxLength="30" placeholder="******" class="form-control" name="OldPassword" ID="txtOldPassword" runat="server" TextMode="Password" Visible="false" onchange="return CheckPassword()" />
                                            <asp:Label ID="lblOldPwdErrorMsg" class="lbl-red" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row clearfix">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <div class="form-group">
                                                <label class="control-label" for="username">New Password<span class="text-danger">*</span>:</label>
                                                <asp:TextBox MaxLength="30" placeholder="******" class="form-control" name="NewPassword" ID="txtNewPassword" runat="server" TextMode="Password" />
                                                <asp:Label ID="lblRequiredNewPwd" class="lbl-red" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row clearfix">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="control-label" for="username">Confirm Password<span class="text-danger">*</span>:</label>
                                            <asp:TextBox MaxLength="30" placeholder="******" class="form-control" name="ConfirmPassword" ID="txtConfirmPassword" runat="server" TextMode="Password" />
                                            <asp:Label ID="lblRequiredConfirmPwd" class="lbl-red" runat="server"></asp:Label>

                                        </div>
                                    </div>
                                </div>
                                <div class="row clearfix">
                                    <div class="col-md-12">
                                        <div class=" text-right">
                                            <asp:Button ID="btnChangePassword" title="Change Password" runat="server" Text="Change Password" class="btn btn-primary  btn-big" OnClick="btnChangePassword_Click" OnClientClick="return validation()" />
                                        </div>
                                    </div>
                                </div>
                                <asp:Literal ID="litScript" runat="server"></asp:Literal>
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


