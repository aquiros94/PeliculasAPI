using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace PeliculasAPI.Utilidades
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valor = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valor == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            try
            {
                bindingContext.Result = ModelBindingResult.Success(JsonConvert.DeserializeObject<T>(valor.FirstValue));
            }
            catch
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "El valor dado no es del tipo adecuado");
            }

            return Task.CompletedTask;
        }
    }
}
