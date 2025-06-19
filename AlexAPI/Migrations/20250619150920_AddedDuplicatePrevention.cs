using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedDuplicatePrevention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YachtDuplicates");

            migrationBuilder.CreateTable(
                name: "YachtDuplicateLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    YachtName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfidenceScore = table.Column<double>(type: "float", nullable: false),
                    MatchedFields = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateDetected = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OriginalYachtId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DuplicateYacht = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YachtDuplicateLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YachtDuplicateLogs_Yachts_OriginalYachtId",
                        column: x => x.OriginalYachtId,
                        principalTable: "Yachts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_YachtDuplicateLogs_OriginalYachtId",
                table: "YachtDuplicateLogs",
                column: "OriginalYachtId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YachtDuplicateLogs");

            migrationBuilder.CreateTable(
                name: "DuplicateYachtLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConflictFields = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateFlagged = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuplicateYachtLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YachtDuplicates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfidenceScore = table.Column<double>(type: "float", nullable: false),
                    DateDetected = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IncomingYachtData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchedFields = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalYachtId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    YachtName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YachtDuplicates", x => x.Id);
                });
        }
    }
}
