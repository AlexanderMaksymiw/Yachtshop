using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedPriceNumericToDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceNumeric",
                table: "Details",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE Details
                SET PriceNumeric = TRY_CAST(REPLACE(REPLACE(Price, '$', ''), ',', '') AS DECIMAL(18, 2))
                WHERE TRY_CAST(REPLACE(REPLACE(Price, '$', ''), ',', '') AS DECIMAL(18, 2)) IS NOT NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceNumeric",
                table: "Details");
        }
    }
}
