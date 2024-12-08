using System.Text.RegularExpressions;
// #pragma warning disable CA1416
namespace LAB3OOP
{
    class FileData
    {
        public string FilePath { get; }
        public string FileName => Path.GetFileName(FilePath);
        public string Extension => Path.GetExtension(FilePath);
        public DateTime CreationTime { get; private set; }
        public DateTime LastModifiedTime { get; private set; }
        public FileData(string filePath)
        {
            FilePath = filePath;
            UpdateFileInfo();
        }
        public void UpdateFileInfo()
        {
            var fileInfo = new FileInfo(FilePath);
            CreationTime = fileInfo.CreationTime;
            LastModifiedTime = fileInfo.LastWriteTime;
        }
        public void DisplayInfo()
        {
            Console.WriteLine($"Filename: {FileName}, Extension: {Extension}, Created: {CreationTime}, Last Modified: {LastModifiedTime}");
            
            if (Extension == ".txt")
            {
                int lineCount = File.ReadAllLines(FilePath).Length;
                int wordCount = File.ReadAllText(FilePath).Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
                int charCount = File.ReadAllText(FilePath).Length;
                Console.WriteLine($"Text File Info - Lines: {lineCount}, Words: {wordCount}, Characters: {charCount}");
            }
            else if (Extension == ".png" || Extension == ".jpg")
            {
                Console.WriteLine($"Image        ->{FileName}");
                Console.WriteLine($"Extension    ->{Extension}");
                Console.WriteLine($"CreationTime -> {CreationTime}");
            }
            else if (Extension == ".py" || Extension == ".java")
            {
                string[] lines = File.ReadAllLines(FilePath);
                int lineCount = lines.Length;
                Regex classRegex = new Regex(@"\bclass\b");
                Regex methodRegex1 = new Regex(@"\b(public|private|protected|internal)\s+\b(static\s+)?\b(\w+)\s+\w+\s*\(.*\)");
                Regex methodRegex2 = new Regex(@"^\s*def\s+(\w+)\s*\(.*\)\s*:");

                int classCount = 0;
                int methodCount = 0;

                foreach (string line in lines)
                {
                    if (classRegex.IsMatch(line))
                    {
                        classCount++;
                    }
                    if (methodRegex1.IsMatch(line) || methodRegex2.IsMatch(line) )
                    {
                        methodCount++;
                    }
                }

                Console.WriteLine($"Număr de linii: {lineCount}");
                Console.WriteLine($"Număr de clase: {classCount}");
                Console.WriteLine($"Număr de metode: {methodCount}");
                Console.WriteLine($"Program File Info - Lines: {lineCount}");
            }
        }
    }
}