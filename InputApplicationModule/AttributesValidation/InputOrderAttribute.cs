using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class InputOrderAttribute: Attribute
{
    public readonly int _Order;

    public InputOrderAttribute(int order)
    {
        this._Order = order;
    }
}

