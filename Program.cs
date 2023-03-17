/**
Caracters Especials
╔══╦══╗ ┌──┬──┐
║  ║  ║ │  │  │
╠══╬══╣ ├──┼──┤
║  ║  ║ │  │  │
╚══╩══╝ └──┴──┘

© ¤ ¢ ¥ Ð
**/

using managerFiles.Models;
using System.Collections;
using System.Collections.Generic;
using System.IO;

internal class Program
{
    private static void Main(string[] args)
    {
        DictionaryModel dictionary = new();
        bool validList = true;
        while (validList)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{dictionary.Margin}╔═══════════════════════════════════════════╗");
            Console.WriteLine($"{dictionary.Margin}║                                           ║");
            Console.WriteLine($"{dictionary.Margin}║   Information the process                 ║");
            Console.WriteLine($"{dictionary.Margin}║   0 - Clear                               ║");
            Console.WriteLine($"{dictionary.Margin}║   1 - Rename Files and Remove Character   ║");
            Console.WriteLine($"{dictionary.Margin}║   2 - Copy Files from list.txt            ║");
            Console.WriteLine($"{dictionary.Margin}║                                           ║");
            Console.WriteLine($"{dictionary.Margin}╚═══════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.White;

            LineBreak();
            Console.Write($"{dictionary.Margin}Number:{dictionary.Margin}");
            string? read = Console.ReadLine();
            int type = string.IsNullOrEmpty(read) ? 0 : int.Parse(read);
            LineBreak();


            switch (type)
            {
                case 0:
                    Console.Clear();
                    break;
                case 1:
                    Console.Clear();
                    RenameFilesAndRemoveCharacter();
                    validList = false;
                    break;
                case 2:
                    Console.Clear();
                    CopyFiles();
                    validList = false;
                    break;
                default:
                    ConsoleWarning($"{dictionary.Margin}Number Incorrect!!!");
                    LineBreak();
                    break;
            }
        }
    }


    private static void RenameFilesAndRemoveCharacter()
    {

    }

    private static void CopyFiles()
    {
        LineBreak();
        DictionaryModel dictionary = new();

        //Wait information Origin
        string origin = ValidationPath("Origin", false, true);

        //Wait information Destination
        string destination = ValidationPath("Destination", false, true);

        if (origin == destination)
        {
            ConsoleWarning($"{dictionary.Margin}Origin and Destination cannot be the same.");
            CopyFiles();
            return;
        }

        //Wait information List
        string pathList = ValidationPath("PathList", true, false);

        string[] files = Directory.GetFiles(origin);

        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            string fileType = Path.GetExtension(file);

            if (!string.IsNullOrEmpty(pathList))
            {
                StreamReader leitor = new("list.txt");

                // Lê o conteúdo do arquivo linha por linha até o final
                while (!leitor.EndOfStream)
                {
                    string? linha = leitor.ReadLine();
                    Console.WriteLine(linha);
                }
            }
            else
            {
                //File.Copy($"{origin}/{fileName}", $"{destination}/{fileName}");
                ConsoleSuccess($"{dictionary.Margin}Arquivo copiado com sucesso. {fileName}");
            }
        }
    }

    #region ::SERVICES::
    private static string ValidationPath(string msg, bool validationFile, bool mandatory)
    {
        DictionaryModel dictionary = new();
        bool validationWhile = true;
        string? path = string.Empty;
        string msgNotFound = validationFile ? dictionary.FileNotFound : dictionary.DirectoryNotFound;

        while (validationWhile)
        {
            Console.Write($"{dictionary.Margin}Information {msg}:{dictionary.Margin}");
            path = Console.ReadLine();

            if (mandatory)
            {
                if (string.IsNullOrEmpty(path))
                {
                    ConsoleWarning($"{dictionary.Margin}{dictionary.Mandatory} {msg}!!");
                    LineBreak();
                }
                else
                {
                    path = ValidationDirectory(path, $"{dictionary.Margin}{msgNotFound}");
                }

                validationWhile = string.IsNullOrEmpty(path);
            }
            else
            {
                if (!string.IsNullOrEmpty(path))
                    path = ValidationDirectory(path, $"{dictionary.Margin}{msgNotFound}");

                validationWhile = false;
            }

        }

        return path;
    }

    private static string ValidationDirectory(string path, string msgNotFound)
    {
        path = FixPath(path);
        if (!Directory.Exists(path))
        {
            path = string.Empty;
            ConsoleWarning(msgNotFound);
        }

        LineBreak();

        return path;
    }
    private static string FixPath(string path)
    {
        if (path[^1..] == "\\")
            return path[..^1];

        return path;
    }
    private static void ConsoleSuccess(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(msg);
        Console.ForegroundColor = ConsoleColor.White;
    }
    private static void ConsoleWarning(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(msg);
        Console.ForegroundColor = ConsoleColor.White;
    }
    private static void LineBreak()
    {
        Console.WriteLine("");
    }

    #endregion
}