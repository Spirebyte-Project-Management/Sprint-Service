using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Spirebyte.Services.Sprints.Infrastructure.Migrations
{
    public partial class Addoutboxentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InboxMessages",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ProcessedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    OriginatedMessageId = table.Column<string>(nullable: true),
                    CorrelationId = table.Column<string>(nullable: true),
                    SpanContext = table.Column<string>(nullable: true),
                    HeadersJson = table.Column<string>(nullable: true),
                    MessageType = table.Column<string>(nullable: true),
                    MessageContextType = table.Column<string>(nullable: true),
                    MessageJson = table.Column<string>(nullable: true),
                    MessageContextJson = table.Column<string>(nullable: true),
                    SerializedMessage = table.Column<string>(nullable: true),
                    SerializedMessageContext = table.Column<string>(nullable: true),
                    SentAt = table.Column<DateTime>(nullable: false),
                    ProcessedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InboxMessages");

            migrationBuilder.DropTable(
                name: "OutboxMessages");
        }
    }
}
