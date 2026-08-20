using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Products.Chat.Infrastructure.Sql.Conventions;
using DomainWorkspace = Nexus.Products.Chat.Domain.Workspace.Workspace;

namespace Nexus.Products.Chat.Infrastructure.Sql.Configurations;

public sealed class WorkspaceConfiguration : IEntityTypeConfiguration<DomainWorkspace>
{
    public void Configure(EntityTypeBuilder<DomainWorkspace> builder)
    {
        builder.ToTable("Workspace");

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
    }
}
