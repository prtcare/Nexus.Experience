using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.ProductCore.Scope.Subproject;
using Nexus.Products.Chat.Infrastructure.Sql.Conventions;

namespace Nexus.Products.Chat.Infrastructure.Sql.Configurations;

public sealed class SubprojectConfiguration : IEntityTypeConfiguration<Subproject>
{
    public void Configure(EntityTypeBuilder<Subproject> builder)
    {
        builder.ToTable("Subproject", "project");

        builder.HasKey(subproject => subproject.Id);

        builder.Property(subproject => subproject.Id)
            .HasConversion(StronglyTypedIdConverters.SubprojectId)
            .ValueGeneratedNever();

        builder.Property(subproject => subproject.ProjectId)
            .HasConversion(StronglyTypedIdConverters.ProjectId)
            .IsRequired();

        builder.Property(subproject => subproject.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(subproject => subproject.Description)
            .HasColumnType("nvarchar(max)");

        builder.Property(subproject => subproject.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(subproject => subproject.CreatedAt)
            .IsRequired();

        // Mirrors ProjectConfiguration exactly: the Layer 06 Subproject entity carries a
        // Reference property that ADR-014 maps to the computed SPR- ref column. The reference
        // prefix is SPR- per Subproject.cs's own doc comment (deliberately distinct from
        // Developer's SUB- Subtask prefix). The Dataverse "narrow slice" mapper never
        // populates Reference, but EF reads it back from the store-generated Ref column -
        // matching how Workspace and Project behave.
        builder.Property<int>("Seq")
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        var reference = builder.Property(subproject => subproject.Reference)
            .HasColumnName("Ref")
            .HasComputedColumnSql(
                "('SPR-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))",
                stored: true)
            .ValueGeneratedOnAddOrUpdate();

        reference.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        reference.Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

        builder.HasIndex(subproject => subproject.Reference)
            .IsUnique()
            .HasDatabaseName("UQ_Subproject_Ref");

        builder.HasIndex(subproject => subproject.ProjectId)
            .HasDatabaseName("IX_Subproject_ProjectId");
    }
}
