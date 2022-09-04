using System;

[Label("Массив простых значений")]
public class InputPrimitiveCollectionAttribute : BaseInputAttribute
{


    public InputPrimitiveCollectionAttribute(): base(InputTypes.PrimitiveCollection){
        
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