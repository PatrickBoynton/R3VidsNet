using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace R3vids.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVideoStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPlayTime",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "LastPlayed",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "PlayCount",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Played",
                table: "Videos");

            migrationBuilder.AddColumn<Guid>(
                name: "VideoStatusId",
                table: "Videos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "VideoStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Played = table.Column<bool>(type: "boolean", nullable: false),
                    CurrentPlayTime = table.Column<decimal>(type: "numeric", nullable: false),
                    PlayCount = table.Column<int>(type: "integer", nullable: false),
                    IsWatchLater = table.Column<bool>(type: "boolean", nullable: false),
                    RandomVideoId = table.Column<Guid>(type: "uuid", nullable: true),
                    CurrentVideoId = table.Column<Guid>(type: "uuid", nullable: true),
                    PreviousVideoId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastPlayed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VideoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoStatus_Videos_CurrentVideoId",
                        column: x => x.CurrentVideoId,
                        principalTable: "Videos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VideoStatus_Videos_PreviousVideoId",
                        column: x => x.PreviousVideoId,
                        principalTable: "Videos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VideoStatus_Videos_RandomVideoId",
                        column: x => x.RandomVideoId,
                        principalTable: "Videos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VideoStatus_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoStatus_CurrentVideoId",
                table: "VideoStatus",
                column: "CurrentVideoId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoStatus_PreviousVideoId",
                table: "VideoStatus",
                column: "PreviousVideoId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoStatus_RandomVideoId",
                table: "VideoStatus",
                column: "RandomVideoId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoStatus_VideoId",
                table: "VideoStatus",
                column: "VideoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoStatus");

            migrationBuilder.DropColumn(
                name: "VideoStatusId",
                table: "Videos");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentPlayTime",
                table: "Videos",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPlayed",
                table: "Videos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayCount",
                table: "Videos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Played",
                table: "Videos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
