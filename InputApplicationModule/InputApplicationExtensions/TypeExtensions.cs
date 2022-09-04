using Newtonsoft.Json;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
 

/// <summary>
/// расширения типов
/// </summary>
public static class StringExtensions
{
    public static int CountOfChar(this string text, char ch)
    {
        int n = 0;
        foreach(char p in text.ToCharArray())
        {
            if (p == ch)
                n++;
        }
        return n;
    }
    
    public static string ValidateIsPositiveInt(this string text)
    {
        string str = text.ToString();
        if (str.Length == 0)
        {
            return "Числовое значение не может быть записано в 0 сиволов";
        }
        if ((str.Substring(0, 1) == "-" || str.Substring(0, 1).IsNumber()) == false)
        {
            return "Числовое значение должно начинаться либо с цифры либо со знака '-'";
        }
        if (text.Substring(1).CountOfChar('-') != 0)
        {
            return "Числовое значение может содержать только один знак минус";
        }
        foreach (var ch in text)
        {
            if ((ch.IsNumber() || ch == '-') == false)
            {
                return "Числовое значение не может содержать символ " + ch;
            }
        }
        return text.ToInt() < 0? "Числовое значение не может быть меньше нуля" : null;
        
    }
  
    public static bool IsNumber(this string text)
    {
        foreach(var p in text.ToCharArray())
        {
            if ("0123456789".Contains(p) == false)
                return false;
        }
        return true;
    }
}
public static class TypeAttributesExtensions
{
    public static IDictionary<string, string> GetAttributes(this Type p)
    {

        Dictionary<string, string> attrs = new Dictionary<string, string>();
        if (p == null)
        {
            Console.WriteLine($"Вам слендует передать ссылку на Type в метод Utils.GetEntityContrainsts() вместо null");
            Console.WriteLine($"{new ArgumentNullException("p")}");
            return attrs;
        }
        foreach (var data in p.GetCustomAttributesData())
        {
            string key = data.AttributeType.Name;
            foreach (var arg in data.ConstructorArguments)
            {
                string value = arg.Value.ToString();
                attrs[key] = value;
            }

        }
        return attrs;
    }

    public static string @label(this MethodInfo method)
    {
        Dictionary<string, string> attrs = new Dictionary<string, string>(method.GetCustomAttributesData().Select(kv => new KeyValuePair<string, string>(kv.AttributeType.Name, kv.ConstructorArguments.Count > 0 ? kv.ConstructorArguments.First().ToString() : "")));
        return attrs.ContainsKey(nameof(LabelAttribute)) ? attrs[nameof(LabelAttribute)] :
            attrs.ContainsKey(nameof(DisplayAttribute)) ? attrs[nameof(DisplayAttribute)] : method.Name;
    }
}
public static class TypeExtensions2
{

    /*public static Dictionary<string, string> @attrs(this MethodInfo method)
    {
        
        return method.Commit<Dictionary<string, string>>(() => {
            return Utils.ForMethod(method.DeclaringType, method.Name);
        });
    }*/
    public static string GetNameOfType(this Type propertyType)
    {
        if (propertyType == null)
            throw new ArgumentNullException("type");
        string name = propertyType.Name;
        if (name == null) return "";
        if (name.IndexOf("`") != -1)
            name = name.Substring(0, name.IndexOf("`"));

        var arr = propertyType.GetGenericArguments();
        if (arr.Length > 0)
        {
            name += '<';
            foreach (var arg in arr)
            {
                name += arg.GetNameOfType() + ",";
            }
            name = name.Substring(0, name.Length - 1);
            name += '>';
        }
        return name;
    }
    public static string GetTypeName(this object target)
    {
        Type targetType = target is Type ? ((Type)target) : target.GetType();
        string result = targetType.GetNameOfType();
        return result;
    }
    
    

    public static IEnumerable<Type> @extended(this Type method)
        => Assembly.GetExecutingAssembly().GetTypes().Where(type=>type.IsClass).Where(t => t.IsExtendsFrom(method));

    



    public static TResult GetValue<TResult>(this PropertyInfo Property, object target)
    {
        return (TResult)Property.GetValue(target);
    }
    public static IEnumerable<MethodInfo> SearchMethods<StringMatcherType>(this Type type, string pattern)
        where StringMatcherType : Comparer<string>
    {
        Comparer<string> comp = (StringMatcherType)typeof(StringMatcherType).GetConstructors().First(c => c.GetParameters().Count()==0).Invoke(new object[0]);
        return type.GetMethods().Where(m => comp.Compare(m.Name, pattern) == 0);
    }

    public static IEnumerable<ParameterInfo> GetActionParameters( this Type target, string action)
    {
        MethodInfo methodInfo = target.GetMethods().FirstOrDefault(m => m.Name == action);
        if(methodInfo == null)
            throw new InvalidOperationException(action);
        return methodInfo.GetParameters();
    }

    /// <summary>
    /// Возвращает функцию связывания объекта с функцией для вызова метода этого объекта полученную в результате связывания
    /// </summary>    
    public static Func<object,Func<IDictionary<string,object>,object>> GetAction(this Type type, string name )
    {
       
        var method = type.GetMethods().Where(m => m.Name == name).First();

        /// local - объект контекст которого
        return (localscope) => {

            var local = new Dictionary<string, object>();
            foreach(PropertyInfo p in localscope.GetType().GetProperties()){
                local[p.Name] = p.GetValue(localscope);
            }
            return (IDictionary<string, object>  scope) =>
            {
                var transclusion = new Dictionary<string, object>();
                foreach (var kv in local)
                    transclusion[kv.Key] = kv.Value;

                foreach (var kv in scope)
                    transclusion[kv.Key] = kv.Value;

                var started = DateTime.Now;
                var pars = new Dictionary<string, object>();
                var args = new List<object>();
                object result = null;
                try
                {
                    foreach (var name in method.GetParameters().Select(p => p.Name))
                    {
                        if (transclusion.ContainsKey(name))
                        {
                            args.Add(transclusion[name]);
                            pars[name] = transclusion[name];
                        }
                        else
                        {
                            throw new ArgumentException(name);
                        }
                    }

                }
                catch (Exception ex)
                {
                    return MethodResult.OnError(ex);
                }
                try
                {
                    
                    result = method.Invoke(localscope, args.ToArray());
                    return MethodResult.OnComplete(result, pars, started);
                }
                catch (Exception ex)
                {
                    localscope.Error(ex);
                    return MethodResult.OnFailed(ex, started);
                }
                
            };
        };
    }



    public static string ToDocument( this Type type)
        => JsonConvert.SerializeObject(new TypeDocumentation(type));

    public static Type[] GetParamTypes(this Type type)
    {
        return type.GenericTypeArguments;
    }
    public static bool IsProperty(this Type type, string name)
        => type.GetProperties().Any(p => p.Name == name);
    public static bool IsMethod(this Type type, string name)
        => type.GetMethods().Any(p => p.Name == name);

    public static bool IsExtends(this System.Object type, Type baseType)
    {
        if (type is Type)
            return ((Type)type).IsExtendsFrom(baseType);
        else return type.GetType().IsExtendsFrom(baseType);
    }
    public static bool IsImplements(this System.Object type, Type baseType)
    {
        if (type is Type)
            return ((Type)type).IsImplementsFrom(baseType);
        else return type.GetType().IsImplementsFrom(baseType);
    }
    public static bool IsExtendsFrom(this Type type, Type baseType)
    {
        return IsExtendedFrom(type, baseType.Name);
    }
    public static bool IsImplementsFrom(this Type targetType, Type baseType)
    {
        Type typeOfObject = new object().GetType();
        Type p = targetType;
        while (p != typeOfObject && p != null)
        {
            if (p.Name == baseType.Name || p.GetInterfaces().Select(i => i.GetTypeName()).Contains(baseType.GetTypeName()))
            {
                return true;
            }
            p = p.BaseType;
        }
        return false;
    }
     

    public static bool IsExtendsFrom(this Type type, string baseType)
    {
        return IsExtendedFrom(type, baseType);
    }

    public static bool IsExtendedFrom(Type targetType, string baseType)
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
   
    public static List<string> GetOwnPropertyNames(this object type)
    {
        if (type is Type)
            return (from p in new List<PropertyInfo>(((Type)type).GetProperties()) where p.DeclaringType == ((Type)type) select p.Name).ToList();
        else
            return (from p in new List<PropertyInfo>(type.GetType().GetProperties()) where p.DeclaringType == type.GetType() select p.Name).ToList();
    }
    public static List<string> GetOwnMethodNames(this Type type)
    {
        return type.GetMethods().Where(m => m.Name.StartsWith("get_") == false && m.Name.StartsWith("set_") == false && type.BaseType.GetMethods().Select(m2 => m2.Name).Contains(m.Name) == false).Select(m => m.Name).ToList();        
    }

    public static List<string> GetPropertyNames(this Type type)
    {
        var list = (from p in new List<PropertyInfo>(type.GetProperties()) select p.Name).ToList();
        list.Reverse();
        return list;
    }
    public static List<string> GetFieldNames(this Type type)
    {
        return (from p in new List<FieldInfo>(type.GetFields()) select p.Name).ToList();
    }
   

}
