using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexus.Products.Chat.Infrastructure.Sql.Migrations
{
    /// <inheritdoc />
    public partial class Stage2aProjectConversation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "conversation");

            migrationBuilder.EnsureSchema(
                name: "project");

            migrationBuilder.CreateTable(
                name: "Conversation",
                schema: "conversation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Visibility = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastMessageOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Ref = table.Column<string>(type: "nvarchar(450)", nullable: false, computedColumnSql: "('CON-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))", stored: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversationMessage",
                schema: "conversation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                schema: "project",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Ref = table.Column<string>(type: "nvarchar(450)", nullable: false, computedColumnSql: "('PRJ-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))", stored: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_ProjectId",
                schema: "conversation",
                table: "Conversation",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "UQ_Conversation_Ref",
                schema: "conversation",
                table: "Conversation",
                column: "Ref",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMessage_ConversationId_CreatedOn",
                schema: "conversation",
                table: "ConversationMessage",
                columns: new[] { "ConversationId", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Project_WorkspaceId",
                schema: "project",
                table: "Project",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "UQ_Project_Ref",
                schema: "project",
                table: "Project",
                column: "Ref",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conversation",
                schema: "conversation");

            migrationBuilder.DropTable(
                name: "ConversationMessage",
                schema: "conversation");

            migrationBuilder.DropTable(
                name: "Project",
                schema: "project");
        }
    }
}
