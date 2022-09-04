using System;

public class TextCountingExtensionsTest : TestingElement
{
 
    public void ToMultiCountTest() {
        try
        {
            if ("User".ToMultiCount() != "Users")
            {
                Messages.Add("Не реализована функция получения формы сущетвительного в форме множественного числа");
            }
            else
            {
                Messages.Add("Реализована функция получения формы сущетвительного в форме множественного числа");

            }
        }
        catch(Exception ex)
        {
            Messages.Add("Не реализована функция получения формы сущетвительного в форме множественного числа "+ex);

        }
        
    }
    public void ToSingleCountTest() {
        try
        {
            if ("Users".ToSingleCount() != "User")
            {
                Messages.Add("Не реализована функция получения формы сущетвительного в форме единственного числа");
            }
            else
            {
                Messages.Add("Реализована функция получения формы сущетвительного в форме единственного числа");

            }
        }
        catch (Exception ex)
        {
            Messages.Add("Не реализована функция получения формы сущетвительного в форме единственного числа "+ex);

        }
    }
    public override void OnTest()

    {

        ToMultiCountTest();
        ToSingleCountTest();  
    }
}
