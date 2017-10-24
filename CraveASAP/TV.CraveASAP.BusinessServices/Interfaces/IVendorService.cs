using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.CraveASAP.BusinessEntities;

namespace TV.CraveASAP.BusinessServices.Interfaces
{
    public interface IVendorService
    {
        IEnumerable<SuperAdminLoginEntity> SuperAdminLogin(string emailId, string password);
        IEnumerable<VendorListEntity> GetAllVendors();
        int CreateVendor(VendorEntity vendorEntity);
        string UpdateVendor(int vendorId, VendorEntity vendorEntity);
        bool DeleteVendor(int Id);
        int CreateVendorBranch(VendorBranchEntity vendorBranchEntity);
        IEnumerable<VendorEntity> VendorLogin(string emailId, string password, string deviceToken, string devicePlatform);
        VendorEntity GetVendorById(int ID);
        bool ForgotVendorPassword(string email);
        bool ChangeVendorPassword(int vendorId, string password, string oldPassword);
        IEnumerable<CategoryEntity> GetAllVendorCategory();
        bool VendorLogout(VendorEntity vendorEntity);
        IEnumerable<PromotionCodeEntity> GetPromotionByVendorId(int ID);
        IEnumerable<CategoryMapEntity> GetAllCategoryByCategoryId(int ID);
        IEnumerable<VendorCategoryEntity> GetAllVendorByCategoryId(int categoryId, int userId, string userLocationLat, string userLocationLong);
        IEnumerable<ContentManagementEntity> GetAllVideoWebApp();
        IEnumerable<UserFavouriteEntity> GetRestaurants();
        SubCategoryEntity GetSubCategoryById(int Id);
        int AddSubCategory(SubCategoryEntity subCategoryEntity);
        bool UpdateSubCategory(SubCategoryEntity subCategoryEntity);
        bool DeleteSubCategory(int Id);
        OptionalCategoryEntity GetOptCategoryById(int Id);
        int AddOptionalCategory(OptionalCategoryEntity optCategoryEntity);
        bool UpdateOptCategory(OptionalCategoryEntity optCategoryEntity);
        bool DeleteOptCategory(int Id);
        bool ChangeAdminPassword(int vendorId, string password, string oldPassword);
        //IEnumerable<VendorListEntity> GetVendorsList();

    }
}
