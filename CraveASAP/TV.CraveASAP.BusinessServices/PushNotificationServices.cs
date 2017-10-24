using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using TV.CraveASAP.BusinessServices.HelperClass;
using PushSharp;
using PushSharp.Core;
using PushSharp.Android;
using PushSharp.Apple;
using System.Web.Hosting;
using System.Web;
using TV.CraveASAP.BusinessEntities;
using TV.CraveASAP.DataModel.UnitOfWork;
using TV.CraveASAP.BusinessServices.Interfaces;
using TV.CraveASAP.DataModel;
using System.Transactions;
using AutoMapper;

namespace TV.CraveASAP.BusinessServices
{
    public class PushNotificationServices : IPushNotificationServices
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public PushNotificationServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public bool NotificationManual(DeviceEntity deviceEntity)
        {
            bool success = false;
            if (deviceEntity.AppType != null)
            {
                List<DeviceEntity> Device = new List<DeviceEntity>();
                PushNotification pn = new PushNotification();
                var UserDevice = _unitOfWork.UserRepository.GetByCondition(x => x.deviceToken != null && x.deviceToken != "" && x.deviceToken.Length > 30 && x.devicePlatform != null && x.deviceToken != "simulator" && x.devicePlatform != null).Select(z => new UserEntity { deviceToken = z.deviceToken, deviceType = z.devicePlatform }).ToList();
                var VendorDevice = _unitOfWork.VendorRepository.GetByCondition(x => x.deviceToken != null && x.deviceToken != "" && x.deviceToken.Length > 30 && x.devicePlatform != null && x.deviceToken != "simulator" && x.devicePlatform != null).Select(z => new VendorEntity { deviceToken = z.deviceToken, deviceType = z.devicePlatform }).ToList();
                if (deviceEntity.AppType == "User" || deviceEntity.AppType == "Both")
                {
                    Device = new List<DeviceEntity>();
                    foreach (var device in UserDevice)
                    {
                        Device.Add(new DeviceEntity
                        {
                            Device = device.deviceToken,
                            Alert = deviceEntity.Alert,
                            MessageEnglish = deviceEntity.MessageEnglish,
                            DeviceType = device.deviceType,
                            AppType = "User",
                            type = "ManualNotification"
                        });
                    }
                    pn.SendPushNotification(Device);
                }
                if (deviceEntity.AppType == "Vendor" || deviceEntity.AppType == "Both")
                {
                    Device = new List<DeviceEntity>();
                    foreach (var device in VendorDevice)
                    {
                        Device.Add(new DeviceEntity
                        {
                            Device = device.deviceToken,
                            Alert = deviceEntity.Alert,
                            MessageEnglish = deviceEntity.MessageEnglish,
                            DeviceType = device.deviceType,
                            AppType = "Vendor",
                            type = "ManualNotification"
                        });
                    }
                    pn.SendPushNotification(Device);
                }
            }
            return success;
        }

        public bool PredictiveNotication(PredictiveNotificationEntity predictiveNotificationEntity)
        {
            List<DeviceEntity> Device = new List<DeviceEntity>();
            PushNotification pn = new PushNotification();
            bool success = false; int minCatA = 0; int minCatB = 0; int minCatC = 0; int minCatId = 0; List<UserPrefrencesEntity> tempList = new List<UserPrefrencesEntity>();
            var UserDevice = _unitOfWork.UserPromotionRepository.GetAll().Join(_unitOfWork.PromotionCodeRepository.GetAll(), u => u.promotionId, p => p.promotionCodeId, (u, p) => new { u, p }).Select(x => new PromotionCodeEntity { userId = Convert.ToInt32(x.u.userId), promotionCodeId = x.p.categoryId, categoryId = x.p.categoryId }).OrderBy(f => f.userId).ToList();
            var UserGroup = UserDevice.GroupBy(z => z.userId);
            int tempUserId = 0;
            foreach (var listitem in UserDevice)
            {

                if (tempUserId == listitem.userId)
                {
                    if (listitem.categoryId == 1)
                        minCatA++;
                    if (listitem.categoryId == 2)
                        minCatB++;
                    if (listitem.categoryId == 3)
                        minCatC++;
                    tempUserId = listitem.userId;

                    if (minCatA > minCatB)
                    {
                        if (minCatA > minCatC)
                        {
                            minCatId = 3;
                        }
                        else
                        {
                            minCatId = 2;
                        }
                    }
                    else
                    {
                        if (minCatA > minCatC)
                        {
                            minCatId = 3;

                        }
                        else { minCatId = 1; }
                    }
                    tempList.RemoveAt(tempList.Count - 1);
                    tempList.Add(new UserPrefrencesEntity { userId = listitem.userId, categoryId = minCatId });
                }
                else
                {
                    if (listitem.categoryId == 1)
                        minCatA++;
                    if (listitem.categoryId == 2)
                        minCatB++;
                    if (listitem.categoryId == 3)
                        minCatC++;
                    tempUserId = listitem.userId;

                    if (minCatA > minCatB)
                    {
                        if (minCatA > minCatC)
                        {
                            minCatId = 3;
                        }
                        else
                        {
                            minCatId = 2;
                        }
                    }
                    else
                    {
                        if (minCatA > minCatC)
                        {
                            minCatId = 3;

                        }
                        else { minCatId = 1; }
                    }
                    tempList.Add(new UserPrefrencesEntity { userId = listitem.userId, categoryId = minCatId });
                }

            }

            var ids = tempList.Select(x => x.userId).ToList();
            var user = _unitOfWork.UserRepository.GetByCondition(x => ids.Contains(x.userId)).ToList();
            foreach (var item in tempList)
            {
                Random rnd = new Random();
                var promotion = _unitOfWork.PromotionCodeRepository.GetByCondition(c => c.categoryId == item.categoryId).OrderBy(c => rnd.Next()).ToList();

                using (var scope = new TransactionScope())
                {
                    var preuser = new PredictiveNotification
                    {
                        userId = item.userId,
                        sendTime = DateTime.Now,
                        status = false,
                        promotionCodeId = promotion.FirstOrDefault().promotionCodeId,
                        reSendTime = predictiveNotificationEntity.reSendTime

                    };
                    _unitOfWork.PredictiveNotificationRepository.Insert(preuser);
                    _unitOfWork.Save();
                    scope.Complete();

                }
                Device.Add(new DeviceEntity
                {
                    Device = user.Where(x => x.userId == item.userId).FirstOrDefault().deviceToken,
                    Alert = "A New promotion for  you Please use this Promotion Code" + promotion.FirstOrDefault().code,
                    MessageEnglish = "Hi!! New promotion for  you Please use this Promotion Code" + promotion.FirstOrDefault().code,
                    DeviceType = user.Where(x => x.userId == item.userId).FirstOrDefault().devicePlatform,
                    AppType = "User",
                    type = "PredictiveNotication"
                });
            }

            pn.SendPushNotification(Device);
            return success;
        }

        public PredictiveNotificationEntity GetPredictiveNotication()
        {
            PredictiveNotificationEntity NitiEnty = new PredictiveNotificationEntity();
            var predictiveNoti = _unitOfWork.PredictiveNotificationRepository.GetAll().ToList();
            predictiveNoti.Count(x => x.status == true).ToString();
            NitiEnty.success = predictiveNoti.Count(x => x.status == true).ToString();
            NitiEnty.Failure = predictiveNoti.Count(x => x.status == false).ToString();

            return NitiEnty;
        }

    }

}


