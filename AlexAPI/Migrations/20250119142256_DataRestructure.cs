using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class DataRestructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summer",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "Winter",
                table: "Prices");

            migrationBuilder.AddColumn<bool>(
                name: "OnSale",
                table: "Yachts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceValue",
                table: "Yachts",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnSale",
                table: "Yachts");

            migrationBuilder.DropColumn(
                name: "PriceValue",
                table: "Yachts");

            migrationBuilder.AddColumn<decimal>(
                name: "Summer",
                table: "Prices",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Winter",
                table: "Prices",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
