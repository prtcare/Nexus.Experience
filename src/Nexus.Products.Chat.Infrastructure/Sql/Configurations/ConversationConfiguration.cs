using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Infrastructure.Sql.Conventions;

namespace Nexus.Products.Chat.Infrastructure.Sql.Configurations;

public sealed class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.ToTable("Conversation", "conversation");

        builder.HasKey(conversation => conversation.Id);

        builder.Property(conversation => conversation.Id)
            .HasConversion(StronglyTypedIdConverters.ConversationId)
            .ValueGeneratedNever();

        // Conversation keys on Chat's own opaque ProjectId/WorkspaceId structs (AGENTS.md
        // boundary rule) - not Nexus.ProductCore.Scope's. SendChatHandler converts at the
        // one scope boundary; the Sql repositories must keep Chat-local ids as-is.
        builder.Property(conversation => conversation.ProjectId)
            .HasConversion(StronglyTypedIdConverters.ChatProjectId)
            .IsRequired();

        builder.Property(conversation => conversation.WorkspaceId)
            .HasConversion(StronglyTypedIdConverters.ChatWorkspaceId)
            .IsRequired();

        builder.Property(conversation => conversation.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(conversation => conversation.Description)
            .HasColumnType("nvarchar(max)");

        builder.Property(conversation => conversation.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(conversation => conversation.Visibility)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(conversation => conversation.CreatedAt)
            .IsRequired();

        builder.Property(conversation => conversation.LastMessageOn);

        builder.Property(conversation => conversation.Status)
            .HasConversion<int>()
            .IsRequired();

        // The Conversation domain entity exposes no Reference property (HARD constraint: no
        // Domain edits). ADR-014's schema map still gives conversation.Conversation a CON-
        // ref for external tracing, so the column is an EF shadow property fed by the same
        // Seq identity pattern Workspace uses, mapped to no CLR property.
        builder.Property<int>("Seq")
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        var reference = builder.Property<string>("Ref")
            // The computed CON- ref is never NULL (Seq is an identity seed), so model it
            // non-nullable like Workspace/Project - a nullable unique index would need a
            // [Ref] IS NOT NULL filter, which SQL Server rejects on a computed column
            // (error 10609).
            .IsRequired()
            .HasComputedColumnSql(
                "('CON-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))",
                stored: true)
            .ValueGeneratedOnAddOrUpdate();

        reference.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        reference.Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

        builder.HasIndex("Ref")
            .IsUnique()
            .HasDatabaseName("UQ_Conversation_Ref");

        builder.HasIndex(conversation => conversation.ProjectId)
            .HasDatabaseName("IX_Conversation_ProjectId");
    }
}
