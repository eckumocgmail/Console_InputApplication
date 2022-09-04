 

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml;

public interface ITypeNode<T> : TreeWalker<T>
{
    ConcurrentDictionary<string, ITypeNode<T>> HierElements { get; set; }
    T Item { get; set; }
    string Name { get; set; }
    ITypeNode<T> Parent { get; set; }

    ITypeNode<T> Append(ITypeNode<T> pchild);
    string GetIntPath();
    int GetLevel();
    List<string> GetPath();
    string GetPath(string separator);
    bool Has(string name);
    bool Remove(string name);

    IList<ITypeNode<T>> GetChildren();
}


public interface TreeWalker<T>
{
    public void GoToParent(Action<ITypeNode<T>> handle);
    public void GoByLevels(Action<ITypeNode<T>> handle);
    public void GoFromChildren(Action<ITypeNode<T>> handle);
    public void GoToChildren(Action<ITypeNode<T>> handle);
}

/// <summary>
/// Иерархическая структура данных
/// </summary>
/// <typeparam name="T"></typeparam>
public class TypeNode<T> : ITypeNode<T> where T: class
{
    /// <summary>
    /// Уникальное имя обьекта в родительском контексте
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Элемент
    /// </summary>
    public T Item { get; set; }

    /// <summary>
    /// Дочерние элементы
    /// </summary>
    public IDictionary<string, ITypeNode<T>> HierElements { get; set; }

    public TypeNode()
    {
        this.Item = null;// (T)typeof(T).GetConstructors().First(c => c.GetParameters().Count() == 0).Invoke(new object[0]);
        this.Name = typeof(T).GetLabel();
        this.Parent = null;
    }
    public TypeNode(ITypeNode<T> parent)
    {
        this.Item = null;// (T)typeof(T).GetConstructors().First(c => c.GetParameters().Count() == 0).Invoke(new object[0]);
        this.Name = typeof(T).GetLabel();
        this.Parent = parent;
    }

    public void ForEach<TNode>(Action<TNode> todo)
    {

    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="name"></param>
    /// <param name="item"></param>
    /// <param name="parent"></param>
    public TypeNode(string name, T item, ITypeNode<T> parent)
    {
        if (name == null)
        {
            throw new ArgumentNullException("name");
        }
        if (item == null)
        {
            throw new ArgumentNullException("item");
        }
        Name = name;
        Item = item;
        Parent = parent;
        HierElements = new SortedDictionary<string, ITypeNode<T>>();
    }


    /// <summary>
    /// Удаление дочернего элемента
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool Remove(string name)
    {
        ITypeNode<T> output;
        return HierElements.Remove(name, out output);
    }


    /// <summary>
    /// Проверка наличия потомка с заданным именем
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool Has(string name)
    {
        return HierElements.ContainsKey(name);
    }


    /// <summary>
    /// Добавление потомка
    /// </summary>
    /// <param name="pchild"></param>
    /// <returns></returns>
    public ITypeNode<T> Append(ITypeNode<T> pchild)
    {
        if (pchild == null)
        {
            throw new ArgumentNullException("pchild");
        }
        if (Has(pchild.Name))
        {
            throw new Exception($"Обьект с именем {pchild.Name} уже зарегистрирован в узле: {GetPath()}");
        }
        else
        {

            return HierElements[pchild.Name] = pchild;
        }
    }


    /// <summary>
    /// Ссылка на родительский элемент
    /// </summary>
    /// 
    [JsonIgnore]
    private ITypeNode<T> _Parent { get; set; }

    public IList<ITypeNode<T>> GetChildren()
        => new List<ITypeNode<T>>(this.HierElements.Values);

    /// <summary>
    /// Перемещение узла
    /// </summary>
    [JsonIgnore]
    public ITypeNode<T> Parent
    {
        get
        {
            return _Parent;
        }
        set
        {
            if (_Parent != null)
            {
                _Parent.Remove(Name);
            }
            if (value != null)
            {
                _Parent = value;
                _Parent.Append(this);
            }

        }
    }


    /// <summary>
    /// Получение глубины иерархии
    /// </summary>
    /// <returns></returns>
    public int GetLevel()
    {
        int level = 1;
        ITypeNode<T> p = this;
        while (p.Parent != null)
        {
            p = p.Parent;
            level++;
        }
        return level;
    }

    /// <summary>
    /// Получение пути от истока
    /// </summary>
    /// <returns></returns>
    public List<string> GetPath()
    {
        
        if (Parent != null)
        {
            List<string> path = Parent.GetPath();
            path.Add(Name);

            path.ForEach(id => Console.Write($"/{id}")); Console.WriteLine("");
            return path;
        }
        return new List<string> { Name };
    }

   


    /// <summary>
    /// Получение абсолюного идентификатора
    /// </summary>
    /// <param name="separator">разделитель</param>
    /// <returns></returns>
    public string GetPath(string separator)
    {
        string path = "";
        foreach (string name in GetPath())
        {
            if (path.Length != 0)
            {
                path += separator + name;
            }
            else
            {
                path = name;
            }
        }
        return path;
    }


    /// <summary>
    /// Обработка узлов поддерева вертикально вниз
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="handle"></param>
    public void GoToChildren(Action<ITypeNode<T>> handle)
    {
        handle(this);
        foreach (ITypeNode<T> pchild in HierElements.Values)
        {
            pchild.GoToChildren(handle);
        }
    }


    /// <summary>
    /// Обработка узлов поддерева снизу вверх
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="handle"></param>
    public void GoFromChildren(Action<ITypeNode<T>> handle)
    {
        foreach (ITypeNode<T> pchild in HierElements.Values)
        {
            pchild.GoFromChildren(handle);
        }
        handle(this);
    }


    /// <summary>
    /// Обход всей иерархии
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="handle"></param>
    public void GoToParent(Action<ITypeNode<T>> handle) 
    {
        handle((ITypeNode<T>)this);
        if (Parent != null)
        {
            Parent.GoToParent(handle);
        }
    }


    /// <summary>
    /// Обработка узлов поддерева сверху вниз
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="handle"></param>
    public void GoByLevels(Action<ITypeNode<T>> handle) 
    {
        if (Parent == null)
        {
            handle((ITypeNode<T>)this);
        }
        foreach (ITypeNode<T> pchild in HierElements.Values)
        {
            handle((ITypeNode<T>)pchild);
        }
        foreach (ITypeNode<T> pchild in HierElements.Values)
        {
            pchild.GoByLevels(handle);
        }
    }


    public void ForEach(Action<object> todo) 
    {

    }

    ConcurrentDictionary<string, ITypeNode<T>> ITypeNode<T>.HierElements { get; set; }

    public string GetIntPath()
    {
        if (this.Parent != null)
        {
            string path = this.Parent.GetIntPath() + "." + (Parent.GetChildren().IndexOf(this)+1);
            Console.WriteLine($"{Name} => {SerializeObject(GetPath(),Formatting.Indented)} => {path}");

            Console.Write("\n");
            for (int i=0; i<GetLevel(); i++)
                Console.Write("  ");
            

            return path;

        }
        else
        {
            return "1";
        }
    }

    private object SerializeObject(List<string> list, Formatting indented)
    {
        throw new NotImplementedException();
    }
}
