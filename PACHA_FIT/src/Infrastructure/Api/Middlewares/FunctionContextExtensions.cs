using System.Reflection;
using Microsoft.Azure.Functions.Worker;

namespace PACHA_FIT.Api.Middlewares;

public static class FunctionContextExtensions
{
    public static MethodInfo GetTargetFunctionMethod(this FunctionContext context)
    {
        // PACHA_FIT.API.FUNTION.NOMBREFUNCION
        var entryPoint = context.FunctionDefinition.EntryPoint;
        
        // C:\ruta\al\ensamblado\PACHA_FIT.API.FUNCTIONS.dll
        var assemblyPath = context.FunctionDefinition.PathToAssembly;
        
        // Cargar el ensamblado
        var assembly = Assembly.LoadFrom(assemblyPath);
        
        // PACHA_FIT.Api.FUNCTION
        var typeName = entryPoint.Substring(0, entryPoint.LastIndexOf('.'));
        // NOMBREFUNCION
        var methodName = entryPoint.Substring(entryPoint.LastIndexOf('.') + 1);
        
        // Obtener el tipo y luego el método
        var type = assembly.GetType(typeName);
        return type?.GetMethod(methodName) ?? throw new InvalidOperationException("No se pudo encontrar el método de la función.");
    }
}