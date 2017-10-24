using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.CraveASAP.BusinessEntities;
using TV.CraveASAP.BusinessServices.Interfaces;
using TV.CraveASAP.DataModel.UnitOfWork;
using TV.CraveASAP.DataModel;
using System.Transactions;
using AutoMapper;
using TV.Yokal.BusinessServices.HelperClass;
using System.Web;
using System.Net;

namespace TV.CraveASAP.BusinessServices
{
    public class DynamicHTMLContentServices : IDynamicHTMLContentServices
    {
        private readonly UnitOfWork _unitOfWork;

        public DynamicHTMLContentServices()
        {
            _unitOfWork = new UnitOfWork();

        }

        public IEnumerable<DynamicHTMLContentEntity> GetAllHTMLContent(string page, string content, string lang)
        {
            var HTMLContent = _unitOfWork.DynamicHTMLContentRepository.GetByCondition(d => d.pageName == page.Trim() && d.contentFor == content.Trim() && d.language == lang.Trim()).ToList();
            if (HTMLContent.Any())
            {
                Mapper.CreateMap<DynamicHTMLContent, DynamicHTMLContentEntity>();
                var HTMLContentModel = Mapper.Map<List<DynamicHTMLContent>, List<DynamicHTMLContentEntity>>(HTMLContent);
                return HTMLContentModel;
            }
            return null;
        }

        public IEnumerable<DynamicHTMLContentEntity> GetAllContentByType(string content)
        {
            var HTMLContent = _unitOfWork.DynamicHTMLContentRepository.GetByCondition(d => d.pageName == content.Trim()).ToList();
            if (HTMLContent.Any())
            {
                Mapper.CreateMap<DynamicHTMLContent, DynamicHTMLContentEntity>();
                var HTMLContentModel = Mapper.Map<List<DynamicHTMLContent>, List<DynamicHTMLContentEntity>>(HTMLContent);
                return HTMLContentModel;
            }
            return null;
        }

        public IEnumerable<ConfigurationEntity> GetConfiguration()
        {
            var HTMLContent = _unitOfWork.ConfigurationRepository.GetAll().ToList();
            if (HTMLContent.Any())
            {
                Mapper.CreateMap<Configuration, ConfigurationEntity>();
                var ConfigurationModel = Mapper.Map<List<Configuration>, List<ConfigurationEntity>>(HTMLContent);
                return ConfigurationModel;
            }
            return null;
        }

        public DynamicHTMLContentEntity GetAllHTMLContentById(int Id)
        {
            var HTMLContent = _unitOfWork.DynamicHTMLContentRepository.GetByCondition(d => d.Id == Id).FirstOrDefault();
            if (HTMLContent != null)
            {
                Mapper.CreateMap<DynamicHTMLContent, DynamicHTMLContentEntity>();
                var HTMLContentModel = Mapper.Map<DynamicHTMLContent, DynamicHTMLContentEntity>(HTMLContent);
                return HTMLContentModel;
            }
            return null;
        }

        public int CreateHTMLContent(DynamicHTMLContentEntity HTMLContentEntity)
        {


            using (var scope = new TransactionScope())
            {
                var dynamicHTMLContent = new DynamicHTMLContent
                {
                    pageName = HTMLContentEntity.pageName,
                    language = HTMLContentEntity.language,
                    pageContent = HTMLContentEntity.pageContent,
                    contentFor = HTMLContentEntity.contentFor

                };
                _unitOfWork.DynamicHTMLContentRepository.Insert(dynamicHTMLContent);
                _unitOfWork.Save();

                scope.Complete();


                return dynamicHTMLContent.Id;
            }
        }
        public bool UpdateHTMLContent(int Id, DynamicHTMLContentEntity HTMLContentEntity)
        {
            var success = false;
            if (HTMLContentEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var DynamicHTMLContent = _unitOfWork.DynamicHTMLContentRepository.GetByID(Id);
                    if (DynamicHTMLContent != null)
                    {
                        DynamicHTMLContent.pageName = HTMLContentEntity.pageName;
                        DynamicHTMLContent.language = HTMLContentEntity.language;
                        DynamicHTMLContent.pageContent = HTMLContentEntity.pageContent;
                        DynamicHTMLContent.contentFor = HTMLContentEntity.contentFor;
                        _unitOfWork.DynamicHTMLContentRepository.Update(DynamicHTMLContent);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public bool DeleteHTMLContent(int Id)
        {
            var success = false;
            if (Id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var user = _unitOfWork.UserRepository.GetByID(Id);
                    if (user != null)
                    {
                        _unitOfWork.DynamicHTMLContentRepository.Delete(user);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

       
        public void MakeCallToBossGeoApi()
        {
            string result;
           // var uri = new Uri(@"http://yboss.yahooapis.com/geo/placefinder?country=SE&flags=J&locale=sv_SE&postal=41311");
            var uri = new Uri(@"http://craveservices.techvalens.net");
            var o = new OAuthBase();
            string nonce = o.GenerateNonce();
            var timestamp = o.GenerateTimeStamp();

            string normalizedUrl;
            string normalizedParameters;
            string signature = HttpUtility.UrlEncode(
                o.GenerateSignature(uri,
                                    "yourconsumerkeyhere",
                                    "yourconsumersecrethere", null, null, "GET",
                                    timestamp, nonce, out normalizedUrl,
                                    out normalizedParameters));

            uri = new Uri(normalizedUrl + "?" + normalizedParameters + "&oauth_signature=" + signature);

            using (var httpClient = new WebClient())
            {
                result = httpClient.DownloadString(uri.AbsoluteUri);
            }

            Console.WriteLine(result);
        }
    }
}
