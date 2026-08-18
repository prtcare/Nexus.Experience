using Nexus.Products.Chat.Domain.Project;

namespace Nexus.Products.Chat.Application.Projects.Commands.CreateProject;

public sealed record CreateProjectResult(
    ProjectId ProjectId,
    string Name);