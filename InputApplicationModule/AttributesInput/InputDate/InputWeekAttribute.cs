using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Label("Отчётная неделя")]
public class InputWeekAttribute: BaseInputAttribute
{
    public InputWeekAttribute( ) : base(InputTypes.Week) { }
    public InputWeekAttribute(string exts) : base(InputTypes.Week)
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

