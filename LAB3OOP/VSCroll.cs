using System.Runtime.CompilerServices;

namespace LAB3OOP
{
    public class VersionHistory
    {
        static public List<ChangeRecord> changes = new List<ChangeRecord>();
        static public Dictionary<int,Dictionary<string,Dictionary<int, string>>> fileSnapshot = new Dictionary<int,Dictionary<string,Dictionary<int, string>>>();
        static public int VersionsNumber {get; set;} = 1;
        static public Dictionary<int, string> GetLastSnapshot(string FilePath)
        {
            var lastVersion = fileSnapshot.Keys.Any() ? fileSnapshot.Keys.Max() : 1;

            if (fileSnapshot.Count == 0)
            {
                Console.WriteLine("Nu există versiuni salvate.");
                return new Dictionary<int, string>();
            }
            
            if (fileSnapshot[lastVersion].TryGetValue(FilePath, out var fileData))
            {
                Console.WriteLine($"Version number is {VersionsNumber}");
                return new Dictionary<int, string>(fileData);
            }
                return new Dictionary<int, string>();
        }
        public VersionHistory(string FolderPath)
        {
            if(!fileSnapshot.Keys.Any())
            {
                fileSnapshot[VersionsNumber] = new Dictionary<string,Dictionary<int, string>>();
            }
            foreach(var OneFile in Directory.GetFiles(FolderPath))
            {

                if (!fileSnapshot[VersionsNumber].Keys.Any())
                {
                    fileSnapshot[VersionsNumber][OneFile] = new Dictionary<int, string>();
                }

                var NewAddedFile = File.ReadAllLines(OneFile)
                    .Select((line, index) => new { index, line })
                    .ToDictionary(x => x.index + 1, x => x.line);

                fileSnapshot[VersionsNumber][OneFile] = NewAddedFile;

            }
        }
        public void SaveSnapshot(string filePath)
        {
             Dictionary<int, string> one_fileSnapshot = File.ReadAllLines(filePath)
                .Select((line, index) => new { index, line })
                .ToDictionary(x => x.index + 1, x => x.line);

            int nextVersion = fileSnapshot.Keys.DefaultIfEmpty(0).Max() + 1;

            Console.WriteLine($"Next version is {nextVersion}");

            if (!fileSnapshot.ContainsKey(nextVersion))
            {
                fileSnapshot[nextVersion] = new Dictionary<string, Dictionary<int, string>>();
            }
            
            Console.WriteLine($"Versiunea din saveSNAPSHOT marita");
            fileSnapshot[nextVersion][filePath] = one_fileSnapshot;
            VersionsNumber++;
        }
        static public Dictionary<string,Dictionary<int, string>> GetSpecificSnapshot(int version)
        {
            if(!fileSnapshot.ContainsKey(version))
            {
                Console.WriteLine($"There is no key such version as '{version}'");
            }
            if (fileSnapshot.TryGetValue(version, out var versionData)) 
            {
                return fileSnapshot[version];
            }
            else
            {
                Console.WriteLine($"Versiunea {version} nu există.");
            }
            return new Dictionary<string,Dictionary<int, string>>(); 
        }
        public void AddChange(ChangeRecord change)
        {
            changes.Add(change);
        }
        public void ShowChanges()
        {
            foreach (var change in changes)
            {
                Console.WriteLine($"[{change.Timestamp}] {change.ChangeType} at Line {change.LineNumber}:");
                Console.WriteLine($"Old: {change.OldContent}");
                Console.WriteLine($"New: {change.NewContent}");
            }
        }
    }
}