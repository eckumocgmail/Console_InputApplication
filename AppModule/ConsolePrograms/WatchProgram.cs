using System;
using System.IO;



using static System.Console;
using static System.Threading.Thread;

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

[Label("Программа прослушивания файловой системы")]
public class WatchProgram: ConsoleProgram
{

    /// <summary>
    /// Выпорлнение программы
    /// </summary>        
    public static void Run(string[] args)
    {     
        Watch(@"D:\\", "echo");
    }

        
    /// <summary>
    /// Прослушивание событий измекнения файловых ресурсов и передача
    /// изменений в программе через параметры
    /// </summary>
    private static void Watch(string dir, string cmd)
    {
        Console.WriteLine("Начинаем прослушивать изменения файлов "+dir);
        using (var watcher = new FileSystemWatcher(dir, "*.*"))
        {
            watcher.IncludeSubdirectories = true;
            watcher.NotifyFilter = NotifyFilters.Attributes
                                | NotifyFilters.CreationTime
                                | NotifyFilters.DirectoryName
                                | NotifyFilters.FileName
                                | NotifyFilters.LastAccess
                                | NotifyFilters.LastWrite
                                | NotifyFilters.Security    
                                | NotifyFilters.Size;
                 
            watcher.Changed += (sender, evt) => {

                WriteLine($"{evt.FullPath}");

                if (evt.FullPath.EndsWith(".cs") 
                && evt.FullPath.StartsWith(@"D:\System-Config")==false
                && evt.FullPath.StartsWith(@"D:\\System-Config")== false
                )
                {
                    string file1 = evt.FullPath;
                    string file2 = evt.FullPath.Substring(0, 2) + "\\System-Config" + evt.FullPath.Substring(2);              
                    if (System.IO.File.Exists(file2))
                    {
                        var comp = new CompareProgram();
                        var patch = comp.Compare(file1, file2);
                        patch.ToJsonOnScreen().WriteToFile("history.txt");
                    }
                    else
                    {
                        "Нет файла предыдущей версии".WriteToConsole();
                    }
                    Copy(evt.FullPath, file2);
                    //CmdExec(cmd + $@" ""{evt.ChangeType}"" ""{evt.FullPath}""");
                } 
                    

            };
           

            watcher.Error += (sender, evt) => {
                WriteLine();
            };

            watcher.Filter = "*.*";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
              
        }
    }

    private static void Copy(string fullPath, string file2)
    {
        throw new NotImplementedException($"{TypeExtensions2.GetTypeName(typeof(WatchProgram))}");
    }
}
