using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PACHA_FIT.Infrastructure.Persistence;

public class PachaFitContextFactory : IDesignTimeDbContextFactory<PachaFitContext>
{
    public PachaFitContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PachaFitContext>();
        
        // Esta cadena solo se usa en tiempo de diseño (para generar migraciones)
        // No necesita ser una base de datos real o válida para Azure en este momento
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PachaFitTemp;Trusted_Connection=True;");

        return new PachaFitContext(optionsBuilder.Options);
    }
}
