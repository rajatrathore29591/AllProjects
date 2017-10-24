using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.CraveASAP.BusinessEntities;

namespace TV.CraveASAP.BusinessServices.Interfaces
{
    public interface IBannerServices
    {
        IEnumerable<BannersEntity> GetAllBanners(string type, string platfrom, string language);
        int CreateBanner(IEnumerable<BannersEntity> Banner);
        BannersEntity UploadVideo(Stream Uploading, string VidoType);
        bool UpdateBanners(int bannerId, BannersEntity bannerEntity);
        IEnumerable<BannersEntity> GetAllHowItWork(string platform);
        int CreateHowItWork(BannersEntity bannerEntity);
        bool DeleteHowItWork(int id);
        bool DeleteBanners(int id);
       
    }
}
