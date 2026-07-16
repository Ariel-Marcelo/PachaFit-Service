using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PACHA_FIT.Infrastructure.Persistence;

public class PachaFitContextFactory : IDesignTimeDbContextFactory<PachaFitContext>
{
    public PachaFitContext CreateDbContext(string[] args)
    {
        // Try to find local.settings.json in current directory or PACHA_FIT subdirectory
        var basePath = Directory.GetCurrentDirectory();
        if (!File.Exists(Path.Combine(basePath, "local.settings.json")))
        {
            basePath = Path.Combine(basePath, "PACHA_FIT");
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("local.settings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<PachaFitContext>();

        // Try SqlConnectionString first (standard Azure/manual setting)
        var connectionString = configuration.GetSection("Values")["SqlConnectionString"];

        if (string.IsNullOrEmpty(connectionString))
        {
            // Switch logic
            bool useAzureSql = configuration.GetSection("Values")["UseAzureSql"]?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
            string key = useAzureSql ? "AzureSqlConnectionString" : "LocalSqlConnectionString";
            connectionString = configuration.GetSection("Values")[key];
        }

        // Fallback for development if no settings file or variables found
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = "Server=(localdb)\\mssqllocaldb;Database=PachaFitTemp;Trusted_Connection=True;";
        }

        optionsBuilder.UseSqlServer(connectionString);

        return new PachaFitContext(optionsBuilder.Options);
    }
}
