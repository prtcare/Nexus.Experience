using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Products.Chat.Domain.Branch;
using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Domain.Snapshot;
using Nexus.Products.Chat.Infrastructure.Sql.Conventions;

namespace Nexus.Products.Chat.Infrastructure.Sql.Configurations;

public sealed class SnapshotConfiguration : IEntityTypeConfiguration<Snapshot>
{
    public void Configure(EntityTypeBuilder<Snapshot> builder)
    {
        builder.ToTable("Snapshot", "session");

        builder.HasKey(snapshot => snapshot.Id);

        builder.Property(snapshot => snapshot.Id)
            .HasConversion(StronglyTypedIdConverters.SnapshotId)
            .ValueGeneratedNever();

        // Snapshot has TWO FKs (verified directly against Domain/Snapshot/Snapshot.cs):
        // BranchId -> Branch and ConversationId -> Conversation. BranchId is the owning
        // path - deleting a Branch takes its Snapshots.
        builder.HasOne<Branch>()
            .WithMany()
            .HasForeignKey(snapshot => snapshot.BranchId)
            .OnDelete(DeleteBehavior.Cascade);

        // Snapshot.ConversationId is a SECOND reference to the same ultimate ancestor the
        // Branch path already reaches. If this FK were also Cascade, deleting a Conversation
        // would reach the Snapshot table twice (Conversation -> Branch -> Snapshot, and
        // Conversation -> Snapshot directly) - a converging cascade path that SQL Server
        // rejects with error 1785 (multiple cascade paths to the same table). Restrict
        // deliberately breaks that second path: a Conversation is still fully removed because
        // its Branches cascade first (taking their Snapshots along via Snapshot.BranchId)
        // before the direct Snapshot rows are ever inspected. This is the one non-Cascade FK
        // in the stage, and it must stay Restrict.
        builder.HasOne<Conversation>()
            .WithMany()
            .HasForeignKey(snapshot => snapshot.ConversationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(snapshot => snapshot.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(snapshot => snapshot.State)
            .HasColumnType("nvarchar(max)");

        builder.Property(snapshot => snapshot.CreatedAt)
            .IsRequired();

        // ADR-014 hot-path index; also backs the owning FK.
        builder.HasIndex(snapshot => snapshot.BranchId)
            .HasDatabaseName("IX_Snapshot_BranchId");

        // The Snapshot domain entity exposes no Reference property (HARD constraint: no
        // Domain edits). ADR-014's schema map still gives session.Snapshot an SNP- ref for
        // external tracing, so the column is an EF shadow property fed by the same Seq
        // identity pattern Workspace uses, mapped to no CLR property.
        builder.Property<int>("Seq")
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        var reference = builder.Property<string>("Ref")
            .IsRequired()
            .HasComputedColumnSql(
                "('SNP-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))",
                stored: true)
            .ValueGeneratedOnAddOrUpdate();

        reference.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        reference.Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

        builder.HasIndex("Ref")
            .IsUnique()
            .HasDatabaseName("UQ_Snapshot_Ref");
    }
}
