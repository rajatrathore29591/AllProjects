using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.CraveASAP.BusinessEntities;

namespace TV.CraveASAP.BusinessServices.Interfaces
{
   public partial interface IUserPrefrenceServices
    {
       IEnumerable<UserPrefrencesEntity> GetUserPrefrenceById(int ID);
        int CreateUserPrefrence(UserPrefrencesEntity UserPrefrencesEntity);
        bool UpdateUserPrefrence(UserPrefrencesEntity UserPrefrencesEntity);
        bool DeleteUserPrefrence(int Id);
        
    }
}
