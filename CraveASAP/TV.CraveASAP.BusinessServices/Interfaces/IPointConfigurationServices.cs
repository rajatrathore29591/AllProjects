﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.CraveASAP.BusinessEntities;

namespace TV.CraveASAP.BusinessServices.Interfaces
{
    public interface IPointConfigurationServices
    {
        IEnumerable<PointsConfigurationEntity> GetAllPoint();
        IEnumerable<PointsConfigurationEntity> UpdatePointsRewards(PointsConfigurationEntity pointsConfigurationEntity);

    }
}