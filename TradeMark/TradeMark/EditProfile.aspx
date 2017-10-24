<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="TradeMark.EditProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function showEditProfileSuccessmsg() {
            
            $('#divSuccess').show();
            setTimeout(hideEditProfileSuccessmsg, 5000);
        }
        function hideEditProfileSuccessmsg(Id)
        {
            
            $('#divSuccess').hide();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!--Edit Profile Content Start-->
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="heading">
                    <h2><span>Edit Profile</span></h2>
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
                            <form role="form" name="signUp_form" runat="server">
                                <asp:HiddenField ID="hndStatus" runat="server" Value="true" />
                                <div class="row clearfix" id="divSuccess" style="display:none;">
                                     <div class="col-md-12">
                                          <div class="alert alert-success">
                                               <asp:Label ID="lblSuccessmsg" runat="server" ></asp:Label>
                                   </div>
                                         </div>
                                    </div>
                               
                               
                                <div class="row clearfix">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="control-label" for="name">First Name<span class="text-danger">*</span>:</label>
                                            <asp:TextBox MaxLength="100" placeholder="First Name" class="form-control" name="name" ID="txtEFirstName" runat="server" />
                                            <span id="spnFirstName" class="lbl-red"></span>

                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="control-label" for="contact">Last Name<span class="text-danger">*</span>:</label>
                                            <asp:TextBox MaxLength="100" placeholder="Last Name" class="form-control" name="lastName" ID="txtELastName" runat="server" />
                                            <span id="spnLastName" class="lbl-red"></span>
                                        </div>
                                    </div>
                                     </div>
                                   <div class="row clearfix">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="control-label" for="username">Email:</label>
                                            <asp:TextBox MaxLength="250" placeholder="example@example.com" class="form-control" name="email" ID="txtEEmail" runat="server" />
                                            <span id="spnEmail"></span>
                                            <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="control-label" for="username">Registered Date:</label>
                                            <asp:TextBox MaxLength="50" placeholder="Registered Date" class="form-control" name="registeredDate" ID="txtRegisteredDate" runat="server" ReadOnly="true" />
                                        </div>
                                    </div>
                                       </div>

                                   <div class="row clearfix">

                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="control-label" for="contact">Contact Number<span class="text-danger">*</span>:</label>
                                            <asp:TextBox MaxLength="14" placeholder="Contact Number" class="form-control" name="contact" ID="txtEContactNo" runat="server" />
                                            <span id="spnContactNo" class="lbl-red"></span>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="control-label" for="password">Company Name:</label>
                                            <asp:TextBox MaxLength="200" placeholder="Company Name" class="form-control" name="companyName" ID="txtECompanyName" runat="server" />
                                            <span id="spnCompanyName" class="lbl-red"></span>
                                        </div>
                                    </div>

                                       </div>
                                   <div class="row clearfix">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="control-label" for="contact">Title:</label>
                                            <asp:TextBox MaxLength="50" placeholder="Title" class="form-control" name="title" ID="txtETitle" runat="server" />
                                            <span id="spnTitle" class="lbl-red"></span>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="control-label" for="password">Street Address:</label>
                                            <asp:TextBox MaxLength="300" placeholder="Street Address" class="form-control" name="streetAddress" ID="txtEStreetAddress" runat="server" />
                                            <span id="spnStreetAddress" class="lbl-red"></span>
                                        </div>
                                    </div>

                                       </div>
                                   <div class="row clearfix">
                                    <div class="col-md-12">
                                        <div class=" text-right">

                                            <button title="Cancel" class="btn btn-default btn-big" onclick="return redirect();">Cancel</button>
                                            <asp:Button ID="btnEditProfile" title="Submit" runat="server" Text="Submit" class="btn btn-primary btn-big" OnClick="btnEditProfile_Click" OnClientClick="return Validation()" />
                                            <span class="clr"></span>
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
    <!--Edit Profile Content End-->




    <script type="text/javascript">

        function redirect() {
            window.location.href = 'Search.aspx';
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
        
    </script>
</asp:Content>
