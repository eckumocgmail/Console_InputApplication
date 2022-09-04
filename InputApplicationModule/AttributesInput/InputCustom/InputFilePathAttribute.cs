[Label("Файл")]
public class InputFilePathAttribute : BaseInputAttribute
{
    public InputFilePathAttribute() : base(InputTypes.File) { }
    public InputFilePathAttribute(string exts) : base(InputTypes.File)
    {

    }

    public override string OnValidate(object model, string property, object value)
    {
        if (System.IO.File.Exists(value.ToString()))
        {
            return null;
        }            
        return "Путь задан неверно";
    }

    public override string OnGetMessage(object model, string property, object value)
    {
        throw new System.NotImplementedException();
    }
}
