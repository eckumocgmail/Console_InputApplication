using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


using static InputApplicationProgram;

/// <summary>
/// Программа выполняет редактирование файла конфигурации,
/// состоящего массива комманд с доступом
/// </summary>
public class ProgramConfiguration: ProgramHistory
{

    public static string _ProgramConfigurationFile { get;set; } = "appsettings.json";
    public static string _ProgramWorkDirectory = System.IO.Directory.GetCurrentDirectory();
    public static string GetProgramWorkDirectory() => _ProgramWorkDirectory;
    public static void SetProgramWorkDirectory(string value) => _ProgramWorkDirectory=value;
    public static string GetProgramDirectoryDefault() => $@"D:\System-Config\ProgramsExt";
        
    public static string SetProgramConfigurationFileName(string value) => _ProgramConfigurationFile=value;
    public static string GetProgramConfigurationFileName() => _ProgramConfigurationFile;
    public static string GetProgramConfigurationFile()
        => Path.Combine(
            GetProgramWorkDirectory(),
            GetProgramConfigurationFileName()
        );


    /// <summary>
    /// Правила обработки сообщений, считаны из файла или наоборот записанные в файл
    /// </summary>
    public static IDictionary<string, List<string>> RuleSet = new Dictionary<string, List<string>>();

    public ProgramConfiguration(IServiceProvider provider, string path) : base(provider, path)
    {
    }



    /// <summary>
    /// Выполнение программы через 
    /// </summary>
    /// <param name="args">
    ///   1-ый аргумент должен содержать путь к файлу конфигурации
    /// </param>
    public static new void Run(params string[] args)
    {
        if( args.Length != 1)
        {
            Warn("Необходимо указать 1 аргумент содержащий путь к файлу конфигурации");
        }
        SetProgramConfigurationPath(args[0]);
        OnConfigurationBuilderStarted( );          
    }

        
    /// <summary>
    /// Устанавливает параметр пути к файлу конфигураци
    /// </summary>        
    public static void SetProgramConfigurationPath(string path)
    {
        int i1 = path.LastIndexOf("/");
        int i2 = path.LastIndexOf("\\");
        string sep = i1>i2? "/": "\\";
        string dir = path.Substring(0, Math.Max(i1, i2));
        string file = path.Substring(Math.Max(i1, i2)+1);
        SetProgramWorkDirectory(dir);
        SetProgramConfigurationFileName(file);
    }

    


    /// <summary>
    /// Вход в консоль редактора конфигурации
    /// </summary>        
    public static void OnConfigurationBuilderStarted( )
    {            
        NextState("OnConfigurationBuilderStarted");
        Info("Конфигурация директории: "+GetProgramWorkDirectory());
            
        ReadRules();
        OutputRules();

        var operations = new List<string>(
            RuleSet.Count()>0?
                new string[] {
                    "назад",
                    "сбросить конфигурацию",
                    "добавить правило",
                    "исключить правило"}:
                new string[] {
                    "назад",
                    "сбросить конфигурацию",
                    "добавить правило" }
        );
        switch (SingleSelect(operations.ToArray()))
        {
            case "назад":
                Clear();
                GoBack();                     
                break;

            case "сбросить конфигурацию":
                Clear();
                OnSetDafaults();
                break;

            case "добавить правило":
                Clear();
                OnAddRule();
                break;

            case "исключить правило":
                Clear();
                OnRemoveRule();
                break;
 
        };
    }


    /// <summary>
    /// Добавление правила обработки события нового документа
    /// </summary>
    public static void OnAddRule( )
    {
        NextState("OnAddRule");

        ReadRules();
        OutputRules();
        InputRules(ValidateRule);
        WriteRules();

        ReturnState();
    }


    /// <summary>
    /// Установить конфигурацию по-умолчанию
    /// </summary>
    public static void OnSetDafaults()
    {
        NextState("OnSetDafaults");
        ReadRules();
        RuleSet.Clear();
        RuleSet["*.HTML"] = new List<string>() {
            "ProgramHtml OnChange"
        };
        RuleSet["*.CSS"] = new List<string>() {
            "ProgramCss OnChange"
        };
        RuleSet["*.*"] = new List<string>() {
            "ProgramCss OnChange"
        };
        WriteRules();
        ReturnState();
    }


    /// <summary>
    /// Проверка правильности ввода операции по событию System.IO.FileSystemEventArgs
    /// </summary>
    public static string ValidateRule(string message)
    {             
        var messages = message
            .Split(' ')
            .Select(text => text.Trim())
            .Where(text => String.IsNullOrEmpty(text) == false)
            .ToArray();
        if( messages.Length != 2)
        {                
            return "предполагается ввод команды состоящей из 2 строк с именеи типа и именен статического метода который принимает 2 аргумента ('Created' | 'Deleted' | 'Changed' | 'Renamed' && FullPath)";
        }
        else
        {
            var type = ProgramCall.GetType(messages[0]);
            if (type == null)
                return "Тип с именем " + messages[0] + " не найден";
            var method = type.GetMethod(messages[1]);
            if (method == null)
                return "Метод с именем " + messages[1] + " не найден в типе " + messages[0];
            if (method.IsStatic == false)
                return "Метод с именем " + messages[1] +" типа " + messages[0] + " не является статическим";
            var parameters = method.GetParameters();
            if (parameters.Count() != 2)
                return "Метод должен принимать 2 аргумента";
            if (parameters[0].ParameterType != typeof(string))
                return "1-ый аргумент должен быть типа " + typeof(string).Name;
            if (parameters[1].ParameterType != typeof(string))
                return "2-ой аргумент должен быть типа " + typeof(string).Name;
            return "";
        }            
    }


    /// <summary>
    /// Считывание конфигураци из файла
    /// </summary>
    public static void ReadRules() {
        try
        {
            string filename = GetProgramConfigurationFile();
            Info("\n\tСчитывание файла конфигурации:");
            Info("\t>"+filename);
            string json = System.IO.File.ReadAllText(filename);
            RuleSet = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
                
        }
        catch (Exception ex)
        {
            Warn("\n\tНе удалось считать файл конфигурацуии: \n\t" + ex.Message);
        }

    }



    /// <summary>
    /// Запись правил в файл
    /// </summary>
    public static void WriteRules()
    {
        Info("\n\tСохранение конфигурации в файл\n \t");
        string filaname = GetProgramConfigurationFile();
        try
        {
            string json = JsonConvert.SerializeObject(RuleSet);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
            if( Equals(RuleSet, deserialized)==false)
            {
                throw new Exception("\n \tНужно использоватать другой сериализатор, т.к. обнаружено усечение синтаксиса\n \t");
            }                 
            System.IO.File.WriteAllText(filaname, json);
        }
        catch (Exception)
        {
            Warn("\n \tНе удалось сохранить файл конфигурации на диск\n \t");
        }
    }


    /// <summary>
    /// Сравнение справочников операции 
    /// </summary>
    public static bool Equals(IDictionary<string, List<string>> before, IDictionary<string, List<string>> after)
    {
        if (before.Keys.Count != after.Keys.Count)
            return false;            
        foreach (var key in before.Keys)
        {
            if (after.ContainsKey(key) == false)
                return false;
            var beforeValues=before[key];
            var afterValues=after[key];
            if (beforeValues == null)
                if (afterValues != null)
                    return false;
            if (beforeValues != null)
                if (afterValues == null)
                    return false;
            if (beforeValues.Count != afterValues.Count)                    
                return false;

            for(int i=0; i< beforeValues.Count; i++)
            {
                if (beforeValues[i] != afterValues[i])
                    return false;                    
            }
        }
        return true;
    }


    /// <summary>
    /// Считывание конфигурации из стандартного потока ввода.
    /// При работа в автоматическом режиме 
    /// </summary>
    public static void InputRules( Func<string, string> validate )
    {
        var pattern = ReadLine("\n\tПаттерн (применяется к имени файла при определнии операций): ").ToUpper();
        RuleSet[pattern] = RuleSet.ContainsKey(pattern)? RuleSet[pattern]: new List<string>();
        WriteLine($"\n\tСписок операций применяемых к файлас {pattern}: ");
        WriteLine("\t( ! ): ввод '' завершает ввод списка операций");
        string next = "", message = "";
        do
        {
            next = ReadLine($"\n\tВведите операцию №{(RuleSet[pattern].Count() + 1)}");
            message = validate(next);
            if ( String.IsNullOrEmpty(message))
            {
                RuleSet[pattern].Add(next);
            }
            else
            {
                Warn(message);
            }
                
        } while (String.IsNullOrWhiteSpace(next) == false);
    }


    /// <summary>
    /// Вывод конфигурации в тандарнтный поток вывода
    /// </summary>
    public static void OutputRules()
    {
        Info("\n\tПравила: ");
        if (RuleSet.Count == 0)
        {
            WriteLine("\n \tНе определно ни одного правила\n\n");
        }
        else
        {
            foreach (var ruleset in RuleSet)
            {
                    
                if (ruleset.Value.Count == 0)
                {
                    Info($"\t\t {ruleset.Key}: Не зарегистрировано ни одной опрации");
                }
                else
                {
                    Info($"\t\t {ruleset.Key}: ");
                    foreach (var todo in ruleset.Value)
                    {
                        Info("\t\t\t" + todo);
                    }
                }
            }
        }
    }


    /// <summary>
    /// Добавление правила обработки события нового документа
    /// </summary>
    public static void OnRemoveRule()
    {
        NextState("OnRemoveRule");
        WriteLine("\n\n");
        Info("Что хотите удалить?");
        string key = SingleSelect(RuleSet.Keys.ToArray());
        RuleSet.Remove(key);
        WriteRules();
        ReturnState();
    }
}
