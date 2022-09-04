using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// Конвертирование данных
/// </summary>
public static class TextConvertExtensions
{
    public static int ToInt(this string text)
    {
        text.EnsureIsInt();
        return int.Parse(text);
    }
    public static bool ToBool(this string text)
    {
        text.EnsureIsBool();
        return text=="true";
    }
    public static float ToFloat(this string text)
    {
        text.EnsureIsFloat();
        return float.Parse(text.Replace(".", ","));
    }
    public static DateTime? ToDate(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;
        text.EnsureIsDate();
        char? separator = text.FirstChar(@"-.\/:");
        string[] arr = text.Split((char)separator);
        if ((arr[0].Length == 4 && arr[1].Length == 2 && arr[2].Length == 2))
        {
            int year = int.Parse(arr[0]);
            int month = int.Parse(arr[1]);
            int day = int.Parse(arr[2]);

            return new DateTime(year, month, day);
        }
        else if ((arr[0].Length == 2 && arr[1].Length == 2 && arr[2].Length == 4))
        {
            int year = int.Parse(arr[2]);
            int month = int.Parse(arr[1]);
            int day = int.Parse(arr[0]);
            return new DateTime(year, month, day);
        }
        throw new Exception("Не удалось преобразовать текст в дату");
    }
}