using System.Reflection;
using NetArchTest.Rules;
using Xunit;

namespace Nexus.Products.Chat.Architecture.Tests;

public sealed class BoundaryTests
{
    private static readonly Assembly DomainAssembly =
        typeof(Nexus.Products.Chat.Domain.Workspace.Workspace).Assembly;

    private static readonly Assembly ApplicationAssembly =
        typeof(Nexus.Products.Chat.Application.DependencyInjection.ServiceCollectionExtensions).Assembly;

    private static readonly Assembly InfrastructureAssembly =
        typeof(Nexus.Products.Chat.Infrastructure.DependencyInjection.ServiceCollectionExtensions).Assembly;

    private static readonly Assembly ApiAssembly =
        typeof(Nexus.Products.Chat.Api.ChatProductModule).Assembly;

    private static readonly Assembly[] ProductAssemblies =
    [
        DomainAssembly,
        ApplicationAssembly,
        InfrastructureAssembly,
        ApiAssembly
    ];

    [Fact]
    public void Products_MustNotReference_Platform()
    {
        var failures = new List<string>();

        foreach (var assembly in ProductAssemblies)
        {
            var result = Types.InAssembly(assembly)
                .Should()
                .NotHaveDependencyOn("Nexus.Platform")
                .GetResult();

            if (!result.IsSuccessful)
            {
                failures.AddRange(result.FailingTypeNames ?? []);
            }
        }

        Assert.True(
            failures.Count == 0,
            "NEXUS_ARCHITECTURE_V2.md section 2.3: no type in any Nexus.Products.* " +
            "assembly may depend on a Nexus.Platform.* type. Offending types: " +
            string.Join(", ", failures));
    }

    [Fact]
    public void Products_MustOnlyReference_IntelligenceContracts()
    {
        // Core, Context, Agents and Memory are internals of the Nexus.Int host - the
        // product may only take a dependency on Nexus.Intelligence.Contracts.
        var forbidden = new[]
        {
            "Nexus.Intelligence.Core",
            "Nexus.Intelligence.Context",
            "Nexus.Intelligence.Agents",
            "Nexus.Intelligence.Memory"
        };

        var failures = new List<string>();

        foreach (var assembly in ProductAssemblies)
        {
            var result = Types.InAssembly(assembly)
                .Should()
                .NotHaveDependencyOnAny(forbidden)
                .GetResult();

            if (!result.IsSuccessful)
            {
                failures.AddRange(result.FailingTypeNames ?? []);
            }
        }

        Assert.True(
            failures.Count == 0,
            "NEXUS_ARCHITECTURE_V2.md section 2.3: the only Nexus.Intelligence.* " +
            "dependency the product may take is Nexus.Intelligence.Contracts. Offending " +
            "types: " + string.Join(", ", failures));
    }

    [Fact]
    public void Domain_MustNotReference_Dataverse()
    {
        var result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOnAny(
                "Microsoft.PowerPlatform.Dataverse",
                "Nexus.Products.Chat.Infrastructure")
            .GetResult();

        Assert.True(
            result.IsSuccessful,
            "NEXUS_ARCHITECTURE_V2.md section 2.3: Nexus.Products.Chat.Domain must not " +
            "depend on Microsoft.PowerPlatform.Dataverse or any Infrastructure type - the " +
            "domain must not know how it is stored. Offending types: " +
            string.Join(", ", result.FailingTypeNames ?? []));
    }

    [Fact]
    public void Application_MustNotReference_Dataverse()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOnAny(
                "Microsoft.PowerPlatform.Dataverse",
                "Nexus.Products.Chat.Infrastructure")
            .GetResult();

        Assert.True(
            result.IsSuccessful,
            "NEXUS_ARCHITECTURE_V2.md section 2.3: Nexus.Products.Chat.Application must " +
            "not depend on Microsoft.PowerPlatform.Dataverse or any Infrastructure type - " +
            "it depends on repository interfaces only. Offending types: " +
            string.Join(", ", result.FailingTypeNames ?? []));
    }
}
