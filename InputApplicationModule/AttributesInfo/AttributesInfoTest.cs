using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using static InputApplicationProgram;

[Label("Проверяет работоспособность получения информационных аттрибутов")]
public class AttributesInfoTest : TestingElement
{
    public AttributesInfoTest() { }
    public AttributesInfoTest(TestingUnit parent) : base(parent) {}

    [Label("Label")]
    [Description("Description")]
    [Icon("Icon")]
    [Help("Help")]
    public class Model
    {

        [Label("Label")]
        [Description("Description")]
        [Icon("Icon")]
        [Help("Help")]
        public byte[] Data { get; set; }

        [Label("Label")]
        [Description("Description")]
        [Icon("Icon")]
        [Help("Help")]
        public void Init()
        {

        }
    }


    [Label("Выполнение проверки")]
    public override void OnTest()
    {
         
        var documentation = new TypeDocumentation(typeof(Model));
        Console.Clear();
        Console.WriteLine(TypeExtensions2.GetTypeName(typeof(Model)));
        Console.WriteLine(documentation.ToJsonOnScreen());
        ConfirmContinue();
        if (String.IsNullOrEmpty(documentation.EntityIcon))
        {
            Messages.Add("Функция полчения ключа к иконке возвращает константы переданные через атрибуты работает корректно+");
        }
        else
        {
            Messages.Add("Функция полчения ключа к иконке возвращает константы переданные через атрибуты не работает корректно+");
        }
        if (String.IsNullOrEmpty(documentation.ClassDescription))
        {
            Messages.Add("Функция определения класса возвращает константы переданные через атрибуты работает корректно+");
        }
        else
        {
            Messages.Add("Функция определения класса возвращает константы переданные через атрибуты не работает корректно+");
        }

                            
        if (String.IsNullOrEmpty(documentation.ClassDescription))
        {
            Messages.Add("Функция определения класса возвращает константы переданные через атрибуты работает корректно+");
        }
        else
        {
            Messages.Add("Функция определения класса возвращает константы переданные через атрибуты не работает корректно+");
        }
        +
                        Messages.Add(documentation.Validate().ToJsonOnScreen());
    } 
}
 
