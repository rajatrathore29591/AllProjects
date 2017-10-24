using System.Net.Http;
using System.Net.Http.Headers;
using System;

namespace BorgCivil.MVCWeb.Providers
{
    public class BCAMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        string path = string.Empty;
        public BCAMultipartFormDataStreamProvider(string rootPath) : base(rootPath)
        {
            path = rootPath;
        }

        public BCAMultipartFormDataStreamProvider(string rootPath, int bufferSize) : base(rootPath, bufferSize)
        {
            path = rootPath;
        }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            //Make the file name URL safe and then use it & is the only disallowed url character allowed in a windows filename
            var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ?  headers.ContentDisposition.FileName : "NoName";
            return name.Trim('"').Replace("&", "and");

            //var fullPath = path + name.Trim('"').Replace("&", "and");
            //var newName = Guid.NewGuid().ToString() + Path.GetExtension(fullPath);
            //return newName;
        }
    }
}