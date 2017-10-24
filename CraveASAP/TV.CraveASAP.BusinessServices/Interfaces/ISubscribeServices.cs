using System;
using System.Collections.Generic;
using TV.CraveASAP.BusinessEntities;

namespace TV.CraveASAP.BusinessServices.Interfaces
{
    public interface ISubscribeServices
    {
        IEnumerable<SubscribeEntity> GetAllSubscribeUsers();
        int CreateSubscribeUser(SubscribeEntity subscribeEntity);
        bool DeleteSubscribeUser(int id);
        SubscribeEntity GetSubscribeUserById(int id);
        int GetIdByEmail(string email);
    }
}
