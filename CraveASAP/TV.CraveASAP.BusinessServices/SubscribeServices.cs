using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TV.CraveASAP.BusinessEntities;
using TV.CraveASAP.BusinessServices.Interfaces;
using TV.CraveASAP.DataModel.UnitOfWork;
using TV.CraveASAP.DataModel;

using AutoMapper;
using System.Net.Mail;
using TV.CraveASAP.BusinessServices.HelperClass;
using System.IO;
using System.Web.Hosting;

namespace TV.CraveASAP.BusinessServices
{
    public class SubscribeServices : ISubscribeServices
    {
        private readonly UnitOfWork _unitOfWork;


        public SubscribeServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public IEnumerable<SubscribeEntity> GetAllSubscribeUsers()
        {
            var subscribes = _unitOfWork.SubscribeRepository.GetAll().ToList();
            if (subscribes.Any())
            {
                Mapper.CreateMap<Subscribe, SubscribeEntity>();
                var subscribesModel = Mapper.Map<List<Subscribe>, List<SubscribeEntity>>(subscribes);
                return subscribesModel;
            }
            return null;
        }

        

        public int CreateSubscribeUser(SubscribeEntity subscribeEntity)
        {
            MailMessage message = new MailMessage();
            string Subject = string.Empty;
            string Body = string.Empty;
            string htmlBody = string.Empty;
            using (var scope = new TransactionScope())
            {
                var userExist = _unitOfWork.SubscribeRepository.GetByCondition(d => d.email == subscribeEntity.email.Trim()).FirstOrDefault();
                if (userExist == null)
                {
                    var subscribe = new Subscribe
                    {
                        email = subscribeEntity.email,
                        isSubscribe = subscribeEntity.isSubscribe,
                    };
                    _unitOfWork.SubscribeRepository.Insert(subscribe);
                    _unitOfWork.Save();
                    scope.Complete();




                    /////////////////////////////////////////// Start Sent Mail /////////////////////////////////////////
                   
                    //message = new MailMessage();
                    //Subject = "Eddee welcomes you ! Here's a special Thank You!";
                    //str += "Hi," + Environment.NewLine + Environment.NewLine +
                    //   "Welcome to Eddee Newsletter :)" + Environment.NewLine + Environment.NewLine +
                    //   "We are happy that you decided to join our successful community of Restaurant. Stay tuned for our valuable Updates!" + Environment.NewLine + "Sincerely" + Environment.NewLine +
                    //   "Vendor Support Team";
                    //Body = str ;
                    //message.Subject = Subject;
                    //message.IsBodyHtml = true;
                    //message.Body = Body;
                    //message.To.Add(subscribeEntity.email);
                    //EmailUtility SendEmail = new EmailUtility();
                    //SendEmail.SendMail(Subject, Body, subscribeEntity.email);



                    message = new MailMessage();
                    string str = "";
                    str = "FROM: info@eddee.it" + Environment.NewLine;
                    str += "TO: " + subscribeEntity.email + Environment.NewLine;
                    str += "SUBJECT: Eddee welcomes you ! Here's a special Thank You!" + Environment.NewLine;
                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath("~/App_Data/Template/WelcomeEmail.txt")))
                    {
                        body = reader.ReadToEnd();
                    }

                    str += body;
                    if (File.Exists(@"C:\inetpub\wwwroot\eddee\Pictures\Email_Subscribe.txt"))
                    {
                        File.Delete(@"C:\inetpub\wwwroot\eddee\Pictures\Email_Subscribe.txt");
                    }
                    string path = @"C:\inetpub\wwwroot\eddee\Pictures\Email_Subscribe.txt";
                    File.AppendAllLines(path, new[] { str });

                    FileInfo fi = new FileInfo(@"C:\inetpub\wwwroot\eddee\Pictures\Email_Subscribe.txt");
                    fi.CopyTo(@"C:\inetpub\mailroot\Pickup\Email_Subscribe.txt", true);

                    /////////////////////////////////////////End Sent mail///////////////////////////////////////////////

                    return subscribe.subscribeId;
                }

                return 0;
            }
        }

        public SubscribeEntity GetSubscribeUserById(int id)
        {
            var subscribe = _unitOfWork.SubscribeRepository.GetByID(id);
            if (subscribe != null)
            {
                Mapper.CreateMap<Subscribe, SubscribeEntity>();
                var subscribeModel = Mapper.Map<Subscribe, SubscribeEntity>(subscribe);
                return subscribeModel;
            }
            return null;
        }

        public int GetIdByEmail(string email)
        {
            var subscribe = _unitOfWork.SubscribeRepository.GetSingle(c => c.email.Equals(email));
            int id = 0;
            if (subscribe != null)
            {
                Mapper.CreateMap<Subscribe, SubscribeEntity>();
                var subscribeModel = Mapper.Map<Subscribe, SubscribeEntity>(subscribe);
                id = subscribeModel.subscribeId;
            }
            return id;
        }

        public bool DeleteSubscribeUser(int id)
        {
            var success = false;
            if (id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var user = _unitOfWork.SubscribeRepository.GetByID(id);
                    if (user != null)
                    {

                        _unitOfWork.SubscribeRepository.Delete(user);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }
    }
}
