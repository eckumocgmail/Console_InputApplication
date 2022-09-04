using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Label("Файл")]
public class InputFileAttribute: BaseInputAttribute
{
    public InputFileAttribute() : base(InputTypes.File) { }
    public InputFileAttribute(string exts) : base(InputTypes.File)
    {

    }

    public override string OnValidate(object model, string property, object value)
    {
        return "";
    }

    public override string OnGetMessage(object model, string property, object value)
    {
        throw new NotImplementedException();
    }
}
