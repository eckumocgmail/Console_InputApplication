using System;
using System.Threading.Tasks;

using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.Threading;
using static StackTraceExtensions.ExceptionInfo;
using System.Collections.Concurrent;
using Newtonsoft.Json;

public static class StackTraceExtensions
{

    /// <summary>
    /// Преобразование к формату JSON
    /// </summary>
    public static Dictionary<string, object> ToDictionary(this object target)
    {
        var result = new Dictionary<string, object>();
        target.GetType().GetProperties().Select(P => P.Name).ToList().ForEach(name => {
            result[name] = target.GetType().GetProperty(name).GetValue(target);
        });
        return result;
    }
    public static object GetProperty(this object target, string property)
    {
        var prop = target.GetType().GetProperties().FirstOrDefault(n => n.Name == property);
        var result = prop == null ? null : prop.GetValue(target);
        return result;
    }

    /// <summary>
    /// Возвращает значение свойства
    /// </summary> 
    public static void SetProperty(this object target, string property, object value)
    {
        target.GetType().GetProperty(property).SetValue(target, value);
    }
    public static Dictionary<string, object> ToDict(object target)
    {
        var result = new Dictionary<string, object>();
        target.GetType().GetProperties().Select(P => P.Name).ToList().ForEach(name => {
            result[name] = target.GetType().GetProperty(name).GetValue(target);
        });
        return result;
    }
    /// <summary>
    /// Сохраняет параметры вызова
    /// </summary>    
    public static object Commit(this object target,
        Func<object> todo,
        object Input = null)
    {
        IDictionary<string, object> args =
            Input == null ?
                new Dictionary<string, object>() :
                (Input is IDictionary<string, object>) ? ((IDictionary<string, object>)Input) :
                ToDict(Input);
        target.Info(args);

        if (todo == null)
            throw new ArgumentNullException("todo");
        var info = target.GetActionInfo(args);
        target.Info(info.ToString());

        object result = null;
        try
        {
            result = todo();
            info = target.GetActionInfo(args, true);
            target.Info(info.ToString());
        }
        catch (Exception e)
        {
            info = target.GetActionInfo(args, false, e);
            target.Info(info.ToString());
        }
        return result;
    }

    public static T Commit<T>(this object target,
        Func<object> todo,
        IDictionary<string, object> args = null) where T : class

    {
        try
        {
            //target.LogActionInfo();
            if (todo == null)
                throw new ArgumentNullException("todo");
            var info = target.GetActionInfo(args);
            target.Info(info.ToString());

            object result = null;
            try
            {
                result = todo();
                info = target.GetActionInfo(args, true);
                target.Info(info.ToString());
            }
            catch (Exception e)
            {
                info = target.GetActionInfo(args, false, e);
                target.Info(info.ToString());
            }
            return result != null ? ((T)result) : null;
        }
        catch (Exception e)
        {
            var info = target.GetActionInfo(args);
            throw new Exception("Ошибка при выполнении: ", e)
            {
                HelpLink = $"{info.ClassName}:{info.LineNumber}"
            };
        }
    }
    public static CallInfo GetActionInfo(this object target, IDictionary<string, object> arguments = null, bool completed = false, Exception ex = null)
    {
        if (arguments == null)
            arguments = new Dictionary<string, object>();
        CallInfo info = null;
        var stack = target.GetInvokeInfo();
        if (stack.Count >= 4)
        {
            info = stack[3];
            info.CallArguments = arguments;
            info.ActionCompleted = completed;


        }
        else
        {
            info = stack.Last();
            info.CallArguments = arguments;
            info.ActionCompleted = completed;
        }
        if (ex != null)
        {
            info.TextMessage = ex.Message;
        }
        return info;
    }
    public static void LogActionInfo(this object target)
    {
        var actionInfo = target.GetActionInfo();
        var actionName = actionInfo.ActionName;
        MethodInfo methodInfo = ToType(actionInfo.ClassName).GetMethods().FirstOrDefault(met => met.Name == actionName);
        target.Info(GetMethodInfoText(methodInfo));
    }

    private static Type ToType(string className)
    {
        return Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Name == className).FirstOrDefault();
    }

    private static string[] GetMethodInfoText(MethodInfo methodInfo)
    {
        return methodInfo.GetInvokeInfo().Select(info => info.ToJsonOnScreen()).ToArray();        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="arguments"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static CallInfo GetActionInfo(this object target, IDictionary<string, object> arguments, int level)
    {
        CallInfo info = target.GetInvokeInfo()[level];
        info.CallArguments = arguments;
        return info;
    }

    /// <summary>
    /// Возвращает сведения о методах исполняющихся в настоящий момент
    /// </summary>
    public static IList<CallInfo> GetInvokeInfo(this object target)
    {
        string stackTrace = Environment.StackTrace;

        var list = new List<CallInfo>();
        try
        {
            foreach (string line in stackTrace.Split("\n"))
            {
                if (line.IndexOf(":line") == -1)
                    continue;
                int i = line.IndexOf(":") - 1;
                int li = line.IndexOf(":line");

                if (!(i >= 0 && li >= 0))
                {
                    list.Add(new CallInfo()
                    {
                        TextMessage = line
                    });

                }
                else
                {

                    string action = line.Substring(0, i - 4);
                    string call = action.Substring(action.IndexOf("at ") + 3);
                    string classAndAction =  Before(call,"(");
                    action = classAndAction.Substring(classAndAction.LastIndexOf(".") + 1);
                    string className = classAndAction.Substring(0, classAndAction.Length - action.Length - 1);

                    CallInfo info = null;
                    list.Add(info = new CallInfo()
                    {
                        //TextMessage = line,
                        ThreadId = Thread.CurrentThread.ManagedThreadId,
                        TextMessage = "",
                        ClassName = className,
                        ActionName = action,
                        ActionStarted = true,
                        FilePath = "file:///" + line.Substring(i, line.IndexOf(":line") - i).Replace(@"\", "/"),
                        LineNumber = int.Parse(line.Substring(line.IndexOf(":line") + ":line".Length).Trim() )
                    });
                    string text = info.FilePath.Substring(0, info.FilePath.LastIndexOf("."));

                    info.FileName = text.Substring(text.LastIndexOf("/") + 1);
                }
            }

            list.Reverse();
        }
        catch (Exception ex)
        {
            target.Error(ex.Message);
            target.Error(ex.StackTrace);
        }


        return list;
    }
    public static string Before(string text, string subtext)
    {
        return text.Substring(0, text.IndexOf(subtext));
    }
    public static ExceptionInfo ToDocument(this Exception target)
    {
        var result = new ExceptionInfo();
        Exception p = target;
        do
        {
            foreach (var next in GetStack(p.StackTrace))
            {
                result.Add(next);
            };
            result.Add(new CallInfo()
            {
                TextMessage = p.Message
            });
            p = p.InnerException;
        } while (p != null);
        return result;
    }
    public static string ToTextDocument(this Exception target)
    {
        var result = new ExceptionInfo();
        Exception p = target;
        do
        {
            foreach(var next in GetStack(p.StackTrace)){
                result.Add(next);
            };
            result.Add(new CallInfo()
            {
                TextMessage = p.Message
            });
            p = p.InnerException;
        } while (p != null);

        return result.ToString();
    }
    public class ExceptionInfo
    {
        public ConcurrentBag<CallInfo> Data { get; set; } = new ConcurrentBag<CallInfo>();

        public void Add(CallInfo callInfo)
        {
            Data.Add(callInfo);
        }

        public override string ToString()
        {
            string text = "";
            foreach (var callInfo in Data)
            {

                text += "\n" + callInfo.TextMessage;
            }
            return text;
        }

        public class CallInfo
        {
            public string ClassName { get; set; }
            public string ActionName { get; set; }
            public IDictionary<string, object> CallArguments { get; set; } = new Dictionary<string, object>();
            public int ThreadId { get; set; } = Thread.CurrentThread.ManagedThreadId;
            public bool ActionCompleted { get; set; } = false;
            public bool ActionStarted { get; set; } = true;
            public string FilePath { get; set; }
            public int LineNumber { get; set; }
            public string FileName { get; set; }
            public string TextMessage { get; set; }

            public override string ToString()
            {
                if (String.IsNullOrWhiteSpace(TextMessage))
                {
                    return
                        $"{(this.ActionCompleted ? "Завершено" : "Приступаю")} " +
                        $"{FileName}:{LineNumber}";
                }
                else
                {
                    return
                        $"{(this.ActionCompleted ? "Завершено" : "Приступаю")} " +
                        $"{FileName}:{LineNumber} => { TextMessage}";
                }

            }
        }

    }

    private static IEnumerable<CallInfo> GetStack(string stackTrace)
    {

        var list = new List<CallInfo>();
        foreach (string line in stackTrace.Split("\n"))
        {
            int i = line.IndexOf(":") - 1;
            int li = line.IndexOf(":line");

            if (!(i >= 0 && li >= 0))
            {
                list.Add(new CallInfo()
                {
                    TextMessage = line
                });

            }
            else
            {
                list.Add(new CallInfo()
                {
                    TextMessage = line,
                    FilePath = "file:///" + line.Substring(i, line.IndexOf(":line") - i).Replace(@"\", "/"),
                    LineNumber = int.Parse(line.Substring(line.IndexOf(":line") + ":line".Length).Trim())
                });
            }

        }
        return list;
    }
}
/// <summary>
/// Расширения для динамической компиляции
/// </summary>
public static class ObjectLogExtensions
{
 
    
    public static void Log(this object target) => new object().LogInformation(ToJson(target));

    private static string ToJson(object target)
        => System.Text.Json.JsonSerializer.Serialize(target);
    

    public static void Info(this object target, params object[] messages)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        target.LogInformation(messages);
        Console.ResetColor();
    }

    public static void WriteToConsole(this object target )
    {        
        Console.WriteLine(target);        
    }
    public static void Warn(this object target, params object[] messages)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        target.LogInformation(messages);
        Console.ResetColor();
    }
    public static void Error(this object target, params object[] messages)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        if (messages.Length > 0 && messages[0] is Exception)
        {
            var list = messages.ToList();
            Exception ex = (Exception)messages[0];
            list.RemoveAt(0);
            target.Error(ex, list.ToArray());
        }
        else if (messages.Length > 0 && messages[messages.Length - 1] is Exception)
        {
            var list = messages.ToList();
            Exception ex = (Exception)messages[messages.Length - 1];
            list.RemoveAt(list.Count - 1);
            target.Error(ex, list.ToArray());
        }
        else
        {
            string message = "";
            foreach (var line in messages)
                message += line + "\n";
            target.LogInformation(message);
        }

        Console.ResetColor();
        string[] args = new string[] { };
        
    }

   
    public static void Error(this object target, Exception ex, params object[] messages)
    {

        while (ex != null)
        {


            Console.ForegroundColor = ConsoleColor.Cyan;
            WriteLine($"\n[{ex.Source }][{ex.TargetSite }] => {ex.Message}");
            Console.ResetColor();
            WriteLine();
            ex = ex.InnerException;
        }
        
        target.LogInformation(messages);

    }

    private static void WriteLine(params object[] messages)
    {
        foreach(var message in messages)
            Console.WriteLine(message);
    }

    public static void LogError(this object target, object[] messages, Exception ex) => target.LogError(ex, messages);
    public static void LogError(this object target, object message, Exception ex) => target.LogError(ex, message);
    public static void LogError(this object target, params object[] messages) => target.LogInformation(messages);
    public static void LogError(this object target, Exception ex, params object[] messages)
    {
        if (messages != null)
        {
            target.LogInformation();
        }
        if (ex != null)
        {
            target.LogInformation(ex.Message);
            target.LogInformation(ToDocument(ex));
        }

    }

    private static string[] ToDocument(Exception ex)
    {
        return ex.StackTrace.Split("at");
    }

    public static void Write(object arg)
        => Console.Write(arg);

    public static void Write(object[] args)
    {
        foreach (var arg in args)
        {
            WriteLine(arg);
        }
    }
    public static void WriteYellow(this object target, params object[] messages)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Write(messages);
        Console.ResetColor();
    }

    public static void WriteGreenLine(this object target, params object[] messages)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        WriteLine(messages);
        Console.ResetColor();
    }
    public static void WriteYellowLine(this object target, params object[] messages)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        WriteLine(messages);
        Console.ResetColor();
    }
    public static void WriteLIne(this object target, params object[] messages)
    {
        Console.WriteLine(messages);
    }
    public static string GetId(this object target)
        => $"{target.GetType().Name}";
    public static void LogInformation(this object target, params object[] messages)
    {
        try { 

            
            string id = target.GetId();
            Console.ForegroundColor = ConsoleColor.Yellow;

            Write($"\n[{id}] ");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (var message in messages)
            try
            {
                if (IsPrimitiveType(message.GetType()) == false)
                {

                    WriteLine(message.ToString());
                }
                else
                {
                    if (message is String)
                    {
                        WriteLine(message);
                    }
                    else if (IsImplements(message,typeof(IEnumerable)) == true)
                    {
                        foreach (object next in ((IEnumerable)message))
                        {
                            target.Info(next);
                        }
                    }
                    else
                    {
                        string json = ToJsonOnScreen(message);
                        WriteLine(json);

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"{target.GetId()} не удалось вывести текс выходной поток: {message} \n {ex.Message} \n {ex.ToString()}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"{target.GetId()} не удалось вывести текс выходной поток: {ToJsonOnScreen(messages)} \n {ex.Message}\n {ex.ToString()}");
        }
    }

    private static string ToJsonOnScreen(object target)
    {
        return JsonConvert.SerializeObject(target, Formatting.Indented);
    }

    private static bool IsImplements(object message, Type type)
    {
        return message.IsImplements(type);
    }

    private static bool IsPrimitiveType(Type type)
    {
        return Typing.IsPrimitive(type);
    }
}
