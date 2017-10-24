using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.CraveASAP.BusinessEntities;

namespace TV.CraveASAP.BusinessServices.Interfaces
{
    public interface IDynamicHTMLContentServices
    {
        IEnumerable<DynamicHTMLContentEntity> GetAllHTMLContent(string page, string content, string lang);
        int CreateHTMLContent(DynamicHTMLContentEntity HTMLContentEntity);
        bool UpdateHTMLContent(int Id, DynamicHTMLContentEntity HTMLContentEntity);
        DynamicHTMLContentEntity GetAllHTMLContentById(int Id);
        bool DeleteHTMLContent(int Id);
        IEnumerable<ConfigurationEntity> GetConfiguration();
        IEnumerable<DynamicHTMLContentEntity> GetAllContentByType(string content);
    }
}
