using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;

namespace Lattice.Infrastructure.Data;

public class LatticeDbContextFactory : IDesignTimeDbContextFactory<LatticeDbContext>
{
    public LatticeDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        var optionsBuilder = new DbContextOptionsBuilder<LatticeDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("LatticeSql"));

        return new LatticeDbContext(optionsBuilder.Options);
    }
}