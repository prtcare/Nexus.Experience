using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Domain.Project;

namespace Nexus.Products.Chat.Application.Conversations.Commands.CreateConversation;

public sealed record CreateConversationCommand(
    ProjectId ProjectId,
    WorkspaceId WorkspaceId,
    string Title,
    string Description,
    ConversationType Type,
    ConversationVisibility Visibility);