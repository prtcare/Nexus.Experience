using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexus.Products.Chat.Infrastructure.Sql.Migrations
{
    /// <inheritdoc />
    public partial class Stage2cBranchSnapshotSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "session");

            migrationBuilder.CreateTable(
                name: "Branch",
                schema: "session",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Ref = table.Column<string>(type: "nvarchar(450)", nullable: false, computedColumnSql: "('BRN-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))", stored: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branch_Conversation_ConversationId",
                        column: x => x.ConversationId,
                        principalSchema: "conversation",
                        principalTable: "Conversation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Session",
                schema: "session",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Ref = table.Column<string>(type: "nvarchar(450)", nullable: false, computedColumnSql: "('SES-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))", stored: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Session_Conversation_ConversationId",
                        column: x => x.ConversationId,
                        principalSchema: "conversation",
                        principalTable: "Conversation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Snapshot",
                schema: "session",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Ref = table.Column<string>(type: "nvarchar(450)", nullable: false, computedColumnSql: "('SNP-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))", stored: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Snapshot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Snapshot_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "session",
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Snapshot_Conversation_ConversationId",
                        column: x => x.ConversationId,
                        principalSchema: "conversation",
                        principalTable: "Conversation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branch_ConversationId",
                schema: "session",
                table: "Branch",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "UQ_Branch_Ref",
                schema: "session",
                table: "Branch",
                column: "Ref",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Session_ConversationId",
                schema: "session",
                table: "Session",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "UQ_Session_Ref",
                schema: "session",
                table: "Session",
                column: "Ref",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Snapshot_BranchId",
                schema: "session",
                table: "Snapshot",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Snapshot_ConversationId",
                schema: "session",
                table: "Snapshot",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "UQ_Snapshot_Ref",
                schema: "session",
                table: "Snapshot",
                column: "Ref",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Session",
                schema: "session");

            migrationBuilder.DropTable(
                name: "Snapshot",
                schema: "session");

            migrationBuilder.DropTable(
                name: "Branch",
                schema: "session");
        }
    }
}
