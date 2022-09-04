using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[Label("Программа индикатора прогресса в консоли")]
public class ProgressProgram
{
    private static bool notInterrupted = true;
    public static T Wait<T>(string title, Func<T> todo)
    {
        
        var GetResultTask = Task<T>.Run(() => {
            var res = todo();
            notInterrupted = false;
            return res;
        });
        Task.Run(() => {
            int n = 0;

            
            while(notInterrupted)
            {
                Console.Clear();
                Console.WriteLine(notInterrupted);
                Console.WriteLine(title);
                for (int i = 0; i < n; i++)
                    Console.Write("*");
                n++;
                if (n == 10)
                    n = 0;
                Thread.Sleep(100);
            }
            
        });
        GetResultTask.Wait();
        
        Thread.Sleep(111);

        return GetResultTask.Result;
    }
}