using NetCoreConstructorAngular.Data.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


/// <summary>
/// Атрибут свойства модели, указывает на необходимость удостовериться, что текст свойства содержит только латинские символы
/// </summary>
public class InputEngWordAttribute : BaseInputAttribute, MyValidation
{
    /// <summary>
    /// Сообщение в случае возникновения исключения
    /// </summary>
    protected string _message;


    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="message"> сообщение в случае возникновения исключения </param>
    public InputEngWordAttribute(string message = ""):base(InputTypes.Text)
    {
        _message = message;
    }


    /// <summary>
    /// Получение значения по имени свойства объекта
    /// </summary>
    /// <param name="target">ссылка на целевой оъект</param>
    /// <param name="property">имф свойства</param>
    /// <returns>значение свойства</returns>
    private object GetValue(object target, string property)
    {
        PropertyInfo propertyInfo = target.GetType().GetProperty(property);
        FieldInfo fieldInfo = target.GetType().GetField(property);
        return
            fieldInfo != null ? fieldInfo.GetValue(target) :
            propertyInfo != null ? propertyInfo.GetValue(target) :
            null;

    }

    public override string OnGetMessage(object model, string property, object value)
    {
        if (string.IsNullOrEmpty(_message))
        {
            return "Значение может содержать только буквы латинского алфавита";
        }
        else
        {
            return _message;
        }
    }

    public override string OnValidate(object model, string property, object value)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return null;
        }
        else
        {

            string alf =  "qwertyuiopasdfghjklzxcvbnm" + " " + "qwertyuiopasdfghjklzxcvbnm".ToUpper();
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
}

