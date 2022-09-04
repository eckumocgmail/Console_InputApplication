[Label("Адрибут помечает своства в которых долны сохраняться IP-адреса")]
public class InputTcpIp4AddressAttribute: BaseInputAttribute
{
    public InputTcpIp4AddressAttribute(string mask = "255.255.255.0"):base(InputTypes.Text)
    {
    }

    public override string OnValidate(object model, string property, object value)
    {
        string message = GetMessage(model, property, value);
        string text = value.ToString();
        if (text.CountOfChar('.') != 3)
            return $"{message}." +
                $" Кол-во точек должно быть равно 3";
        int ctn = 0;
        foreach(string word in text.Split("."))
        {
            ctn++;
            if (word.IsNumber() == false)
            {
                return $"{message}." +
                    $"Сегмент №{ctn}: не является числом";
            }
            int number = word.ToInt();
            if (number < 1)
            {
                return $"{message}." +
                    $"Сегмент №{ctn}: содержит число ниже разрешенного диапазона";
            }
            if (number > 254)
            {
                return $"{message}." +
                    $"Сегмент №{ctn}: содержит число ниже разрешенного диапазона";
            }

        }
        return null;
    }

    public override string OnGetMessage(object model, string property, object value)
    {
        return "Tcp Ip 4 предполагает, что адрес состояит из 4 чисел из [1,254] разделённых '.'";
    }
}