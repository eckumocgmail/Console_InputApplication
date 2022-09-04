[Label("Тестирование функций определения значимых типов в  тексте")]
public class TextValueExtensionsTest : TestingElement
{
    public TextValueExtensionsTest()
    {
    }

    [Label("Проверка идентификации линейных операторов")]
    public void IsLinearOperationTest() {

        if ("+".IsLinearOperation())
        {
            Messages.Add("Реализована функция определения арифметических опереторов");
        }
        else
        {
            Messages.Add("Не реализована функция определения арифметических опереторов");
        }
    }

    [Label("Проверка идентификации числовых данных")]
    public void IsNumberTest() {
        if ("110".IsNumber())
        {
            Messages.Add("Реализована функция определения чисел в тексте");
        }
        else
        {
            Messages.Add("Не реализована функция определения чисел в тексте");
        }
    }
    [Label("Проверка идентификации дат")]
    public void IsDateTest()
    {
        if ("26.08.1989".IsDate())
        {
            Messages.Add("Реализована функция определения дат в тексте");
        }
        else
        {
            Messages.Add("Не реализована функция определения дат в тексте");

        }
    }

    public override void OnTest()

    {

        IsLinearOperationTest();
        IsNumberTest();
        IsDateTest(); 
    }
}
