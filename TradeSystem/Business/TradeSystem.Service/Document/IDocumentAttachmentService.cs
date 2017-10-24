using Ipm.Hub.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipm.Hub.Service
{
    public interface IDocumentAttachmentService : IService
    {
        /// <summary>
        ///This method use for get attachment by id.
        /// </summary>
        /// <param name="attachmentId">unique id</param>
        /// <returns>entity object</returns>
        DocumentAttachment GetAttachment(Guid attachmentId);

        /// <summary>
        /// This mehod use for get Attachments by contextType and id.
        /// </summary>
        /// <param name="contextId">document type of id like project id/program id etc.</param>
        /// <param name="contextType">document type like project/task/program</param>
        /// <returns>entities objcet</returns>
        List<DocumentAttachment> GetAttachments(Guid contextId, string contextType);

        /// <summary>
        /// This method use for set record into "DocumentAttachment" entity
        /// </summary>
        /// <param name="DocumentId"></param>
        /// <param name="ContextId"></param>
        /// <param name="ContextType"></param>
        /// <param name="OwnerId"></param>
        /// <returns>result</returns>
        bool AddAttachment(Guid documentId, Guid contextId, string contextType, string ownerId);

        /// <summary>
        /// This method use for set record into "DocumentAttachment" entity
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
        bool AddAttachment(Guid id, Guid documentId, Guid contextId, string contextType, string ownerId, string originalName, string name, string url, string title, string description, string extension, int? fileSize, bool? isPrivate, string tags, string thumbnailFileName);

        /// <summary>
        /// This method use for modify record into "DocumentAttachment" entity by id
        /// </summary>
        /// <param name="AttachmentId"></param>
        /// <param name="DocumentId"></param>
        /// <param name="ContextId"></param>
        /// <param name="ContextType"></param>
        /// <param name="OwnerId"></param>
        /// <returns></returns>
        bool EditAttachment(Guid attachmentId, Guid documentId, Guid contextId, string contextType, string ownerId, string Title);

        /// <summary>
        /// This method use for delete record from "DocumentAttachment" entity by id.
        /// </summary>
        /// <param name="AttachmentId"></param>
        /// <returns>result</returns>
        bool DeleteAttachment(Guid attachmentId);

        /// <summary>
        /// This method use for bulk delete records from "DocumentAttachment" entity by id.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>no of delete record</returns>
        int DeleteAttachments(List<Guid> ids);

        /// <summary>
        /// This mehod use for get Attachments by ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>list of entity</returns>
        List<DocumentAttachment> GetAttachments(List<Guid> ids);
    }
}


