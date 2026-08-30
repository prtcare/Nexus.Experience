using Nexus.ProductCore.Scope.Common.Identifiers;

namespace Nexus.Products.Chat.Application.Subprojects.Commands.CreateSubproject;

public sealed record CreateSubprojectCommand(
    ProjectId ProjectId,
    string Name,
    string Description,
    CancellationToken CancellationToken = default);
