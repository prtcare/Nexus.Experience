using Nexus.ProductCore.Scope.Common.Identifiers;

namespace Nexus.Products.Chat.Application.Subprojects.Commands.CreateSubproject;

public sealed record CreateSubprojectResult(
    SubprojectId SubprojectId,
    string Name,
    string Reference);
