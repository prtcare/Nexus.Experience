using Nexus.ProductCore.Scope.Common.Identifiers;

namespace Nexus.Products.Chat.Application.Projects.Commands.CreateProject;

public sealed record CreateProjectResult(
    ProjectId ProjectId,
    string Name);