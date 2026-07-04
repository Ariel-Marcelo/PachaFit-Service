using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using PACHA_FIT.Core.Domain.User.Ports;

namespace PACHA_FIT.Api.Middlewares;

public class CustomAuthenticationMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ICredentialService _jwtProvider; // Tu servicio que valida tokens

    public CustomAuthenticationMiddleware(ICredentialService jwtProvider)
    {
        _jwtProvider = jwtProvider;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        // 1. Obtener los datos de la petición HTTP
        var requestData = await context.GetHttpRequestDataAsync();
        
        // 2. Buscar el Header "Authorization: Bearer <token>"
        if (requestData != null && requestData.Headers.TryGetValues("Authorization", out var values))
        {
            var token = values.FirstOrDefault()?.Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                // 3. VALIDACIÓN: Aquí es donde "Cargamos al usuario"
                var claimsPrincipal = _jwtProvider.ValidateToken(token);

                if (claimsPrincipal != null)
                {
                    // 4. EL PASO CLAVE: Guardamos al usuario en el contexto
                    // Esta es la "carga previa" que tu otro middleware está buscando
                    context.Items["User"] = claimsPrincipal;
                }
            }
        }

        await next(context);
    }
}