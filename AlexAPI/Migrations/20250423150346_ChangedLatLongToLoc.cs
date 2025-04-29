using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangedLatLongToLoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromLat",
                table: "ItineraryDay");

            migrationBuilder.DropColumn(
                name: "FromLong",
                table: "ItineraryDay");

            migrationBuilder.DropColumn(
                name: "ToLat",
                table: "ItineraryDay");

            migrationBuilder.DropColumn(
                name: "ToLong",
                table: "ItineraryDay");

            migrationBuilder.DropColumn(
                name: "FromLat",
                table: "DealDays");

            migrationBuilder.DropColumn(
                name: "FromLong",
                table: "DealDays");

            migrationBuilder.DropColumn(
                name: "ToLat",
                table: "DealDays");

            migrationBuilder.DropColumn(
                name: "ToLong",
                table: "DealDays");

            migrationBuilder.AddColumn<Guid>(
                name: "FromLocationId",
                table: "ItineraryDay",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ToLocationId",
                table: "ItineraryDay",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FromLocationId",
                table: "DealDays",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ToLocationId",
                table: "DealDays",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryDay_FromLocationId",
                table: "ItineraryDay",
                column: "FromLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryDay_ToLocationId",
                table: "ItineraryDay",
                column: "ToLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DealDays_FromLocationId",
                table: "DealDays",
                column: "FromLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DealDays_ToLocationId",
                table: "DealDays",
                column: "ToLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealDays_Locations_FromLocationId",
                table: "DealDays",
                column: "FromLocationId",
                principalTable: "Locations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DealDays_Locations_ToLocationId",
                table: "DealDays",
                column: "ToLocationId",
                principalTable: "Locations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryDay_Locations_FromLocationId",
                table: "ItineraryDay",
                column: "FromLocationId",
                principalTable: "Locations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryDay_Locations_ToLocationId",
                table: "ItineraryDay",
                column: "ToLocationId",
                principalTable: "Locations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealDays_Locations_FromLocationId",
                table: "DealDays");

            migrationBuilder.DropForeignKey(
                name: "FK_DealDays_Locations_ToLocationId",
                table: "DealDays");

            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryDay_Locations_FromLocationId",
                table: "ItineraryDay");

            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryDay_Locations_ToLocationId",
                table: "ItineraryDay");

            migrationBuilder.DropIndex(
                name: "IX_ItineraryDay_FromLocationId",
                table: "ItineraryDay");

            migrationBuilder.DropIndex(
                name: "IX_ItineraryDay_ToLocationId",
                table: "ItineraryDay");

            migrationBuilder.DropIndex(
                name: "IX_DealDays_FromLocationId",
                table: "DealDays");

            migrationBuilder.DropIndex(
                name: "IX_DealDays_ToLocationId",
                table: "DealDays");

            migrationBuilder.DropColumn(
                name: "FromLocationId",
                table: "ItineraryDay");

            migrationBuilder.DropColumn(
                name: "ToLocationId",
                table: "ItineraryDay");

            migrationBuilder.DropColumn(
                name: "FromLocationId",
                table: "DealDays");

            migrationBuilder.DropColumn(
                name: "ToLocationId",
                table: "DealDays");

            migrationBuilder.AddColumn<decimal>(
                name: "FromLat",
                table: "ItineraryDay",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FromLong",
                table: "ItineraryDay",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ToLat",
                table: "ItineraryDay",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ToLong",
                table: "ItineraryDay",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FromLat",
                table: "DealDays",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FromLong",
                table: "DealDays",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ToLat",
                table: "DealDays",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ToLong",
                table: "DealDays",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
