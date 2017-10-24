using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Transactions;
using TV.CraveASAP.BusinessServices.HelperClass;
using TV.CraveASAP.DataModel.UnitOfWork;
using TV.CraveASAP.BusinessEntities;
using TV.CraveASAP.DataModel;
using TV.CraveASAP.BusinessServices.Interfaces;
using System.Web.Hosting;
using System.IO;
using System.Net.Mail;
using System.Threading;


namespace TV.CraveASAP.BusinessServices
{

    public class AdminReward : IAdminReward
    {
        private readonly UnitOfWork _unitOfWork;
        List<UserEntity> userEntityList = new List<UserEntity>();
        MailMessage message = new MailMessage();
        string Subject = string.Empty;
        string Body = string.Empty;
        bool check;

        public AdminReward()
        {
            _unitOfWork = new UnitOfWork();
        }
        public IEnumerable<RewardEntity> GetALLReward()
        {
            List<RewardEntity> rewardEntity = new List<RewardEntity>();
            var Reward = _unitOfWork.RewardRepository.GetAll().ToList();
            if (Reward.Any())
            {

                foreach (var Item in Reward)
                {
                    rewardEntity.Add(new RewardEntity()
                    {

                        rewardName = Item.rewardName,
                        description = Item.description,
                        platform = Item.platform,
                        code = Item.code,
                        endDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.endDate),
                        expiryHours = Item.expiryHours,
                        isSpecial = Item.isSpecial,
                        nextAvailability = Item.nextAvailability,
                        type = Item.type,
                        usedCount = Item.usedCount,
                        isActive = Item.isActive,
                        language = Item.language,
                        point = Item.point

                    });
                }
                return rewardEntity;
            }
            return rewardEntity;
        }

        public IEnumerable<RewardEntity> GetALLRewardByType(string type)
        {
            List<RewardEntity> rewardEntity = new List<RewardEntity>();
            var Reward = _unitOfWork.RewardRepository.GetByCondition(x => x.type == type.Trim()).ToList();

            if (Reward.Any())
            {
                Mapper.CreateMap<Reward, RewardEntity>().ForMember(d => d.endDate, o => o.MapFrom(s => s.rewardId == null ? "Reward" : String.Format("{0:dd/MM/yyyy h:mm:ss tt}", s.endDate)));
                var rewardsModel = Mapper.Map<List<Reward>, List<RewardEntity>>(Reward);
                return rewardsModel;
            }
            return null;

        }

        public IEnumerable<UserEntity> GetRandomUsers(string num)
        {

            Random rnd = new Random();

            var user = _unitOfWork.UserRepository.GetByCondition(d => d.points >= 1000).OrderBy(c => rnd.Next()).Take(Convert.ToInt32(num)).ToList();
            if (user.Any())
            {

                foreach (var Item in user)
                {
                    userEntityList.Add(new UserEntity()
                    {
                        userId = Item.userId,
                        firstName = Item.firstName,
                        lastName = Item.lastName,
                        profilePicture = Item.profilePicture,
                        email = Item.email,
                        points = Item.points,
                        deviceToken = Item.deviceToken,
                        devicePlatform = Item.devicePlatform

                    });
                }
            }
            return userEntityList;
        }

        public RewardEntity GetRewardById(int ID)
        {
            RewardEntity rewardEntity = new RewardEntity();
            var Reward = _unitOfWork.RewardRepository.GetByID(ID);
            var userCount = _unitOfWork.UserRepository.GetByCondition(d => d.points > 500).Count();
            if (Reward != null)
            {
                rewardEntity.rewardId = Reward.rewardId;
                rewardEntity.rewardName = Reward.rewardName;
                rewardEntity.description = Reward.description;
                rewardEntity.image = Reward.image;
                rewardEntity.platform = Reward.platform;
                rewardEntity.code = Reward.code;
                rewardEntity.endDate = Reward.endDate.ToString();
                rewardEntity.expiryHours = Reward.expiryHours;
                rewardEntity.isSpecial = Reward.isSpecial;
                rewardEntity.nextAvailability = Reward.nextAvailability;
                rewardEntity.type = Reward.type;
                rewardEntity.usedCount = Reward.usedCount;
                rewardEntity.isActive = Reward.isActive;
                rewardEntity.language = Reward.language;
                rewardEntity.hyperLink = Reward.link;
                rewardEntity.point = Reward.point;
                rewardEntity.count = userCount.ToString();
                rewardEntity.link = Reward.link;
            }

            return rewardEntity;
        }

        public RewardEntity GetUserCount()
        {
            RewardEntity rewardEntity = new RewardEntity();
            var userCount = _unitOfWork.UserRepository.GetByCondition(d => d.points >= 1000).Count();
            if (userCount != null)
            {

                rewardEntity.count = userCount.ToString();
            }

            return rewardEntity;
        }

        public int CreateReward(RewardEntity rewardEntity)
        {
            string[] temp = null;
            List<DeviceEntity> Device = new List<DeviceEntity>();
            PushNotification pn = new PushNotification();
            DateTime UTCTime = Convert.ToDateTime(rewardEntity.endDate);
            DateTime dates2 = UTCTime.AddHours(7.0);

            using (var scope = new TransactionScope())
            {
                var Reward = new Reward
                {
                    rewardName = rewardEntity.rewardName,
                    description = rewardEntity.description,
                    platform = rewardEntity.platform,
                    code = rewardEntity.code,
                    endDate = dates2,
                    expiryHours = rewardEntity.expiryHours,
                    isSpecial = rewardEntity.isSpecial,
                    nextAvailability = rewardEntity.nextAvailability,
                    type = rewardEntity.type,
                    usedCount = rewardEntity.usedCount,
                    isActive = rewardEntity.isActive,
                    language = rewardEntity.language,
                    point = rewardEntity.point,
                    link = rewardEntity.link
                };
                _unitOfWork.RewardRepository.Insert(Reward);
                _unitOfWork.Save();
                var AdminReward1 = _unitOfWork.RewardRepository.GetAll().LastOrDefault();
                if (rewardEntity.temp != null)
                {
                    temp = rewardEntity.temp.Split(',');
                }
                if (temp != null)
                {
                    var i = 0;

                    foreach (var item in temp)
                    {
                        if (temp[i].ToString() != "undefined")
                        {
                            int id = Convert.ToInt32(temp[i].ToString());
                            var user = _unitOfWork.UserRepository.GetByCondition(x => x.userId == id).FirstOrDefault();

                            Device.Add(new DeviceEntity
                            {
                                Device = user.deviceToken,
                                Alert = "Reward",
                                MessageEnglish = rewardEntity.message + "  " + rewardEntity.link,
                                DeviceType = user.devicePlatform,
                                type = "Reward",
                                AppType = "User",
                               
                            });
                            message = new MailMessage();


                            string str = "";
                            str = "FROM: info@eddee.it" + Environment.NewLine;
                            str += "TO: " + user.email + Environment.NewLine;
                            str += "SUBJECT: Thank you ! Eddee has sent a special gift for you !" + Environment.NewLine;
                            str += "Hi " + user.firstName + "," + Environment.NewLine + Environment.NewLine +
                                //@"<table border='0' cellpadding='0' cellspacing='0'><tr><td><img src='http://eddee.cloudapp.net/eddee/Pictures/'"+user.profilePicture+"'/></td></tr></table>"
                                "Please visit the below link and contact the Eddee team for further details." + Environment.NewLine + rewardEntity.link + "" + Environment.NewLine
                             + Environment.NewLine + "Sincerely" + Environment.NewLine +
                             "Vendor Support Team";
                            if (File.Exists(@"C:\inetpub\wwwroot\eddee\Pictures\Email_RewardC.html"))
                            {
                                File.Delete(@"C:\inetpub\wwwroot\eddee\Pictures\Email_RewardC.html");
                            }
                            string path = @"C:\inetpub\wwwroot\eddee\Pictures\Email_RewardC.html";
                            File.AppendAllLines(path, new[] { str });

                            FileInfo fi = new FileInfo(@"C:\inetpub\wwwroot\eddee\Pictures\Email_RewardC.html");
                            fi.CopyTo(@"C:\inetpub\mailroot\Pickup\Email_RewardC.html", true);



                            //Subject = "Thank you ! Eddee has sent a special gift for you !";
                            //Body = @"<table border='0' cellpadding='0' cellspacing='0'><tr><td>Hello" + user.firstName +
                            //",<br/><br/> Reward notification </b></br><br/>" +
                            //  "Eddee premium Rewards</td></tr></table>";
                            //message.Subject = Subject;
                            //message.IsBodyHtml = true;
                            //message.Body = Body;
                            //message.To.Add(new MailAddress(user.email));
                            //message.From = new MailAddress("rajat.rathore@techvalens.com");
                            //EmailUtility SendEmail = new EmailUtility();
                            //SendEmailInBackgroundThread(message);
                        }
                        i = i + 1;
                    }
                    pn.SendPushNotification(Device);

                }
                using (var scopes = new TransactionScope())
                {
                    var rewardID = _unitOfWork.RewardRepository.GetByID(Reward.rewardId);
                    if (rewardEntity.image != null)
                    {
                        Reward.image = base64ToImage(rewardEntity.image, rewardID.rewardId, rewardID.image);
                        _unitOfWork.RewardRepository.Update(Reward);
                        _unitOfWork.Save();
                        scopes.Complete();
                    }
                }
                scope.Complete();
                return Reward.rewardId;
            }
        }

        public string base64ToImage(string logImg, int MaxId, string oldImage)
        {
            string filePath = string.Empty;
            string image = string.Empty;
            var path = DateTime.Now;
            var data = String.Format("{0:d/M/yyyy HH:mm:ss}", path);
            data = data.Replace(@"/", "").Trim(); data = data.Replace(@":", "").Trim(); data = data.Replace(" ", String.Empty);
            if (!string.IsNullOrEmpty(logImg))
            {
                filePath = HostingEnvironment.MapPath("~/Pictures/");
                image = MaxId + "_RewardImgpic_" + data + ".png";
                if (File.Exists(filePath + oldImage))
                {
                    System.IO.File.Delete((filePath + oldImage));
                }
                byte[] bytes = System.Convert.FromBase64String(logImg);
                FileStream fs = new FileStream(filePath + image, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
            return image;
        }

        public bool UpdateReward(RewardEntity rewardEntity)
        {
            string[] temp = null;
            List<DeviceEntity> Device = new List<DeviceEntity>();
            PushNotification pn = new PushNotification();
            var success = false;
            DateTime UTCTime = Convert.ToDateTime(rewardEntity.endDate);
            DateTime dates2 = UTCTime.AddHours(7.0);
            if (rewardEntity != null)
            {
                var AdminReward1 = _unitOfWork.RewardRepository.GetByID(rewardEntity.rewardId);
                using (var scope = new TransactionScope())
                {
                    if (AdminReward1 != null)
                    {
                        AdminReward1.rewardName = rewardEntity.rewardName;
                        AdminReward1.description = rewardEntity.description;
                        AdminReward1.platform = rewardEntity.platform;
                        AdminReward1.code = rewardEntity.code;
                        AdminReward1.endDate = dates2;
                        AdminReward1.expiryHours = rewardEntity.expiryHours;
                        AdminReward1.isSpecial = rewardEntity.isSpecial;
                        AdminReward1.nextAvailability = rewardEntity.nextAvailability;
                        AdminReward1.type = rewardEntity.type;
                        AdminReward1.usedCount = rewardEntity.usedCount;
                        AdminReward1.isActive = rewardEntity.isActive;
                        AdminReward1.language = rewardEntity.language;
                        AdminReward1.link = rewardEntity.link;
                        AdminReward1.point = rewardEntity.point;
                        _unitOfWork.RewardRepository.Update(AdminReward1);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
                if (rewardEntity.temp != null)
                {
                    temp = rewardEntity.temp.Split(',');
                }
                if (temp != null)
                {
                    var i = 0;


                    foreach (var item in temp)
                    {
                        if (temp[i].ToString() != "undefined")
                        {
                            var user = _unitOfWork.UserRepository.GetByCondition(d => d.userId == Convert.ToInt32(temp[i])).FirstOrDefault();

                            Device.Add(new DeviceEntity
                            {
                                Device = user.deviceToken,
                                Alert = rewardEntity.message,
                                DeviceType = user.devicePlatform,
                                AppType = "User"
                            });
                            message = new MailMessage();

                            string str = "";
                            str = "FROM: info@eddee.it" + Environment.NewLine;
                            str += "TO: " + user.email + Environment.NewLine;
                            str += "SUBJECT: Thank you ! Eddee has sent a special gift for you !" + Environment.NewLine;
                            str += "Hi " + user.firstName + "," + Environment.NewLine + Environment.NewLine +
                                //@"<table border='0' cellpadding='0' cellspacing='0'><tr><td><img src=http://eddee.cloudapp.net/eddee/Pictures/" + user.profilePicture + "/></td></tr></table>"
                                  "Please visit the below link and contact the Eddee team for further details." + Environment.NewLine + rewardEntity.link + Environment.NewLine
                             + Environment.NewLine + "Sincerely" + Environment.NewLine +
                             "Vendor Support Team";
                            if (File.Exists(@"C:\inetpub\wwwroot\eddee\Pictures\Email_RewardU.html"))
                            {
                                File.Delete(@"C:\inetpub\wwwroot\eddee\Pictures\Email_RewardU.html");
                            }
                            string path = @"C:\inetpub\wwwroot\eddee\Pictures\Email_RewardU.html";
                            File.AppendAllLines(path, new[] { str });

                            FileInfo fi = new FileInfo(@"C:\inetpub\wwwroot\eddee\Pictures\Email_RewardU.html");
                            fi.CopyTo(@"C:\inetpub\mailroot\Pickup\Email_RewardU.html", true);


                            //Subject = "Reward Notification.";
                            //Body = @"<table border='0' cellpadding='0' cellspacing='0'><tr><td>Hello" + user.firstName +
                            //",<br/><br/> Reward notification </b></br><br/>" +
                            //  "Eddee premium Rewards</td></tr></table>";
                            //message.Subject = Subject;
                            //message.IsBodyHtml = true;
                            //message.Body = Body;
                            //message.To.Add(new MailAddress(user.email));
                            //message.From = new MailAddress("rajat.rathore@techvalens.com");
                            //EmailUtility SendEmail = new EmailUtility();
                            //SendEmailInBackgroundThread(message);
                        }
                        pn.SendPushNotification(Device);
                        i++;
                    }
                }
                using (var scopes = new TransactionScope())
                {
                    var rewardID = _unitOfWork.RewardRepository.GetByID(AdminReward1.rewardId);
                    if (rewardEntity.image != "")
                    {
                        AdminReward1.image = base64ToImage(rewardEntity.image, rewardID.rewardId, rewardID.image);
                        _unitOfWork.RewardRepository.Update(AdminReward1);
                        _unitOfWork.Save();
                        scopes.Complete();
                    }
                }
            }
            return success;
        }

        public void SendEmailInBackgroundThread(MailMessage message)
        {
            EmailUtility SendEmail = new EmailUtility();
            Thread bgThread = new Thread(new ParameterizedThreadStart(SendEmail.SendMailToVendor));
            bgThread.IsBackground = true;
            bgThread.Start(message);
        }

        public bool DeleteReward(int Id)
        {
            var success = false;
            if (Id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var AdminReward = _unitOfWork.RewardRepository.GetByID(Id);
                    if (AdminReward != null)
                    {
                        _unitOfWork.RewardRepository.Delete(AdminReward);
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
