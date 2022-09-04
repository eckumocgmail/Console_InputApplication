using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Label("Пароль")]
public class InputPasswordAttribute : BaseInputAttribute
{
    public static string GetPasswordValue( object target ){
        object val = GetValueMarkedByAttribute(target, nameof(InputPasswordAttribute));
        return val != null ? val.ToString(): "";
    }

    private static object GetValueMarkedByAttribute(object target, string v)
    {
        throw new NotImplementedException();
    }

    public InputPasswordAttribute(  ) : base(InputTypes.Password)
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

