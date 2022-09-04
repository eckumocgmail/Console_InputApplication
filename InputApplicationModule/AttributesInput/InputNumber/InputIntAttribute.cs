using NetCoreConstructorAngular.Data.DataAttributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class InputIntAttribute: BaseInputAttribute
{
    public InputIntAttribute( ) : base(InputTypes.Number)
    {
    }

    public override string OnValidate(object model, string property, object value)
    {
        return (value != null && value.ToString().Trim().IsInt()) ? null : GetMessage(model, property, value);

    }

    public override string OnGetMessage(object model, string property, object value)
    {
        return $"Значение свойства {model.GetType().Name}.{property} не является целочисленным";

    }
}

