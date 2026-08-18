namespace Nexus.Products.Chat.Infrastructure.Dataverse.Clients;

public sealed class DataverseClient : IDataverseClient
{
    public DataverseClient(IDataverseContext context)
    {
        Context = context;
    }

    public IDataverseContext Context { get; }
}