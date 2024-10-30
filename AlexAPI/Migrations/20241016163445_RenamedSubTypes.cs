using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenamedSubTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecificationSubType_SubTypes_SubTypeId",
                table: "SpecificationSubType");

            migrationBuilder.RenameColumn(
                name: "SubTypeId",
                table: "SpecificationSubType",
                newName: "SubTypesId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecificationSubType_SubTypeId",
                table: "SpecificationSubType",
                newName: "IX_SpecificationSubType_SubTypesId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificationSubType_SubTypes_SubTypesId",
                table: "SpecificationSubType",
                column: "SubTypesId",
                principalTable: "SubTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecificationSubType_SubTypes_SubTypesId",
                table: "SpecificationSubType");

            migrationBuilder.RenameColumn(
                name: "SubTypesId",
                table: "SpecificationSubType",
                newName: "SubTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecificationSubType_SubTypesId",
                table: "SpecificationSubType",
                newName: "IX_SpecificationSubType_SubTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificationSubType_SubTypes_SubTypeId",
                table: "SpecificationSubType",
                column: "SubTypeId",
                principalTable: "SubTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
