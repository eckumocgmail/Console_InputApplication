using System;


[Description("Макркирует свойства, для которых запрещён ввода через формы пользовательского интерфейса")]
public class NotInputAttribute : Attribute
{
    public NotInputAttribute(string message=null)
    {

    }
}