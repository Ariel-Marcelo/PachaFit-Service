using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PACHA_FIT.Api.Functions;

public class SwaggerFunc
{
    private readonly ILogger<SwaggerFunc> _logger;

    public SwaggerFunc(ILogger<SwaggerFunc> logger)
    {
        _logger = logger;
    }

    [Function("SwaggerSpec")]
    public IActionResult GetSwaggerSpec(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/spec")] HttpRequest req)
    {
        // En el modelo aislado, los archivos se copian al directorio base de la aplicación
        var path = Path.Combine(AppContext.BaseDirectory, "docs", "openapi.yaml");
        
        _logger.LogInformation("Intentando leer el archivo OpenAPI desde: {path}", path);

        if (!File.Exists(path))
        {
            _logger.LogError("No se encontró el archivo en: {path}", path);
            return new NotFoundObjectResult($"No se encontró el archivo OpenAPI en {path}");
        }

        var content = File.ReadAllText(path);
        return new ContentResult
        {
            Content = content,
            ContentType = "text/yaml",
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [Function("SwaggerUi")]
    public IActionResult GetSwaggerUi(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/ui")] HttpRequest req)
    {
        var html = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"">
  <title>PachaFit API - Swagger UI</title>
  <link rel=""stylesheet"" type=""text/css"" href=""https://unpkg.com/swagger-ui-dist@5/swagger-ui.css"" >
  <style>
    html { box-sizing: border-box; overflow: -moz-scrollbars-vertical; overflow-y: scroll; }
    *, *:before, *:after { box-sizing: inherit; }
    body { margin:0; background: #fafafa; }
  </style>
</head>
<body>
  <div id=""swagger-ui""></div>
  <script src=""https://unpkg.com/swagger-ui-dist@5/swagger-ui-bundle.js""></script>
  <script src=""https://unpkg.com/swagger-ui-dist@5/swagger-ui-standalone-preset.js""></script>
  <script>
    window.onload = function() {
      const ui = SwaggerUIBundle({
        url: ""/api/swagger/spec"",
        dom_id: '#swagger-ui',
        deepLinking: true,
        presets: [
          SwaggerUIBundle.presets.apis,
          SwaggerUIStandalonePreset
        ],
        plugins: [
          SwaggerUIBundle.plugins.DownloadUrl
        ],
        layout: ""StandaloneLayout""
      });
      window.ui = ui;
    };
  </script>
</body>
</html>";

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html",
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
