using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

 

/// <summary>
/// Коллекция сетевых сервисов
/// </summary>
//[EntityLabel("Модель приложения")]
public class MyAreaModel : Dictionary<string, MyControllerModel>
{
    public MyAreaModel()
    {
        var controllers = GetControllers(Assembly.GetExecutingAssembly());
        if (controllers == null || controllers.Count == 0)
        {
            Console.WriteLine("Контроллеры не найдены в приложении");
        }
        foreach (Type controllerType in controllers)
        {
            if (controllerType.IsAbstract) continue;
            var model = CreateModel(controllerType);
            this[controllerType.Name] = model;
        }
    }

    private List<Type> GetControllers(Assembly assembly)
    {
        return assembly.GetControllers().ToList();
    }

    public static string RoleFor(Type type)
    {
        var attrs = ForType(type);
        if (attrs.ContainsKey("ForRoleAttribute"))
        {
            return attrs["ForRoleAttribute"];
        }
        else
        {
            return null;
        }
    }

    private static IDictionary<string, string> ForType(Type type)
    {
        throw new NotImplementedException();
    }

    private string PathForController(Type controllerType)
    {
        string role = RoleFor(controllerType);
        if (role != null)
        {
            return "/" + role + "/" + controllerType.Name.Replace("Controller", "");
        }
        else
        {
            return "/" + controllerType.Name.Replace("Controller", "");
        }
    }
    /// <summary>
    /// <button>ok</button>
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<MethodInfo> GetOwnPublicMethods(Type type)
    {
        return (from m in new List<MethodInfo>(type.GetMethods())
                where m.IsPublic &&
                        !m.IsStatic &&
                        m.DeclaringType.FullName == type.FullName
                select m).ToList<MethodInfo>();
    }
    public MyControllerModel CreateModel(Type controllerType)
    {
        var uri = "/";
        var attrs = ForType(controllerType);
        if (attrs.ContainsKey("AreaAttribute")) uri += attrs["AreaAttribute"].ToString() + "/";
        if (attrs.ContainsKey("ForRoleAttribute")) uri += attrs["ForRoleAttribute"].ToString() + "/";
        string path = PathForController(controllerType);
        ToConsole(path);
        MyControllerModel model = new MyControllerModel()
        {
            Name = controllerType.Name.Replace("`1", ""),
            Path = path,
            Actions = new Dictionary<string, MyActionModel>()
        };



        while (controllerType != null)
        {
            foreach (MethodInfo method in GetOwnPublicMethods(controllerType))
            {
                if (method.IsPublic && method.Name.StartsWith("get_") == false && method.Name.StartsWith("set_") == false)
                {

                    Dictionary<string, string> attributes = ForMethod(controllerType, method.Name);
                    Dictionary<string, object> pars = new Dictionary<string, object>();
                    model.Actions[method.Name] = new MyActionModel()
                    {
                        Name = method.Name,
                        Attributes = attributes,
                        Method = ParseHttpMethod(attributes),
                        Parameters = new Dictionary<string, MyParameterDeclarationModel>(),
                        Path = model.Path + "/" + method.Name
                    };
                    foreach (ParameterInfo par in method.GetParameters())
                    {
                        model.Actions[method.Name].Parameters[par.Name] = new MyParameterDeclarationModel()
                        {
                            Name = par.Name,
                            Type = par.ParameterType.Name,
                            IsOptional = par.IsOptional
                        };
                    }
                }
            }
            controllerType = controllerType.BaseType;
        }
        return model;
    }

    private Dictionary<string, string> ForMethod(Type controllerType, string name)
    {
        throw new NotImplementedException();
    }

    private string ParseHttpMethod(Dictionary<string, string> attributes)
    {
        throw new NotImplementedException();
    }

    private void ToConsole(string path)
    {
        throw new NotImplementedException();
    }
}

