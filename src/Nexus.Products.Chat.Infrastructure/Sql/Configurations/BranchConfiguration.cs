using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Products.Chat.Domain.Branch;
using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Infrastructure.Sql.Conventions;

namespace Nexus.Products.Chat.Infrastructure.Sql.Configurations;

public sealed class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branch", "session");

        builder.HasKey(branch => branch.Id);

        builder.Property(branch => branch.Id)
            .HasConversion(StronglyTypedIdConverters.BranchId)
            .ValueGeneratedNever();

        // Branch has exactly one FK - ConversationId. No self-referencing FK exists here
        // (ADR-014's text claiming one does is wrong for the current source, verified
        // directly against Domain/Branch/Branch.cs).
        builder.HasOne<Conversation>()
            .WithMany()
            .HasForeignKey(branch => branch.ConversationId)
            // Conversation is the owning parent - deleting a Conversation takes its Branches.
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(branch => branch.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(branch => branch.Description)
            .HasColumnType("nvarchar(max)");

        builder.Property(branch => branch.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(branch => branch.CreatedAt)
            .IsRequired();

        // ADR-014 hot-path index; also backs the FK.
        builder.HasIndex(branch => branch.ConversationId)
            .HasDatabaseName("IX_Branch_ConversationId");

        // The Branch domain entity exposes no Reference property (HARD constraint: no Domain
        // edits). ADR-014's schema map still gives session.Branch a BRN- ref for external
        // tracing, so the column is an EF shadow property fed by the same Seq identity
        // pattern Workspace uses, mapped to no CLR property.
        builder.Property<int>("Seq")
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        var reference = builder.Property<string>("Ref")
            .IsRequired()
            .HasComputedColumnSql(
                "('BRN-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))",
                stored: true)
            .ValueGeneratedOnAddOrUpdate();

        reference.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        reference.Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

        builder.HasIndex("Ref")
            .IsUnique()
            .HasDatabaseName("UQ_Branch_Ref");
    }
}
