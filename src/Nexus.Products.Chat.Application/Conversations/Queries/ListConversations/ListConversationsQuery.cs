using Nexus.Products.Chat.Domain.Project;

namespace Nexus.Products.Chat.Application.Conversations.Queries.ListConversations;

public sealed record ListConversationsQuery(
    ProjectId ProjectId);