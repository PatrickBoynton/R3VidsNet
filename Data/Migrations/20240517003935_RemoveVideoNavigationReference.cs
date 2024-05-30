using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace R3vids.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveVideoNavigationReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
