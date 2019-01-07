using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
/// <summary>
/// Класс для предотвращения ошибок конвертации даты
/// </summary>
public class DateTimeBinder : IModelBinder
{
    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
        var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        if (value == null) return null;
        try
        {
            var date = value.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);
            return date;
        }
        catch (Exception)
        {
            var date = value.ConvertTo(typeof(DateTime), CultureInfo.InvariantCulture);
            return date;
        }
    }
}
