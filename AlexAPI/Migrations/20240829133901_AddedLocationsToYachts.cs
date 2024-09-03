using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedLocationsToYachts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YachtGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Yachts_YachtGuid",
                        column: x => x.YachtGuid,
                        principalTable: "Yachts",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_YachtGuid",
                table: "Locations",
                column: "YachtGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
