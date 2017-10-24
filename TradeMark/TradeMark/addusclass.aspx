<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="addusclass.aspx.cs" Inherits="TradeMark.addusclass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Add US-Class Content Start-->
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="heading">
                    <h2><span>Add US-Class</span></h2>
                </div>
                <!--Panel Shadow Box Start-->
                <div class="panel-shadow-box">
                    <!--Panel Body End-->
                    <div class="panel-body-content addusclass">
                        <div class="content-info form-content">
                         <form role="form" name="addUsClass_form" runat="server" id="addUsClassForm">
                    <asp:HiddenField ID="hndStatus" runat="server" Value="true" />
                    <asp:Label ID="lblSuccessMsg" runat="server"></asp:Label>

                    <div class="row clearfix">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="control-label" for="classno">Class No<span class="text-danger">*</span>:</label>
                                <asp:TextBox MaxLength="10" placeholder="Class No" class="form-control" name="ClassNo" ID="txtClassNo" runat="server" />
                                <span id="spnClassNo" class="lbl-red"></span>
                            </div>
                        </div>
                                         <div class="col-md-6">
                            <div class="form-group">
                                <label class="control-label" for="usclassdescriptions">US Class Description<span class="text-danger">*</span>:</label>
                                <asp:TextBox MaxLength="500" placeholder="US Class Descriptions" class="form-control" name="USClassDescriptions" ID="txtUSClassDescriptions" runat="server" onkeyup="CheckUSClassDescriptionsAvailability()" />
                                <span id="spnUSClassDescriptions" class="lbl-red"></span>
                            </div>
                        </div>
                                            <div class="col-md-12">
                        <div class="text-right">
                            <a title="Cancel" class="btn btn-default btn-big" href="Search.aspx">Cancel</a>
                            <asp:Button ID="btnUSClassDescriptions" runat="server" Text="Save" class="btn btn-primary btn-big" OnClick="btnUSClassDescriptions_Click" OnClientClick="return Validation()" />
                            <span class="clr"></span>
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
<!--Add US-Class Content End-->


    <script type="text/javascript">

        // apply validation on form on button click
        function Validation() {
            var isValid = "1";
            //$('#spnUSClassDescriptions').html("");
            $('#spnClassNo').html("");

            //check value can't be null
            if ($.trim($("#<%=txtClassNo.ClientID%>").val()) == "") {
                $('#spnClassNo').html("Required!");
                isValid = "0";
            }
            if ($.trim($('#<%=txtUSClassDescriptions.ClientID %>').val()) == "") {
                var str = "Required!";

                $('#spnUSClassDescriptions').html(str);
                document.getElementById('spnUSClassDescriptions').className = 'lbl-red';
                isValid = "0";
            }
            if (isValid == "1" && $("#<%=hndStatus.ClientID%>").val() == "true") { return true; } else { return false; }
        }
        //Function to check the us class already exists
        function CheckUSClassDescriptionsAvailability() { //This function call on text change.

            //get value into txtUSClassDiscription text box
            var usclassdescriptions = $("#<%=txtUSClassDescriptions.ClientID%>")[0].value;

            if (usclassdescriptions != null) {
                $.ajax({
                    url: 'AjaxHandler.aspx?status=CheckUSClassDescription&USClassDescription=' + usclassdescriptions, // this for calling the web method function in cs code. 
                    dataType: 'text',
                    success:
                        function (result) {
                            if (result == "true") {
                                var str = "US Class Description is already exist"
                                $("#spnUSClassDescriptions").html(str);
                                document.getElementById('spnUSClassDescriptions').className = 'lbl-red';
                                $("#<%=hndStatus.ClientID%>").val("false");
                            }
                            else {
                                $("#spnUSClassDescriptions").html("");
                                $("#<%=hndStatus.ClientID%>").val("true");
                                }
                        }
                });
                    }
                }

                //message 
                $(document).ready(function () {
                    setTimeout(hideMessage, 3000);
                });
                function hideMessage() {
                    $("#<%=lblSuccessMsg.ClientID%>").html("");
            }
    </script>
</asp:Content>
