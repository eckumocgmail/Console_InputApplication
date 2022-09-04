
[Icon("home")]
[Label("Логическая переменная")]
[Description("Атрибут логическая переменной определяет способ ввода через элемент управления checkbox")]
public class InputBoolAttribute : BaseInputAttribute {
    public InputBoolAttribute() : base(InputTypes.Custom) { }
    public override string OnValidate(object model, string property, object value)
    {
        return (value != null && value is bool) ? null:
            "Тип данных свойства ввода задан некорректно";
    }
    public override string OnGetMessage(object model, string property, object value)

    {
        return "Тип данных свойства ввода задан некорректно";
    }

    
}