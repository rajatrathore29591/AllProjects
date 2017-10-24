using BorgCivil.Framework.Entities;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IDocumentService : IService
    {
       
        Document GetDocument(Guid id);

        Document GetDocumentByName(string Name);
        Guid AddDocument(Guid id, string originalName, string name, string url, string title, string description, string extension, int? fileSize, bool? isPrivate, string tags, string thumbnailFileName);
        bool EditDocument(Guid id, string originalName, string extension);
        bool DeleteDocument(Guid id);
        List<Document> GetDocuments(Guid userId);

    }
}
