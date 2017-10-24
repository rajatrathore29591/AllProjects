using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TV.CraveASAP.BusinessEntities;
using TV.CraveASAP.DataModel;
using TV.CraveASAP.DataModel.UnitOfWork;

namespace TV.CraveASAP.BusinessServices.HelperClass
{
    public class ScheduledJobs
    {
        private readonly UnitOfWork _unitOfWork;

        public ScheduledJobs()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Delete all 30 days older promotions
        /// </summary>
        public void DeletePromotionsMonthly()
        {

            PromotionCode pmProperty = new PromotionCode();
            OldPromotionDelete();

            // isActive = false, ispinned = false & datecompare = DateTime.Compare(x.createdDate, DateTime.Now.AddMonths(-1))
        }

        /// <summary>
        /// Deactivate all expired promotions in every 30 minutes
        /// </summary>
        public void DeactivatePromotions()
        {
            PromotionDeActive();
            // isActive = true & 
            // either compare date and check if end date is expired
        }

        public void PredictiveNotification()
        {
            List<DeviceEntity> Device = new List<DeviceEntity>();
            PushNotification pn = new PushNotification();
            var predictiveNoti = _unitOfWork.PredictiveNotificationRepository.GetByCondition(z => z.status == false).ToList();
            var userData = _unitOfWork.UserRepository.GetByCondition(uid => predictiveNoti.Select(p => p.userId).Contains(uid.userId)).ToList();
            var promo = _unitOfWork.PromotionCodeRepository.GetByCondition(p => predictiveNoti.Select(z => z.promotionCodeId).Contains(p.promotionCodeId)).ToList();
            foreach (var item in predictiveNoti)
            {
                Device.Add(new DeviceEntity
                {
                    Device = userData.Where(x => x.userId == item.userId).FirstOrDefault().deviceToken,
                    Alert = "Eddee" + promo.FirstOrDefault().code,
                    MessageEnglish = "Hi!! New promotion for  you Please use this Promotion Code" + promo.FirstOrDefault().code,
                    DeviceType = userData.Where(x => x.userId == item.userId).FirstOrDefault().devicePlatform,
                    AppType = "User",
                    type = "PredictiveNotification"
                });
            }

            pn.SendPushNotification(Device);
        }

        public void PromotionDeActive()
        {
            var promoCode = _unitOfWork.PromotionCodeRepository.GetByCondition(z => z.isActive == true && z.quantity == 0).ToList();
            foreach (var promo in promoCode)
            {
                DateTime dates = Convert.ToDateTime(promo.expiryDate);
                DateTime UTCTime = System.DateTime.UtcNow; DateTime dates2 = UTCTime.AddHours(7.0);
                // DateTime dates2 = DateTime.Now;
                int dateCompare = -2;
                if (promo.expiryDate != null) { dateCompare = DateTime.Compare(dates2, dates); }
                else
                {
                    if (promo.quantity != null)
                        if (promo.quantity == 0)
                        {
                            if (promo.isActive)
                            {
                                using (var scope = new TransactionScope())
                                {
                                    var promoCD = promo; //_unitOfWork.PromotionCodeRepository.GetByID(promo.promotionCodeId);
                                    if (promoCD != null)
                                    {
                                        promoCD.isActive = false;
                                        _unitOfWork.PromotionCodeRepository.Update(promoCD);
                                        _unitOfWork.Save();
                                    }
                                    scope.Complete();
                                }
                            }
                        }
                }

                if (promo != null && dateCompare >= 0)
                {
                    if (promo.isActive)
                    {
                        using (var scope = new TransactionScope())
                        {
                            var promoCD = promo; //_unitOfWork.PromotionCodeRepository.GetByID(promo.promotionCodeId);
                            if (promoCD != null)
                            {
                                promoCD.isActive = false;
                                _unitOfWork.PromotionCodeRepository.Update(promoCD);
                                _unitOfWork.Save();
                            }
                            scope.Complete();
                        }
                    }
                }

            }
        }

        public void OldPromotionDelete()
        {
            var promoCode = _unitOfWork.PromotionCodeRepository.GetAll();
            foreach (var promo in promoCode)
            {
                var matchFound = (DateTime.Now - DateTime.Now).TotalDays < 30;
                int dateCompare = 0; if (promo.expiryDate != null) dateCompare = Convert.ToInt32((DateTime.Now - Convert.ToDateTime(promo.expiryDate)).TotalDays);
                if (promo != null && (promo.quantity > 0 || dateCompare >= 30))
                {
                    if (!promo.isPinned)
                    {
                        using (var scope = new TransactionScope())
                        {
                            var promoCD = _unitOfWork.PromotionCodeRepository.GetByID(promo.promotionCodeId);
                            if (promoCD != null)
                            {
                                promoCD.isDeleted = true;
                                _unitOfWork.PromotionCodeRepository.Update(promoCD);
                                _unitOfWork.Save();
                            }
                            scope.Complete();
                        }
                    }
                }
            }
        }


    }
}
