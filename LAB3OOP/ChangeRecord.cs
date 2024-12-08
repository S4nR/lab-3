
namespace LAB3OOP
{
    public class ChangeRecord
    {
        public string ?ChangeType { get; set; }
        public int LineNumber { get; set; }
        public string ?OldContent { get; set; }
        public string ?NewContent { get; set; }
        public DateTime Timestamp { get; set; }
        public int VersionNumber {get; set;}
    }
}