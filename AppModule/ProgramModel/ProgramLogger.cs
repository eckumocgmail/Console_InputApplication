using System;



/// <summary>
/// Функции логирования сообщений
/// </summary>
public class ProgramLogger: MyValidatableObject
{

    /// <summary>
    /// 
    /// </summary>        
    public static void LogInfo(params string[] args)
    {
        Console.ForegroundColor = (args.Length == 1) ?
            ConsoleColor.Yellow : ConsoleColor.White;
        for ( int i=0; i<args.Length; i++ )
        {                
            var arg = args[i];
            if (arg.StartsWith("\n\t") == false && arg.StartsWith("\t") == false)
                Console.WriteLine("\t" + arg);
            else Console.WriteLine(arg);
        }
        Console.ResetColor();
    }


    /// <summary>
    /// Вывод инф-ого сообщения 
    /// </summary>        
    public static void Log(params string[] args)
    {
        Console.ForegroundColor = ConsoleColor.White;
        foreach (var arg in args)
        {
            if (arg.StartsWith("\t") == false)
            {
                Console.WriteLine("\t" + arg);
            }
            else
            {
                Console.WriteLine( arg);
            }
        }
        Console.ResetColor();
    }
     

    /// <summary>
    /// Вывод сообщения об ошибке
    /// </summary>        
    public static void LogWarn(params string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        foreach (var arg in args)
        {
            if(arg.StartsWith("\t") == false)
            {
                Console.WriteLine("\t"+arg);
            }
            else
            {
                Console.WriteLine(arg);
            }
        }
        Console.ResetColor();
    }


}
