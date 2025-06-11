using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddWebpDataToImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "WebpData",
                table: "Images",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WebpData",
                table: "Images");
        }
    }
}
