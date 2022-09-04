using System;
using System.IO;
using System.Linq;

using ConsoleApp;
using static InputApplicationProgram;

/// <summary>
/// Прослушивает события изменения файловой системы
/// </summary>
public class ProgramDirectory : ProgramDialog
{
    public ProgramDirectory(IServiceProvider provider, string path) : base(provider, path)
    {
    }


    /// Процесс выполняет отслеживание появление новых      *
    /// текстовых файлов в каталогах заданными через аргументы
    public static void Run(params string[] args)
    {            
        if (Environment.UserInteractive)
        {
            System.Console.ReadLine();
        }
        else
        {
            if (args.Length == 0)
                WatchDir
                (
                    System.IO.Directory.GetCurrentDirectory(), 
                    (sender, evt) => {
                        Console.WriteLine($"{evt.ChangeType}{evt.FullPath}");
                    },
                    (sender, evt) =>
                    {
                        Console.WriteLine($"{evt.GetException()}");
                    }
                );
            else foreach (var dir in args)
                WatchDir(dir, (sender, evt) => {
                    Console.WriteLine($"{evt.ChangeType}{evt.FullPath}");
                }, ProgramDirectory.OnError );
        }                        
    }


    /// <summary>
    /// Вывод  сведений об исключении
    /// </summary>
    public static void OnError(object sender, ErrorEventArgs error)
    {
        sender.Error($"Сообщение: {error.GetException().Message}");
        sender.Error($"Метод:     {error.GetException().Source}");
        sender.Error($"Справка:   {error.GetException().HelpLink}");
        sender.Error($"Трассиовка:");
        sender.Error($" {error.GetException().StackTrace}");
    }


    /// <summary>
    /// Возвращает размер файла в байтах
    /// </summary>
    public static string ParseFileName(string FilePath)
    {
        int i1 = FilePath.LastIndexOf("/");
        int i2 = FilePath.LastIndexOf("\\");
        string sep = i1 > i2 ? "/" : "\\";
        string dir = FilePath.Substring(0, Math.Max(i1, i2));
        string file = FilePath.Substring(Math.Max(i1, i2) + 1);
        return file;
    }


    /// <summary>
    /// Возвращает путь к диерктории
    /// </summary> 
    public static string ParseDirName(string FilePath)
    {
        int i1 = FilePath.LastIndexOf("/");
        int i2 = FilePath.LastIndexOf("\\");
        string sep = i1 > i2 ? "/" : "\\";
        string dir = FilePath.Substring(0, Math.Max(i1, i2));
        string file = FilePath.Substring(Math.Max(i1, i2) + 1);
        return dir;
    }


    /// <summary>
    /// Возвращает размер файла в байтах
    /// </summary>
    public static string GetFileSize(string FilePath)
    {
        int i1 = FilePath.LastIndexOf("/");
        int i2 = FilePath.LastIndexOf("\\");
        string sep = i1 > i2 ? "/" : "\\";
        string dir = FilePath.Substring(0, Math.Max(i1, i2));


        string file = FilePath.Substring(Math.Max(i1, i2) + 1);

        DirectoryInfo directoryInfo = new DirectoryInfo(dir);

        FileInfo fileInfo = directoryInfo.GetFiles()
                .Where(f => f.FullName == FilePath)
                .FirstOrDefault();

        if (fileInfo == null)
            return "NotFound";
        return fileInfo.Length.ToString();
    }


    /// <summary>
    /// Прослушивание файловой системы
    /// </summary>        
    public static void WatchDir(string DirectoryAbsolutelyPath, 
        Action<object, FileSystemEventArgs> OnFileChanged, 
        Action<object, ErrorEventArgs> OnFileError)
    {
        /*  Результаты расчетов необходимо сохранить в текстовый документ в виде
                “<имя файла>-<имя операции>-<результат>”. */

        Info("Ожидание изменений файлов "+DirectoryAbsolutelyPath);
        using (var watcher = new FileSystemWatcher(DirectoryAbsolutelyPath))
        {
            watcher.NotifyFilter = NotifyFilters.Attributes
                                | NotifyFilters.CreationTime
                                | NotifyFilters.DirectoryName
                                | NotifyFilters.FileName
                                | NotifyFilters.LastAccess
                                | NotifyFilters.LastWrite
                                | NotifyFilters.Security
                                | NotifyFilters.Size;                 
            watcher.Created += (sender, evt) => {
                OnFileChanged(sender, evt);
            }; 
            watcher.Error +=   (sender, evt) => {
                OnFileError(sender,evt);
            };

            watcher.Filter = "*.*";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            watcher.Info("Нажмите ESC для завершения");
            char? ch = null;
            do
            {
                ch = Console.ReadKey().KeyChar;
                Console.WriteLine("Нажатие клавиши: "+ch);
            } while (ch!= '←');
        }
    }        
}
