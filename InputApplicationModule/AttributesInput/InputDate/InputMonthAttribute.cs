using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Label("Отчётный месяц")]
public class InputMonthAttribute : BaseInputAttribute
{
    public InputMonthAttribute( ) : base(InputTypes.Month)
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

