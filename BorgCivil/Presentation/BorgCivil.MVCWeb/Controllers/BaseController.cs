using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace BorgCivil.MVCWeb.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BaseController : ApiController
    {
        // variable used while uploading image as multipart
        public readonly string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/");
        
    }
}


