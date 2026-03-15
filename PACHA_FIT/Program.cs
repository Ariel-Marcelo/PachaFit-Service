using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PACHA_FIT.Api.Functions.Middlewares;
using PACHA_FIT.Core.Application.Shared;
using PACHA_FIT.Core.Application.User;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.Infrastructure.Persistence;
using PACHA_FIT.Infrastructure.Repositories;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.UseMiddleware<ExceptionHandlerMiddleware>();
builder.UseMiddleware<CustomAuthenticationMiddleware>();
builder.UseMiddleware<CustomAuthorizationMiddleware>();
builder.UseMiddleware<ResultMappingMiddleware>();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();
/*
string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString") 
                          ?? throw new InvalidOperationException("La cadena de conexión 'SqlConnectionString' no está configurada.");

builder.Services.AddDbContext<PachaFitContext>(options =>
    options.UseSqlServer(connectionString));
*/
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<ICredentialService, CredentialService>();

builder.Build().Run();
