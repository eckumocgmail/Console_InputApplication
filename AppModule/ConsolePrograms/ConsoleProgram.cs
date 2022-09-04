using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static InputApplicationProgram;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Пробрасывается в результате программной или пользовательской
/// отмены операции
/// </summary>
public class CancelException : Exception { }

/// <summary>
/// Пробрасывается в результате программной или пользовательской
/// отмены операции
/// </summary>
public class CompleteException : Exception { }

/// <summary>
/// Программа управления объектом через консоль
/// </summary>
public class ConsoleProgram<ServiceModel> : ConsoleProgram
{
    public override void Run()
    {
        RunInteractive<ServiceModel>();
    }
}



/// <summary>
/// Программа управления объектом через консоль
/// </summary>
public class ConsoleProgram: ProgressProgram
{
    [JsonIgnore]
    [NotMapped]
    public ILogger<ConsoleProgram> Logger = LoggerFactory.Create(options=>options.AddConsole()).CreateLogger<ConsoleProgram>();

    [JsonIgnore]
    [NotMapped]
    public object ProgramData;

    [JsonIgnore]
    [NotMapped]
    public MethodInfo ProgramAction;

    [JsonIgnore]
    [NotMapped]
    public Dictionary<string, object> ProgramArguments { get; set; }

    [JsonIgnore]
    [NotMapped]
    public object ActionResult { get; set; }

    
    public static bool DebugMode { get; private set; } = true;

    public static void RunInteractive( object target )
    {
        Type ProgramType = target.GetType();
        var console = new ConsoleProgram();
        console.ProgramData = target == null ? ProgramType.New() : target;
        while (true)
        {
            console.PrintProgramData();
            console.SelectNextAction();
            console.InputActionParameters();
            console.ShowExecuteActionResults();
        }
    }

    public static void RunInteractive<TypeOfPogram>( params string[] args)
    {
        RunInteractive<TypeOfPogram>((TypeOfPogram)typeof(TypeOfPogram).New(), args);
    }

    public static void RunInteractive<TypeOfPogram>(TypeOfPogram instance, params string[] args)
    {
        Type ProgramType = typeof(TypeOfPogram);
        var console = new ConsoleProgram();
        console.ProgramData = instance==null? ProgramType.New(): instance;
        while (true)
        {
            console.PrintProgramData();
            console.SelectNextAction();
            console.InputActionParameters();
            console.ShowExecuteActionResults();
        }
    }




    /// <summary>
    /// Интерфейс множественного выбора элементов из заданого множества
    /// </summary>
    /// <typeparam name="T">Тип выбираемого объекта</typeparam>
    /// <param name="applications">Множество доступное для выбора</param>
    /// <param name="p">Функция предосавляет заголовок элемента в списке</param>
    /// <returns>Выбранные элементы</returns>
    /// <exception cref="CancelException"></exception>
    public IEnumerable<T> CheckListTitle<T>(
        string title,
        IEnumerable<T> applications,
        Func<T, string> convert)
    {
        if(ProgramDialog.UserInteractive==false)
            return applications; 
        var result = new List<T>();
        Action complete = () => { throw new CompleteException(); };
        var items = applications
                    .Select(Item => new Dictionary<string, object> {
                        { "Item",Item},
                        { "Selected",true},
                        { "Label",convert(Item)}
                    }).ToList();
        int cursor = 0;
        ReadKeyRepeat<List<T>>(

            // before read
            () => {
                Console.WriteLine("\n" + "\n  " + title + "\n");
                int ctn = 0;
                foreach (dynamic ListItemTitle in items)
                {
                    if (ctn++ == cursor)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"  [{((bool)(ListItemTitle["Selected"]) ? "x" : " ")}]  {ListItemTitle["Label"]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  [{((bool)(ListItemTitle["Selected"]) ? "x" : " ")}]  {ListItemTitle["Label"]}");
                    }

                }
            },

            // onKeyPressed
            (key) => {
                Clear();
                Console.WriteLine(key + " " + cursor);
                switch (key.ToString().Trim())
                {
                    case "Enter": throw new CompleteException();
                    case "Spacebar":
                        items.ToArray()[cursor]["Selected"] = ((bool)items.ToArray()[cursor]["Selected"]) ? false : true;
                        break;
                    case "UpArrow":
                        cursor = cursor - 1;
                        if (cursor < 0)
                            cursor = applications.Count() - 1;
                        break;
                    case "DownArrow":
                        cursor = cursor + 1;
                        if (cursor >= applications.Count())
                            cursor = 0;
                        break;
                    default: break;
                }
            },

            // onCompleted
            () => {
                return result = items.Where(item => ((bool)item["Selected"])).Select(item => (T)item["Item"]).ToList();
            },

            // onCanceled
            () => {
            }
        );
        return result;
    }

    public IEnumerable<T> CheckListTitle<T>(
        string title,
        IEnumerable<T> applications,
        IEnumerable<T> selected,
        Func<T, string> convert)
    {
        if (ProgramDialog.UserInteractive == false)
            return applications;
        var result = new List<T>();
        Action complete = () => { throw new CompleteException(); };
        var items = applications
                    .Select(Item => new Dictionary<string, object> {
                        { "Item",Item},
                        { "Selected",selected.Contains(Item)},
                        { "Label",convert(Item)}
                    }).ToList();
        int cursor = 0;
        ReadKeyRepeat<List<T>>(

            // before read
            () => {
                Console.WriteLine("\n" + "\n  " + title + "\n");
                int ctn = 0;
                foreach (dynamic ListItemTitle in items)
                {
                    if (ctn++ == cursor)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"  [{((bool)(ListItemTitle["Selected"]) ? "x" : " ")}]  {ListItemTitle["Label"]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  [{((bool)(ListItemTitle["Selected"]) ? "x" : " ")}]  {ListItemTitle["Label"]}");
                    }

                }
            },

            // onKeyPressed
            (key) => {
                Clear();
                Console.WriteLine(key + " " + cursor);
                switch (key.ToString().Trim())
                {
                    case "Enter": throw new CompleteException();
                    case "Spacebar":
                        items.ToArray()[cursor]["Selected"] = ((bool)items.ToArray()[cursor]["Selected"]) ? false : true;
                        break;
                    case "UpArrow":
                        cursor = cursor - 1;
                        if (cursor < 0)
                            cursor = applications.Count() - 1;
                        break;
                    case "DownArrow":
                        cursor = cursor + 1;
                        if (cursor >= applications.Count())
                            cursor = 0;
                        break;
                    default: break;
                }
            },

            // onCompleted
            () => {
                return result = items.Where(item => ((bool)item["Selected"])).Select(item => (T)item["Item"]).ToList();
            },

            // onCanceled
            () => {
            }
        );
        return result;
    }

  
    public string InputString(ref string[] args, string name )
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine();
        Console.Write(name+">");
        Console.ResetColor();
        if (ProgramDialog.UserInteractive)
        {
            string result = Console.ReadLine();
            return result;
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
            Console.WriteLine(value);
            return value;
        }
    }

    public bool ConfirmContinue(ref string[] args, string message = "" )
    {

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n");
        Info(message);


        Console.WriteLine("\nДля продолжения нажмите ENTER....");
        Console.ResetColor();
        if (ProgramDialog.UserInteractive)
        {
            bool result = String.IsNullOrWhiteSpace(Console.ReadLine());
            Clear();
            return result;
        }
        else
        {
            if (args.Length == 0)
            {
                if (DebugMode)
                {
                    bool result = String.IsNullOrWhiteSpace(Console.ReadLine());
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


    public void ConfirmExecute(string Name, Action Todo)
    {
        Clear();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n");


        Console.WriteLine("\nРазрешите выполнить операцию: "+ Name);
        Console.Write(">");
        Console.ResetColor();
        if((Console.ReadKey().KeyChar + "").ToLower() == "Y")
        {
            Todo();
        }
    }


    /// <summary>
    /// Передача управления клавиатуре
    /// </summary> 
    public T ReadKeyRepeat<T>(
        Action beforeRead,
        Action<object> onKeyPressed,
        Func<T> onCompleted,
        Action onCanceled)
    {
        try
        {
            while (true)
            {
                beforeRead();
                onKeyPressed(Console.ReadKey().Key);
            }
        }
        catch (CompleteException)
        {
            return onCompleted();
        }
        catch (CancelException ex)
        {
            this.Error(ex.Message);
            throw;
        }
    }

    /// <summary>
    /// вывод результатов процедуры
    /// </summary>
    public void ShowExecuteActionResults()
    {
        WriteLine(ActionResult.ToJsonOnScreen());        
        WriteLine("Нажмите клавишу для продолжения ... ");
        ReadKey();
    }


    /// <summary>
    /// ввод параметров процедуры
    /// </summary>
    public void InputActionParameters()
    {      
        Clear();
        PrintProgramData();
        WriteLine($"\n {(ProgramAction!=null?ProgramAction.Name:"")}");
        int n = 1;
        var args = new List<object>();
        ProgramArguments = new Dictionary<string, object>();
        foreach(ParameterInfo par in ProgramAction.GetParameters())
        {
            WriteLine($"{n++}) {par.Name}: {par.ParameterType.Name}>");            
            object result = TextDataSetter.ToType(ReadLine(), par.ParameterType);
            ProgramArguments[par.Name] = result;
            args.Add(result);
        }
        try
        {
            ActionResult = ProgramAction.Invoke(ProgramData, args.ToArray());
        }
        catch (Exception ex)
        {
            throw new Exception($"Исключение проброшено из метода {ProgramData.GetTypeName()}.{ProgramAction.Name}",ex);
        }
    }


    public void SelectNextAction()
    {
        try
        {
            WriteLine($"\n\n{"Выберите действие"}");


            int i = 1;
            var methods = ProgramData.GetType().GetOwnMethodNames();
            WriteLine(0 + ")" + "назад");
            foreach (var next in methods)
            {
                string label = ProgramData.GetType().GetMethodLabel( next );
                Console.WriteLine(i + $"){label}");
                i++;
            }
            string action = SingleSelect(methods);
            ProgramAction = ProgramData.GetType().GetMethods().Where(m => m.Name == action).FirstOrDefault();
            if(ProgramAction != null)
            WriteLine(ProgramAction.GetType().GetMethodLabel( ProgramAction.Name));
        }
        catch (CancelException)
        {
            throw;
        
        }catch (Exception ex)
        {
            WriteLine(ex);
        }
    }

    public static T SingleSelect<T>( IEnumerable<T> items) where T : class
    {
        try
        {
            int i = 1;

            ConsoleKeyInfo key;
            do
            {
                WriteLine(0 + ")" + "назад");
                foreach (var next in items)
                {
                    string label = next.ToString();
                    Console.WriteLine(i + $"){label}");
                    i++;
                }
                key = Console.ReadKey();
            } while (key.KeyChar < '0' || key.KeyChar > items.Count().ToString().ToCharArray()[0]);
            int index = int.Parse(key.KeyChar.ToString()) - 1;
            if (index == -1)
            {
                throw new CancelException();
            }

            return items.ToArray()[index];
        }catch(Exception ex)
        {
            return null;
        }
    }

    public void PrintProgramData()
    {
        if (ProgramData == null)
            throw new Exception("Нужно присвоить значение свойству ProgramData");
        Type ProgramType = ProgramData.GetType();     
      
        WriteLine($"[{ProgramType.GetTypeName()}] Свойства: ");
        ProgramType.GetProperties().ToList().ForEach(prop => {
            Write($"\t");
            Write($"{prop.Name}");
            WriteLine($"=");
            WriteLine(ProgramData.GetType().GetProperty(prop.Name).GetValue(ProgramData));           
        });

  
    }


    /// <summary>
    /// 
    /// </summary>    
    public TResult Invoke<TService,TResult>(string action, params object[] args)
    {
        object injected = typeof(TResult).New();
        if (injected == null)
            throw new Exception("Не удалось найти сервис " + typeof(TService).Name);
        TService instance = (TService)injected;
        var methodInfo = typeof(TService).GetMethod(action);
        if (methodInfo == null)
            throw new Exception($"Не удалось найти метод {action} у сервиса " + typeof(TService).Name);
        try
        {
            object result = methodInfo.Invoke(injected, args);
            return (TResult)result;
        }
        catch (Exception ex)
        {
            string text = "";
            if (args != null)
            {
                foreach (var arg in args)
                {
                    text += arg + ",";
                }
                if (args.Length > 0)
                {
                    text = text.Substring(0, text.Length - 1);
                }
            }
            throw new Exception($"Не удалось выполнить метод {action} у сервиса " + typeof(TService).Name +
                " с аргументами: " + text, ex);
        }
    }

    public virtual void Run() { }

    /// <summary>
    /// 
    /// </summary>    
    public static void RunConsoleProgram(params string[] args)
    {
        while (true)
        {
            try
            {
                Clear();
                Info("Укажите путь к сборке dll или exe файлу чтобы подключить консоль управления.");
                string path = Console.ReadLine();
                var assembly = Assembly.LoadFile(path);
                //TODO
            }
            catch(Exception ex)
            {
                ex.ToString().WriteToConsole();
            }
        }
    }


    
}


public class ConsoleProgramTest : TestingUnit
{


    public ConsoleProgramTest(TestingUnit parent) : base(parent)
    {
        canRunAsConsole();
    }

    class ClassBuilder
    {
        string NameSpace;
        string Name;
        string Source
        {
            get =>


                $@"namespace {NameSpace}" + @"{
                    public class " + Name + @"{
                    }
                }";

        }
    }

    private void canRunAsConsole()
    {
        ConsoleProgram.RunInteractive<MyControllerModel>();
    }
}
