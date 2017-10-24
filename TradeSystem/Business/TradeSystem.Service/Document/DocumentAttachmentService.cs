using Ipm.Hub.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ipm.Hub.Framework.Entities;

namespace Ipm.Hub.Service
{
    /// <summary>
    /// This Service For All CRUD Operation FROM "Document" Entity. 
    /// </summary>
    public class DocumentAttachmentService : BaseService, IDocumentAttachmentService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public DocumentAttachmentService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        /// <summary>
        ///This method use for get attachment by id.
        /// </summary>
        /// <param name="attachmentId">unique id</param>
        /// <returns>entity object</returns>
        public DocumentAttachment GetAttachment(Guid attachmentId)
        {
            try
            {
                //map entity.
                return unitOfWork.AttachmentRepository.GetById<DocumentAttachment>(attachmentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This mehod use for get Attachments by contextType and id.
        /// </summary>
        /// <param name="contextId">document type of id like project id/program id etc.</param>
        /// <param name="contextType">document type like project/task/program</param>
        /// <returns>entities objcet</returns>
        public List<DocumentAttachment> GetAttachments(Guid contextId, string contextType)
        {
            try
            {
                //map entity.
                return unitOfWork.AttachmentRepository.SearchBy<DocumentAttachment>(x => x.ContextId == contextId && x.ContextType == contextType).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method use for set record into "DocumentAttachment" entity
        /// </summary>
        /// <param name="DocumentId"></param>
        /// <param name="ContextId"></param>
        /// <param name="ContextType"></param>
        /// <param name="OwnerId"></param>
        /// <returns>result</returns>
        public bool AddAttachment(Guid documentId, Guid contextId, string contextType, string ownerId)
        {
            try
            {
                //map entity.
                var entity = new DocumentAttachment()
                {
                    Id = Guid.NewGuid(),
                    ContextId = contextId,
                    DocumentId = documentId,
                    OwnerId = ownerId,
                    ContextType = contextType,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };
                //add record from in database.
                unitOfWork.AttachmentRepository.Insert<DocumentAttachment>(entity);

                ////save changes in database.
                unitOfWork.AttachmentRepository.Commit();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method use for set record into "DocumentAttachment" & "Doocument"  entity
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="contextId"></param>
        /// <param name="contextType"></param>
        /// <param name="ownerId"></param>
        /// <param name="originalName"></param>
        /// <param name="organisationName"></param>
        /// <param name="url"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="extension"></param>
        /// <param name="fileSize"></param>
        /// <param name="isPrivate"></param>
        /// <param name="tags"></param>
        /// <param name="thumbnailFileName"></param>
        /// <returns>result</returns>
        public bool AddAttachment(Guid id,  Guid documentId, Guid contextId, string contextType, string ownerId, string originalName, string name, string url, string title, string description, string extension, int? fileSize, bool? isPrivate, string tags, string thumbnailFileName)
        {
            try
            {
                //map Document entity.
                Document document = new Document()
                {
                    Id = documentId,
                    OriginalName = originalName,
                    Name = name,
                    URL = url,
                    Title = title,
                    Description = description,
                    Extension = extension,
                    FileSize = fileSize,
                    Private = isPrivate,
                    Tags = tags,
                    ThumbnailFileName = thumbnailFileName,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                };

                //map attachment entity.
                var attachment = new DocumentAttachment()
                {
                    Id = Guid.NewGuid(),
                    ContextId = contextId,
                    DocumentId = document.Id,
                    OwnerId = ownerId,
                    ContextType = contextType,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };

                //add record from in database.
                unitOfWork.AttachmentRepository.Insert<Document>(document);
                
                //add record from in database.
                unitOfWork.AttachmentRepository.Insert<DocumentAttachment>(attachment);

                ////save changes in database.
                unitOfWork.AttachmentRepository.Commit();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// This method use for modify record into "DocumentAttachment" entity by id
        /// </summary>
        /// <param name="AttachmentId"></param>
        /// <param name="DocumentId"></param>
        /// <param name="ContextId"></param>
        /// <param name="ContextType"></param>
        /// <param name="OwnerId"></param>
        /// <returns></returns>
        public bool EditAttachment(Guid attachmentId, Guid documentId, Guid contextId, string contextType, string ownerId,string Title)
        {
            try
            {
                //get existing record.
                var entity = GetAttachment(attachmentId);

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.Document.Title = Title;
                    entity.ContextType = !string.IsNullOrEmpty(contextType) ? contextType : entity.ContextType;
                    entity.ModifiedDate = DateTime.UtcNow;

                    //update record from existing entity in database.
                    unitOfWork.AttachmentRepository.Update<DocumentAttachment>(entity);

                    ////save changes in database.
                    unitOfWork.AttachmentRepository.Commit();

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
        /// This method use for delete record into "DocumentAttachment" entity by id
        /// </summary>
        /// <param name="AttachmentId"></param>
        /// <returns>result</returns>
        public bool DeleteAttachment(Guid attachmentId)
        {
            try
            {
                ///get existing record.
                var entity = GetAttachment(attachmentId);

                //check entity is null.
                if (entity != null)
                {
                    ////delete file from folder database. Forler
                    if (entity.Document.URL != null)
                        Utilities.ServiceHelper.FileDeleteFromFolder(entity.Document.URL);
                    ////delete record from existing entity in database. Document
                    if (entity.Document != null)
                        unitOfWork.AttachmentRepository.Delete<Document>(entity.Document);

                    ////delete record from existing entity in database. Attachment
                    unitOfWork.AttachmentRepository.Delete<DocumentAttachment>(entity);

                   

                    ////save changes in database.
                    unitOfWork.AttachmentRepository.Commit();

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
        /// This method use for bulk delete records from "DocumentAttachment" entity by id.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>no of delete record</returns>
        public int DeleteAttachments(List<Guid> ids)
        {
            try
            {
                ////get list of attachment.
                var entities = GetAttachments(ids);

                ////check entity is null.
                if (entities != null && entities.Count > 0)
                {
                    //delete record from existing entity in database. Document
                    foreach (var entity in entities)
                    {
                        ////delete record from existing entity in database. Attachment
                        if (entity.Document != null)
                            unitOfWork.AttachmentRepository.Delete<Document>(entity.Document);

                        ////delete file from folder database. Forler
                        if (entity.Document.URL != null)
                            Utilities.ServiceHelper.FileDeleteFromFolder(entity.Document.URL);
                    }

                    ////delete record from existing entity in database.
                    unitOfWork.AttachmentRepository.Delete<DocumentAttachment>(entities);

                    ////save changes in database.
                    unitOfWork.CommitTransaction();

                    return entities.Count;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This mehod use for get Attachments by ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>list of entity</returns>
        public List<DocumentAttachment> GetAttachments(List<Guid> ids)
        {
            try
            {
                ////define list.
                var entities = new List<DocumentAttachment>();

                ////bind entity in a list.
                foreach (var id in ids)
                {
                    ////get existing record.
                    var entity = GetAttachment(id);

                    ////attach
                    entities.Add(entity);
                }
                return entities;
            }
            catch (Exception ex)
            {
                ////write log
                Console.Write(ex.Message);

                ////return in case or error.
                return new List<DocumentAttachment>();
            }
        }

    }
}
