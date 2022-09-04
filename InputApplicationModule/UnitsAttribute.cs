using System;

public class UnitsAttribute : Attribute
{
    protected string _postfix;

    public UnitsAttribute( string postfix ) {
        _postfix = postfix;
    }
}