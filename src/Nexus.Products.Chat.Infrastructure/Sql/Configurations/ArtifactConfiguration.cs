using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Products.Chat.Domain.Artifact;
using Nexus.Products.Chat.Domain.WorkItem;
using Nexus.Products.Chat.Infrastructure.Sql.Conventions;

namespace Nexus.Products.Chat.Infrastructure.Sql.Configurations;

public sealed class ArtifactConfiguration : IEntityTypeConfiguration<Artifact>
{
    public void Configure(EntityTypeBuilder<Artifact> builder)
    {
        builder.ToTable("Artifact", "work");

        builder.HasKey(artifact => artifact.Id);

        builder.Property(artifact => artifact.Id)
            .HasConversion(StronglyTypedIdConverters.ArtifactId)
            .ValueGeneratedNever();

        // Artifact has exactly one FK - WorkItemId (ADR-014's text claiming more is wrong for
        // this stage, verified against live source).
        builder.HasOne<WorkItem>()
            .WithMany()
            .HasForeignKey(artifact => artifact.WorkItemId)
            // WorkItem is the owning parent - deleting a WorkItem takes its Artifacts.
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(artifact => artifact.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(artifact => artifact.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(artifact => artifact.Content)
            .HasColumnType("nvarchar(max)");

        builder.Property(artifact => artifact.CreatedAt)
            .IsRequired();

        // ADR-014 hot-path index; also backs the FK.
        builder.HasIndex(artifact => artifact.WorkItemId)
            .HasDatabaseName("IX_Artifact_WorkItemId");

        // The Artifact domain entity exposes no Reference property (HARD constraint: no
        // Domain edits). ADR-014's schema map still gives work.Artifact an ART- ref for
        // external tracing, so the column is an EF shadow property fed by the same Seq
        // identity pattern Workspace uses, mapped to no CLR property.
        builder.Property<int>("Seq")
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        var reference = builder.Property<string>("Ref")
            .IsRequired()
            .HasComputedColumnSql(
                "('ART-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))",
                stored: true)
            .ValueGeneratedOnAddOrUpdate();

        reference.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        reference.Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

        builder.HasIndex("Ref")
            .IsUnique()
            .HasDatabaseName("UQ_Artifact_Ref");
    }
}
