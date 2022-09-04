using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TextFactoryExtensionsTest : TestingElement
{
    public override void OnTest()
    {
        CanFindTypeFromText();
        CanCreateInstanceFromTypeName();
  
    }

    private void CanCreateInstanceFromTypeName()
    {
        try
        {
            nameof(TestingReport).New();
            Messages.Add($"Удалось создать экземпляр класса по имени типа");
        }
        catch (Exception ex)
        {
            Messages.Add($"Не удалось создать экземпляр класса по имени типа "+ex);
        }
    }

    private void CanFindTypeFromText()
    {
        try
        {
            if (nameof(DataTable).ToType() == null)
                throw new Exception($"Удалось найти тип по имени DataTable");
            nameof(TestingReport).ToType();
            Messages.Add($"Удалось найти тип по имени");
        }
        catch (Exception ex)
        {
            Messages.Add($"Не удалось найти тип по имени "+ex);
        }
    }
}