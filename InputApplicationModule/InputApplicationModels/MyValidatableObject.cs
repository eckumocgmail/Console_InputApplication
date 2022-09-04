using NetCoreConstructorAngular.Data.DataAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

public class MyValidatableObject<TContext> : MyValidatableObject
{

}
public class MyValidatableObject : IValidatableObject
{

    /// <summary>
    /// true, если тип обьекта наследуется от заданного типа по имени
    /// </summary>
    /// <param name="baseType"></param>
    /// <returns></returns>
    public bool IsExtendedFrom(string baseType)
    {
        Type typeOfObject = new object().GetType();
        Type p = GetType();
        while (p != typeOfObject)
        {
            if (p.Name == baseType)
            {
                return true;
            }
            p = p.BaseType;
        }
        return false;
    }

    
    

    /// <summary>
    /// Валидация модели по правилам определённым через атрибуты
    /// </summary>
    /// <returns></returns>
    public virtual Dictionary<string, List<string>> Validate(string[] keys)
    {
 
        Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
        foreach (var property in GetType().GetProperties())
        {
            string key = property.Name;

            if (IsPrimitive(property.PropertyType))
            {
                List<string> errors = Validate(key);
                if (errors.Count > 0)
                {
                    result[key] = errors;
                }

            }
        }
        var optional = ValidateOptional();
        foreach (var p in optional)
        {
            if (result.ContainsKey(p.Key))
            {
                result[p.Key].AddRange(optional[p.Key]);
            }
            else
            {
                result[p.Key] = optional[p.Key];
            }
        }
        return result;
    }



    /// <summary>
    /// Валидация свойства объявленного заданным ключем
    /// </summary>
    /// <param name="key">ключ свойства</param>
    /// <returns></returns>
    public List<string> Validate(string key)
    {
        
        List<string> errors = new List<string>();
        var attributes = ForProperty(this.GetType(), key);

        foreach (var data in this.GetType().GetProperty(key).GetCustomAttributesData())
        {
            if (data.AttributeType.GetInterfaces().Contains(typeof(MyValidation)))
            {
                List<object> args = new List<object>();
                foreach (var a in data.ConstructorArguments)
                {
                    args.Add(a.Value);
                }
                MyValidation validation =
                    Create<MyValidation>(data.AttributeType, args.ToArray());
                object value = GetValue(this, key);
                string validationResult =
                    validation.Validate(this, key, value);
                if (validationResult != null)
                {

                    errors.Add(validationResult);
                }
            }
        }
        return errors;
    }

    private object ForProperty(Type type, string key)
    {
        return type.GetPropertyAttributes(key);
    }

    private T Create<T>(Type attributeType, object[] vs)
    {
        return (T)attributeType.Create(vs);
    }



    /// <summary>
    /// Проверка данных порождает исключение при не соответвии требованиям
    /// </summary>
    public void EnsureIsValide()
    {
        var r = Validate();
        if(r.Count() > 0)
        {
            string message = $"Обьект " + GetType().Name + " не валидный: \n" + ToJson(r);
            throw new ValidationException(message);
        }
    }

    private string ToJson(Dictionary<string, List<string>> r)
    {
        throw new NotImplementedException();
    }

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> results = new List<ValidationResult>();
        Dictionary<string, List<string>> errors = Validate();
        foreach(var errorEntry in errors)
        {
            string propertyName = errorEntry.Key;
            List<string> propertyErrors = errorEntry.Value;
            foreach(string propertyError in propertyErrors)
            {
                ValidationResult result = new ValidationResult(propertyError, new List<string>() { propertyName });               
                results.Add(result);
            }
        }        
        return results;
    }


    /// <summary>
    /// Получение значения свойства заданным ключем
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public object GetValue(MyValidatableObject  myValidatableObject, string name)
    {
        PropertyInfo propertyInfo = this.GetType().GetProperty(name);
        FieldInfo fieldInfo = this.GetType().GetField(name);
        return
            fieldInfo != null ? fieldInfo.GetValue(this) :
            propertyInfo != null ? propertyInfo.GetValue(this) :
            null;
    }

    /// <summary>
    /// Установка значения свойства
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetValue(string key, object value)
    {
        PropertyInfo prop = this.GetType().GetProperty(key);
        if (prop != null)
        {
            prop.SetValue(this, value);
        }
        FieldInfo field = this.GetType().GetField(key);
        if (field != null)
        {
            field.SetValue(this, value);
        }
    }
    /// <summary>
    /// Валидация модели по правилам определённым через атрибуты
    /// </summary>
    /// <returns></returns>
    public virtual Dictionary<string, List<string>> Validate(  )
    {
        this.Info("Приступаю к валидации: "+ this.GetType().GetProperties().Select(p => p.Name).ToJsonOnScreen());
        object target = this;
        
        Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
        foreach (var property in target.GetType().GetProperties())
        {
            string key = property.Name;

            this.Info($"Валидация свойства: {key}");
            if (IsPrimitive(property.PropertyType))
            {
                List<string> errors = Validate(key);
                if (errors.Count > 0)
                {
                    result[key] = errors;
                }

            }
        }
        var optional = ValidateOptional();
        foreach (var p in optional)
        {
            if (result.ContainsKey(p.Key))
            {
                result[p.Key].AddRange(optional[p.Key]);
            }
            else
            {
                result[p.Key] = optional[p.Key];
            }
        }


        return result;
    }


    private bool IsPrimitive(Type propertyType)
        => Typing.IsPrimitive(propertyType);


    /// <summary>
    /// Процедура дополнительной валидации можеди
    /// </summary>
    /// <returns></returns>
    public virtual Dictionary<string, List<string>> ValidateOptional()
    {
        return new Dictionary<string, List<string>>();
    }

}