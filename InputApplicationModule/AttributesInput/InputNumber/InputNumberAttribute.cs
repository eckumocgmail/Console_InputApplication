using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

[Label("Численный тип")]
public class InputNumberAttribute : BaseInputAttribute
{
    public InputNumberAttribute() : base(InputTypes.Number) { }
    public InputNumberAttribute( string expression = null) : base(InputTypes.Number)
    {
    }

    public override string OnValidate(object model, string property, object value)
    {
        throw new NotImplementedException();
    }

    public override string OnGetMessage(object model, string property, object value)
    {
        throw new NotImplementedException();
    }
}

