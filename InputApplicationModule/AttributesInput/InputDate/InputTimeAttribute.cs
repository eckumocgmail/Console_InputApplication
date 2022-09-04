using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Label("Время")]
[Icon("access_time")]
public class InputTimeAttribute : BaseInputAttribute
{
    public InputTimeAttribute( ) : base(InputTypes.Time)
    {
    }
  
    public override string OnValidate(object model, string property, object value)
    {
        string message = this.GetMessage(model, property, value);
        if (value == null)
        {
            return message;
        }
        else
        {
            return (value is DateTime)? message: null;
        }
    }

    public override string OnGetMessage(object model, string property, object value)
    {
        throw new NotImplementedException();
    }
}

