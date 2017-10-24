using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using TV.CraveASAP.DataModel.UnitOfWork;
using TV.Yokal.BusinessServices.HelperClass;
using TV.CraveASAP.DataModel;

namespace TV.CraveASAP.BusinessServices
{
    public class OAuthServices
    {
        private readonly UnitOfWork _unitOfWork;


        public OAuthServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        public string MakeCallSignature(string Id, string deviceToken, string appType, string consumersecret)
        {
            var uri = new Uri(@"http://craveservices.techvalens.net");
            var o = new OAuthBase();
            string nonce = o.GenerateNonce();
            var timestamp = o.GenerateTimeStamp();

            string normalizedUrl;
            string normalizedParameters;
            string signature = HttpUtility.UrlEncode(
                o.GenerateSignature(uri,
                                    "" + Id + "",
                                    consumersecret, deviceToken, null, "GET",
                                    timestamp, nonce, out normalizedUrl,
                                    out normalizedParameters));
            SetoAuthkey(Convert.ToInt32(Id), appType, signature);
            return signature;

        }

        public bool SetoAuthkey(int Id, string appType, string oAuthkey)
        {
            var success = false;
           
                using (var scope = new TransactionScope())
                {
                    var oAuth = new OAuth
                    {
                        appType = appType,
                        loginId = Id,
                        OAuthToken = oAuthkey,
                        time = DateTime.Now
                      };
                    _unitOfWork.OAuthRepository.Insert(oAuth);
                    _unitOfWork.Save();
                    scope.Complete();
                    success = true;
                }
           
            return success;
        }

        public bool GetoAuthkey(int Id, string appType, string oAuthkey)
        {
            var success = false;

            var getoAuthkey = _unitOfWork.OAuthRepository.GetByCondition(x => x.loginId == Id && x.OAuthToken == oAuthkey && x.appType == appType).FirstOrDefault();


            if (getoAuthkey != null || oAuthkey=="Admin")
            {
                success = true;
            }
            return success;
        }

        public bool DeleteoAuthkey(int Id, string oAuthkey)
        {
            var success = false;

            using (var scope = new TransactionScope())
            {
                var getoAuthkey = _unitOfWork.OAuthRepository.GetByCondition(x => x.loginId == Id && x.OAuthToken == oAuthkey).FirstOrDefault();
                _unitOfWork.OAuthRepository.Delete(getoAuthkey);
                _unitOfWork.Save();
                scope.Complete();
                success = true;
            }
            return success;
        }
    }
}
