using System.Web.Mvc;
using AzureLogsViewer.Web.Code.Json;
using Newtonsoft.Json.Linq;

namespace AzureLogsViewer.Web.Code
{
    /// <summary>
    /// Default model binder that aware of json conversion.
    /// </summary>
    public class AlvDefaultModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueResult != null)
            {
                var jsonValue = valueResult.RawValue as JToken;
                if (jsonValue != null)
                {
                    return JsonConversion.Deserialize(jsonValue, bindingContext.ModelType);
                }
            }

            // falling back to default binding
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}