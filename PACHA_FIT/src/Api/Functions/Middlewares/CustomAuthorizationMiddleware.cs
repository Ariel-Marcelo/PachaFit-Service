using System.Security.Claims;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using PACHA_FIT.Api.Shared;

namespace PACHA_FIT.Api.Functions.Middlewares;

public class CustomAuthorizationMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        // 1. Obtener el atributo de la función
        var targetMethod = context.GetTargetFunctionMethod();
        var authAttribute = targetMethod.GetCustomAttributes(typeof(PachaAuthorizeAttribute), true)
            .FirstOrDefault() as PachaAuthorizeAttribute;

        if (authAttribute == null)
        {
            await next(context); // No requiere autorización
            return;
        }

        // 2. Obtener el usuario autenticado (previamente cargado por el CustomAuthenticationMiddleware)
        var user = context.Items["User"] as ClaimsPrincipal;

        if (user == null || !user.Identity!.IsAuthenticated)
        {
            await SetUnauthorizedResponse(context);
            return;
        } 

        // 3. Validar Roles
        if (!string.IsNullOrEmpty(authAttribute.Roles))
        {
            var roles = authAttribute.Roles.Split(',');
            if (!roles.Any(role => user.IsInRole(role.Trim())))
            {
                await SetForbiddenResponse(context);
                return;
            }
        }

        await next(context);
    }
    
    private async Task SetUnauthorizedResponse(FunctionContext context)
    {
        var request = await context.GetHttpRequestDataAsync();
        if (request != null)
        {
            var response = request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
            await response.WriteAsJsonAsync(new { Message = "No autenticado. Token faltante o inválido." });
        
            // Esto le dice a Azure que no ejecute la función y use esta respuesta
            context.GetInvocationResult().Value = response;
        }
    }

    private async Task SetForbiddenResponse(FunctionContext context)
    {
        var request = await context.GetHttpRequestDataAsync();
        if (request != null)
        {
            var response = request.CreateResponse(System.Net.HttpStatusCode.Forbidden);
            await response.WriteAsJsonAsync(new { Message = "No tienes los permisos necesarios (Rol insuficiente)." });
        
            context.GetInvocationResult().Value = response;
        }
    }
}