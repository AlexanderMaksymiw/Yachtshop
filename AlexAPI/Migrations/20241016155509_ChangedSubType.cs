using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangedSubType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubType",
                table: "Specifications");

            migrationBuilder.CreateTable(
                name: "SubTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecificationSubType",
                columns: table => new
                {
                    SpecificationsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificationSubType", x => new { x.SpecificationsId, x.SubTypeId });
                    table.ForeignKey(
                        name: "FK_SpecificationSubType_Specifications_SpecificationsId",
                        column: x => x.SpecificationsId,
                        principalTable: "Specifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecificationSubType_SubTypes_SubTypeId",
                        column: x => x.SubTypeId,
                        principalTable: "SubTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecificationSubType_SubTypeId",
                table: "SpecificationSubType",
                column: "SubTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecificationSubType");

            migrationBuilder.DropTable(
                name: "SubTypes");

            migrationBuilder.AddColumn<string>(
                name: "SubType",
                table: "Specifications",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
