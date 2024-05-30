using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace R3vids.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoNavigation_Videos_CurrentVideoId",
                table: "VideoNavigation");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoNavigation_Videos_PreviousVideoId",
                table: "VideoNavigation");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoNavigation_Videos_RandomVideoId",
                table: "VideoNavigation");

            migrationBuilder.DropIndex(
                name: "IX_VideoNavigation_CurrentVideoId",
                table: "VideoNavigation");

            migrationBuilder.DropIndex(
                name: "IX_VideoNavigation_PreviousVideoId",
                table: "VideoNavigation");

            migrationBuilder.DropIndex(
                name: "IX_VideoNavigation_RandomVideoId",
                table: "VideoNavigation");

            migrationBuilder.DropColumn(
                name: "CurrentVideoId",
                table: "VideoNavigation");

            migrationBuilder.RenameColumn(
                name: "RandomVideoId",
                table: "VideoNavigation",
                newName: "PreviousVideo");

            migrationBuilder.RenameColumn(
                name: "PreviousVideoId",
                table: "VideoNavigation",
                newName: "CurrentVideo");

            migrationBuilder.AddColumn<Guid>(
                name: "VideoNavigationId",
                table: "Videos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Videos_VideoNavigationId",
                table: "Videos",
                column: "VideoNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_VideoNavigation_VideoNavigationId",
                table: "Videos",
                column: "VideoNavigationId",
                principalTable: "VideoNavigation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_VideoNavigation_VideoNavigationId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_VideoNavigationId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "VideoNavigationId",
                table: "Videos");

            migrationBuilder.RenameColumn(
                name: "PreviousVideo",
                table: "VideoNavigation",
                newName: "RandomVideoId");

            migrationBuilder.RenameColumn(
                name: "CurrentVideo",
                table: "VideoNavigation",
                newName: "PreviousVideoId");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentVideoId",
                table: "VideoNavigation",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VideoNavigation_CurrentVideoId",
                table: "VideoNavigation",
                column: "CurrentVideoId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoNavigation_PreviousVideoId",
                table: "VideoNavigation",
                column: "PreviousVideoId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoNavigation_RandomVideoId",
                table: "VideoNavigation",
                column: "RandomVideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoNavigation_Videos_CurrentVideoId",
                table: "VideoNavigation",
                column: "CurrentVideoId",
                principalTable: "Videos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoNavigation_Videos_PreviousVideoId",
                table: "VideoNavigation",
                column: "PreviousVideoId",
                principalTable: "Videos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoNavigation_Videos_RandomVideoId",
                table: "VideoNavigation",
                column: "RandomVideoId",
                principalTable: "Videos",
                principalColumn: "Id");
        }
    }
}
