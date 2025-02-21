using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedDealsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CharterDealId",
                table: "Yachts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CharterDeals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Departure = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Arrival = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StrIncluded = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StrNotIncluded = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharterDeals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DealDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromLat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FromLong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ToLat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ToLong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CharterDealId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealDays_CharterDeals_CharterDealId",
                        column: x => x.CharterDealId,
                        principalTable: "CharterDeals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Yachts_CharterDealId",
                table: "Yachts",
                column: "CharterDealId");

            migrationBuilder.CreateIndex(
                name: "IX_DealDays_CharterDealId",
                table: "DealDays",
                column: "CharterDealId");

            migrationBuilder.AddForeignKey(
                name: "FK_Yachts_CharterDeals_CharterDealId",
                table: "Yachts",
                column: "CharterDealId",
                principalTable: "CharterDeals",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Yachts_CharterDeals_CharterDealId",
                table: "Yachts");

            migrationBuilder.DropTable(
                name: "DealDays");

            migrationBuilder.DropTable(
                name: "CharterDeals");

            migrationBuilder.DropIndex(
                name: "IX_Yachts_CharterDealId",
                table: "Yachts");

            migrationBuilder.DropColumn(
                name: "CharterDealId",
                table: "Yachts");
        }
    }
}
