using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace R3vids.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSeries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SeriesId",
                table: "Videos",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SeriesName = table.Column<string>(type: "text", nullable: false),
                    GenreCode = table.Column<string>(type: "text", nullable: false),
                    NumberOfVideos = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Videos_SeriesId",
                table: "Videos",
                column: "SeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Series_SeriesId",
                table: "Videos",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Series_SeriesId",
                table: "Videos");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Videos_SeriesId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "Videos");
        }
    }
}
