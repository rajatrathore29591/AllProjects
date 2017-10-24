using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    /// <summary>
    /// This Service For All CRUD Operation FROM "Document" Entity. 
    /// </summary>
    public class DocumentService : BaseService, IDocumentService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public DocumentService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        /// <summary>
        /// this method for insert record in database.
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Description"></param>
        /// <param name="Extension"></param>
        /// <param name="FileSize"></param>
        /// <param name="Private"></param>
        /// <param name="Tags"></param>
        /// <param name="ThumbnailFileName"></param>
        /// <returns>response</returns>
        public Guid AddDocument(Guid id, string originalName, string name, string url, string title, string description, string extension, int? fileSize, bool? isPrivate, string tags, string thumbnailFileName)
        {
            try
            {
                //map entity.
                Document entity = new Document()
                {
                    DocumentId = id,
                    OriginalName = originalName,
                    Name = name,
                    URL = url,
                    Title = title,
                    Description = description,
                    Extension = extension,
                    FileSize = fileSize.Value,
                    Private = isPrivate.Value,
                    Tags = string.Empty,
                    ThumbnailFileName = thumbnailFileName,
                    //IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    EditedDate = DateTime.UtcNow
                };
                //add record from in database.
                unitOfWork.DocumentRepository.Insert<Document>(entity);

                ////save changes in database.
                unitOfWork.DocumentRepository.Commit();

                return entity.DocumentId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool EditDocument(Guid id, string originalName, string extension)
        {
            try
            {
                ////get existing record.
                Document entity = GetDocument(id);

                ////check entity is null.
                if (entity != null)
                {
                    ////map entity
                    entity.OriginalName = originalName;
                    entity.Extension = extension;

                    ////update record from existing entity in database.
                    unitOfWork.DocumentRepository.Update<Document>(entity);

                    ////save changes in database.
                    unitOfWork.DocumentRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// this method for add documnet by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Document GetDocument(Guid id)
        {
            try
            {
                //map entity.
                return unitOfWork.DocumentRepository.GetById<Document>(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method for get documnet by Name.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public Document GetDocumentByName(string Name)
        {
            try
            {
                //map entity.
                return unitOfWork.DocumentRepository.FindBy<Document>(x => x.Name == Name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Document> GetDocuments(Guid userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteDocument(Guid id)
        {
            try
            {
                //get existing record.
                var entity = GetDocument(id);

                //check entity is null.
                if (entity != null)
                {
                    //delete record from existing entity in database.
                    unitOfWork.DocumentRepository.Delete<Document>(entity);

                    ////save changes in database.
                    unitOfWork.DocumentRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
