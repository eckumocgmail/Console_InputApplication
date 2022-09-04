[Label("XML-разметка")]
public class InputXmlAttribute : BaseInputAttribute
{
    public InputXmlAttribute() : base(InputTypes.Xml) { }

    public override string OnValidate(object model, string property, object value)
    {
        return null;
    }

    public override string OnGetMessage(object model, string property, object value)
    {
        throw new System.NotImplementedException();
    }
}