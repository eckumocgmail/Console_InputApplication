using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

[Label("Сравнение файлов")]
[Description("Программа сравнивает текстовые файлы. Если в аргументы передано несколько файлов, то программа сравнивает каждый с каждым. Результат сравнения содержит сведения об изменениях.")]
public class CompareProgram: ConsoleProgram<CompareProgram>
{
      
    [Label("Событие изменения текста")]
    public class TextChangedEvent
    {
            
        public Dictionary<int, string> before { get; set; } = new Dictionary<int, string>();
        public Dictionary<int, string> current { get; set; } = new Dictionary<int, string>();
    }


    [Label("Выполнение инструкции через командную строку")]
    [Description("Выполнение операции через коммандную оболоочку в отдельном процессе.")]
    private string CmdExec(string command)
    { 
        string result = "";
        command = command.ReplaceAll("\n", "").ReplaceAll("\r", "").ReplaceAll(@"\\", @"\").ReplaceAll(@"//", @"/");

        ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " + command);

        info.RedirectStandardError = true;
        info.RedirectStandardOutput = true;
        info.UseShellExecute = false;
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
        string response = process.StandardOutput.ReadToEnd();
        result = response.ReplaceAll("\r", "\n");
        result = result.ReplaceAll("\n\n", "\n");
        while (result.EndsWith("\n"))
        {
            result = result.Substring(0, result.Length - 1);
        }
        process.WaitForExit();
        return result;
    }


    /// <summary>
    /// Сравнение файлов
    /// </summary>
    /// <param name="file1"></param>
    /// <param name="file2"></param>
    /// <returns></returns>
    public TextChangedEvent Compare(string file1, string file2)
    {

        string message = CmdExec(@$"fc /N ""{file1}"" ""{file2}""");
        return OnCompareResult(message,file1,file2);
    }



    /// <summary>
    /// 
    /// </summary>
    private TextChangedEvent OnCompareResult(string message, string file1, string file2)
    {
        WriteLine(message+"\n\n");

        var result = new TextChangedEvent()
        {

        };

        int sepCount = 0;
        string[] lines = message.ReplaceAll("\r", "\n").ReplaceAll("\n\n", "\n").Split("\n");
        for(int i=0;i< lines.Length; i++)
        {
                
            if (lines[i].StartsWith("*****"))
            {
                sepCount++;                     
                continue;
            }
                    
            if (sepCount == 1)
            { 
                //WriteLine($"f1 {lines[i]}");
                string text = lines[i].Trim(); 
                int pos = text.Substring(0, text.IndexOf(":")).ToInt();
                string value = text.Substring(text.IndexOf(":") + 1);
                result.current[pos] = value;
            } 
            else if(sepCount == 2)
            {
                //WriteLine($"f2 {lines[i]}");
                string text = lines[i].Trim();
                int pos = text.Substring(0, text.IndexOf(":")).ToInt();
                string value = text.Substring(text.IndexOf(":") + 1);
                result.before[pos] = value;
            }
        }
        return result ;
    }




    /// <summary>
    /// Проверка
    /// </summary>
    public static void Test()
    {
        string message = @"
Сравнение файлов D:\NETPROJECTS\WIN6_MOVIE\MYBUSINESSAPPLICATIONMODEL\MyBusinessApplicationView.cs и D:\SYSTEM-CONFIG\NETPROJECTS\WIN6_MOVIE\MYBUSINESSAPPLICATIONMODEL\MYBUSINESSAPPLICATIONVIEW.CS
***** D:\NETPROJECTS\WIN6_MOVIE\MYBUSINESSAPPLICATIONMODEL\MyBusinessApplicationView.cs
7:      {
8:          public string IP { get; set; }
9:          public string URL { get; set; }
***** D:\SYSTEM-CONFIG\NETPROJECTS\WIN6_MOVIE\MYBUSINESSAPPLICATIONMODEL\MYBUSINESSAPPLICATIONVIEW.CS
7:      {
8:          public string URL { get; set; }
*****";

        var comparator = new CompareProgram();

        comparator.OnCompareResult(message, $@"D:\NETPROJECTS\WIN6_MOVIE\MYBUSINESSAPPLICATIONMODEL\MyBusinessApplicationView.cs", $@"D:\SYSTEM-CONFIG\NETPROJECTS\WIN6_MOVIE\MYBUSINESSAPPLICATIONMODEL\MYBUSINESSAPPLICATIONVIEW.CS").ToJsonOnScreen().WriteToConsole();

    }
}



/******

bool first = true;
                            bool toCurrent = false;
                            bool toBefore = false;
                            var current = new Dictionary<int, string>();
                            var before = new Dictionary<int, string>();
                            string result = Comp(evt.FullPath, newpath);
                            foreach ( string line in result.Split("\n"))
                            {
                                
                                WriteLine(line);
                                if(first)
                                {
                                    first = false;
                                    continue;
                                }
                                if (line.StartsWith("***** "))
                                {
                                    //WriteLine(line.Substring("***** ".Length).Trim());
                                    //WriteLine(evt.FullPath);
                                    /*string parsedPath = l ine.Substring("***** ".Length).Trim();* /
                                    //if (evt.FullPath.ToUpper().Equals(parsedPath.ToUpper()))
                                    if (toCurrent == false)
                                    {
                                        toCurrent = true;
                                        toBefore = false;
                                    }
                                    else
                                    {
                                        toBefore = true;
                                        toCurrent = false;

                                    }
                                    continue;
                                }
                                else
                                {
                                    string text = line.Trim();
                                    int iint = text.IndexOf(":");
                                    if (iint != -1)
                                    {
                                        int pos = text.Substring(0, iint).ToInt();
                                        string value = text.Substring(text.IndexOf(":") + 1);
                                        current[pos] = value;
                                    }
                                   
                                }
                                

                            }

                            (new { before = before, current = current }).ToJsonOnScreen().WriteToConsole();

*/ 
