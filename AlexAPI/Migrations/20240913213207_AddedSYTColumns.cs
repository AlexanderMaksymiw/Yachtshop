using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedSYTColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MaxSpeed",
                table: "Specifications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "FuelCapacity",
                table: "Specifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Port",
                table: "Specifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropulsionType",
                table: "Specifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotalPowerOutput",
                table: "Specifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Specifications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FuelCapacity",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "Port",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "PropulsionType",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "TotalPowerOutput",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Specifications");

            migrationBuilder.AlterColumn<int>(
                name: "MaxSpeed",
                table: "Specifications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
