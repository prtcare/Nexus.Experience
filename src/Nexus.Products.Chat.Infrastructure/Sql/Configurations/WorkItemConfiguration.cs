using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Products.Chat.Domain.WorkItem;
using Nexus.Products.Chat.Infrastructure.Sql.Conventions;

namespace Nexus.Products.Chat.Infrastructure.Sql.Configurations;

public sealed class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
{
    public void Configure(EntityTypeBuilder<WorkItem> builder)
    {
        builder.ToTable("WorkItem", "work");

        builder.HasKey(workItem => workItem.Id);

        builder.Property(workItem => workItem.Id)
            .HasConversion(StronglyTypedIdConverters.WorkItemId)
            .ValueGeneratedNever();

        // WorkItem keys on Chat's own opaque ProjectId struct (AGENTS.md boundary rule), not
        // Nexus.ProductCore.Scope's. The project.Project principal is a different CLR type
        // than Chat's ProjectId, so like Conversation.ProjectId/WorkspaceId (Stage 2a) this
        // is an index-only column - there is no EF relationship and no DB FK to cascade.
        builder.Property(workItem => workItem.ProjectId)
            .HasConversion(StronglyTypedIdConverters.ChatProjectId)
            .IsRequired();

        builder.Property(workItem => workItem.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(workItem => workItem.Description)
            .HasColumnType("nvarchar(max)");

        builder.Property(workItem => workItem.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(workItem => workItem.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(workItem => workItem.CreatedAt)
            .IsRequired();

        // The WorkItem domain entity exposes no Reference property (HARD constraint: no
        // Domain edits). ADR-014's schema map still gives work.WorkItem a WRK- ref for
        // external tracing, so the column is an EF shadow property fed by the same Seq
        // identity pattern Workspace uses, mapped to no CLR property.
        builder.Property<int>("Seq")
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        var reference = builder.Property<string>("Ref")
            .IsRequired()
            .HasComputedColumnSql(
                "('WRK-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))",
                stored: true)
            .ValueGeneratedOnAddOrUpdate();

        reference.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        reference.Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

        builder.HasIndex("Ref")
            .IsUnique()
            .HasDatabaseName("UQ_WorkItem_Ref");

        // ADR-014 hot-path index.
        builder.HasIndex(workItem => new { workItem.ProjectId, workItem.Status })
            .HasDatabaseName("IX_WorkItem_ProjectId_Status");
    }
}
