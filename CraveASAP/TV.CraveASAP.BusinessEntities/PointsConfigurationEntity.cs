using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public class PointsConfigurationEntity
    {
        public int ptConfigurationId { get; set; }
        public string action { get; set; }
        public int limit { get; set; }
        public int pointsEarned { get; set; }
        //public string limit1 = { get { return Convert.ToString(limit); } set { Convert.ToString(limit); } }
        //public string pointsEarned1 = Convert.ToString(pointsEarned);
        //public string limit1 { get { return Convert.ToString(limit); } set { Convert.ToString(limit); } }
        //public string pointsEarned1 { get { return Convert.ToString(pointsEarned); } set { Convert.ToString(pointsEarned); } }
    }
}
