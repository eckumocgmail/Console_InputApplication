using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class InputDecimalAttribute : BaseInputAttribute
{
    private readonly int length;

    public InputDecimalAttribute(int length=2 ) : base( InputTypes.Number )
    {
        this.length = length;
    }

    public override string OnValidate(object model, string property, object value)
    {
        if( value is null)
        {
            return this.GetMessage(model, property, value);
        }
        else
        {
            string text = value.ToString();
            if( text.IsFloat())
            {
                return null;
            }
            else
            {
                return GetMessage(model,property,value);
            }
        }
    }

    public override string OnGetMessage(object model, string property, object value)
    {
        return $"Значение не является десятичным числом";
    }
}

