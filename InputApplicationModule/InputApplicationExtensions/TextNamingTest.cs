[Label("Проверка идентфиифкации имён")] 
public class TextNamingTest : TestingElement
{
    public override void OnTest()

    {
        canConvertIdentifier();
        canConvertNameToDiffrentStyles();
         
    }


    protected  void canConvertIdentifier()
    {
        if (TextNaming.ToCamelStyle("HomeController") != "homeController")
            Messages.Add("Не удалось применить CamelStyle");
        Messages.Add("Есть функция получения идентификатора в форме CamelStyle");
    }

    private void canConvertNameToDiffrentStyles()
    {
        string capitalStyle = "AppModule";
        
        Messages.Add($"Имя: [{capitalStyle}] в SnakeStyle:[{capitalStyle.ToSnakeStyle()}]");
        Messages.Add($"Имя: [{capitalStyle}] в CamelStyle:[{capitalStyle.ToCamelStyle()}]");
        Messages.Add($"Имя: [{capitalStyle}] в KebabStyle:[{capitalStyle.ToKebabStyle()}]");        
    }

}

 
