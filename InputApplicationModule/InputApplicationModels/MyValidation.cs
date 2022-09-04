using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
public interface MyValidation
{
    public string Validate(object model, string property, object value);
    public string GetMessage(object model, string property, object value);
}