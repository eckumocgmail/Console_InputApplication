using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static InputApplicationProgram;
using System.Threading.Tasks;

public class TestingUnit : TestingElement
{
    public TestingUnit()
    {
    }

    public TestingUnit(TestingUnit parent) : base(parent)
    {
    }

    public override void OnTest()
    {
        
     
    }
}
public class TestingElement 
{
    protected List<string> Messages = new List<string>();
    protected string Name;
    protected TestingElement Parent;

    public TestingElement( ) : base( )
    {
        this.Name = GetTypeName(this.GetType());
    }
 

    public TestingElement(TestingUnit parent)
    {
        Parent = parent;
    }


    public virtual void OnTest() { 
         
    }
    public virtual void  Test() { }
    public virtual TestingReport DoTest(){
        this.Info($"Выполняем тест {this.GetType().Name}");
        if (this.IsExtends(typeof(TestingUnit)))
        {
            this.Info(this.Children.Select(p => p.Name).ToJsonOnScreen());
            ConfirmContinue();
        }
        var Report = new TestingReport();
        TestingReport report = Report;
        report.Messages = this.Messages;
        report.Name = this.GetType().GetLabel();
        try
        {
            report.Started = DateTime.Now;
            this.OnTest();
            this.OnTest();
        }
        catch (Exception ex)
        {
            this.Error("При выполнении текста "+this.GetTypeName()+" проброшено исключение: ", ex);

            report.Failed = true;
            report.Messages.Add(ex.ToString());

        }
        finally
        {
            report.Ended = DateTime.Now;
            if( (this.IsExtends(typeof(TestingUnit))) == false)
            {
                InputApplicationProgram.Clear();
                this.Info(report.ToDocument());
                
                InputApplicationProgram.ConfirmContinue("Продолжить выполнение?");
            }
            foreach (var p in GetChildren())
            {
                report.SubReports[p.Name] = p.DoTest();
                if (report.SubReports[p.Name].Failed)
                {
                    report.Failed = true;
                }
            }
            
        }
        return report;
    }

    public LinkedList<TestingElement> Children=new LinkedList<TestingElement>();
    public void Push(TestingElement pchild)
        => Append(pchild);
    public void Append(TestingElement pchild)
    {
        this.Children.AddLast(pchild);
        pchild.Parent = this;
    }
    private IEnumerable<TestingElement> GetChildren()
        => Children;

    private string GetTypeName(Type propertyType)
    {
        try
        {
            if (propertyType == null)
                throw new ArgumentNullException("type");
            string name = propertyType.Name;
            if (name == null) return "";
            if (name.IndexOf("`") != -1)
                name = name.Substring(0, name.IndexOf("`"));

            var arr = propertyType.GetGenericArguments();
            if (arr.Length > 0)
            {
                name += '<';
                foreach (var arg in arr)
                {
                    name += GetTypeName(arg) + ",";
                }
                name = name.Substring(0, name.Length - 1);
                name += '>';
            }
            return name;
        }
        catch (Exception ex)
        {
            throw new Exception(nameof(GetTypeName)+" => ",ex);
        }
       
    }
}
 
