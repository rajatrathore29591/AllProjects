<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="edituser.aspx.cs" Inherits="TradeMark.edituser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="normalheader transition">
        <div class="hpanel">
            <div class="panel-body">
                <div class="container">
                    <h2 class="font-light m-b-xs">Edit User </h2>
                </div>
                <!--/.container-->
            </div>
        </div>
    </div>

    <div class="container">
        <div class="login-container signup-container">
            <div>
                <div class="panel-body">
                    <form role="form" name="editUser_form" runat="server">
                        <asp:HiddenField ID="hndStatus" runat="server" Value="true" />
                        <asp:Label ID="lblSuccessmsg" runat="server" class="successmsg"></asp:Label>
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="control-label" for="name">First Name:</label>
                                    <asp:TextBox MaxLength="100" placeholder="First Name" class="form-control" name="name" ID="txtEFirstName" runat="server" />
                                    <span id="spnFirstName" class="lbl-red"></span>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="control-label" for="contact">Last Name :</label>
                                    <asp:TextBox MaxLength="100" placeholder="Last Name" class="form-control" name="lastName" ID="txtELastName" runat="server" />
                                    <span id="spnLastName" class="lbl-red"></span>
                                </div>
                            </div>

                        </div>

                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="control-label" for="username">Email:</label>
                                    <asp:TextBox MaxLength="250" placeholder="example@example.com" class="form-control" name="email" ID="txtEEmail" runat="server" onchange="UserOrEmailAvailability()" />
                                    <span id="spnEmail"></span>
                                    <asp:Label ID="lblStatus" class="lbl-red" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="control-label" for="username">Registered Date:</label>
                                    <asp:TextBox MaxLength="50" placeholder="Registered Date" class="form-control" name="registeredDate" ID="txtRegisteredDate" runat="server" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="control-label" for="contact">Contact Number:</label>
                                    <asp:TextBox MaxLength="14" placeholder="Contact Number" class="form-control" name="contact" ID="txtEContactNo" runat="server" />
                                    <span id="spnContactNo" class="lbl-red"></span>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="control-label" for="password">Company Name:</label>
                                    <asp:TextBox MaxLength="200" placeholder="Company Name" class="form-control" name="companyName" ID="txtECompanyName" runat="server" />
                                    <span id="spnCompanyName" class="lbl-red"></span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="control-label" for="contact">Title:</label>
                                    <asp:TextBox MaxLength="50" placeholder="Title" class="form-control" name="title" ID="txtETitle" runat="server" />
                                    <span id="spnTitle" class="lbl-red"></span>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="control-label" for="password">Street Address:</label>
                                    <asp:TextBox MaxLength="300" placeholder="Street Address" class="form-control" name="streetAddress" ID="txtEStreetAddress" runat="server" />
                                    <span id="spnStreetAddress" class="lbl-red"></span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="control-label" for="contact">Status:</label>
                                    <asp:CheckBox class="form-control" name="Status" ID="chkStatus" Text="&nbsp;&nbsp;IsActive" runat="server" />
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <%-- <div class="form-group">
                                    <label class="control-label" for="password">Street Address:</label>
                                    <asp:TextBox MaxLength="50" placeholder="Street Address" class="form-control" name="streetAddress" ID="TextBox2" runat="server" />
                                    <span id="spnStreetAddress" class="lbl-red"></span>
                                </div>--%>
                            </div>
                        </div>

                        <span class="clr"></span>
                        <div class=" text-center">
                            <%--  <button title="Cancel" class="btn btn-success fr mrg" onclick="return redirect();">Cancel</button>--%>
                            <a title="Cancel" class="btn btn-success fr mrg" onclick="return redirect();">Cancel</a>
                            <asp:Button ID="btnEditUser" runat="server" Text="Submit" class="btn btn-success fr" OnClientClick="return Validation()" OnClick="btnEditUser_Click" />
                            <span class="clr"></span>
                        </div>
                    </form>
                </div>
            </div>

        </div>
    </div>

    <script type="text/javascript">

        function redirect() {
            window.location.href = 'users.aspx';
            return false;
        }
        // apply validation on form on button click
        function Validation() {
            var isValid = "1";
            $('#spnFirstName').html("");
            $('#spnLastName').html("");
            $('#spnConfirmpassword').html("");
            $('#spnPassword').html("");
            $('#spnEmail').html("");
            //check value can't be null
            if ($.trim($("#<%=txtEFirstName.ClientID%>").val()) == "") {

                $('#spnFirstName').html("Required!");
                isValid = "0";
            }

            if ($.trim($("#<%=txtELastName.ClientID%>").val()) == "") {

                $('#spnLastName').html("Required!");
                isValid = "0";
            }
            if ($.trim($('#<%=txtEEmail.ClientID %>').val()) == "") {
                var str = "Required!";

                $('#spnEmail').html(str);
                document.getElementById('spnEmail').className = 'lbl-red';
                isValid = "0";
            }

            if ($.trim($('#<%=txtEContactNo.ClientID %>').val()) == "") {
                $('#spnContactNo').html("Required!");
                isValid = "0";
            }
            if (isValid == "1" && $("#<%=hndStatus.ClientID%>").val() == "true") { return true; } else { return false; }
        }

        function UserOrEmailAvailability() { //This function call on text change.
            $("#<%=lblStatus.ClientID%>").html("");

            var email = $("#<%=txtEEmail.ClientID%>")[0].value;

            var re = /^\w+([-+.'][^\s]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
            //  var re = /^([a-zA-Z0-9.])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
            var emailFormat = re.test($("#<%=txtEEmail.ClientID%>")[0].value);

            if (emailFormat) {
                $.ajax({
                    url: 'AjaxHandler.aspx?status=CheckEmail&emailId=' + email, // this for calling the web method function in cs code. 
                    dataType: 'text',
                    success:
                        function (result) {
                            var msg = $("#<%=lblStatus.ClientID%>")[0];
                            switch (result) {
                                case "true":
                                    var str = "Email is not available";
                                    $("#spnEmail").html(str);
                                    document.getElementById('spnEmail').className = 'lbl-red';
                                    $("#<%=hndStatus.ClientID%>").val("false");
                                    break;
                                case "false":
                                    var str = "Email is available";
                                    $("#spnEmail").html(str)
                                    document.getElementById('spnEmail').className = 'successmsg';
                                    $("#<%=hndStatus.ClientID%>").val("true");
                                    break;
                            }
                        }
                });
                }
                else {
                    setTimeout("", 4000);
                    $("#<%=lblStatus.ClientID%>").html("Invalid email format")
            }
        }

        $(document).ready(function () {
            setTimeout(hideMessage, 3000);
        });
        function hideMessage() {
            $("#<%=lblSuccessmsg.ClientID%>").html("");
            }
    </script>
</asp:Content>
