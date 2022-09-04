using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Label("URL-адрес")]
public class InputUrlAttribute : BaseInputAttribute   
{
    

    public InputUrlAttribute() : base(InputTypes.Url) { }
    public InputUrlAttribute( string ErrorMessage) : base(InputTypes.Url)
    {
        this.ErrorMessage = ErrorMessage;
    }

    public override string OnGetMessage(object model, string property, object value)
    {
        if( string.IsNullOrEmpty(ErrorMessage))
        {
            return "Некорректно задан URL адрес";
            
        }
        else
        {
            return ErrorMessage;
        }
    }

    public override string OnValidate(object model, string property, object value)
    {
        if (value == null)
        {
            return GetMessage(model, property,value);
        }
        else
        {
            string textValue = value.ToString();

            if(IsValidUrl(textValue) == false)
            {
                return GetMessage(model, property, value);
            }
            else
            {
                return null;
            }            
        }        
    }

    private bool IsValidUrl(string textValue)
    {
        throw new NotImplementedException();
    }
}

