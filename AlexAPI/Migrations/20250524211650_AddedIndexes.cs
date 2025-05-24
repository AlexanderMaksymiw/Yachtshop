using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Yachts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Specifications",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HullType",
                table: "Specifications",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Builder",
                table: "Specifications",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Locations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Yachts_Name",
                table: "Yachts",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Yachts_OnSale",
                table: "Yachts",
                column: "OnSale");

            migrationBuilder.CreateIndex(
                name: "IX_Yachts_Price",
                table: "Yachts",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_Builder",
                table: "Specifications",
                column: "Builder");

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_Cabins",
                table: "Specifications",
                column: "Cabins");

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_CruisingSpeed",
                table: "Specifications",
                column: "CruisingSpeed");

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_GrossTonnage",
                table: "Specifications",
                column: "GrossTonnage");

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_Guests",
                table: "Specifications",
                column: "Guests");

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_HullType",
                table: "Specifications",
                column: "HullType");

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_Length",
                table: "Specifications",
                column: "Length");

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_MaxSpeed",
                table: "Specifications",
                column: "MaxSpeed");

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_Type",
                table: "Specifications",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_YearBuilt",
                table: "Specifications",
                column: "YearBuilt");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Name",
                table: "Locations",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Yachts_Name",
                table: "Yachts");

            migrationBuilder.DropIndex(
                name: "IX_Yachts_OnSale",
                table: "Yachts");

            migrationBuilder.DropIndex(
                name: "IX_Yachts_Price",
                table: "Yachts");

            migrationBuilder.DropIndex(
                name: "IX_Specifications_Builder",
                table: "Specifications");

            migrationBuilder.DropIndex(
                name: "IX_Specifications_Cabins",
                table: "Specifications");

            migrationBuilder.DropIndex(
                name: "IX_Specifications_CruisingSpeed",
                table: "Specifications");

            migrationBuilder.DropIndex(
                name: "IX_Specifications_GrossTonnage",
                table: "Specifications");

            migrationBuilder.DropIndex(
                name: "IX_Specifications_Guests",
                table: "Specifications");

            migrationBuilder.DropIndex(
                name: "IX_Specifications_HullType",
                table: "Specifications");

            migrationBuilder.DropIndex(
                name: "IX_Specifications_Length",
                table: "Specifications");

            migrationBuilder.DropIndex(
                name: "IX_Specifications_MaxSpeed",
                table: "Specifications");

            migrationBuilder.DropIndex(
                name: "IX_Specifications_Type",
                table: "Specifications");

            migrationBuilder.DropIndex(
                name: "IX_Specifications_YearBuilt",
                table: "Specifications");

            migrationBuilder.DropIndex(
                name: "IX_Locations_Name",
                table: "Locations");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Yachts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Specifications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HullType",
                table: "Specifications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Builder",
                table: "Specifications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
