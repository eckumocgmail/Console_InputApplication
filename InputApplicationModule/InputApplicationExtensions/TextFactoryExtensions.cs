using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// Расширения к строкового типа
/// </summary>
public static class TextFactoryExtensions
{

    /// <summary>
    /// Поиск типа по имени
    /// </summary>
    public static Type ToType(this string text)
    {                      
        if(text == null)
            throw new ArgumentNullException("text");
        return text.Contains(".")? ReflectionService.TypeForName(text): ReflectionService.TypeForShortName(text);
    }




    /// <summary>
    /// Поиск типа по имени
    /// </summary>
    public static object New(this string text)
    {
        if (text == null)
            throw new ArgumentNullException("text");
        return ReflectionService.CreateWithDefaultConstructor<object>(text.ToType());
    }

    /// <summary>
    /// Поиск типа по имени
    /// </summary>
    public static object New(this Type type)
    {
        if (type == null)
            throw new ArgumentNullException("type");
        
        return ReflectionService.CreateWithDefaultConstructor<object>(type);
    }


    /// <summary>
    /// Поиск типа по имени
    /// </summary>
    public static object New(this Type type, object[] args)
    {
        if (type == null)
            throw new ArgumentNullException("type");
        return ReflectionService.Create<object>(type, args);
    }


    /// <summary>
    /// Поиск типа по имени
    /// </summary>
    public static object Copy(this object type, IDictionary<string,object> keyValues)
    {
        var names = type.GetType().GetProperties().Select(p => p.Name);
        foreach (var kv in keyValues)
        {
            if (names.Contains(kv.Key))
            {
                Setter.SetValue(type,kv.Key, kv.Value); 

            }
        }
        return type;
    }

    /// <summary>
    /// Поиск типа по имени
    /// </summary>
    public static object CopyObject(this object type, object keyValues)
    {
        var names = type.GetType().GetProperties().Select(p => p.Name);
        foreach (var kv in keyValues.ToJson().FromJson<Dictionary<string,object>>())        {
            if (names.Contains(kv.Key))
            {
                try
                {
                    if (Typing.IsCollectionType(type.GetType().GetProperty(kv.Key).PropertyType) == false)
                        Setter.SetValue(type, kv.Key, kv.Value);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Исключение при копироавнии данных в поле с ключом {kv.Key} => {ex.Message}");
                }

            }
        }
        return type;
    }

    /// <summary>
    /// Поиск типа по имени
    /// </summary>
    public static object Create(this Type type, object[] parameters)
    {
        return ReflectionService.Create<object>(type, parameters);
    }

    /// <summary>
    /// Поиск типа по имени
    /// </summary>
    public static object New(this string text, string deps)
    {
        var constructor = text.ToType().GetConstructors()[0];
        return constructor.Invoke(new object[] { deps });
    }

}