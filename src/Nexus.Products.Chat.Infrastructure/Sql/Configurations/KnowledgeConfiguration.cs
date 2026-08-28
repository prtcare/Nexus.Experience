using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Products.Chat.Domain.Knowledge;
using Nexus.Products.Chat.Infrastructure.Sql.Conventions;

namespace Nexus.Products.Chat.Infrastructure.Sql.Configurations;

public sealed class KnowledgeConfiguration : IEntityTypeConfiguration<Knowledge>
{
    public void Configure(EntityTypeBuilder<Knowledge> builder)
    {
        builder.ToTable("Knowledge", "knowledge");

        builder.HasKey(knowledge => knowledge.Id);

        builder.Property(knowledge => knowledge.Id)
            .HasConversion(StronglyTypedIdConverters.KnowledgeId)
            .ValueGeneratedNever();

        // Knowledge keys on Chat's own opaque WorkspaceId struct (AGENTS.md boundary rule),
        // not Nexus.ProductCore.Scope's. The org.Workspace principal is a different CLR type
        // than Chat's WorkspaceId, so like Conversation.ProjectId/WorkspaceId (Stage 2a) this
        // is an index-only column - there is no EF relationship and no DB FK to cascade.
        builder.Property(knowledge => knowledge.WorkspaceId)
            .HasConversion(StronglyTypedIdConverters.ChatWorkspaceId)
            .IsRequired();

        builder.Property(knowledge => knowledge.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(knowledge => knowledge.Content)
            .HasColumnType("nvarchar(max)");

        builder.Property(knowledge => knowledge.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(knowledge => knowledge.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(knowledge => knowledge.CreatedAt)
            .IsRequired();

        // The Knowledge domain entity exposes no Reference property (HARD constraint: no
        // Domain edits). ADR-014's schema map still gives knowledge.Knowledge a KNW- ref for
        // external tracing, so the column is an EF shadow property fed by the same Seq
        // identity pattern Workspace uses, mapped to no CLR property.
        builder.Property<int>("Seq")
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        var reference = builder.Property<string>("Ref")
            // The computed KNW- ref is never NULL (Seq is an identity seed), so model it
            // non-nullable - a nullable unique index would need a [Ref] IS NOT NULL filter,
            // which SQL Server rejects on a computed column (error 10609).
            .IsRequired()
            .HasComputedColumnSql(
                "('KNW-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))",
                stored: true)
            .ValueGeneratedOnAddOrUpdate();

        reference.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        reference.Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

        builder.HasIndex("Ref")
            .IsUnique()
            .HasDatabaseName("UQ_Knowledge_Ref");

        // ADR-014 hot-path index.
        builder.HasIndex(knowledge => new { knowledge.WorkspaceId, knowledge.Status })
            .HasDatabaseName("IX_Knowledge_WorkspaceId_Status");
    }
}
