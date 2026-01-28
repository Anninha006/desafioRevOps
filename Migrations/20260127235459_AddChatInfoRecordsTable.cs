using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REVOPS.DevChallenge.Migrations
{
    /// <inheritdoc />
    public partial class AddChatInfoRecordsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatInfoRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChatId = table.Column<string>(type: "TEXT", nullable: true),
                    SearchedAtUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsAnyAttendantAssigned = table.Column<bool>(type: "INTEGER", nullable: false),
                    AssignedMemberId = table.Column<string>(type: "TEXT", nullable: true),
                    IsOpen = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsWaiting = table.Column<bool>(type: "INTEGER", nullable: false),
                    WaitingSinceUTC = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ChatCreatedAtUTC = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ClosedAtUTC = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ContactId = table.Column<string>(type: "TEXT", nullable: true),
                    ContactName = table.Column<string>(type: "TEXT", nullable: true),
                    ContactPhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    ContactProfilePictureUrl = table.Column<string>(type: "TEXT", nullable: true),
                    ContactIsBlocked = table.Column<bool>(type: "INTEGER", nullable: false),
                    ChannelId = table.Column<string>(type: "TEXT", nullable: true),
                    ChannelName = table.Column<string>(type: "TEXT", nullable: true),
                    SectorId = table.Column<string>(type: "TEXT", nullable: true),
                    SectorName = table.Column<string>(type: "TEXT", nullable: true),
                    ChatTags = table.Column<string>(type: "TEXT", nullable: true),
                    ContactTags = table.Column<string>(type: "TEXT", nullable: true),
                    TotalUnread = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalAIResponses = table.Column<int>(type: "INTEGER", nullable: false),
                    LastMessageContent = table.Column<string>(type: "TEXT", nullable: true),
                    LastMessageAtUTC = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastMessageSource = table.Column<string>(type: "TEXT", nullable: true),
                    ActiveBotName = table.Column<string>(type: "TEXT", nullable: true),
                    ActiveBotStatus = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatInfoRecords", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatInfoRecords_ChatId",
                table: "ChatInfoRecords",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatInfoRecords_SearchedAtUTC",
                table: "ChatInfoRecords",
                column: "SearchedAtUTC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatInfoRecords");
        }
    }
}
