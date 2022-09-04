using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


using static System.Reflection.Assembly;

/// <summary>
/// Формирует динамический вызов исп. рефлексию
/// </summary>
public class ProgramCall: ProgramDirectory
{
    public ProgramCall(IServiceProvider provider, string path) : base(provider, path)
    {
    }

    /// <summary>
    /// Получение типа из такущей сборки
    /// </summary>
    public static Type GetType(string TypeName)
    {
         
        return GetExecutingAssembly().GetTypes().Where(t => t.Name == TypeName).FirstOrDefault();
    }
        


    /// <summary>
    /// Разбор тектового выражения
    /// </summary>
    /// <param name="exp"></param>
    /// <returns></returns>
    public static List<string> Parse(string exp)
    {
        List<string> expressions = new List<string>();
        while (exp.IndexOf("{{") >= 0)
        {
            int x2 = exp.IndexOf("}}");
            int x1 = exp.IndexOf("{{");
            expressions.Add(exp.Substring(x1 + 2, x2 - x1 - 2));
            exp = exp.Substring(x2 + 2);
        }
        expressions.ForEach((p) => { Console.WriteLine(p); });
        return expressions;
    }



    /// <summary>
    /// Преобразование строки форму пригодную для функции string.Format
    /// </summary>
    /// <param name="exp"></param>
    /// <returns></returns>
    public static string ToFormatable(string exp)
    {
        string formatableString = "";
        List<string> expressions = new List<string>();
        int ctn = 0;
        while (exp.IndexOf("{{") >= 0)
        {
            int x2 = exp.IndexOf("}}");
            int x1 = exp.IndexOf("{{");
            expressions.Add(exp.Substring(x1 + 2, x2 - x1 - 2));
            formatableString += exp.Substring(0, x1) + "{" + ctn + "}";
            ctn++;
            exp = exp.Substring(x2 + 2);
        }
        formatableString += exp;
        return formatableString;
    }


    public static string Interpolate(string exp, object p)
    {
        List<object> paramsList = new List<object>();
        List<string> expressions = Parse(exp);
        foreach (string s in expressions)
        {
            object value = Compile(s, p);
            paramsList.Add(value);
        }
        //while (exp.IndexOf("{{") > 0) exp = exp.Replace("{{", "{");
        //while (exp.IndexOf("}}") > 0) exp = exp.Replace("}}", "}");
        string formatingString = ToFormatable(exp);
        return string.Format(formatingString, paramsList.ToArray());


    }

    public static object Compile(string exp, object p)
    {
        var value = p;
        if (IsExtendsFrom(value.GetType(), "BaseEntity"))
        {
            value.GetType().GetMethod("JoinAll").Invoke(value, new object[0]);
        }
        foreach (string operation in exp.Trim().Split("."))
        {
            if (operation.IndexOf("(") == -1)
            {
                string ssexpression = operation.Trim();
                if (IsLiteral(ssexpression))
                {
                    value = GetLiteral(ssexpression);
                }
                else
                {
                    value = GetPropertyOrFieldValue(value, ssexpression);
                }
            }
            else
            {
                string actionName = operation.Substring(0, operation.IndexOf("("));
                int x1 = operation.IndexOf("(");
                int x2 = operation.LastIndexOf(")");
                string paramsStr = operation.Substring(x1 + 1, x2 - x1 - 1).Trim();
                List<object> args = new List<object>();
                if (paramsStr.Length > 0)
                {
                    var arr = new List<string>();

                    string temp = "";
                    foreach (string s in paramsStr.Split(","))
                    {
                        if (s.Trim().StartsWith("'") && !s.Trim().EndsWith("'"))
                        {
                            temp += s.Substring(1);
                        }
                        else if (!s.Trim().StartsWith("'") && s.Trim().EndsWith("'"))
                        {
                            temp += ',' + s.Substring(0, s.Length - 1);
                            arr.Add("'" + temp + "'");
                            temp = "";
                        }
                        else if (s.Trim().StartsWith('"') && !s.Trim().EndsWith('"'))
                        {
                            temp += s.Substring(1);
                        }
                        else if (!s.Trim().StartsWith('"') && s.Trim().EndsWith('"'))
                        {
                            temp += ',' + s.Substring(0, s.Length - 1);
                            arr.Add("'" + temp + "'");
                            temp = "";
                        }
                        else
                        {
                            arr.Add(s);
                        }
                    }

                    foreach (string s in arr)
                    {
                        string sarg = s.Trim();
                        if (string.IsNullOrEmpty(sarg))
                        {
                            throw new Exception("Строка параметров считана с ошибками. параметр не может иметь имя длиной 0 символов");
                        }
                        args.Add(Compile(sarg, value));
                    }
                }

                value = Execute(value, actionName, args);

            }

        }


        return value;

    }
    public static bool IsExtendsFrom(Type targetType, string baseType)
    {
        Type typeOfObject = new object().GetType();
        Type p = targetType;
        while (p != typeOfObject && p != null)
        {
            if (p.Name == baseType)
            {
                return true;
            }
            p = p.BaseType;
        }
        return false;
    }


    public static object GetLiteral(string ssexpression)
    {
        if (IsNumber(ssexpression[0]))
        {
            return int.Parse(ssexpression);
        }
        else
        {
            if (ssexpression[0] == '"')
            {
                int i2 = ssexpression.Substring(1).IndexOf('"');
                return ssexpression.Substring(1, i2);
            }
            else if ((ssexpression[0] + "") == "'")
            {
                int i2 = ssexpression.Substring(1).IndexOf("'");
                return ssexpression.Substring(1, i2);
            }
            else
            {
                throw new Exception("Компиляция выражения: " + ssexpression + " на настоящий момент не возможна");
            }
        }
        throw new Exception("Компиляция выражения: " + ssexpression + " на настоящий момент не возможна");
    }

    public static bool IsNumber(char ch) => "0987654321".Contains(ch);

    public static bool IsLiteral(string ssexpression)
    {
        if (string.IsNullOrEmpty(ssexpression))
        {
            throw new Exception("Текст выражения пуст");
        }
        if (IsNumber(ssexpression[0]) || ssexpression[0] == '"' || (ssexpression[0] + "") == "'")
        {
            return true;
        }
        //TODO: дописать проверку является ли выражение литералом
        return false;

    }

    public static object Execute(object value, string actionName, List<object> args)
    {
        var method = value.GetType().GetMethod(actionName);
        if (method == null)
        {
            throw new Exception("Не удалось найти метод с именем: " + actionName + " для типа " + value.GetType().Name);
        }
        else
        {

            return method.Invoke(value, args.ToArray());
            //return InvokeHelper.Do(value, actionName, args);
        }
    }

    public static object GetPropertyOrFieldValue(object value, string operation)
    {
        var property = value.GetType().GetProperty(operation);
        if (property != null)
        {
            return property.GetValue(value);
        }
        var field = value.GetType().GetField(operation);
        if (field != null)
        {
            return field.GetValue(value);
        }
        throw new Exception("Обьект типа" + value.GetType().Name + " не имеет свойства " + operation);
    }


    /// <summary>
    /// Выполнение статической операции
    /// </summary>
    public static object Call(string ActionPath, IDictionary<string,string> argv )
    {
        var messages = ActionPath.Split('.');
        if (messages.Length != 2)
            throw new ArgumentException(
                "Аргумент StaticAction предполагает фактически" +
                " передачу строки составленой из имени типа и" +
                " имени статического метода разделённые точкой.");
        var type = GetType(messages[0]);
        var method = type.GetMethod(messages[1]);
        object result = null;
        try
        {
            var argList = new List<object>();
            foreach(var parg in method.GetParameters())
            {
                if (argv.ContainsKey(parg.Name) == false)
                    throw new KeyNotFoundException(parg.Name);
                string textValue = argv[parg.Name];
                Type parameterValue = parg.ParameterType;


                if(parameterValue.Equals(typeof(System.String)) == false)
                {
                    throw new NotSupportedException(parg.ParameterType.Name);
                }
                else
                {
                    argList.Add(textValue);
                }
            }
            result = method.Invoke(null, argList.ToArray());
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("Вызов операции вызвал исключение: " + ex.Message);
        }
    }


    /// <summary>
    /// Выполнение статической операции
    /// </summary>
    public static object Call(string TypeName, string StaticMethodName, params string[] StringArguments )
        => Call(TypeName, StaticMethodName, StringArguments);

    /// <summary>
    /// Выполнение статической операции
    /// </summary>
    /// <param name="StaticAction">операция=[тип].[статический метод]</param>
    /// <param name="StringArguments">строковые аргументы</param>
    /// <returns></returns>
    public static object Call(string StaticAction, params string[] StringArguments)
    {
        var messages = StaticAction.Split('.');
        if (messages.Length != 2)
            throw new ArgumentException(
                "Аргумент StaticAction предполагает фактически" +
                " передачу строки составленой из имени типа и" +
                " имени статического метода разделённые точкой.");
        var type = GetType(messages[0]);
        var method = type.GetMethod(messages[1]);
        object result = null;
        try
        {
            result = method.Invoke(null, StringArguments);
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("Вызов операции вызвал исключение: "+ex.Message);
        }
    }


        
}
