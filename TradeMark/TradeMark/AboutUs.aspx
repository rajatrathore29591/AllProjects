<%@ Page Title="" Language="C#" MasterPageFile="~/StaticPagesMaster.Master" AutoEventWireup="true" CodeBehind="AboutUs.aspx.cs" Inherits="TradeMark.AboutUs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>About Us- Trademark</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <!--About Content Start-->
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="heading">
                    <h2><span>About Us</span></h2>
                </div>
                <!--Panel Shadow Box Start-->
               <div class="panel-shadow-box-white">
                    <!--Panel Heading Start-->
                    <%--<div class="panel-heading">
                        <h3>About Us</h3>
                    </div>--%>
                    <!--Panel Heading End-->
                    <!--Panel Body End-->
                    <div class="panel-body-content p-0">
                        <div class="content-info">
                         <p>
            BOB is a simple, quick, comprehensive, and reliable software application for conducting preliminary
trademark searches.
        </p>
        <p>
            <b>Simple -</b>
             Just enter the mark you are interested in and the goods or services associated with the mark and
BOB does the rest.
        </p>
        <p><b>Quick -</b> BOB will return the preliminary trademark search results in about ___ seconds.</p>
        <p><b>Comprehensive -</b> BOB’s algorithm considers more than just the similarity of the marks. That is because
the United States Patent and Trademark Office Trademark Manual of Examining Procedure says that (1)
the similarity of the marks; and (2) relatedness of the goods or services are "key considerations in any
likelihood of confusion determination." Another key consideration is the number and nature of similar
marks in use on similar goods (i.e., dilution).</p>
        <p><b>Reliable -</b> BOB’s algorithm was developed by an experienced trademark lawyer with over 10 years of
experience and includes the 3 key considerations to determine whether your proposed mark is likely to
cause confusion with another registered mark or pending application. BOB also searches the same data
used by the United States Patent and Trademark Office.</p>
                        
                            </div>
                    </div>
                    <!--Panel Body End-->
                </div>
                <!--Panel Shadow Box End-->
            </div>
        </div>
    </div>
  <!--About Content End-->

</asp:Content>
