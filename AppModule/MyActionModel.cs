using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Модель параметров вызова удаленной процедуры
/// </summary>
public class MyActionModel
{

    /// <summary>
    /// Http-метод
    /// </summary>
    public string Name { get; set; }
    public string Path { get; set; }    
    public string Method { get; set; }

    [NotMapped]
    [JsonIgnore]
    public List<string> PathStr
    {
        set
        {
            string spath = "";
            value.ForEach(p => { spath += "/" + p; });
            Path = spath;
        }
    }



    public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, MyParameterDeclarationModel> Parameters { get; set; } = new Dictionary<string, MyParameterDeclarationModel>();


}
