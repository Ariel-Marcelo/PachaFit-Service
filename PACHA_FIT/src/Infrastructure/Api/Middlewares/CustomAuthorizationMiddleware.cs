using System.Security.Claims;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using PACHA_FIT.Api;

namespace PACHA_FIT.Infrastructure.Api.Middlewares;

public class CustomAuthorizationMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var functionName = context.FunctionDefinition.Name;

        if (!GeneratedAuthorizationMetadata.PolicyMap.TryGetValue(functionName, out var requiredRoles))
        {
            await next(context);
            return;
        }

        var user = context.Items["User"] as ClaimsPrincipal;

        if (user == null || !user.Identity!.IsAuthenticated)
        {
            await SetUnauthorizedResponse(context);
            return;
        } 

        if (requiredRoles.Length > 0)
        {
            if (!requiredRoles.Any(role => user.IsInRole(role)))
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