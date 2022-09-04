using System;
using System.Collections.Generic;

[Label("Массив структурированного типа")]
public class InputStructureCollectionAttribute : BaseInputAttribute
{

    public Type ItemType { get; }
    public Type ProviderType { get; }

 
    public InputStructureCollectionAttribute(string type="System.String", string typeOptionsProvider=null) : base(InputTypes.StructureCollection)
    {
        try
        {
            this.ItemType = TypeForName(type);
            if (ItemType == null)
            {
                throw new Exception($"Тип {type} элемент коллекции не найден"); ; ;
            }
        }
        catch (Exception ex)
        {
            throw new ArgumentException("type",ex);
        }
        try
        {
            this.ProviderType = TypeForName(typeOptionsProvider);
            if (ProviderType == null)
            {
                throw new Exception($"Тип {typeOptionsProvider} поставщика свойств элемент коллекции не найден");
            }
            if (IsExtendsFrom(typeof(InputStructureCollectionAttribute.IOptionsProvider))==false)
            {
                throw new Exception($"Тип {typeOptionsProvider} поставщика должен реализовать интерфейс "+nameof(InputStructureCollectionAttribute.IOptionsProvider));

            }
        }
        catch (Exception ex)
        {
            throw new ArgumentException("type", ex);
        }
    }

    private bool IsExtendsFrom(Type type)
    {
        throw new NotImplementedException();
    }

    private Type TypeForName(string type)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Предоставляет объекты 
    /// </summary>
    public interface IOptionsProvider
    {
        public IEnumerable<object> Get();
    }

    public override string OnValidate(object model, string property, object value)
    {
        throw new NotImplementedException();
    }

    public override string OnGetMessage(object model, string property, object value)
    {
        throw new NotImplementedException();
    }
}