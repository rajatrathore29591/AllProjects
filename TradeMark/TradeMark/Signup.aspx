<%@ Page Language="C#" MasterPageFile="~/Login.Master" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="TradeMark.Signup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div class="container-fluid">
     <div class="row row-flex login">
         <div class="col-lg-5 col-md-5 col-xs-12">
            <div class="login-container signup-container">
               <h2>Signup Here</h2>
               <div class="panel-bddody" id="signUpForm">
                  <div class="row">
                  <asp:HiddenField ID="hndStatus" runat="server" />
                  <asp:Label ID="lblMessage" runat="server" CssClass="lbl-red"></asp:Label>
                    
                  <div class="col-md-12">
                     <div class="form-group">
                        <asp:TextBox MaxLength="100" placeholder="First Name" class="form-control" name="firstName" ID="txtFirstName" runat="server" />
                        <span id="spnFirstName" class="lbl-red"></span>
                     </div>
                  </div>
                  <div class="col-md-12">
                     <div class="form-group">
                        <asp:TextBox MaxLength="100" placeholder="Last Name" class="form-control" name="lastName" ID="txtLastName" runat="server" />
                        <span id="spnLastName" class="lbl-red"></span>
                     </div>
                  </div>
                  <div class="col-sm-12">
                     <div class="form-group">
                        <asp:TextBox MaxLength="250" placeholder="example@example.com" class="form-control" name="email" ID="txtEmail" runat="server" onchange="UserOrEmailAvailability()" />
                        <span id="spnEmail"></span>
                     </div>
                  </div>
                  
                  <div  class="col-md-12">
                     <div class="form-group">
                        <asp:TextBox MaxLength="250" placeholder="******" class="form-control" name="password" ID="txtPassword" runat="server" TextMode="Password" />
                        <span id="spnPassword" class="lbl-red"></span>
                     </div>
                  </div>
                  <div class="col-md-12">
                     <div class="form-group">
                        <asp:TextBox MaxLength="250" placeholder="******" class="form-control" name="password" ID="txtConfirmpassword" runat="server" TextMode="Password" />
                        <span id="spnConfirmpassword" class="lbl-red"></span>
                     </div>
                  </div>
                  <div class="col-md-12">
                     <div class="form-group">
                        <asp:TextBox MaxLength="14" placeholder="Contact Number" class="form-control" name="contact" ID="txtContactNo" runat="server" />
                        <span id="spnContactNo" class="lbl-red"></span>
                     </div>
                  </div>
                  <div class="col-md-12">
                     <div class="form-group">
                        <asp:TextBox MaxLength="200" placeholder="Company Name" class="form-control" name="companyName" ID="txtCompanyName" runat="server" />
                        <span id="spnCompanyName" class="lbl-red"></span>
                     </div>
                  </div>
                  <div class="col-md-12">
                     <div class="form-group">
                        <asp:TextBox MaxLength="50" placeholder="Title" class="form-control" name="title" ID="txtTitle" runat="server" />
                        <span id="spnTitle" class="lbl-red"></span>
                     </div>
                  </div>
                  <div class="col-md-12">
                     <div class="form-group">
                        <asp:TextBox MaxLength="300" placeholder="Street Address" class="form-control" name="streetAddress" ID="txtStreetAddress" runat="server" />
                        <span id="spnStreetAddress" class="lbl-red"></span>
                     </div>
                  </div>
                  <span class="clearfix"></span>
                  <div class="col-md-12 no-padding chkStyle">
<%--                     <div class="form-group common-checkbox m-t0">
                        <asp:CheckBox ID="CheckBoxSubscriptionAgreement" runat="server" CssClass="checkbox-custom" name="checkbox-1" type="checkbox"/>
                        <label class="control-label" for="CheckBox">Subscription Agreement</label>
                        <span id="spnSubscriptionAgreement" class="lbl-red"></span>
                     </div>--%>

       <%--               <div class="common-checkbox">
                      <input id="CheckBoxSubscriptionAgreement" class="checkbox-custom" name="checkbox-1" type="checkbox"/>
                       <label for="CheckBoxSubscriptionAgreement" class="checkbox-custom-label"> <a href="SubscriptionAgreement.aspx"><a href="SubscriptionAgreement.aspx">Subscription Agreement</a> </a></label>
                    </div>--%>
                  </div>
                     
                  <div class="col-md-12">
                     <div class=" text-center login-btn">
                        <%--<button title="Cancel" class="btn btn-success fr mrg" onclick="return redirect();">Cancel</button>--%>
                        <asp:Button UseSubmitBehavior="true" ID="btnSignup" runat="server" ToolTip="Register" Text="REGISTER" class="btn btn-primary"  OnClick="btnSignup_Click" OnClientClick="return Validation()" />
                        <%--<a title="Cancel"class="btn btn-default"  href="Login.aspx" >Cancel</a>--%>
                        <span class="clr"></span>
                         <p class="m-t15"><a href="login.aspx" class="btn">
                        <i aria-hidden="true" class="fa fa-arrow-circle-o-left"></i> Back to Login</a></p>
                          
                     </div>
                  </div>
              </div>
               </div>
               <span class="clearfix"></span>
            </div>
         </div>
         <div class="col-lg-7 col-md-7 col-xs-12 hide-for-small p-r0">
            <div class="account-right-bg device-height-vh">
               <div class="banner-content">
                  <div><a href="Home.aspx" title="BOB"><img src="Images/logo.png" alt="logo"/></a> </div>
                   <h1 class="p-b0">
                     <span class="green">Fast</span>, <span class="orange">Easy</span> and <span class="yellow">Affordable</span> Trademark Searches
                  </h1>
               </div>
            </div>
         </div>
      </div>
   </div>

   <script type="text/javascript">
      function redirect() {
          window.location.href = 'users.aspx';
          return false;
      }     
      
      function RedirectToOtherTab() {
          window.open('/SubscriptionAgreement.aspx', '_newtab')
          //window.location.href = '/Contact.aspx';
      }
      function UserOrEmailAvailability() { //This function call on text change.
          var email = $("#<%=txtEmail.ClientID%>")[0].value;
          var re = /^\w+([-+.'][^\s]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
          var emailFormat = re.test($("#<%=txtEmail.ClientID%>")[0].value);
          // $('#spnEmail').html("");
          if (emailFormat) {
              $.ajax({
                  url: 'AjaxHandler.aspx?status=CheckEmail&emailId=' + email, // this for calling the web method function in cs code. 
                  // data: '{useroremail: "' + $("#<%=txtEmail.ClientID%>")[0].value + '" }',// user name or email value 
                  dataType: 'text',
                  success:
                      function (result) {
                          switch (result) {
                              case "true":
                                  var str = "Email is not available"
                                  $("#spnEmail").html(str);
                                  document.getElementById('spnEmail').className = 'lbl-red';
                                  $("#<%=hndStatus.ClientID%>").val("false");
                                  break;
                              case "false":
                                  var str = "Email is available"
                                  $("#spnEmail").html(str);
                                  document.getElementById('spnEmail').className = 'successmsg';
                                  $("#<%=hndStatus.ClientID%>").val("true");
                                  break;
                          }
                      }
              });
              }
              else {
              $("#<%=hndStatus.ClientID%>").val("false");
              document.getElementById('spnEmail').className = 'lbl-red';
              var str = "Invalid email format";
              $("#spnEmail").html(str);
              
          }
      }
      
      // function OnSuccess 
      // jquery validation for the form on button click
      
      function Validation() {
          var isValid = "1";
          $('#spnFirstName').html("");
          $('#spnLastName').html("");
      
          $('#spnConfirmpassword').html("");
          $('#spnPassword').html("");
          // $('#spnEmail').html("");
          $('#spnContactNo').html("");
          $('#spnSubscriptionAgreement').html("");          
      
          if ($("#<%=txtFirstName.ClientID%>").val() == "") {
              $('#spnFirstName').append("Required!");
              isValid = "0";
          }
      
          if ($("#<%=txtLastName.ClientID%>").val() == "") {
              $('#spnLastName').append("Required!");
              isValid = "0";
          }
      
          if ($('#<%=txtPassword.ClientID %>').val() == "") {
              $('#spnPassword').append("Required!");
              isValid = "0";
          }          
          else {
              // var regex = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/;
              var regex = /^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$/;
              var passwordFormat = regex.test($("#<%=txtPassword.ClientID %>")[0].value);
      
              if (passwordFormat) {
              }
              else {
                  isValid = "0";
                  $('#spnPassword').append("Please enter alpha numeric with special character password with minimum 8 characters");
              }
          }
          if ($('#<%=txtConfirmpassword.ClientID %>').val() == "") {
              $('#spnConfirmpassword').append("Required!");
              isValid = "0";
          }
          else
              if ($('#<%=txtConfirmpassword.ClientID %>').val() != $('#<%=txtPassword.ClientID %>').val()) {
                  $('#spnConfirmpassword').append("Password and confirm password should be same.")
                  isValid = "0";
              }
      
          if ($('#<%=txtEmail.ClientID %>').val() == "") {
              
              $('#spnEmail').text("");
              var str = "Required!";
              $('#spnEmail').append(str);
              document.getElementById('spnEmail').className = 'lbl-red';
              isValid = "0";
          }
      
          if ($('#<%=txtContactNo.ClientID %>').val() == "") {
              $('#spnContactNo').append("Required!");
              isValid = "0";
          }
      
          <%--console.log($('#<%=CheckBoxSubscriptionAgreement.ClientID %>').is(':checked'));
          if ($('#<%=CheckBoxSubscriptionAgreement.ClientID %>').is(':checked') == false) {
              $('#spnSubscriptionAgreement').append("Required!");
              isValid = "0";
          }--%>
      
          var a;
      
          if (isValid == "1" && $("#<%=hndStatus.ClientID%>").val() == "true") { return true; } else { return false; }
      }
   </script>
</asp:Content>