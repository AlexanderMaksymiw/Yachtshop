using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class LinkingItinerariesToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Yachts_AspNetUsers_UserId",
                table: "Yachts");

            migrationBuilder.DropIndex(
                name: "IX_Yachts_UserId",
                table: "Yachts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Yachts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DealDays");

            migrationBuilder.DropColumn(
                name: "is2FA",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "secretKey2FA",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "UserItineraryId",
                table: "Yachts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "DealDays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserItinerary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Departure = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Arrival = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StrIncluded = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StrNotIncluded = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserItinerary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserItinerary_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItineraryDay",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromLat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FromLong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ToLat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ToLong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserItineraryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItineraryDay", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ItineraryDay_UserItinerary_UserItineraryId",
                        column: x => x.UserItineraryId,
                        principalTable: "UserItinerary",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Yachts_UserItineraryId",
                table: "Yachts",
                column: "UserItineraryId");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryDay_UserItineraryId",
                table: "ItineraryDay",
                column: "UserItineraryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserItinerary_UserId",
                table: "UserItinerary",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Yachts_UserItinerary_UserItineraryId",
                table: "Yachts",
                column: "UserItineraryId",
                principalTable: "UserItinerary",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Yachts_UserItinerary_UserItineraryId",
                table: "Yachts");

            migrationBuilder.DropTable(
                name: "ItineraryDay");

            migrationBuilder.DropTable(
                name: "UserItinerary");

            migrationBuilder.DropIndex(
                name: "IX_Yachts_UserItineraryId",
                table: "Yachts");

            migrationBuilder.DropColumn(
                name: "UserItineraryId",
                table: "Yachts");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "DealDays");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Yachts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DealDays",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is2FA",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "secretKey2FA",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Yachts_UserId",
                table: "Yachts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Yachts_AspNetUsers_UserId",
                table: "Yachts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
