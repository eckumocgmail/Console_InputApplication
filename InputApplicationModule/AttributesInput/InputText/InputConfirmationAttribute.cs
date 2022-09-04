using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 
public class InputConfirmationAttribute: BaseInputAttribute
{
    private readonly string _property;

    public InputConfirmationAttribute( string property) : base(InputTypes.Password)
    {
        this._property = property;
    }

    public override string OnValidate(object model, string property, object value)
    {
        object value2 = model.GetType().GetProperty(this._property).GetValue(model);
        if (value != null && value2 != null)
        {
            if (value.ToString() != value2.ToString())
                return GetMessage(model, property, value);
        }
        return null;
    }

    public override string OnGetMessage(object model, string property, object value)
    {
        return $"Значение не эвивалентно свойству {GetPropertyLabel(model.GetType(), this._property)}";
    }

    private object GetPropertyLabel(Type type, string property)
    {
        var data = type.GetProperties().First(p => p.Name == property).GetCustomAttributesData()
            .Where(data => data.AttributeType.Name == nameof(LabelAttribute)).FirstOrDefault();
        return data != null ? data.ConstructorArguments.First().Value.ToString() : property;
    }
}
 