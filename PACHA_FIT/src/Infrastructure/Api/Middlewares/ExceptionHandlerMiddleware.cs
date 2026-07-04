using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace PACHA_FIT.Api.Middlewares;

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
            _logger.LogError(ex, "Ocurrió una excepción.");
            var httpRequestData = await context.GetHttpRequestDataAsync();

            if (httpRequestData != null)
            {
                var statusCode = ex is ValidationException || ex is JsonException
                    ? HttpStatusCode.BadRequest
                    : HttpStatusCode.InternalServerError;

                var response = httpRequestData.CreateResponse(statusCode);

                string detail = ex.Message;
                if (ex is ValidationException)
                {
                    detail = detail
                        .Replace("The", "El campo")
                        .Replace("field is required.", "es obligatorio.")
                        .Replace("is required", "es obligatorio")
                        .Replace("must be a string with a minimum length of", "debe tener una longitud mínima de")
                        .Replace("and a maximum length of", "y una máxima de");
                }
                
                var errorBody = new 
                { 
                    Message = statusCode == HttpStatusCode.BadRequest 
                        ? "Error de validación en los datos." 
                        : "Se produjo un error interno en el servidor.",
                    Details = detail
                };

                await response.WriteAsJsonAsync(errorBody);
                context.GetInvocationResult().Value = response;
            }
        }
    }
}