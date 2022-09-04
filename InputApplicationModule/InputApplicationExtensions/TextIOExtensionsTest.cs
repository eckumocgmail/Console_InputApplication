public class TextIOExtensionsTest : TestingElement
{

    public void GetFilesTest() {
        "D:\\".GetFiles().ToJsonOnScreen().WriteToConsole();
        Messages.Add("Строки расширены функциями доступа к файлам");
    }
 
    public void ReadTextTest() {
        "asd".WriteToFile("D:\\1.txt");
    }

    public override void OnTest()

    {
        GetFilesTest();
    
        ReadTextTest();  
    }
}
