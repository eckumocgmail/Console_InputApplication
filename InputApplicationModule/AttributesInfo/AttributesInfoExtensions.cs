using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Label("Расширение типов информационными атрибутами")]
[Description("Предоставляет типу Type функции для получения значения атрибутов группы AttributesInfo")]
public static class AttributesInfoExtensions
{
    [Label("Получить метку к данному типу")]
    public static string GetLabel(this Type ptype) => ptype.GetAttribute<LabelAttribute>(ptype.Name);

    [Label("Получить иконку к данному типу")]
    public static string GetIcon(this Type ptype) => ptype.GetAttribute<IconAttribute>("help");

    [Label("Получить описание к данному типу")]
    public static string GetDescription(this Type ptype) => ptype.GetAttribute<DescriptionAttribute>("");

    [Label("Получить вспомогательное сообщение к данному типу")]
    public static string GetHelp(this Type ptype) => ptype.GetAttribute<HelpAttribute>("");


    [Label("Получить метку свойсва")]
    public static string GetPropertyLabel(this Type ptype, string property) => ptype.GetPropertyAttribute<LabelAttribute>(property, ptype.Name);

    [Label("Получить иконку свойсва")]
    public static string GetPropertyIcon(this Type ptype, string property) => ptype.GetPropertyAttribute<IconAttribute>(property, "help");

    [Label("Получить описание свойства")]
    public static string GetPropertyDescription(this Type ptype, string property) => ptype.GetPropertyAttribute<DescriptionAttribute>(property, "");

    [Label("Получить вспомогательное сообщение к свойству")]
    public static string GetPropertyHelp(this Type ptype, string property) => ptype.GetPropertyAttribute<HelpAttribute>(property, "");

    [Label("Получить метку к методу")]
    public static string GetMethodLabel(this Type ptype, string method) => ptype.GetMethodAttribute<LabelAttribute>(method, ptype.Name);

    [Label("Получить иконку к методу")]
    public static string GetMethodIcon(this Type ptype, string method) => ptype.GetMethodAttribute<IconAttribute>(method, "help");

    [Label("Получить описание к методу")]
    public static string GetMethodDescription(this Type ptype, string method) => ptype.GetMethodAttribute<DescriptionAttribute>(method, "");

    [Label("Получить вспомогательное сообщение к методу")]
    public static string GetMethodHelp(this Type ptype, string method) => ptype.GetMethodAttribute<HelpAttribute>(method, "");

    [Label("Получить атрибуты к методу")]
    public static string GetAttribute<TAttribute>(this Type ptype, string defaults = null) where TAttribute : Attribute
    {
        var pattribute = ptype.GetCustomAttributesData().FirstOrDefault(data => data.AttributeType == typeof(TAttribute));
        if (pattribute != null)
        {
            return defaults;
        }
        else
        {
            if (pattribute!=null && pattribute.ConstructorArguments!=null && pattribute.ConstructorArguments.Count() > 0)
            {
                return pattribute.ConstructorArguments.First().ToString();
            }
            else
            {
                return "";
            }
        }
    }

    [Label("Получить атрибуты к свойству")]
    public static IDictionary<string, string> GetPropertyAttributes(
        this Type ptype, string property)

    {
        var res = new Dictionary<string, string>();
        var pproperty = ptype.GetProperties().First(p => p.Name == property);
        if (pproperty == null)
            throw new ArgumentException(nameof(property));
        foreach (var kv in pproperty.GetCustomAttributesData())
        {
            res[TypeExtensions2.GetTypeName(kv.AttributeType)] = kv.ConstructorArguments.Count() > 0 ? kv.ConstructorArguments.First().ToString() : "";
        }
        return res;
    }

   
    public static IDictionary<string, string> GetMethodAttributes(
        this Type ptype, string method)

    {
        var res = new Dictionary<string, string>();
        var pproperty = ptype.GetMethods().First(p => p.Name == method);
        if (pproperty == null)
            throw new ArgumentException(nameof(method));
        foreach (var kv in pproperty.GetCustomAttributesData())
        {
            res[TypeExtensions2.GetTypeName(kv.AttributeType)] = kv.ConstructorArguments.Count() > 0 ? kv.ConstructorArguments.First().ToString() : "";
        }
        return res;
    }

    [Label("Получить значение атрибута к свойству")]
    public static string GetPropertyAttribute<TAttribute>(this Type ptype, string property, string defaults = null) where TAttribute : Attribute
    {
        var pproperty = ptype.GetProperties().First(p => p.Name == property);
        if (pproperty == null)
            throw new ArgumentException(nameof(property));
        var pattribute = pproperty.GetCustomAttributesData().FirstOrDefault(data => data.AttributeType == typeof(TAttribute));
        if (pattribute == null)
        {
            return defaults;
        }
        else
        {
            if (pattribute.ConstructorArguments.Count() > 0)
            {
                return pattribute.ConstructorArguments.First().ToString();
            }
            else
            {
                return "";
            }
        }
    }

    [Label("Получить значение атрибута к методу")]
    public static string GetMethodAttribute<TAttribute>(this Type ptype, string method, string defaults = null) where TAttribute : Attribute
    {
        var pmethod = ptype.GetMethods().First(p => p.Name == method);
        if (pmethod == null)
            throw new ArgumentException(nameof(method));
        var pattribute = pmethod.GetCustomAttributesData().FirstOrDefault(data => data.AttributeType == typeof(TAttribute));
        if (pattribute == null)
        {
            return defaults;
        }
        else
        {
            if (pattribute.ConstructorArguments.Count() > 0)
            {
                return pattribute.ConstructorArguments.First().Value.ToString();
            }
            else
            {
                return "";
            }
        }
    }
}