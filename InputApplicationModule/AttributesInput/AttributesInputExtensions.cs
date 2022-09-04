using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Label("Расширение информационными атрибутами")]
[Description("Предоставляет функции для волучения значения атрибутов AttributesInfo")]
public static class AttributesIputExtensions
{

    public static string GetPropertyInputType( this Type type, string proper )
    {
        var data = type.GetProperties().First(p => p.Name == proper)
            .GetCustomAttributesData()
            .Where(data => data.AttributeType.IsExtendsFrom(typeof(BaseInputAttribute)))
            .FirstOrDefault();
        if (data == null)
        {
            //определяем тип по типу данных
            return "Text";
        }
        else
        {
            return TypeExtensions2.GetTypeName(data.AttributeType).Replace("Attribute", "").Replace("Input", "");
        }
    }
}