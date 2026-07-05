using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using PACHA_FIT.Core.Domain.User.Ports;

namespace PACHA_FIT.Infrastructure.Api.Middlewares;

public class CustomAuthenticationMiddleware(ICredentialService jwtProvider) : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var requestData = await context.GetHttpRequestDataAsync();
        
        if (requestData != null && requestData.Headers.TryGetValues("Authorization", out var values))
        {
            var token = values.FirstOrDefault()?.Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                var claimsPrincipal = jwtProvider.GetTokenClaims(token);

                if (claimsPrincipal != null)
                {
                    context.Items["User"] = claimsPrincipal;
                }
            }
        }

        await next(context);
    }
}