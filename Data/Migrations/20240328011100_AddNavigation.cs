using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace R3vids.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VideoNavigation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RandomVideoId = table.Column<Guid>(type: "uuid", nullable: true),
                    CurrentVideoId = table.Column<Guid>(type: "uuid", nullable: true),
                    PreviousVideoId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoNavigation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoNavigation_Videos_CurrentVideoId",
                        column: x => x.CurrentVideoId,
                        principalTable: "Videos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VideoNavigation_Videos_PreviousVideoId",
                        column: x => x.PreviousVideoId,
                        principalTable: "Videos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VideoNavigation_Videos_RandomVideoId",
                        column: x => x.RandomVideoId,
                        principalTable: "Videos",
                        principalColumn: "Id");
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoNavigation");
        }
    }
}
