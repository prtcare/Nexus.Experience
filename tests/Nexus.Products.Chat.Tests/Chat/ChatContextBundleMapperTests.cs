using Nexus.Intelligence.Contracts;
using Nexus.Products.Chat.Application.Chat.Context;
using Nexus.Products.Chat.Domain.Adr;
using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Domain.ConversationMessage;
using Nexus.Products.Chat.Domain.Knowledge;
using Nexus.Products.Chat.Domain.Project;
using Nexus.Products.Chat.Domain.WorkItem;
using Xunit;

namespace Nexus.Products.Chat.Tests.Chat;

public sealed class ChatContextBundleMapperTests
{
    private static readonly DateTimeOffset FixedNow = new(2026, 8, 18, 0, 0, 0, TimeSpan.Zero);

    private readonly ChatContextBundleMapper _mapper = new();

    private static Project CreateProject(WorkspaceId? workspaceId = null, string name = "Project X")
        => new(ProjectId.New(), workspaceId ?? WorkspaceId.New(), name, FixedNow);

    private static Conversation CreateConversation(Project project, string description = "")
        => new(
            ConversationId.New(),
            project.Id,
            project.WorkspaceId,
            "Conversation Title",
            description,
            ConversationType.Standalone,
            ConversationVisibility.Private,
            FixedNow);

    private static ConversationMessage CreateMessage(ConversationId conversationId)
        => new(ConversationMessageId.New(), conversationId, ConversationMessageRole.User, "Hello there", FixedNow);

    private static Knowledge CreateKnowledge(WorkspaceId workspaceId, KnowledgeStatus status = KnowledgeStatus.Active)
    {
        var knowledge = new Knowledge(
            new KnowledgeId(Guid.NewGuid()),
            workspaceId,
            "Knowledge Title",
            "Knowledge Content",
            KnowledgeType.Documentation,
            FixedNow);

        if (status != KnowledgeStatus.Active)
        {
            knowledge.ChangeStatus(status);
        }

        return knowledge;
    }

    private static Adr CreateAdr(KnowledgeId knowledgeId)
        => new(AdrId.New(), knowledgeId, "Adr Title", "Adr Decision", AdrStatus.Accepted, FixedNow);

    private static WorkItem CreateWorkItem(ProjectId projectId, string? description = null)
    {
        var workItem = new WorkItem(WorkItemId.New(), projectId, "Work Item Title", WorkItemType.Task, FixedNow);

        if (description is not null)
        {
            workItem.UpdateDescription(description);
        }

        return workItem;
    }

    private static ChatContextSources EmptySources(Project project, Conversation conversation)
        => new()
        {
            Project = project,
            Conversation = conversation,
            History = [],
            Knowledge = [],
            Decisions = [],
            OpenWorkItems = []
        };

    // --- 1. one test per source type: ContextItemKind + TrustLevel -----------

    [Fact]
    public void Map_ConversationMessage_MapsToMessageReported()
    {
        var project = CreateProject();
        var conversation = CreateConversation(project);
        var message = CreateMessage(conversation.Id);
        var sources = EmptySources(project, conversation) with { History = [message] };

        var bundle = _mapper.Map(sources);

        var item = Assert.Single(bundle.Items, i => i.Id == message.Id.Value.ToString());
        Assert.Equal(ContextItemKind.Message, item.Kind);
        Assert.Equal(TrustLevel.Reported, item.Trust);
    }

    [Fact]
    public void Map_ActiveKnowledge_MapsToFactApproved()
    {
        var project = CreateProject();
        var conversation = CreateConversation(project);
        var knowledge = CreateKnowledge(project.WorkspaceId, KnowledgeStatus.Active);
        var sources = EmptySources(project, conversation) with { Knowledge = [knowledge] };

        var bundle = _mapper.Map(sources);

        var item = Assert.Single(bundle.Items, i => i.Id == knowledge.Id.Value.ToString());
        Assert.Equal(ContextItemKind.Fact, item.Kind);
        Assert.Equal(TrustLevel.Approved, item.Trust);
    }

    [Fact]
    public void Map_Adr_MapsToDecisionAuthoritative()
    {
        var project = CreateProject();
        var conversation = CreateConversation(project);
        var knowledge = CreateKnowledge(project.WorkspaceId);
        var adr = CreateAdr(knowledge.Id);
        var sources = EmptySources(project, conversation) with { Decisions = [adr] };

        var bundle = _mapper.Map(sources);

        var item = Assert.Single(bundle.Items, i => i.Id == adr.Id.Value.ToString());
        Assert.Equal(ContextItemKind.Decision, item.Kind);
        Assert.Equal(TrustLevel.Authoritative, item.Trust);
    }

    [Fact]
    public void Map_Project_MapsToObjectiveAuthoritative()
    {
        var project = CreateProject();
        var conversation = CreateConversation(project);
        var sources = EmptySources(project, conversation);

        var bundle = _mapper.Map(sources);

        var item = Assert.Single(bundle.Items, i => i.Id == project.Id.Value.ToString());
        Assert.Equal(ContextItemKind.Objective, item.Kind);
        Assert.Equal(TrustLevel.Authoritative, item.Trust);
    }

    [Fact]
    public void Map_WorkItem_MapsToConstraintCurated()
    {
        var project = CreateProject();
        var conversation = CreateConversation(project);
        var workItem = CreateWorkItem(project.Id);
        var sources = EmptySources(project, conversation) with { OpenWorkItems = [workItem] };

        var bundle = _mapper.Map(sources);

        var item = Assert.Single(bundle.Items, i => i.Id == workItem.Id.Value.ToString());
        Assert.Equal(ContextItemKind.Constraint, item.Kind);
        Assert.Equal(TrustLevel.Curated, item.Trust);
    }

    [Fact]
    public void Map_ConversationWithDescription_MapsToInstructionCurated()
    {
        var project = CreateProject();
        var conversation = CreateConversation(project, description: "Follow these instructions");
        var sources = EmptySources(project, conversation);

        var bundle = _mapper.Map(sources);

        var item = Assert.Single(bundle.Items, i => i.Kind == ContextItemKind.Instruction);
        Assert.Equal(conversation.Id.Value.ToString(), item.Id);
        Assert.Equal(TrustLevel.Curated, item.Trust);
    }

    // --- 2. every KnowledgeStatus member maps explicitly, without throwing ---

    public static IEnumerable<object[]> AllKnowledgeStatuses()
        => Enum.GetValues<KnowledgeStatus>().Select(status => new object[] { status });

    [Theory]
    [MemberData(nameof(AllKnowledgeStatuses))]
    public void Map_EveryKnowledgeStatus_MapsExplicitlyWithoutThrowing(KnowledgeStatus status)
    {
        var project = CreateProject();
        var conversation = CreateConversation(project);
        var knowledge = CreateKnowledge(project.WorkspaceId, status);
        var sources = EmptySources(project, conversation) with { Knowledge = [knowledge] };

        var exception = Record.Exception(() => _mapper.Map(sources));

        Assert.Null(exception);
    }

    // --- 3. BuildScope --------------------------------------------------------

    [Fact]
    public void BuildScope_ReturnsConversationScopeWithThreeOrderedPathSegments()
    {
        var project = CreateProject();
        var conversation = CreateConversation(project);

        var scope = _mapper.BuildScope(project, conversation);

        Assert.Equal("conversation", scope.Kind);
        Assert.Equal(conversation.Id.Value.ToString(), scope.Key);
        Assert.Equal(
            [
                $"workspace:{project.WorkspaceId.Value}",
                $"project:{project.Id.Value}",
                $"conversation:{conversation.Id.Value}"
            ],
            scope.Path);
    }

    // --- 4. completeness: one of every source type, nothing dropped ----------

    [Fact]
    public void Map_OneOfEverySourceType_ReturnsExactlyOneItemPerSourceWithNoneDropped()
    {
        var project = CreateProject();
        var conversation = CreateConversation(project, description: "Has an instruction");
        var message = CreateMessage(conversation.Id);
        var knowledge = CreateKnowledge(project.WorkspaceId);
        var adr = CreateAdr(knowledge.Id);
        var workItem = CreateWorkItem(project.Id);

        var sources = new ChatContextSources
        {
            Project = project,
            Conversation = conversation,
            History = [message],
            Knowledge = [knowledge],
            Decisions = [adr],
            OpenWorkItems = [workItem]
        };

        var bundle = _mapper.Map(sources);

        var expectedIds = new HashSet<string>
        {
            project.Id.Value.ToString(),
            conversation.Id.Value.ToString(),
            message.Id.Value.ToString(),
            knowledge.Id.Value.ToString(),
            adr.Id.Value.ToString(),
            workItem.Id.Value.ToString()
        };

        Assert.Equal(expectedIds.Count, bundle.Items.Count);
        Assert.Equal(expectedIds, bundle.Items.Select(i => i.Id).ToHashSet());
    }

    // --- 5. empty collections / blank description ----------------------------

    [Fact]
    public void Map_EmptyCollections_ReturnsNonNullBundleWithOnlyTheMandatoryProjectItem()
    {
        var project = CreateProject();
        var conversation = CreateConversation(project, description: "   ");
        var sources = EmptySources(project, conversation);

        var bundle = _mapper.Map(sources);

        Assert.NotNull(bundle);
        Assert.NotNull(bundle.Items);
        var item = Assert.Single(bundle.Items);
        Assert.Equal(ContextItemKind.Objective, item.Kind);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Map_ConversationWithNullOrWhitespaceDescription_ProducesNoInstructionItem(string? description)
    {
        var project = CreateProject();
        var conversation = CreateConversation(project, description: description ?? "");
        var sources = EmptySources(project, conversation);

        var bundle = _mapper.Map(sources);

        Assert.DoesNotContain(bundle.Items, i => i.Kind == ContextItemKind.Instruction);
    }

    // --- 6. ContextItem.Id is the source entity's own id, for citation resolution --

    [Fact]
    public void Map_Message_ItemIdIsSourceMessageId()
    {
        var project = CreateProject();
        var conversation = CreateConversation(project);
        var message = CreateMessage(conversation.Id);
        var sources = EmptySources(project, conversation) with { History = [message] };

        var bundle = _mapper.Map(sources);

        var item = Assert.Single(bundle.Items, i => i.Kind == ContextItemKind.Message);
        Assert.Equal(message.Id.Value.ToString(), item.Id);
    }

    [Fact]
    public void Map_Knowledge_ItemIdIsSourceKnowledgeId()
    {
        var project = CreateProject();
        var conversation = CreateConversation(project);
        var knowledge = CreateKnowledge(project.WorkspaceId);
        var sources = EmptySources(project, conversation) with { Knowledge = [knowledge] };

        var bundle = _mapper.Map(sources);

        var item = Assert.Single(bundle.Items, i => i.Kind == ContextItemKind.Fact);
        Assert.Equal(knowledge.Id.Value.ToString(), item.Id);
    }

    [Fact]
    public void Map_WorkItem_ItemIdIsSourceWorkItemId()
    {
        var project = CreateProject();
        var conversation = CreateConversation(project);
        var workItem = CreateWorkItem(project.Id);
        var sources = EmptySources(project, conversation) with { OpenWorkItems = [workItem] };

        var bundle = _mapper.Map(sources);

        var item = Assert.Single(bundle.Items, i => i.Kind == ContextItemKind.Constraint);
        Assert.Equal(workItem.Id.Value.ToString(), item.Id);
    }
}
