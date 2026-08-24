# Nexus.Web — the Chat product

The first product built on Nexus: a chatbot with persistent workspaces, projects,
conversations, knowledge, decisions and work items. .NET API at `/api/v1` plus a React
client.

This is the reference implementation for every product that follows.

## Is / is not

**Is:** all the product data and all the product experience. Workspace, Project,
Conversation, ConversationMessage, Knowledge, Adr, WorkItem, Artifact, Session, Branch,
Snapshot — the domain, the database, the API and the UI are all owned here. Future products
will own *different* entities; that is the point of the split.

**Is not:** it does not decide anything. Ranking, agent choice, model choice and prompt
assembly happen in `Nexus.Int`, reached over HTTP. It does not hold a model-provider
credential — that belongs to `Nexus.Int` alone. And the React client knows only about
`/api/v1`; it has no idea Intelligence or Platform exist, which is why there is no page
named after either.

The seam is `ChatContextBundleMapper`: it flattens this product's entities into the canonical
`ContextItem` shape so Intelligence never sees a product table. Changing this mapper changes
answer *quality* silently — no exception, no failing status code — so it is the highest-risk
file in the repo.

> **Intelligence decides. Platform executes. Products own the data and the experience.**

## Local development

```powershell
dotnet build Nexus.Web.slnx
dotnet test  Nexus.Experience.slnx
dotnet run --project src\Nexus.Products.Chat.Api\Nexus.Products.Chat.Api.csproj

cd src\Nexus.Experience.Client
npm install
npm run dev
```

`Nexus.Int` must be running for any chat turn to succeed.

### Persistence

Selected by the `Nexus:Persistence` config key. Migrating Dataverse → Azure SQL by the
strangler pattern: both implementations coexist until every repository interface resolves to
a SQL type, and only then is Dataverse deleted (ADR-014).

```powershell
dotnet ef migrations add <Name> --project src\Nexus.Products.Chat.Infrastructure
dotnet ef database update    --project src\Nexus.Products.Chat.Infrastructure
```

EF Core code-first, always: Domain class → `IEntityTypeConfiguration` → migration → DDL.
Nobody hand-writes DDL that a migration doesn't know about.

### Frontend

One variable in `.env`: `VITE_NEXUS_API_URL`. **Never** an Intelligence or Platform URL —
the product API is the frontend's entire world. Every HTTP call goes through `ApiClient`;
raw `fetch` and direct `import.meta.env` reads outside it are a defect, not a shortcut.

## Documentation

Cross-cutting architecture, conventions and decisions: **`..\NexusAI\docs\`** —
start at `DOCUMENTATION_INDEX.md`. Relevant to this repository specifically:
`API_STANDARDS.md`, `TYPESCRIPT_REACT_STANDARDS.md`, `DATA_OWNERSHIP.md`. This
repo has no `docs\` folder of its own.
