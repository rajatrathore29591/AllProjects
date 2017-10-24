<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="TradeMark.Home" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Trademark</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width initial-scale=1.0 maximum-scale=1.0 user-scalable=no" />
    <!-- Page title set in pageTitle directive -->
    <!--CDN CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="css/bootstrap.min.css" />
    <link rel="stylesheet" href="css/style.css" />
    <link href="Css/chosen.css" rel="stylesheet" />

</head>
<body>
    <!-- Header Start-->
    <header>
        <nav class="navbar navbar-inverse navbar-fixed-top">
            <div class="container">
                <div class="row">
                    <div class="col-md-7 col-sm-8">
                        <div class="top-subscribe m-text-center">
                            <%--Your Prepaid Searches Never Expire!--%>
                     <button class="btn btn-sml-subscription" title="Buy A Subscription" onclick="location.href='Pricing.aspx'">Buy A Subscription</button>
                        </div>
                    </div>
                    <div class="col-md-5 col-sm-4">
                        <div class="top-right-link text-right m-text-center">
                            <button class="btn btn-moneyback" title="Money Back Guarantee" onclick="location.href='Pricing.aspx'"><i class="fa fa-star" aria-hidden="true"></i><span>Money Back Guarantee</span> <i class="fa fa-star" aria-hidden="true"></i></button>
                        </div>
                    </div>
                </div>
            </div>
        </nav>
        <!--main navigation start-->
        <div class="container nav">
            <div class="row">
                <div class="col-md-12">
                    <div class="navbar navbar-default top-navbar primary-top-navbar">
                        <div class="navbar-header">
                            <h4 style="display: none">Menu</h4>
                            <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1" aria-expanded="false"><span class="sr-only">Toggle navigation</span> <span class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span></button>
                        </div>
                        <!-- Collect the nav links, forms, and other content for toggling -->
                        <div class="collapse navbar-collapse p-r0" id="bs-example-navbar-collapse-1">
                            <ul class="nav navbar-nav large-center">
                                <li><a href="Pricing.aspx" class="dropdown-toggle">Pricing</a>
                                </li>
                                <li><a href="WhySearch.aspx" class="dropdown-toggle">Why Search?</a>
                                </li>
                                <li><a href="WhyBob.aspx" class="dropdown-toggle">Why Bob?</a>
                                </li>
                                <%-- <li> <a href="WhyBob.aspx" class="dropdown-toggle">How Can Bob Help? </a></li>--%>

                                <li><a href="HowBobWorks.aspx" class="dropdown-toggle">How Bob Works</a>
                                </li>
                                <li><a href="TalkTrademarkAttorney.aspx" class="dropdown-toggle">Talk To A Trademark Attorney</a>
                                </li>
                                <li><a href="Login.aspx" class="dropdown-toggle"><i class="fa fa-user"></i> Login </a>
                                </li>
                            </ul>
                        </div>
                        <!-- /.navbar-collapse -->
                    </div>
                </div>
            </div>
        </div>
        <!--main navigation end-->
        <div class="container">
            <div class="row">
                <div class="banner-content">
                    <div><a href="Home.aspx" title="BOB">
                        <img src="Images/logo.png" alt="logo" /></a> </div>
                    <h1>
                        <span class="green">Fast</span>, <span class="orange">Easy</span> and <span class="yellow">Affordable</span> Trademark Searches
                    </h1>
                    <button class="btn btn-primary" title="Register To Search" onclick="window.location='Signup.aspx'">Register To Search</button>
                </div>
            </div>
        </div>
    </header>
    <!-- Header End-->
    <section class="search-content">
        <div class="container">
            <div class="row clearfix">
                <!--Reliable Searching Content Start-->
                <div class="col-md-12 ">
                    <div class="searching-simplified">
                        <h2 class="text-center">Reliable Searching Simplified<sup>&trade;</sup></h2>
                        <p class="text-center m-b15">Do not spend time and money building a brand without first searching the United States Patent and Trademark Office. A proper search requires the evaluation of 13 factors, not merely looking for identical marks. </p>
                        <p class="text-center">You do not have to spend thousands or even hundreds of dollars to get the comfort you need that the name of your business, product, or service is available to you.</p>
                    </div>
                </div>
                <div class="clearfix"></div>
                <!--Reliable Searching Content End-->
                <!--Search input Result Content Strat-->
                <div class="search-view m-text-center">
                    <div class="col-md-6 col-sm-6">
                        <img src="Images/search-img.png" alt="search-img">
                        <h3>Search Input</h3>
                    </div>
                    <div class="col-md-6 col-sm-6">
                        <img src="Images/search-result.png" alt="search-result">
                        <h3>Search Result</h3>
                    </div>
                </div>
                <!--Search input Result Content End-->
            </div>
        </div>
    </section>
    <!--why bob Section Start-->
    <section class="why-bob-section">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h2>Why BOB<sup>&trade;</sup></h2>
                    <p class="subtitle text-center">BOB is a simple, quick, comprehensive, and reliable software application for conducting preliminary trademark searches</p>
                </div>
            </div>
            <!--Bob Circle Content Start-->
            <div class="row whybob-content">

                <div class="col-md-4 col-sm-4">
                    <div class="box">
                        <div class="bob-circle-left text-right m-text-center">
                            <h3>Simple <span>
                                <img src="Images/simple-icon.png" alt="simple-icon"></span></h3>
                            <p>Just enter the mark you are interested in and the goods or services associated with the mark and BOB does the rest...</p>
                            <a href="WhyBob.aspx">READ MORE <i class="fa fa-angle-right" aria-hidden="true"></i></a>
                        </div>
                        <div class="bob-circle-left text-right m-t46 m-text-center">
                            <h3>Comprehensive <span>
                                <img src="Images/comprehensive-icon.png" alt="comprehensive-icon"></span></h3>
                            <p>BOB’s algorithm considers more than just the similarity of the marks. That is because the United States Patent and Trademark...</p>
                            <a href="WhyBob.aspx">READ MORE <i class="fa fa-angle-right" aria-hidden="true"></i></a>
                        </div>
                    </div>
                </div>

                <div class="col-md-4 col-sm-4 p-l0 p-r0">
                    <div class="box">
                        <div class="bob-circle-middle">
                            <img src="Images/bob-circle.png" />
                        </div>
                    </div>


                </div>

                <div class="col-md-4 col-sm-4">
                    <div class="box">

                        <div class="bob-circle-right m-text-center">
                            <h3><span>
                                <img src="Images/clock-icon.png" alt="clock-icon"></span> Quick</h3>
                            <p>BOB will return the search results in seconds...</p>
                            <a href="WhyBob.aspx">READ MORE <i class="fa fa-angle-right" aria-hidden="true"></i></a>
                        </div>
                        <div class="bob-circle-right m-t46 m-text-center">
                            <h3><span>
                                <img src="Images/reliable-icon.png" alt="reliable-icon"></span> Reliable</h3>
                            <p>BOB’S algorithm was developed by a team of trademark experts with over 12 years of experience and includes...</p>
                            <a href="WhyBob.aspx">READ MORE <i class="fa fa-angle-right" aria-hidden="true"></i></a>
                        </div>
                    </div>

                </div>
            </div>

        </div>
    </section>
    <!--Why bob Section End-->

    <!--How bob Section Start-->
    <section class="how-bob-section">
        <div class="container">
            <div class="row">
                <h2>How BOB Can Help</h2>

                <div class="how-can-help clearfix">
                    <ul>
                        <li>
                            <img src="Images/marketing-icon.png" alt="marketing-icon"></li>
                        <li>
                            <div class="heading">Naming Firms</div>
                        </li>
                        <li>
                            <div class="dis">
                                <b>Naming Firms</b> – Your time is too valuable to spend on searching the USPTO database for conflicting marks. Let <b>BOB</b> do the searching so you can focus on dreaming up the next famous brand name - and save you money in the process!
                            </div>
                           <%-- <a href="WhyBob.aspx">READ MORE <i class="fa fa-angle-right" aria-hidden="true"></i></a>--%>
                        </li>
                    </ul>

                    <ul>
                        <li>
                            <img src="Images/business-icon.png" alt="business-icon"></li>
                        <li>
                            <div class="heading">Business</div>
                        </li>
                        <li>
                            <div class="dis"><b>Business</b> – <b>BOB</b> is an affordable alternative to traditional, preliminary trademark searching, and is available 24-7 to provide you with almost instantaneous feedback on the availability of the name you have spent hours thinking about so you can check naming off your To-Do list and be confident in the name you selected.</div>
                           <%-- <a href="WhyBob.aspx">READ MORE <i class="fa fa-angle-right" aria-hidden="true"></i></a>--%>
                        </li>
                    </ul>
                    
                    <ul>
                        <li>
                            <img src="Images/lawyer-icon.png" alt="lawyer-icon"></li>
                        <li>
                            <div class="heading">Lawyers</div>
                        </li>
                        <li>
                            <div class="dis"><b>Lawyers</b> – <b>BOB</b> is your paralegal or associate allowing you to quickly respond to client inquiries about the availability of a mark, freeing you up to spend more time on the substantive legal issues involving your clients.</div>
                           <%-- <a href="WhyBob.aspx">READ MORE <i class="fa fa-angle-right" aria-hidden="true"></i></a>--%>
                        </li>
                    </ul>
                </div>

            </div>
        </div>
    </section>
    <!--How bob Section End-->
    <!--Our Customer Section Start-->
    <section class="our-customer-section">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h2>What Our Customers Are Saying</h2>
                    <div class="single-item">
                        <div>
                            <div class="customers-detail">
                                <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
                                <p class="companyname"><span>Unde Omnis</span><span>ABC Company</span></p>
                            </div>
                        </div>

                        <div>
                            <div class="customers-detail">
                                <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
                                <p class="companyname"><span>Unde Omnis</span><span>ABC Company</span></p>
                            </div>
                        </div>

                        <div>
                            <div class="customers-detail">
                                <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
                                <p class="companyname"><span>Unde Omnis</span><span>ABC Company</span></p>
                            </div>
                        </div>

                    </div>
                </div>

            </div>
        </div>
    </section>
    <!--Our Customer Section End-->

    <!--Subscribe Section Start-->
    <section class="subscribe-section">
        <div class="container">
            <div class="row clearfix">
                <div class="col-md-9 col-sm-7">
                    <p>
                        Try <b>BOB<sup>&trade;</sup></b>, quicker and less expensive than<br />
                        a manual trademark search.
                    </p>
                </div>
                <div class="col-md-3 col-sm-5 text-right">
                    <button class="btn btn-subscription" title="Buy A Subscription">Buy A Subscription</button>
                </div>
            </div>
        </div>
    </section>
    <!--Subscribe Section End-->
    <!-- footer -->
     <footer>
         <div class="container">
            <div class="row row-flex row-flex-none">
               <div class="col-md-4 col-sm-4 col-xs-12">
                  <p><img src="Images/bob-footer-logo.png" alt="footer-logo"/></p>
                  <div class="address">
                  	<p class="first-child">Address:</p>
                  	<p>2156 Cedar Grove Trail, St. Paul,<br/> MN 55122</p>
                    <div class="clearfix">&nbsp;</div>
                  	<p class="first-child">Phone:</p>
                  	<p> </p>
                     <div class="clearfix">&nbsp;</div>
                  	<p class="first-child">Email:</p>
                  	<p><a href="mailto:bob@trademarkbob.com">bob@trademarkbob.com</a></p>
                  </div>

               </div>
               <div class="col-md-8 col-sm-8 contact-info col-xs-12">
                  <div class="footer-link pull-right">
                  	<h3>QUICK LINKS</h3>
                     <div class="col-md-3 col-sm-3 p-l0">
                        <ul>
                           <li><a href="Home.aspx">Home</a></li>
                           <li><a href="Pricing.aspx">Pricing </a></li>
                           <li><a href="WhySearch.aspx">Why Search</a></li>
                        </ul>
                     </div>
                     <div class="col-md-4 col-sm-4 m-p-l0">
                        <ul>
                           <li><a href="WhyBob.aspx">Why Bob</a></li>
                           <%--<li><a href="WhyBob.aspx">How Can Bob Help</a></li>--%>
                           <li><a href="HowBobWorks.aspx">How Bob Works</a></li>
                        </ul>
                     </div>
                      <div class="col-md-5 col-sm-5 m-p-l0">
                        <ul>
                           <li><a href="TalkTrademarkAttorney.aspx">Talk To A Trademark Attorney</a></li>
                           <li><a href="Login.aspx">Login</a></li>
                        </ul>
                     </div>
                     <div class="clearfix"></div>
                    <div class="footer-nav">
                         <hr class="hr bg-gray m-b30" />
                        <a href="https://trademarkbob.wordpress.com/" target="_blank" class="p-l0">Bob&rsquo;s Blog</a><a href="PrivacyPolicy.aspx">Privacy Policy</a><a href="Termscondition.aspx">Terms & Conditions</a><a href="Disclaimer.aspx">Disclaimer</a><a href="AboutUs.aspx">About Us</a><a href="ContactUs.aspx">Contact Us</a> <a href="Sitemap.aspx">Sitemap</a>
                            
                        <p>&copy; 2017 EGW Solutions, LLC  </p>
                     </div>
                  </div>
                  
               </div>
            </div>
         </div>
      </footer>
    <%--<script src="scripts/jquery.min.js"></script>--%>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="Scripts/jquery-1.10.2.js"></script>
    <script src="Scripts/jquery-1.10.2.min.js"></script>
    <script src="Scripts/chosen.jquery.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
    <!-- slick slider -->
    <script type="text/javascript" src="Scripts/slick.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.matchHeight/0.7.0/jquery.matchHeight-min.js"></script>
    <script type="text/javascript" src="Scripts/common.js"></script>

</body>
</html>
