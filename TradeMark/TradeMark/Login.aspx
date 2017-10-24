<%@ Page Language="C#" MasterPageFile="~/Login.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TradeMark.Login" %>
<%--<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script>
      function Validation() {
      
          var isValid = "1";
         
          var email = $("#<%=txtEmail.ClientID%>")[0].value;
      
          var re = /^\w+([-+.'][^\s]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
          //var re = /^([a-zA-Z0-9.])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
      var emailFormat = re.test($("#<%=txtEmail.ClientID%>")[0].value);
          $("#<%=lblEmailErrorMsg.ClientID%>").html("");
          $("#<%=lblPwdErrorMsg.ClientID%>").html("");
      if ($("#<%=txtPassword.ClientID%>").val() == "") {
        $("#<%=lblPwdErrorMsg.ClientID%>").append("Required!");
              isValid = "0";
          }
          if ($("#<%=txtEmail.ClientID%>").val() == "") {
        $("#<%=lblEmailErrorMsg.ClientID%>").append("Required!");
                  isValid = "0";
      
          }
          else {
              if(emailFormat)
              {
                  
              }
              else {
                  setTimeout("", 4000);
                  $("#<%=lblEmailErrorMsg.ClientID%>").append("Invalid email format");
                  isValid = "0";
              }
          }
      
              if (isValid == "1") { return true; } else { return false; }
      }
      expireCookies();
      //Function to expire cookies
      function expireCookies()
      {
          document.cookie = "visited=yes; expires=" + new Date().toUTCString();
          document.cookie = "CheckboxCheck=yes; expires=" + new Date().toUTCString();
      }
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div class="container-fluid" style="height:100vh">
      <div class="row is-flex login" style="height:100%;">
         <div class="col-lg-5 col-md-5 col-xs-12 ">
            <div class="login-container">
                <h2>Login Here</h2>
                  <div class="panel-bdody">
                      <div class="lbl-red alert alert-danger" id="divMsg" runat="server" visible="false">
                     <asp:Label ID="lblMessage" runat="server" ></asp:Label>
                          </div>
                     <div class="form-group">

                        <asp:TextBox placeholder="example@example.com" class="form-control" name="email" ID="txtEmail" runat="server" />
                        <asp:Label ID="lblEmailErrorMsg" runat="server" CssClass="lbl-red" placeholdar="Email Address"></asp:Label>
                     </div>
                     <div class="form-group">
                        <asp:TextBox maxlength="250" placeholder="******" class="form-control" name="password" ID="txtPassword" runat="server" TextMode="Password" />
                        <asp:Label ID="lblPwdErrorMsg" runat="server" CssClass="lbl-red"></asp:Label>
                     </div>
                     <div class="col-md-7 col-sm-6 col-xs-6 p-l0" style="display: none;">
                        <div class="common-checkbox">
                           <asp:CheckBox ID="chkRememberMe" runat="server" />
                     
                           <label for="rememberLogin" class="checkbox-custom-label">Remember&nbsp;login</label>
                        </div>
                     </div>
                     <div class="col-md-7 col-sm-6  col-xs-6 p-l0">
                       <div class="common-checkbox">
                      <input id="rememberLogin" class="checkbox-custom" name="checkbox-1" type="checkbox"/>
                       <label for="rememberLogin" class="checkbox-custom-label"> Remember me?</label>
                    </div>
                     


                     </div>
                     <div class="col-md-5 col-sm-6  col-xs-6 p-r0 link-forgot-password text-right">
                        <a  class="" href="ForgotUserPassword.aspx">Forgot password?</a>
                     </div>
                     <span class="clearfix"></span>
                     <div class=" text-center login-btn">
                        <!-- <a class="btn btn-success btn-block" href="#/dashboard">Login</a> -->
                        <asp:Button ID="btnLogin" runat="server" Text="Login" ToolTip="Login" class="btn btn-primary" OnClick="btnLogin_Click" OnClientClick="return Validation()" />
                        <!-- start by me test twitter code-->
                        <!-- end by me test code-->
                        <!-- start by me test facebook code-->
                        <!-- end by me test facebook code-->
                        <span class="clr"></span>
                     </div>
                      <p class="new-user text-center">New Users? <a href="Signup.aspx">Register Now</a></p>
                  </div>
                  <span class="clearfix"></span>
                
                  <div><p class="or-txt"><span>Or Login Using</span></p></div>
                <div class="row clearfix">
                                <div class="col-md-6">
                     <div class="login-social">
                        <asp:Button ID="btnFacebookLogin" runat="server" CssClass="login-fb" Text=" " tooltip="Login with Facebook" OnClick="btnFacebookLogin_Click" />

                     </div>
                  </div>
                  <div class="col-md-6">
                     <div class="login-social">
                        <asp:Button ID="btnTwitterLogin" runat="server" CssClass="login-tw" tooltip="Login with Twitter" Text="" OnClick="btnTwitterLogin_Click" />

                     </div>
                  </div>
                </div>
   

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
</asp:Content>