using System;

public class TextConvertExtensionsTest : TestingElement
{

    public void ToIntTest() {
        try
        {
            if ("-11".ToInt() != -11)
            {
                throw new Exception("Error");
            }
            else            
            {
                Messages.Add("Реализована функция преобразования текста в целочисленный тип");
            }
        }catch(Exception ex)
        {
            Messages.Add("Не Реализована функция преобразования текста в целочисленный тип "+ex);


        }
    }
    public void ToBoolTest() {
    }
    public void ToFloatTest() { 
    }
    public void ToDateTest() { 
    }

    public override void OnTest()
    {
        ToIntTest();
        ToBoolTest();
        ToFloatTest();
        ToDateTest(); 
    }
}
