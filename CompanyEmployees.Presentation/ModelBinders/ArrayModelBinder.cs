using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CompanyEmployees.Presentation.ModelBinders;
public class ArrayModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (!bindingContext.ModelMetadata.IsEnumerableType)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }
        //It retrieves the value provided for the model from the value
        // provider (e.g., query string, form data) and converts it to a string
        var providedValue = bindingContext.ValueProvider
        .GetValue(bindingContext.ModelName)
        .ToString();
        if (string.IsNullOrEmpty(providedValue))
        {
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }
        //Convert Provided Value to Array
        var genericType =
       bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
        var converter = TypeDescriptor.GetConverter(genericType);
        var objectArray = providedValue.Split(new[] { "," },
       StringSplitOptions.RemoveEmptyEntries)
        .Select(x => converter.ConvertFromString(x.Trim()))
        .ToArray();
        var guidArray = Array.CreateInstance(genericType, objectArray.Length);
        objectArray.CopyTo(guidArray, 0);
        bindingContext.Model = guidArray;
        bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
        return Task.CompletedTask;
    }
}