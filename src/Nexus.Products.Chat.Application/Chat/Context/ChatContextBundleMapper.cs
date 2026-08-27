using Nexus.Intelligence.Contracts;
using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Domain.ConversationMessage;
using Nexus.ProductCore.Scope.Project;
using AdrModel = Nexus.Products.Chat.Domain.Adr.Adr;
using KnowledgeModel = Nexus.Products.Chat.Domain.Knowledge.Knowledge;
using KnowledgeStatus = Nexus.Products.Chat.Domain.Knowledge.KnowledgeStatus;
using WorkItemModel = Nexus.Products.Chat.Domain.WorkItem.WorkItem;

namespace Nexus.Products.Chat.Application.Chat.Context;

public interface IChatContextBundleMapper
{
    ContextBundle Map(ChatContextSources sources);

    ScopeRef BuildScope(Project project, Conversation conversation);
}

public sealed record ChatContextSources
{
    public required Project Project { get; init; }

    public required Conversation Conversation { get; init; }

    public required IReadOnlyList<ConversationMessage> History { get; init; }

    public IReadOnlyList<KnowledgeModel> Knowledge { get; init; } = [];

    public IReadOnlyList<AdrModel> Decisions { get; init; } = [];

    public IReadOnlyList<WorkItemModel> OpenWorkItems { get; init; } = [];
}

public sealed class ChatContextBundleMapper : IChatContextBundleMapper
{
    public ContextBundle Map(ChatContextSources sources)
    {
        var items = new List<ContextItem>(
            sources.History.Count
            + sources.Knowledge.Count
            + sources.Decisions.Count
            + sources.OpenWorkItems.Count
            + 2);

        items.Add(MapProject(sources.Project));

        var conversationInstruction = MapConversationInstruction(sources.Conversation);
        if (conversationInstruction is not null)
        {
            items.Add(conversationInstruction);
        }

        foreach (var message in sources.History)
        {
            items.Add(MapMessage(message));
        }

        foreach (var knowledge in sources.Knowledge)
        {
            items.Add(MapKnowledge(knowledge));
        }

        foreach (var adr in sources.Decisions)
        {
            items.Add(MapDecision(adr));
        }

        foreach (var workItem in sources.OpenWorkItems)
        {
            items.Add(MapWorkItem(workItem));
        }

        return new ContextBundle(items);
    }

    public ScopeRef BuildScope(Project project, Conversation conversation)
    {
        return new ScopeRef(
            "conversation",
            conversation.Id.Value.ToString(),
            [
                $"workspace:{project.WorkspaceId.Value}",
                $"project:{project.Id.Value}",
                $"conversation:{conversation.Id.Value}"
            ]);
    }

    private static ContextItem MapMessage(ConversationMessage message)
    {
        return new ContextItem
        {
            Id = message.Id.Value.ToString(),
            Kind = ContextItemKind.Message,
            Trust = TrustLevel.Reported,
            Title = null,
            Body = message.Content,
            Author = message.Role.ToString(),
            OccurredAt = message.CreatedOn
        };
    }

    private static ContextItem MapKnowledge(KnowledgeModel knowledge)
    {
        return new ContextItem
        {
            Id = knowledge.Id.Value.ToString(),
            Kind = ContextItemKind.Fact,
            Trust = MapKnowledgeTrust(knowledge.Status),
            Title = knowledge.Title,
            Body = knowledge.Content,
            OccurredAt = knowledge.CreatedAt
        };
    }

    // KnowledgeStatus has no member literally named "Approved" or "Draft" - Active is the
    // product's stand-in for approved/published, Draft is pending. Archived and Superseded
    // are neither: the content is no longer the current truth, so it is deliberately
    // downgraded rather than left at whatever trust it had while Active.
    private static TrustLevel MapKnowledgeTrust(KnowledgeStatus status) => status switch
    {
        KnowledgeStatus.Active => TrustLevel.Approved,
        KnowledgeStatus.Draft => TrustLevel.Curated,
        KnowledgeStatus.Archived => TrustLevel.Unverified,
        KnowledgeStatus.Superseded => TrustLevel.Unverified,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status,
                 "Unmapped KnowledgeStatus - add an explicit TrustLevel mapping.")
    };

    private static ContextItem MapDecision(AdrModel adr)
    {
        return new ContextItem
        {
            Id = adr.Id.Value.ToString(),
            Kind = ContextItemKind.Decision,
            Trust = TrustLevel.Authoritative,
            Title = adr.Title,
            Body = adr.Decision,
            OccurredAt = adr.CreatedAt
        };
    }

    private static ContextItem MapProject(Project project)
    {
        return new ContextItem
        {
            Id = project.Id.Value.ToString(),
            Kind = ContextItemKind.Objective,
            Trust = TrustLevel.Authoritative,
            Title = project.Name,
            Body = project.Name
        };
    }

    private static ContextItem MapWorkItem(WorkItemModel workItem)
    {
        return new ContextItem
        {
            Id = workItem.Id.Value.ToString(),
            Kind = ContextItemKind.Constraint,
            Trust = TrustLevel.Curated,
            Title = workItem.Title,
            Body = workItem.Description ?? string.Empty,
            OccurredAt = workItem.CreatedAt
        };
    }

    private static ContextItem? MapConversationInstruction(Conversation conversation)
    {
        if (string.IsNullOrWhiteSpace(conversation.Description))
        {
            return null;
        }

        return new ContextItem
        {
            Id = conversation.Id.Value.ToString(),
            Kind = ContextItemKind.Instruction,
            Trust = TrustLevel.Curated,
            Title = conversation.Title,
            Body = conversation.Description,
            OccurredAt = conversation.CreatedAt
        };
    }
}
