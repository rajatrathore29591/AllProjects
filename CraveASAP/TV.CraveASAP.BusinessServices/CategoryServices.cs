using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.CraveASAP.BusinessEntities;
using TV.CraveASAP.BusinessServices.Interfaces;
using TV.CraveASAP.DataModel;
using TV.CraveASAP.DataModel.UnitOfWork;

namespace TV.CraveASAP.BusinessServices
{
    public class CategoryServices : ICategoryServices
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public CategoryServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public IEnumerable<CategoryEntity> GetAllCategory()
        {
            var category = _unitOfWork.CategoryRepository.GetAll().ToList();
            if (category.Any())
            {
                Mapper.CreateMap<Category, CategoryEntity>();
                Mapper.CreateMap<AppDefaultLandingPage, AppDefaultLandingPageEntity>();
                var categoryModel = Mapper.Map<List<Category>, List<CategoryEntity>>(category);
                return categoryModel;
            }
            return null;
        }

        public IEnumerable<SubCategoryEntity> GetAllSubCategory()
        {
            List<SubCategoryEntity> subCategoryCodeEntity = new List<SubCategoryEntity>();
            var subcategory = _unitOfWork.SubCategoryRepository.GetByCondition(d => d.isDeleted.Equals(false)).ToList();
            
            if (subcategory.Count() > 0)
            {

                foreach (var Item in subcategory)
                {
                    var category = _unitOfWork.CategoryRepository.GetByID(Item.categoryId);
                    subCategoryCodeEntity.Add(new SubCategoryEntity()
                    {
                        subCategoryId = Item.subCategoryId,
                        subCategoryName = Item.subCategoryName,
                        description = Item.description,
                        language = Item.language,
                        categoryId = Item.categoryId,
                        categoryName = category.categoryName
                    });
                }
                return subCategoryCodeEntity;
            }
            return subCategoryCodeEntity;

        }

        public IEnumerable<OptionalCategoryEntity> GetAllOptionalCategory()
        {
            List<OptionalCategoryEntity> optCategoryEntity = new List<OptionalCategoryEntity>();
            var optcategory = _unitOfWork.OptionalCategoryRepository.GetByCondition(d => d.isDeleted == false).ToList();
            if (optcategory.Count() > 0)
            {

                foreach (var Item in optcategory)
                {
                    var category = _unitOfWork.CategoryRepository.GetByID(Item.categoryId);
                    optCategoryEntity.Add(new OptionalCategoryEntity()
                    {
                        optCategoryId = Item.optCategoryId,
                        optCategoryName = Item.optCategoryName,
                        description = Item.description,
                        language = Item.language,
                        categoryId = Item.categoryId,
                        categoryName = category.categoryName,
                        subCategoryId = Item.subCategoryId

                    });
                }
                return optCategoryEntity;
            }
            return optCategoryEntity;
        }

    }
}
