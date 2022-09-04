
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

 


/// <summary>
/// Модель mvc-контроллера ///{controller]///{action]
/// </summary>
public class MyControllerModel: MyActionModel
{
    //{Icon("home")]
    //{Label("Наименование")]
    //{EngText()]
    //{InputText()]
    //{NotNullNotEmpty("Необходимо ввести имя")]
    public string Name { get; set; }
    
    //{InputIcon()]
    //{Label("Иконка на панель инструментов")]
    public string Icon { get; set; }


    //{Icon("home")]
    //{Label("Наименование")]
    //{EngText()]
    //{InputMultilineText()]
    //{NotNullNotEmpty("Необходимо ввести имя")]
    public string Description { get; set; }


    //{Icon("account_tree")]
    //{Label("Путь")]
    //{InputText()]
    //{NotNullNotEmpty("Необходимо ввести путь")]
    public string Path { get; set; }



    //{NotNullNotEmpty("Необходимо зарегистрировать операции")]
    //{Label("Поддерживаемые операции")]
    //{InputStructureCollection()]
    public Dictionary<string, MyActionModel> Actions { get; set; }
            = new Dictionary<string, MyActionModel>();


    //{InputStructureCollection()]
    //{NotNullNotEmpty("Необходимо зарегистрировать операции")]
    //{Label("Поддерживаемые операции")]    
   
    public IList<string> Services { get; set; } = new List<string>();


    /// <summary>
    /// Запись информации о методах в справочник
    /// </summary>
    /// <param name="data"></param>
    public void WriteTo(IDictionary data)
    {
        Actions.ToList().ForEach(a =>
        {
            data[a.Key] = ToJson(a.Value);
        });
    }

    private object ToJson(MyActionModel value)
    {
        throw new NotImplementedException();
    }


    /*public string GetAnnotationForService()
    {
        return "@Injectable({ providedIn: 'root' })\n";
    }


    public string GetImportsForService()
    {
        return
            "import { Observable } from 'rxjs';\n" +
            "import { Injectable } from '@angular/core';\n" +
            "import { HttpClient } from '@angular/common/http';\n\n";
    }*/
}