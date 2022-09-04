 
using System;
using System.Collections;


/// <summary>
/// Выполняет установку значений свойств
/// </summary>
public class TextDataSetter
{

    public static void SetValue(object target, string property, object value)
    {
        var p = target.GetType().GetProperty(property);
        var ptype = p.PropertyType;
        if (Typing.IsDateTime(ptype))
        {
            DateTime? date = value.ToString().ToDate();
            p.SetValue(target, date);
        }
        else if (Typing.IsNumber(ptype))
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                if (Typing.IsNullable(ptype) && Typing.IsPrimitive(ptype) == false)
                {
                    p.SetValue(target, null);
                }
                else
                {
                    throw new Exception($"Свойство {property} не может хранить ссылку на null");
                }
            }
            else
            {
                string propertyType = Typing.ParsePropertyType(ptype);
                switch (propertyType)
                {

                    case "Single": { p.SetValue(target, System.Single.Parse(value.ToString())); break; }
                    case "System.Single": { p.SetValue(target, System.Single.Parse(value.ToString())); break; }
                    case "Double": { p.SetValue(target, System.Double.Parse(value.ToString())); break; }
                    case "System.Double": { p.SetValue(target, System.Double.Parse(value.ToString())); break; }
                    case "Decimal": { p.SetValue(target, System.Decimal.Parse(value.ToString())); break; }
                    case "System.Decimal": { p.SetValue(target, System.Decimal.Parse(value.ToString())); break; }
                    case "Int16": { p.SetValue(target, System.Int16.Parse(value.ToString())); break; }
                    case "System.Int16": { p.SetValue(target, System.Int16.Parse(value.ToString())); break; }
                    case "Int32": { p.SetValue(target, System.Int32.Parse(value.ToString())); break; }
                    case "System.Int32": { p.SetValue(target, System.Int32.Parse(value.ToString())); break; }
                    case "Nullable<Int32>": { p.SetValue(target, System.Int32.Parse(value.ToString())); break; }
                    case "Nullable<System.Int32>": { p.SetValue(target, System.Int32.Parse(value.ToString())); break; }
                    case "Int64": { p.SetValue(target, System.Int64.Parse(value.ToString())); break; }
                    case "System.Int64": { p.SetValue(target, System.Int64.Parse(value.ToString())); break; }
                    case "UInt16": { p.SetValue(target, System.UInt16.Parse(value.ToString())); break; }
                    case "System.UInt16": { p.SetValue(target, System.UInt16.Parse(value.ToString())); break; }
                    case "UInt32": { p.SetValue(target, System.UInt32.Parse(value.ToString())); break; }
                    case "System.UInt32": { p.SetValue(target, System.UInt32.Parse(value.ToString())); break; }
                    case "UInt64": { p.SetValue(target, System.UInt64.Parse(value.ToString())); break; }
                    case "System.UInt64": { p.SetValue(target, System.UInt64.Parse(value.ToString())); break; }
                    default:
                        throw new Exception($"Тип свойства {property} {propertyType} неподдрживается");
                }
            }
            if (value != null && (value.GetType().Name == "Int64" || value.GetType().Name == "Int32"))
            {
                value = Int32.Parse(value.ToString());
            }
        }
        else if (Typing.IsText(ptype))
        {
            p.SetValue(target, value.ToString());
        }
        else
        {
            var pvalue = FromText(value, ptype.Name);
            p.SetValue(target, pvalue);
        }
    }

    public static object ToType(string textValue, Type propertyType)
    {
        return FromText(textValue, propertyType.GetNameOfType());
    }

    /// <summary> 
    /// </summary>
    public static object FromText(object value, string propertyType)
    {
        switch (propertyType)
        {
            case "Boolean": { return value.ToString().ToBool(); }
            case "System.Boolean": { return value.ToString().ToBool(); }
            case "String": { return value.ToString(); }
            case "System.String": { return value.ToString(); }
            case "Single": { return System.Single.Parse(value.ToString()); }
            case "System.Single": { return System.Single.Parse(value.ToString()); }
            case "Double": { return System.Double.Parse(value.ToString()); }
            case "System.Double": { return System.Double.Parse(value.ToString()); }
            case "Decimal": { return System.Decimal.Parse(value.ToString()); }
            case "System.Decimal": { return System.Decimal.Parse(value.ToString()); }
            case "Int16": { return System.Int16.Parse(value.ToString()); }
            case "System.Int16": { return System.Int16.Parse(value.ToString()); }
            case "Int32": { return System.Int32.Parse(value.ToString()); }
            case "System.Int32": { return System.Int32.Parse(value.ToString()); }
            case "Nullable<Int32>": { return System.Int32.Parse(value.ToString()); }
            case "Nullable<System.Int32>": { return System.Int32.Parse(value.ToString()); }
            case "Int64": { return System.Int64.Parse(value.ToString()); }
            case "System.Int64": { return System.Int64.Parse(value.ToString()); }
            case "UInt16": { return System.UInt16.Parse(value.ToString()); }
            case "System.UInt16": { return System.UInt16.Parse(value.ToString()); }
            case "UInt32": { return System.UInt32.Parse(value.ToString()); }
            case "System.UInt32": { return System.UInt32.Parse(value.ToString()); }
            case "UInt64": { return System.UInt64.Parse(value.ToString()); }
            case "System.UInt64": { return System.UInt64.Parse(value.ToString()); }
            default:
                throw new Exception($"Тип  {propertyType} неподдрживается");
        }
    }
}
/// <summary>
/// Выполняет установку значений свойств
/// </summary>
public class Setter
{
    public static object FromText(object value,string propertyType)
    {
        if (value == null)
            return null;
        switch (propertyType)
        {
            case "Boolean": { return value.ToString().ToBool(); }
            case "System.Boolean": { return value.ToString().ToBool(); }
            case "String": { return value.ToString(); }
            case "System.String": { return value.ToString(); }
            case "Single": { return System.Single.Parse(value.ToString()); }
            case "System.Single": { return System.Single.Parse(value.ToString()); }
            case "Double": { return System.Double.Parse(value.ToString()); }
            case "System.Double": { return System.Double.Parse(value.ToString()); }
            case "Decimal": { return System.Decimal.Parse(value.ToString()); }
            case "System.Decimal": { return System.Decimal.Parse(value.ToString()); }
            case "Int16": { return System.Int16.Parse(value.ToString()); }
            case "System.Int16": { return System.Int16.Parse(value.ToString()); }
            case "Int32": { return System.Int32.Parse(value.ToString()); }
            case "System.Int32": { return System.Int32.Parse(value.ToString()); }
            case "Nullable<Int32>": { return System.Int32.Parse(value.ToString()); }
            case "Nullable<System.Int32>": { return System.Int32.Parse(value.ToString()); }
            case "Int64": { return System.Int64.Parse(value.ToString()); }
            case "System.Int64": { return System.Int64.Parse(value.ToString()); }
            case "UInt16": { return System.UInt16.Parse(value.ToString()); }
            case "System.UInt16": { return System.UInt16.Parse(value.ToString()); }
            case "UInt32": { return System.UInt32.Parse(value.ToString()); }
            case "System.UInt32": { return System.UInt32.Parse(value.ToString()); }
            case "UInt64": { return System.UInt64.Parse(value.ToString()); }
            case "System.UInt64": { return System.UInt64.Parse(value.ToString()); }
            default:
                throw new Exception($"Тип  {propertyType} неподдрживается");
        }
    }
    public static void SetValue( object target, string property, object value )
    {
        var p = target.GetType().GetProperty(property);
        if (Typing.IsDateTime(p))
        {
            string text = value == null ? "" :value.ToString();
            DateTime? date = text.ToDate();
            p.SetValue(target, date);
        }
        else if (Typing.IsNumber(p))
        {
            if( value == null || string.IsNullOrEmpty(value.ToString()))
            {
                if(Typing.IsNullable(p) && Typing.IsPrimitive(p.PropertyType)==false)
                {
                    p.SetValue(target, null);
                }
                else
                {
                    throw new Exception($"Свойство {property} не может хранить ссылку на null");
                }
            }
            else
            {
                string propertyType = Typing.ParsePropertyType(p.PropertyType);
                switch (propertyType)
                {
                  
                    case "Single": { p.SetValue(target, System.Single.Parse(value.ToString())); break; }
                    case "System.Single": { p.SetValue(target, System.Single.Parse(value.ToString())); break; }
                    case "Double": { p.SetValue(target, System.Double.Parse(value.ToString())); break; }
                    case "System.Double": { p.SetValue(target, System.Double.Parse(value.ToString())); break; }
                    case "Decimal": { p.SetValue(target, System.Decimal.Parse(value.ToString())); break; }
                    case "System.Decimal": { p.SetValue(target, System.Decimal.Parse(value.ToString())); break; }
                    case "Int16": { p.SetValue(target, System.Int16.Parse(value.ToString())); break; }
                    case "System.Int16": { p.SetValue(target, System.Int16.Parse(value.ToString())); break; }
                    case "Int32": { p.SetValue(target, System.Int32.Parse(value.ToString())); break; }
                    case "System.Int32": { p.SetValue(target, System.Int32.Parse(value.ToString())); break; }
                    case "Nullable<Int32>": { p.SetValue(target, System.Int32.Parse(value.ToString())); break; }
                    case "Nullable<System.Int32>": { p.SetValue(target, System.Int32.Parse(value.ToString())); break; }
                    case "Int64": { p.SetValue(target, System.Int64.Parse(value.ToString())); break; }
                    case "System.Int64": { p.SetValue(target, System.Int64.Parse(value.ToString())); break; }
                    case "UInt16": { p.SetValue(target, System.UInt16.Parse(value.ToString())); break; }
                    case "System.UInt16": { p.SetValue(target, System.UInt16.Parse(value.ToString())); break; }
                    case "UInt32": { p.SetValue(target, System.UInt32.Parse(value.ToString())); break; }
                    case "System.UInt32": { p.SetValue(target, System.UInt32.Parse(value.ToString())); break; }
                    case "UInt64": { p.SetValue(target, System.UInt64.Parse(value.ToString())); break; }
                    case "System.UInt64": { p.SetValue(target, System.UInt64.Parse(value.ToString())); break; }
                    default:
                        throw new Exception($"Тип свойства {property} {propertyType} неподдрживается");
                }


            }
            /*if (value != null && (value.GetType().Name == "Int64" || propertyTypeName == "Int32"))
            {
                value = Int32.Parse(value.ToString());
            }*/
        }
        else if (Typing.IsText(p)) 
        {
            p.SetValue(target, value.ToString());
        }
        else
        {
            if (Typing.IsCollectionType(p.PropertyType) == false)
            {
                var pvalue = FromText(value, p.PropertyType.Name);
                p.SetValue(target, pvalue);
            }
            
        }

    }
}