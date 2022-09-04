using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

[Label("Дата")]
public class InputDateAttribute : BaseInputAttribute
{
    public InputDateAttribute( ) : base(InputTypes.Date)
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

