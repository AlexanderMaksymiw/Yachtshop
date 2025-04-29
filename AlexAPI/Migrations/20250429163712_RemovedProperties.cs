using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemovedProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StrIncluded",
                table: "UserItinerary");

            migrationBuilder.DropColumn(
                name: "StrNotIncluded",
                table: "UserItinerary");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StrIncluded",
                table: "UserItinerary",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StrNotIncluded",
                table: "UserItinerary",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
