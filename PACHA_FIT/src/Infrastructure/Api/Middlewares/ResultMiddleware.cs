using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;

namespace PACHA_FIT.Api.Middlewares;

public class ResultMappingMiddleware : IFunctionsWorkerMiddleware
{
    public static HttpStatusCode ResolveStatusCode(IResult result) =>
        result.IsSuccess
            ? (HttpStatusCode)result.SuccessStatus
            : result.Error?.Code.ToHttpStatusCode() ?? HttpStatusCode.InternalServerError;

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        await next(context);

        var invocationResult = context.GetInvocationResult();

        if (invocationResult.Value is IResult result)
        {
            var request = await context.GetHttpRequestDataAsync();
            if (request != null)
            {
                var statusCode = ResolveStatusCode(result);
                var response = request.CreateResponse(statusCode);

                object responseBody = result.IsSuccess
                    ? new { data = result.Value }
                    : new
                    {
                        error = new
                        {
                            code = result.Error?.Code.ToString() ?? "Unexpected",
                            message = result.Error?.Message ?? "Ocurrió un error inesperado"
                        }
                    };

                await response.WriteAsJsonAsync(responseBody);

                invocationResult.Value = response;
            }
        }
    }
}

public static class SystemErrorExtensions
{
    public static HttpStatusCode ToHttpStatusCode(this SystemError error)
    {
        return error switch
        {
            SystemError.NotFound => HttpStatusCode.NotFound,
            SystemError.UserNotFound => HttpStatusCode.NotFound,
            SystemError.Unauthorized => HttpStatusCode.Unauthorized,
            SystemError.InvalidCredentials => HttpStatusCode.Unauthorized,
            SystemError.Validation => HttpStatusCode.BadRequest,
            SystemError.Conflict => HttpStatusCode.Conflict,
            SystemError.UserAlreadyExists => HttpStatusCode.Conflict,
            SystemError.Unexpected => HttpStatusCode.InternalServerError,
            _ => HttpStatusCode.InternalServerError
        };
    }
}