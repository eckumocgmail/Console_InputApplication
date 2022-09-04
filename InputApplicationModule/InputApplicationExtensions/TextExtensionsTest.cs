using System;
using System.Linq;
using System.Reflection;
namespace A {
    public class B { }
}
public class TextExtensionsTest : TestingElement
{

    public void GetFilePathTest() {
        try
        {
            throw new NotImplementedException($"{TypeExtensions2.GetTypeName(GetType())}");
        }
        catch(Exception ex)
        {
            this.Error(ex );
        }
    }

    public void GetTypesTest() {
        try
        {
            if ("A".GetTypes().Count() == 0)
                throw new Exception("Получить типы определённые в пространстве имён по имени namespace'а не удалось");
            Messages.Add("Получить типы определённые в пространстве имён по имени namespace'а удалось");
        }
        catch(Exception ex)
        {
            Messages.Add(ex.Message);
        }        
    }

    public override void OnTest()


    {
        GetFilePathTest();
        GetTypesTest();  
    }
}
