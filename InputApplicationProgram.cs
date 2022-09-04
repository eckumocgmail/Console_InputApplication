using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Label("Программма ввода данных")]
public class InputApplicationProgram: ProgramDialog
{
    public static void Start(ref string[] args)
    {
        Test(ref args);
        Run(ref args);
    }

    private static void Test(ref string[] args)
    {
        Console.Title = "Тестирование";
        var program = new TestingUnit();
        program.Info(args.ToJsonOnScreen());
        program.Append(new AttributesInfoTest());
        program.Append(new AssemblyExtensionsTest());
        program.Append(new GetEngWordsTest());
        program.Append(new TextConvertExtensionsTest());
        program.Append(new TextFactoryExtensionsTest());
        program.Append(new TextIOExtensionsTest());
        program.Append(new TextLangExtensionsTest());
        program.Append(new TextNamingExtensionsTest());
        program.Append(new TextNamingTest());
        program.Append(new TextValueExtensionsTest());
        program.Append(new AttributesInputTest());
        program.DoTest().ToDocument().WriteToConsole();
    }

    public static void Run(ref string[] path)
    {
        Console.Title = "Выполнение";
        ConsoleProgram.RunInteractive<AppBuilderService>();
    }
    //[Label("Интерактивный режим предполагает что все необходимые данные будут введены пользователем")]
    
      

    public new static string InputString(ref string[] args, string name)
    {
        return Instance.InputString(ref args, name);
    }

    public static void Info(params object[] args) => GetApp().Info(args);
    public static void Warn(params object[] args) => GetApp().Warn(args);
    public static void Error(params object[] args) => GetApp().Error(args);

      
    public static void Clear()
    {
        Console.Clear();    
        Console.ResetColor();
    }

    public static string GetWrk()
        => System.IO.Directory.GetCurrentDirectory();
    public static IEnumerable<string> GetFiles()
        => GetWrk().GetFiles();
    public static IEnumerable<string> GetDlls()
        => GetWrk().GetFilesInAllSubdirectories("*.dll");
    public static IEnumerable<string> GetJsonFiles()
        => GetWrk().GetFiles().Where(f => f.ToLower().EndsWith(".json"));

    public static IEnumerable<string> GetIniFiles()
        => GetWrk().GetFiles().Where(f => f.ToLower().EndsWith(".ini"));


    public static void Write(object arg)
        => Console.Write(arg);

    public static void Write(object[] args)
    {
        foreach (var arg in args)
        {
            WriteLine(arg);
        }
    }
    private static ConsoleProgram Instance = new ConsoleProgram();

    public InputApplicationProgram( ) : this(AppProviderService.GetInstance(), System.IO.Directory.GetCurrentDirectory()) { }
    public InputApplicationProgram(IServiceProvider provider, string path) : base(provider, path)
    {
    }

    public static string ReadLine() => Console.ReadLine();
    public static void ConfirmExecute(string name, Action todo)
        => Instance.ConfirmExecute(name, todo);
    public static ConsoleKeyInfo ReadKey() => Console.ReadKey();
    public static ConsoleProgram @Get() => Instance;
    public static void Exit() => Process.GetCurrentProcess().Kill();
    public static T Wait<T>(string title, Func<T> todo)
            => ProgressProgram.Wait<T>(title, todo);

    public new static IEnumerable<T> CheckListTitle<T>(string title, IEnumerable<T> applications, Func<T, string> convert)
        => Instance.CheckListTitle<T>(title, applications, convert);

    public new static IEnumerable<T> CheckListTitle<T>(string title, IEnumerable<T> applications, IEnumerable<T> selected, Func<T, string> convert)
        => Instance.CheckListTitle<T>(title, applications, selected, convert);


    public static string GetDirectoryName(string path) => Path.GetDirectoryName(path);
    public static string GetProcessName() => Process.GetCurrentProcess().ProcessName;
    public static string Combine(params string[] path) => System.IO.Path.Combine(path);
    public static object GetApp() => Instance;
    public static void WriteYellow(params object[] args)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        WriteLine(args);
        Console.ForegroundColor = ConsoleColor.White;

    }
    public static void WriteLine(params object[] args)
    {
        foreach (var arg in args)
        {
            if (arg is IEnumerable && !(arg is String))
            {
                foreach (var arg2 in ((IEnumerable)arg))
                {
                    WriteLine(arg2);
                }

            }
            else
            {
                Console.WriteLine(arg);
            }

        }
    }
    public static bool ConfirmContinue(string message = "")
    {
        string[] args = new string[] { };
        return ConfirmContinue(ref args, message);
    }
    public static bool ConfirmContinue(ref string[] args, string message = "")
    {

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n");
        Info(message);


        Console.WriteLine("\nДля продолжения нажмите ENTER....");
        Console.ResetColor();
        if (UserInteractive)
        {
            bool result = String.IsNullOrWhiteSpace(Console.ReadLine());
            Clear();
            return result;
        }
        else
        {
            if (args.Length == 0)
            {
                if (ConsoleProgram.DebugMode)
                {
                    bool result = String.IsNullOrWhiteSpace("");
                    Clear();
                    return result;
                }
                else
                {
                    throw new Exception("Не достаточно аргументов");
                }
            }
            else
            {
                string value = args[0];
                var argsOut = new string[args.Length - 1];
                for (int i = 1; i < args.Length; i++)
                {
                    argsOut[i - 1] = args[i];
                }
                args = argsOut;
                return value.Trim().ToLower() == "1" || value.Trim().ToLower() == "true";
            }
        }
    }
} 
