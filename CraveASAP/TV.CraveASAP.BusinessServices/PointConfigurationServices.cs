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
    public class PointConfigurationServices : IPointConfigurationServices
    {
        private readonly UnitOfWork _unitOfWork;
        public PointConfigurationServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public IEnumerable<PointsConfigurationEntity> GetAllPoint()
        {
            var points = _unitOfWork.PointConfigurationRepository.GetAll().ToList();
            if (points.Any())
            {
                Mapper.CreateMap<PointsConfiguration, PointsConfigurationEntity>();
                var PointsConfigurationModel = Mapper.Map<List<PointsConfiguration>, List<PointsConfigurationEntity>>(points);
                return PointsConfigurationModel;
            }
            return null;
        }

        public IEnumerable<PointsConfigurationEntity> UpdatePointsRewards(PointsConfigurationEntity pointsConfigurationEntity)
        {
            var success = false;
            var points = _unitOfWork.PointConfigurationRepository.GetByID(pointsConfigurationEntity.ptConfigurationId);
            if (points != null)
            {
                using (var scope = new TransactionScope())
                {

                    points.limit = pointsConfigurationEntity.limit;
                    points.pointsEarned = Convert.ToInt32(pointsConfigurationEntity.pointsEarned);
                    _unitOfWork.PointConfigurationRepository.Update(points);
                    _unitOfWork.Save();
                    scope.Complete();
                    success = true;

                }
            }
            return null;
        }

        public bool DeletePromotion(int Id)
        {
            var success = false;
            if (Id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var user = _unitOfWork.AdminManageActivePromotionRepository.GetByID(Id);
                    if (user != null)
                    {
                        _unitOfWork.AdminManageActivePromotionRepository.Delete(user);
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



