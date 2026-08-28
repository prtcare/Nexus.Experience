using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Domain.Session;
using Nexus.Products.Chat.Infrastructure.Sql.Conventions;

namespace Nexus.Products.Chat.Infrastructure.Sql.Configurations;

public sealed class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("Session", "session");

        builder.HasKey(session => session.Id);

        builder.Property(session => session.Id)
            .HasConversion(StronglyTypedIdConverters.SessionId)
            .ValueGeneratedNever();

        // Session has exactly one FK - ConversationId. No self-referencing FK exists here
        // (ADR-014's text claiming one does is wrong for the current source, verified
        // directly against Domain/Session/Session.cs).
        builder.HasOne<Conversation>()
            .WithMany()
            .HasForeignKey(session => session.ConversationId)
            // Conversation is the owning parent - deleting a Conversation takes its Sessions.
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(session => session.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(session => session.StartedAt)
            .IsRequired();

        // EndedAt is a nullable DateTimeOffset? - optional by convention, like
        // Conversation.LastMessageOn.
        builder.Property(session => session.EndedAt);

        // ADR-014 hot-path index; also backs the FK.
        builder.HasIndex(session => session.ConversationId)
            .HasDatabaseName("IX_Session_ConversationId");

        // The Session domain entity exposes no Reference property (HARD constraint: no Domain
        // edits). ADR-014's schema map still gives session.Session an SES- ref for external
        // tracing, so the column is an EF shadow property fed by the same Seq identity
        // pattern Workspace uses, mapped to no CLR property.
        builder.Property<int>("Seq")
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        var reference = builder.Property<string>("Ref")
            .IsRequired()
            .HasComputedColumnSql(
                "('SES-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))",
                stored: true)
            .ValueGeneratedOnAddOrUpdate();

        reference.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        reference.Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

        builder.HasIndex("Ref")
            .IsUnique()
            .HasDatabaseName("UQ_Session_Ref");
    }
}
