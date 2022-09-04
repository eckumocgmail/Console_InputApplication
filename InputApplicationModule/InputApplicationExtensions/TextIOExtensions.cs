using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class TextIOExtensions
{

    public static string GetFileExt(this string path){
        int i = path.LastIndexOf(".");
        return i==-1? "": path.Substring(i+1).ToLower();
    }


    /// <summary>
    /// Создаёт файл и все необходимые директории
    /// </summary>
    public static bool CreateFile( this string path )
    {
        path.Info($"CreateFile({path})");
        List<string> names = new List<string>();
        string[] arr = path.Split(Path.DirectorySeparatorChar);
        for (int i=0; i<arr.Length; i++ )
        {
            string name = arr[i];
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Не удалось создать файл по пути "+path,"path");
            names.Add(name);
            string pdir = "";
            foreach(string next in names)
            {
                pdir += Path.DirectorySeparatorChar + next;
            }
            pdir = pdir.Substring(1);
            if( i==(arr.Length-1))
            {                
                //create dir
                if (System.IO.File.Exists(pdir) == false)
                {
                    "{}".WriteToFile(pdir);
                }
            }
            else
            {
                //create dir
                if(System.IO.Directory.Exists(pdir) == false)
                {
                    System.IO.Directory.CreateDirectory(pdir);                    
                }
            }

        }
        return System.IO.File.Exists(path);
    }

    /// <summary>
    /// Получение списка файлов
    /// </summary>    
    public static bool Exists(this string path)
    {
        return System.IO.Directory.Exists(path) || System.IO.File.Exists(path);
    }

    /// <summary>
    /// Получение списка файлов
    /// </summary>    
    public static bool IsImage(this string path)
    {

        var ImageFileExts = new List<string>(){ "jpg","png","ico" };
        return (System.IO.Directory.Exists(path) || System.IO.File.Exists(path)) && ImageFileExts.Contains(path.GetFileExt());
    }

    /// <summary>
    /// Получение списка файлов
    /// </summary>    
    public static List<string> GetFiles(this string path)
    {
        return new List<string>(System.IO.Directory.GetFiles(path));
    }
    public static List<string> GetFiles(this string path, string pattern)
    {
        return new List<string>(System.IO.Directory.GetFiles(path, pattern));
    }
    public static List<string> GetFilesInAllSubdirectories(this string path)
    {
        return new List<string>(System.IO.Directory.GetFiles(path, "*.*", SearchOption.AllDirectories));
    }
    public static List<string> GetFilesInAllSubdirectories(this string path, string pattern)
    {
        return new List<string>(System.IO.Directory.GetFiles(path, pattern, SearchOption.AllDirectories));
    }

    /// <summary>
    /// Вывод в консоль
    /// </summary>    
    public static string Log(this string path)
    {
        path.Info(path);
        return path;
    }

    /// <summary>
    /// Вывод в консоль
    /// </summary>    
    public static string WriteOrangle(this string path)
    {
        Console.ForegroundColor= ConsoleColor.Yellow;
        path.Info(path);
        Console.ResetColor();
        return path;
    }


    /// <summary>
    /// Получение списка файлов
    /// </summary>    
    public static string WriteToFile(this string text, string path)
    {
        System.IO.File.WriteAllText(path,text);
        return text;
    }
    public static string AppendToFile(this string text, string path)
    {
        System.IO.File.AppendAllText(path, text);
        return text;
    }



    /// <summary>
    /// Получение списка файлов
    /// </summary>    
    public static string ReadText(this string path)
    {
        return System.IO.File.ReadAllText(path);
    }
    /// <summary>
    /// Получение списка файлов
    /// </summary>    
    public static void WriteText(this string path, string text)
    {
        System.IO.File.WriteAllText(path,text);
    }
    public static long FileSize(this string path)
    => new FileInfo(path).Length;
    public static bool FileExists(this string path)
        => System.IO.File.Exists(path);


    public static long WriteBytes(this string path, List<byte> data, Action<long, long> onprogress)
    {    
        long len = data.Count();
        long pos = 0;

        using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            foreach (var next in data)
            {
                stream.WriteByte(next);
                onprogress((long)Math.Floor((decimal)++pos)/(len/100),100);
            }
            stream.Flush();
        }
        return pos;      
    }


    /// <summary>
    /// Получение списка файлов
    /// </summary>    
    public static List<byte> Read(this string path ){
        return path.ReadBytes((r,l)=>{
          
        });
    }
    public static List<byte> ReadBytes(this string path, Action<long,long> onprogress)
    {
        var bytes = new List<byte>();
        if (path.FileExists())
        {
            int bufferSize = 1024 * 1024;
            var burred = new byte[bufferSize];
            
            using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
            {
                long l = stream.Length;
                long v = 0;
                while (v < l)
                {
                    int readed = stream.Read(burred);
                    l-= readed;
                    v += readed;
                    for(int i=0; i<readed; i++)
                    {
                        bytes.Add(burred[i]);

                        
                    }
                }
            }
        }
        return bytes;
    }
}