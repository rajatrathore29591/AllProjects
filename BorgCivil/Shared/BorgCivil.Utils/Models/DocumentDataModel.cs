namespace BorgCivil.Utils.Models
{
    public partial class DocumentDataModel
    {
        public string Id { get; set; }
       
        public string Name { get; set; }

        public string OriginalName { get; set; }

        public string URL { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Extension { get; set; }

        public int? FileSize { get; set; }

        public string ThumbnailFileName { get; set; }
        public string Tags { get; set; }
        public bool Private { get; set; }



    }
}
