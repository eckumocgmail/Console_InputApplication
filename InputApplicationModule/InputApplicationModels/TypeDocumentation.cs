using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// 
/// </summary>
public class TypeDocumentation : FromAttributes
{




    public class SummaryOfProperty : FromAttributes
    {
        public string Name { get; set; }
        public string Icon { get; set; } = "home";
        public string Label { get; set; } = "";
        public string Description { get; set; } = "";
        public string HelpMessage { get; set; } = "";

        public SummaryOfProperty(Type t, string prop)=>Init(t);
        
    }


    [NotNullNotEmpty]
    public string TypeName { get; set; }

    [InputIcon]
    public string EntityIcon { get; set; } = "home";

    [InputText]
    public string EntityLabel { get; set; } = "";

    [InputText]
    public string HelpMessage { get; set; } = "";

    [InputMultilineText]
    public string ClassDescription { get; set; } = "";




    public override Dictionary<string, List<string>> ValidateOptional()
    {
        var result = new Dictionary<string, List<string>>();
        base.ValidateOptional().ToList().ForEach(kv=>result[kv.Key]=kv.Value);
 
            
        return result;
    }

    public Dictionary<string, SummaryOfProperty> PropertiesDictionary { get; set; } 
        = new Dictionary<string, SummaryOfProperty>();
    public Dictionary<string, SummaryOfProperty> ActionDictionary { get; set; }
        = new Dictionary<string, SummaryOfProperty>();


    public TypeDocumentation(Type type)
    {
        Init(type);
        TypeName = type.Name;
        type.GetOwnPropertyNames().ToList().ForEach(name =>
            PropertiesDictionary[name] = new SummaryOfProperty(type, name)
        );
        type.GetOwnMethodNames().ToList().ForEach(name =>
            ActionDictionary[name] = new SummaryOfProperty(type, name)
        );
    }
}