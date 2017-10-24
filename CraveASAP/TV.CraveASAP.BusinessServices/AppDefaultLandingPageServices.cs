using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TV.CraveASAP.BusinessEntities;
using TV.CraveASAP.BusinessServices.Interfaces;
using TV.CraveASAP.DataModel;
using TV.CraveASAP.DataModel.UnitOfWork;

namespace TV.CraveASAP.BusinessServices
{
    public class AppDefaultLandingPageServices : IAppDefaultLandingPageServices
    {
        private readonly UnitOfWork _unitOfWork;
        public AppDefaultLandingPageServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        public IEnumerable<CategoryEntity> GetLandingDetails()
        {
            var landing = _unitOfWork.CategoryRepository.GetAll().ToList();
            if (landing.Any())
            {
                Mapper.CreateMap<Category, CategoryEntity>();
                Mapper.CreateMap<AppDefaultLandingPage, AppDefaultLandingPageEntity>();

                var landingModel = Mapper.Map<List<Category>, List<CategoryEntity>>(landing);

                return landingModel;
            }
            return null;
        }

        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">It should be in 24 hours format</param>
        /// <returns></returns>
        public string GetLandingDetailApp(string time)
        {
            string category = string.Empty;
            string[] requestTime = time.Split(' ');
            int catId = 0;
            var landing = _unitOfWork.AppDefaultLandingPageRepository.GetByCondition(d => d.categoryStatus == "success").ToList();
            if (landing.Any())
            {
                foreach (var item in landing)
                {
                    string[] fromTime = item.fromTime.Split('.');
                    TimeSpan from = new TimeSpan(Convert.ToInt32(fromTime[0]), Convert.ToInt32(fromTime[1]), 0);
                    string[] toTime = item.toTime.Split('.');
                    TimeSpan to = new TimeSpan(Convert.ToInt32(toTime[0]), Convert.ToInt32(toTime[1]), 0);
                    string[] currentTime = time.Split('.');
                    TimeSpan current = new TimeSpan(Convert.ToInt32(currentTime[0]), Convert.ToInt32(currentTime[1]), 0);


                    if (from <= current && to >= current)
                    {
                        catId = item.categoryId;
                    }

                    DateTime dtNow = DateTime.Now;
                    DateTime dtCurrent = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, current.Hours, current.Minutes, current.Seconds);
                    DateTime dtFrom = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, from.Hours, from.Minutes, from.Seconds);
                    DateTime dtTo = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, to.Hours, to.Minutes, to.Seconds);

                    if (IsBetween(dtCurrent, dtFrom, dtTo))
                    {
                        catId = item.categoryId;
                    }
                    
                }
            }
            var selectedCategory = _unitOfWork.CategoryRepository.GetByCondition(x => x.categoryId == catId).FirstOrDefault();
            return selectedCategory.categoryName;
        }

        public string GetLandingDetailTime(string time)
        {
            string category = string.Empty;
            string[] requestTime = time.Split(' ');
            string type = "";
            var landing = _unitOfWork.AppDefaultLandingPageRepository.GetByCondition(s => s.categoryStatus == "success").ToList();
            if (landing.Any())
            {
                foreach (var item in landing)
                {
                    string[] fromTime = item.fromTime.Split('.');
                    TimeSpan from = new TimeSpan(Convert.ToInt32(fromTime[0]), Convert.ToInt32(fromTime[1]), 0);
                    string[] toTime = item.toTime.Split('.');
                    TimeSpan to = new TimeSpan(Convert.ToInt32(toTime[0]), Convert.ToInt32(toTime[1]), 0);
                    string[] currentTime = time.Split('.');
                    TimeSpan current = new TimeSpan(Convert.ToInt32(currentTime[0]), Convert.ToInt32(currentTime[1]), 0);

                    if (from <= current && to >= current)
                    {
                        type = item.type;
                    }
                 
                        DateTime dtNow = DateTime.Now;
                        DateTime dtCurrent = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, current.Hours, current.Minutes, current.Seconds);
                        DateTime dtFrom = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, from.Hours, from.Minutes, from.Seconds);
                        DateTime dtTo = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, to.Hours, to.Minutes, to.Seconds);

                        if (IsBetween(dtCurrent, dtFrom, dtTo))
                        {
                            type = item.type;
                        }

                }
            }
            var selectedCategory = _unitOfWork.AppDefaultLandingPageRepository.GetByCondition(x => x.type == type).FirstOrDefault();
            return selectedCategory.type;

        }
        public bool IsBetween(DateTime time, DateTime startTime, DateTime endTime)
        {
            if (time.TimeOfDay == startTime.TimeOfDay) return true;
            if (time.TimeOfDay == endTime.TimeOfDay) return true;

            if (startTime.TimeOfDay <= endTime.TimeOfDay)
                return (time.TimeOfDay >= startTime.TimeOfDay && time.TimeOfDay <= endTime.TimeOfDay);
            else
                return !(time.TimeOfDay >= endTime.TimeOfDay && time.TimeOfDay <= startTime.TimeOfDay);
        }

        public bool UpdateLandingDetails(int landingPageId)
        {
            var success = false;

            if (landingPageId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var landingPageIdDetails = _unitOfWork.AppDefaultLandingPageRepository.GetByID(landingPageId);

                    if (landingPageIdDetails != null)
                    {
                        if (landingPageIdDetails.categoryStatus == "success")
                        {
                            landingPageIdDetails.categoryStatus = "default";
                            _unitOfWork.AppDefaultLandingPageRepository.Update(landingPageIdDetails);
                            _unitOfWork.Save();
                            scope.Complete();
                            success = true;
                        }
                        else
                        {
                            var getTypeStatus = _unitOfWork.AppDefaultLandingPageRepository.GetByCondition(x => x.type.Equals(landingPageIdDetails.type)).Select(y => y.categoryStatus);

                            var chkdata = getTypeStatus.Where(y => y.Equals("success")).Count();

                            if (chkdata <= 0)
                            {
                                landingPageIdDetails.categoryStatus = "success";
                                _unitOfWork.AppDefaultLandingPageRepository.Update(landingPageIdDetails);
                                _unitOfWork.Save();
                                scope.Complete();
                                success = true;
                            }

                        }

                    }
                }
            }
            return success;
        }

    }
}
