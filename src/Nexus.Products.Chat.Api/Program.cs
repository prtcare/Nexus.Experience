using System.Text.Json.Serialization;
using Microsoft.OpenApi;
using Nexus.Products.Chat.Api;
using Nexus.Products.Chat.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddChatProduct(builder.Configuration);

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Nexus Chat API v1",
            Version = "v1",
            Description = "REST API for the Nexus Chat product"
        });
});

var allowedOrigins =
    builder.Configuration
        .GetSection("Nexus:Cors:AllowedOrigins")
        .Get<string[]>()
    ?? ["http://localhost:5173"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("NexusWebDevelopment", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("NexusWebDevelopment");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Nexus Chat API v1");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapChatProduct();
app.MapHealthEndpoint();

app.Run();
