using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using PACHA_FIT.Api.Shared;

namespace PACHA_FIT.Api.Functions.Middlewares;

public class ResultMappingMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        await next(context);

        var invocationResult = context.GetInvocationResult();
        
        if (invocationResult.Value is Result<object> result)
        {
            var request = await context.GetHttpRequestDataAsync();
            if (request != null)
            {
                var response = request.CreateResponse((HttpStatusCode)result.StatusCode);
                await response.WriteAsJsonAsync(result.IsSuccess ? (object)result.Value! : new { result.Error });
                invocationResult.Value = response;
            }
        }
    }
}