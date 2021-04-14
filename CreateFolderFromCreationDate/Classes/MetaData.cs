
namespace CreateFolderFromCreationDate
{
    internal class MetaData
    {
        public string Directory { get; set; }
        public string Metadata { get; set; }
        public string Description { get; set; }

        public MetaData(string directory, string metadata, string description)
        {
            Directory = directory;
            Metadata = metadata;
            Description = description;
        }
    }
}
