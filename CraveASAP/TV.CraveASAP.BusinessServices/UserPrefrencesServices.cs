using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Transactions;
using TV.CraveASAP.BusinessServices.HelperClass;
using TV.CraveASAP.DataModel.UnitOfWork;
using TV.CraveASAP.BusinessEntities;
using TV.CraveASAP.DataModel;
using TV.CraveASAP.BusinessServices.Interfaces;


namespace TV.CraveASAP.BusinessServices
{
    public class UserPrefrenceServices : IUserPrefrenceServices
    {
        private readonly UnitOfWork _unitOfWork;


        public UserPrefrenceServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        public IEnumerable<UserPrefrencesEntity> GetUserPrefrenceById(int ID)
        {
            var UserPrefrenceInfo = _unitOfWork.UserPrefrenceRepository.GetByCondition(d => d.userId == ID).ToList();
            var subCat = _unitOfWork.SubCategoryRepository.GetByCondition(d => d.isDeleted == false).ToList();
            var optCat = _unitOfWork.OptionalCategoryRepository.GetByCondition( d => d.isDeleted == false).ToList();
            List<UserPrefrencesEntity> UserPrefrencesList = new List<UserPrefrencesEntity>();
            foreach (var sub in subCat)
            {
                UserPrefrencesList.Add(new UserPrefrencesEntity
                {
                    prefrencesId = sub.subCategoryId,
                    description = sub.subCategoryName,
                    categoryId = sub.categoryId,
                    isActive = UserPrefrenceInfo.Where(d => d.prefrencesId.Equals(sub.subCategoryId) && d.type.Equals("sub")).Count() == 0 ? false : true,
                    type = "sub",
                });
            }
            foreach (var opt in optCat)
            {
                UserPrefrencesList.Add(new UserPrefrencesEntity
                {
                    prefrencesId = opt.optCategoryId,
                    description = opt.optCategoryName,
                    categoryId = opt.categoryId,
                    isActive = UserPrefrenceInfo.Where(d => d.prefrencesId == opt.optCategoryId && d.type == "opt").Count() == 0 ? false : true,
                    type = "opt",
                });
            }
            UserPrefrencesList = UserPrefrencesList.OrderBy(u => u.description).ToList();
            return UserPrefrencesList;
        }

        public int CreateUserPrefrence(UserPrefrencesEntity UserPrefrencesEntity)
        {
            int id = 0;
            var chk = _unitOfWork.UserPrefrenceRepository.GetByCondition(z => z.userId == UserPrefrencesEntity.userId && z.prefrencesId == UserPrefrencesEntity.prefrencesId && z.type.Equals(UserPrefrencesEntity.type)).ToList();
            if (chk.Count() == 0)
            {
                using (var scope = new TransactionScope())
                {
                    var UserPrefrence = new UserPrefrence
                    {
                        userId = UserPrefrencesEntity.userId,
                        categoryId = UserPrefrencesEntity.categoryId,
                        prefrencesId = UserPrefrencesEntity.prefrencesId,
                        type = UserPrefrencesEntity.type,
                        isActive = UserPrefrencesEntity.isActive,

                    };
                    _unitOfWork.UserPrefrenceRepository.Insert(UserPrefrence);
                    _unitOfWork.Save();
                    scope.Complete();
                    id = UserPrefrence.id;

                }
            }
            else
            {
                if (UserPrefrencesEntity.isActive == false)
                {

                    DeleteUserPrefrence(chk.FirstOrDefault().id);
                    id = chk.FirstOrDefault().userId;
                }

            }
            return id;
        }
        public bool UpdateUserPrefrence(UserPrefrencesEntity UserPrefrencesEntity)
        {
            var success = false;
            var checkData = _unitOfWork.UserPrefrenceRepository.GetByCondition(c => c.prefrencesId == UserPrefrencesEntity.prefrencesId && c.userId == UserPrefrencesEntity.userId && c.type == UserPrefrencesEntity.type.Trim() && c.categoryId == UserPrefrencesEntity.categoryId).ToList();
            if (checkData.Count > 0)
            {

                if (UserPrefrencesEntity != null)
                {
                    using (var scope = new TransactionScope())
                    {
                        var UserPrefrence = _unitOfWork.UserPrefrenceRepository.GetByID(UserPrefrencesEntity.id);
                        if (UserPrefrence != null)
                        {
                            UserPrefrence.userId = UserPrefrencesEntity.userId;
                            UserPrefrence.categoryId = UserPrefrencesEntity.categoryId;
                            UserPrefrence.prefrencesId = UserPrefrencesEntity.prefrencesId;
                            UserPrefrence.type = UserPrefrencesEntity.type;
                            UserPrefrence.isActive = UserPrefrencesEntity.isActive;

                            _unitOfWork.UserPrefrenceRepository.Update(UserPrefrence);
                            _unitOfWork.Save();
                            scope.Complete();
                            success = true;
                        }
                    }
                }
            }
            else
            {

                var chk = _unitOfWork.UserPrefrenceRepository.GetByCondition(z => z.userId == UserPrefrencesEntity.userId && z.prefrencesId == UserPrefrencesEntity.prefrencesId).ToList();
                if (chk.Count() == 0)
                {
                    using (var scope = new TransactionScope())
                    {
                        var UserPrefrence = new UserPrefrence
                        {
                            userId = UserPrefrencesEntity.userId,
                            categoryId = UserPrefrencesEntity.categoryId,
                            prefrencesId = UserPrefrencesEntity.prefrencesId,
                            type = UserPrefrencesEntity.type,
                            isActive = UserPrefrencesEntity.isActive,

                        };
                        _unitOfWork.UserPrefrenceRepository.Insert(UserPrefrence);
                        _unitOfWork.Save();
                        scope.Complete();

                    }
                }

            }
            return success;
        }

        public bool DeleteUserPrefrence(int Id)
        {
            var success = false;
            if (Id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var UserPrefrence = _unitOfWork.UserPrefrenceRepository.GetByID(Id);
                    if (UserPrefrence != null)
                    {
                        _unitOfWork.UserPrefrenceRepository.Delete(UserPrefrence);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }
    }
}
