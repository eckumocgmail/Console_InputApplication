using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Label("Процент")]
public class InputPercentAttribute : BaseInputAttribute, MyValidation
{
    private string _error;

    public InputPercentAttribute( ) : base(InputTypes.Percent) { }
    public InputPercentAttribute(string error) : base(InputTypes.Percent)
    {
        _error = error;
    }

    public override string OnGetMessage(object model, string property, object value)
    {
        if (string.IsNullOrEmpty(_error))
        {
            return "Процент задаётся действительным числом в диапазоне от 0 до 100";
        }
        else
        {
            return _error;
        }
    }

    public override string OnValidate(object model, string property, object value)
    {        
        int x = int.Parse(value.ToString());
        if (x < 0 || x > 100)
        {
            return GetMessage(model, property, value);
        }
        else
        {
            return null;
        }   
    }

    
}