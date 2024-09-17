using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNumericFieldsToSpecification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GrossTonnageNumeric",
                table: "Specifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxSpeedNumeric",
                table: "Specifications",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE Specifications
                SET GrossTonnageNumeric = TRY_CAST(SUBSTRING(GrossTonnage, 1, CHARINDEX(' ', GrossTonnage + ' ') - 1) AS INT)
                WHERE ISNUMERIC(SUBSTRING(GrossTonnage, 1, CHARINDEX(' ', GrossTonnage + ' ') - 1)) = 1;

                UPDATE Specifications
                SET MaxSpeedNumeric = TRY_CAST(SUBSTRING(MaxSpeed, 1, CHARINDEX(' ', MaxSpeed + ' ') - 1) AS DECIMAL(5,2))
                WHERE ISNUMERIC(SUBSTRING(MaxSpeed, 1, CHARINDEX(' ', MaxSpeed + ' ') - 1)) = 1;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GrossTonnageNumeric",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "MaxSpeedNumeric",
                table: "Specifications");
        }
    }
}
