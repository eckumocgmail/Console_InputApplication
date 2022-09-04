public class TextNamingExtensionsTest : TestingElement
{

    public void ParseStyleTest() {
        this.Info("ToString".ParseStyle().ToString());
        if("ToString".ParseStyle().ToString()!= "Capital")
        {
            Messages.Add("Реализована функция определения стиля записи идентификатора");
        }
        else
        {
            Messages.Add("Не реализована функция определения стиля записи идентификатора "+ "ToString".ParseStyle().ToString());

        }
    }

    public override void OnTest()

    {
        ParseStyleTest();
         
    }
}
