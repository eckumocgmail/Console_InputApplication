using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// расширения для работы с текстом
/// </summary>
public static class TextExtensions
{
    public static string ReplaceAll(this string text, string s1, string s2)
    {
        string p = text;
        while (p.IndexOf(s1) != -1)
        {
            p = p.Replace(s1, s2);
        }
        return p;
    }
    public static string GetPath(this string message)
    {
        int start = Math.Min(message.IndexOf(@" C:\"), message.IndexOf(@" C:\"));
        if (start == -1)
        {
            return null;
        }
        else
        {
            string filepath = message.Substring(start);
            filepath = filepath.Substring(0, filepath.IndexOf(":line")).Replace(@"\\", @"\").Trim();
            if (!System.IO.File.Exists(filepath))
            {
                return "";

            }
            else
            {
                return filepath;
            }

        }
    }

    public static string Filter(this string text, string characters)
    {
        string result = "";
        foreach(var ch in text)
        {
            if (characters.Contains(ch) == false)
                result += ch;
        }
        return result;
    }
    /// <summary>
    /// Получение типов из пространства имён
    /// </summary>    
    public static IEnumerable<Type> GetTypes(this string @namespace)
    {            
        return Assembly.GetCallingAssembly().GetTypes().Where(t => t.Namespace == @namespace).Where(t => t.Name.Contains("<") == false ).ToList();        
    }


    /// <summary>
    /// Получение типов из пространства имён
    /// </summary>    
    public static IEnumerable<Type> GetTypesAll(this string @namespace)
    {
        return Assembly.GetCallingAssembly().GetTypes().Where(t => t.Namespace!=null && t.Namespace.StartsWith(@namespace)).Where(t => t.Name!=null && t.Name.Contains("<") == false).ToList();
    }

    /// <summary>
    /// Поиск первого символа из заданного текста
    /// </summary>
    public static char? FirstChar(this string text, string charset)
    {
        foreach (var character in text)
        {
            if (charset.Contains(character))
            {
                return character;
            }
        }
        return null;
    }

    /// <summary>
    /// Кол-во подстрок
    /// </summary>
    /*public static int CountOfChar(this string text, char ch)
    {
        int counter = 0;
        foreach (var character in text)
        {
            if (character == ch)
            {
                counter++;
            }
        }
        return counter;
    }*/
    /// <summary>
    /// Замена подстрок
    /// </summary>
    public static string ReplaceText(this string text, string s1, string s2)
    {
        while (text.IndexOf(s1) != -1)
        {
            text = text.Replace(s1, s2);
        }
        return text;
    }
}