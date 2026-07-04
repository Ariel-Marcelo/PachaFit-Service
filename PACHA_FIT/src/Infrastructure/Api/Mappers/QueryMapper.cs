using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace PACHA_FIT.Api.Mappers;

public static class QueryMapper
{
    public static T Map<T>(HttpRequest req) where T : new()
    {
        var obj = new T();
        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in props)
        {
            // Buscamos si la Key existe en la Query (ignorando mayúsculas/minúsculas)
            if (req.Query.TryGetValue(prop.Name, out var value))
            {
                try 
                {
                    // Obtenemos el tipo real (manejando Nullables como int?)
                    Type targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    
                    // Convertimos el valor del string de la query al tipo de la propiedad
                    var convertedValue = Convert.ChangeType(value.ToString(), targetType);
                    
                    prop.SetValue(obj, convertedValue);
                }
                catch
                {
                    // Si falla la conversión (ej: mandan "abc" en un int), lo ignoramos
                    continue; 
                }
            }
        }
        return obj;
    }
}   