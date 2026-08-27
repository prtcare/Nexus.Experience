using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nexus.ProductCore.Scope.Common.Identifiers;
using ChatProjectId = Nexus.Products.Chat.Domain.Project.ProjectId;
using ChatWorkspaceId = Nexus.Products.Chat.Domain.Common.Identifiers.WorkspaceId;
using ChatConversationId = Nexus.Products.Chat.Domain.Conversation.ConversationId;
using ChatConversationMessageId = Nexus.Products.Chat.Domain.ConversationMessage.ConversationMessageId;

namespace Nexus.Products.Chat.Infrastructure.Sql.Conventions;

public static class StronglyTypedIdConverters
{
    // Nexus.ProductCore.Scope's shared identifiers (Workspace, Project).
    public static ValueConverter<WorkspaceId, Guid> WorkspaceId { get; } =
        Create<WorkspaceId>(id => id.Value, value => new WorkspaceId(value));

    public static ValueConverter<ProjectId, Guid> ProjectId { get; } =
        Create<ProjectId>(id => id.Value, value => new ProjectId(value));

    // Chat's own opaque id structs. Conversation and ConversationMessage stay in this repo's
    // Domain and key on Chat-local ProjectId/WorkspaceId (AGENTS.md boundary rule - see
    // SendChatHandler's conversion at the one scope boundary), so they get their own
    // converters rather than reusing Nexus.ProductCore.Scope's.
    public static ValueConverter<ChatProjectId, Guid> ChatProjectId { get; } =
        Create<ChatProjectId>(id => id.Value, value => new ChatProjectId(value));

    public static ValueConverter<ChatWorkspaceId, Guid> ChatWorkspaceId { get; } =
        Create<ChatWorkspaceId>(id => id.Value, value => new ChatWorkspaceId(value));

    public static ValueConverter<ChatConversationId, Guid> ConversationId { get; } =
        Create<ChatConversationId>(id => id.Value, value => new ChatConversationId(value));

    public static ValueConverter<ChatConversationMessageId, Guid> ConversationMessageId { get; } =
        Create<ChatConversationMessageId>(id => id.Value, value => new ChatConversationMessageId(value));

    private static ValueConverter<TId, Guid> Create<TId>(
        Func<TId, Guid> toGuid,
        Func<Guid, TId> fromGuid)
    {
        return new ValueConverter<TId, Guid>(
            id => toGuid(id),
            value => fromGuid(value));
    }
}
