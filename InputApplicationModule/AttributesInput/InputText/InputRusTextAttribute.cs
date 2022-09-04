using NetCoreConstructorAngular.Data.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



public class InputRusTextAttribute : InputTextAttribute, MyValidation
{

    protected string _message;

    public InputRusTextAttribute(string message = "Значение может содержать только буквы русского алфавита")
    {
        _message = message;
    }

    public override string GetMessage(object model, string property, object value)
    {
        if (string.IsNullOrEmpty(_message))
        {
            return "Значение может содержать только буквы русского алфавита";
        }
        else
        {
            return _message;
        }
    }


    public static object GetValue(object i, string v)
    {
        PropertyInfo propertyInfo = i.GetType().GetProperty(v);
        FieldInfo fieldInfo = i.GetType().GetField(v);
        return
            fieldInfo != null ? fieldInfo.GetValue(i) :
            propertyInfo != null ? propertyInfo.GetValue(i) :
            null;
    }


    /// <summary>
    /// Проверка теста на наличие инностранных литералов
    /// </summary>
    /// <param name="model"></param>
    /// <param name="property"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public override string Validate(object model, string property, object value)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return null;
        }
        else
        {
            string alf = ",.!?-;:()'\"() "+"абвгджеёжзйиклмнпорстуфхцчшщъыьэюя" + " .,1234567890" + "абвгджеёжзйиклмнпорстуфхцчшщъыьэюя".ToUpper() + " ";
            string text = GetValue(model, property).ToString();
            for (int i = 0; i < text.Length; i++)
            {
                if (!alf.Contains(text[i]))
                {
                    return GetMessage(model, property, value);
                }
            }
            return null;
        }
    }

    public static bool IsRus(string word)
    {
        return Regex.Match(word, "/^[а-яА-ЯёЁ]+$/", RegexOptions.IgnoreCase).Success;
    }
}
