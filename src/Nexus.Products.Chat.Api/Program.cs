using Microsoft.OpenApi;
using Nexus.Products.Chat.Api.Endpoints;
using Nexus.Products.Chat.Api.Endpoints.Artifacts;
using Nexus.Products.Chat.Api.Endpoints.Branches;
using Nexus.Products.Chat.Api.Endpoints.Chat;
using Nexus.Products.Chat.Api.Endpoints.Conversations;
using Nexus.Products.Chat.Api.Endpoints.Knowledge;
using Nexus.Products.Chat.Api.Endpoints.Projects;
using Nexus.Products.Chat.Api.Endpoints.Sessions;
using Nexus.Products.Chat.Api.Endpoints.Snapshots;
using Nexus.Products.Chat.Api.Endpoints.WorkItems;
using Nexus.Products.Chat.Api.Endpoints.Workspaces;
using Nexus.Products.Chat.Application.DependencyInjection;
using Nexus.Products.Chat.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Nexus.Products.Chat API",
            Version = "v1",
            Description = "REST API for the Nexus Chat product"
        });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("NexusWebDevelopment", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("NexusWebDevelopment");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Nexus.Products.Chat API v1");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapChatEndpoints();
app.MapConversationEndpoints();
app.MapConversationMessageEndpoints();
app.MapProjectEndpoints();
app.MapWorkItemEndpoints();
app.MapKnowledgeEndpoints();
app.MapWorkspaceEndpoints();
app.MapSnapshotEndpoints();
app.MapBranchEndpoints();
app.MapSessionEndpoints();
app.MapArtifactEndpoints();
app.MapHealthEndpoint();

app.Run();
