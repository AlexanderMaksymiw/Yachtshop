using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class MadeGalleryNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brochures_Galleries_GalleriesId",
                table: "Brochures");

            migrationBuilder.AlterColumn<Guid>(
                name: "GalleriesId",
                table: "Brochures",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Brochures_Galleries_GalleriesId",
                table: "Brochures",
                column: "GalleriesId",
                principalTable: "Galleries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brochures_Galleries_GalleriesId",
                table: "Brochures");

            migrationBuilder.AlterColumn<Guid>(
                name: "GalleriesId",
                table: "Brochures",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Brochures_Galleries_GalleriesId",
                table: "Brochures",
                column: "GalleriesId",
                principalTable: "Galleries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
