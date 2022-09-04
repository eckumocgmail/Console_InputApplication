using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
 

using System.Reflection;

using static InputApplicationProgram;


/// <summary>
/// Реализует функций регистрации операций с выполнением
/// в обратной последовательности
/// </summary>
public class ProgramHistory: ProgramDirectory
{        

    /// <summary>
    /// Номер состояния в истории
    /// </summary>
    public static int Index = -1;

    /// <summary>
    /// Истрия последовательность переходов
    /// </summary>
    public static List< string > History = new List< string >();

    /// <summary>
    /// Когда true-индекс перемещается вперёд по истории
    /// </summary>
    public static bool FastForward = true;

    public ProgramHistory(IServiceProvider provider, string path) : base(provider, path)
    {
    }


    /// <summary>
    /// Переход к следующему состоянию
    /// </summary>
    /// <param name="State"></param>
    public static void NextState(string state)
    {
        Clear();
        if(FastForward == true)
        {
            History.Add(state);
            Index++;
        }
        else
        {
            FastForward = true;
            while(History.Count()>(Index+1))
                History.RemoveAt(History.Count()-1);
        }                       
        ProgramHistory.WriteState();
    }


    /// <summary>
    /// Печать состояния в консоли
    /// </summary>
    public static void WriteState()
    {
        Warn($"\n\tИстория: ");
        for (int ind = 0; ind < History.Count; ind++)
        {
            if (ind != Index)
            {
                Info("\t  (" + (ind + 1) + ") " + History[ind]);
            }
            else
            {
                Warn("\t  (" + (ind + 1) + ") " + History[ind]);

            }
        }
    }


    /// <summary>
    /// Переход к предыдущему состоянию
    /// </summary>
    public static void GoBack()
    {
        if (Index > 0)
        {
            Index = Index - 1;
        } 
        FastForward = false;
        UpdateState();
    }


    /// <summary>
    /// Вернуться к предыдущему состоянию.
    /// Отличается от GoBack() тем что индекс передвигается вперёд
    /// </summary>
    public static void ReturnState()
    {            
        GoBack();
    }

    
    /// <summary>
    /// Восстановление состояния
    /// </summary>        
    public static void LoadState(string state)
    {
        var type = GetTypeByMethod(state);
        if (type != null)
            Warn("Не найден тип реализующий операцию " + state);
        LoadState(type, state);
    }


    /// <summary>
    /// Выполнение метода перехода к текущему состоянию
    /// </summary>
    public static void UpdateState()
    {
        string state = History[Index];
        Clear();                    
        var type = GetTypeByMethod(state);                        
        LoadState(type, state);         
    }


    /// <summary>
    /// Поиск типа реализующего операцию с заданным наименованием
    /// </summary>
    public static Type GetTypeByMethod(string name)
    {
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {             
            foreach (var method in type.GetMethods())
            {                    
                if (method.Name == name)
                    return type;                    
            }
        }
        throw new Exception("Не найден тип реализующий операцию "+name);
    }


    /// <summary>
    /// Вывод пмаршрутов к операциям
    /// </summary>
    public static void TraceMethods()
    {
        foreach(var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            Info(type.Name);
            foreach(var method in type.GetMethods())
            {
                Info($"/{type.Name}/{method.Name}()");
            }                
        }
    }


    /// Восстановление состояния 
    public static void LoadState(Type type, string state)
    {        
        Info(type.Name);
        var method = type.GetMethod(state);
        method.Invoke(null, new object[0]);
    }
}
