using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TextModel : MyValidatableObject
{
    public string Name { get; set; }

    [InputAriphmetic()] string Ariphmetic { get; set; }
    [InputEmail()] string Email { get; set; } = "gmail";
    [InputEngText()] string EngText { get; set; }
    [InputEngWord()] string EngWord { get; set; }
    [InputMultilineText()] string MultilineText { get; set; }
    [InputPassword()] string Password { get; set; }
    [InputConfirmation("Password")] string Confirmation { get; set; }
    [InputPhone()] string Phone { get; set; }
    [InputPunctuation()] string Punctuation { get; set; }
    [InputRusText()] string RusText { get; set; }
    [InputRusWord()] string RusWord { get; set; }
    [InputTcpIp4Address()] string TcpIp4Address { get; set; }
    [InputText()] string Text { get; set; }
    [InputUrl()] string Url { get; set; }
    [InputXml()] string Xml { get; set; }

    public TextModel(): base()
    {
       
    }
}



[Label("Тестирование функций определения функций вокруг атрибутов ввода")]
public class AttributesInputTest : TestingElement
{
    public AttributesInputTest()    {    }
    public AttributesInputTest(TestingUnit parent) : base(parent)    {    }

    public override void OnTest()

    {
        var model = new TextModel();
        this.Info(model.Validate().ToJsonOnScreen());
        base.OnTest();
    }
}
 


[Label("Модель для теста")]
public class TestInputModel : MyValidatableObject
{
    
    public class NumberModel : MyValidatableObject
    {
        [InputPositiveInt] int PositiveInt { get; set; }
        [InputPositiveInt] int Int { get; set; }
        [InputPercent] int Percent { get; set; }
        [InputDecimal] int Decimal { get; set; }
    }
    public class NumberTextModel : MyValidatableObject
    {
        [InputPositiveInt] string PositiveInt { get; set; }
        [InputPositiveInt] string Int { get; set; }
        [InputPercent] string Percent { get; set; }
        [InputDecimal] string Decimal { get; set; }
    }
    public class DateModel : MyValidatableObject
    {
        [InputYear] int Year { get; set; }
        [InputMonth] int Month { get; set; }
        [InputDate] int Date { get; set; }
        [InputTime] int Time { get; set; }
        [InputWeek] int Week { get; set; }
        [InputDateTime] int DateTime { get; set; }
    }
    public class CustomModel : MyValidatableObject
    {
        [InputBool] bool Bool { get; set; }
        [InputColor] string Color { get; set; }
        [InputCreditCard] string CreditCard { get; set; }
        [InputCurrency] float Currency { get; set; }
        [InputWeek] int Week { get; set; }
        [InputFile] byte[] File { get; set; }
        [InputFilePath] string FilePath { get; set; }
        [InputHidden] string Hidden { get; set; }
        [InputIcon] string Icon { get; set; }
        [InputImage] byte[] Image { get; set; }
    }
    public class CollectionModel : MyValidatableObject
    {
        public List<string> ListString { get; set; }
        [InputStructureCollection(nameof(DateModel))] public List<DateModel> ListDateModel { get; set; }
    }
}