#pragma warning disable CS8618
#pragma warning disable CS8604
#pragma warning disable CS0219
namespace LAB3OOP
{
    class DocumentChangeDetectionSystem
    {
        private readonly string folderPath;
        static public Dictionary<string, FileData> files = new Dictionary<string, FileData>();
        private DateTime snapshotTime;
        private  VersionHistory versionHistory;
        private FileSystemWatcher watcher;
        public DocumentChangeDetectionSystem(string path)
        {
            folderPath = path;
            InitializeFiles();
            versionHistory = new VersionHistory(path);
            Commit();
        }
        private void InitializeFiles()
        {
            foreach (var filePath in Directory.GetFiles(folderPath))
            {
                files[Path.GetFileName(filePath)] = new FileData(filePath); //files[DOAR numele fisierului ] = FileData(Full pATH)
            }
        }
        private void SetupWatcher()
        {
            watcher = new FileSystemWatcher(folderPath)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime
            };
            watcher.Changed += (s, e) => DetectChanges(e.FullPath);
            watcher.Created += WatchCreatedFile;
            watcher.Deleted += WatchDeletedFile;
            watcher.Renamed += WatchRenamedFile;
            watcher.EnableRaisingEvents = true;
        }
        private void DetectChanges(string filePath)
        {
            if (files.ContainsKey(filePath))
            {
                files[filePath].UpdateFileInfo();
            }
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File doesn't exist");
                return;
            }
            var oldSnapshot = new Dictionary<int, string>(VersionHistory.GetLastSnapshot(filePath));
            var newSnapshot = File.ReadAllLines(filePath)
                .Select((line, index) => new { index, line })
                .ToDictionary(x => x.index + 1, x => x.line);
            bool HasChanges = false;
            int vers = VersionHistory.fileSnapshot.Keys.Any() ?  VersionHistory.fileSnapshot.Keys.Max() + 1 : 1;
            foreach (var line in oldSnapshot.ToList())
            {
                if (oldSnapshot.ContainsKey(line.Key) && oldSnapshot[line.Key] != line.Value)
                {
                    versionHistory.AddChange(new ChangeRecord
                    {
                        ChangeType = "Modified",
                        LineNumber = line.Key,
                        OldContent = oldSnapshot[line.Key],
                        NewContent = line.Value,
                        Timestamp = DateTime.Now,
                        VersionNumber = vers
                    });
                    HasChanges = true;Console.WriteLine("se salveaza   Modified");
                }
                if (!oldSnapshot.ContainsKey(line.Key))
                {
                    versionHistory.AddChange(new ChangeRecord
                    {
                        ChangeType = "Added",
                        LineNumber = line.Key,
                        NewContent = line.Value,
                        Timestamp = DateTime.Now,
                        VersionNumber = vers
                    });
                    HasChanges = true;Console.WriteLine("se salveaza  Added");
                }
            }

            foreach (var line in oldSnapshot.ToList())
            {
                if (!newSnapshot.ContainsKey(line.Key))
                {
                    versionHistory.AddChange(new ChangeRecord
                    {
                        ChangeType = "Deleted",
                        LineNumber = line.Key,
                        OldContent = line.Value,
                        Timestamp = DateTime.Now,
                        VersionNumber = vers
                    });
                    HasChanges = true;Console.WriteLine("se salveaza  Deleted");
                }
            }
            if(HasChanges == true)
            {
                Console.WriteLine("Version SaveSnapshot called from DocumentChangeDetectionSystem.cs");
                versionHistory.SaveSnapshot(filePath);
                VersionHistory.VersionsNumber++;
            }
        }
        private void WatchCreatedFile(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File Created: {e.Name}");
            files[e.Name] = new FileData(e.FullPath);
        }
        private void WatchDeletedFile(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File Deleted: {e.Name}");
            files.Remove(e.Name);
        }
        private void WatchRenamedFile(object sender, RenamedEventArgs e)
        {
            if (files.ContainsKey(e.OldName))
            {
                files[e.Name] = new FileData(e.FullPath);
                files.Remove(e.OldName);
                Console.WriteLine($"File Renamed from '{e.OldName}' to '{e.FullPath}' ");
            }
        }
        public void SaveAutomatically()
        {
            while(true)
            {
                foreach (var file in Directory.GetFiles(folderPath))
                {
                    DetectChanges(file);
                }
                Thread.Sleep(5000);
            }
        }
        public void ShowChanges()
        {
            versionHistory.ShowChanges();
        }
        public void Commit()
        {
            snapshotTime = DateTime.Now;
            Console.WriteLine("Snapshot updated. All files are up to date.");
        }
        public void Info(string filename)
        {
            if (files.ContainsKey(filename))
            {
                files[filename].DisplayInfo();
            }
            else
            {
                Console.WriteLine("File not found.");
            }
        }
        public void Status()
        {
            foreach (var file in files.Values)
            {
                string status = file.LastModifiedTime > snapshotTime ? "Modified" : "Unchanged";
                Console.WriteLine("{0,-40} - {1}",file.FileName,status);
            }
        }
        public void StartMonitoring()
        {
            SetupWatcher();
        }
        public void Checkout()
        {
            int version_nr;
            Console.WriteLine("Choose a version:");
            int versions = VersionHistory.fileSnapshot.Keys.Any() ?  VersionHistory.fileSnapshot.Keys.DefaultIfEmpty(0).Max() : 1;
            Console.WriteLine($"Number of versions is {versions}");
            for(int a = 1; a <= VersionHistory.VersionsNumber; a++)
            {
                Console.WriteLine($"Version {a}.");
            }
            int.TryParse(Console.ReadLine(),out version_nr);
            Dictionary<string,Dictionary<int, string>> version_content = VersionHistory.GetSpecificSnapshot(version_nr);
            foreach(var FileName in version_content)
            {
                lock(watcher)
                {   
                    using(StreamWriter writer  = new StreamWriter(FileName.Key))
                    {
                        foreach(var FileContent in FileName.Value)
                        {
                            writer.WriteLine(FileContent.Value);
                        }
                    }
                }
            }         
        }
    }
}
/*
    Console.WriteLine($"DocumentChangeDetectionSystem     and version count is {VersionHistory.VersionsNumber} ");
    foreach(var val in oldSnapshot)
    {
        Console.WriteLine($"oldSnapshot = {val}");
    }
    foreach(var val in newSnapshot)
    {
        Console.WriteLine($"newSnapshot = {val}");
    }
start monitoring
    // Task.Run(() => SaveAutomatically());


main
    foreach(var i in VersionHistory.fileSnapshot)
                {
                    Console.WriteLine($"fileSnapshot = {i}");
                    foreach(var j in i.Value)
                    {
                        Console.WriteLine($"j = {j}");
                        foreach(var k in j.Value)
                        {
                            Console.WriteLine($"k = {k}");
                        }
                    }
                }
                //fileSnapshot[VersionsNumber].Add(OneFile,NewAddedFile);
*/