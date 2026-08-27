using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.ProductCore.Scope.Project;
using Nexus.Products.Chat.Infrastructure.Sql.Conventions;

namespace Nexus.Products.Chat.Infrastructure.Sql.Configurations;

public sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Project", "project");

        builder.HasKey(project => project.Id);

        builder.Property(project => project.Id)
            .HasConversion(StronglyTypedIdConverters.ProjectId)
            .ValueGeneratedNever();

        builder.Property(project => project.WorkspaceId)
            .HasConversion(StronglyTypedIdConverters.WorkspaceId)
            .IsRequired();

        builder.Property(project => project.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(project => project.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(project => project.CreatedAt)
            .IsRequired();

        // Mirrors WorkspaceConfiguration exactly: the Layer 06 Project entity (like
        // Workspace) carries a Reference property that ADR-014 maps to the computed PRJ- ref
        // column. The Dataverse "narrow slice" mapper never populates Reference, but EF reads
        // it back from the store-generated Ref column - matching how Workspace behaves.
        builder.Property<int>("Seq")
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        var reference = builder.Property(project => project.Reference)
            .HasColumnName("Ref")
            .HasComputedColumnSql(
                "('PRJ-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))",
                stored: true)
            .ValueGeneratedOnAddOrUpdate();

        reference.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        reference.Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

        builder.HasIndex(project => project.Reference)
            .IsUnique()
            .HasDatabaseName("UQ_Project_Ref");

        builder.HasIndex(project => project.WorkspaceId)
            .HasDatabaseName("IX_Project_WorkspaceId");
    }
}
