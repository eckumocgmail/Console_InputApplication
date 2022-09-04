using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

 
/// <summary>
/// Методы упрощают инициаллизацию объектов
/// </summary>
public class FactoryUtils 
{
    private static FactoryUtils Instance = null;
    public static FactoryUtils Get()
    {
        
        if (Instance == null)
        {
            Instance = new FactoryUtils();
        }
        
        Instance.AddTypes(typeof(FactoryUtils).Assembly);
        Instance.AddTypes(typeof(DataColumnCollection).Assembly);
        Instance.AddTypes(typeof(System.String).Assembly);
        Instance.AddTypes(typeof(System.Data.DataTable).Assembly);
        Instance.AddTypes(typeof(System.Data.DataSet).Assembly);
        Instance.AddTypes(Assembly.GetExecutingAssembly());
        Instance.AddTypes(Assembly.GetCallingAssembly());
        Instance.AddTypes(Assembly.GetEntryAssembly());
        return Instance;
    }


    private ConcurrentDictionary<string, Type> ByShortNames =
        new ConcurrentDictionary<string, Type>() {};
    private ConcurrentDictionary<string, Type> ByFullNames =
        new ConcurrentDictionary<string, Type>();

    public void AddType(string name, Type type)
    {
        ByShortNames[name] = type;
    }

    private ConcurrentDictionary<string, Type> ViewComponents =
        new ConcurrentDictionary<string, Type>();
    private ConcurrentDictionary<string, Type> Controllers =
        new ConcurrentDictionary<string, Type>();
    private ConcurrentDictionary<string, Type> DataModels =
       new ConcurrentDictionary<string, Type>();
   

    private FactoryUtils():base()
    {

        ByFullNames["Nulalble<System.DateTime>"] = typeof(DateTime?);
        ByShortNames["Nulalble<System.DateTime>"] = typeof(DateTime?);
        ByFullNames["Nulalble<System.Int32>"] = typeof(int?);
        ByFullNames["Nulalble<System.Int64>"] = typeof(long?);
        ByShortNames["Nulalble<System.Int32>"] = typeof(int?);
        ByShortNames["Nulalble<System.Int64>"] = typeof(long?);
        ByShortNames["Nulalble<DateTime>"] = typeof(DateTime?);
        ByShortNames["Single"] = typeof(Single);
        ByShortNames["Double"] = typeof(Double);
        ByShortNames["Object"] = typeof(Object);
        ByShortNames["DateTime"] = typeof(DateTime);

        ByShortNames["Byte[]"] = typeof(byte[]);
        ByShortNames["byte[]"] = typeof(byte[]);
        ByShortNames["String"] = typeof(System.String);
        ByShortNames["Int32"] = typeof(System.Int32);
        ByShortNames["Int64"] = typeof(System.Int64);
        ByShortNames["string"] = typeof(System.String);
        ByShortNames["int"] = typeof(System.Int32);
        ByShortNames["float"] = typeof(System.Decimal);
        ByShortNames["byte"] = typeof(System.Byte);
        ByShortNames["long"] = typeof(System.Int64);
    }
    /*
    public IMvcBuilder AddParts(IMvcBuilder mvcBuilder)
    {
        
        Assemblies.ToList().ForEach(assembly =>
        {
            mvcBuilder = mvcBuilder.AddApplicationPart(assembly);
        });
        return mvcBuilder;
    }*/


    public IEnumerable<string> GetNames() => this.ByShortNames.Keys.ToList();


    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void AddDataModel(Type context)
    {
        DataModels[context.Name] = context;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ConcurrentDictionary<string, Type> GetDataModels()
    {
        return DataModels;
    }

    /// <summary>
    /// Компоненты представлений
    /// </summary>
    /// <returns></returns>
    public ConcurrentDictionary<string, Type> GetControllers()
    {
        return Controllers;
    }


    /// <summary>
    /// Компоненты представлений
    /// </summary>
    /// <returns></returns>
    public ConcurrentDictionary<string, Type> GetViewComponents()
    {
        return ViewComponents;
    }


    /// <summary>
    /// Регистрация типов объявленных в сборке
    /// </summary>    
    /// <param name="assembly">сборка</param>
    public HashSet<Assembly> Assemblies = new HashSet<Assembly>();
    public void AddTypes(Assembly assembly)
    {
        if (Assemblies.Contains(assembly) == true)
        {
            return;
        }
        Assemblies.Add(assembly);
        foreach (var ptype in assembly.GetTypes())
        {
            if (ptype.Name.EndsWith("ViewComponent"))
            {
                ViewComponents[ptype.Name] = ptype;
            }
            if (ptype.Name.EndsWith("Controller") && ptype.IsAbstract==false)
            {
                Controllers[ptype.Name] = ptype;
            }
            if (ptype.Name.Contains("`") ==false)
            {
                ByShortNames[ptype.Name] = ptype;
                ByFullNames[ptype.FullName] = ptype;
            }
            
            else if(ptype.Name.StartsWith("<")==false) 
            {
                string typeName = ParsePropertyType(ptype);
                 
                ByShortNames[typeName] = ptype;
                ByFullNames[ptype.Namespace+"."+ typeName] = ptype;
            }
            
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string ParsePropertyType(Type propertyType)
    {
        string name = propertyType.Name;
        if (name.Contains("`"))
        {
            string text = propertyType.AssemblyQualifiedName;
            text = text.Substring(text.IndexOf("[[") + 1);
            text = text.Substring(0, text.IndexOf(","));
            name = name.Substring(0, name.IndexOf("`")) + "<" + text.Replace("`1", "").Replace("`2", "") + ">";

        }
        return name;
    }

    /// <summary>
    /// Регистрация типов объявленных в сборке
    /// </summary>
    /// <param name="assembly">сборка</param>
    public void AddTypes(Assembly[] assembly)
    {
        foreach (var ptype in assembly)
        {
            AddTypes(ptype);
        }
    }

    /// <summary>
    /// Создание новоги экземпляра класса конструктором по-умолчанию
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public T CreateWithDefaultConstructor<T>(string typeName)
    {
        Type type = null;
        if (typeName.Contains("."))
        {
            type = TypeForName(typeName);
        }
        else
        {
            type = TypeForShortName(typeName);
        }
        return CreateWithDefaultConstructor<T>(type);
    }


    /// <summary>
    /// Поиск конструктора по-умолчанию
    /// </summary>
    /// <param name="type">Ссылка на тип</param>
    /// <returns>конструктор</returns>
    public static ConstructorInfo GetDefaultConstructor(Type type)
    {
        return (from c in new List<ConstructorInfo>(type.GetConstructors()) where c.GetParameters().Length == 0 select c).FirstOrDefault();
    }


    /// <summary>
    /// Создание новоги экземпляра класса конструктором по-умолчанию
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public T CreateWithDefaultConstructor<T>(Type type)
    {
        ConstructorInfo constructor = GetDefaultConstructor(type);
        if (constructor == null)
        {
            throw new Exception($"Тип {type.Name} не обьявляет контруктор по-умолчанию");
        }         
        return (T)constructor.Invoke(new object[0]);
    }


    /// <summary>
    /// Возвращает тип по имени
    /// </summary>
    /// <param name="type">имя типа</param>
    /// <returns>ссылка на тип</returns>
    public Type TypeForName(string type)
    {
        if (type.IndexOf("List") == 0 || type.IndexOf("List") == 1)
        {
            return typeof(List<object>);
        }
        if (type.IndexOf(".") == -1)
        {
            return TypeForShortName(type);
        }
        if (type == "Single")
        {
            return typeof(Single);
        }
        if (type == "Nullable<System.DateTime>")
        {
            return typeof(Nullable<DateTime>);
        }
        if (type == "Nullable<System.Single>")
        {
            return typeof(Nullable<float>);
        }
        if (type == "Nullable<System.Int32>")
        {
            return typeof(Nullable<int>);
        }
        if (type == "Nullable<System.Int32>")
        {
            return typeof(Nullable<int>);
        }
        return ByFullNames[type];
    }


    /// <summary>
    /// Возвращает тип по имени
    /// </summary>
    /// <param name="type">имя типа</param>
    /// <returns>ссылка на тип</returns>
    public Type TypeForShortName(string type)
    {
        if (type.IndexOf("List") == 0|| type.IndexOf("List") == 1)
        {
            return typeof(List<object>);
        }
        if (type.IndexOf(".") != -1)
        {
            return TypeForName(type);
        }
        if (ByShortNames.ContainsKey(type)) 
        {
            return ByShortNames[type];
        }
        else
        {
            return null;
        }
        
    }
}
 
