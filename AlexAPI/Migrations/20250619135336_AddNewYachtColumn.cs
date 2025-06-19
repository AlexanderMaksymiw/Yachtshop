using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNewYachtColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequiresUserAccess",
                table: "Yachts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiresUserAccess",
                table: "Yachts");
        }
    }
}
