using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nexus.ProductCore.Scope.Common.Identifiers;
using Nexus.Products.Chat.Domain.Adr;
using Nexus.Products.Chat.Domain.Artifact;
using Nexus.Products.Chat.Domain.Branch;
using Nexus.Products.Chat.Domain.Knowledge;
using Nexus.Products.Chat.Domain.Session;
using Nexus.Products.Chat.Domain.Snapshot;
using Nexus.Products.Chat.Domain.WorkItem;
using ChatProjectId = Nexus.Products.Chat.Domain.Project.ProjectId;
using ChatWorkspaceId = Nexus.Products.Chat.Domain.Common.Identifiers.WorkspaceId;
using ChatConversationId = Nexus.Products.Chat.Domain.Conversation.ConversationId;
using ChatConversationMessageId = Nexus.Products.Chat.Domain.ConversationMessage.ConversationMessageId;

namespace Nexus.Products.Chat.Infrastructure.Sql.Conventions;

public static class StronglyTypedIdConverters
{
    // Nexus.ProductCore.Scope's shared identifiers (Workspace, Project, Subproject).
    public static ValueConverter<WorkspaceId, Guid> WorkspaceId { get; } =
        Create<WorkspaceId>(id => id.Value, value => new WorkspaceId(value));

    public static ValueConverter<ProjectId, Guid> ProjectId { get; } =
        Create<ProjectId>(id => id.Value, value => new ProjectId(value));

    public static ValueConverter<SubprojectId, Guid> SubprojectId { get; } =
        Create<SubprojectId>(id => id.Value, value => new SubprojectId(value));

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

    // Stage 2b aggregates - Knowledge, Adr, WorkItem, Artifact are all Chat's own domain
    // types with Chat-local id structs (no Nexus.ProductCore.Scope counterpart), so their
    // converters are plain like the Conversation pair above.
    public static ValueConverter<KnowledgeId, Guid> KnowledgeId { get; } =
        Create<KnowledgeId>(id => id.Value, value => new KnowledgeId(value));

    public static ValueConverter<AdrId, Guid> AdrId { get; } =
        Create<AdrId>(id => id.Value, value => new AdrId(value));

    public static ValueConverter<WorkItemId, Guid> WorkItemId { get; } =
        Create<WorkItemId>(id => id.Value, value => new WorkItemId(value));

    public static ValueConverter<ArtifactId, Guid> ArtifactId { get; } =
        Create<ArtifactId>(id => id.Value, value => new ArtifactId(value));

    // Stage 2c aggregates - Branch, Snapshot, Session are all Chat's own domain types with
    // Chat-local id structs (no Nexus.ProductCore.Scope counterpart), so their converters
    // are plain like the Conversation pair above.
    public static ValueConverter<BranchId, Guid> BranchId { get; } =
        Create<BranchId>(id => id.Value, value => new BranchId(value));

    public static ValueConverter<SnapshotId, Guid> SnapshotId { get; } =
        Create<SnapshotId>(id => id.Value, value => new SnapshotId(value));

    public static ValueConverter<SessionId, Guid> SessionId { get; } =
        Create<SessionId>(id => id.Value, value => new SessionId(value));

    private static ValueConverter<TId, Guid> Create<TId>(
        Func<TId, Guid> toGuid,
        Func<Guid, TId> fromGuid)
    {
        return new ValueConverter<TId, Guid>(
            id => toGuid(id),
            value => fromGuid(value));
    }
}
