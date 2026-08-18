namespace Nexus.Products.Chat.Infrastructure.Dataverse.Clients;

public interface IDataverseClient
{
    IDataverseContext Context { get; }
}