using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static InputApplicationProgram;


/// <summary>
/// Необходимо чтобы объект был Singleton
/// </summary>
public class AppProviderService: IServiceProvider   
{      
    private static AppProviderService _instance = null;
    public class ServiceAttribute : Attribute { }
    public static AppProviderService GetInstance()
    {
        if (_instance == null)
        {
            lock(_instance = new AppProviderService())
            {
                _instance.AddSingletons(Assembly.GetExecutingAssembly().GetClassTypes());
            }
        }
        return _instance;
    }

   
    public static AppProviderService UpgradeInstance(AppProviderService next) 
        => _instance;


    /// <summary>
    /// Функции внедрения зависимостей
    /// </summary>
    protected ConcurrentDictionary<Type, Func<IServiceProvider, object>> Factories =
        new ConcurrentDictionary<Type, Func<IServiceProvider, object>>();

    /// <summary>
    /// 
    /// </summary>
    [Service] 
    public List<Type> serviceModuleTypes = new List<Type>();

    /// <param name="serviceProvider">Исп. в случае если сервис не зарегистрирован</param>
    /// <param name="names">Имена типов сервисов</param>
    public AppProviderService(

        
            IServiceProvider serviceProvider = null, 
            IEnumerable<string> names = null)  
    {
    }

    /// <summary>
    /// 
    /// </summary>    
    public void Init(Type[] serviceModuleTypes = null)
    {
        this.serviceModuleTypes.AddRange(serviceModuleTypes == null ?
            Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass == true && ("" + t.Name[0]).IsEng()).ToArray() :
            serviceModuleTypes);
    }










    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> GetNames() =>
        this.serviceModuleTypes.Select(t => t.Name);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public virtual bool HasService(Type type) => 
        Factories.ContainsKey(type);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    public virtual object GetService(Type serviceType)
        => Factories.ContainsKey(serviceType) ?
            Factories[serviceType].Invoke(this) :
            throw new ArgumentException("serviceType");

    

    
 

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Get<T>() where T : class
    {
        this.Info("Внедрение сервиса "+typeof(T).GetTypeName());
        var type = Assembly.GetExecutingAssembly().GetTypes().Where(t => t == typeof(T) || t.GetInterfaces().Contains(typeof(T))).FirstOrDefault();
        if (type != null)
        {
            try
            {
                var constructor = type.GetConstructors().FirstOrDefault();
                var argsList = new List<object>();
                if(constructor == null)
                {
                    throw new Exception(type.GetTypeName() + " не реаклизует конструктор");
                }
                else
                {
                    foreach (Type ptype in constructor.GetParameters().Select(p => p.ParameterType).ToList())
                    {
                        this.Info("Получение зависимости " + ptype.GetTypeName());
                        if (ptype.IsInterface)
                        {
                            if (ptype.GetTypeName().Equals(nameof(IServiceProvider)))
                            {
                                argsList.Add(this);
                            }
                            else
                            {
                                object pobj = this.Factories.Where(kv => kv.Key.IsImplements(ptype)).FirstOrDefault().Value(this);
                                argsList.Add(pobj);
                            }
                        }
                        else
                        {
                            object pobj = GetService(ptype);
                            argsList.Add(pobj);
                        }


                    }
                    var newInstance = constructor.Invoke(argsList.ToArray());
                    return (T)newInstance;
                }
             
            }
            catch(Exception ex)
            {
                throw new Exception("Не удалось получить ссылку на сервис типа " + typeof(T).GetTypeName(),ex);
            }
        }
        return null;
    }

    //
    public void AddSingletons(IEnumerable<Type> enumerable)
    {

        foreach (Type ptype in enumerable)
        {
            //this.Info("Приступаю к регистрации сервиса " + ptype.GetTypeName());
            this.Factories[ptype] = (sp) => {
                try
                {
                    var constructor = ptype.GetConstructors().FirstOrDefault();
                    var argsList = new List<object>();
                    foreach (Type ptype in constructor.GetParameters().Select(p => p.ParameterType).ToList())
                    {
                        this.Info(ptype.GetTypeName());
                        object pobj = GetService(ptype);
                        argsList.Add(pobj);

                    }
                    var newInstance = constructor.Invoke(argsList.ToArray());
                    return newInstance;
                }
                catch (Exception ex)
                {
                    throw new Exception("Не удалось получить ссылку на сервис типа " + ptype.GetTypeName(), ex);
                }
            };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disc"></param>
    public void Add(IEnumerable<ServiceDescriptor> disc)
    {

        disc.ToList().ForEach(d =>
        {
            Factories[d.ServiceType] = d.ImplementationFactory;
        });
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="filepath"></param>
    public void LoadFile(string filepath)
    {
        try
        {
            var types = Assembly.LoadFile(filepath).GetClassTypes();
            serviceModuleTypes.AddRange(types);
            this.Info(new
            {
                FilePath=filepath,
                ClassTypes=types
            });
        }
        catch (Exception ex)
        {
            this.Error(ex, "Не удалось поджключить сборку из файла: " + filepath);
        }
    }


    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
    public IServiceProvider AddProvider(IServiceProvider provider)
    {
        return this;
    }


    /// <summary>
    /// 
    /// </summary>    
    public virtual void AddServiceDescriptions(ServiceDescriptor[] descriptors)
    {
        foreach (var descriptor in descriptors)
        {
            var serviceType = descriptor.ServiceType;
            this.Factories[descriptor.ServiceType] = (sp) => {
                return sp.GetService(descriptor.ServiceType);
            };
        }
    }

    public void Log()    
        => this.Info(this.GetNames().ToJsonOnScreen());         
}


/// <summary>
/// 
/// </summary>
public static class IServiceProviderExtrensions
{
    public static T Get<T>(this IServiceProvider sp)
    {

        return sp.GetService<T>();
    }
}