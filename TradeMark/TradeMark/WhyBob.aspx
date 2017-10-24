<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WhyBob.aspx.cs" MasterPageFile="~/StaticPagesMaster.Master" Inherits="TradeMark.Pricing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Why Bob - Trademark</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <!--Why Bob Content Start-->
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="heading">
                    <h2><span>Why BOB?</span></h2>
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
                        <%--<p><b>Reliable searching simplified<sup>&trade;</sup></b></p>--%>
                         <p class="subheading text-center m-b30"><b class="first-word-head">BOB</b> is a simple, quick, comprehensive, and reliable software application<br /> for conducting preliminary trademark searches.</p>

                            <div class="search-box">
                                <div class="row row-flex row-flex-none clearfix">
                                    <div class="col-md-10 m-text-center">
                                        <h2>Simple</h2>
                                        <p>Just enter the mark you are interested in and the goods or services associated with the mark and BOB does the rest.</p>
                                            <%--Don’t see a description that reflects your goods or services? Let us know. We’ll add it to our database and give you a free search once it is added.--%>
                                    </div>
                                    <div class="col-md-2 text-center">
                                       <img src="Images/simple-icon.png" alt="simple-icon">
                                    </div>
                                </div>
                            </div>
                            <div >
                                     <div class="search-box searchbox-bdr-bottom">
                                <div class="row row-flex row-flex-none clearfix">
                                    <div class="col-md-2 text-center">
                                         <img src="Images/clock-icon.png" alt="clock-icon">
                                    </div>
                                    <div class="col-md-10 m-text-center">
                                        <h2>QUICK</h2>
                                        <p>BOB will return the search results in seconds.</p>
                                    </div>
                                </div>
                            </div>


                            </div>

                            <div class="search-box">
                                <div class="row row-flex row-flex-none clearfix">
                                    <div class="col-md-10 m-text-center">
                                        <h2>COMPREHENSIVE</h2>
                                        <p>BOB’s algorithm considers more than just the similarity of the marks. That is because the United States Patent and Trademark Office’s Trademark Manual of Examining Procedure says that (1) <span class="doubleUnderline" data-toggle="tooltip" title="" data-original-title="a likelihood of confusion factor that assesses the similarity between two or more trademarks or service marks in terms of sight, sound, and meaning.">the similarity of the marks</span>; and (2) <span class="doubleUnderline" data-toggle="tooltip" title="" data-original-title="a likelihood of confusion factor that assesses whether certain goods or services are likely to be encountered by relevant consumers in the marketplace at the same time.">relatedness of the goods or services</span> are <u>key considerations</u> in any <span class="doubleUnderline" data-toggle="tooltip" title="" data-original-title="a statutory basis for refusing registration of a trademark or service mark or finding infringement because it is likely to cause confusion with a mark or marks already registered or pending before the U.S. Patent and Trademark Office, or that were used first in commerce.">likelihood of confusion</span> determination.” Another key consideration in the likelihood of confusion analysis and included in BOB’s algorithm is the number and nature of similar marks in use on similar goods. In other words, the concept of <span class="doubleUnderline" data-toggle="tooltip" title="" data-original-title="a trademark law concept that lessens the conceptual strength of a trademark or service mark such that extraneous material in a proposed mark may be sufficient to differentiate the proposed trademark or service mark from another trademark or service mark so that consumer confusion is unlikely.">dilution</span>.</p>
                                    </div>
                                    <div class="col-md-2 text-center">
                                       <img src="Images/comprehensive-icon.png" alt="comprehensive-icon">
                                    </div>
                                </div>
                            </div>

                                                        <div class="search-box">
                                <div class="row row-flex row-flex-none clearfix">
                                    <div class="col-md-10 m-text-center">
                                        <h2>RELIABLE</h2>
                                        <p>BOB’S algorithm was developed by any experienced trademark lawyer with over 12 years of experience and includes the 3 key considerations to determine whether your proposed trade name is likely to cause confusion with another federally registered mark or pending application. BOB also searches the same data used by the United States Patent and Trademark Office, which is updated daily.</p>
                                    </div>
                                    <div class="col-md-2 text-center">
                                       <img src="Images/reliable-icon.png" alt="reliable-icon">
                                    </div>
                                </div>
                            </div>
                      </div>
                    </div>
                    <!--Panel Body End-->
                </div>
                <!--Panel Shadow Box End-->
            </div>
        </div>
    </div>
  <!--Why Bob Content End-->

    </asp:Content>
