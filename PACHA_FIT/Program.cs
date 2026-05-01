using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PACHA_FIT.Api.Middlewares;
using PACHA_FIT.Core.Application.User;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.Infrastructure.Persistence;
using PACHA_FIT.Infrastructure.Repositories;
using PACHA_FIT.Infrastructure.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.UseMiddleware<ExceptionHandlerMiddleware>();
builder.UseMiddleware<CustomAuthenticationMiddleware>();
builder.UseMiddleware<CustomAuthorizationMiddleware>();
builder.UseMiddleware<ResultMappingMiddleware>();

// Application Insights
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

string? connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

if (string.IsNullOrEmpty(connectionString))
{
    bool useAzureSql = Environment.GetEnvironmentVariable("UseAzureSql")?.ToLower() == "true";
    string connectionStringKey = useAzureSql ? "AzureSqlConnectionString" : "LocalSqlConnectionString";
    connectionString = Environment.GetEnvironmentVariable(connectionStringKey);
}

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("La cadena de conexión 'SqlConnectionString', 'LocalSqlConnectionString' o 'AzureSqlConnectionString' no está configurada.");
}

builder.Services.AddDbContext<PachaFitContext>(options =>
    options.UseSqlServer(connectionString));

// User
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ICredentialService, CredentialService>();
builder.Services.AddSingleton<IPasswordService, BCryptPasswordService>();

var host = builder.Build();

// Aplicar migraciones automáticamente
using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<PachaFitContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        // Log error if needed, for now just continue
        Console.WriteLine($"Error applying migrations: {ex.Message}");
    }
}

host.Run();
