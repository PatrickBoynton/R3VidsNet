using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace R3vids.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNavigationFromVideoStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoStatus_Videos_CurrentVideoId",
                table: "VideoStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoStatus_Videos_PreviousVideoId",
                table: "VideoStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoStatus_Videos_RandomVideoId",
                table: "VideoStatus");

            migrationBuilder.DropIndex(
                name: "IX_VideoStatus_CurrentVideoId",
                table: "VideoStatus");

            migrationBuilder.DropIndex(
                name: "IX_VideoStatus_PreviousVideoId",
                table: "VideoStatus");

            migrationBuilder.DropIndex(
                name: "IX_VideoStatus_RandomVideoId",
                table: "VideoStatus");

            migrationBuilder.DropColumn(
                name: "CurrentVideoId",
                table: "VideoStatus");

            migrationBuilder.DropColumn(
                name: "PreviousVideoId",
                table: "VideoStatus");

            migrationBuilder.DropColumn(
                name: "RandomVideoId",
                table: "VideoStatus");

            migrationBuilder.AddColumn<Guid>(
                name: "VideoId",
                table: "VideoStatus",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "VideoStatus");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentVideoId",
                table: "VideoStatus",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PreviousVideoId",
                table: "VideoStatus",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RandomVideoId",
                table: "VideoStatus",
                type: "uuid",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_VideoStatus_Videos_CurrentVideoId",
                table: "VideoStatus",
                column: "CurrentVideoId",
                principalTable: "Videos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoStatus_Videos_PreviousVideoId",
                table: "VideoStatus",
                column: "PreviousVideoId",
                principalTable: "Videos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoStatus_Videos_RandomVideoId",
                table: "VideoStatus",
                column: "RandomVideoId",
                principalTable: "Videos",
                principalColumn: "Id");
        }
    }
}
