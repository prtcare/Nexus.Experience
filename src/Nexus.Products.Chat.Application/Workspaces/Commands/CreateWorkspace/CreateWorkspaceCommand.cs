namespace Nexus.Products.Chat.Application.Workspaces.Commands.CreateWorkspace;

public sealed record CreateWorkspaceCommand(
    string Name,
    string Owner,
    string Description);