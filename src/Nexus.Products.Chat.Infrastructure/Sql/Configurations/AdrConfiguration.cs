using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Products.Chat.Domain.Adr;
using Nexus.Products.Chat.Domain.Knowledge;
using Nexus.Products.Chat.Infrastructure.Sql.Conventions;

namespace Nexus.Products.Chat.Infrastructure.Sql.Configurations;

public sealed class AdrConfiguration : IEntityTypeConfiguration<Adr>
{
    public void Configure(EntityTypeBuilder<Adr> builder)
    {
        builder.ToTable("Adr", "knowledge");

        builder.HasKey(adr => adr.Id);

        builder.Property(adr => adr.Id)
            .HasConversion(StronglyTypedIdConverters.AdrId)
            .ValueGeneratedNever();

        // Adr has exactly one FK - KnowledgeId (no SupersedesAdrId, no WorkspaceId/ProjectId;
        // ADR-014's text claiming more is wrong for this stage, verified against live source).
        builder.HasOne<Knowledge>()
            .WithMany()
            .HasForeignKey(adr => adr.KnowledgeId)
            // Knowledge is the owning parent - deleting a Knowledge entry takes its Adrs.
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(adr => adr.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(adr => adr.Decision)
            .HasColumnType("nvarchar(max)");

        builder.Property(adr => adr.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(adr => adr.CreatedAt)
            .IsRequired();

        // The Adr domain entity exposes no Reference property (HARD constraint: no Domain
        // edits). ADR-014's schema map still gives knowledge.Adr an ADR- ref for external
        // tracing, so the column is an EF shadow property fed by the same Seq identity
        // pattern Workspace uses, mapped to no CLR property.
        builder.Property<int>("Seq")
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        var reference = builder.Property<string>("Ref")
            .IsRequired()
            .HasComputedColumnSql(
                "('ADR-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))",
                stored: true)
            .ValueGeneratedOnAddOrUpdate();

        reference.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        reference.Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

        builder.HasIndex("Ref")
            .IsUnique()
            .HasDatabaseName("UQ_Adr_Ref");

        // EF auto-creates an index on the FK column (IX_Adr_KnowledgeId) to back the
        // KnowledgeId lookups from IAdrRepository.ListByKnowledgeIdsAsync.
    }
}
