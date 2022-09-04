using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

[Label("Многострочный комментарий")]
public class InputMultilineTextAttribute : BaseInputAttribute
{
    public InputMultilineTextAttribute() : base(InputTypes.MultilineText) { }
    public InputMultilineTextAttribute(string expression =null): base(InputTypes.MultilineText)
    {
    }

    public override string OnValidate(object model, string property, object value)
    {
        return null;
    }

    public override string OnGetMessage(object model, string property, object value)
    {
        throw new NotImplementedException();
    }
}

