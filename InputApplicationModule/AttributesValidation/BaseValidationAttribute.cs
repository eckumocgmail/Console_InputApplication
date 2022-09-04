using NetCoreConstructorAngular.Data.DataAttributes;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 
public abstract class BaseValidationAttribute: ValidationAttribute, MyValidation  
{
    public BaseValidationAttribute():base() {
       
    }
    public abstract string Validate(object model, string property, object value);
    public abstract string GetMessage(object model, string property, object value);
    protected  override ValidationResult IsValid(object value, ValidationContext validationContext)
    {            
        string result = this.Validate(validationContext.ObjectInstance, validationContext.MemberName, value);
        if (result == null)
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult(result);
        }

    }
}
 
