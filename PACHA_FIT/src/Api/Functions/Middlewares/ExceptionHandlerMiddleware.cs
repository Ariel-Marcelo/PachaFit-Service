using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace PACHA_FIT.Api.Functions.Middlewares;

public class ExceptionHandlerMiddleware: IFunctionsWorkerMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }


    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió una excepción no controlada.");
            var httpRequestData = await context.GetHttpRequestDataAsync();

            if (httpRequestData != null)
            {
                var response = httpRequestData.CreateResponse(HttpStatusCode.InternalServerError);
                
                var errorBody = new 
                { 
                    Message = "Se produjo un error interno en el servidor.",
                    Details = ex.InnerException?.Message ?? ex.Message 
                };

                await response.WriteAsJsonAsync(errorBody);
                
                // 3. Establecer la respuesta en el contexto para que Azure la envíe
                context.GetInvocationResult().Value = response;
            }
        }
    }
}