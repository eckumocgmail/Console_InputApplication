using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

using static System.Console;
using static System.IO.Directory;
using static DirectoryProgram; 

/// <summary>
/// Создаёт командные файлы
/// </summary>
public class SystemProgram
{
    public static string DistrDir = @"D:\\System-Config\\MyApplications\\SystemConsole\\";
    public static string CommandsDir = @"D:\\Commands";

    /// <summary>
    /// История
    /// </summary>
    public static List<string> History = new List<string>();

    /// <summary>
    /// ВНИМАНИЕ!!!
    /// Много зависимостей
    /// </summary>
    public static void Publish()
    {
        bool dist = false;
        WriteLine(GetCurrentDirectory());
        var files =  GetFiles(GetCurrentDirectory());
        foreach(var filename in files)
        {
            if (filename.IndexOf("Console0_System.exe") != -1)
            {
                dist = true;
                break;
            }
        }
        if (dist)
        {
            Console.WriteLine("Устанавливаю программу в систему ... ");
            if (Exists(DistrDir) == false)
            {
                CreateDirectory(DistrDir);
            }
            foreach (var file in GetFiles(GetCurrentDirectory()))
            {
                string target = file.Replace(GetCurrentDirectory(), DistrDir);
                System.IO.File.Copy(file, target, true);
                    
            }
            System.IO.File.Copy(DistrDir+ "Console0_System.exe", DistrDir+"todo.exe", true);
        }
               
         
        Console.WriteLine("Нажмите любую клавишу для продолжения ... ");
        Console.ReadKey();
    }


    /// <summary>
    /// Умное считывание с консоли        
    /// </summary>
    public static void ReadCommand()
    {
        //string command = "";
        var startedAt = Console.GetCursorPosition();
        ConsoleKeyInfo key; 
        do
        {
            key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.Backspace:
                    break;
                default: break;
            }
            Console.SetCursorPosition(startedAt.Left, startedAt.Top);

        } while(key.Key!=ConsoleKey.Enter);            
    }

    public static IEnumerable<string> GetSystemPrograms()
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// Программа и интерфейсом CLI
    /// </summary>
    public static int Run( params string[] args )
    {
        RunModel();
        Publish();
        RunTests();
        string command = "";
        do
        {
            Console.ForegroundColor = ConsoleColor.White;
            //Clear();
            Console.WriteLine("\n");
            if (History.Count() > 0)
            {
                Console.WriteLine("операции:");
                History.Select(cmd => ("    " + cmd)).ToList().ForEach(Console.WriteLine);
            }
            Console.WriteLine("\nдействия:");
            Console.WriteLine("  back- удалить последнюю операцию");
            Console.WriteLine("  programs- прикладные программы");
            Console.WriteLine("  save- сохранить последовательность операций");
            Console.WriteLine("  restart- перезапустить программу");
            Console.WriteLine("  exit- выход");
            Console.Write("\n>");

            //bool first = true;
            //bool completed = true;
            switch (command = Console.ReadLine())
            {
                case "help":
                    Console.WriteLine(GetHelpMessage());
                    break;
                case "restart":
                    History.Clear();
                    break;
                case "clear":
                    History.Clear();
                    break;
                case "programs":
                    DirectoryProgram.GetProgramFiles().ToList().ForEach(Console.WriteLine);
                    break;
                case "save":
                    {
                        Console.Write("имя пакета операций >");
                        string name = Console.ReadLine();
                        string text = "";
                        foreach (string line in History)
                            text += line + " && ";
                        if (text.EndsWith(" && "))
                            text = text.Substring(0, text.Length - 4);
                        System.IO.File.WriteAllText(CommandsDir + name + ".bat", text);
                        break;
                    }
                case "back":
                    if (History.Count() > 0)
                        History.RemoveAt(History.Count() - 1);
                    break;
                case "exit":
                    return 1;
                default:
                    switch ( PickUpCommandBasedOnUserInput(command))
                    {
                        case "restart":
                            History.Clear();
                            break;
                        case "clear":
                            History.Clear();
                            break;
                        case "programs":
                            DirectoryProgram.GetProgramFiles().ToList().ForEach(Console.WriteLine);
                            break;
                        case "save":
                            {
                                Console.Write("имя пакета операций >");
                                string name = Console.ReadLine();
                                string text = "";
                                foreach (string line in History)
                                    text += line + " && ";
                                if (text.EndsWith(" && "))
                                    text = text.Substring(0, text.Length - 4);
                                System.IO.File.WriteAllText(CommandsDir + name + ".bat", text);
                                break;
                            }
                        case "back":
                            if (History.Count() > 0)
                                History.RemoveAt(History.Count() - 1);
                            break;
                        case "exit":
                            return 1;
                        default:
                            break;

                    }
                    break;




            }
            History.Add("cmd /c \"" + command + "\"");
            Console.WriteLine(Execute(command));
            Console.WriteLine("Нажмите любую клавишу для продолжения ... ");
            Console.ReadKey();



        } while (command.ToUpper() != "exit");
        return -1;
    }


    /// <summary>
    /// Формирвоание модели
    /// </summary>
    private static void RunModel()
    {
        var model = new MyApplicationModel();
        model.AddAction(new MyControllerModel()
        {
            Name = "Home",
            Actions = new Dictionary<string, MyActionModel>() {
                { "Index", new MyActionModel() }
            }

        });
        model.ToString().WriteToConsole();

    }

    /// <summary>
    /// Возвращает сообщение со справочяной информацией
    /// </summary>        
    private static string GetHelpMessage()
        => @"Привет бро! Знакомься! Это наш новый софт. Если ты будешь применять его в своей работе сможешь зарабатывать гораздо больше. Если я не прав звони боту, если тупой пиши мне почту пока только так, хотя пока мне ещё никто не звонил =). 7-904-334-1124";

    /// <summary>
    /// Выполняется попытка подбора необходимой операции предполагая, что
    /// присутствует маловероятная погрешность при ввода.        
    /// </summary>
    /// <param name="command">с погрешностью</param>     
    public static string PickUpCommandBasedOnUserInput( params object[] command) => @concatenateAsString(@translite(command));

    /// <summary>
    /// Формирование строки
    /// </summary>
    private static string concatenateAsString(IEnumerable<object> enumerable)
    {
        string result = "";
        foreach(var item in enumerable)
        {
            result += " " + item;
        }
        return result;
    }

    /// <summary>
    /// Транслирование латиницу
    /// </summary>
    private static IEnumerable<object> translite(params object[] command)
    {
        var resutls = new List<object>();
        foreach(var next in command)
        {
            if(next is String)
            {
                resutls.Add(enru_or_ruen((string)next));
            }
        }
        return resutls;
    }


    /// <summary>
    /// Транслирование в латиницу
    /// </summary>
    private static string enru_or_ruen(string command)
    {
        return IsRus(command) ? TransliteToEn(command) : IsEn(command) ? TransliteToRu(command) : throw new ArgumentException("command");
    }

    /// <summary>
    /// Транслирование в кирилицу        
    /// </summary>   
    private static string TransliteToRu(string command)
    {
        return command;
    }

    /// <summary>
    /// Признак принадлежности к латинице
    /// </summary>        
    private static bool IsEn(string command) => 
        command.IsEng();

    /// <summary>
    /// Транслирование в латиницу        
    /// </summary>        
    private static string TransliteToEn(string command) =>
        throw new ArgumentException();

    /// <summary>
    /// Кирилица или нет
    /// </summary>
    private static bool IsRus(string command) => 
        command.IsRus();

    /// <summary>
    /// Тестирование методов класса SystemProgram,ActiveDirectoryProgram
    /// </summary>        
    private static void RunTests()
    {
        foreach(var path in DirectoryProgram.GetProgramFiles())
        {
            WriteLine();
                
        }
    }


    /// <summary>
    /// Исполнение команды
    /// </summary>                
    public static string Execute(string command)
    {
        InputApplicationProgram.Info(command);

        ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " + command);

        info.RedirectStandardError = true;
        info.RedirectStandardOutput = true;
        info.UseShellExecute = false;
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
            
        string response = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return response;
    }


    /// <summary>
    /// Исполнение команды
    /// </summary>                
    public static async Task RunInteractive(string command)
    {
        if(InputApplicationProgram.ConfirmContinue("Выполнить комманду: " + command))
        {
            ProcessStartInfo info = new ProcessStartInfo(@"%SystemRoot%\system32\cmd.exe", "/C " + command);

            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
            process.OutputDataReceived += (sender, evt) => { };
            await process.WaitForExitAsync();
        }


        
            
    }
}
