using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Email;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace BorgCivil.Framework.Identity
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
            #region Create Email Template

            ////this templet for forgot password
            var template = context.EmailTemplate.FirstOrDefault(x =>
                         x.Name == EmailTemplatesHelper.ForgotEmail);
            if (template == null)
            {
                template = new EmailTemplate
                {
                    Id = Guid.NewGuid(),
                    Template = 1,
                    Name = EmailTemplatesHelper.ForgotEmail,
                    CreatedDate = DateTime.UtcNow,
                    OrganisationId = new Guid("55E71506-E241-407C-9521-AEC394F734D4"),
                    Subject = "Forgot Password.",
                    Body = @"<!DOCTYPE html>
                    <html lang='en'>
                     <body>
                        <h1>Borg Civil</h1>
                        <div style='margin-top:20px; padding:20px; max-width:600px; -webkit-border-radius: 20px;-moz-border-radius: 20px;border-radius: 20px; border: solid 1px #ccc;'>
                            <h1>Thanks for choosing BorgCivil!</h1>
                            <p>Hello <%FirstName%> <%LastName%>,</p>
                            <p>Welcome to BorgCivil. You can access your account from.</p>   
                                 
                                <p>your login password is <%Password%>.</p> 
                            <p>Thanks,</p>
                            <p>Borg Civil</p>
                        </div>
                    </body>
                    </html>"
                };
                context.EmailTemplate.Add(template);
                context.SaveChanges();
            }

            ////this templet for email attachment
            template = context.EmailTemplate.FirstOrDefault(x =>
                         x.Name == EmailTemplatesHelper.SendAttachment);
            if (template == null)
            {
                template = new EmailTemplate
                {
                    Id = Guid.NewGuid(),
                    Template = 2,
                    Name = EmailTemplatesHelper.SendAttachment,
                    CreatedDate = DateTime.UtcNow,
                    OrganisationId = new Guid("55E71506-E241-407C-9521-AEC394F734D4"),
                    Subject = "List Attachment",
                    Body = @"<!DOCTYPE html>
                    <html lang='en'>
                     <body>
                        <h1>Borg Civil</h1>
                        <div style='margin-top:20px; padding:20px; max-width:600px; -webkit-border-radius: 20px;-moz-border-radius: 20px;border-radius: 20px; border: solid 1px #ccc;'>
                            <h1>Thanks for choosing BorgCivil!</h1>
                            <p>Hello,</p>
                            <p>Welcome to BorgCivil. You have recieved list attachment</p>   
                                 
                             
                            <p>Thanks,</p>
                            <p>Borg Civil</p>
                        </div>
                    </body>
                    </html>"
                };
                context.EmailTemplate.Add(template);
                context.SaveChanges();
            }
            #endregion

            #region Create Email Setting Configuration

            var isExistSettings = context.EmailSetting.FirstOrDefault();
            if (isExistSettings == null)
            {
                EmailSetting emailSetting = new EmailSetting()
                {
                    Id = Guid.NewGuid(),
                    SmtpEmail = "rajat.rathore@systematixindia.com",
                    SmtpPassword = "123",
                    SmtpPort = 587,
                    SmtpServer = "smtp.gmail.com",
                    SmtpUserName = "rajat.rathore@systematixindia.com",
                    IntervelTime = 60,
                    IsActive = true,
                };
                context.EmailSetting.Add(emailSetting);
                context.SaveChanges();
            }

            #endregion
        }

    }
}