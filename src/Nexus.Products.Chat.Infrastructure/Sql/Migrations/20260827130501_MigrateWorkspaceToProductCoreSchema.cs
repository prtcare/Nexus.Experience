using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexus.Products.Chat.Infrastructure.Sql.Migrations
{
    /// <inheritdoc />
    public partial class MigrateWorkspaceToProductCoreSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "product_core");

            migrationBuilder.RenameTable(
                name: "Workspace",
                schema: "org",
                newName: "Workspace",
                newSchema: "product_core");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "org");

            migrationBuilder.RenameTable(
                name: "Workspace",
                schema: "product_core",
                newName: "Workspace",
                newSchema: "org");
        }
    }
}
