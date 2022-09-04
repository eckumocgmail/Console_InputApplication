using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


public static class DbContextExtensions
{
    /// <summary>
    /// Нехороший способ извеления наименований сущностей
    /// </summary>
    /// <param name="dbContext"> контекст данных </param>
    /// <returns> множество наименований сущностей </returns>
    public static HashSet<Type> GetEntitiesTypes(this object dbContext)
    {
        Type type = dbContext.GetType();
        HashSet<Type> entities = new HashSet<Type>();
        foreach (MethodInfo info in type.GetMethods())
        {
            if (info.Name.StartsWith("get_") == true && info.ReturnType.Name.StartsWith("ISuperSer"))
            {
                if (info.Name.IndexOf("MigrationHistory") == -1)
                {
                    entities.Add(info.ReturnType);
                }
            }
        }
        return entities;
    }
    public static dynamic GetDbSet(this DbContext dbContext, string entity)
    {
        Type type = dbContext.GetType();
        HashSet<Type> entities = new HashSet<Type>();
        foreach (MethodInfo info in type.GetMethods())
        {
            if (info.Name.StartsWith("get_") == true && info.ReturnType.Name.StartsWith("ISuperSer"))
            {
                if (info.Name.IndexOf("MigrationHistory") == -1)
                {
                    return info.Invoke(dbContext, new object[0]);
                }
            }
        }
        return entities;
    }
}
    
