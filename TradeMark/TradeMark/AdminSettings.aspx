<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeBehind="AdminSettings.aspx.cs" Inherits="TradeMark.AdminSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function ShowHide() {
            document.getElementById('spnmessage').innerText = 'Save successfully!';
            $('#divSuccess').show();
            setTimeout(hideSuccessmsg, 5000);
        }
        function hideSuccessmsg(Id) {
            $('#divSuccess').hide();
        }
    </script>
    <!--Admin Setting Content Start-->
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="heading">
                    <h2><span>Admin Setting</span></h2>
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
                    <!--Panel Body Start-->
                    <div class="panel-body-content editprofile">
                        <div class="content-info form-content">
                            <form role="form" name="add_factory_form" runat="server">
                                <asp:Panel ID="Panel1" runat="server">
                                    <div class="row clearfix" id="divSuccess" style="display:none;">
                                     <div class="col-md-12">
                                          <div class="alert alert-success">
                                                <span id="spnmessage" class="successmsg"></span>
                                   </div>
                                         </div>
                                    </div>
                                    <div class="row clearfix">
                                        <div class="col-md-6">
                                            <div class="form-group chkStyle">
                                                <div class="common-checkbox-item m-t0">
                                                    <asp:CheckBox ID="chkIsProgress" Text="Imprort is in progress?" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row clearfix">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Admin Email<span class="text-danger">*</span>:</label>
                                                <asp:TextBox ID="txtEmail" runat="server" class="form-control"></asp:TextBox>
                                                <span id="spnEmail" class="spanstyle"></span>
                                                <span class="lbl-red" id="spnEmailError" style="display: none;">Required!</span>

                                            </div>
                                        </div>
                                    </div>
                                    <div class="row clearfix">
                                        <div class="col-md-12">
                                            <div class=" text-right">
                                                <asp:Button ID="btnSave" title="Save" Text="Save" runat="server" OnClientClick="return ValidateEmail();" Class="btn btn-primary btn-big" OnClick="btnSave_Click" />
                                            </div>
                                        </div>
                                    </div>


                                </asp:Panel>

                                <asp:HiddenField ID="hdnRdoOption" Value="1" runat="server" />

                            </form>
                        </div>
                    </div>
                    <!--Panel Body End-->
                    <asp:Literal ID="litScript" runat="server"></asp:Literal>
                </div>
                <!--Panel Shadow Box End-->
            </div>
        </div>
    </div>
    <!--Admin Setting Content End-->

    <style>
        #ui-id-1 {
            max-width: 500px;
            width: 77% !important;
            margin-right: 10px;
        }
    </style>
    <script type="text/javascript">
        function ValidateEmail() {
            var isValid = 1;
            var email = $("#<%=txtEmail.ClientID%>")[0].value;
            var str = "Required!";
            if (email == "") {

                $('#spnEmail').text("");
                var str = "Required!";
                $('#spnEmail').append(str);
                document.getElementById('spnEmail').className = 'lbl-red';
                isValid = 0;
            }
            var re = /^\w+([-+.'][^\s]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
            var emailFormat = re.test($("#<%=txtEmail.ClientID%>")[0].value);
            // $('#spnEmail').html("");
            if (!emailFormat) {
                alert('in');
                isValid = 0;
            }
            if (isValid == 1) {
                return true;
            } else { return false; }
        }
    </script>

</asp:Content>
