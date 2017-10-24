using TradeSystem.Framework.Entities;
using TradeSystem.Utilities.Email;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace TradeSystem.Framework.Identity
{
    class AppIdentityDbInitializer : DbMigrationsConfiguration<AppIdentityDbContext>
    {
        public AppIdentityDbInitializer()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AppIdentityDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }
        //string email = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        public void InitializeIdentityForEF(AppIdentityDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            //#endregion
            string _UserName = "Wayne Hellmuth";
            string _Email = "waynehellmuth@tradesystem.com.au";
            string _Password = "Superadmin@123";

            #region Create Default User As a SuperAdmin

            //Create super admin user if it does not exist
            var user = userManager.FindByName(_UserName);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = _Email,
                    Email = _Email,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    LockoutEnabled = false,
                    LockoutEndDateUtc = System.DateTime.UtcNow,
                    AccessFailedCount = 0,
                    TwoFactorEnabled = false
                };
                var result = userManager.Create(user, _Password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            #endregion

            #region Create Email Template

            //this templet for create organisation
            var template = context.EmailTemplates.FirstOrDefault(x =>
                         x.Name == EmailTemplatesHelper.CompanyCreateEmail);
            if (template == null)
            {
                template = new EmailTemplate
                {
                    Id = Guid.NewGuid(),
                    Template = 1,
                    Name = EmailTemplatesHelper.CompanyCreateEmail,
                    CreatedDate = DateTime.UtcNow,
                    OrganisationId = new Guid("55E71506-E241-407C-9521-AEC394F734D4"),
                    Subject = "Create your Organisation in SplitDeals System network.",
                    Body = @"<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    <title>SplitDeals</title>
    <style type='text/css'>
        body {
            margin: 0px;
            padding: 0px;
            background-color: #ffffff;
            color: #777777;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            -webkit-text-size-adjust: 100%;
            -ms-text-size-adjust: 100%;
            width: 100% !important;
        }

        a, a:link, a:visited {
            color: #3f51b5;
            text-decoration: underline;
        }

            a:hover, a:active {
                text-decoration: none;
                color: #125f96 !important;
            }

        h1, h2, h3, h1 a, h2 a, h3 a {
            color: #3f51b5 !important;
        }

        h2 {
            padding: 0px 0px 10px 0px;
            margin: 0px 0px 10px 0px;
        }

            h2.name {
                padding: 0px 0px 7px 0px;
                margin: 0px 0px 7px 0px;
            }

        h3 {
            padding: 0px 0px 5px 0px;
            margin: 0px 0px 5px 0px;
        }

        p {
            margin: 0 0 14px 0;
            padding: 0;
        }

        img {
            border: 0;
            -ms-interpolation-mode: bicubic;
            max-width: 100%;
        }

        a img {
            border: none;
        }

        table td {
            border-collapse: collapse;
        }

        td.quote {
            font-family: Georgia, 'Times New Roman', Times, serif;
            font-size: 18px;
            line-height: 20pt;
            color: #3f51b5;
        }

        span.phone a, span.noLink a {
            color: 2c8fd6;
            text-decoration: none;
        }

        /* Hotmail */
        .ReadMsgBody {
            width: 100%;
        }

        .ExternalClass {
            width: 100%;
        }
        /* / Hotmail */

        /* Media queries */
        @media (max-width: 767px) {
            td[class=shareContainer], td[class=topContainer], td[class=container] {
                padding-left: 20px !important;
                padding-right: 20px !important;
            }

            table[class=row] {
                width: 100% !important;
                max-width: 600px !important;
            }

            img[class=wideImage], img[class=banner] {
                width: 100% !important;
                height: auto !important;
                max-width: 100%;
            }
        }

        @media (max-width: 560px) {
            td[class=twoFromThree] {
                display: block;
                width: 100% !important;
            }

            td[class=inner2], td[class=authorInfo] {
                padding-right: 30px !important;
            }

            td[class=socialIconsContainer] {
                display: block;
                width: 100% !important;
                border-top: 0px !important;
            }

            td[class=socialIcons], td[class=socialIcons2] {
                padding-top: 0px !important;
                text-align: left !important;
                padding-left: 30px !important;
                padding-bottom: 20px !important;
            }
        }

        @media (max-width: 480px) {
            html, body {
                margin-right: auto;
                margin-left: auto;
            }

            td[class=oneFromTwo] {
                display: block;
                width: 100% !important;
            }

            td[class=inner] {
                padding-left: 30px !important;
                padding-right: 30px !important;
            }

            td[class=inner_image] {
                padding-left: 30px !important;
                padding-right: 30px !important;
                padding-bottom: 25px !important;
            }

            img[class=wideImage] {
                width: auto !important;
                margin: 0 auto;
            }

            td[class=viewOnline] {
                display: none !important;
            }

            td[class=date] {
                font-size: 14px !important;
                padding: 10px 30px !important;
                background-color: #f4f4f4;
                text-align: left !important;
            }

            td[class=title] {
                font-size: 24px !important;
                line-height: 32px !important;
            }

            table[class=quoteContainer] {
                width: 100% !important;
                float: none;
            }

            td[class=quote] {
                padding-right: 0px !important;
            }

            td[class=spacer] {
                padding-top: 18px !important;
            }
        }

        @media (max-width: 380px) {
            td[class=shareContainer] {
                padding: 0px 10px !important;
            }

            td[class=topContainer] {
                padding: 10px 10px 0px 10px !important;
                background-color: #e9e9e9 !important;
            }

            td[class=container] {
                padding: 0px 10px 10px 10px !important;
            }

            table[class=row] {
                min-width: 240px !important;
            }

            img[class=wideImage] {
                width: 100% !important;
                max-width: 255px;
            }

            td[class=authorInfo], td[class=socialIcons2] {
                text-align: center !important;
            }

            td[class=spacer2] {
                display: none !important;
            }

            td[class=spacer3] {
                padding-top: 23px !important;
            }

            table[class=iconContainer], table[class=iconContainer_right] {
                width: 100% !important;
                float: none !important;
            }

            table[class=authorPicture] {
                float: none !important;
                margin: 0px auto !important;
                width: 80px !important;
            }

            td[class=icon] {
                padding: 5px 0px 25px 0px !important;
                text-align: center !important;
            }

                td[class=icon] img {
                    display: inline !important;
                }

            img[class=buttonRight] {
                float: none !important;
            }

            img[class=bigButton] {
                width: 100% !important;
                max-width: 224px;
                height: auto !important;
            }

            h2[class=website] {
                font-size: 22px !important;
            }
        }
        /* / Media queries */
    </style>
    <!-- Internet Explorer fix -->
    <!--[if IE]>
    <style type='text/css'>
    @media (max-width: 560px) {
    td[class=twoFromThree], td[class=socialIconsContainer] {float:left; padding:0px;}
    }
    @media only screen and (max-width: 480px) {
    td[class=oneFromTwo] {float:left; padding:0px;}
    }
    @media (max-width: 380px) {
    span[class=phone] {display:block !important;}
    }
    </style>
    <![endif]-->
    <!-- / Internet Explorer fix -->
    <!-- Windows Mobile 7 -->
    <!--[if IEMobile 7]>
    <style type='text/css'>
    td[class=shareContainer], td[class=topContainer], td[class=container] {padding-left:10px !important; padding-right:10px !important;}
    table[class=row] {width:100% !important; max-width:600px !important;}
    td[class=oneFromTwo], td[class=twoFromThree] {float:left; padding:0px; display:block; width:100% !important;}
    td[class=socialIconsContainer] {float:left; padding:0px; display:block; width:100% !important; border-top:0px !important;}
    td[class=socialIcons], td[class=socialIcons2] {padding-top:0px !important; text-align:left !important; padding-left:30px !important; padding-bottom:20px !important;}
    td[class=inner], td[class=inner2], td[class=authorInfo] {padding-left:30px !important; padding-right:30px !important;}
    td[class=inner_image] {padding-left:30px !important; padding-right:30px !important; padding-bottom:25px !important;}
    td[class=viewOnline] {display:none !important;}
    td[class=date] {font-size:14px !important; padding:10px 30px !important; background-color:#f4f4f4; text-align:left !important;}
    td[class=title] {font-size:24px !important; line-height:32px !important;}
    table[class=quoteContainer] {width:100% !important; float:none;}
    td[class=quote] {padding-right:0px !important;}
    td[class=spacer] {padding-top:18px !important;}
    span[class=phone] {display:block !important;}
    img[class=banner] {width:100% !important; height:auto !important; max-width:100%;}
    img[class=wideImage] {width:auto !important; margin:0 auto;}
    </style>
    <![endif]-->
    <!-- / Windows Mobile 7 -->
    <!-- Outlook -->
    <!--[if gte mso 15]>
    <style type='text/css'>
    .iconContainer, .quoteContainer {mso-table-rspace:0px; border-right:1px solid #ffffff;}
    .iconContainer_right {mso-table-rspace:0px; border-right:1px solid #ffffff; padding-right:1px;}
    .authorPicture {mso-table-rspace:0px; border-right:1px solid #f4f4f4;}
    </style>
    <![endif]-->
    <!-- / Outlook -->
</head>
<body>
    <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse;'>
        <!-- Start of view online, tweet and share -->
        <tr>
            <td class='shareContainer' style='padding-left:5px; padding-right:5px; background-color:#3f51b5;'>
                <table class='row' width='600' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'>
                    <tr>
                        <td class='viewOnline' width='50%' style='padding-top:11px; padding-bottom:10px; font-family:Arial, Helvetica, sans-serif; font-size:12px;height:20px;line-height:100%; color:#ffffff;'></td>
                        <td class='share' width='50%' style='height:20px;padding-top:10px; padding-bottom:10px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:100%; color:#ffffff; text-align:right;'></td>
                    </tr>
                </table>
            </td>
        </tr>
        <!-- End of view online, tweet and share -->
        <!-- Start of logo and date -->
        <tr>
            <td class='topContainer' style='padding-left:5px; padding-right:5px; background-color:#3f51b5;'>

                <table class='row' width='600' bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'>
                    <tr>
                        <td class='oneFromTwo' width='50%' valign='middle'>
                            <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'>
                                <tr>
                                    <td class='inner'>
                                        <a href='#' title='SplitDeals' style='display:block; margin-top:30px; margin-right:15px; margin-bottom:30px; margin-left:28px;font-family:'Segoe UI', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size:24px; line-height:100%; color:#3f51b5; font-weight:normal;'><img width='200' alt='SplitDeals' src='http://183.182.84.29//SplitDeals/Content/images/Logo-splitdeals.png' border='0' align='left' vspace='0' hspace='0' style='display:block;margin-bottom:30px;' /></a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
        <!-- End of logo and date -->
        <!-- Start of main container -->
        <tr>
            <td class='container' style='padding-left:5px; padding-right:5px; padding-bottom:20px; background-color:#e9e9e9;'>
                <!-- Start of text with picture on the left -->
                <table class='row' width='600' bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'>
                    <tr>
                        <td width='100%' valign='top'>
                            <table width='580' class='deviceWidth' border='0' cellpadding='0' cellspacing='0' align='center'>
                                <tr>
                                    <td valign='top' style='padding:0' bgcolor='#ffffff'></td>
                                </tr>
                                <tr>
                                    <td style='font-size: 13px; color:#333; font-weight: normal; text-align: left; font-family:Arial; line-height: 24px; vertical-align: top; padding:0px 20px 25px'>
                                        <table border='0' cellpadding='0' cellspacing='0'>
                                            <tr>
                                                <td valign='middle' style='padding:0 10px 0px 0'>
                                                    <h3 style='text-decoration: none; color: #272727 !important; font-size:14px;font-weight: bold; font-family:Arial;margin:0px;padding:0px;'>Hello <%FirstName%> <%LastName%>,</h3>
                                                </td>
                                            </tr>
                                            <tr><td height='20'></td></tr>
                                            <tr>
                                                <td>
                                                    <p style='color:#333;font-size:13px;margin-bottom:5px;'>Thanks for choosing SplitDeals! Welcome to SplitDeals Organisation.</p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <p style='color:#333;font-size:13px;margin-top:0px;margin-bottom:2px;'>You can access your account from using the login details below: -</p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table border='0' cellpadding='0' cellspacing='0'>
                                                        <tr>
                                                            <td width='80'><b style='font-size:13px;'>Email:</b></td>
                                                            <td><%email%></td>
                                                        </tr>
                                                        <tr>
                                                            <td width='80'><b style='font-size:13px;'>Password:</b></td>
                                                            <td><%Password%></td>
                                                        </tr>

                                                    </table>
                                                </td>
                                            </tr>
                                            <td height='20'></td>
                                            <tr>
                                                <td height='60'></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style='font-size:12px;font-weight:bold;'>
                                                        Warm Regards,<br />
                                                        SplitDeals Team.
                                                    </span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <!-- End of text with picture on the left -->
                <!-- Start of footer -->
                <table class='row' width='600' bgcolor='#f4f4f4' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'>
                    <tr>
                        <td class='twoFromThree' width='65%' valign='top'>
                            <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'>
                                <tr>
                                    <td class='inner2' style='padding-top:34px; padding-left:30px; padding-right:15px; padding-bottom:25px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:15pt; color:#777;'>
                                        Copyright &copy; 2017 <a style='text-decoration:none; color:#3f51b5;' href='http://183.182.84.29/SplitDeals/CustomerManagement/Login'> SplitDeals</a>, All rights reserved.
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class='socialIconsContainer' width='35%' valign='bottom' style='border-top:1px #dddddd dotted;'>
                            <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0; min-width:210px;'>
                                <tr>
                                    <td class='socialIcons' style='padding-top:25px; padding-left:15px; padding-right:30px; padding-bottom:25px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:15pt; color:#777777; text-align:right;'>
                                    
                                        <!--  <a href='#'><img alt='Twitter' src='images/twitterIcon.png' border='0' vspace='0' hspace='0' /></a>&nbsp;&nbsp;
                                        <a href='#'><img alt='Google Plus' src='images/googlePlusIcon.png' border='0' vspace='0' hspace='0' /></a>&nbsp;&nbsp;
                                        <a href='#'><img alt='Linkedin' src='images/linkedinIcon.png' border='0' vspace='0' hspace='0' /></a> -->
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <!-- End of footer -->

            </td>
        </tr>
        <!-- End of main container -->
    </table>
</body>
</html>"
                };
                context.EmailTemplates.Add(template);
                context.SaveChanges();
            }

            ////this templet for create user
            template = context.EmailTemplates.FirstOrDefault(x =>
                         x.Name == EmailTemplatesHelper.PromotionCreateEmail);
            if (template == null)
            {
                template = new EmailTemplate
                {
                    Id = Guid.NewGuid(),
                    Template = 2,
                    Name = EmailTemplatesHelper.PromotionCreateEmail,
                    CreatedDate = DateTime.UtcNow,
                    OrganisationId = new Guid("55E71506-E241-407C-9521-AEC394F734D4"),
                    Subject = "<%Subject%>",
                    Body = @"<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    <title>SplitDeals</title>
    <style type='text/css'>
        body {
            margin: 0px;
            padding: 0px;
            background-color: #ffffff;
            color: #777777;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            -webkit-text-size-adjust: 100%;
            -ms-text-size-adjust: 100%;
            width: 100% !important;
        }

        a, a:link, a:visited {
            color: #3f51b5;
            text-decoration: underline;
        }

            a:hover, a:active {
                text-decoration: none;
                color: #125f96 !important;
            }

        h1, h2, h3, h1 a, h2 a, h3 a {
            color: #3f51b5 !important;
        }

        h2 {
            padding: 0px 0px 10px 0px;
            margin: 0px 0px 10px 0px;
        }

            h2.name {
                padding: 0px 0px 7px 0px;
                margin: 0px 0px 7px 0px;
            }

        h3 {
            padding: 0px 0px 5px 0px;
            margin: 0px 0px 5px 0px;
        }

        p {
            margin: 0 0 14px 0;
            padding: 0;
        }

        img {
            border: 0;
            -ms-interpolation-mode: bicubic;
            max-width: 100%;
        }

        a img {
            border: none;
        }

        table td {
            border-collapse: collapse;
        }

        td.quote {
            font-family: Georgia, 'Times New Roman', Times, serif;
            font-size: 18px;
            line-height: 20pt;
            color: #3f51b5;
        }

        span.phone a, span.noLink a {
            color: 2c8fd6;
            text-decoration: none;
        }

        /* Hotmail */
        .ReadMsgBody {
            width: 100%;
        }

        .ExternalClass {
            width: 100%;
        }
        /* / Hotmail */

        /* Media queries */
        @media (max-width: 767px) {
            td[class=shareContainer], td[class=topContainer], td[class=container] {
                padding-left: 20px !important;
                padding-right: 20px !important;
            }

            table[class=row] {
                width: 100% !important;
                max-width: 600px !important;
            }

            img[class=wideImage], img[class=banner] {
                width: 100% !important;
                height: auto !important;
                max-width: 100%;
            }
        }

        @media (max-width: 560px) {
            td[class=twoFromThree] {
                display: block;
                width: 100% !important;
            }

            td[class=inner2], td[class=authorInfo] {
                padding-right: 30px !important;
            }

            td[class=socialIconsContainer] {
                display: block;
                width: 100% !important;
                border-top: 0px !important;
            }

            td[class=socialIcons], td[class=socialIcons2] {
                padding-top: 0px !important;
                text-align: left !important;
                padding-left: 30px !important;
                padding-bottom: 20px !important;
            }
        }

        @media (max-width: 480px) {
            html, body {
                margin-right: auto;
                margin-left: auto;
            }

            td[class=oneFromTwo] {
                display: block;
                width: 100% !important;
            }

            td[class=inner] {
                padding-left: 30px !important;
                padding-right: 30px !important;
            }

            td[class=inner_image] {
                padding-left: 30px !important;
                padding-right: 30px !important;
                padding-bottom: 25px !important;
            }

            img[class=wideImage] {
                width: auto !important;
                margin: 0 auto;
            }

            td[class=viewOnline] {
                display: none !important;
            }

            td[class=date] {
                font-size: 14px !important;
                padding: 10px 30px !important;
                background-color: #f4f4f4;
                text-align: left !important;
            }

            td[class=title] {
                font-size: 24px !important;
                line-height: 32px !important;
            }

            table[class=quoteContainer] {
                width: 100% !important;
                float: none;
            }

            td[class=quote] {
                padding-right: 0px !important;
            }

            td[class=spacer] {
                padding-top: 18px !important;
            }
        }

        @media (max-width: 380px) {
            td[class=shareContainer] {
                padding: 0px 10px !important;
            }

            td[class=topContainer] {
                padding: 10px 10px 0px 10px !important;
                background-color: #e9e9e9 !important;
            }

            td[class=container] {
                padding: 0px 10px 10px 10px !important;
            }

            table[class=row] {
                min-width: 240px !important;
            }

            img[class=wideImage] {
                width: 100% !important;
                max-width: 255px;
            }

            td[class=authorInfo], td[class=socialIcons2] {
                text-align: center !important;
            }

            td[class=spacer2] {
                display: none !important;
            }

            td[class=spacer3] {
                padding-top: 23px !important;
            }

            table[class=iconContainer], table[class=iconContainer_right] {
                width: 100% !important;
                float: none !important;
            }

            table[class=authorPicture] {
                float: none !important;
                margin: 0px auto !important;
                width: 80px !important;
            }

            td[class=icon] {
                padding: 5px 0px 25px 0px !important;
                text-align: center !important;
            }

                td[class=icon] img {
                    display: inline !important;
                }

            img[class=buttonRight] {
                float: none !important;
            }

            img[class=bigButton] {
                width: 100% !important;
                max-width: 224px;
                height: auto !important;
            }

            h2[class=website] {
                font-size: 22px !important;
            }
        }
        /* / Media queries */
    </style>
    <!-- Internet Explorer fix -->
    <!--[if IE]>
    <style type='text/css'>
    @media (max-width: 560px) {
    td[class=twoFromThree], td[class=socialIconsContainer] {float:left; padding:0px;}
    }
    @media only screen and (max-width: 480px) {
    td[class=oneFromTwo] {float:left; padding:0px;}
    }
    @media (max-width: 380px) {
    span[class=phone] {display:block !important;}
    }
    </style>
    <![endif]-->
    <!-- / Internet Explorer fix -->
    <!-- Windows Mobile 7 -->
    <!--[if IEMobile 7]>
    <style type='text/css'>
    td[class=shareContainer], td[class=topContainer], td[class=container] {padding-left:10px !important; padding-right:10px !important;}
    table[class=row] {width:100% !important; max-width:600px !important;}
    td[class=oneFromTwo], td[class=twoFromThree] {float:left; padding:0px; display:block; width:100% !important;}
    td[class=socialIconsContainer] {float:left; padding:0px; display:block; width:100% !important; border-top:0px !important;}
    td[class=socialIcons], td[class=socialIcons2] {padding-top:0px !important; text-align:left !important; padding-left:30px !important; padding-bottom:20px !important;}
    td[class=inner], td[class=inner2], td[class=authorInfo] {padding-left:30px !important; padding-right:30px !important;}
    td[class=inner_image] {padding-left:30px !important; padding-right:30px !important; padding-bottom:25px !important;}
    td[class=viewOnline] {display:none !important;}
    td[class=date] {font-size:14px !important; padding:10px 30px !important; background-color:#f4f4f4; text-align:left !important;}
    td[class=title] {font-size:24px !important; line-height:32px !important;}
    table[class=quoteContainer] {width:100% !important; float:none;}
    td[class=quote] {padding-right:0px !important;}
    td[class=spacer] {padding-top:18px !important;}
    span[class=phone] {display:block !important;}
    img[class=banner] {width:100% !important; height:auto !important; max-width:100%;}
    img[class=wideImage] {width:auto !important; margin:0 auto;}
    </style>
    <![endif]-->
    <!-- / Windows Mobile 7 -->
    <!-- Outlook -->
    <!--[if gte mso 15]>
    <style type='text/css'>
    .iconContainer, .quoteContainer {mso-table-rspace:0px; border-right:1px solid #ffffff;}
    .iconContainer_right {mso-table-rspace:0px; border-right:1px solid #ffffff; padding-right:1px;}
    .authorPicture {mso-table-rspace:0px; border-right:1px solid #f4f4f4;}
    </style>
    <![endif]-->
    <!-- / Outlook -->
</head>
<body>
    <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse;'>
        <!-- Start of view online, tweet and share -->
        <tr>
            <td class='shareContainer' style='padding-left:5px; padding-right:5px; background-color:#3f51b5;'>
                <table class='row' width='600' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'>
                    <tr>
                        <td class='viewOnline' width='50%' style='padding-top:11px; padding-bottom:10px; font-family:Arial, Helvetica, sans-serif; font-size:12px;height:20px;line-height:100%; color:#ffffff;'></td>
                        <td class='share' width='50%' style='height:20px;padding-top:10px; padding-bottom:10px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:100%; color:#ffffff; text-align:right;'></td>
                    </tr>
                </table>
            </td>
        </tr>
        <!-- End of view online, tweet and share -->
        <!-- Start of logo and date -->
        <tr>
            <td class='topContainer' style='padding-left:5px; padding-right:5px; background-color:#3f51b5;'>

                <table class='row' width='600' bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'>
                    <tr>
                        <td class='oneFromTwo' width='50%' valign='middle'>
                            <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'>
                                <tr>
                                    <td class='inner'>
                                        <a href='#' title='SplitDeals' style='display:block; margin-top:30px; margin-right:15px; margin-bottom:30px; margin-left:28px;font-family:'Segoe UI', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size:24px; line-height:100%; color:#3f51b5; font-weight:normal;'><img width='200' alt='SplitDeals' src='http://183.182.84.29//SplitDeals/Content/images/Logo-splitdeals.png' border='0' align='left' vspace='0' hspace='0' style='display:block;margin-bottom:30px;' /></a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
        <!-- End of logo and date -->
        <!-- Start of main container -->
        <tr>
            <td class='container' style='padding-left:5px; padding-right:5px; padding-bottom:20px; background-color:#e9e9e9;'>
                <!-- Start of text with picture on the left -->
                <table class='row' width='600' bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'>
                    <tr>
                        <td width='100%' valign='top'>
                            <table width='580' class='deviceWidth' border='0' cellpadding='0' cellspacing='0' align='center'>
                                <tr>
                                    <td valign='top' style='padding:0' bgcolor='#ffffff'></td>
                                </tr>
                                <tr>
                                    <td style='font-size: 13px; color:#333; font-weight: normal; text-align: left; font-family:Arial; line-height: 24px; vertical-align: top; padding:0px 20px 25px'>
                                        <table border='0' cellpadding='0' cellspacing='0'>
                                            <tr>
                                                <td valign='middle' style='padding:0 10px 0px 0'>
                                                    <h3 style='text-decoration: none; color: #272727 !important; font-size:14px;font-weight: bold; font-family:Arial;margin:0px;padding:0px;'>Hello <%FirstName%> <%LastName%>,</h3>
                                                </td>
                                            </tr>
                                            <tr><td height='20'></td></tr>
                                                     <tr><td height='20'><%Description%></td></tr>                            
                                            <td height='20'></td>
                                            <tr>
                                                <td height='60'></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style='font-size:12px;font-weight:bold;'>
                                                        Warm Regards,<br />
                                                        SplitDeals Team.
                                                    </span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <!-- End of text with picture on the left -->
                <!-- Start of footer -->
                <table class='row' width='600' bgcolor='#f4f4f4' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'>
                    <tr>
                        <td class='twoFromThree' width='65%' valign='top'>
                            <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'>
                                <tr>
                                    <td class='inner2' style='padding-top:34px; padding-left:30px; padding-right:15px; padding-bottom:25px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:15pt; color:#777;'>
                                        Copyright &copy; 2017 <a style='text-decoration:none; color:#3f51b5;' href='#'> SplitDeals</a>, All rights reserved.
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class='socialIconsContainer' width='35%' valign='bottom' style='border-top:1px #dddddd dotted;'>
                            <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0; min-width:210px;'>
                                <tr>
                                    <td class='socialIcons' style='padding-top:25px; padding-left:15px; padding-right:30px; padding-bottom:25px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:15pt; color:#777777; text-align:right;'>
                                    
                                        <!--  <a href='#'><img alt='Twitter' src='images/twitterIcon.png' border='0' vspace='0' hspace='0' /></a>&nbsp;&nbsp;
                                        <a href='#'><img alt='Google Plus' src='images/googlePlusIcon.png' border='0' vspace='0' hspace='0' /></a>&nbsp;&nbsp;
                                        <a href='#'><img alt='Linkedin' src='images/linkedinIcon.png' border='0' vspace='0' hspace='0' /></a> -->
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <!-- End of footer -->

            </td>
        </tr>
        <!-- End of main container -->
    </table>
</body>
</html>"

                };
                context.EmailTemplates.Add(template);
                context.SaveChanges();
            }

            ////this templet for create user
            template = context.EmailTemplates.FirstOrDefault(x =>
                         x.Name == EmailTemplatesHelper.AmountLostEmail);
            if (template == null)
            {
                template = new EmailTemplate
                {
                    Id = Guid.NewGuid(),
                    Template = 3,
                    Name = EmailTemplatesHelper.AmountLostEmail,
                    CreatedDate = DateTime.UtcNow,
                    OrganisationId = new Guid("55e71506-e241-407c-9521-aec394f734d4"),
                    Subject = "Referal Earning Lost Due To InActive",
                    Body = @"<!DOCTYPE html>
                    <html lang='en'>
                     <body>
                        <div style='margin-top:20px; padding:20px; max-width:600px; -webkit-border-radius: 20px;-moz-border-radius: 20px;border-radius: 20px; border: solid 1px #ccc;'>
                            <h1>Thanks for choosing SplitDeals!</h1>
                            <p>Hello <%FirstName%> <%LastName%>,</p>
      
                            <p>You have lost the amount of <%Amount%> because your account is InActive.Please active account getting referal benfits </p> 
                            <p>Thanks,</p>
                            <p><SplitDeals></p>
                        </div>
                    </body>
                    </html>"
                };
                context.EmailTemplates.Add(template);
                context.SaveChanges();
            }

            ////this templet for create user
            template = context.EmailTemplates.FirstOrDefault(x =>
                         x.Name == EmailTemplatesHelper.NewProductEmail);
            if (template == null)
            {
                template = new EmailTemplate
                {
                    Id = Guid.NewGuid(),
                    Template = 4,
                    Name = EmailTemplatesHelper.NewProductEmail,
                    CreatedDate = DateTime.UtcNow,
                    OrganisationId = new Guid("55e71506-e241-407c-9521-aec394f734d4"),
                    Subject = "SplitDeals Launches New Investment!!",
                    Body = @"<!DOCTYPE html>
                    <html lang='en'>
                     <body>
                        <div style='margin-top:20px; padding:20px; max-width:600px; -webkit-border-radius: 20px;-moz-border-radius: 20px;border-radius: 20px; border: solid 1px #ccc;'>
                            <h1>Thanks for choosing SplitDeals!</h1>
                            <p>Hello <%FirstName%> <%LastName%>,</p>
      
                            <p>We have created a new investment named <%InvestemtName%></p> 
                            <p>Thanks,</p>
                            <p><SplitDeals></p>
                        </div>
                    </body>
                    </html>"
                };
                context.EmailTemplates.Add(template);
                context.SaveChanges();
            }

            //this templet for create organisation
            var templates = context.EmailTemplates.FirstOrDefault(x =>
                         x.Name == EmailTemplatesHelper.CustomerCreateEmail);
            if (templates == null)
            {
                templates = new EmailTemplate
                {
                    Id = Guid.NewGuid(),
                    Template = 5,
                    Name = EmailTemplatesHelper.CustomerCreateEmail,
                    CreatedDate = DateTime.UtcNow,
                    OrganisationId = new Guid("55E71506-E241-407C-9521-AEC394F734D4"),
                    Subject = "<%Subject%>",
                    Body = @"<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    <title>SplitDeals</title>
    <style type='text/css'>
        body {
            margin: 0px;
            padding: 0px;
            background-color: #ffffff;
            color: #777777;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            -webkit-text-size-adjust: 100%;
            -ms-text-size-adjust: 100%;
            width: 100% !important;
        }

        a, a:link, a:visited {
            color: #3f51b5;
            text-decoration: underline;
        }

            a:hover, a:active {
                text-decoration: none;
                color: #125f96 !important;
            }

        h1, h2, h3, h1 a, h2 a, h3 a {
            color: #3f51b5 !important;
        }

        h2 {
            padding: 0px 0px 10px 0px;
            margin: 0px 0px 10px 0px;
        }

            h2.name {
                padding: 0px 0px 7px 0px;
                margin: 0px 0px 7px 0px;
            }

        h3 {
            padding: 0px 0px 5px 0px;
            margin: 0px 0px 5px 0px;
        }

        p {
            margin: 0 0 14px 0;
            padding: 0;
        }

        img {
            border: 0;
            -ms-interpolation-mode: bicubic;
            max-width: 100%;
        }

        a img {
            border: none;
        }

        table td {
            border-collapse: collapse;
        }

        td.quote {
            font-family: Georgia, 'Times New Roman', Times, serif;
            font-size: 18px;
            line-height: 20pt;
            color: #3f51b5;
        }

        span.phone a, span.noLink a {
            color: 2c8fd6;
            text-decoration: none;
        }

        /* Hotmail */
        .ReadMsgBody {
            width: 100%;
        }

        .ExternalClass {
            width: 100%;
        }
        /* / Hotmail */

        /* Media queries */
        @media (max-width: 767px) {
            td[class=shareContainer], td[class=topContainer], td[class=container] {
                padding-left: 20px !important;
                padding-right: 20px !important;
            }

            table[class=row] {
                width: 100% !important;
                max-width: 600px !important;
            }

            img[class=wideImage], img[class=banner] {
                width: 100% !important;
                height: auto !important;
                max-width: 100%;
            }
        }

        @media (max-width: 560px) {
            td[class=twoFromThree] {
                display: block;
                width: 100% !important;
            }

            td[class=inner2], td[class=authorInfo] {
                padding-right: 30px !important;
            }

            td[class=socialIconsContainer] {
                display: block;
                width: 100% !important;
                border-top: 0px !important;
            }

            td[class=socialIcons], td[class=socialIcons2] {
                padding-top: 0px !important;
                text-align: left !important;
                padding-left: 30px !important;
                padding-bottom: 20px !important;
            }
        }

        @media (max-width: 480px) {
            html, body {
                margin-right: auto;
                margin-left: auto;
            }

            td[class=oneFromTwo] {
                display: block;
                width: 100% !important;
            }

            td[class=inner] {
                padding-left: 30px !important;
                padding-right: 30px !important;
            }

            td[class=inner_image] {
                padding-left: 30px !important;
                padding-right: 30px !important;
                padding-bottom: 25px !important;
            }

            img[class=wideImage] {
                width: auto !important;
                margin: 0 auto;
            }

            td[class=viewOnline] {
                display: none !important;
            }

            td[class=date] {
                font-size: 14px !important;
                padding: 10px 30px !important;
                background-color: #f4f4f4;
                text-align: left !important;
            }

            td[class=title] {
                font-size: 24px !important;
                line-height: 32px !important;
            }

            table[class=quoteContainer] {
                width: 100% !important;
                float: none;
            }

            td[class=quote] {
                padding-right: 0px !important;
            }

            td[class=spacer] {
                padding-top: 18px !important;
            }
        }

        @media (max-width: 380px) {
            td[class=shareContainer] {
                padding: 0px 10px !important;
            }

            td[class=topContainer] {
                padding: 10px 10px 0px 10px !important;
                background-color: #e9e9e9 !important;
            }

            td[class=container] {
                padding: 0px 10px 10px 10px !important;
            }

            table[class=row] {
                min-width: 240px !important;
            }

            img[class=wideImage] {
                width: 100% !important;
                max-width: 255px;
            }

            td[class=authorInfo], td[class=socialIcons2] {
                text-align: center !important;
            }

            td[class=spacer2] {
                display: none !important;
            }

            td[class=spacer3] {
                padding-top: 23px !important;
            }

            table[class=iconContainer], table[class=iconContainer_right] {
                width: 100% !important;
                float: none !important;
            }

            table[class=authorPicture] {
                float: none !important;
                margin: 0px auto !important;
                width: 80px !important;
            }

            td[class=icon] {
                padding: 5px 0px 25px 0px !important;
                text-align: center !important;
            }

                td[class=icon] img {
                    display: inline !important;
                }

            img[class=buttonRight] {
                float: none !important;
            }

            img[class=bigButton] {
                width: 100% !important;
                max-width: 224px;
                height: auto !important;
            }

            h2[class=website] {
                font-size: 22px !important;
            }
        }
        /* / Media queries */
    </style>
    <!-- Internet Explorer fix -->
    <!--[if IE]>
    <style type='text/css'>
    @media (max-width: 560px) {
    td[class=twoFromThree], td[class=socialIconsContainer] {float:left; padding:0px;}
    }
    @media only screen and (max-width: 480px) {
    td[class=oneFromTwo] {float:left; padding:0px;}
    }
    @media (max-width: 380px) {
    span[class=phone] {display:block !important;}
    }
    </style>
    <![endif]-->
    <!-- / Internet Explorer fix -->
    <!-- Windows Mobile 7 -->
    <!--[if IEMobile 7]>
    <style type='text/css'>
    td[class=shareContainer], td[class=topContainer], td[class=container] {padding-left:10px !important; padding-right:10px !important;}
    table[class=row] {width:100% !important; max-width:600px !important;}
    td[class=oneFromTwo], td[class=twoFromThree] {float:left; padding:0px; display:block; width:100% !important;}
    td[class=socialIconsContainer] {float:left; padding:0px; display:block; width:100% !important; border-top:0px !important;}
    td[class=socialIcons], td[class=socialIcons2] {padding-top:0px !important; text-align:left !important; padding-left:30px !important; padding-bottom:20px !important;}
    td[class=inner], td[class=inner2], td[class=authorInfo] {padding-left:30px !important; padding-right:30px !important;}
    td[class=inner_image] {padding-left:30px !important; padding-right:30px !important; padding-bottom:25px !important;}
    td[class=viewOnline] {display:none !important;}
    td[class=date] {font-size:14px !important; padding:10px 30px !important; background-color:#f4f4f4; text-align:left !important;}
    td[class=title] {font-size:24px !important; line-height:32px !important;}
    table[class=quoteContainer] {width:100% !important; float:none;}
    td[class=quote] {padding-right:0px !important;}
    td[class=spacer] {padding-top:18px !important;}
    span[class=phone] {display:block !important;}
    img[class=banner] {width:100% !important; height:auto !important; max-width:100%;}
    img[class=wideImage] {width:auto !important; margin:0 auto;}
    </style>
    <![endif]-->
    <!-- / Windows Mobile 7 -->
    <!-- Outlook -->
    <!--[if gte mso 15]>
    <style type='text/css'>
    .iconContainer, .quoteContainer {mso-table-rspace:0px; border-right:1px solid #ffffff;}
    .iconContainer_right {mso-table-rspace:0px; border-right:1px solid #ffffff; padding-right:1px;}
    .authorPicture {mso-table-rspace:0px; border-right:1px solid #f4f4f4;}
    </style>
    <![endif]-->
    <!-- / Outlook -->
</head>
<body>
    <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse;'>
        <!-- Start of view online, tweet and share -->
        <tr>
            <td class='shareContainer' style='padding-left:5px; padding-right:5px; background-color:#3f51b5;'>
                <table class='row' width='600' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'>
                    <tr>
                        <td class='viewOnline' width='50%' style='padding-top:11px; padding-bottom:10px; font-family:Arial, Helvetica, sans-serif; font-size:12px;height:20px;line-height:100%; color:#ffffff;'></td>
                        <td class='share' width='50%' style='height:20px;padding-top:10px; padding-bottom:10px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:100%; color:#ffffff; text-align:right;'></td>
                    </tr>
                </table>
            </td>
        </tr>
        <!-- End of view online, tweet and share -->
        <!-- Start of logo and date -->
        <tr>
            <td class='topContainer' style='padding-left:5px; padding-right:5px; background-color:#3f51b5;'>

                <table class='row' width='600' bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'>
                    <tr>
                        <td class='oneFromTwo' width='50%' valign='middle'>
                            <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'>
                                <tr>
                                    <td class='inner'>
                                        <a href='javascript:void(0);' title='SplitDeals' style='display:block; margin-top:30px; margin-right:15px; margin-bottom:30px; margin-left:28px;font-family:'Segoe UI', 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size:24px; line-height:100%; color:#3f51b5; font-weight:normal;'><img width='200' alt='SplitDeals' src='http://183.182.84.29//SplitDeals/Content/images/Logo-splitdeals.png' border='0' align='left' vspace='0' hspace='0' style='display:block;margin-bottom:30px;' /></a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
        <!-- End of logo and date -->
        <!-- Start of main container -->
        <tr>
            <td class='container' style='padding-left:5px; padding-right:5px; padding-bottom:20px; background-color:#e9e9e9;'>
                <!-- Start of text with picture on the left -->
                <table class='row' width='600' bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'>
                    <tr>
                        <td width='100%' valign='top'>
                            <table width='580' class='deviceWidth' border='0' cellpadding='0' cellspacing='0' align='center'>
                                <tr>
                                    <td valign='top' style='padding:0' bgcolor='#ffffff'></td>
                                </tr>
                                <tr>
                                    <td style='font-size: 13px; color:#333; font-weight: normal; text-align: left; font-family:Arial; line-height: 24px; vertical-align: top; padding:0px 20px 25px'>
                                        <table border='0' cellpadding='0' cellspacing='0'>
                                            <tr>
                                                <td valign='middle' style='padding:0 10px 0px 0'>
                                                    <h3 style='text-decoration: none; color: #272727 !important; font-size:14px;font-weight: bold; font-family:Arial;margin:0px;padding:0px;'>Hello <%FirstName%> <%LastName%>,</h3>
                                                </td>
                                            </tr>
                                            <tr><td height='20'></td></tr>
                                            <tr>
                                                <td>
                                                    <p style='color:#333;font-size:13px;margin-bottom:5px;'>Thanks for choosing SplitDeals! Welcome to SplitDeals Organisation.</p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <p style='color:#333;font-size:13px;margin-top:0px;margin-bottom:2px;'>You can access your account from using the login details below when admin will active your account: -</p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table border='0' cellpadding='0' cellspacing='0'>
                                                        <tr>
                                                            <td width='80'><b style='font-size:13px;'>Email:</b></td>
                                                            <td><%email%></td>
                                                        </tr>
                                                        <tr>
                                                            <td width='80'><b style='font-size:13px;'>Password:</b></td>
                                                            <td><%Password%></td>
                                                        </tr>

                                                    </table>
                                                </td>
                                            </tr>
                                            <td height='20'></td>
                                            <tr>
                                                <td height='60'></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style='font-size:12px;font-weight:bold;'>
                                                        Warm Regards,<br />
                                                        SplitDeals Team.
                                                    </span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <!-- End of text with picture on the left -->
                <!-- Start of footer -->
                <table class='row' width='600' bgcolor='#f4f4f4' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'>
                    <tr>
                        <td class='twoFromThree' width='65%' valign='top'>
                            <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'>
                                <tr>
                                    <td class='inner2' style='padding-top:34px; padding-left:30px; padding-right:15px; padding-bottom:25px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:15pt; color:#777;'>
                                        Copyright &copy; 2016 <a style='text-decoration:none; color:#3f51b5;' href='http://183.182.84.29/SplitDeals/'> SplitDeals</a>, All rights reserved.
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class='socialIconsContainer' width='35%' valign='bottom' style='border-top:1px #dddddd dotted;'>
                            <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0; min-width:210px;'>
                                <tr>
                                    <td class='socialIcons' style='padding-top:25px; padding-left:15px; padding-right:30px; padding-bottom:25px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:15pt; color:#777777; text-align:right;'>
                                    
                                        <!--<a href='#'><img alt='Twitter' src='~/images/twitterIcon.png' border='0' vspace='0' hspace='0' /></a>&nbsp;&nbsp;
                                        <a href='#'><img alt='Google Plus' src='~/images/googlePlusIcon.png' border='0' vspace='0' hspace='0' /></a>&nbsp;&nbsp;
                                        <a href='#'><img alt='Linkedin' src='~/images/linkedinIcon.png' border='0' vspace='0' hspace='0' /></a> -->
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <!-- End of footer -->

            </td>
        </tr>
        <!-- End of main container -->
    </table>
</body>
</html>"
                };
                context.EmailTemplates.Add(templates);
                context.SaveChanges();
            }
            #endregion

            #region Create Email Setting Configuration

            var isExistSettings = context.EmailSettings.FirstOrDefault();
            if (isExistSettings == null)
            {
                EmailSetting emailSetting = new EmailSetting()
                {
                    Id = Guid.NewGuid(),
                    SmtpEmail = "noreplytradingsystem@gmail.com",
                    SmtpPassword = "Test@12345",
                    SmtpPort = 587,
                    SmtpServer = "smtp.gmail.com",
                    SmtpUserName = "noreplytradingsystem@gmail.com",
                    IntervelTime = 60,
                    IsActive = true,
                };
                context.EmailSettings.Add(emailSetting);
                context.SaveChanges();
            }

            #endregion
            base.Seed(context);
        }
    }
}