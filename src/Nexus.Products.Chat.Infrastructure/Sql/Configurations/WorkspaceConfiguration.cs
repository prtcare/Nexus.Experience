using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Products.Chat.Infrastructure.Sql.Conventions;
using DomainWorkspace = Nexus.ProductCore.Scope.Workspace.Workspace;

namespace Nexus.Products.Chat.Infrastructure.Sql.Configurations;

public sealed class WorkspaceConfiguration : IEntityTypeConfiguration<DomainWorkspace>
{
    public void Configure(EntityTypeBuilder<DomainWorkspace> builder)
    {
        builder.ToTable("Workspace", "org");

        builder.HasKey(workspace => workspace.Id);

        builder.Property(workspace => workspace.Id)
            .HasConversion(StronglyTypedIdConverters.WorkspaceId)
            .ValueGeneratedNever();

        builder.Property(workspace => workspace.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(workspace => workspace.Owner)
            .HasMaxLength(100);

        builder.Property(workspace => workspace.Description)
            .HasColumnType("nvarchar(max)");

        builder.Property(workspace => workspace.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(workspace => workspace.CreatedAt)
            .IsRequired();

        builder.Property<int>("Seq")
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        var reference = builder.Property(w => w.Reference)
            .HasColumnName("Ref")
            .HasComputedColumnSql(
                "('WKS-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))",
                stored: true)
            .ValueGeneratedOnAddOrUpdate();

        reference.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        reference.Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

        builder.HasIndex(w => w.Reference)
            .IsUnique()
            .HasDatabaseName("UQ_Workspace_Ref");
    }
}
