using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class MadeAwardsAList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Yachts_Awards_AwardsId",
                table: "Yachts");

            migrationBuilder.DropTable(
                name: "AmenitiesEquipment");

            migrationBuilder.DropTable(
                name: "AmenitiesToy");

            migrationBuilder.DropIndex(
                name: "IX_Yachts_AwardsId",
                table: "Yachts");

            migrationBuilder.DropColumn(
                name: "AwardsId",
                table: "Yachts");

            migrationBuilder.AddColumn<Guid>(
                name: "YachtId",
                table: "Awards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AmenityEquipment",
                columns: table => new
                {
                    AmenitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmenityEquipment", x => new { x.AmenitiesId, x.EquipmentId });
                    table.ForeignKey(
                        name: "FK_AmenityEquipment_Amenities_AmenitiesId",
                        column: x => x.AmenitiesId,
                        principalTable: "Amenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AmenityEquipment_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AmenityToy",
                columns: table => new
                {
                    AmenitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToysId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmenityToy", x => new { x.AmenitiesId, x.ToysId });
                    table.ForeignKey(
                        name: "FK_AmenityToy_Amenities_AmenitiesId",
                        column: x => x.AmenitiesId,
                        principalTable: "Amenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AmenityToy_Toys_ToysId",
                        column: x => x.ToysId,
                        principalTable: "Toys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Awards_YachtId",
                table: "Awards",
                column: "YachtId");

            migrationBuilder.CreateIndex(
                name: "IX_AmenityEquipment_EquipmentId",
                table: "AmenityEquipment",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AmenityToy_ToysId",
                table: "AmenityToy",
                column: "ToysId");

            migrationBuilder.AddForeignKey(
                name: "FK_Awards_Yachts_YachtId",
                table: "Awards",
                column: "YachtId",
                principalTable: "Yachts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Awards_Yachts_YachtId",
                table: "Awards");

            migrationBuilder.DropTable(
                name: "AmenityEquipment");

            migrationBuilder.DropTable(
                name: "AmenityToy");

            migrationBuilder.DropIndex(
                name: "IX_Awards_YachtId",
                table: "Awards");

            migrationBuilder.DropColumn(
                name: "YachtId",
                table: "Awards");

            migrationBuilder.AddColumn<Guid>(
                name: "AwardsId",
                table: "Yachts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AmenitiesEquipment",
                columns: table => new
                {
                    AmenitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmenitiesEquipment", x => new { x.AmenitiesId, x.EquipmentId });
                    table.ForeignKey(
                        name: "FK_AmenitiesEquipment_Amenities_AmenitiesId",
                        column: x => x.AmenitiesId,
                        principalTable: "Amenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AmenitiesEquipment_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AmenitiesToy",
                columns: table => new
                {
                    AmenitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToysId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmenitiesToy", x => new { x.AmenitiesId, x.ToysId });
                    table.ForeignKey(
                        name: "FK_AmenitiesToy_Amenities_AmenitiesId",
                        column: x => x.AmenitiesId,
                        principalTable: "Amenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AmenitiesToy_Toys_ToysId",
                        column: x => x.ToysId,
                        principalTable: "Toys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Yachts_AwardsId",
                table: "Yachts",
                column: "AwardsId");

            migrationBuilder.CreateIndex(
                name: "IX_AmenitiesEquipment_EquipmentId",
                table: "AmenitiesEquipment",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AmenitiesToy_ToysId",
                table: "AmenitiesToy",
                column: "ToysId");

            migrationBuilder.AddForeignKey(
                name: "FK_Yachts_Awards_AwardsId",
                table: "Yachts",
                column: "AwardsId",
                principalTable: "Awards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
