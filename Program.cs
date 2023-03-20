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
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;

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
            Console.WriteLine($"{dictionary.Margin}║   1 - Generate List.json                  ║");
            Console.WriteLine($"{dictionary.Margin}║   2 - Rename Files and Remove Character   ║");
            Console.WriteLine($"{dictionary.Margin}║   3 - Copy Files                          ║");
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
                    GenerateList();
                    break;
                case 2:
                    Console.Clear();
                    RenameFilesAndRemoveCharacter();
                    validList = false;
                    break;
                case 3:
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

    private static void GenerateList()
    {
        DictionaryModel dictionary = new();
        string origin = "";
        string originTXT = "";
        bool copySeparatingFileName = false;
        bool copySeparatingFileType = false;

        //Wait information TXT read
        bool readTXT = ValidationBoolReadLine("Read File.txt (Y/N)", true);
        if (readTXT)
        {
            ConsoleWarning($"{dictionary.Margin}Example: (id, name.type)");
            originTXT = ValidationPath("Origin File.txt", true, true);
        }


        //Wait information Origin
        if (!readTXT)
            origin = ValidationPath("Origin", false, true);

        //Wait information Type Separating
        bool copySeparatingId = ValidationBoolReadLine("Separating by Id (Y/N)", false);
        if (!copySeparatingId)
            copySeparatingFileName = ValidationBoolReadLine("Separating by File Name (Y/N)", false);
        else if(!copySeparatingId && !copySeparatingFileName)
            copySeparatingFileType = ValidationBoolReadLine("Separating by File Type (Y/N)", false);

        ListModel listModel = new()
        {
            CopySeparatingById = copySeparatingId,
            CopySeparatingByFileName = copySeparatingFileName,
            CopySeparatingByFileType = copySeparatingFileType,
        };


        int index = 0;
        if (readTXT)
        {
            StreamReader reader = new(originTXT);
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] split = line.Split(", ");

                if (copySeparatingId)
                {
                    listModel.CopyFiles.Add(new CopyFilesModel
                    {
                        Id = long.Parse(split[0]),
                        Name = split[1],
                        Type = GetTypeFile(split[1]),
                    });
                }
                else
                {
                    listModel.CopyFiles.Add(new CopyFilesModel
                    {
                        Id = index,
                        Name = split[1],
                        Type = GetTypeFile(split[1]),
                    });
                    index++;
                }
            }
        }
        else
        {
            string[] files = Directory.GetFiles(origin);

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string fileType = Path.GetExtension(file);
                string filePath = Path.GetFullPath(file);

                if (copySeparatingId)
                {
                    Console.Write($"{dictionary.Margin}Information ID:{dictionary.Margin}");
                    string? id = Console.ReadLine();
                    LineBreak();

                    listModel.CopyFiles.Add(new CopyFilesModel
                    {
                        Id = string.IsNullOrEmpty(id) ? 0 : long.Parse(id),
                        Name = fileName,
                        Type = fileType,
                    });
                }
                else
                {
                    listModel.CopyFiles.Add(new CopyFilesModel
                    {
                        Id = index,
                        Name = fileName,
                        Type = fileType,
                    });
                    index++;
                }
            }
        }

        File.WriteAllText($"{origin}\\List.json", JsonConvert.SerializeObject(listModel));
    }
    
    private static void RenameFilesAndRemoveCharacter()
    {
        DictionaryModel dictionary = new();

        //Wait information Origin
        string origin = ValidationPath("Origin", false, true);

        string characterRemove = ValidationStringReadLine("Remove Character", true);
        string characterReplace = ValidationStringReadLine("Replace Character", false);

        if (string.IsNullOrEmpty(characterReplace))
            characterReplace = " ";

        string[] files = Directory.GetFiles(origin);

        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            string fileType = Path.GetExtension(file);
            string filePath = Path.GetFullPath(file);
            
            string newFileName = Regex.Replace(fileName, characterRemove, characterReplace, RegexOptions.IgnoreCase);

            File.Replace(newFileName, origin, $"{origin}\\OLD");
        }
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
            string filePath = Path.GetFullPath(file);

            if (string.IsNullOrEmpty(pathList))
            {
                //File.Copy($"{origin}/{fileName}", $"{destination}/{fileName}");
                ConsoleSuccess($"{dictionary.Margin}Arquivo copiado com sucesso. {fileName}");
            }
            else
            {
                StreamReader streamReader = new (pathList);
                ListModel? listModel = JsonConvert.DeserializeObject<ListModel>(streamReader.ReadToEnd());

                if (listModel == null)
                {
                    ConsoleWarning("List cannot be empty");
                    return;
                };

                listModel.CopyFiles.ForEach(files =>
                {
                    if (files.Name == fileName)
                    {
                        string destinationPath = "";
                        if (listModel.CopySeparatingById)
                            destinationPath = $"{destination}\\{files.Id}";
                        else if (listModel.CopySeparatingByFileName)
                            destinationPath = $"{destination}\\{files.Name}";
                        else if (listModel.CopySeparatingByFileType)
                            destinationPath = $"{destination}\\{files.Type}";
                        else
                            destinationPath = destination;

                        if (!Directory.Exists(destinationPath))
                            Directory.CreateDirectory(destinationPath);

                        File.Copy(filePath, $"{destinationPath}\\{fileName}", true);
                    };
                });
            }
        }
    }

    #region ::SERVICES::
    private static string ValidationStringReadLine(string msg, bool mandatory)
    {
        DictionaryModel dictionary = new();
        bool validationWhile = true;
        string value = "";

        while (validationWhile)
        {
            Console.Write($"{dictionary.Margin}Information {msg}:{dictionary.Margin}");
            string? readLine = Console.ReadLine();

            if (mandatory)
            {
                if (string.IsNullOrEmpty(readLine))
                {
                    ConsoleWarning($"{dictionary.Margin}{dictionary.Mandatory} {msg}!!");
                    LineBreak();
                }
                else
                    value = readLine;

                validationWhile = string.IsNullOrEmpty(readLine);
            }
            else
            {
                value = string.IsNullOrEmpty(readLine)? "" : readLine;
                validationWhile = false;
            }

        }

        return value;
    }
    private static bool ValidationBoolReadLine(string msg, bool mandatory)
    {
        DictionaryModel dictionary = new();
        bool validationWhile = true;
        bool value = false;

        while (validationWhile)
        {
            Console.Write($"{dictionary.Margin}Information {msg}:{dictionary.Margin}");
            string? readLine = Console.ReadLine();

            if (mandatory)
            {
                if (string.IsNullOrEmpty(readLine))
                {
                    ConsoleWarning($"{dictionary.Margin}{dictionary.Mandatory} {msg}!!");
                    LineBreak();
                }
                else
                {
                    if (readLine.ToUpper() == "Y" || readLine.ToUpper() == "N")
                    {
                        value = readLine.ToUpper() == "Y";
                        validationWhile = false;
                    }
                    else
                    {
                        ConsoleWarning($"{dictionary.Margin}{dictionary.Mandatory} {msg}!!");
                        LineBreak();
                    }
                }
            }
            else
            {
                value = string.IsNullOrEmpty(readLine)? false : readLine.ToUpper() == "Y";
                validationWhile = false;
            }

        }

        return value;
    }
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
                    path = ValidationDirectory(path, validationFile, msgNotFound);
                }

                validationWhile = string.IsNullOrEmpty(path);
            }
            else
            {
                if (!string.IsNullOrEmpty(path))
                    path = ValidationDirectory(path, validationFile, msgNotFound);

                validationWhile = false;
            }

        }

        return string.IsNullOrEmpty(path)? string.Empty : path;
    }
    private static string ValidationDirectory(string path, bool validationFile, string msgNotFound)
    {
        DictionaryModel dictionary = new();
        string pathFile = string.Empty;
        path = FixPath(path);

        if (path.Contains(".json") || path.Contains(".txt"))
        {
            if (validationFile)
                pathFile = path.Substring(0, path.LastIndexOf("\\"));
            else
            {
                ConsoleWarning($"{dictionary.Margin}{dictionary.PathInvalid}");
                LineBreak();
                return string.Empty;
            }
        }
        else
            pathFile = path;

        if (!Directory.Exists(pathFile))
        {
            ConsoleWarning($"{dictionary.Margin}{msgNotFound}");
            LineBreak();
            return string.Empty;
        }

        LineBreak();

        return path;
    }
    private static string GetTypeFile(string path)
    {
        List<string> fileType = new()
        {
            ".json",
            ".txt",
            ".pdf",
            ".docx",
            ".xls",
            ".xlsx",
            ".csv",
            ".png",
            ".jpg",
            ".jpeg",
            ".jfif",
            ".gif",
            ".sql",
            ".xml",
            ".exe",
            ".ovpn",
            ".zip",
            ".rar"
        };

        foreach (string type in fileType)
        {
            if (path.Contains(type) || path.Contains(type.ToUpper()))
                return type;
        }

        return string.Empty;
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