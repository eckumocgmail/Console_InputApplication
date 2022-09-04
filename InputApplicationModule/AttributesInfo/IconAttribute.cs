using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Icon("help")]
[Label("Иконка")]
public class IconAttribute : Attribute
{
    public string Icon { get; }
    public IconAttribute(string Icon)
    {
        this.Icon = Icon;
    }

}