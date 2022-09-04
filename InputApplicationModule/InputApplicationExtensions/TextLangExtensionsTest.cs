using System.Collections.Generic;

public class TextLangExtensionsTest : TestingElement
{

    public void IsRusTest() {
        if ("Привет".IsRus())
        {
            Messages.Add("Реализована функция определния слов записанных кириллицей");
        }
        else
        {
            Messages.Add("Не реализована функция определния слов записанных кириллицей");
        }
    }
    public void IsEngTest() {
        if ("Hello".IsEng())
        {
            Messages.Add("Реализована функция определния слов записанных латиницей");
        }
        else
        {
            Messages.Add("Не реализована функция определния слов записанных латиницей");
        }
    }

    public override void OnTest()
    {
        IsRusTest();
        IsEngTest();  
    }
}
