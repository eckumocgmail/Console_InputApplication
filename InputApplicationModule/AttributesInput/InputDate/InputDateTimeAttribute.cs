using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Label("Дата/Время")]
public class InputDateTimeAttribute : BaseInputAttribute
{
    public InputDateTimeAttribute( ) : base(InputTypes.DateTime)
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

