using TradeSystem.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Service
{
    public interface IDocumentService : IService
    {
        /// <summary>
        /// this method for add documnet by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Document GetDocument(Guid id);
        
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
        Guid AddDocument(Guid id, string originalName, string name, string url, string title, string description, string extension, int? fileSize, bool? isPrivate, string tags, string thumbnailFileName);

        bool EditDocument(Guid id, string originalName, string extension);
        bool DeleteDocument(Guid id);
        
        List<Document> GetDocuments(Guid userId);
    }
}
