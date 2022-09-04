[Label("Номер кредитной карты")]
[Description(
    "Номер карты состоит из 4-групп " +
    "по 4-цифры разделённых знаком тирэ." +
    "Пример: 3306-3305-4440-5555")]
public class InputCreditCardAttribute: BaseInputAttribute
{
    public InputCreditCardAttribute() : base(InputTypes.CreditCard) { }

    public override string OnValidate(object model, string property, object value)
    {
        string textValue = value.ToString();
        if (textValue.Length != "0000-0000-0000-0000".Length)
            return GetMessage(model, property, value);
        if(textValue[4] != '-' || textValue[9] != '-' || textValue[14] != '-' )
            return GetMessage(model, property, value);
        for(int i=0; i<textValue.Length; i++)
        {
            if (textValue[i].IsNumber() == false && i!=14 && i!=4 && i!=9)
                return i+" сивол должен быть числом";
        }
        return null;
    }

    public override string OnGetMessage(object model, string property, object value)
    {
        return "Номер карты имеет формат XXXX-XXXX-XXXX-XXXX где X-цифра";
    }
}