using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;

namespace PACHA_FIT.Api.Middlewares;

public class ResultMappingMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        await next(context);

        var invocationResult = context.GetInvocationResult();
        
        if (invocationResult.Value is IResult result)
        {
            var request = await context.GetHttpRequestDataAsync();
            if (request != null)
            {
                var response = request.CreateResponse((HttpStatusCode)result.StatusCode);
                object responseBody = result.IsSuccess 
                    ? new { data = result.GetValue() } 
                    : new { error = result.Error ?? "Ocurrió un error inesperado" };
            
                await response.WriteAsJsonAsync(responseBody);
                
                invocationResult.Value = response;
            }
        }
    }
}