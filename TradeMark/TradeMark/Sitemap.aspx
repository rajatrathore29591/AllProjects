<%@ Page Title="" Language="C#" MasterPageFile="~/StaticPagesMaster.Master" AutoEventWireup="true" CodeBehind="Sitemap.aspx.cs" Inherits="TradeMark.Sitemap" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Disclimer Content Start-->
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="heading">
                    <h2><span>Sitemap</span></h2>
                </div>
                <!--Panel Shadow Box Start-->
                <div class="panel-shadow-box-white">
                    <!--Panel Heading Start-->
                    <%--<div class="panel-heading">
                        <h3>Disclaimer</h3>
                    </div>--%>
                    <!--Panel Heading End-->
                    <!--Panel Body End-->
                    <div class="panel-body-content p-0">
                        <div class="content-info">
                                   <div class="sitemap">
                                  <ul class="wsp-pages-list">
                                      <li class="page_item"><a href="Home.aspx">Home</a></li>
                                    <li class="page_item"><a href="Signup.aspx">Signup</a></li>
                                    <li class="page_item"><a href="Login.aspx">Login</a></li>
                                    <li class="page_item"><a href="Search.aspx">Search Process</a></li>
                                    <li class="page_item"><a href="EditProfile.aspx">My Account</a></li>
                                    <li class="page_item"><a href="history.aspx">History</a></li>
                                    
                                    <li class="page_item"><a href="PrivacyPolicy.aspx">Privacy Policy</a></li>
                                    <li class="page_item"><a href="Termscondition.aspx">Terms &amp; condition</a></li>
                                    <li class="page_item"><a href="Disclaimer.aspx">Disclaimer</a></li>
	                                  <li class="page_item"><a href="Sitemap.aspx">Sitemap</a></li>
                                    <li class="page_item"><a href="AboutUs.aspx">About Us </a></li>
                                    <li class="page_item"><a href="ContactUs.aspx">Contact Us</a></li>
                                    <%--<li class="page_item"><a href="Resources.aspx">Resources</a></li>  --%>             
                                  </ul>              
                                </div>
                        
                            </div>
                    </div>
                    <!--Panel Body End-->
                </div>
                <!--Panel Shadow Box End-->
            </div>
        </div>
    </div>
  <!--Disclimer Content End-->
</asp:Content>
