using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace R3vids.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVideoVideoStatusRelationship : Migration
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

            migrationBuilder.DropForeignKey(
                name: "FK_VideoStatus_Videos_VideoId",
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

            migrationBuilder.DropIndex(
                name: "IX_VideoStatus_VideoId",
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

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "VideoStatus");

            migrationBuilder.AddColumn<Guid>(
                name: "VideoStatusId",
                table: "Videos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "VideoStatusId1",
                table: "Videos",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VideoStatusId2",
                table: "Videos",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VideoStatusId3",
                table: "Videos",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_VideoStatusId1",
                table: "Videos",
                column: "VideoStatusId1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_VideoStatusId2",
                table: "Videos",
                column: "VideoStatusId2",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_VideoStatusId3",
                table: "Videos",
                column: "VideoStatusId3",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_VideoStatus_VideoStatusId1",
                table: "Videos",
                column: "VideoStatusId1",
                principalTable: "VideoStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_VideoStatus_VideoStatusId2",
                table: "Videos",
                column: "VideoStatusId2",
                principalTable: "VideoStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_VideoStatus_VideoStatusId3",
                table: "Videos",
                column: "VideoStatusId3",
                principalTable: "VideoStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoStatus_Videos_Id",
                table: "VideoStatus",
                column: "Id",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_VideoStatus_VideoStatusId1",
                table: "Videos");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_VideoStatus_VideoStatusId2",
                table: "Videos");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_VideoStatus_VideoStatusId3",
                table: "Videos");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoStatus_Videos_Id",
                table: "VideoStatus");

            migrationBuilder.DropIndex(
                name: "IX_Videos_VideoStatusId1",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_VideoStatusId2",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_VideoStatusId3",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "VideoStatusId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "VideoStatusId1",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "VideoStatusId2",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "VideoStatusId3",
                table: "Videos");

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

            migrationBuilder.AddColumn<Guid>(
                name: "VideoId",
                table: "VideoStatus",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.AddForeignKey(
                name: "FK_VideoStatus_Videos_VideoId",
                table: "VideoStatus",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
