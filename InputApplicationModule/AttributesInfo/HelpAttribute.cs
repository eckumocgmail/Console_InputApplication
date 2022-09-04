using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Label("Вспомогательное сообщение")]
[Description("Вспомогательное сообщение содержит информацию об использовании свойства, метода или типа")]
public class HelpAttribute : Attribute
{
    [Label("Текст сообщения")]
    public string Message { get; }

    public HelpAttribute(string Message)
    {
        this.Message = Message;
    }
}