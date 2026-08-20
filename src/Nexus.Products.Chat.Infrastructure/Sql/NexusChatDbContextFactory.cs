using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Nexus.Products.Chat.Infrastructure.Sql;

public sealed class NexusChatDbContextFactory : IDesignTimeDbContextFactory<NexusChatDbContext>
{
    private const string LocalDevConnectionString =
        "Server=(localdb)\\MSSQLLocalDB;Database=NexusChat;Trusted_Connection=True;MultipleActiveResultSets=True";

    public NexusChatDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__NexusChat")
            ?? LocalDevConnectionString;

        var optionsBuilder = new DbContextOptionsBuilder<NexusChatDbContext>();

        optionsBuilder.UseSqlServer(
            connectionString,
            sqlOptions => sqlOptions.EnableRetryOnFailure());

        return new NexusChatDbContext(optionsBuilder.Options);
    }
}
