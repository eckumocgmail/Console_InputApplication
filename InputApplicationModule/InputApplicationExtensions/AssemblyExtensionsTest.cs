using System;
using System.Linq;
using System.Reflection;

[Label("Тетсрование функции выбора типов из сборки")]
public class AssemblyExtensionsTest : TestingElement
{
    public override void OnTest()

    {
        try
        {
            if (Assembly.GetExecutingAssembly().GetAttributes().Count() > 0)
                Messages.Add("Реализована функция получения атрибутов из сборки" );
            else Messages.Add("Не реализована функция получения атрибутов из сборки");
        }
        catch (Exception)
        {
            Messages.Add("Не реализована функция получения атрибутов из сборки");
        }
        try
        {
            Assembly.GetExecutingAssembly().GetControllers();
            Messages.Add("Реализована функция получения контроллеров из сборки => \n" + 
                    Assembly.GetExecutingAssembly().GetControllers().Select(t => t.GetTypeName()).ToJsonOnScreen()); 
        }
        catch (Exception ex)
        {
            Messages.Add("Не реализована функция получения контроллеров из сборки: "+ex.Message);
        }
        try
        {
            Assembly.GetExecutingAssembly().GetDataContexts() ;
            Messages.Add("Реализована функция получения контектов базы данных из сборки");
        }
        catch (Exception)
        {
            Messages.Add("Не реализована функция получения контектов базы данных из сборки");
        }

        try
        {
            Assembly.GetExecutingAssembly().GetEntitiesTypes().Count();
            Messages.Add("Реализована функция получения сущностей из сборки ");
        }
        catch (Exception)
        {
            Messages.Add("Не реализована функция получения сущностей из сборки");
        }
         
    }
}
