using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace LAB3OOP
{
    class Program
    {
        static public int UserMenu()
        {
            Console.WriteLine("Enter command (commit, info <filename>, status, exit): ");
            Console.WriteLine($"1. commit");
            Console.WriteLine($"2. info <filename>");
            Console.WriteLine($"3. status");
            Console.WriteLine($"4. changes");
            Console.WriteLine($"5. Choose one version of the files");
            Console.WriteLine($"0. exit");
            int.TryParse(Console.ReadLine(),out int input);
            return input;
        }
        static void Main()
        {
            string folderPath = @"C:\Users\chicu\OneDrive\Desktop\Semestru 3 UTM FCIM\OOP\FolderLab3OOP";
            Console.WriteLine($"You are now in folder {folderPath}");
            var system = new DocumentChangeDetectionSystem(folderPath);
            system.StartMonitoring();
            while (true)
            {
                switch (UserMenu())
                {
                    case 1:
                        system.Commit();
                    break;
                    case 2:
                        while(true)
                        {
                            Console.WriteLine("Type the filename (WITH EXTENSION):");
                            string ?filename = Console.ReadLine();
                            if(filename != null)
                            {
                                system.Info(filename);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please specify a valid filename.");
                            }
                        }
                    break;

                    case 3:
                        system.Status();
                    break;

                    case 4:
                        system.ShowChanges();
                    break;
                    
                    case 5:
                        system.Checkout();
                    break;

                    case 0:
                        Environment.Exit(0);
                    break;

                    default:
                        Console.WriteLine("Unknown command.");
                    break;
                }
                // system.SaveAutomatically();
            }
        }
    }
}

// class Program
// {
//     static void Main()
//     {
//         Console.WriteLine();
//     }
// }
/*
    1.Path.GetFileName(FilePath): Extrage numele fișierului (inclusiv extensia) din calea completă.

    2.Path.GetExtension(FilePath): Extrage extensia fișierului (de exemplu, ".txt", ".jpg") din calea completă.

    3.Directory.GetFiles(folderPath): Returnează un array de căi complete pentru toate fișierele dintr-un director specificat.

    4.files.Remove(e.Name): Îndepărtează un element din colecția files pe baza numelui fișierului e.Name.

    5.Thread.Sleep(5000): Pauză de 5000 de milisecunde (5 secunde) în execuția thread-ului curent.

    6.readonly: Modificator care face ca valoarea unei variabile să fie setată doar în timpul inițializării 
    sau în constructorul clasei, iar ulterior să nu poată fi schimbată.
    7.
    8.
    9.
    10.
    11.
    12.
    13.
    14.
    15.
    16.
*/

/*
using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "C:\\LearnCsharp\\LAB3OOP\\Program.cs";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Fișierul nu există.");
            return;
        }

        // Citim toate liniile din fișier
        string[] lines = File.ReadAllLines(filePath);
        int lineCount = lines.Length;

        // Expresii regulate pentru a găsi clase și metode
        Regex classRegex = new Regex(@"\bclass\b");
        Regex methodRegex = new Regex(@"\b(public|private|protected|internal)\s+\b(static\s+)?\b(\w+)\s+\w+\s*\(.*\)");

        int classCount = 0;
        int methodCount = 0;

        // Parcurgem fiecare linie pentru a căuta clase și metode
        foreach (string line in lines)
        {
            if (classRegex.IsMatch(line))
            {
                classCount++;
            }

            if (methodRegex.IsMatch(line))
            {
                methodCount++;
            }
        }

        // Afișăm rezultatele
        Console.WriteLine($"Număr de linii: {lineCount}");
        Console.WriteLine($"Număr de clase: {classCount}");
        Console.WriteLine($"Număr de metode: {methodCount}");
    }
}
*/